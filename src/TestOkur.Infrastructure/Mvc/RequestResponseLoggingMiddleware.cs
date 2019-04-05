namespace TestOkur.Infrastructure.Mvc
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Threading.Tasks;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Http.Internal;
	using Microsoft.IO;

	public class RequestResponseLoggingMiddleware
	{
		private const int ReadChunkBufferLength = 4096;

		private static readonly IEnumerable<string> NoLogExtensions =
			new[]
			{
			"image/png",
			"image/jpeg",
			"application/octet-stream"
			};

		private readonly RequestDelegate _next;
		private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;
		private readonly IRequestResponseLogger _requestResponseLogger;

		public RequestResponseLoggingMiddleware(RequestDelegate next, IRequestResponseLogger requestResponseLogger)
		{
			_next = next;
			_requestResponseLogger = requestResponseLogger;
			_recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
		}

		public async Task Invoke(HttpContext context)
		{
			var log = new RequestResponseLog
			{
				RequestDateTimeUtc = new DateTimeOffset(DateTime.UtcNow),
				Request = await FormatRequest(context)
			};

			var originalBody = context.Response.Body;

			using (var newResponseBody = _recyclableMemoryStreamManager.GetStream())
			{
				context.Response.Body = newResponseBody;

				await _next(context);

				newResponseBody.Seek(0, SeekOrigin.Begin);
				await newResponseBody.CopyToAsync(originalBody);
				newResponseBody.Seek(0, SeekOrigin.Begin);
				log.Response = FormatResponse(context, newResponseBody);
				log.ResponseDateTimeUtc = new DateTimeOffset(DateTime.UtcNow);

				await _requestResponseLogger.PersistAsync(log);
			}
		}

		private static string ReadStreamInChunks(Stream stream)
		{
			stream.Seek(0, SeekOrigin.Begin);
			string result;
			using (var textWriter = new StringWriter())
			using (var reader = new StreamReader(stream))
			{
				var readChunk = new char[ReadChunkBufferLength];
				int readChunkLength;

				do
				{
					readChunkLength = reader.ReadBlock(readChunk, 0, ReadChunkBufferLength);
					textWriter.Write(readChunk, 0, readChunkLength);
				}
				while (readChunkLength > 0);

				result = textWriter.ToString();
			}

			return result;
		}

		private string FormatResponse(HttpContext context, MemoryStream newResponseBody)
		{
			var request = context.Request;
			var response = context.Response;

			var body = response.ContentType;

			if (!NoLogExtensions.Contains(response.ContentType))
			{
				body = ReadStreamInChunks(newResponseBody);
			}

			return body;
		}

		private async Task<string> FormatRequest(HttpContext context)
		{
			var request = context.Request;
			var body = request.ContentType;

			if (!NoLogExtensions.Contains(request.ContentType))
			{
				body = await GetRequestBodyAsync(request);
			}

			return $"Http Request Information: {Environment.NewLine}" +
						$"Schema:{request.Scheme} {Environment.NewLine}" +
						$"Host: {request.Host} {Environment.NewLine}" +
						$"Path: {request.Path} {Environment.NewLine}" +
						$"Method: {request.Method} {Environment.NewLine}" +
						$"ContentType: {request.ContentType} {Environment.NewLine}" +
						$"QueryString: {request.QueryString} {Environment.NewLine}" +
						$"Request Body: {body}";
		}

		private async Task<string> GetRequestBodyAsync(HttpRequest request)
		{
			request.EnableBuffering();
			request.EnableRewind();
			using (var requestStream = _recyclableMemoryStreamManager.GetStream())
			{
				await request.Body.CopyToAsync(requestStream);
				request.Body.Seek(0, SeekOrigin.Begin);
				return ReadStreamInChunks(requestStream);
			}
		}
	}
}

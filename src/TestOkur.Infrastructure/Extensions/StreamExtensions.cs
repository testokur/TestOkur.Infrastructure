namespace TestOkur.Infrastructure.Extensions
{
	using System.IO;

	public static class StreamExtensions
	{
		public static byte[] ToByteArray(this Stream input)
		{
			var buffer = new byte[16 * 1024];
			using (var ms = new MemoryStream())
			{
				for (int read; (read = input.Read(buffer, 0, buffer.Length)) > 0;)
				{
					ms.Write(buffer, 0, read);
				}

				return ms.ToArray();
			}
		}
	}
}

namespace TestOkur.Serialization
{
    using System.IO;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using SpanJson;

    public static class JsonUtils
    {
        public static async ValueTask<T> DeserializerFromHttpContentAsync<T>(
            HttpContent content,
            CancellationToken cancellationToken = default)
        {
            await using var stream = await content.ReadAsStreamAsync();

            return await JsonSerializer.Generic.Utf8.DeserializeAsync<T, ApiResolver<byte>>(
                stream,
                cancellationToken);
        }

        public static async Task<T> DeserializeFromFileAsync<T>(string path, CancellationToken cancellationToken = default)
        {
            await using var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            var result = await JsonSerializer.Generic.Utf8.DeserializeAsync<T, ApiResolver<byte>>(
                stream,
                cancellationToken);

            return result;
        }

        public static string Serialize<T>(T obj)
        {
            var bytes = JsonSerializer.Generic.Utf8.Serialize<T>(obj);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
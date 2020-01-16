namespace TestOkur.Serialization
{
    using System.Net.Http;
    using System.Net.Mime;
    using System.Text;
    using SpanJson;

    public static class ObjectExtensions
    {
        public static StringContent ToJsonContent<TModel>(this TModel obj)
        {
            var jsonBytes = JsonSerializer.Generic.Utf8.Serialize<TModel, ApiResolver<byte>>(obj);
            var json = Encoding.UTF8.GetString(jsonBytes);
            return new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
        }
    }
}
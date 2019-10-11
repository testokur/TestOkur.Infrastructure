namespace TestOkur.Infrastructure.Mvc.Extensions
{
    using Newtonsoft.Json;
    using System.Net.Http;
    using System.Text;

    public static class ObjectExtensions
    {
        public static StringContent ToJsonContent<TModel>(this TModel obj)
        {
            return new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
        }
    }
}

namespace TestOkur.Infrastructure.Mvc.Helpers
{
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;

    public static class FileEx
    {
        public static async Task<string> ReadAllTextAsync(string path)
        {
            using (var reader = File.OpenText(path))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}

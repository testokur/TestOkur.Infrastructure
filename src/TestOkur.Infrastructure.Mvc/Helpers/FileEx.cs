namespace TestOkur.Infrastructure.Mvc.Helpers
{
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;

    public static class FileEx
    {
        public static async Task<string> ReadAllTextAsync(string path)
        {
            var lines = await File.ReadAllLinesAsync(path, Encoding.UTF8);

            return string.Concat(lines);
        }
    }
}

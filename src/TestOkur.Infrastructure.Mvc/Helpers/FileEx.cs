namespace TestOkur.Infrastructure.Mvc.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;

    public static class FileEx
    {
        public static async Task<string> ReadAllTextAsync(string path)
        {
            var lines = await File.ReadAllLinesAsync(path, Encoding.UTF8);

            return Concat(lines);
        }

        private static string Concat(string[] lines)
        {
            var s = new string('\0', lines.Sum(l => l.Length + Environment.NewLine.Length));
            var target = MemoryMarshal.AsBytes(MemoryMarshal.AsMemory(s.AsMemory()).Span);
            var newLine = MemoryMarshal.AsBytes(MemoryMarshal.AsMemory(Environment.NewLine.AsMemory()).Span);

            for (int i = 0, dest = 0; i < lines.Length; i++)
            {
                var lineSource = MemoryMarshal.AsBytes(MemoryMarshal.AsMemory(lines[i].AsMemory()).Span);
                Unsafe.CopyBlock(ref target[dest], ref lineSource[0], (uint)lineSource.Length);
                dest += lineSource.Length;
                Unsafe.CopyBlock(ref target[dest], ref newLine[0], (uint)newLine.Length);
                dest += newLine.Length;
            }

            return s;
        }
    }
}

using BenchmarkDotNet.Attributes;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace TestOkur.Infrastructure.BenchmarkDotNet.Tests
{
    public class ConcatWithCopyBlockVsSimpleConcat
    {
        private static readonly string[] Lines = Enumerable.Range(0, 100)
            .Select(x => new Guid().ToString("N"))
            .ToArray();

        [Benchmark]
        public void ConcatWithCopyBlockBench()
        {
            ConcatWithCopyBlock(Lines);
        }

        [Benchmark]
        public void SimpleConcatBench()
        {
            SimpleConcat(Lines);
        }


        private string SimpleConcat(string[] lines)
        {
            return string.Concat(lines);
        }

        private string ConcatWithCopyBlock(string[] lines)
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
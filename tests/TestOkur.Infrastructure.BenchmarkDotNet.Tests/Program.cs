using BenchmarkDotNet.Running;

namespace TestOkur.Infrastructure.BenchmarkDotNet.Tests
{
    class Program
    {
        static void Main(string[] args) => BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);

    }
}

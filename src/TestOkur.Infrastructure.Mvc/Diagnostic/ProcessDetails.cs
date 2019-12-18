namespace TestOkur.Infrastructure.Mvc.Diagnostic
{
    using System;

    public sealed class ProcessDetails
    {
        public int PID { get; internal set; }

        public string Name { get; internal set; }

        public DateTimeOffset Started { get; internal set; }

        public TimeSpan LoadedIn { get; internal set; }

        public string IsOptimized { get; internal set; }

        public bool Is64Bit { get; internal set; }

        public bool IsServerGC { get; internal set; }

        public bool IsLargeAddressAware { get; internal set; }

        public uint ThreadCount { get; internal set; }

        public uint ThreadPoolMinWorkerCount { get; internal set; }

        public uint ThreadPoolMaxWorkerCount { get; internal set; }

        public uint ThreadPoolMinCompletionPortCount { get; internal set; }

        public uint ThreadPoolMaxCompletionPortCount { get; internal set; }

        public string ModuleName { get; internal set; }

        public string ModuleFileName { get; internal set; }

        public string ProductName { get; internal set; }

        public string OriginalFileName { get; internal set; }

        public string FileName { get; internal set; }

        public string FileVersion { get; internal set; }

        public string ProductVersion { get; internal set; }

        public string Language { get; internal set; }

        public string Copyright { get; internal set; }

        public double WorkingSetInMegaBytes { get; internal set; }

        public bool IsInteractive { get; internal set; }

        public string[] CommandLine { get; internal set; }
    }
}
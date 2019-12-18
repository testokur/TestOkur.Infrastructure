namespace TestOkur.Infrastructure.Mvc.Diagnostic
{
    using System;

    public sealed class SystemDetails
    {
        public string OSName { get; internal set; }

        public string OSType { get; internal set; }

        public bool Is64BitOS { get; internal set; }

        public string DotNetFrameworkVersion { get; internal set; }

        public string MachineName { get; internal set; }

        public string FQDN { get; internal set; }

        public string User { get; internal set; }

        public string CPU { get; internal set; }

        public uint CPUCoreCount { get; internal set; }

        public long InstalledRAMInGigaBytes { get; internal set; }

        public string SystemDirectory { get; internal set; }

        public string CurrentDirectory { get; internal set; }

        public string RuntimeDirectory { get; internal set; }

        public TimeSpan Uptime { get; internal set; }
    }
}
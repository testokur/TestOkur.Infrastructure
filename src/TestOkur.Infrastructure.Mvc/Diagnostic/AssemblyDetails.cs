namespace TestOkur.Infrastructure.Mvc.Diagnostic
{
    using System;

    public sealed class AssemblyDetails
    {
        public string Name { get; internal set; }

        public bool IsGAC { get; internal set; }

        public bool Is64Bit { get; internal set; }

        public bool IsOptimized { get; internal set; }

        public string Framework { get; internal set; }

        public string Location { get; internal set; }

        public Uri CodeBase { get; internal set; }
    }
}
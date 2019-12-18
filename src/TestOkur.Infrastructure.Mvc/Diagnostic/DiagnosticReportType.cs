namespace TestOkur.Infrastructure.Mvc.Diagnostic
{
    using System;

    [Flags]
    public enum DiagnosticReportType
    {
        System = 1,

        Process = 2,

        Drives = 4,

        Assemblies = 8,

        EnvironmentVariables = 16,

        Networks = 32,

        Full = System | Process | Drives | Assemblies | EnvironmentVariables | Networks,
    }
}
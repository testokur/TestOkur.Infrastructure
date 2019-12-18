namespace TestOkur.Infrastructure.Mvc.Extensions
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Versioning;

    public static class AssemblyExtensions
    {
        public static string GetFrameworkVersion(this Assembly assembly)
        {
            var targetFrameAttribute = assembly.GetCustomAttributes(true)
                .OfType<TargetFrameworkAttribute>().FirstOrDefault();

            return targetFrameAttribute is null ? ".NET 2, 3 or 3.5" : targetFrameAttribute.FrameworkName;
        }

        public static bool IsOptimized(this Assembly assembly)
        {
            var attributes = assembly.GetCustomAttributes(typeof(DebuggableAttribute), false);

            if (attributes.Length == 0)
            {
                return true;
            }

            foreach (Attribute attr in attributes)
            {
                if (attr is DebuggableAttribute)
                {
                    var d = attr as DebuggableAttribute;

                    return !d.IsJITOptimizerDisabled;
                }
            }

            return false;
        }

        public static bool Is32Bit(this Assembly assembly)
        {
            Ensure.NotNull(assembly, nameof(assembly));
            var location = assembly.Location;
            if (location.IsNullOrEmptyOrWhiteSpace())
            {
                location = assembly.CodeBase;
            }

            var uri = new Uri(location);
            Ensure.That(uri.IsFile, "Assembly location is not a file.");

            var assemblyName = AssemblyName.GetAssemblyName(uri.LocalPath);
            return assemblyName.ProcessorArchitecture == ProcessorArchitecture.X86;
        }
    }
}
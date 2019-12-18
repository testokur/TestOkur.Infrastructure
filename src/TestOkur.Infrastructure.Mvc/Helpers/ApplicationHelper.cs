namespace TestOkur.Infrastructure.Mvc.Helpers
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.InteropServices;

    public static class ApplicationHelper
    {
        public static bool IsWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        public static bool IsLinux => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

        public static bool IsOSX => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

        public static OSPlatform OSPlatform => GetOSPlatform();

        public static bool IsProcessLargeAddressAware()
        {
            using (var p = Process.GetCurrentProcess())
            {
                return IsLargeAddressAware(p.MainModule.FileName);
            }
        }

        public static TimeSpan GetProcessStartupDuration() =>
            DateTime.Now.Subtract(Process.GetCurrentProcess().StartTime);

        internal static bool IsLargeAddressAware(string file)
        {
            Ensure.NotNullOrEmptyOrWhiteSpace(file);
            var fileInfo = new FileInfo(file);
            Ensure.Exists(fileInfo);

            const int ImageFileLargeAddressAware = 0x20;

            using (var stream = File.Open(fileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var reader = new BinaryReader(stream))
            {
                //No MZ Header
                if (reader.ReadInt16() != 0x5A4D)
                {
                    return false;
                }

                reader.BaseStream.Position = 0x3C;
                var peloc = reader.ReadInt32(); //Get the PE header location.

                reader.BaseStream.Position = peloc;

                //No PE header
                if (reader.ReadInt32() != 0x4550)
                {
                    return false;
                }

                reader.BaseStream.Position += 0x12;
                return (reader.ReadInt16() & ImageFileLargeAddressAware) == ImageFileLargeAddressAware;
            }
        }

        // ReSharper disable once InconsistentNaming
        private static OSPlatform GetOSPlatform()
        {
            if (IsWindows)
            {
                return OSPlatform.Windows;
            }

            return IsLinux ? OSPlatform.Linux : IsOSX ? OSPlatform.OSX : OSPlatform.Create("UNKNOWN");
        }
    }
}
namespace TestOkur.Infrastructure.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using TestOkur.Infrastructure.Mvc.Extensions;

    public static class Ensure
    {
        [DebuggerStepThrough]
        public static void That<TException>(bool condition, string message = "The given condition is false.")
            where TException : Exception
        {
            if (!condition)
            {
                throw (TException)Activator.CreateInstance(typeof(TException), message);
            }
        }

        [DebuggerStepThrough]
        public static void That(bool condition, string message = "The given condition is false.")
            => That<ArgumentException>(condition, message);

        [DebuggerStepThrough]
        public static T NotNull<T>(T value, string argName)
            where T : class
        {
            if (argName.IsNullOrEmptyOrWhiteSpace())
            {
                argName = "Invalid";
            }

            That<ArgumentNullException>(value != null, argName);
            return value;
        }

        [DebuggerStepThrough]
        public static string NotNullOrEmptyOrWhiteSpace(string value, string message = "String must not be null, empty or whitespace.")
        {
            That<ArgumentException>(value.IsNotNullOrEmptyOrWhiteSpace(), message);
            return value;
        }

        [DebuggerStepThrough]
        public static FileInfo Exists(FileInfo fileInfo)
        {
            NotNull(fileInfo, nameof(fileInfo));

            fileInfo.Refresh();
            That<FileNotFoundException>(fileInfo.Exists, $"Cannot find: '{fileInfo.FullName}'.");
            return fileInfo;
        }
    }
}
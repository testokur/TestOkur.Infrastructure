﻿using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("TestOkur.Infrastructure.Tests")]

namespace TestOkur.Infrastructure.Data
{
    using System.Text.RegularExpressions;

    internal static class StringExtensions
    {
        public static string ToSnakeCase(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            var startUnderscores = Regex.Match(input, @"^_+");
            return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
        }
    }
}

namespace TestOkur.Infrastructure.Mvc.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    public static class EnumerableExtensions
    {
        [DebuggerStepThrough]
        public static void ForEach<T>(this IEnumerable<T> sequence, Action<T> action)
        {
            Ensure.NotNull(action, nameof(action));
            foreach (var item in sequence)
            {
                action(item);
            }
        }
    }
}

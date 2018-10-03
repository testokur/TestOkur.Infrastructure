namespace TestOkur.Infrastructure.Threading
{
    using System;

    public class CrossProcessLockFactory
    {
        private const int DefaultTimoutInMinutes = 1;

        public static IDisposable CreateCrossProcessLock()
        {
            return new ProcessLock(TimeSpan.FromMinutes(DefaultTimoutInMinutes));
        }

        public static IDisposable CreateCrossProcessLock(TimeSpan timespan)
        {
            return new ProcessLock(timespan);
        }
    }
}

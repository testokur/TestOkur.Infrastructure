namespace TestOkur.Infrastructure.Mvc.Diagnostic
{
    public static class UnitConverter
    {
        private const double KiloBytes = 1024;
        private const double MegaBytes = KiloBytes * KiloBytes;

        public static double BytesToMegaBytes(this double bytes) => bytes / MegaBytes;

        public static double BytesToGigaBytes(this double bytes) => bytes / MegaBytes / KiloBytes;

        public static double KiloBytesToMegaBytes(this double kiloBytes) => kiloBytes / KiloBytes;

        public static double MegaBytesToGigaBytes(this double megaBytes) => megaBytes / KiloBytes;
    }
}

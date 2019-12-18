namespace TestOkur.Infrastructure.Mvc.Diagnostic
{
    public sealed class DriveDetails
    {
        public string Name { get; internal set; }

        public string Type { get; internal set; }

        public string Format { get; internal set; }

        public string Label { get; internal set; }

        public double TotalCapacityInGigaBytes { get; internal set; }

        public double FreeCapacityInGigaBytes { get; internal set; }

        public double AvailableCapacityInGigaBytes { get; internal set; }
    }
}
namespace TestOkur.Infrastructure.Mvc.Diagnostic
{
    using System.Net;
    using System.Net.NetworkInformation;

    public sealed class NetworkInterfaceDetails
    {
        public string MAC { get; internal set; }

        public NetworkInterface Interface { get; internal set; }

        public IPAddress[] Addresses { get; internal set; }
    }
}
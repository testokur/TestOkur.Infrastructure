namespace TestOkur.Infrastructure.Mvc.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.NetworkInformation;
    using System.Net.Sockets;

    public static class NetworkHelper
    {
        public static string GetFQDN()
        {
            var domainName = IPGlobalProperties.GetIPGlobalProperties().DomainName;
            var hostName = Dns.GetHostName();

            domainName = "." + domainName;
            if (!hostName.EndsWith(domainName, StringComparison.InvariantCultureIgnoreCase))
            {
                hostName += domainName;
            }

            return hostName;
        }

        internal static IEnumerable<IPAddress> GetLocalIPAddresses(NetworkInterface nic)
        {
            // Read the IP configuration for each network
            var properties = nic.GetIPProperties();

            // Each network interface may have multiple IP addresses
            foreach (var address in properties.UnicastAddresses)
            {
                // We're only interested in IPv4 addresses for now
                if (address.Address.AddressFamily != AddressFamily.InterNetwork)
                {
                    continue;
                }

                // Ignore loopback addresses (e.g., 127.0.0.1)
                if (IPAddress.IsLoopback(address.Address))
                {
                    continue;
                }

                yield return address.Address;
            }
        }
    }
}

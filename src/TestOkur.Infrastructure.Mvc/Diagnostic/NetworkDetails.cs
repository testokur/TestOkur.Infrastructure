namespace TestOkur.Infrastructure.Mvc.Diagnostic
{
    public sealed class NetworkDetails
    {
        public string DHCPScope { get; internal set; }

        public string Domain { get; internal set; }

        public string Host { get; internal set; }

        public bool IsWINSProxy { get; internal set; }

        public string NodeType { get; internal set; }

        public NetworkInterfaceDetails[] InterfaceDetails { get; internal set; }
    }
}
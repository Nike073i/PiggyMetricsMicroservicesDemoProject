namespace Core.Configuration
{
    public class ConsulConfiguration
    {
        public string Uri { get; set; }

        public DiscoveryConfiguration Discovery { get; set; }
    }
}

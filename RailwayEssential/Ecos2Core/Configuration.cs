namespace Ecos2Core
{
    public class Configuration
    {
        public string IpAddress { get; set; }
        public ushort Port { get; set; }

        public Configuration()
        {
#if DEBUG
            IpAddress = "192.168.178.61";
#else
            IpAddress = "127.0.0.1";
#endif
            Port = 15471;
        }
    }
}

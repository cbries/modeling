namespace RailwayEssentialCore
{
    public class Configuration
    {
        public string IpAddress { get; set; }
        public ushort Port { get; set; }

        public string ThemeName { get; set; }

        public Configuration()
        {
            IpAddress = "127.0.0.1";
            Port = 15471;
            ThemeName = "SpDrS60used";
        }
    }
}

namespace RailwayEssentialCore
{
    using System;

    public interface IConfiguration
    {
        string IpAddress { get; set; }
        UInt16 Port { get; set; }
        string ThemeName { get; set; }
    }
}

namespace SkyLightMqtt
{
    internal class MqttConfiguration
    {
        public bool Enabled { get; set; } = false;
        public string BrokerAddress { get; set; }

        public int DefaultRed { get; set; } = 255;
        public int DefaultGreen { get; set; } = 255;
        public int DefaultBlue { get; set; } = 255;
        public int DefaultWhite { get; set; } = 1023;
    }
}

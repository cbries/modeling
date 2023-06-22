using System;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace SkyLightMqtt
{
    internal class Mqtt
    {
        public bool HasFailed { get; private set; }

        private MqttClient _mqttClient;
        private string _mqttClientId;
        private readonly MqttConfiguration _cfgMqtt;
        private static Mqtt _instance;

        public static Mqtt Instance(MqttConfiguration cfg)
        {
            if (_instance != null) return _instance;
            _instance = new Mqtt(cfg);
            return _instance;
        }

        private Mqtt(MqttConfiguration cfg)
        {
            _cfgMqtt = cfg;
        }

        public bool Init(out string resultMessage)
        {
            resultMessage = string.Empty;

            if (_cfgMqtt == null || !_cfgMqtt.Enabled)
            {
                resultMessage = "mqtt is not configured or enabled";
                return false;
            }

            if (_mqttClient is {IsConnected: true}) return true;

            try
            {
                _mqttClient = new MqttClient(_cfgMqtt.BrokerAddress);
                _mqttClientId = Guid.NewGuid().ToString();
                _mqttClient.Connect(_mqttClientId);
                return true;
            }
            catch (Exception ex)
            {
                resultMessage = ex.Message;
            }

            return false;
        }

        public async Task Send(string topic, string state)
        {
            if (string.IsNullOrEmpty(topic)) return;
            if (state == null) return;

            await Task.Run(() =>
            {
                var r = Init(out var resultMessage);
                if (!r)
                {
                    Console.WriteLine($"Error: {resultMessage}");
                    Program.Log($"Error: {resultMessage}", Program.LogLevel.Error);
                    HasFailed = true;
                    return;
                }

                try
                {
                    _mqttClient?.Publish(
                        topic,
                        Encoding.UTF8.GetBytes(state),
                        MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE,
                        true);
                }
                catch (Exception ex)
                {
                    Program.Log($"publish error: {ex.Message}", Program.LogLevel.Error);
                    HasFailed = true;
                }
            });
        }
    }
}

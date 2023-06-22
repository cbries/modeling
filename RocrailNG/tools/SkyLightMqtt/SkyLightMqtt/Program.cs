using System;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SkyLightMqtt
{
    internal class Program
    {
        // ATTENTION
        // configuration files needs to be placed in the rocrail working directory
        // this seems to be the directory of the loaded workspace
        // the basement workspace is locally cloned into this directory:
        //    C:\git\privateProjekte\modeling\RocrailNG

        internal static string DefaultCfgFileName = "rocrailMqtt.json";
        internal const string DefaultLogFileName = "rocrailMqtt.log";

        internal enum LogLevel
        {
            Info,
            Debug,
            Error
        }

        internal static void Log(string message, LogLevel logLevel = LogLevel.Info, string logFilePath = DefaultLogFileName)
        {
            using var writer = new StreamWriter(logFilePath, true);
            var logLine = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {logLevel}: {message}";
            writer.WriteLine(logLine);
        }

        /*
         * Example Sky values:
         * Red: 255
         * Green: 255
         * Blue: 255
         *
         * White Morgens:       300
         * White Tag:           1023
         * White Nachmittag:    500
         * White Abend:         250
         * White Nacht:         100
         */

        /*
            private const string MqttBrokerAddress = "192.168.178.29";		{value}
            private const string MqttTopicR = "Haus/Railway/Sky/R";			{value}
            private const string MqttTopicG = "Haus/Railway/Sky/G";			{value}
            private const string MqttTopicB = "Haus/Railway/Sky/B";			{value}
            private const string MqttTopicW = "Haus/Railway/Sky/W"; 		{value}	// W or A (white or alpha)
            private const string MqttTopicOff = "Haus/Railway/Sky/Off";
            private const string MqttTopicOn = "Haus/Railway/Sky/On";         
         */
        internal static string TopicColorRed = "Haus/Railway/Sky/R";
        internal static string TopicColorGreen = "Haus/Railway/Sky/G";
        internal static string TopicColorBlue = "Haus/Railway/Sky/B";
        internal static string TopicColorWhite = "Haus/Railway/Sky/W";
        internal static string TopicColorOff = "Haus/Railway/Sky/Off";
        internal static string TopicColorOn = "Haus/Railway/Sky/On";

        /*
            // MQTT
            // Haus/Switches/Railway01   True|False
            // Haus/Switches/Railway02   True|False
            // Haus/Switches/Railway03   True|False
            // Haus/Switches/Railway04   True|False
            // Haus/Switches/Off
            // Haus/Switches/On
         */

        private static MqttConfiguration _mqttConfiguration;

        internal class Color
        {
            public int Red { get; set; }
            public int Green { get; set; }
            public int Blue { get; set; }
            public int White { get; set; }
        }

        internal static Color ParseRgbaString(string input)
        {
            var values = input.Replace("rgba(", string.Empty).Replace(")", string.Empty).Split(',');

            var wint = 0;

            var w = values[3].Trim();
            if (w.IndexOf(".", StringComparison.Ordinal) == -1)
            {
                wint = int.Parse(w);
            }
            else
            {
                wint = (int) (float.Parse(w) * 1023);
            }


            return new Color
            {
                Red = int.Parse(values[0].Trim()),
                Green = int.Parse(values[1].Trim()),
                Blue = int.Parse(values[2].Trim()),
                White = wint
            };
        }

        static async Task Main(string[] args)
        {
            try
            {
                if (args.Length < 2 || args.Length > 3)
                {
                    Console.WriteLine($"incorrect call");
                    Console.WriteLine(string.Empty);
                    Console.WriteLine($"Usage:    .\\rocrailMqtt sky \"rgba( [red], [green], [blue], [white] )\"");
                    Console.WriteLine($"Example:  .\\rocrailMqtt sky \"rgba( 255, 255, 255, 1.0 )\"");
                    Console.WriteLine(string.Empty);
                    Console.WriteLine($"Usage:    .\\rocrailMqtt power [topic] [True|False]");
                    Console.WriteLine($"Example:  .\\rocrailMqtt power \"Haus/Switches/Railway01\" False");
                    Console.WriteLine($"Example:  .\\rocrailMqtt power \"Haus/Switches/Railway02\" True");
                    Console.WriteLine($"Example:  .\\rocrailMqtt power \"Haus/Switches/Railway03\" True");
                    Console.WriteLine($"Example:  .\\rocrailMqtt power \"Haus/Switches/Railway04\" False");
                    return;
                }

                var cfgCnt = File.ReadAllText(DefaultCfgFileName, Encoding.UTF8);
                _mqttConfiguration = JsonConvert.DeserializeObject<MqttConfiguration>(cfgCnt);

                Log($"Parameters: {string.Join(" ", args)}", LogLevel.Info);

                switch (args[0].ToLower())
                {
                    case "sky":
                        {
                            var c = ParseRgbaString(args[1].Trim().Trim(new[] { '"' }));
                            await Mqtt.Instance(_mqttConfiguration).Send(TopicColorRed, $"{c.Red}");
                            // cancel execution if first call failed
                            if (Mqtt.Instance(_mqttConfiguration).HasFailed) return;
                            
                            await Mqtt.Instance(_mqttConfiguration).Send(TopicColorGreen, $"{c.Green}");
                            // cancel execution if call failed
                            if (Mqtt.Instance(_mqttConfiguration).HasFailed) return;
                            
                            await Mqtt.Instance(_mqttConfiguration).Send(TopicColorBlue, $"{c.Blue}");
                            // cancel execution if call failed
                            if (Mqtt.Instance(_mqttConfiguration).HasFailed) return;
                            
                            await Mqtt.Instance(_mqttConfiguration).Send(TopicColorWhite, $"{c.White}");
                            // cancel execution if call failed
                            if (Mqtt.Instance(_mqttConfiguration).HasFailed) return;

                            await Task.Delay(TimeSpan.FromSeconds(1));
                        }
                        break;

                    case "power":
                        {
                            var topic = args[1].Trim().Trim(new[] { '"' });
                            var state = args[2].Trim();
                            await Mqtt.Instance(_mqttConfiguration).Send(topic, $"{state}");

                            await Task.Delay(TimeSpan.FromSeconds(1));
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");

                Log($"{ex.Message}", LogLevel.Error);
            }

            Environment.Exit(0);
        }
    }
}

using System;
using System.ComponentModel;
using System.Text;
using System.Windows;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace SendRgbaMqtt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int MinValue = 0;
        private const int MaxValue = 255;

        private const string MqttBrokerAddress = "192.168.178.29";
        private const string MqttTopicR = "Haus/Railway/Sky/R";
        private const string MqttTopicG = "Haus/Railway/Sky/G";
        private const string MqttTopicB = "Haus/Railway/Sky/B";
        private const string MqttTopicW = "Haus/Railway/Sky/W"; // W or A (white or alpha)
        private const string MqttTopicOff = "Haus/Railway/Sky/Off";
        private const string MqttTopicOn = "Haus/Railway/Sky/On";

        public MainWindow()
        {
            InitializeComponent();

            SliderR.Minimum = MinValue;
            SliderR.Maximum = MaxValue;

            SliderG.Minimum = MinValue;
            SliderG.Maximum = MaxValue;

            SliderB.Minimum = MinValue;
            SliderB.Maximum = MaxValue;

            SliderW.Minimum = MinValue;
            SliderW.Maximum = MaxValue;
        }

        private static MqttClient _client;
        private static string _clientId;

        private void UpdateRgba()
        {
            var r = (int)SliderR.Value;
            var g = (int)SliderG.Value;
            var b = (int)SliderB.Value;
            var w = (int)SliderW.Value;

            _client.Publish(MqttTopicR, Encoding.UTF8.GetBytes($"{r}"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
            _client.Publish(MqttTopicG, Encoding.UTF8.GetBytes($"{g}"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
            _client.Publish(MqttTopicB, Encoding.UTF8.GetBytes($"{b}"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
            _client.Publish(MqttTopicW, Encoding.UTF8.GetBytes($"{w}"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
        }

        private static bool ConnectToMqtt(out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                _client = new MqttClient(MqttBrokerAddress);
                _client.MqttMsgPublishReceived += ClientOnMqttMsgPublishReceived;
                _clientId = Guid.NewGuid().ToString();
                _client.Connect(_clientId);
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            return false;
        }

        private static void ClientOnMqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            
        }

        private void SliderR_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdateRgba();
        }

        private void SliderG_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdateRgba();
        }

        private void SliderB_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdateRgba();
        }

        private void SliderW_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdateRgba();
        }

        private void CmdOff_OnClick(object sender, RoutedEventArgs e)
        {
            _client.Publish(MqttTopicOff, Encoding.UTF8.GetBytes(string.Empty), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
        }

        private void CmdOn_OnClick(object sender, RoutedEventArgs e)
        {
            _client.Publish(MqttTopicOn, Encoding.UTF8.GetBytes(string.Empty), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
        }

        private void MainWindow_OnInitialized(object sender, EventArgs e)
        {
            ConnectToMqtt(out _);

            UpdateRgba();
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}

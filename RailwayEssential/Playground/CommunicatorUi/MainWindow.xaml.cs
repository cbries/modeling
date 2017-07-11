using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Communicator;
using Ecos2Core;
using Ecos2Core.Replies;

namespace CommunicatorUi
{
    public partial class MainWindow : Window
    {
        private void Log(string m)
        {
            if (_ctx == null)
                return;

            _ctx.Send(state =>
            {
                string currentMsg = TxtLogging.Text;
                TxtLogging.Text = m.Trim() + "\r\n" + currentMsg.Trim();
            }, null);
        }

        private Connector _client = null;
        private bool _isConnected = false;

        private SynchronizationContext _ctx = null;

        public async Task ConnectToEcos2()
        {
            _client = new Connector()
            {
                IpAddr = "192.168.178.61",
                Port = 15471
            };
            _client.Started += COnStarted;
            _client.Stopped += COnStopped;
            _client.Failed += COnFailed;
            _client.MessageReceived += COnMessageReceived;
            _client.Start();
        }

        private readonly List<string> _lines = new List<string>();

        private void COnMessageReceived(object o, string msg)
        {
            Trace.WriteLine("Message: " + msg);

            var line = msg.Trim();

            if (Utils.HasAnyBlock(msg))
            {
                _lines.Clear();
                _lines.AddRange(msg.Split(new[] {'\r'}, StringSplitOptions.RemoveEmptyEntries));
            }
            else
            {
                _lines.Add(line);
            }

            IReadOnlyList<IBlock> blocks = null;

            if (Utils.HasAnyBlock(_lines))
            {
                blocks = Utils.GetBlocks(_lines);
                _lines.Clear();
            }
            
            if (blocks != null && blocks.Count > 0)
            {
                foreach (var block in blocks)
                {
                    if(block.Command != null)
                        Log("Command: " + block.Command.Name + " -> " + block.Command.NativeCommand.Trim());
                    if(block.ObjectId != null)
                        Log("ObjectId: " + block.ObjectId);
                    Log("Entries: ");
                    foreach (var e in block.ListEntries)
                    {
                        if (e == null)
                            continue;

                        Log("E: " + e.ObjectId + " -> " + string.Join(", ", e.Arguments));
                    }
                }
            }

        }

        private void COnFailed(object o)
        {
            Log("Failed");
            _isConnected = false;
        }

        private void COnStopped(object o)
        {
            Log("Stopped");
            _isConnected = false;
        }

        private void COnStarted(object sender)
        {
            Log("Started");
            _isConnected = true;
        }

        public MainWindow()
        {
            InitializeComponent();

            _ctx = SynchronizationContext.Current;
        }

        private void Send()
        {
            if (!_isConnected)
                return;
            
            _client.SendMessage(TxtCommand.Text.Trim());
        }

        private void SendMessage(string msg)
        {
            if (!_isConnected)
                return;

            _client.SendMessage(msg.Trim());
        }

        private void CmdSend_OnClick(object sender, RoutedEventArgs e)
        {
            Send();
        }

        private void TxtCommand_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Send();
                TxtCommand.Clear();
            }
        }

        private void MainWindow_OnInitialized(object sender, EventArgs e)
        {
            ConnectToEcos2();
        }

        private void CmdClear_OnClick(object sender, RoutedEventArgs e)
        {
            TxtLogging.Clear();
        }

        private void CmdReq1_OnClick(object sender, RoutedEventArgs e)
        {
            SendMessage("request(1, view)");
        }

        private void CmdReq26_OnClick(object sender, RoutedEventArgs e)
        {
            SendMessage("request(26, view)");
        }

        private void CmdReq5_OnClick(object sender, RoutedEventArgs e)
        {
            SendMessage("request(5, view)");
        }

        private void CmdReq100_OnClick(object sender, RoutedEventArgs e)
        {
            SendMessage("request(100, view)");
        }

        private void CmdReq10_OnClick(object sender, RoutedEventArgs e)
        {
            SendMessage("request(10, view)");
        }

        private void CmdReq11_OnClick(object sender, RoutedEventArgs e)
        {
            SendMessage("request(11, view, viewswitch)");
        }

        private void CmdReqAll_OnClick(object sender, RoutedEventArgs e)
        {
            SendMessage("request(1, view)");
            SendMessage("request(26, view)");
            SendMessage("request(5, view)");
            SendMessage("request(100, view)");
            SendMessage("request(10, view)");
            SendMessage("request(11, view, viewswitch)");
        }
    }
}

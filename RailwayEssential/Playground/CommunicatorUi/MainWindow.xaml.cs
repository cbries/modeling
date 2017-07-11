using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Communicator;

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
            var line = msg.Trim();

            if (Ecos2Core.Replies.ReplyBlock.HasReplyBlock(msg))
            {
                _lines.Clear();
                _lines.AddRange(msg.Split(new[] {'\r'}, StringSplitOptions.RemoveEmptyEntries));

            }
            else
            {
                _lines.Add(line);
            }

            if (Ecos2Core.Replies.ReplyBlock.HasReplyBlock(_lines))
            {
                Log("REPLY RECEIVED !!!");
                Log("++++++++++++++++++");

                var block = new Ecos2Core.Replies.ReplyBlock();
                if (block.Parse(_lines))
                {
                    Log("Command: " + block.Command.Name + " -> " + block.Command.NativeCommand.Trim());
                    Log("Entries: ");
                    foreach (var e in block.ListEntries)
                    {
                        if (e == null)
                            continue;

                        Log("E: " + e.ObjectId + " -> " + string.Join(", ", e.Arguments));
                    }
                }
                Log("++++++++++++++++++");

                _lines.Clear();
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
    }
}

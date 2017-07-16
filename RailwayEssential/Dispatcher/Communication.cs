using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Communicator;
using Ecos2Core;
using Ecos2Core.Replies;

namespace Dispatcher
{
    public class Communication : ICommunication
    {
        public event BlocksReceivedDelegator BlocksReceived;
        public event CommunicationStartedDelegator CommunicationStarted;

        private Connector _client;
        public RailwayEssentialCore.Configuration Cfg { get; set; }

        public bool IsConnected { get; private set; }

        public bool HasError { get; private set; }

        public string ErrorMessage { get; private set; }

        public ILogging Logger { get; set; }

        public Communication(RailwayEssentialCore.Configuration cfg)
        {
            Cfg = cfg;

            IsConnected = false;
            HasError = false;
            ErrorMessage = "";
        }

        public bool Start()
        {
            return BringItUp();
        }

        public bool Shutdown()
        {
            try
            {
                if(_client != null)
                    return _client.Stop();
                return true;
            }
            catch (Exception ex)
            {
                HasError = true;
                ErrorMessage = ex.Message;
                return false;
            }
        }

        public async Task SendCommand(ICommand command)
        {
            if (command == null || !IsConnected || _client == null)
                return;

            await _client.SendMessage(command.ToString());
        }

        public async Task SendCommands(IReadOnlyList<ICommand> commands)
        {
            if (commands.Count <= 0 || !IsConnected || _client == null)
                return;

            await _client.SendMessage(string.Join("\r\n", commands));
        }

        private bool BringItUp()
        {
            return ConnectToEcos2();
        }

        private bool ConnectToEcos2()
        {
            try
            {
                HasError = false;
                ErrorMessage = "";

                _client = new Connector {Cfg = Cfg};
                _client.Started += COnStarted;
                _client.Stopped += COnStopped;
                _client.Failed += COnFailed;
                _client.MessageReceived += COnMessageReceived;
                _client.Start();

                return true;
            }
            catch(Exception ex)
            {
                HasError = true;
                ErrorMessage = ex.Message;
            }

            return false;
        }

        private readonly List<string> _lines = new List<string>();

        private void COnMessageReceived(object o, string msg)
        {
            var line = msg.Trim();

            if (!string.IsNullOrEmpty(line))
                Logger?.LogNetwork(line.Trim());

            if (Utils.HasAnyBlock(msg))
            {
                _lines.Clear();
                _lines.AddRange(msg.Split(new[] { '\r' }, StringSplitOptions.RemoveEmptyEntries));
            }
            else
            {
                _lines.Add(line);
            }

            if (Utils.HasAnyBlock(_lines))
            {
                var blocks = Utils.GetBlocks(_lines);

                if (BlocksReceived != null)
                    BlocksReceived(this, blocks);

                _lines.Clear();
            }           
        }

        private void COnFailed(object o)
        {
            IsConnected = false;
            HasError = true;
            ErrorMessage = "Connection failed";
        }

        private void COnStopped(object o)
        {
            IsConnected = false;
            HasError = false;
            ErrorMessage = "";
        }

        private void COnStarted(object sender)
        {
            IsConnected = true;
            HasError = false;
            ErrorMessage = "";

            if (CommunicationStarted != null)
                CommunicationStarted(this);
        }
    }
}

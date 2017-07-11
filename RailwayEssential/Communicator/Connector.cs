using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using ThreadState = System.Threading.ThreadState;

namespace Communicator
{
    public delegate void StartedDelegator(object sender);

    public delegate void FailedDelegator(object sender);

    public delegate void StoppedDelegator(object sender);

    public delegate void MessageReceivedDelegator(object sender, string msg);
    

    public class Connector
    {
        public event StartedDelegator Started;
        public event FailedDelegator Failed;
        public event StoppedDelegator Stopped;
        public event MessageReceivedDelegator MessageReceived;

        public string IpAddr { get; set; }
        public UInt16 Port { get; set; }

        private bool _run = false;
        private Thread _thread = null;
        private PrimS.Telnet.Client _clientConnection;

        public async void SendMessage(string msg)
        {
            if (string.IsNullOrEmpty(msg))
                return;

            if (_clientConnection == null)
                return;

            if (_clientConnection.IsConnected)
            {
                Trace.WriteLine("Send message: " + msg);

                await _clientConnection.WriteLine(msg.Trim());
            }
        }

        public bool Start()
        {
            if (_thread != null && _thread.IsAlive)
                return true;

            _thread = new Thread(async () =>
            {
                Thread.CurrentThread.IsBackground = true;

                await StartHandler();
            });

            _thread.Start();

            _run = _thread.ThreadState == ThreadState.Running || _thread.ThreadState == ThreadState.Background;

            if(_run)
                return true;

            try
            {
                _thread.Abort(null);
                _thread = null;
            }
            catch
            {
                // ignore
            }

            return false;
        }

        public bool Stop()
        { 
            _run = false;

            return true;
        }

        private async Task StartHandler()
        {
            string ipaddr = IpAddr;
            int port = Port;

            using (_clientConnection = new PrimS.Telnet.Client(ipaddr, port, new System.Threading.CancellationToken()))
            {
                if (_clientConnection.IsConnected)
                {
                    if (Started != null)
                        Started(this);
                }
                else
                {
                    if (Failed != null)
                        Failed(this);
                }

                while (_run)
                {
                    var msg = await _clientConnection.TerminatedReadAsync("\r\n", TimeSpan.FromMilliseconds(2500));

                    if (!string.IsNullOrEmpty(msg))
                    {
                        if (MessageReceived != null)
                            MessageReceived(this, msg);
                    }
                }

                if (Stopped != null)
                    Stopped(this);
            }
        }
    }
}

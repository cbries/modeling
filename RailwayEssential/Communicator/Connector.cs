using System;
using System.Threading;
using System.Threading.Tasks;
using Ecos2Core;
using RailwayEssentialCore;
using ThreadState = System.Threading.ThreadState;

namespace Communicator
{
    public delegate void StartedDelegator(object sender);
    public delegate void FailedDelegator(object sender, string message);
    public delegate void StoppedDelegator(object sender);
    public delegate void MessageReceivedDelegator(object sender, string msg);
    
    public class Connector
    {
        public event StartedDelegator Started;
        public event FailedDelegator Failed;
        public event StoppedDelegator Stopped;
        public event MessageReceivedDelegator MessageReceived;

        public ILogging Logger { get; set; }

        public IConfiguration Cfg { get; set; }

        private string IpAddr { get { return Cfg.IpAddress; } }
        private UInt16 Port { get { return Cfg.Port; } }

        private bool _run = false;
        private Thread _thread = null;
        private PrimS.Telnet.Client _clientConnection;

        public async Task<bool> SendMessage(string msg)
        {
            if (string.IsNullOrEmpty(msg))
                return false;

            if (_clientConnection == null)
                return false;

            if (_clientConnection.IsConnected)
                await _clientConnection.WriteLine(msg.Trim());

            return false;
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

            try
            {

                using (_clientConnection = new PrimS.Telnet.Client(ipaddr, port, new CancellationToken()))
                {
                    if (_clientConnection.IsConnected)
                    {
                        if (Logger != null)
                            Logger.Log("<Connector> Connection established");

                        if (Started != null)
                            Started(this);
                    }
                    else
                    {
                        if (Logger != null)
                            Logger.Log("<Connector> Connection failed");

                        if (Failed != null)
                            Failed(this, "Connection failed");
                    }

                    while (_run)
                    {
                        var msg = await _clientConnection.TerminatedReadAsync("\r\n", TimeSpan.FromMilliseconds(2500));

                        if (!string.IsNullOrEmpty(msg))
                        {
                            if (Logger != null)
                                Logger.Log("<Connector> Message received, Length: " + msg.Length);

                            if (MessageReceived != null)
                                MessageReceived(this, msg);
                        }
                    }

                    if (Stopped != null)
                        Stopped(this);
                }
            }
            catch (Exception ex)
            {
                if (Failed != null)
                    Failed(this, ex.Message);
            }
        }
    }
}

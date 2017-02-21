using System;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;

namespace DesktopStation
{
    public class DsRemoteClient
    {
        private readonly DsIpcCommandObject _msg = null;

        public DsRemoteClient()
        {
            IpcClientChannel channel = new IpcClientChannel();
            ChannelServices.RegisterChannel(channel, true);
            _msg = Activator.GetObject(typeof(DsIpcCommandObject), "ipc://cameras88/cmddata") as DsIpcCommandObject;
        }

        public void SendData(string inCommand)
        {
            _msg.DataTrance(inCommand);
        }

        public void ReceiveFromServer(string inCommand)
        {
        }
    }

    public class DsIpcCommandObject : MarshalByRefObject
    {
        public class RemoteObjectEventArg : EventArgs
        {
            public string Command { get; set; }

            public RemoteObjectEventArg(string inCommand)
            {
                Command = inCommand;
            }
        }

        public delegate void CallEventHandler(RemoteObjectEventArg e);
        public event CallEventHandler OnTrance;

        public void DataTrance(string inCommand)
        {
            OnTrance?.Invoke(new RemoteObjectEventArg(inCommand));
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}

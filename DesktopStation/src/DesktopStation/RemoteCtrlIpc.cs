using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;

namespace DesktopStation
{

    public class DSRemoteClient
    {
        private DSIpcCommandObject m_msg = null;

        public DSRemoteClient()
        {
            // クライアントチャンネルの生成
            IpcClientChannel channel = new IpcClientChannel();

            // チャンネルを登録
            ChannelServices.RegisterChannel(channel, true);

            // リモートオブジェクトを取得
            m_msg = Activator.GetObject(typeof(DSIpcCommandObject), "ipc://cameras88/cmddata") as DSIpcCommandObject;


        }

        public void SendData(String inCommand)
        {
            m_msg.DataTrance(inCommand);
        }

        public void ReceiveFromServer(String inCommand)
        {
        
        }

    }

    public class DSIpcCommandObject : MarshalByRefObject
    {

        public class RemoteObjectEventArg : EventArgs            //情報を引き渡すイベント引数クラス
        {
            private String m_command = "";                    //モード

            public String Command { get { return m_command; } set { m_command = value; } }

            public RemoteObjectEventArg(String inCommand)
            {
                m_command = inCommand;
            }
        }

        public delegate void CallEventHandler(RemoteObjectEventArg e);
        public event CallEventHandler OnTrance;

        public void DataTrance(String inCommand)
        {
            if (OnTrance != null)
            {
                OnTrance(new RemoteObjectEventArg(inCommand));
            }
        }

        /// <summary>
        /// 自動的に切断されるのを回避する
        /// </summary>
        public override object InitializeLifetimeService()
        {
            return null;
        }

    }



}

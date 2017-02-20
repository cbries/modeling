using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;

namespace CameraS88
{
    public class IpcClient
    {
        IpcRemoteObject RemoteObject;

        private IpcRemoteObject m_msg = null;

        public IpcClient()
        {
            // クライアントチャンネルの生成
            IpcClientChannel channel = new IpcClientChannel();

            // チャンネルを登録
            ChannelServices.RegisterChannel(channel, true);

            // リモートオブジェクトを取得
            RemoteObject = Activator.GetObject(typeof(IpcRemoteObject), "ipc://cameras88/s88data") as IpcRemoteObject;


        }

        public void SendData(int inAddress, int inStatus)
        {
            m_msg.DataTrance(inAddress, inStatus);
        }

    }

    public class IpcRemoteObject : MarshalByRefObject
    {

        public class RemoteObjectEventArg : EventArgs            //情報を引き渡すイベント引数クラス
        {
            private int m_address = 0;                    //モード
            private int m_status = 0;                    //モード

            public int Address { get { return m_address; } set { m_address = value; } }
            public int Status { get { return m_status; } set { m_status = value; } }

            public RemoteObjectEventArg(int tmpAddress, int tmpStatus)
            {
                m_address = tmpAddress;
                m_status = tmpStatus;
            }
        }

        public delegate void CallEventHandler(RemoteObjectEventArg e);
        public event CallEventHandler OnTrance;

        public void DataTrance(int tmpAddress, int tmpStatus)
        {
            if (OnTrance != null)
            {
                OnTrance(new RemoteObjectEventArg(tmpAddress, tmpStatus));
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

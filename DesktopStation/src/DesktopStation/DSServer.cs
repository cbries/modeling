//参考：　http://printf.jp/blog/2013/03/18/sample-web-server-4/

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Net.NetworkInformation;

namespace DesktopStation
{
    public delegate void TSendWebCommand(string inCommandText);
    public delegate string TGetStatus(int inMode);

    class DSServer
    {
        private bool ContinuosCommand = true;
        private string textIPaddress = "";
        private Socket server;
        private Thread thread;
        public string ExePath;
        public int PortNo;
        public string UrlPath;
        public string HostnamePath;
        public string DomainName;
        TSendWebCommand SendCommand;
        TGetStatus GetStatus;

        public DSServer(TSendWebCommand inFuncSend, TGetStatus inFuncStatus, int inPortNo)
        {
            SendCommand = new TSendWebCommand(inFuncSend);
            GetStatus = new TGetStatus(inFuncStatus);
            PortNo = inPortNo;

            UrlPath = "";
            HostnamePath = "";
            DomainName = "";

            /*DNS suffixを取得する。*/
            NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface networkInterface in networkInterfaces)
            {
                if (networkInterface.OperationalStatus == OperationalStatus.Up &&
                    networkInterface.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
                    networkInterface.NetworkInterfaceType != NetworkInterfaceType.Tunnel)
                {
                    IPInterfaceProperties ipProperties = networkInterface.GetIPProperties();
                    DomainName = ipProperties.DnsSuffix;
                }
            }
        }

        public void Start()
        {
            string hostname = Dns.GetHostName();
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress[] ip = Dns.GetHostAddresses(hostname);

            foreach (IPAddress address in ip)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    textIPaddress = address.ToString();

                    IPEndPoint ipEndPoint = new IPEndPoint(address, PortNo);
                    server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
                    server.Bind(ipEndPoint);
                    server.Listen(5);

                    UrlPath = "http://" + textIPaddress + ":" + PortNo.ToString() + "/";
                    HostnamePath = "http://" + hostname.ToLower() + (DomainName == "" ? "" : "." + DomainName)　 + ":" + PortNo.ToString() + "/";

                    ContinuosCommand = true;

                    // 要求待ち（無限ループ）
                    thread = new Thread(Monitoring);
                    thread.Start();

                    break;
                }
            }
        }

        public void Monitoring()
        {
            while (ContinuosCommand)
            {
                Socket client = server.Accept();
                Response response = new Response(client, SendCommand, GetStatus);
                response.ExePath = ExePath;
                response.Start();
            }
        }

        public void Stop()
        {
            ContinuosCommand = false;
            try
            {
                using (WebClient wc = new WebClient())
                    wc.DownloadString("http://" + textIPaddress + ":1192/");
            }
            catch
            {
                // ignore
            }

            if (thread != null)
            {
                thread.Join();
                thread = null;
            }

            if (server != null)
            {
                server.Close();
            }
        }
    }

    class Response
    {
         enum STATUS
         {
             CHECKING,   // 調査中
             OK,         // OK
             ERROR,      // ERROR
         };

         public string ExePath;
         private Socket mClient;
         private STATUS  mStatus;
         TSendWebCommand SendCommand;
         TGetStatus GetStatus;

         public Response(Socket client, TSendWebCommand inFuncSend, TGetStatus inFuncStatus)
        {
            mClient = client;
             mStatus = STATUS.CHECKING;
             SendCommand = new TSendWebCommand(inFuncSend);
             GetStatus = new TGetStatus(inFuncStatus);

        }
        public void Start()
        {
            Thread thread = new Thread(Run);
            thread.Start();
        }

        public void Run()
        {
            int aPosCmd;
            int aPosEnd;
            string aContentType = "text/html";
            byte[] buffer = new byte[4096];
            string aCssFile = "";

            int recvLen = mClient.Receive(buffer);
            if (recvLen <= 0)
                return;
            string message = Encoding.ASCII.GetString(buffer, 0, recvLen);
             Console.WriteLine(message);
            // 要求URL確認 ＆ 応答内容生成
             int pos = message.IndexOf("GET /");
 
             if (mStatus == STATUS.CHECKING && 0 == pos)
             {
                 aPosEnd = message.IndexOf(" HTTP/", 5);

                 if (5 == aPosEnd)
                 {
                     mStatus = STATUS.OK;
                     message = getContent();
                 }
                 else
                 {
                     aPosCmd = message.IndexOf("?CMD=");

                     if (aPosCmd > 0)
                     {
                         string aCmd = message.Substring(aPosCmd + 5, aPosEnd - aPosCmd - 5);

                         //アプリ側に通知する
                         SendCommand(aCmd);
                         mStatus = STATUS.OK;
                     }
                     else
                     {
                         aPosCmd = message.IndexOf("css/");

                         if (aPosCmd > 0)
                         {
                             aCssFile = message.Substring(aPosCmd + 4, aPosEnd - aPosCmd - 4);

                             string aCssExt = System.IO.Path.GetExtension(aCssFile);

                             switch (aCssExt.ToLower())
                             {
                                 case ".css":
                                     aContentType = "text/css";

                                     if (File.Exists(ExePath + "\\webapp\\css\\" + aCssFile) == true)
                                     {
                                         message = System.IO.File.ReadAllText(ExePath + "\\webapp\\css\\" + aCssFile);
                                     }
                                     break;
                                 case ".js":
                                     aContentType = "text/javascript";
                                     if (File.Exists(ExePath + "\\webapp\\css\\" + aCssFile) == true)
                                     {
                                         message = System.IO.File.ReadAllText(ExePath + "\\webapp\\css\\" + aCssFile);
                                     }
                                     break;
                                 case ".html":
                                 case ".htm":
                                     aContentType = "text/html";
                                     if (File.Exists(ExePath + "\\webapp\\css\\" + aCssFile) == true)
                                     {
                                        message = System.IO.File.ReadAllText(ExePath + "\\webapp\\css\\" + aCssFile);
                                     }
                                     break;
                                 /*case ".manifest":
                                     aContentType = "text/cache-manifest";
                                     message = System.IO.File.ReadAllText(ExePath + "\\webapp\\css\\" + aCssFile);
                                     break;*/
                                 case ".png":
                                     aContentType = "image/png";                               
                                     break;
                                 case ".gif":
                                     aContentType = "image/gif";
                                     break;
                                 case ".jpe":
                                 case ".jpeg":
                                 case ".jpg":
                                     aContentType = "image/jpeg";
                                     break;



                             }

                             mStatus = STATUS.OK;

                         }
                         else
                         {

                             aPosCmd = message.IndexOf("/GetStatus");

                             if (aPosCmd > 0)
                             {
                                 message = GetStatus(0);
                                 mStatus = STATUS.OK;
                             }
                             else
                             {

                                 aPosCmd = message.IndexOf("/GetConfig");

                                 if (aPosCmd > 0)
                                 {
                                     message = GetStatus(1);
                                     mStatus = STATUS.OK;
                                 }
                                 else
                                 {
                                     /* エラー */
                                     mStatus = STATUS.ERROR;
                                 }
                             }
                         }
                     }
                 }
             }
  
             if (mStatus != STATUS.OK)
            {
                message =
                    "<title>404 Not Found</title>" +
                    "<h1>Not Found</h1>";
            }

            /* 応答 */

             long contentLen = 0;

             if (aContentType.IndexOf("image") >= 0)
             {
                 /*バイナリを流し込む*/

                 if (File.Exists(ExePath + "\\webapp\\css\\" + aCssFile) == true)
                 {
                     FileStream aFs = new FileStream(ExePath + "\\webapp\\css\\" + aCssFile, System.IO.FileMode.Open, System.IO.FileAccess.Read);


                     buffer = new byte[aFs.Length];
                     //ファイルの内容をすべて読み込む
                     aFs.Read(buffer, 0, buffer.Length);

                     contentLen = buffer.Length;

                     //閉じる
                     aFs.Close();
                 }
                 
             }
             else
             {
                 /* テキストを流し込む */
                 buffer = Encoding.UTF8.GetBytes(message);
                 contentLen = buffer.GetLength(0);
             }


            // HTTPヘッダー生成
            string httpHeader = string.Format(
                "HTTP/1.1 200 OK\n" +
                "Content-type: " + aContentType + "; charset=UTF-8\n" +
                "Content-length: {0}\n" +
                "\n",
                contentLen);
            byte[] httpHeaderBuffer = new byte[4096];
            httpHeaderBuffer = Encoding.UTF8.GetBytes(httpHeader);

            if (mClient.Connected == true)
            {

                // 応答内容送信
                mClient.Send(httpHeaderBuffer);
                mClient.Send(buffer);
            }
            mClient.Close();
        }
 
         // 応答内容取得
         private string getContent()
         {

             return System.IO.File.ReadAllText(ExePath + "\\webapp\\index.html");
         }
    }

}

using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace DesktopStation
{

    class MyWebClient : WebClient
    {
        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest request = base.GetWebRequest(address);
            if (request is HttpWebRequest)
            {
                (request as HttpWebRequest).KeepAlive = false;
            }
            return request;
        }
    }

    public class DSWebCtrl : IDisposable
    {
        MyWebClient WClient;


        public DSWebCtrl(System.Net.DownloadStringCompletedEventHandler inUploadedFunc)
        {
            //文字コードを指定する
            System.Text.Encoding enc = System.Text.Encoding.GetEncoding("utf-8");


            WClient = new MyWebClient();
            //文字コードを指定する
            WClient.Encoding = enc;

            //ヘッダにContent-Typeを加える
            WClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            //WClient.Headers.Add("Connnection", "Close");

            //送信完了用の関数登録
            WClient.DownloadStringCompleted += inUploadedFunc;

        }

        public void Dispose()
        {
            WClient.CancelAsync();
            WClient.Dispose();

        }


        public void SendWithPOST(String inIPAddress, String inCommand)
        {

            if ((inIPAddress == "") || (inCommand == ""))
            {
                return;
            }

            if (WClient.IsBusy)
            {
                WClient.CancelAsync();
            }


            //WebRequestの作成
            String aUrl = "http://" + inIPAddress + "/";

            //POST送信する文字列を作成
            string postData = "?CMD=" + inCommand;

            Uri aUri = new Uri(aUrl + postData);

            var servicePoint = ServicePointManager.FindServicePoint(aUri);
            servicePoint.Expect100Continue = false;

            //データを送信し、また受信する
            WClient.DownloadStringAsync(aUri);

        }

    }
}

using System;
using System.Net;
using System.Text;

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

    public class DsWebCtrl : IDisposable
    {
        private readonly MyWebClient _wClient;

        public DsWebCtrl(DownloadStringCompletedEventHandler inUploadedFunc)
        {
            Encoding enc = Encoding.GetEncoding("utf-8");
            _wClient = new MyWebClient {Encoding = enc};
            _wClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            _wClient.DownloadStringCompleted += inUploadedFunc;
        }

        public void Dispose()
        {
            _wClient.CancelAsync();
            _wClient.Dispose();

        }

        public void SendWithPost(string inIpAddress, string inCommand)
        {
            if ((inIpAddress == "") || (inCommand == ""))
                return;

            if (_wClient.IsBusy)
                _wClient.CancelAsync();

            string aUrl = "http://" + inIpAddress + "/";
            string postData = "?CMD=" + inCommand;
            Uri aUri = new Uri(aUrl + postData);
            var servicePoint = ServicePointManager.FindServicePoint(aUri);
            servicePoint.Expect100Continue = false;
            _wClient.DownloadStringAsync(aUri);
        }
    }
}

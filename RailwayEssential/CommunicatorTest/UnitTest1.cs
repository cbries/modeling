using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Communicator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommunicatorTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task ReadmeExample()
        {
            bool isConnected = false;

            Connector c = new Connector()
            {
                IpAddr = "192.168.178.61",
                Port = 15471
            };

            c.Started += sender =>
            {
                Trace.WriteLine("Started");
                isConnected = true;
            };

            c.Stopped += sender =>
            {
                Trace.WriteLine("Stopped");
                isConnected = false;
            };

            c.Failed += sender =>
            {
                Trace.WriteLine("Failed");
                isConnected = false;
            };

            c.MessageReceived += (sender, msg) =>
            {
                Trace.WriteLine("Message received: " + msg.Trim());

                c.Stop();
            };

            c.Start();

            for (int i = 0; i < 10; ++i)
            {
                if (isConnected)
                    c.SendMessage("help()");

                Thread.Sleep(1000);
            }
        }
    }
}

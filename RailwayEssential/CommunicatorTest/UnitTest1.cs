using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommunicatorTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestInitialize]
        public void Initialize()
        {
        }

        [TestMethod]
        public async Task ReadmeExample()
        {
            string ipaddr = "127.0.0.1";
            int port = 23;

            using (PrimS.Telnet.Client client = new PrimS.Telnet.Client(ipaddr, port, new System.Threading.CancellationToken()))
            {
                client.IsConnected.Should().Be(true);
                (await client.TryLoginAsync("", "", 2500)).Should().Be(true);
                await client.WriteLine("show statistic wan2");
                string s = await client.TerminatedReadAsync(">", TimeSpan.FromMilliseconds(2500));
                s.Should().Contain(">");
                s.Should().Contain("WAN2");
                Regex regEx = new Regex("(?!WAN2 total TX: )([0-9.]*)(?! GB ,RX: )([0-9.]*)(?= GB)");
                regEx.IsMatch(s).Should().Be(true);
                MatchCollection matches = regEx.Matches(s);
                decimal tx = decimal.Parse(matches[0].Value);
                decimal rx = decimal.Parse(matches[1].Value);
                (tx + rx).Should().BeLessThan(50);
            }
        }
    }
}

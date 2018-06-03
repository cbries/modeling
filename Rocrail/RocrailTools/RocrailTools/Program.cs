using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebSocketSharp;

namespace SetDayLight
{
    class Program
    {
        private static string _targetAddress;

        static void Send(JObject o)
        {
            using (var ws = new WebSocket(_targetAddress))
            {
                ws.OnMessage += (sender, e) => Console.WriteLine("MSG from Controller: " + e.Data);
                ws.Connect();
                ws.Send(o.ToString(Formatting.None));
            }

        }

        static void Main(string[] args)
        {
            if (args.Length != 5)
                return;

            _targetAddress = args[0];
            string red = args[1];
            string green = args[2];
            string blue = args[3];
            string white = args[4];

            // {"r":0,"g":0,"b":84,"w":1023}
            var o = new JObject();
            o["r"] = red;
            o["g"] = green;
            o["b"] = blue;
            o["w"] = white;

            Send(o);

            //var rnd = new Random();

            //for (int i = 0; i < 25; ++i)
            //{
            //    int cr = rnd.Next(10, 1023);
            //    int cg = rnd.Next(10, 1023);
            //    int cb = rnd.Next(10, 1023);
            //    int cw = rnd.Next(10, 1023);

            //    var oo = new JObject();
            //    oo["r"] = cr;
            //    oo["g"] = cg;
            //    oo["b"] = cb;
            //    oo["w"] = cw;

            //    Send(oo);

            //    Thread.Sleep(500);
            //}
        }
    }
}

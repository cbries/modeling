using System;
using System.Diagnostics;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebSocketSharp;

namespace SetDayLight
{
    class Program
    {
        private static string _targetAddress;
        private static int _steps = 25;
        private static int _delayBetweenSteps = 1000;

        private static AutoResetEvent _waitFor = new AutoResetEvent(false);
        private static WebSocket _ws;
        private static JObject _o;

        static void StopApp()
        {
            Environment.Exit(0);
        }

        static void Send(JObject o)
        {
            _o = o;

            if (_ws == null)
            {
                try
                {
                    _ws = new WebSocket(_targetAddress);
                    _ws.OnMessage += (sender, e) => Console.WriteLine("MSG from Controller: " + e.Data);
                    _ws.OnOpen += WsOnOnOpen;
                    _ws.OnError += WsOnOnError;
                    _ws.Connect();
                }
                catch
                {
                    StopApp();
                }
            }
            else
            {
                _ws.Send(o.ToString(Formatting.None));

                Console.WriteLine("Send: " + _o.ToString(Formatting.None));

                _waitFor.Set();
            }

            if (_ws != null && !_ws.IsAlive)
            {
                StopApp();
            }
        }

        private static void WsOnOnError(object sender, ErrorEventArgs ev)
        {
            string m = "WebSocket failed: " + ev.Message;
            Trace.WriteLine(m);
            Console.WriteLine(m);

            Environment.Exit(0);
        }

        private static void WsOnOnOpen(object sender1, EventArgs ev)
        {
            _ws.Send(_o.ToString(Formatting.None));

            Console.WriteLine("Send: " + _o.ToString(Formatting.None));

            _waitFor.Set();
        }

        private static bool singleShot = true;

        static void ShowHelp()
        {
            string m = "";
            m += "Usage #1 (set static color): SetDayLight ws://HOST:PORT red green blue white \r\n";
            m += "Usage #2 (set fading): SetDayLight ws://HOST:PORT redFrom greenFrom blueFrom whiteFrom \r\n";
            m += "Usage #3 (set fading): SetDayLight ws://HOST:PORT redFrom greenFrom blueFrom whiteFrom fadeSteps pauseFadeSteps[msec] \r\n";

            Trace.WriteLine(m.Trim());
            Console.WriteLine(m.Trim());
        }

        static void Main(string[] args)
        {
            if (args.Length < 5)
            {
                ShowHelp();

                return;
            }

            if (args.Length > 5)
                singleShot = false;

            if (args.Length >= 10)
                _steps = Convert.ToInt16(args[9]);
            if (args.Length >= 11)
                _delayBetweenSteps = Convert.ToInt16(args[10]);

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

            if (singleShot)
            {
                Send(o);

                _waitFor.WaitOne();
            }
            else
            {
                int ir0 = Convert.ToInt16(red);
                int ig0 = Convert.ToInt16(green);
                int ib0 = Convert.ToInt16(blue);
                int iw0 = Convert.ToInt16(white);

                red = args[5];
                green = args[6];
                blue = args[7];
                white = args[8];

                int ir1 = Convert.ToInt16(red);
                int ig1 = Convert.ToInt16(green);
                int ib1 = Convert.ToInt16(blue);
                int iw1 = Convert.ToInt16(white);

                var delta_red = ir1 - ir0;
                var delta_green = ig1 - ig0;
                var delta_blue = ib1 - ib0;
                var delta_white = iw1 - iw0;

                int n = _steps;

                var step_red = delta_red / n;
                var step_green = delta_green / n;
                var step_blue = delta_blue / n;
                var step_white = delta_white / n;

                for (int i = 0; i < (int)n; ++i)
                {
                    ir0 += step_red;
                    ig0 += step_green;
                    ib0 += step_blue;
                    iw0 += step_white;

                    var oo = new JObject();
                    oo["r"] = ir0;
                    oo["g"] = ig0;
                    oo["b"] = ib0;
                    oo["w"] = iw0;

                    Send(oo);

                    Thread.Sleep(500);
                }

                var oo0 = new JObject();
                oo0["r"] = ir1;
                oo0["g"] = ig1;
                oo0["b"] = ib1;
                oo0["w"] = iw1;

                Send(oo0);

                _waitFor.WaitOne();
            }

            Thread.Sleep(_delayBetweenSteps);
        }
    }
}

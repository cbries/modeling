/*
 * MIT License
 *
 * Copyright (c) 2018 Dr. Christian Benjamin Ries
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using System;
using System.Diagnostics;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SetDayLightDotNet
{
    class Program
    {
        private static string WsApp = @"/usr/local/bin/wsta";

        private static string _targetAddress;
        private static int _steps = 25;
        private static int _delayBetweenSteps = 1000;

        private static AutoResetEvent _waitFor = new AutoResetEvent(false);
        private static JObject _o;

        static void StopApp()
        {
            Environment.Exit(0);
        }

        static void Send(JObject o)
        {
            _o = o;

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = WsApp;
            startInfo.Arguments = string.Format("{0} \"{1}\"",
                _targetAddress, _o.ToString(Formatting.None).Replace("\"", "\\\""));
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = true;
            Process.Start(startInfo);
        }

        static void ShowHelp()
        {
            string m = "";
            m += "Usage #1 (set static color): SetDayLight ws://HOST:PORT red green blue white \r\n";
            m += "Usage #2 (set fading): SetDayLight ws://HOST:PORT redFrom greenFrom blueFrom whiteFrom \r\n";
            m += "Usage #3 (set fading): SetDayLight ws://HOST:PORT redFrom greenFrom blueFrom whiteFrom fadeSteps pauseFadeSteps[msec] \r\n";

            Trace.WriteLine(m.Trim());
            Console.WriteLine(m.Trim());
        }

        private static bool singleShot = true;

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

                    Thread.Sleep(_delayBetweenSteps);
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

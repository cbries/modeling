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
using System.Collections.Generic;
using NDesk.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SetIoTSteckdose
{
	public class Executor
	{
		private Ws _ws = null;

		private string _uri;
		private string _in;
		private string _onoff;

		private void ParseArguments(string[] args)
		{
			var opts = new OptionSet();
			opts = new OptionSet {
				{ "h|help", "", v => ShowHelp(opts) },
				{ "host=", "URI of WebSocket host", v =>  { _uri = v; } },
				{ "in=", "Index of the Steckdose to enable/disable.", v => _in = v },
				{ "state=", "'on' or 'off'", v => _onoff = v }
			};
			if (args.Length == 0) ShowHelp(opts);
			opts.Parse(args);
			if (string.IsNullOrEmpty(_uri)) ShowHelp(opts);
		}

		private void ShowHelp(OptionSet set)
		{
			Console.WriteLine("Usage: .\\SetIoTSteckdoese --host=ws://ip:port --in=INDEX --state=on|off");
			set.WriteOptionDescriptions(Console.Out);
			Environment.Exit(0);
		}

		public Executor(string[] args) : base()
		{
			ParseArguments(args);
		}

		public bool Run()
		{
			if(_ws == null)
				_ws =  new Ws();

			var res = _ws.ConnectTo(_uri);
			if (!res)
			{
				Console.WriteLine($"Connection failed to {_uri} -> {_ws.LastError}");
				return false;
			}

			System.Threading.Thread.Sleep(500);

			// {"in1":0,"in2":1,"in3":0,"in4":0}

			var cmd = new JObject
			{
				["in1"] = 0,
				["in2"] = 0,
				["in3"] = 0,
				["in4"] = 0
			};

			var allowedFields = new List<string> {"in1", "in2", "in3", "in4"};
			if (!allowedFields.Contains(_in))
			{
				Console.WriteLine("Not allowed: {0} -- select one of these: {1}", 
					_in, string.Join(", ", allowedFields));
				return false;
			}

			cmd[_in] = _onoff.Equals("on", StringComparison.OrdinalIgnoreCase) ? 1 : 0;

			Console.WriteLine("Send: {0}", cmd.ToString(Formatting.None));

			var resSend = _ws.SendJson(cmd);
			if(!resSend) Console.WriteLine("Send failed: {0}", _ws.LastError);

			return true;
		}
	}
}

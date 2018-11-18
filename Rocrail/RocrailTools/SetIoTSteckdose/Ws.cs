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
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebSocket4Net;

namespace SetIoTSteckdose
{
	public delegate void WsConnectStop(object sender);
	public delegate void WsConnectError(object sender);
	public delegate void WsConnectData(object sender, string msg);

	public class Ws
	{
		public WsConnectStop WsStop;
		public WsConnectError WsError;
		public WsConnectData WsData;

		private string _host;

		private WebSocket _ws = null;
		private ManualResetEvent _waitFor = null;
		public string LastError { get; private set; }

		public bool IsConnected()
		{
			if (_ws == null) return false;
			if (_ws.State == WebSocketState.Open) return true;
			if (_ws.State == WebSocketState.Connecting) return true;
			return false;
		}

		private void CleanUp()
		{
			if (_ws == null) return;
			try
			{
				_ws.Dispose();
			}
			catch
			{
				// ignore
			}
			_ws = null;
		}

		public bool ConnectTo(string host)
		{
			_host = host;

			if (string.IsNullOrEmpty(host)) return false;
			if (_ws != null && IsConnected()) return true;

			CleanUp();

			try
			{
				_waitFor = new ManualResetEvent(false);

				_ws = new WebSocket(host);
				_ws.Error += WsOnError;
				_ws.Closed += WsOnClosed;
				_ws.DataReceived += WsOnDataReceived;
				_ws.MessageReceived += WsOnMessageReceived;
				_ws.Opened += WsOnOpened;

				_ws.Open();

				var res = _waitFor.WaitOne(5 * 1000);

				if (!res)
					LastError = "Connection timeout";

				return res;
			}
			catch (Exception ex)
			{
				Console.WriteLine("<Error> {0}", ex.Message);
				return false;
			}
		}

		private void WsOnDataReceived(object sender, DataReceivedEventArgs e)
		{
			Console.WriteLine("Data received: {0}", e.Data);
		}

		public bool SendJson(JToken tkn)
		{
			if (_ws == null) return false;
			if (!IsConnected()) return false;

			var m = tkn.ToString(Formatting.None);
			_ws.Send(m);
			Console.WriteLine("Message sent: {0}", m);

			return true;
		}

		private void WsOnOpened(object sender, EventArgs e)
		{
			Console.WriteLine("Connected to {0}", _host);
			_waitFor?.Set();
		}

		private void WsOnMessageReceived(object sender, MessageReceivedEventArgs e)
		{
			var msg = e.Message;

			if (string.IsNullOrEmpty(msg)) return;

			try
			{
				Console.WriteLine("Message received: {0}", msg);
				WsData?.Invoke(this, msg);
			}
			catch (Exception ex)
			{
				LastError = ex.Message;
				WsError?.Invoke(this);
			}
		}

		private void WsOnClosed(object sender, EventArgs e)
		{
			LastError = string.Empty;
			Console.WriteLine("Connection closed");
		}

		private void WsOnError(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
		{
			LastError = e.Exception.Message;
			Console.WriteLine("<Error> {0}", LastError);
		}
	}
}

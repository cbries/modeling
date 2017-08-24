using System;
using Newtonsoft.Json.Linq;

namespace RailwayEssentialCore
{
    public class JsonObjectEventArgs : EventArgs
    {
        private readonly JObject _data;

        public JsonObjectEventArgs(JObject obj)
        {
            _data = obj;
        }

        public JObject GetData()
        {
            return _data;
        }
    }
}

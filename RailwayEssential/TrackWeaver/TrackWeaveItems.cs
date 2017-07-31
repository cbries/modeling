using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TrackWeaver
{
    public class TrackWeaveItems
    {
        private readonly List<TrackWeaveItem> _items = new List<TrackWeaveItem>();

        private string _filePath;

        public List<TrackWeaveItem> Items => _items;

        public TrackWeaveItems()
        {
            
        }

        public bool Load(string filePath)
        {
            _filePath = filePath;

            if (!File.Exists(filePath))
                return false;

            string cnt = File.ReadAllText(filePath, Encoding.UTF8);
            if (string.IsNullOrEmpty(cnt))
                return true;

            try
            {
                JArray ar = JArray.Parse(cnt);

                if (ar == null)
                    return true;

                foreach (var o in ar)
                {
                    JObject oo = o as JObject;
                    if (oo == null)
                        continue;

                    TrackWeaveItem item = new TrackWeaveItem();
                    if(item.Parse(oo))
                        _items.Add(item);
                }

                return true;
            }
            catch(Exception ex)
            {
                Trace.WriteLine("<Exception> " + ex.Message);
                return false;
            }
        }

        public bool Save()
        {
            if (string.IsNullOrEmpty(_filePath))
                return false;

            try
            {
                JArray ar = new JArray();
                foreach (var e in _items)
                {
                    if (e == null)
                        continue;

                    ar.Add(e.ToJson());
                }
                File.WriteAllText(_filePath, ar.ToString(Formatting.Indented), Encoding.UTF8);
                return true;
            }
            catch(Exception ex)
            {
                Trace.WriteLine("<Error> " + ex.Message);
                return false;
            }
        }
    }
}

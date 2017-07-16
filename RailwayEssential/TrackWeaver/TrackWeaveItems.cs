using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;

namespace TrackWeaver
{
    public class TrackWeaveItems
    {
        private readonly List<TrackWeaveItem> _items = new List<TrackWeaveItem>();

        public List<TrackWeaveItem> Items => _items;

        public TrackWeaveItems()
        {
            
        }

        public bool Load(string filePath)
        {
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
            catch
            {
                return false;
            }
        }
    }
}

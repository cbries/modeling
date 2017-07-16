using System;
using Newtonsoft.Json.Linq;

namespace TrackWeaver
{
    public enum WeaveItemT
    {
        S88
    }

    public class TrackWeaveItem
    {
        public WeaveItemT Type { get; set; }
        public int ObjectId { get; set; }
        public int Pin { get; set; }
        public int VisuX { get; set; }
        public int VisuY { get; set; }

        public bool Parse(JObject o)
        {
            if (o == null)
                return false;

            if (o["type"] != null)
            {
                var v = o["type"].ToString();
                if (string.IsNullOrEmpty(v))
                    return false;

                if (v.Equals("s88", StringComparison.OrdinalIgnoreCase))
                    Type = WeaveItemT.S88;
                else
                {
                    // TODO
                }
            }

            if (o["setup"] != null)
            {
                JObject os = o["setup"] as JObject;
                if (os == null)
                    return false;

                if (os["objectId"] != null)
                    ObjectId = (int) o["objectOd"];
                if (os["pin"] != null)
                    Pin = (int) os["pin"];
                if (os["visuX"] != null)
                    VisuX = (int) os["visuX"];
                if (os["visuY"] != null)
                    VisuY = (int) os["visuY"];
            }

            return true;
        }
    }
}

using System;
using Newtonsoft.Json.Linq;

namespace TrackWeaver
{
    public enum WeaveItemT
    {
        Unknown, S88, Switch, Accessory, RouteActivator
    }

    public class TrackWeaveItem
    {
        public WeaveItemT Type { get; set; }
        public int ObjectId { get; set; }
        public int Pin { get; set; }
        public int VisuX { get; set; }
        public int VisuY { get; set; }

        public TrackWeaveItem()
        {
            Type = WeaveItemT.Unknown;
            ObjectId = -1;
            VisuX = -1;
            VisuY = -1;
        }

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
                    ObjectId = (int) os["objectId"];
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

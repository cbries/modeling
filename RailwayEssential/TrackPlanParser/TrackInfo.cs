using System;
using Newtonsoft.Json.Linq;

namespace TrackPlanParser
{
    public class TrackInfo
    {
        public Func<bool> CheckState { get; set; }

        public int X { get; set; }
        public int Y { get; set; }
        public int ThemeId { get; set; }
        public string Orientation { get; set; }
        public string Description { get; set; }

        public TrackInfo()
        {
            X = -1;
            Y = -1;
            ThemeId = -1;
            Orientation = "rot0";
        }

        public JObject ToObject()
        {
            JObject o = new JObject
            {
                ["x"] = X,
                ["y"] = Y,
                ["themeId"] = ThemeId,
                ["orientation"] = Orientation,
                ["description"] = Description
            };
            return o;
        }

        public void Parse(JObject o)
        {
            if (o == null)
                return;

            if (o["x"] != null)
                X = (int) o["x"];
            if (o["y"] != null)
                Y = (int) o["y"];
            if (o["themeId"] != null)
                ThemeId = (int) o["themeId"];
            if (o["orientation"] != null)
                Orientation = o["orientation"].ToString();
            if (o["description"] != null)
                Description = o["description"].ToString();
        }

        public override string ToString()
        {
            return $"{X}:{Y} -> {ThemeId}";
        }
    }
}

using Newtonsoft.Json.Linq;

namespace RailwayEssentialMdi.Analyze
{
    public class WayPoint
    {
        public int ThemeId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Orientation { get; set; }
        public bool HasTurn { get; set; }

        public JObject ToJson()
        {
            JObject o = new JObject
            {
                ["themeId"] = ThemeId,
                ["x"] = X,
                ["y"] = Y,
                ["orientation"] = Orientation,
                ["hasTurn"] = HasTurn
            };
            return o;
        }

        public bool Parse(JToken tkn)
        {
            if (tkn == null)
                return false;

            try
            {
                JObject o = tkn as JObject;
                if (o == null)
                    return false;

                if (o["themeId"] != null)
                    ThemeId = (int) o["themeId"];

                if (o["x"] != null)
                    X = (int) o["x"];

                if (o["y"] != null)
                    Y = (int) o["y"];

                if (o["orientation"] != null)
                    Orientation = (int) o["orientation"];

                if (o["hasTurn"] != null)
                    HasTurn = (bool) o["hasTurn"];

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

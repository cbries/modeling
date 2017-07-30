using Newtonsoft.Json.Linq;

namespace RailwayEssentialMdi.DataObjects
{
    public class ProjectTrackView
    {
        public string Name { get; set; }
        public int StartX { get; set; }
        public int StartY { get; set; }
        public bool Show { get; set; }

        public bool Parse(JToken tkn)
        {
            var o = tkn as JObject;
            if (o == null)
                return false;

            try
            {
                if (o["name"] != null)
                    Name = o["name"].ToString();

                if (o["startX"] != null)
                    StartX = (int) o["startX"];

                if (o["startY"] != null)
                    StartY = (int) o["startY"];

                if (o["show"] != null)
                    Show = (bool) o["show"];

                return true;
            }
            catch
            {
                return false;
            }
        }

        public JObject ToJson()
        {
            JObject o = new JObject
            {
                ["name"] = Name,
                ["startX"] = StartX,
                ["startY"] = StartY,
                ["show"] = Show
            };
            return o;
        }

    }
}

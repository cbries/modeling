using Newtonsoft.Json.Linq;

namespace RailwayEssentialMdi.DataObjects
{
    public class ProjectTrack
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Weave { get; set; }
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

                if (o["path"] != null)
                    Path = o["path"].ToString();

                if (o["weave"] != null)
                    Weave = o["weave"].ToString();

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
                ["path"] = Path,
                ["weave"] = Weave,
                ["show"] = Show
            };
            return o;
        }
    }
}

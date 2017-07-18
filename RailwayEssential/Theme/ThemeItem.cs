using Newtonsoft.Json.Linq;

namespace Theme
{
    public class ThemeItemState
    {
        public string Default { get; set; }
        public string Occ { get; set; }
        public string Route { get; set; }

        public ThemeItemState(JObject o)
        {
            Parse(o);
        }

        public void Parse(JObject o)
        {
            if (o == null)
                return;
            if (o["default"] != null)
                Default = o["default"].ToString();
            if (o["occ"] != null)
                Occ = o["occ"].ToString();
            if (o["route"] != null)
                Route = o["route"].ToString();
        }
    }

    public class ThemeItem
    {
        public int UniqueIdentifier { get; set; }
        public string Name { get; set; }
        public bool Clickable { get; set; }
        public ThemeItemState Active { get; set; }
        public ThemeItemState Off { get; set; }

        public ThemeItem()
        {
            Clickable = false;
            Active = new ThemeItemState(null);
            Off = new ThemeItemState(null);
        }

        public bool Parse(JToken tkn)
        {
            JObject o = tkn as JObject;

            if (o == null)
                return false;

            if (o["id"] != null)
                UniqueIdentifier = (int) o["id"];
            if (o["name"] != null)
                Name = o["name"].ToString();
            if (o["clickable"] != null)
                Clickable = (bool) o["clickable"];
            if(o["active"] != null)
                Active = new ThemeItemState(o["active"] as JObject);
            if(o["off"] != null)
                Off = new ThemeItemState(o["off"] as JObject);

            return true;
        }
    }
}

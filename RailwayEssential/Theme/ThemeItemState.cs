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
}
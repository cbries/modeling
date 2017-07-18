using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Theme
{
    public class ThemeCategory
    {
        public string Name { get; set; }

        public List<ThemeItem> Objects { get; set; }

        public ThemeCategory()
        {
            Objects = new List<ThemeItem>();
        }

        public bool Parse(JToken tkn)
        {
            var o = tkn as JObject;
            if (o == null)
                return false;

            if (o["category"] != null)
                Name = o["category"].ToString();

            if (o["objects"] != null)
            {
                JArray ar = o["objects"] as JArray;
                if (ar == null)
                    return true;

                foreach (var e in ar)
                {
                    if (e == null)
                        continue;

                    var item = new ThemeItem();
                    if (item.Parse(e))
                        Objects.Add(item);
                }
            }

            return true;
        }
    }
}

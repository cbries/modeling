﻿using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Theme
{
    public class ThemeItem
    {
        public int UniqueIdentifier { get; set; }
        public string Name { get; set; }
        public bool Clickable { get; set; }
        public ThemeItemState Active { get; set; }
        public ThemeItemState Off { get; set; }
        public List<ThemeItemRoute> Routes { get; set; }
        public List<ThemeItemDimension> Dimensions { get; set; }

        public ThemeItem()
        {
            Clickable = false;
            Active = new ThemeItemState(null);
            Off = new ThemeItemState(null);
            Routes = new List<ThemeItemRoute>();
            Dimensions = new List<ThemeItemDimension>();
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

            if (o["routes"] != null)
            {
                var ar = o["routes"] as JArray;
                if (ar != null)
                {
                    foreach (var e in ar)
                    {
                        if (e == null)
                            continue;

                        ThemeItemRoute route = new ThemeItemRoute();
                        if (route.Parse(e))
                            Routes.Add(route);
                    }
                }
            }

            if (o["dimensions"] != null)
            {
                var ar = o["dimensions"] as JArray;
                if (ar != null)
                {
                    foreach (var e in ar)
                    {
                        if (e == null)
                            continue;

                        ThemeItemDimension dim = new ThemeItemDimension();
                        if(dim.Parse(e))
                            Dimensions.Add(dim);
                    }
                }
            }

            if (Dimensions == null || Dimensions.Count < 4)
            {
                int n = Dimensions.Count;

                for (int i = n - 1; i < 4; ++i)
                    Dimensions.Add(new ThemeItemDimension {Value = "1x1", X = 1, Y = 1});
            }

            return true;
        }

        /// <summary>
        /// 0:=left to right
        /// 1:=top to bottom
        /// 2:=right to left
        /// 3:=bottom to top
        /// In other words, turning is clockwise.
        /// </summary>
        /// <param name="rotmode"></param>
        /// <returns></returns>
        public ThemeItemRoute GetRoute(int rotmode)
        {
            if (rotmode < 0 || rotmode > 3)
                return null;
            if (Routes.Count != 4)
                return null;
            return Routes[rotmode];
        }
    }
}

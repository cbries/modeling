﻿using System;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json.Linq;

namespace TrackPlanParser
{
    public class TrackInfo
    {
        public Func<bool> CheckState { get; set; }

        public string Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int ThemeId { get; set; }
        public string Orientation { get; set; }
        public string Description { get; set; }
        public JObject Options { get; set; }

        public TrackInfo()
        {
            X = -1;
            Y = -1;
            ThemeId = -1;
            Orientation = "rot0";
            Options = new JObject();
        }

        public JObject ToObject()
        {
            JObject o = new JObject
            {
                ["name"] = Name,
                ["x"] = X,
                ["y"] = Y,
                ["themeId"] = ThemeId,
                ["orientation"] = Orientation,
                ["description"] = Description,
                ["options"] = Options
            };
            return o;
        }

        public void Parse(JObject o)
        {
            if (o == null)
                return;

            if (o["name"] != null)
                Name = o["name"].ToString();
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
            if(o["options"] != null)
                Options = o["options"] as JObject;
        }

        public override string ToString()
        {
            return $"{X}:{Y} -> {ThemeId}";
        }

        public void SetOption(string name, string value)
        {
            if (Options == null)
                Options = new JObject();

            if (value == null)
            {
                try
                {
                    Options.Remove(name);
                }
                catch
                {
                    // ignore
                }

                return;
            }

            Options[name] = value;
        }

        public string GetOption(string name)
        {
            if (Options == null)
                return null;

            if (Options[name] == null)
                return null;

            return Options[name].ToString();
        }
    }
}

using System;
using Newtonsoft.Json.Linq;

namespace TrackPlanParser
{
    public enum Orientation
    {
        North,
        East,
        West,
        South
    }

    public class TrackInfo
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string IconName { get; set; }
        public Orientation Orientation { get; set; }
        public string Description { get; set; }

        public TrackInfo()
        {
            X = -1;
            Y = -1;
            IconName = "blockstate";
            Orientation = Orientation.North;
        }

        public void Parse(string[] fields)
        {
            IconName = fields[0].Trim();

            var orientation = fields[1].Trim();
            if(orientation.Equals("west", StringComparison.OrdinalIgnoreCase))
                Orientation = Orientation.West;
            else if(orientation.Equals("north", StringComparison.OrdinalIgnoreCase))
                Orientation = Orientation.North;
            else if(orientation.Equals("east", StringComparison.OrdinalIgnoreCase))
                Orientation = Orientation.East;
            else if (orientation.Equals("south", StringComparison.OrdinalIgnoreCase))
                Orientation = Orientation.South;
            else
                Orientation = Orientation.North;

            var xparts = fields[2].Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
            var yparts = fields[3].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var p in xparts)
            {
                int v;
                if (int.TryParse(p, out v))
                    X = v;
                else
                    X = -1;
            }

            foreach (var p in yparts)
            {
                int v;
                if (int.TryParse(p, out v))
                    Y = v;
                else
                    Y = -1;
            }

            Description = fields[4].Trim().TrimStart('"').TrimEnd('"');
        }

        public JObject ToObject()
        {
            JObject o = new JObject
            {
                ["x"] = X,
                ["y"] = Y,
                ["iconName"] = IconName,
                ["orientation"] = (int) Orientation,
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
            if (o["iconName"] != null)
                IconName = o["iconName"].ToString();
            if (o["orientation"] != null)
                Orientation = (Orientation) (int) o["orientation"];
            if (o["description"] != null)
                Description = o["description"].ToString();
        }
    }
}

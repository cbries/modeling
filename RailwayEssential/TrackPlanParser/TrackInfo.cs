using System;
using System.Collections.Generic;

namespace TrackPlanParser
{
    public enum Orientation
    {
        West,
        North,
        East,
        South
    }

    public class TrackInfo
    {
        public List<int> X { get; set; }
        public List<int> Y { get; set; }
        public string IconName { get; set; }
        public Orientation Orientation { get; set; }
        public string Description { get; set; }

        public TrackInfo()
        {
            X = new List<int>();
            Y = new List<int>();
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
                    X.Add(v);
                else
                    X.Clear();
            }

            foreach (var p in yparts)
            {
                int v;
                if (int.TryParse(p, out v))
                    Y.Add(v);
                else
                    Y.Clear();
            }

            Description = fields[4].Trim().TrimStart('"').TrimEnd('"');
        }

        public int LengthX()
        {
            if (X.Count != 1)
            {
                if (X.Count > 1)
                    return Math.Abs(X[0] - X[1]) + 1;
            }

            return 1;
        }

        public int LengthY()
        {
            if (Y.Count != 1)
            {
                if (Y.Count > 1)
                    return Math.Abs(Y[0] - Y[1]) + 1;
            }

            return 1;
        }

    }
}

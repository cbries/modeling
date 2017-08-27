using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace RailwayEssentialMdi.Analyze
{
    public class AnalyzeResult
    {
        public int NumberOfRoutes { get; set; }

        public List<WayPoints> Routes { get; set; }

        public List<RouteGroup> RouteGroups { get; set; }

        public AnalyzeResult()
        {
            NumberOfRoutes = -1;
            Routes = new List<WayPoints>();
            RouteGroups= new List<RouteGroup>();
        }

        public override string ToString()
        {
            string m = "";
            m += $"Number of Routes: {NumberOfRoutes}\r\n";
            if (Routes != null)
            {
                for (int i = 0; i < NumberOfRoutes; ++i)
                {
                    var r = Routes[i];
                    m += $"#{i + 1} (Steps {r.Count}): ";
                    foreach (var rr in r)
                    {
                        if (rr.HasTurn)
                            m += $"{rr.Identifier}>->";
                        else
                            m += $"{rr.Identifier}->";
                    }
                    m += "END\r\n";
                }
            }
            m += $"Number of Groups: {RouteGroups.Count}\r\n";
            foreach (var grp in RouteGroups)
            {
                if (grp == null)
                    continue;
                m += grp + "\r\n";
            }
            return m;
        }
    }
}

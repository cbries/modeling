using System.Collections.Generic;

namespace RailwayEssentialMdi.Analyze
{
    public class AnalyzeResult
    {
        public int NumberOfRoutes { get; set; }

        public List<WayPoints> Routes { get; set; }

        public override string ToString()
        {
            string m = "";
            m += $"Number of Routes: {NumberOfRoutes}\r\n";
            if (Routes != null)
            {
                for (int i = 0; i < NumberOfRoutes; ++i)
                {
                    m += $"#{i + 1}: ";
                    var r = Routes[i];
                    foreach (var rr in r)
                        m += $"{rr.Identifier}->";
                    m += "END\r\n";
                }
            }
            return m;
        }
    }
}

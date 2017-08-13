using System.Diagnostics;

namespace RailwayEssentialMdi.Analyze
{
    public class AnalyzeResult
    {
        public int NumberOfRoutes { get; set; }

        public override string ToString()
        {
            string m = "";
            m += $"Number of Routes: {NumberOfRoutes}";
            return m;
        }
    }
}

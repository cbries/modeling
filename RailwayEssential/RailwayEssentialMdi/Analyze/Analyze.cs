using System.Diagnostics;
using RailwayEssentialMdi.ViewModels;

namespace RailwayEssentialMdi.Analyze
{

    public class Analyze
    {
        private readonly RailwayEssentialModel _model;
        

        public Analyze(RailwayEssentialModel model)
        {
            _model = model;
        }

        public AnalyzeResult Execute()
        {
            var m = new Map(_model);
            m.Build();

            foreach (var it in m.Items)
            {
                Trace.WriteLine(" ---- " + it.Identifier);
                Trace.WriteLine(" # Direction: " + it.DirectionInfo);
                Trace.WriteLine(" # Local Idx: " + it.Idx);
                Trace.WriteLine(" # Neighbour Idxs: " + string.Join(", ", it.GetReachableNeighbourIds()));
            }

            var edges = m.GetEdges();

            AnalyzeResult res = new AnalyzeResult();
            return res;
        }
    }
}

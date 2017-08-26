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

            var numberOfRoutes = m.GetRoutes();

            AnalyzeResult res = new AnalyzeResult
            {
                NumberOfRoutes = numberOfRoutes,
                Routes = m.Routes,
                RouteGroups = m.GetRouteGroups()
            };

            return res;
        }
    }
}

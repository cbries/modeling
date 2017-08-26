using System.Collections.Generic;

namespace RailwayEssentialMdi.Analyze
{
    public class RouteGroup
    {
        public int GroupId { get; }
        public string GroupName { get; set; }
        public List<Route> Routes { get; set; }

        private static int _groupIdCounter;

        public RouteGroup()
        {
            GroupId = _groupIdCounter;
            GroupName = $"RouteGroup{_groupIdCounter}";
            ++_groupIdCounter;

            Routes = new List<Route>();
        }

        public override string ToString()
        {
            string m = $"Group {GroupId} / {GroupName}, Elements: {Routes.Count}";

            return m;
        }
    }
}

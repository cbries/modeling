using RailwayEssentialMdi.Analyze;

namespace RailwayEssentialMdi.Items
{
    public class BlockRouteItem : TrackInformation.Item
    {
        public override string IconName
        {
            get => "trace.png";
            set { }
        }

        public Route RoutePoints { get; set; }

        public BlockRouteItem()
        {
            RoutePoints = new Route();
        }
    }
}

using System.Collections.Generic;
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

        public List<WayPoint> WayPoints { get; set; }

        public BlockRouteItem()
        {
            WayPoints = new List<WayPoint>();
        }
    }
}

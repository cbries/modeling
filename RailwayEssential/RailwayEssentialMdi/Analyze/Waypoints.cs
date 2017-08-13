using System.Collections.Generic;
using TrackPlanParser;

namespace RailwayEssentialMdi.Analyze
{
    public class Waypoints
    {
        public List<TrackInfo> Points { get; private set; }

        public Waypoints()
        {
            Points = new List<TrackInfo>();
        }

        public void AddPoint(TrackInfo trackInfo)
        {
            Points.Add(trackInfo);
        }
    }
}

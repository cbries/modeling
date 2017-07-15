// ReSharper disable InconsistentNaming

using TrackPlanParser;

namespace RailwayEssentialWeb
{
    public interface ITrackViewerJsCallback
    {
        ITrackEdit TrackEdit { get; set; }

        void message(string msg);
        void cellClicked(int x, int y, string symbol);
        void cellRotated(int x, int y, int orientation);
    }
}

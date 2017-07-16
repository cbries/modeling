using System.Diagnostics;
using TrackPlanParser;

namespace RailwayEssentialWeb
{
    public class TrackViewerJsCallback : ITrackViewerJsCallback
    {
        public ITrackEdit TrackEdit { get; set; }

        public void message(string msg)
        {
           //Trace.WriteLine("Message: " + msg.Trim());
        }

        public void cellClicked(int x, int y, string symbol)
        {
            //Trace.WriteLine("Clicked: " + x + ", " + y + ", " + symbol);
            if (TrackEdit != null)
                TrackEdit.ChangeSymbol(x, y, symbol);
        }

        public void cellRotated(int x, int y, string orientation)
        {
            //Trace.WriteLine("Rotated: " + x + ", " + y + ", " + orientation);
            if (TrackEdit != null)
                TrackEdit.RotateSymbol(x, y, orientation);
        }
    }
}

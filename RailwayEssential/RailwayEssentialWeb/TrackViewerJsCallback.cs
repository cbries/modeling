using System;
using System.Diagnostics;

namespace RailwayEssentialWeb
{
    public class TrackViewerJsCallback : ITrackViewerJsCallback
    {
        public ITrackViewer Viewer { get; set; }

        public void message(string msg)
        {
           Trace.WriteLine("Message: " + msg.Trim()); 
        }

        public void cellClicked(int x, int y, string symbol)
        {
            var instance = Viewer as TrackViewer;
            if (instance != null)
            {
                //var nextSvg = instance.WebGenerator.GetNextSvg();
                //var nextSvgUrl = new Uri(nextSvg);
                //var script = $"setCellImage({x}, {y}, '{nextSvgUrl.AbsoluteUri}');";
                //instance.ExecuteJs(script);

                Trace.WriteLine($"Cell({x}, {y}) {symbol}");
            }
        }

        public void cellRotated(int x, int y, int orientation)
        {
            Trace.WriteLine($"Cell({x}, {y}) {orientation}");
        }
    }
}

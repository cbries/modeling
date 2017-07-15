// ReSharper disable InconsistentNaming
namespace RailwayEssentialWeb
{
    public interface ITrackViewerJsCallback
    {
        void message(string msg);
        void cellClicked(int x, int y, string symbol);
        void cellRotated(int x, int y, int orientation);
    }
}

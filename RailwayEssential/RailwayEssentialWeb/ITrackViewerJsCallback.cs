// ReSharper disable InconsistentNaming
namespace RailwayEssentialWeb
{
    public interface ITrackViewerJsCallback
    {
        void message(string msg);
        void cellClicked(int x, int y);
    }
}

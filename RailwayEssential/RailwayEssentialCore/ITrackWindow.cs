namespace RailwayEssentialCore
{
    public interface ITrackWindow
    {
        void ViewerReady();

        void PromoteViewer(ITrackViewer trackViewer);
    }
}

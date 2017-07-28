namespace RailwayEssentialCore
{
    public interface ITrackWindow
    {
        object TrackView { get; set; }

        ITrackViewerZoom TrackViewZoomer { get; set; }

        void ViewerReady();

        void PromoteViewer(ITrackViewer trackViewer);
    }
}

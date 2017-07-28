namespace RailwayEssentialMdi.ViewModels
{
    using RailwayEssentialCore;
    using Commands;
    using Entities;

    public class TrackWindow : BaseWindow, ITrackWindow
    {
        public object TrackView { get; set; }
        public ITrackViewerZoom TrackViewZoomer { get; set; }

        private readonly TrackEntity _entity;

        public TrackEntity Entity => _entity;

        public RelayCommand ZoomResetCommand { get; }
        public RelayCommand ZoomPlusCommand { get; }
        public RelayCommand ZoomMinusCommand { get; }

        public TrackWindow(TrackEntity entity)
        {
            _entity = entity;

            ZoomResetCommand = new RelayCommand(ZoomReset);
            ZoomPlusCommand = new RelayCommand(ZoomPlus);
            ZoomMinusCommand = new RelayCommand(ZoomMinus);
        }

        private void ZoomReset(object p)
        {
            if (TrackViewZoomer == null)
                return;

            TrackViewZoomer.ZoomLevel = 0.0;
        }

        private void ZoomPlus(object p)
        {
            if (TrackViewZoomer == null)
                return;

            var v = TrackViewZoomer.ZoomLevel;
            v += TrackViewZoomer.ZoomLevelIncrement;
            TrackViewZoomer.ZoomLevel = v;
        }

        private void ZoomMinus(object p)
        {
            if (TrackViewZoomer == null)
                return;

            var v = TrackViewZoomer.ZoomLevel;
            v -= TrackViewZoomer.ZoomLevelIncrement;
            TrackViewZoomer.ZoomLevel = v;
        }

        #region ITrackWindow

        public void ViewerReady()
        {
            if (_entity != null)
                _entity.ViewerReady();
        }

        public void PromoteViewer(ITrackViewer trackViewer)
        {
            if (_entity != null)
                _entity.PromoteViewer(trackViewer);
        }

        #endregion
    }
}

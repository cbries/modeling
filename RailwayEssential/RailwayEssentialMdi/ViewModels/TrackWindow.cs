using RailwayEssentialCore;
using RailwayEssentialMdi.Entities;

namespace RailwayEssentialMdi.ViewModels
{
    public class TrackWindow : BaseWindow, ITrackWindow
    {
        public override string Title => "Track";

        private readonly TrackEntity _entity;

        public TrackEntity Entity => _entity;

        public TrackWindow(TrackEntity entity)
        {
            _entity = entity;
        }

        #region ITrackWindow

        public void ViewerReady()
        {
            if (_entity != null)
                _entity.ViewerReady();
        }

        public void PromoteViewer(ITrackViewer trackViewer)
        {
            if(_entity != null)
                _entity.PromoteViewer(trackViewer);
        }

        #endregion
    }
}

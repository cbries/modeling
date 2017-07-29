using System.Diagnostics;

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
        public RelayCommand EditCommand { get; }

        public RelayCommand PlusColumRightCommand { get; }
        public RelayCommand MinusColumnRightCommand { get; }
        public RelayCommand MinusRowBottomCommand { get; }
        public RelayCommand PlusRowBottomCommand { get; }

        public TrackWindow(TrackEntity entity)
        {
            _entity = entity;

            PlusColumRightCommand = new RelayCommand(PlusColumRight);
            MinusColumnRightCommand = new RelayCommand(MinusColumnRight);
            PlusRowBottomCommand = new RelayCommand(PlusRowBottom);
            MinusRowBottomCommand = new RelayCommand(MinusRowBottom);

            ZoomResetCommand = new RelayCommand(ZoomReset);
            ZoomPlusCommand = new RelayCommand(ZoomPlus);
            ZoomMinusCommand = new RelayCommand(ZoomMinus);
            EditCommand = new RelayCommand(EditState);
        }

        private void PlusColumRight(object p)
        {
            Trace.WriteLine("+ Column");
        }

        private void MinusColumnRight(object p)
        {
            Trace.WriteLine("- Column");
        }

        private void PlusRowBottom(object p)
        {
            Trace.WriteLine("+ Row");
        }

        private void MinusRowBottom(object p)
        {
            Trace.WriteLine("- Row");
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

        private void EditState(object p)
        {
            if(_entity == null)
                return;

            if (_entity.Viewer == null)
                return;

            _entity.Viewer.ExecuteJs("changeEditMode();");
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

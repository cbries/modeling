namespace RailwayEssentialMdi.ViewModels
{
    using System.Diagnostics;
    using RailwayEssentialCore;
    using DataObjects;
    using Commands;
    using Entities;

    public class TrackWindow : BaseWindow, ITrackWindow
    {
        public object TrackView { get; set; }
        public ProjectTrackView ProjectTrackView { get; set; }
        public ITrackViewerZoom TrackViewZoomer { get; set; }
        private readonly TrackEntity _entity;
        public TrackEntity Entity => _entity;
        public override string Name => ProjectTrackView.Name;
        
        public RelayCommand SaveCommand { get; }

        public RelayCommand ZoomResetCommand { get; }
        public RelayCommand ZoomPlusCommand { get; }
        public RelayCommand ZoomMinusCommand { get; }
        public RelayCommand EditCommand { get; }

        public RelayCommand PlusColumRightCommand { get; }
        public RelayCommand MinusColumnRightCommand { get; }
        public RelayCommand MinusRowBottomCommand { get; }
        public RelayCommand PlusRowBottomCommand { get; }
        


        public TrackWindow(TrackEntity entity, ProjectTrackView trackView)
        {
            _entity = entity;

            ProjectTrackView = trackView;

            PlusColumRightCommand = new RelayCommand(PlusColumRight);
            MinusColumnRightCommand = new RelayCommand(MinusColumnRight);
            PlusRowBottomCommand = new RelayCommand(PlusRowBottom);
            MinusRowBottomCommand = new RelayCommand(MinusRowBottom);

            ZoomResetCommand = new RelayCommand(ZoomReset);
            ZoomPlusCommand = new RelayCommand(ZoomPlus);
            ZoomMinusCommand = new RelayCommand(ZoomMinus);
            EditCommand = new RelayCommand(EditState, CheckEditState);

            SaveCommand = new RelayCommand(Save);

            RaisePropertyChanged("BlockEventNames");
        }

        private void Save(object p)
        {
            if (_entity == null)
                return;

            _entity.ApplyAssignment();
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

        private bool CheckEditState(object p)
        {
            if (_entity == null)
                return false;

            if (_entity.Dispatcher == null)
                return false;

            var m = _entity.Dispatcher.Model as RailwayEssentialModel;
            if (m == null)
                return false;

            if (m.IsDryRun)
                return true;

            if (!_entity.Dispatcher.GetRunMode())
                return false;

            return true;
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

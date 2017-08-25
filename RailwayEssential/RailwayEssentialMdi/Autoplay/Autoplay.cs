using System;
using System.ComponentModel;
using RailwayEssentialMdi.ViewModels;
using TrackInformation;

namespace RailwayEssentialMdi.Autoplay
{
    public partial class Autoplay
    {
        public EventHandler Started;
        public EventHandler Stopped;
        public EventHandler Failed;

        public RailwayEssentialModel Ctx { get; set; }

        private BackgroundWorker _worker = null;

        private bool _stopped = true;
        private bool _started = false;

        public bool IsRunning
        {
            get
            {
                if (_worker == null)
                    return false;

                if (_stopped && !_started)
                    return false;

                if (_worker.CancellationPending)
                    return false;

                return _started && _worker.IsBusy;
            }
        }

        public Autoplay()
        {
            
        }

        public bool Start()
        {
            if (_started)
                return _started;

            _stopped = false;
            _started = true;

            if (IsRunning)
                return true;

            _worker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            _worker.DoWork += Worker_DoWork;
            _worker.ProgressChanged += Worker_ProgressChanged;
            _worker.RunWorkerCompleted += Worker_RunWorkerCompleted;

            _worker.RunWorkerAsync();

            if (Started != null)
                Started(this, null);

            return true;
        }

        public bool Stop()
        {
            if (_stopped)
                return _stopped;

            _started = false;
            _stopped = true;

            if(_worker.WorkerSupportsCancellation)
                _worker?.CancelAsync();

            if (Stopped != null)
                Stopped(this, null);

            return true;
        }
    }
}

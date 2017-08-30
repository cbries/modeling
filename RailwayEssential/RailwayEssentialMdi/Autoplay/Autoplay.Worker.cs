using System;
using System.ComponentModel;
using System.Threading;

namespace RailwayEssentialMdi.Autoplay
{
    public partial class Autoplay
    {
        private static int WorkerDelay = 1 * 1000;

        private string GetTimeStr()
        {
            return string.Format("{0:MM/dd/yy H:mm:ss}", DateTime.UtcNow.ToLocalTime());
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var w = sender as BackgroundWorker;

            for(;;)
            {
                if (w != null && w.CancellationPending)
                {
                    e.Cancel = true;

                    return;
                }

                try
                {
                    Check();
                }
                catch
                {
                    // ignore
                }

                Thread.Sleep(WorkerDelay);
            }
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var msg = "";

            if(e.Cancelled)
                msg = "Canceled!";
            else if (e.Error != null)
                msg = "Error: " + e.Error.Message;
            else
                msg = "Done!";

            if (Ctx != null && Ctx._ctx != null)
            {
                Ctx._ctx.Send(state =>
                {
                    Ctx.LogAutoplay(msg);
                }, null);
            }
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //this.tbProgress.Text = (e.ProgressPercentage.ToString() + "%");
        }
    }
}

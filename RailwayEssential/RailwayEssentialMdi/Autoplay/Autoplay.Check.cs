using System.Diagnostics;
using TrackInformation;

namespace RailwayEssentialMdi.Autoplay
{
    public partial class Autoplay
    {
        private DataProvider DataProvider => Ctx?.Dispatcher?.GetDataProvider();
        private Dispatcher.Dispatcher Dispatcher => Ctx?.Dispatcher;
        
        private void Check()
        {
            Trace.WriteLine($"{GetTimeStr()} ## Autoplay::Check()");
        }
    }
}

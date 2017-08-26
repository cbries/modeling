using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TrackInformation;

namespace RailwayEssentialMdi.Autoplay
{
    public partial class Autoplay
    {
        private DataProvider DataProvider => Ctx?.Dispatcher?.GetDataProvider();
        private Dispatcher.Dispatcher Dispatcher => Ctx?.Dispatcher;

        private List<Locomotive> Locomotives => DataProvider.Objects.OfType<Locomotive>().ToList();

        private void Check()
        {
            Trace.WriteLine($"{GetTimeStr()} ## Autoplay::Check()");

            //Ctx.R

            // TODO
        }
    }
}

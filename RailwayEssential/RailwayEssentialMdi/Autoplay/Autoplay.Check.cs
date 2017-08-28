using System;
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
        private Theme.Theme Theme => Ctx?.Theme;

        private List<Locomotive> Locomotives => DataProvider.Objects.OfType<Locomotive>().ToList();

        private Random _rnd = new Random(DateTime.Now.Millisecond);

        private int _previousIdx = -1;

        private void Check()
        {
            Trace.WriteLine($"{GetTimeStr()} ## Autoplay::Check()");

            if (Ctx == null || Ctx.Project == null)
                return;

            // reset
            if (_previousIdx != -1)
                SetRoute(Ctx.Project.BlockRoutes[_previousIdx], false);

            var max = Ctx.Project.BlockRoutes.Count;           
            var idx = _rnd.Next(0, max);
            while (idx == _previousIdx)
                idx = _rnd.Next(0, max);
            _previousIdx = idx;
            var route = Ctx.Project.BlockRoutes[idx];
            SetRoute(route, true);
        }
    }
}

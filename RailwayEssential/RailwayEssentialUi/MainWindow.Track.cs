
using System;
using System.Diagnostics;
using System.IO;
using RailwayEssentialCore;
using RailwayEssentialWeb;
using TrackWeaver;

namespace RailwayEssentialUi
{
    public partial class MainWindow
    {
        //public ITrackViewerJsCallback JsCallback => TrackViewer.JsCallback;

        //private TrackPlanParser.Track _track;

        //public TrackPlanParser.Track Track => _track;

        public string FilePath { get; set; }

        //private string _tmpTrackName;

        //private Theme.Theme _theme;

        //public Theme.Theme Theme
        //{
        //    get
        //    {
        //        if (_theme == null)
        //        {
        //            var themePath = ThemeName.ExpandRailwayEssential();
        //            var themeDescription = themePath + ".json";
        //            _theme = new Theme.Theme();
        //            _theme.Load(themeDescription);
        //        }

        //        return _theme;
        //    }
        //}
        
        //private void GeneratePhysicalTrackViewerUi()
        //{
        //    if (TrackViewer.WebGenerator == null)
        //        return;

        //    TrackViewer.WebGenerator.Generate(_tmpTrackName);
        //}

        //private bool InitializeTrackViewerUi()
        //{
        //    TrackPlanParser.TrackPlanParser parser = new TrackPlanParser.TrackPlanParser(FilePath);
        //    parser.Parse();

        //    _track = parser.Track;

        //    TrackViewer.ViewerReady += delegate (object sender, EventArgs ev)
        //    {
        //        ITrackViewer trackViewer = sender as ITrackViewer;
        //        if (trackViewer == null)
        //            return;

        //        trackViewer.JsCallback.TrackEdit = _track;

        //        // load current track
        //        foreach (var item in _track)
        //        {
        //            if (item == null)
        //                continue;

        //            var themeItem = Theme.Get(item.ThemeId);
        //            if (themeItem != null)
        //            {
        //                var col = item.X;
        //                var row = item.Y;
        //                var symbol = Path.GetFileNameWithoutExtension(themeItem.Off.Default);
        //                var orientation = item.Orientation;

        //                TrackViewer.ExecuteJs($"simulateClick({col}, {row}, {item.ThemeId}, \"{symbol}\", \"{orientation}\");");
        //            }
        //        }
        //    };

        //    _tmpTrackName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + "_track.html";
        //    _tmpTrackName = Path.Combine(TrackplansEditor, _tmpTrackName);
        //    _tmpTrackName = _tmpTrackName.ExpandRailwayEssential();

        //    TrackViewer.WebGenerator = new WebGenerator { ThemeDirectory = ThemeName.ExpandRailwayEssential() };
        //    GeneratePhysicalTrackViewerUi();
        //    LoadTrackViewerUi();

        //    return true;
        //}

        //private void LoadTrackViewerUi()
        //{
        //    TrackViewer.Url = _tmpTrackName;
        //    TrackViewer.Reload();
        //}

        //private bool UpdateTrackViewerUi(TrackWeaver.TrackWeaver weaver)
        //{
        //    Trace.WriteLine(" ** UpdateUi() ** ");

        //    if (weaver == null)
        //        return false;

        //    var ws = weaver.WovenSeam;
        //    if (ws == null)
        //        return false;

        //    foreach (var seam in ws)
        //    {
        //        if (seam == null)
        //            continue;

        //        if (seam.TrackObjects.Count == 0)
        //            continue;

        //        foreach (var trackItem in seam.TrackObjects.Keys)
        //        {
        //            if (trackItem == null)
        //                continue;

        //            var checkState = seam.TrackObjects[trackItem];

        //            TrackWeaver.TrackCheckResult checkResult = null;
        //            if (checkState != null)
        //                checkResult = checkState();

        //            var x = trackItem.X;
        //            var y = trackItem.Y;
        //            var orientation = trackItem.Orientation;

        //            int themeId = trackItem.ThemeId;
        //            var themeObject = _theme.Get(themeId);
        //            string symbol = "";

        //            switch (seam.ObjectItem.TypeId())
        //            {
        //                case 1: // Locomotive
        //                {
        //                }
        //                    break;

        //                case 2: // Ecos2
        //                {
        //                }
        //                    break;

        //                case 3: // Route
        //                {
        //                }
        //                    break;

        //                case 4: // S88
        //                {
        //                    bool rS88 = checkResult?.State != null && checkResult.State.Value;

        //                    if (rS88)
        //                    {
        //                        if (seam.ObjectItem.IsRouted)
        //                            symbol = themeObject.Active.Route;
        //                        else
        //                            symbol = themeObject.Active.Default;
        //                    }
        //                    else
        //                    {
        //                        if (seam.ObjectItem.IsRouted)
        //                            symbol = themeObject.Off.Route;
        //                        else
        //                            symbol = themeObject.Off.Default;
        //                    }
        //                }
        //                    break;

        //                case 5: // Switch
        //                {
        //                    if (checkResult != null && checkResult.Direction.HasValue)
        //                    {
        //                        var direction = checkResult.Direction.Value;

        //                        if (direction == TrackCheckResult.SwitchDirection.Straight)
        //                        {
        //                            if (seam.ObjectItem.IsRouted)
        //                                symbol = themeObject.Active.Route;
        //                            else
        //                                symbol = themeObject.Active.Default;
        //                        }
        //                        else if (direction == TrackCheckResult.SwitchDirection.Turn)
        //                        {
        //                            if (seam.ObjectItem.IsRouted)
        //                                symbol = themeObject.Off.Route;
        //                            else
        //                                symbol = themeObject.Off.Default;
        //                        }
        //                        else
        //                        {
        //                            Trace.WriteLine("<Switch> Unknown direction: " + direction);
        //                        }
        //                    }
        //                }
        //                    break;

        //                default:
        //                    break;
        //            }

        //            TrackViewer.JsCallback.TrackEdit.ChangeSymbol(x, y, themeId);
        //            TrackViewer.ExecuteJs($"changeSymbol({x}, {y}, {themeId}, \"{orientation}\", \"{symbol}\");");

        //            Trace.WriteLine($"CHANGE: {x},{y} -> {symbol}");
        //        }
        //    }

        //    return true;

        //}
    }
}

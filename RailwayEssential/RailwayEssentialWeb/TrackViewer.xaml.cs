using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using CefSharp;
using RailwayEssentialCore;
using RailwayEssentialWeb.Cef;

namespace RailwayEssentialWeb
{
    public partial class TrackViewer : ITrackViewer, ITrackViewerZoom
    {
        private string _url;

        private readonly SynchronizationContext _ctx;

        public string Url
        {
            get { return _url; }
            set
            {
                _url = value;

                if (!string.IsNullOrEmpty(_url))
                    Browser.Load(_url);
            }
        }

        public IWebGenerator WebGenerator { get; set; }

        #region ITrackViewerZoom

        public double ZoomLevel
        {
            get => Browser.ZoomLevel;
            set { Browser.ZoomLevel = value; }
        }

        public double ZoomLevelIncrement => Browser.ZoomLevelIncrement;

        #endregion

        public TrackViewer()
        {
            var settings = new CefSettings { RemoteDebuggingPort = 1234 };
            // for some reason, performance sucks w/ the gpu enabled
            //settings.CefCommandLineArgs.Add("disable-gpu", "1");
            //settings.CefCommandLineArgs.Add("disable-gpu-vsync", "1");
            if(!CefSharp.Cef.IsInitialized)
                CefSharp.Cef.Initialize(settings);

            InitializeComponent();

            _ctx = SynchronizationContext.Current;

            Browser.ResourceHandlerFactory = new LocalResourceHandlerFactory();
            Browser.MenuHandler = new MenuHandler();

            Browser.BrowserSettings.FileAccessFromFileUrls = CefState.Enabled;
            Browser.BrowserSettings.UniversalAccessFromFileUrls = CefState.Enabled;
            Browser.BrowserSettings.WebSecurity = CefState.Disabled;
            //Browser.BrowserSettings.WebGl = CefSharp.CefState.Disabled;

            Browser.IsBrowserInitializedChanged += BrowserOnIsBrowserInitializedChanged;
            Browser.LoadError += BrowserOnLoadError;
            Browser.ConsoleMessage += BrowserOnConsoleMessage;
            Browser.LoadingStateChanged += BrowserOnLoadingStateChanged;
            Browser.FrameLoadEnd += BrowserOnFrameLoadEnd;

            _jsCallback = new TrackViewerJsCallback();

            Browser.RegisterJsObject("railwayEssentialCallback", _jsCallback);
        }

        private void BrowserOnConsoleMessage(object sender, ConsoleMessageEventArgs arg)
        {
            Trace.WriteLine($"<TackViewer> {arg.Line}: {arg.Message}");
        }

        private void BrowserOnFrameLoadEnd(object sender, FrameLoadEndEventArgs args)
        {
            _ctx.Send(state =>
            {
                if (args.Frame.IsMain)
                {
                    if (Browser.IsBrowserInitialized)
                    {
                        var vm = DataContext as ITrackWindow;
                        if (vm != null)
                            vm.ViewerReady();
                    }
                }
            }, null);
        }

        private void BrowserOnLoadingStateChanged(object sender, LoadingStateChangedEventArgs args)
        {
            Trace.WriteLine("Loading state: " + args.IsLoading);
        }

        public void Reload()
        {
            if (_ctx == null)
                return;

            _ctx.Send(state =>
            {
                if (Browser != null && Browser.IsBrowserInitialized)
                    Browser.Reload();
            }, null);
        }

        private void BrowserOnIsBrowserInitializedChanged(object sender, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            if (Url != null)
                Browser.Address = Url;
        }

        private void BrowserOnLoadError(object sender, LoadErrorEventArgs loadErrorEventArgs)
        {
            Trace.WriteLine("<error> " + loadErrorEventArgs.ErrorCode);
            Trace.WriteLine("<error> " + loadErrorEventArgs.ErrorText);
            Trace.WriteLine("<error> " + loadErrorEventArgs.FailedUrl);
        }

        private void Browser_OnInitialized(object sender, EventArgs e)
        {
        }

        private void TrackViewer_OnInitialized(object sender, EventArgs e)
        {
        }

        private void TrackViewer_OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var vm = DataContext as ITrackWindow;
            if (vm != null)
                vm.PromoteViewer(this);
        }

        #region ITrackViewer

        private readonly ITrackViewerJsCallback _jsCallback;

        public ITrackViewerJsCallback JsCallback => _jsCallback;

        public void ExecuteJs(string scriptCode)
        {
            _ctx.Send(state =>
            {
                if (string.IsNullOrEmpty(scriptCode))
                    return;

                if (Browser == null || !Browser.IsBrowserInitialized)
                    return;

                Browser.ExecuteScriptAsync(scriptCode.Trim());
            }, null);
        }

        public void SetUrl(string url)
        {
            Url = url;
        }

        public void Load()
        {
            Browser.Address = Url;
            Browser.Reload();
        }

        #endregion

    }
}

using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using CefSharp;
using RailwayEssentialWeb.Cef;

namespace RailwayEssentialWeb
{
    public partial class TrackViewer : UserControl, ITrackViewer
    {
        private string _url;
        private SynchronizationContext _ctx;

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

        public ITrackViewerJsCallback JsCallback { get; set; }

        public IWebGenerator WebGenerator { get; set; }

        public TrackViewer()
        {
            // for tests
            JsCallback = new TrackViewerJsCallback();

            var settings = new CefSharp.CefSettings { RemoteDebuggingPort = 1234 };
            // for some reason, performance sucks w/ the gpu enabled
            //settings.CefCommandLineArgs.Add("disable-gpu", "1");
            //settings.CefCommandLineArgs.Add("disable-gpu-vsync", "1");
            CefSharp.Cef.Initialize(settings);

            InitializeComponent();

            _ctx = SynchronizationContext.Current;

            Browser.ResourceHandlerFactory = new LocalResourceHandlerFactory();

            Browser.BrowserSettings.FileAccessFromFileUrls = CefSharp.CefState.Enabled;
            Browser.BrowserSettings.UniversalAccessFromFileUrls = CefSharp.CefState.Enabled;
            Browser.BrowserSettings.WebSecurity = CefSharp.CefState.Disabled;
            //Browser.BrowserSettings.WebGl = CefSharp.CefState.Disabled;

            Browser.IsBrowserInitializedChanged += BrowserOnIsBrowserInitializedChanged;
            Browser.LoadError += BrowserOnLoadError;

            Browser.RegisterJsObject("railwayEssentialCallback", JsCallback);
            (JsCallback as TrackViewerJsCallback).Viewer = this;
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

        #region ITrackViewer

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

        #endregion
    }
}

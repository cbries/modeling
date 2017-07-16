namespace RailwayEssentialWeb
{
    public delegate void ViewerReadyDelegate(object sender);

    public interface ITrackViewer
    {
        event ViewerReadyDelegate ViewerReady;

        ITrackViewerJsCallback JsCallback { get; }

        void ExecuteJs(string scriptCode);

    }
}

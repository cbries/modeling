namespace RailwayEssentialCore
{
    public interface ITrackViewer
    {
        ITrackViewerJsCallback JsCallback { get; }

        void ExecuteJs(string scriptCode);

        void SetUrl(string url);

        void Load();
    }
}

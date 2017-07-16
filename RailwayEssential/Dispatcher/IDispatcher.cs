using System.Runtime.InteropServices;

namespace Dispatcher
{
    public delegate void UpdateUiDelegate(object sender, TrackWeaver.TrackWeaver trackWeaver);

    public interface IDispatcher
    {
        event UpdateUiDelegate UpdateUi;
    }
}

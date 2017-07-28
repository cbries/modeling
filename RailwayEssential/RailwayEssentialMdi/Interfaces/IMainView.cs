using Xceed.Wpf.AvalonDock;

namespace RailwayEssentialMdi.Interfaces
{
    public interface IMainView
    {
        DockingManager GetDock();
        void LoadLayout();
    }
}

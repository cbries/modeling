using System.Windows.Controls.Primitives;

namespace RailwayEssentialMdi.Interfaces
{
    public interface ILocomotiveView
    {
        ToggleButton GetToggleButton(string name);
        void SetToggleButton(string name, bool state);
        void SetToggleButtonVisibility(string name, bool visible);
    }
}

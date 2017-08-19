namespace RailwayEssentialCore
{
    public interface IRailwayEssentialModel
    {
        void TriggerPropertyChanged(string name);
        void SetCurrentLocomotive(object locomotiveItem);
        void SetCurrentSwitch(object switchItem);
        void SetDirty(bool state);
        void ShowBlockRoutePreview(object blockRouteItem);
        void ResetBlockRoutePreview();
    }
}

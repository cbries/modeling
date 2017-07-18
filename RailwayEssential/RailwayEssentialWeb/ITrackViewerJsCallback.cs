// ReSharper disable InconsistentNaming

using TrackPlanParser;

namespace RailwayEssentialWeb
{
    public delegate void EditModeChangedDelegator(object sender, bool state);

    public delegate void CellClickedDelegator(object sender, int x, int y);

    public interface ITrackViewerJsCallback
    {
        event EditModeChangedDelegator EditModeChanged;
        event CellClickedDelegator CellClicked;

        ITrackEdit TrackEdit { get; set; }

        void message(string msg);
        void cellClicked(int x, int y);
        void cellEdited(int x, int y, int themeId);
        void cellRotated(int x, int y, string orientation);
        void editModeChanged(bool state);
    }
}

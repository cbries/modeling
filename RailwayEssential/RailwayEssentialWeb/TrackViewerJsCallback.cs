using System;
using RailwayEssentialCore;

namespace RailwayEssentialWeb
{
    public class TrackViewerJsCallback : ITrackViewerJsCallback
    {
        public event EventHandler CellEdited;
        public event EditModeChangedDelegator EditModeChanged;
        public event CellClickedDelegator CellClicked;
        public event CellSelectedDelegator CellSelected;

        public ITrackEdit TrackEdit { get; set; }

        public void message(string msg)
        {
           //Trace.WriteLine("Message: " + msg.Trim());
        }

        public void cellClicked(int x, int y)
        {
            if (CellClicked != null)
                CellClicked(this, x, y);
        }

        public void cellEdited(int x, int y, int themeId)
        {
            if(CellEdited != null)
                CellEdited(this, EventArgs.Empty);

            if (TrackEdit != null)
                TrackEdit.ChangeSymbol(x, y, themeId);
        }

        public void cellRotated(int x, int y, string orientation)
        {
            if (TrackEdit != null)
                TrackEdit.RotateSymbol(x, y, orientation);
        }

        public void cellSelected(int x, int y)
        {
            if (CellSelected != null)
                CellSelected(this, x, y);
        }

        public void editModeChanged(bool state)
        {
            if (EditModeChanged != null)
                EditModeChanged(this, state);
        }
    }
}

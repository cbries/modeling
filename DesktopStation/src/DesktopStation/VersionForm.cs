using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DesktopStation
{
    public partial class VersionForm : Form
    {
        public VersionForm()
        {
            InitializeComponent();
        }

        public void SetFormLanguage(Language inLangManager)
        {
            if (inLangManager.Loaded() == true)
            {
                this.Text = inLangManager.SetText("TxtInfoTitle", this.Text);

                label_Devices.Text = inLangManager.SetText("TxtInfoDevices", label_Devices.Text);

                button_Ok.Text = inLangManager.SetText("TxtOk", button_Ok.Text);

                lView_TrackBox.Columns[0].Text = inLangManager.SetText("TxtID", lView_TrackBox.Columns[0].Text);
                lView_TrackBox.Columns[1].Text = inLangManager.SetText("TxtTrackBoxType", lView_TrackBox.Columns[1].Text);
                lView_TrackBox.Columns[2].Text = inLangManager.SetText("TxtVersion", lView_TrackBox.Columns[2].Text);

                

            }

        }

    }
}

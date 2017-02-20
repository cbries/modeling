using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DesktopStation
{
    public partial class LeverEditForm : Form
    {
        public LeverEditForm()
        {
            InitializeComponent();
        }

        public void SetFormLanguage(Language inLangManager)
        {
            if (inLangManager.Loaded() == true)
            {
                this.Text = inLangManager.SetText("TxtLeverCfgOption", this.Text);

                label_ControlMode.Text = inLangManager.SetText("TxtLeverCfgControlMode", label_ControlMode.Text);
                gBox_LeverOption.Text = inLangManager.SetText("TxtLeverCfgControlModeOption", gBox_LeverOption.Text);
                label_Gears.Text = inLangManager.SetText("TxtLeverCfgAccelerationgear", label_Gears.Text);

                cBox_LeverMode.Items[0] = inLangManager.SetText("TxtLeverSpeed", cBox_LeverMode.Items[0].ToString());
                cBox_LeverMode.Items[1] = inLangManager.SetText("TxtLeverPower", cBox_LeverMode.Items[1].ToString());

                button_Ok.Text = inLangManager.SetText("TxtOk", button_Ok.Text);
                button_Cancel.Text = inLangManager.SetText("TxtCancel", button_Cancel.Text);


            }

        }
    }
}

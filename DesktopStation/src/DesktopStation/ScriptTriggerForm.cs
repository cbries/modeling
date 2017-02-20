using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DesktopStation
{
    public partial class ScriptTriggerForm : Form
    {
        public ScriptTriggerForm()
        {
            InitializeComponent();
        }

        public void SetFormLanguage(Language inLangManager)
        {
            if (inLangManager.Loaded() == true)
            {
                this.Text = inLangManager.SetText("TxtTriggerTitle", this.Text);
                label_S88SensorAddr.Text = inLangManager.SetText("TxtS88SensorAddr", label_S88SensorAddr.Text);
                label_Hour.Text = inLangManager.SetText("TxtClkCfgHours", label_Hour.Text);
                label_Minutes.Text = inLangManager.SetText("TxtClkCfgMinutes", label_Minutes.Text);
                label_Seconds.Text = inLangManager.SetText("TxtClkCfgSeconds", label_Seconds.Text);

                label_RunThreshold.Text = inLangManager.SetText("TxtCondItemRunning", label_RunThreshold.Text);
                label_StopThreshold.Text = inLangManager.SetText("TxtCondItemStopping", label_StopThreshold.Text);


                gBox_SpecTime.Text = inLangManager.SetText("TxtClkCfgSpecifiedTime", gBox_SpecTime.Text);
                gBox_Speed.Text = inLangManager.SetText("TxtSpeedThreshold", gBox_Speed.Text);

                gBox_Flag.Text = inLangManager.SetText("TxtCondItemFlag", gBox_Flag.Text);
                gBox_Route.Text = inLangManager.SetText("TxtCondItemRoute", gBox_Route.Text);

                labelSelectedRoute.Text = inLangManager.SetText("TxtRouteSelected", labelSelectedRoute.Text);
                labelFlagNo.Text = inLangManager.SetText("TxtScrEditFlagNo", labelFlagNo.Text);
                labelFlagVal.Text = inLangManager.SetText("TxtScrEditEquivVal", labelFlagVal.Text);
                
                
                button_Ok.Text = inLangManager.SetText("TxtOk", button_Ok.Text);
                button_Cancel.Text = inLangManager.SetText("TxtCancel", button_Cancel.Text);

            }

        }
    }
}

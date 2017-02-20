using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DesktopStation
{
    public partial class ScriptEditForm : Form
    {
        public ScriptEditForm()
        {
            InitializeComponent();

            panel_ScriptAcc.Parent = panel_ScriptOuter;
            panel_ScriptDirect.Parent = panel_ScriptOuter;
            panel_ScriptLocFunc.Parent = panel_ScriptOuter;
            panel_ScriptLocSpeed.Parent = panel_ScriptOuter;
            panel_ScriptPwr.Parent = panel_ScriptOuter;
            panel_ScriptWait.Parent = panel_ScriptOuter;
            panel_ScriptLine.Parent = panel_ScriptOuter;
            panel_Jump.Parent = panel_ScriptOuter;
            panel_LineLabel.Parent = panel_ScriptOuter;
            panel_SetFlag.Parent = panel_ScriptOuter;
            panel_RunFile.Parent = panel_ScriptOuter;
            panel_Free.Parent = panel_ScriptOuter;
            panelJumpRun.Parent = panel_ScriptOuter;
            panelRoute.Parent = panel_ScriptOuter;

            tabControl.Visible = false;
            
        }

        private void button_Ok_Click(object sender, EventArgs e)
        {

        }

        private void cBox_AddrDefinedLocSpd_CheckedChanged(object sender, EventArgs e)
        {
            cBox_LocAddressSpd.Enabled = !cBox_AddrReplacedLocSpd.Checked;
            cBox_ProtcolLocSpd.Enabled = !cBox_AddrReplacedLocSpd.Checked;

            if (cBox_LocAddressSpd.Enabled == false)
            {
                cBox_LocAddressSpd.Text = "";
            }

        }

        private void cBox_AddrDefinedLocFnc_CheckedChanged(object sender, EventArgs e)
        {
            cBox_LocAddressFnc.Enabled = !cBox_AddrReplacedLocFnc.Checked;
            cBox_ProtcolLocFnc.Enabled = !cBox_AddrReplacedLocFnc.Checked;

            if (cBox_LocAddressFnc.Enabled == false)
            {
                cBox_LocAddressFnc.Text = "";
            }
        }

        private void cBox_AddrDefinedLocDir_CheckedChanged(object sender, EventArgs e)
        {
            cBox_LocAddressDir.Enabled = !cBox_AddrReplacedLocDir.Checked;
            cBox_ProtcolLocDir.Enabled = !cBox_AddrReplacedLocDir.Checked;

            if (cBox_LocAddressDir.Enabled == false)
            {
                cBox_LocAddressDir.Text = "";
            }
        }

        public void SetFormLanguage(Language inLangManager)
        {
            if (inLangManager.Loaded() == true)
            {
                this.Text = inLangManager.SetText("TxtScrEditOption", this.Text);

                label_LocAddr.Text = inLangManager.SetText("TxtLocAddr", label_LocAddr.Text);
                label_LocAddr2.Text = inLangManager.SetText("TxtLocAddr", label_LocAddr2.Text);
                label_LocAddr3.Text = inLangManager.SetText("TxtLocAddr", label_LocAddr3.Text);
                label_Speed.Text = inLangManager.SetText("TxtScrEditSpeed", label_Speed.Text);
                label_TransitionTime.Text = inLangManager.SetText("TxtScrEditTransition", label_TransitionTime.Text);

                cBox_AddrReplacedLocFnc.Text = inLangManager.SetText("TxtScrEditReplaced", cBox_AddrReplacedLocFnc.Text);
                cBox_AddrReplacedLocSpd.Text = inLangManager.SetText("TxtScrEditReplaced", cBox_AddrReplacedLocSpd.Text);

                label_FncNo.Text = inLangManager.SetText("TxtScrEditFncNo", label_FncNo.Text);
                label_FncOnOff.Text = inLangManager.SetText("TxtScrEditFncOnOff", label_FncOnOff.Text);

                label_LocDirection.Text = inLangManager.SetText("TxtScrEditDirection", label_LocDirection.Text);
                label_AccAddr.Text = inLangManager.SetText("TxtScrEditAccAddress", label_AccAddr.Text);
                label_AccSwitch.Text = inLangManager.SetText("TxtScrEditSwitch", label_AccSwitch.Text);
                label_Power.Text = inLangManager.SetText("TxtScrEditPower", label_Power.Text);
                label_WaitTime.Text = inLangManager.SetText("TxtScrEditWaitTime", label_WaitTime.Text);
                label_LineNum.Text = inLangManager.SetText("TxtScrEditLineNum", label_LineNum.Text);
                label_LabelName.Text = inLangManager.SetText("TxtScrEditLabelName", label_LabelName.Text);
                label_LabelNameJmp.Text = inLangManager.SetText("TxtScrEditLabelName", label_LabelNameJmp.Text);
                label_FlagNo.Text = inLangManager.SetText("TxtScrEditFlagNo", label_FlagNo.Text);
                label_EquivVal.Text = inLangManager.SetText("TxtScrEditEquivVal", label_EquivVal.Text);
                label_FlagNo2.Text = inLangManager.SetText("TxtScrEditFlagNo", label_FlagNo2.Text);
                label_Value.Text = inLangManager.SetText("TxtScrEditValue", label_Value.Text);

                label_AppName.Text = inLangManager.SetText("TxtAppName", label_AppName.Text);
                label_TargetFile.Text = inLangManager.SetText("TxtAppTarget", label_TargetFile.Text);
                button_OpenTarget.Text = inLangManager.SetText("TxtOpen", button_OpenTarget.Text);

                labelRoutename.Text = inLangManager.SetText("TxtRoute", labelRoutename.Text);
                labeRouteLabel.Text = inLangManager.SetText("TxtScrEditLabelName", labeRouteLabel.Text);
                label_RuteState.Text = inLangManager.SetText("TxtRouteState", label_RuteState.Text);

                cBoxRouteState.Items[0] = inLangManager.SetText("TxtRouteOpen", (String)cBoxRouteState.Items[0]);
                cBoxRouteState.Items[1] = inLangManager.SetText("TxtRouteClose", (String)cBoxRouteState.Items[1]);

                labelJumpRunLocAddr.Text = inLangManager.SetText("TxtLocAddr", labelJumpRunLocAddr.Text);
                cBox_AddrReplacedLocJump.Text = inLangManager.SetText("TxtScrEditReplaced", cBox_AddrReplacedLocJump.Text);
                labelJumpRunlabel.Text = inLangManager.SetText("TxtScrEditLabelName", labelJumpRunlabel.Text);
                labelJumpRunEqualVal.Text = inLangManager.SetText("TxtScrEditEquivVal", labelJumpRunEqualVal.Text);


                button_Ok.Text = inLangManager.SetText("TxtOk", button_Ok.Text);
                button_Cancel.Text = inLangManager.SetText("TxtCancel", button_Cancel.Text);


            }

        }

        private void button_OpenTarget_Click(object sender, EventArgs e)
        {
            OpenFileDialog aForm = new OpenFileDialog();
            aForm.CheckFileExists = true;
            aForm.Filter = "Sound files(*.mp3;*.wav;*.mp4;*.ogg;*.mid;)|*.*|All files(*.*)|*.*";

            //押されたボタン別の処理 
            if (aForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox_TargetFile.Text = aForm.FileName;
            }

            aForm.Dispose();
        }

        private void cBox_AddrReplacedLocJump_CheckedChanged(object sender, EventArgs e)
        {
            cBox_JumpRunLocAddr.Enabled = !cBox_AddrReplacedLocJump.Checked;
            cBox_JumpRunLocProt.Enabled = !cBox_AddrReplacedLocJump.Checked;

        }
    }
}

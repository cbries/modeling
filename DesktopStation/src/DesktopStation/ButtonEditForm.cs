using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DesktopStation
{
    public partial class ButtonEditForm : Form
    {
        public ButtonEditForm()
        {
            InitializeComponent();
        }

        private void cBox_FunctionImage_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();

            FunctionImageList.Draw(e.Graphics, (e.Bounds.Width - 28) / 2, e.Bounds.Y, e.Index);


            e.DrawFocusRectangle();
        }

        public void SetFormLanguage(Language inLangManager)
        {
            if (inLangManager.Loaded() == true)
            {
                this.Text = inLangManager.SetText("TxtBtnCfgOption", this.Text);

                cBox_FunctionSwitch.Text = inLangManager.SetText("TxtBtnCfgMomentaryMode", cBox_FunctionSwitch.Text);
                gBox_FuncImage.Text = inLangManager.SetText("TxtBtnCfgFncImage", gBox_FuncImage.Text);

                buttonOpen.Text = inLangManager.SetText("TxtOpen", buttonOpen.Text);
                label_AppName.Text = inLangManager.SetText("TxtAppName", label_AppName.Text);
                label_Filename.Text = inLangManager.SetText("TxtAppTarget", label_Filename.Text);

                label_Assignment.Text = inLangManager.SetText("TxtBtnCfgAssignment", label_Assignment.Text);
                gBox_AssignRunFile.Text = inLangManager.SetText("TxtBtnCfgAssignFunction", gBox_AssignRunFile.Text);
                cBox_Assignment.Items[0] = inLangManager.SetText("TxtAssignItemDefault", cBox_Assignment.Items[0].ToString());
                cBox_Assignment.Items[1] = inLangManager.SetText("TxtAssignItemExApp", cBox_Assignment.Items[1].ToString());
                cBox_Assignment.Items[2] = inLangManager.SetText("TxtAssignItemExLoc", cBox_Assignment.Items[2].ToString());

                label_LocAddr.Text = inLangManager.SetText("TxtLocAddr", label_LocAddr.Text);
                label_LocProtcol.Text = inLangManager.SetText("TxtLocProtcol", label_LocProtcol.Text);
                label_FunctionNo.Text = inLangManager.SetText("TxtLocFunction", label_FunctionNo.Text);
                gBox_AssignLoc.Text = inLangManager.SetText("TxtAssignLoc", gBox_AssignLoc.Text);



                button_Ok.Text = inLangManager.SetText("TxtOk", button_Ok.Text);
                button_Cancel.Text = inLangManager.SetText("TxtCancel", button_Cancel.Text);
                

            }
        }

        public void SetStateAssignRunFile(int inSelectedIndex)
        {

            cBox_FileName.Enabled = inSelectedIndex == 1 ? true : false;
            cBox_AppName.Enabled = inSelectedIndex == 1 ? true : false;
            buttonOpen.Enabled = inSelectedIndex == 1 ? true : false;

            tBox_Addr.Enabled = inSelectedIndex == 2 ? true : false;
            cBox_FunctionNo.Enabled = inSelectedIndex == 2 ? true : false;
            cBox_ProtcolLoc.Enabled = inSelectedIndex == 2 ? true : false;

            cBox_FunctionSwitch.Enabled = !(inSelectedIndex == 1 ? true : false);

        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog aForm = new OpenFileDialog();
            aForm.CheckFileExists = true;
            aForm.Filter = "Sound files(*.mp3;*.wav;*.mp4;*.ogg;*.mid;)|*.*|All files(*.*)|*.*";

            //押されたボタン別の処理 
            if (aForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                cBox_FileName.Text = aForm.FileName;
            }

            aForm.Dispose();
        }

        private void cBox_Assignment_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox aCBox = sender as ComboBox;

            SetStateAssignRunFile(aCBox.SelectedIndex);
        }
    }
}

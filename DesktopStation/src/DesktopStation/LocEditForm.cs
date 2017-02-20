using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using DesktopStation;

namespace DesktopStation
{
    public partial class LocEditForm : Form
    {
        public LocEditForm()
        {
            InitializeComponent();
        }

        private void buttonLoadLocImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog aForm = new OpenFileDialog();
			aForm.CheckFileExists = true;
			aForm.Filter = "Image file(png,jpg,bmp,gif)|*.png;*.jpg;*.jpeg;*.gif;*.bmp|All files(*.*)|*.*";
			aForm.Title = "Load a locomotive image file";

			//押されたボタン別の処理 
			if (aForm.ShowDialog() == System.Windows.Forms.DialogResult.OK) 
			{
                /* ファイルコピー処理 */
                DSCommon.CopyImageFile(aForm.FileName, Application.StartupPath + "\\images" );

                /* パスのセット */
                LocImageBox.ImageLocation = Application.StartupPath + "\\images\\" + Path.GetFileName(aForm.FileName);
			}

			aForm.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LocImageBox.ImageLocation = "";
        }

        private void cBox_ProtcolLoc1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox aCBox = sender as ComboBox;

            tBox_MfxUID.Enabled = aCBox.SelectedIndex == Program.PROTCOL_MFX ? true : false;
            cBox_SpeedStep.Enabled = aCBox.SelectedIndex == Program.PROTCOL_DCC ? true : false;

        }

        public void SetFormLanguage(Language inLangManager)
        {
            if (inLangManager.Loaded() == true)
            {
                this.Text = inLangManager.SetText("TxtCfgLocEdit", this.Text);

                label_LocAddr.Text = inLangManager.SetText("TxtLocAddr", label_LocAddr.Text);
                label_LocProtcol.Text = inLangManager.SetText("TxtLocProtcol", label_LocProtcol.Text);
                label_MfxDecoderUID.Text = inLangManager.SetText("TxtCfgMfxUID", label_MfxDecoderUID.Text);
                label_LocName.Text = inLangManager.SetText("TxtCfgLocName", label_LocName.Text);
                label_Manufacture.Text = inLangManager.SetText("TxtCfgManufacture", label_Manufacture.Text);
                label_ArtNo.Text = inLangManager.SetText("TxtCfgArtNo", label_ArtNo.Text);

                gBox_LocImage.Text = inLangManager.SetText("TxtCfgLocImage", gBox_LocImage.Text);
                label_LocImageDescription.Text = inLangManager.SetText("TxtCfgLocImageMax", label_LocImageDescription.Text);
                buttonLoadLocImage.Text = inLangManager.SetText("TxtLoad", buttonLoadLocImage.Text);
                buttonClearLocImage.Text = inLangManager.SetText("TxtClear", buttonClearLocImage.Text);

                gBox_DblHead.Text = inLangManager.SetText("TxtCfgDblHead", gBox_DblHead.Text);
                label_LocAddr2.Text = inLangManager.SetText("TxtCfgLocAddr2nd", label_LocAddr2.Text);
                label_LocProtcol2.Text = inLangManager.SetText("TxtCfgLocProtcol2nd", label_LocProtcol2.Text);

                gBox_SpdAdj.Text = inLangManager.SetText("TxtCfgSpdAdj", gBox_SpdAdj.Text);
                label_SpdAccRatio.Text = inLangManager.SetText("TxtCfgSpdAccRatio", label_SpdAccRatio.Text);
                label_SpdDecRatio.Text = inLangManager.SetText("TxtCfgSpdDecRatio", label_SpdDecRatio.Text);
                label_SpdMaxVal.Text = inLangManager.SetText("TxtCfgSpdMaxValue", label_SpdMaxVal.Text);

                gBox_SpdMeter.Text = inLangManager.SetText("TxtCfgSpdMeter", gBox_SpdMeter.Text);
                label_SpdMeterMax.Text = inLangManager.SetText("TxtCfgMeterMax", label_SpdMeterMax.Text);

                label_Speedstep.Text = inLangManager.SetText("TxtLocSpeedstep", label_Speedstep.Text);
                label_Speedstep2.Text = inLangManager.SetText("TxtLocSpeedstep", label_Speedstep2.Text);

                button_Ok.Text = inLangManager.SetText("TxtOk", button_Ok.Text);
                button_Cancel.Text = inLangManager.SetText("TxtCancel", button_Cancel.Text);

            }

        }

        private void label_LocAddr_TextChanged(object sender, EventArgs e)
        {
            /* 現状サイズが大きい場合は、フォントサイズを自動調整する */

            Control aUIParts = sender as Control;

            Size stringSize = TextRenderer.MeasureText(aUIParts.Text, aUIParts.Font);

            if (aUIParts.Size.Width < stringSize.Width)
            {
                Font aFont = new Font("Arial", 8);
                aUIParts.Font = aFont;
            }
        }

        private void cBox_ProtcolLoc2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox aCBox = sender as ComboBox;

            cBox_Speedstep2.Enabled = aCBox.SelectedIndex == Program.PROTCOL_DCC ? true : false;

        }

    }
}

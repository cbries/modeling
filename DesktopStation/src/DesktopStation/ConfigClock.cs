using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;


namespace DesktopStation
{
    public partial class ConfigClock : Form
    {
        private MeterDrawing MeterDrawer;

        public ConfigClock()
        {
            InitializeComponent();

            /* 処理クラス */
            MeterDrawer = new MeterDrawing();

        }

        private void pBox_Clock_Paint(object sender, PaintEventArgs e)
        {
            DateTime aTime;

            /* 描画 */
            Graphics aCanvas = e.Graphics;

            //(アンチエイリアス処理されたレタリング)を指定する
            aCanvas.SmoothingMode = SmoothingMode.AntiAlias;

            if (cBox_UseUserClock.Checked == true)
            {

                /* 時刻の算出 */
                aTime = new DateTime(2013, 1, 1, Decimal.ToInt32(numUpDown_hour.Value), Decimal.ToInt32(numUpDown_min.Value), Decimal.ToInt32(numUpDown_sec.Value));

            }
            else
            {
                aTime = DateTime.Now;

            }

            /* 時計の描画 */
            MeterDrawer.DrawClockBox(aCanvas, aTime, 100);

        }

        private void cBox_UseUserClock_CheckedChanged(object sender, EventArgs e)
        {
            pBox_Clock.Refresh();
        }

        private void numUpDown_hour_ValueChanged(object sender, EventArgs e)
        {
            pBox_Clock.Refresh();

        }

        public void SetFormLanguage(Language inLangManager)
        {
            if (inLangManager.Loaded() == true)
            {
                this.Text = inLangManager.SetText("TxtClkCfgOption", this.Text);
                cBox_UseUserClock.Text = inLangManager.SetText("TxtClkCfgUse", cBox_UseUserClock.Text);
                gBox_SpecTime.Text = inLangManager.SetText("TxtClkCfgSpecifiedTime", gBox_SpecTime.Text);
                label_Hour.Text = inLangManager.SetText("TxtClkCfgHours", label_Hour.Text);
                label_Minutes.Text = inLangManager.SetText("TxtClkCfgMinutes", label_Minutes.Text);
                label_Seconds.Text = inLangManager.SetText("TxtClkCfgSeconds", label_Seconds.Text);

                button_Ok.Text = inLangManager.SetText("TxtOk", button_Ok.Text);
                button_Cancel.Text = inLangManager.SetText("TxtCancel", button_Cancel.Text);

            }

        }

        private void label_Seconds_TextChanged(object sender, EventArgs e)
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
    }
}

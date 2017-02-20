using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DesktopStation
{
    public partial class SelectLocForm : Form
    {
        private MeterDrawing MeterDrawer;
        private LocomotiveDB LocDB_spair;

        public SelectLocForm(LocomotiveDB inLocDB)
        {
            InitializeComponent();

            /* 処理クラス */
            MeterDrawer = new MeterDrawing();

            LocDB_spair = inLocDB;
        
        }

        private void cBox_locomotives_DrawItem(object sender, DrawItemEventArgs e)
        {
            int aIndex = e.Index - 1;

            /* 描画アイテム */

            e.DrawBackground();

            if (LocDB_spair.Items.Count <= 0)
            {
                return;
            }

            if( aIndex < 0)
            {
                /* NotSelectedを表示する（選択しない条件を設定できるようにするため） */
                MeterDrawer.DrawLocLabel(e.Graphics, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height, LocDB_spair.Items.Count, Brushes.Black, LocDB_spair, cBox_locomotives.Items[0].ToString(), 100);
            }
            else
            {
                /* 通常の機関車データを表示 */
                MeterDrawer.DrawLocLabel(e.Graphics, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height, aIndex, Brushes.Black, LocDB_spair, cBox_locomotives.Items[0].ToString(), 100);
            }

            /* 選択枠描画 */
            e.DrawFocusRectangle();

        }

        public void SetFormLanguage(Language inLangManager)
        {
            this.Text = inLangManager.SetText("TxtSelectLocOption", this.Text);

            button_Ok.Text = inLangManager.SetText("TxtOk", button_Ok.Text);
            button_Cancel.Text = inLangManager.SetText("TxtCancel", button_Cancel.Text);


        }
    }
}

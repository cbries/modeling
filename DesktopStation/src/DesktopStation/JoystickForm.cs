using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DesktopStation
{
    public partial class JoystickForm : Form
    {
        public JoystickForm()
        {
            InitializeComponent();
        }

        private void cBox_loc0_DrawItem(object sender, DrawItemEventArgs e)
        {
            /* 描画アイテム */

            e.DrawBackground();

            if ((e.Index < 0) || (cBox_loc0.Items.Count <= 0))
            {
                return;
            }

            //MeterDrawer.DrawLocLabel(e.Graphics, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height, e.Index, Brushes.Black, LocDB, "");

            /* 選択枠描画 */
            e.DrawFocusRectangle();



        }
    }
}

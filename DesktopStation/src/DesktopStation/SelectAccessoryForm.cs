using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace DesktopStation
{
    public partial class SelectAccessoryForm : Form
    {
        public int SelectedIndex;
        private readonly ImageList _accImgList;


        public SelectAccessoryForm(ImageList inImgList)
        {
            InitializeComponent();
            _accImgList = inImgList;
        }

        private bool checkEvenIndex(int inValue)
        {
            return (inValue % 2 == 1);
        }

        private void cBox_AccType_SelectedValueChanged(object sender, EventArgs e)
        {
            switch (cBox_AccType.SelectedIndex)
            {
                case 4:
                case 6:
                case 8:
                case 9:
                    if (checkEvenIndex(SelectedIndex))
                    {
                        MessageBox.Show(@"This type is available in the odd address.", @"Error", MessageBoxButtons.OK);
                        cBox_AccType.SelectedIndex = 0;
                    }
                    break;
            }
        }

        private void cBox_AccType_DrawItem(object sender, DrawItemEventArgs e)
        {
            int aIndex;

            /* インデックス選定 */
            switch (e.Index)
            {
                case 0: aIndex = 0; break;
                case 1: aIndex = 6; break;
                case 2: aIndex = 9; break;
                case 3: aIndex = 12; break;
                case 4: aIndex = 2; break;
                case 5: aIndex = 14; break;
                case 6: aIndex = 16; break;
                case 7: aIndex = 19; break;
                case 8: aIndex = 21; break;
                case 9: aIndex = 24; break;
                default: aIndex = 0; break;
            }

            e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            e.DrawBackground();
            e.Graphics.DrawImage(_accImgList.Images[aIndex], e.Bounds.X, e.Bounds.Y, 16, 32);

            string aText = (string)cBox_AccType.Items[e.Index];

            Font aDrawFont2 = new Font("Arial", 16, FontStyle.Bold);
            float aYPos = (e.Bounds.Height - e.Graphics.MeasureString(aText, aDrawFont2).Height) / 2;
            e.Graphics.DrawString(aText, aDrawFont2, Brushes.Black, e.Bounds.X + 18, e.Bounds.Y + aYPos);
            aDrawFont2.Dispose();
            e.DrawFocusRectangle();
        }

        public void SetFormLanguage(Language inLangManager)
        {
            if (inLangManager.Loaded())
            {
                Text = inLangManager.SetText("TxtAccEditTitle", Text);

                label_AccList.Text = inLangManager.SetText("TxtAccEditType", label_AccList.Text);
                label_Comment.Text = inLangManager.SetText("TxtAccEditComment", label_Comment.Text);
                cBox_ReverseSignal.Text = inLangManager.SetText("TxtAccReverseSignal", cBox_ReverseSignal.Text);
                cBox_Invisible.Text = inLangManager.SetText("TxtAccInvisible", cBox_Invisible.Text);

                cBox_AccType.Items[0] = inLangManager.SetText("TxtSigItemSignal", cBox_AccType.Items[0].ToString());
                cBox_AccType.Items[1] = inLangManager.SetText("TxtSigItemTurnoutleft", cBox_AccType.Items[1].ToString());
                cBox_AccType.Items[2] = inLangManager.SetText("TxtSigItemTurnoutright", cBox_AccType.Items[2].ToString());
                cBox_AccType.Items[3] = inLangManager.SetText("TxtSigItemDoubleslipswitch", cBox_AccType.Items[3].ToString());
                cBox_AccType.Items[4] = inLangManager.SetText("TxtSigItemThreewayturnout", cBox_AccType.Items[4].ToString());
                cBox_AccType.Items[5] = inLangManager.SetText("TxtSigItemYardSignal", cBox_AccType.Items[5].ToString());
                cBox_AccType.Items[6] = inLangManager.SetText("TxtSigItemDistantSignal", cBox_AccType.Items[6].ToString());
                cBox_AccType.Items[7] = inLangManager.SetText("TxtSigItemHomeSignal76391", cBox_AccType.Items[7].ToString());
                cBox_AccType.Items[8] = inLangManager.SetText("TxtSigItemHomeSignal76393", cBox_AccType.Items[8].ToString());
                cBox_AccType.Items[9] = inLangManager.SetText("TxtSigItemHomeSignal76394", cBox_AccType.Items[9].ToString());

                button_Ok.Text = inLangManager.SetText("TxtOk", button_Ok.Text);
                button_Cancel.Text = inLangManager.SetText("TxtCancel", button_Cancel.Text);
            }
        }
    }
}

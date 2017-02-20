using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DesktopStation
{
    public partial class SerialConfigForm : Form
    {
        public bool NonVisibleOption;

        public SerialConfigForm()
        {
            InitializeComponent();

            NonVisibleOption = false;
        }

        private void cBox_ScreenNums_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cBox_ScreenNums.Enabled == true)
            {

                switch (cBox_ScreenNums.SelectedIndex)
                {
                    case 1:
                        cBox_Screen1Panel.Enabled = true;
                        cBox_Screen2Panel.Enabled = true;
                        cBox_Screen3Panel.Enabled = false;
                        cBox_Screen4Panel.Enabled = false;
                        break;

                    case 2:
                        cBox_Screen1Panel.Enabled = true;
                        cBox_Screen2Panel.Enabled = false;
                        cBox_Screen3Panel.Enabled = true;
                        cBox_Screen4Panel.Enabled = false;
                        break;

                    case 3:
                        cBox_Screen1Panel.Enabled = true;
                        cBox_Screen2Panel.Enabled = true;
                        cBox_Screen3Panel.Enabled = true;
                        cBox_Screen4Panel.Enabled = true;
                        break;

                }
            }
            else
            {
                cBox_Screen1Panel.Enabled = false;
                cBox_Screen2Panel.Enabled = false;
                cBox_Screen3Panel.Enabled = false;
                cBox_Screen4Panel.Enabled = false;
            }


        }

        private void cBox_Screen1Panel_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool aCheckSameIndex = false;

            /* 排他検出処理 */

            aCheckSameIndex = aCheckSameIndex | checkSameIndex(cBox_Screen1Panel.SelectedIndex, cBox_Screen2Panel.SelectedIndex);
            aCheckSameIndex = aCheckSameIndex | checkSameIndex(cBox_Screen1Panel.SelectedIndex, cBox_Screen3Panel.SelectedIndex);
            aCheckSameIndex = aCheckSameIndex | checkSameIndex(cBox_Screen1Panel.SelectedIndex, cBox_Screen4Panel.SelectedIndex);
            aCheckSameIndex = aCheckSameIndex | checkSameIndex(cBox_Screen2Panel.SelectedIndex, cBox_Screen3Panel.SelectedIndex);
            aCheckSameIndex = aCheckSameIndex | checkSameIndex(cBox_Screen2Panel.SelectedIndex, cBox_Screen4Panel.SelectedIndex);
            aCheckSameIndex = aCheckSameIndex | checkSameIndex(cBox_Screen3Panel.SelectedIndex, cBox_Screen4Panel.SelectedIndex);

            if (aCheckSameIndex == true)
            {
                ComboBox aBox = (ComboBox)sender;
                aBox.SelectedIndex = 0;

                MessageBox.Show("Do not select same panel!");
            }

        }

        private bool checkSameIndex(int inA, int inB)
        {
            bool aResult = false;

            if ((inA == inB) && (inA > 0))
            {
                aResult = true;
            }

            return aResult;
        }


        public void SetFormLanguage(Language inLangManager)
        {
            if (inLangManager.Loaded() == true)
            {
                this.Text = inLangManager.SetText("TxtOption", this.Text);

                labelConnectType.Text = inLangManager.SetText("TxtCfgConnectType", labelConnectType.Text);
                cBox_ConnectionType.Items[0] = inLangManager.SetText("TxtCfgConnectType1", cBox_ConnectionType.Items[0].ToString());
                cBox_ConnectionType.Items[1] = inLangManager.SetText("TxtCfgConnectType2", cBox_ConnectionType.Items[1].ToString());
                cBox_ConnectionType.Items[2] = inLangManager.SetText("TxtCfgConnectType3", cBox_ConnectionType.Items[2].ToString());                
                gBox_Http.Text = inLangManager.SetText("TxtCfgHTTPWeb", gBox_Http.Text);
                label_IPAddr.Text = inLangManager.SetText("TxtCfgIPAddress", label_IPAddr.Text);


                gBox_Serial.Text = inLangManager.SetText("TxtCfgSerialPort", gBox_Serial.Text);
                label_PortNo.Text = inLangManager.SetText("TxtCfgSerialPortNo", label_PortNo.Text);
                label_Baudrate.Text = inLangManager.SetText("TxtCfgSerialPortBaudrate", label_Baudrate.Text);
                cBox_DtrEnable.Text = inLangManager.SetText("TxtCfgSerialPortUseDTR", cBox_DtrEnable.Text);

                gBox_S88.Text = inLangManager.SetText("TxtCfgS88Sensor", gBox_S88.Text);
                cBox_S88.Text = inLangManager.SetText("TxtCfgS88Use", cBox_S88.Text);
                label_S88DetectInterval.Text = inLangManager.SetText("TxtCfgS88DetectInterval", label_S88DetectInterval.Text);
                label_S88SensorNums.Text = inLangManager.SetText("TxtCfgS88NumOfConnection", label_S88SensorNums.Text);

                gBox_Other.Text = inLangManager.SetText("TxtCfgOthers", gBox_Other.Text);
                cBox_StopAllLocPon.Text = inLangManager.SetText("TxtCfgStopAllLoc", cBox_StopAllLocPon.Text);
                cBox_AutoCloseSerial.Text = inLangManager.SetText("TxtCfgAutoCloseSerial", cBox_AutoCloseSerial.Text);
                cBox_ClearAcc.Text = inLangManager.SetText("TxtCfgClearAcc", cBox_ClearAcc.Text);

                gBox_AddVisible.Text = inLangManager.SetText("TxtCfgCabAddtional", gBox_AddVisible.Text);
                label_VisibleRight.Text = inLangManager.SetText("TxtCfgCabRight", label_VisibleRight.Text);
                label_VisibleBottom.Text = inLangManager.SetText("TxtCfgCabBottom", label_VisibleBottom.Text);

                gBox_mfx.Text = inLangManager.SetText("TxtCfgMfxAutoRecognization", gBox_mfx.Text);
                cBox_MfxAutoRegister.Text = inLangManager.SetText("TxtCfgMfxRegister", cBox_MfxAutoRegister.Text);
                cBox_MfxAutoUpdate.Text = inLangManager.SetText("TxtCfgMfxUpdate", cBox_MfxAutoUpdate.Text);

                gBox_Language.Text = inLangManager.SetText("TxtCfgLanguageOption", gBox_Language.Text);
                label_LanguageDescription.Text = inLangManager.SetText("TxtCfgLanguageDescription", label_LanguageDescription.Text);

                gBox_DCC.Text = inLangManager.SetText("TxtCfgDCCOption", gBox_DCC.Text);
                cBox_DCC.Text = inLangManager.SetText("TxtCfgUseDCC", cBox_DCC.Text);

                gBox_MultiScreen.Text = inLangManager.SetText("TxtCfgMultipleScreen", gBox_MultiScreen.Text);
                label_MSNum.Text = inLangManager.SetText("TxtCfgScreenNum", label_MSNum.Text);
                label_Screen1.Text = inLangManager.SetText("TxtCfgScreen1", label_Screen1.Text);
                label_Screen2.Text = inLangManager.SetText("TxtCfgScreen2", label_Screen2.Text);
                label_Screen3.Text = inLangManager.SetText("TxtCfgScreen3", label_Screen3.Text);
                label_Screen4.Text = inLangManager.SetText("TxtCfgScreen4", label_Screen4.Text);

                cBox_ScreenNums.Items[0] = inLangManager.SetText("TxtItemNone", cBox_ScreenNums.Items[0].ToString());

                cBox_SideFuncRight.Items[0] = inLangManager.SetText("TxtItemNone", cBox_SideFuncRight.Items[0].ToString());
                cBox_SideFuncRight.Items[1] = inLangManager.SetText("TxtItemClock", cBox_SideFuncRight.Items[1].ToString());
                cBox_SideFuncRight.Items[2] = inLangManager.SetText("TxtItemEMGbutton", cBox_SideFuncRight.Items[2].ToString());

                cBox_SideFuncBottom.Items[0] = inLangManager.SetText("TxtItemNone", cBox_SideFuncBottom.Items[0].ToString());
                cBox_SideFuncBottom.Items[1] = inLangManager.SetText("TxtItemAccessories", cBox_SideFuncBottom.Items[1].ToString());
                cBox_SideFuncBottom.Items[2] = inLangManager.SetText("TxtItemS88sensors", cBox_SideFuncBottom.Items[2].ToString());

                cBox_Screen1Panel.Items[0] = inLangManager.SetText("TxtItemNone", cBox_Screen1Panel.Items[0].ToString());
                cBox_Screen1Panel.Items[1] = inLangManager.SetText("TxtItemLocomotive", cBox_Screen1Panel.Items[1].ToString());
                cBox_Screen1Panel.Items[2] = inLangManager.SetText("TxtItemMultipleLocomotives", cBox_Screen1Panel.Items[2].ToString());
                cBox_Screen1Panel.Items[3] = inLangManager.SetText("TxtItem6021keypad", cBox_Screen1Panel.Items[3].ToString());
                cBox_Screen1Panel.Items[4] = inLangManager.SetText("TxtItemAccessories", cBox_Screen1Panel.Items[4].ToString());
                cBox_Screen1Panel.Items[5] = inLangManager.SetText("TxtItemTracklayout", cBox_Screen1Panel.Items[5].ToString());
                cBox_Screen1Panel.Items[6] = inLangManager.SetText("TxtItemLearning", cBox_Screen1Panel.Items[6].ToString());
                cBox_Screen1Panel.Items[7] = inLangManager.SetText("TxtItemEvent", cBox_Screen1Panel.Items[7].ToString());
                cBox_Screen1Panel.Items[8] = inLangManager.SetText("TxtItemConsole", cBox_Screen1Panel.Items[8].ToString());

                cBox_Screen2Panel.Items[0] = inLangManager.SetText("TxtItemNone", cBox_Screen2Panel.Items[0].ToString());
                cBox_Screen2Panel.Items[1] = inLangManager.SetText("TxtItemLocomotive", cBox_Screen2Panel.Items[1].ToString());
                cBox_Screen2Panel.Items[2] = inLangManager.SetText("TxtItemMultipleLocomotives", cBox_Screen2Panel.Items[2].ToString());
                cBox_Screen2Panel.Items[3] = inLangManager.SetText("TxtItem6021keypad", cBox_Screen2Panel.Items[3].ToString());
                cBox_Screen2Panel.Items[4] = inLangManager.SetText("TxtItemAccessories", cBox_Screen2Panel.Items[4].ToString());
                cBox_Screen2Panel.Items[5] = inLangManager.SetText("TxtItemTracklayout", cBox_Screen2Panel.Items[5].ToString());
                cBox_Screen2Panel.Items[6] = inLangManager.SetText("TxtItemLearning", cBox_Screen2Panel.Items[6].ToString());
                cBox_Screen2Panel.Items[7] = inLangManager.SetText("TxtItemEvent", cBox_Screen2Panel.Items[7].ToString());
                cBox_Screen2Panel.Items[8] = inLangManager.SetText("TxtItemConsole", cBox_Screen2Panel.Items[8].ToString());

                cBox_Screen3Panel.Items[0] = inLangManager.SetText("TxtItemNone", cBox_Screen3Panel.Items[0].ToString());
                cBox_Screen3Panel.Items[1] = inLangManager.SetText("TxtItemLocomotive", cBox_Screen3Panel.Items[1].ToString());
                cBox_Screen3Panel.Items[2] = inLangManager.SetText("TxtItemMultipleLocomotives", cBox_Screen3Panel.Items[2].ToString());
                cBox_Screen3Panel.Items[3] = inLangManager.SetText("TxtItem6021keypad", cBox_Screen3Panel.Items[3].ToString());
                cBox_Screen3Panel.Items[4] = inLangManager.SetText("TxtItemAccessories", cBox_Screen3Panel.Items[4].ToString());
                cBox_Screen3Panel.Items[5] = inLangManager.SetText("TxtItemTracklayout", cBox_Screen3Panel.Items[5].ToString());
                cBox_Screen3Panel.Items[6] = inLangManager.SetText("TxtItemLearning", cBox_Screen3Panel.Items[6].ToString());
                cBox_Screen3Panel.Items[7] = inLangManager.SetText("TxtItemEvent", cBox_Screen3Panel.Items[7].ToString());
                cBox_Screen3Panel.Items[8] = inLangManager.SetText("TxtItemConsole", cBox_Screen3Panel.Items[8].ToString());

                cBox_Screen4Panel.Items[0] = inLangManager.SetText("TxtItemNone", cBox_Screen4Panel.Items[0].ToString());
                cBox_Screen4Panel.Items[1] = inLangManager.SetText("TxtItemLocomotive", cBox_Screen4Panel.Items[1].ToString());
                cBox_Screen4Panel.Items[2] = inLangManager.SetText("TxtItemMultipleLocomotives", cBox_Screen4Panel.Items[2].ToString());
                cBox_Screen4Panel.Items[3] = inLangManager.SetText("TxtItem6021keypad", cBox_Screen4Panel.Items[3].ToString());
                cBox_Screen4Panel.Items[4] = inLangManager.SetText("TxtItemAccessories", cBox_Screen4Panel.Items[4].ToString());
                cBox_Screen4Panel.Items[5] = inLangManager.SetText("TxtItemTracklayout", cBox_Screen4Panel.Items[5].ToString());
                cBox_Screen4Panel.Items[6] = inLangManager.SetText("TxtItemLearning", cBox_Screen4Panel.Items[6].ToString());
                cBox_Screen4Panel.Items[7] = inLangManager.SetText("TxtItemEvent", cBox_Screen4Panel.Items[7].ToString());
                cBox_Screen4Panel.Items[8] = inLangManager.SetText("TxtItemConsole", cBox_Screen4Panel.Items[8].ToString());


                button_Ok.Text = inLangManager.SetText("TxtOk", button_Ok.Text);
                button_Cancel.Text = inLangManager.SetText("TxtCancel", button_Cancel.Text);

            }


        }

        private void label_PortNo_TextChanged(object sender, EventArgs e)
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

        private void cBox_ConnectionType_SelectedIndexChanged(object sender, EventArgs e)
        {

            switch (cBox_ConnectionType.SelectedIndex)
            {

                case 1:
                    gBox_Http.Enabled = true & NonVisibleOption;
                    gBox_Serial.Enabled = false;
                    break;
                case 0:
                    gBox_Http.Enabled = false;
                    gBox_Serial.Enabled = true & NonVisibleOption;
                    break;
                case 2:
                default:
                    gBox_Http.Enabled = false;
                    gBox_Serial.Enabled = false;
                    break;
            }

        }


    }
}

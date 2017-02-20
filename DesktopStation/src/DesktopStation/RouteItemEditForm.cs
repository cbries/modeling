using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DesktopStation
{
    public partial class RouteItemEditForm : Form
    {
        private Language LangManager;


        public RouteItemEditForm(Language inLangManager)
        {
            LangManager = inLangManager;


            InitializeComponent();
        }

        private void comboBox_Type_SelectedIndexChanged(object sender, EventArgs e)
        {

            switch (comboBox_Type.SelectedIndex)
            {
                    
                case 0:
                    label_Addr.Text = LangManager.SetText("TxtLayoutAccAddr", "Address");
                    label_Dir.Text = LangManager.SetText("TxtRouteDir", "Direction");
                    cBox_SigDir.Items[0] = LangManager.SetText("TxtRouteRed", "Red, Div/, c");
                    cBox_SigDir.Items[1] = LangManager.SetText("TxtRouteGreen", "Green, Str|, t");
                    break;
                case 1:
                    label_Addr.Text = LangManager.SetText("TxtLayoutS88Addr", "S88 Address");
                    label_Dir.Text = LangManager.SetText("TxtRouteStatus", "Status");
                    cBox_SigDir.Items[0] = "OFF";
                    cBox_SigDir.Items[1] = "ON";
                    break;
                case 2:
                    label_Addr.Text = LangManager.SetText("TxtScrEditFlagNo", "Flag No");
                    label_Dir.Text = LangManager.SetText("TxtScrEditValue", "Value");
                    cBox_SigDir.Items[0] = "= 0";
                    cBox_SigDir.Items[1] = "> 0";
                    break;
            }
            

        }

        public void SetFormLanguage(Language inLangManager)
        {
            if (inLangManager.Loaded() == true)
            {

                label_LogicalOpe.Text = inLangManager.SetText("TxtRouteLogicalOpe", label_LogicalOpe.Text);
                label_Type.Text = inLangManager.SetText("TxtRouteType", label_Type.Text);
                this.Text = inLangManager.SetText("TxtRouteItemEdit", this.Text);

                button_Ok.Text = inLangManager.SetText("TxtOk", button_Ok.Text);
                button_Cancel.Text = inLangManager.SetText("TxtCancel", button_Cancel.Text);

            }

        }


    }
}

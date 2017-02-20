using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DesktopStation
{
    public partial class NumInputForm : Form
    {

        private RouteList RoutesData;
        private Language LangManager;

        public NumInputForm(RouteList inList)
        {
            InitializeComponent();

            RoutesData = inList;
        }

        public void SetFormLanguage(Language inLangManager)
        {
            if (inLangManager.Loaded() == true)
            {
                this.Text = inLangManager.SetText("TxtLayoutTitle", this.Text);

                label_AccAddr.Text = inLangManager.SetText("TxtLayoutAccAddr", label_AccAddr.Text);
                label_S88Addr.Text = inLangManager.SetText("TxtLayoutS88Addr", label_S88Addr.Text);
                label_Description.Text = inLangManager.SetText("TxtLayoutMean", label_Description.Text);

                gBox_addr.Text = inLangManager.SetText("TxtLayoutAddrAssign", gBox_addr.Text);

                tabDeco.Text = inLangManager.SetText("TxtDecoration", tabDeco.Text);
                cBox_UseDeco.Text = inLangManager.SetText("TxtUseDecoration", cBox_UseDeco.Text);
                labelDecoX.Text = inLangManager.SetText("TxtDecoPosX", labelDecoX.Text);
                labelDecoY.Text = inLangManager.SetText("TxtDecoPosY", labelDecoY.Text);
                labelDecoText.Text = inLangManager.SetText("TxtDecoText", labelDecoText.Text);

                tabRoute.Text = inLangManager.SetText("TxtRoute", tabRoute.Text);
                labelName.Text = inLangManager.SetText("TxtRouteName", labelName.Text);
                label_SelRoute.Text = inLangManager.SetText("TxtRouteSelected", label_SelRoute.Text);
                gBox_Detail.Text = inLangManager.SetText("TxtRouteDetail", gBox_Detail.Text);
                labelSigAddr.Text = inLangManager.SetText("TxtRouteSignalAddr", labelSigAddr.Text);
                labelSigDir.Text = inLangManager.SetText("TxtRouteSignalDir", labelSigDir.Text);
                cBox_IgnoreOpenState.Text = inLangManager.SetText("TxtRouteIgnoreState", cBox_IgnoreOpenState.Text);

                button1.Text = inLangManager.SetText("TxtMenuAdd", button1.Text);
                buttonAddRouteItem.Text = inLangManager.SetText("TxtMenuAdd", buttonAddRouteItem.Text);
                buttonRouteDelete.Text = inLangManager.SetText("TxtMenuDelete", buttonRouteDelete.Text);
                buttonDelRouteItem.Text = inLangManager.SetText("TxtMenuDelete", buttonDelRouteItem.Text);
                buttonEditRouteItem.Text = inLangManager.SetText("TxtMenuEdit", buttonEditRouteItem.Text);

                cBox_SigDir.Items[0] = inLangManager.SetText("TxtRouteRed", (String)cBox_SigDir.Items[0]);
                cBox_SigDir.Items[1] = inLangManager.SetText("TxtRouteGreen", (String)cBox_SigDir.Items[1]);


                button_Ok.Text = inLangManager.SetText("TxtOk", button_Ok.Text);
                button_Cancel.Text = inLangManager.SetText("TxtCancel", button_Cancel.Text);

                LangManager = inLangManager;

            }
            else
            {
                LangManager = null;
            }

        }


        public void AddRoutes()
        {

            comboBox_Routes.Items.Clear();
            comboBox_Routes.Items.Add("Not selected");

            if (RoutesData.ListItems.Count == 0)
            {
               comboBox_Routes.SelectedIndex = 0;
            }
            else
            {

                for (int i = 0; i < RoutesData.ListItems.Count; i++)
                {
                    comboBox_Routes.Items.Add(i.ToString() + ": " + RoutesData.ListItems[i].NameText);
                }

            }

        }

        private void textBox_RouteName_TextChanged(object sender, EventArgs e)
        {
            if (comboBox_Routes.SelectedIndex <= 0)
            {
                return;
            }

            if (RoutesData.ListItems.Count == 0)
            {
                return;
            }

            RoutesData.ListItems[comboBox_Routes.SelectedIndex - 1].NameText = textBox_RouteName.Text;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (comboBox_Routes.SelectedIndex <= 0)
            {
                return;
            }

            if (RoutesData.ListItems.Count == 0)
            {
                return;
            }

            RoutesData.ListItems[comboBox_Routes.SelectedIndex - 1].SignalAddr = Decimal.ToInt32(numUpDown_SigAddr.Value);

        }

        private void checkBox_SigDir_CheckedChanged(object sender, EventArgs e)
        {
            if (comboBox_Routes.SelectedIndex <= 0)
            {
                return;
            }

            if (RoutesData.ListItems.Count == 0)
            {
                return;
            }

            RoutesData.ListItems[comboBox_Routes.SelectedIndex - 1].SignalDir = cBox_SigDir.SelectedIndex;

        }

        private void comboBox_Routes_SelectedIndexChanged(object sender, EventArgs e)
        {
            //選択が変わったら表示内容を変える

            if ((comboBox_Routes.SelectedIndex <= 0) || (RoutesData.ListItems.Count == 0))
            {
                /* 表示を選択できないようにする */
                AvailabilityRouteEditor(false);

                return;
            }
            else
            {
                AvailabilityRouteEditor(true);
            }


            cBox_SigDir.SelectedIndex = RoutesData.ListItems[comboBox_Routes.SelectedIndex - 1].SignalDir;
            numUpDown_SigAddr.Value = RoutesData.ListItems[comboBox_Routes.SelectedIndex - 1].SignalAddr;
            textBox_RouteName.Text = RoutesData.ListItems[comboBox_Routes.SelectedIndex - 1].NameText;
            cBox_IgnoreOpenState.Checked = RoutesData.ListItems[comboBox_Routes.SelectedIndex - 1].IgnoreOpenStatus;

            //Routeアイテムを更新する
            UpdateRouteItems(RoutesData.ListItems[comboBox_Routes.SelectedIndex - 1]);



        }

        private void AvailabilityRouteEditor(Boolean inAvailability)
        {
            buttonAddRouteItem.Enabled = inAvailability;
            buttonEditRouteItem.Enabled = inAvailability;
            buttonDelRouteItem.Enabled = inAvailability;
            textBox_RouteName.Enabled = inAvailability;

            numUpDown_SigAddr.Enabled = inAvailability;
            cBox_SigDir.Enabled = inAvailability;
            cBox_IgnoreOpenState.Enabled = inAvailability;


        }


        private String getLogicalOpe(int inOpeNo)
        {
            String aRet = "";

            switch (inOpeNo)
            {
                case 0:
                default:
                    aRet = "NOP";
                    break;

                case 1:
                    aRet = "AND";
                    break;

                case 2:
                    aRet = "OR";
                    break;

            }

            return aRet;
        }

        public void UpdateRouteItems(RouteItems inItems)
        {

            listView_RouteAccs.Items.Clear();

            for (int i = 0; i < inItems.Items.Count; i++)
            {
                ListViewItem aItem = new ListViewItem(inItems.Items[i].No.ToString());

                switch (inItems.Items[i].Type)
                {
                    case 0:
                        aItem.SubItems.Add(getLogicalOpe(inItems.Items[i].Logical));
                        aItem.SubItems.Add("Acc");
                        aItem.SubItems.Add(inItems.Items[i].Addr.ToString());
                        aItem.SubItems.Add(inItems.Items[i].Direction == 0 ? "/,Div" : "|,Str");
                        break;

                    case 1:
                        aItem.SubItems.Add(getLogicalOpe(inItems.Items[i].Logical));
                        aItem.SubItems.Add("S88");
                        aItem.SubItems.Add(inItems.Items[i].Addr.ToString());
                        aItem.SubItems.Add(inItems.Items[i].Direction == 0 ? "OFF" : "ON");
                        break;

                    case 2:
                        aItem.SubItems.Add(getLogicalOpe(inItems.Items[i].Logical));
                        aItem.SubItems.Add("Flag");
                        aItem.SubItems.Add(inItems.Items[i].Addr.ToString());
                        aItem.SubItems.Add(inItems.Items[i].Direction == 0 ? "=0" : ">0");
                        break;
                }



                listView_RouteAccs.Items.Add(aItem);
            }

        }

        public void AddRouteItem(int inNo, int inType, int inAddr, int inDir)
        {
                RouteItem aItem = new RouteItem();
                aItem.No = inNo;




        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Add Route

            int aNewIndex = RoutesData.ListItems.Count;

            comboBox_Routes.Items.Add(aNewIndex.ToString() + ": New Route");

            RouteItems aItem = new RouteItems();
            aItem.NameText = "";
            aItem.SignalAddr = 0;
            aItem.SignalDir = 0;
            aItem.Items.Clear();
            aItem.ID =  RoutesData.GenerateID();


            RoutesData.ListItems.Add(aItem);

            comboBox_Routes.SelectedIndex = aNewIndex;
        }

        private void buttonAddRouteItem_Click(object sender, EventArgs e)
        {

            RouteItemEditForm aForm = new RouteItemEditForm(LangManager);

            aForm.SetFormLanguage(LangManager);

            aForm.ShowDialog(this);

            //押されたボタン別の処理 
            if (aForm.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                RouteItem aRItem = new RouteItem();
                aRItem.No = RoutesData.ListItems[comboBox_Routes.SelectedIndex - 1].Items.Count;
                aRItem.Type = aForm.comboBox_Type.SelectedIndex;
                aRItem.Addr = Decimal.ToInt32(aForm.numUpDown_Addr.Value);
                aRItem.Direction = aForm.cBox_SigDir.SelectedIndex;
                aRItem.Logical = aForm.cBox_LogicOpe.SelectedIndex;

                RoutesData.ListItems[comboBox_Routes.SelectedIndex - 1].Items.Add(aRItem);

                //Routeアイテムを更新する
                UpdateRouteItems(RoutesData.ListItems[comboBox_Routes.SelectedIndex - 1]);

            }

            //フォームの解放 
            aForm.Dispose();



        }

        private void buttonEditRouteItem_Click(object sender, EventArgs e)
        {

            RouteItemEditForm aForm = new RouteItemEditForm(LangManager);
            aForm.SetFormLanguage(LangManager);


            int aIndex;           
            

            if( listView_RouteAccs.SelectedItems.Count > 0)
            {
                aIndex = listView_RouteAccs.SelectedItems[0].Index;

            }
            else
            {
                return;
            }

            RouteItem aRItem = RoutesData.ListItems[comboBox_Routes.SelectedIndex - 1].Items[aIndex];
            aForm.comboBox_Type.SelectedIndex = aRItem.Type;
            aForm.numUpDown_Addr.Value = aRItem.Addr;
            aForm.cBox_SigDir.SelectedIndex = aRItem.Direction;
            aForm.cBox_LogicOpe.SelectedIndex = aRItem.Logical;

            aForm.ShowDialog(this);

            //押されたボタン別の処理 
            if (aForm.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                aRItem.Type = aForm.comboBox_Type.SelectedIndex;
                aRItem.Addr = Decimal.ToInt32(aForm.numUpDown_Addr.Value);
                aRItem.Direction = aForm.cBox_SigDir.SelectedIndex;
                aRItem.Logical = aForm.cBox_LogicOpe.SelectedIndex;


                //Routeアイテムを更新する
                UpdateRouteItems(RoutesData.ListItems[comboBox_Routes.SelectedIndex - 1]);

            }

            //フォームの解放 
            aForm.Dispose();
        }

        private void buttonDelRouteItem_Click_1(object sender, EventArgs e)
        {
            int aIndex;


            if (listView_RouteAccs.SelectedItems.Count > 0)
            {
                aIndex = listView_RouteAccs.SelectedItems[0].Index;
                RoutesData.ListItems[comboBox_Routes.SelectedIndex - 1].Items.RemoveAt(aIndex);

                //Routeアイテムを更新する
                UpdateRouteItems(RoutesData.ListItems[comboBox_Routes.SelectedIndex - 1]);

            }
            else
            {
                return;
            }

        }

        private void listView_RouteAccs_DoubleClick(object sender, EventArgs e)
        {

        }

        private void cBox_SigDir_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_Routes.SelectedIndex < 0)
            {
                return;
            }

            if (RoutesData.ListItems.Count == 0)
            {
                return;
            }

            RoutesData.ListItems[comboBox_Routes.SelectedIndex - 1].SignalDir = cBox_SigDir.SelectedIndex;
        }

        private void cBox_IgnoreOpenState_CheckedChanged(object sender, EventArgs e)
        {
            if (comboBox_Routes.SelectedIndex < 0)
            {
                return;
            }

            if (RoutesData.ListItems.Count == 0)
            {
                return;
            }

            RoutesData.ListItems[comboBox_Routes.SelectedIndex - 1].IgnoreOpenStatus = cBox_IgnoreOpenState.Checked;
        }

        private void button_Up_Click(object sender, EventArgs e)
        {
            int aTag;
            int i;
            int aIndex;

            if (listView_RouteAccs.SelectedItems.Count <= 0)
            {
                return;
            }

            if (comboBox_Routes.SelectedIndex < 0)
            {
                return;
            }

            if (RoutesData.ListItems.Count == 0)
            {
                return;
            }

            Button aButton = sender as Button;

            aTag = int.Parse(aButton.Tag.ToString());


            switch (aTag)
            {
                case 1:
                    for (i = 0; i < listView_RouteAccs.SelectedItems.Count; i++)
                    {
                        aIndex = listView_RouteAccs.SelectedItems[i].Index;

                        if (aIndex > 0)
                        {
                            /* 表示側 */
                            ListViewItem aItem = listView_RouteAccs.Items[aIndex].Clone() as ListViewItem;

                            listView_RouteAccs.Items.RemoveAt(aIndex);
                            aItem.Selected = true;
                            listView_RouteAccs.Items.Insert(aIndex - 1, aItem);

                            listView_RouteAccs.Items[aIndex - 1].Text = (aIndex - 1).ToString();
                            listView_RouteAccs.Items[aIndex].Text = aIndex.ToString();

                            /* データ側 */
                            RouteItem aData = new RouteItem();

                            RoutesData.ListItems[comboBox_Routes.SelectedIndex - 1].CopyItemTo(aIndex, ref aData);

                            RoutesData.ListItems[comboBox_Routes.SelectedIndex - 1].Items.RemoveAt(aIndex);
                            RoutesData.ListItems[comboBox_Routes.SelectedIndex - 1].Items.Insert(aIndex - 1, aData);

                        }
                    }

                    break;
                case 2:
                    for (i = 0; i < listView_RouteAccs.SelectedItems.Count; i++)
                    {
                        aIndex = listView_RouteAccs.SelectedItems[i].Index;

                        if (aIndex >= 0 && (aIndex < (listView_RouteAccs.Items.Count - 1)))
                        {
                            /* 表示側 */
                            ListViewItem aItem = listView_RouteAccs.Items[aIndex].Clone() as ListViewItem;

                            listView_RouteAccs.Items.RemoveAt(aIndex);
                            listView_RouteAccs.Items.Insert(aIndex + 1, aItem);

                            listView_RouteAccs.Items[aIndex].Text = aIndex.ToString();
                            listView_RouteAccs.Items[aIndex + 1].Text = (aIndex + 1).ToString();
                            listView_RouteAccs.Items[aIndex + 1].Selected = true;


                            /* データ側 */
                            RouteItem aData = new RouteItem();

                            RoutesData.ListItems[comboBox_Routes.SelectedIndex - 1].CopyItemTo(aIndex, ref aData);

                            RoutesData.ListItems[comboBox_Routes.SelectedIndex - 1].Items.RemoveAt(aIndex);
                            RoutesData.ListItems[comboBox_Routes.SelectedIndex - 1].Items.Insert(aIndex + 1, aData);
                        }
                    }

                    break;
            }
        }

        private void buttonRouteDelete_Click(object sender, EventArgs e)
        {
            /* 選択したものを削除する */

            if (comboBox_Routes.SelectedIndex <= 0)
            {
                return;
            }

            if (RoutesData.ListItems.Count == 0)
            {
                return;
            }


            /* たずねる */
            if (MessageBox.Show(LangManager.SetText("TxtMsgBoxClearRoute", "Would you like to clear current route?"), LangManager.SetText("TxtMsgBoxAttention", "Attention"), MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {

                int aTempIndex = comboBox_Routes.SelectedIndex;
                comboBox_Routes.SelectedIndex = 0;

                //ルートを消す
                RoutesData.ListItems.RemoveAt(aTempIndex - 1);
                AddRoutes();

                //インデックスをひとつ上に移動する（消されるので）
                comboBox_Routes.SelectedIndex = aTempIndex - 1;


            }


        }





    }
}

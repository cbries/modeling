using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DesktopStation
{
    public partial class ExecuteCfgForm : Form
    {
        public ExecuteCfgForm()
        {
            InitializeComponent();
        }

        private void button_Del_Click(object sender, EventArgs e)
        {
            /* 削除してよいか聞く */

            if (listView_RunFile.SelectedItems.Count <= 0)
            {
                return;
            }


            int aDeleteItemCount = listView_RunFile.SelectedItems.Count;

            for (int i = aDeleteItemCount - 1; i >= 0; i--)
            {
                listView_RunFile.Items.Remove(listView_RunFile.SelectedItems[i]);
            }

        }

        private void listView_RunFile_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {

            /* 編集用にデータを引っ張ってくる */

            if (listView_RunFile.SelectedItems.Count <= 0)
            {
                return;
            }

            textBox_Itemname.Text = listView_RunFile.SelectedItems[0].SubItems[1].Text;
            textBox_ExeFile.Text = listView_RunFile.SelectedItems[0].SubItems[2].Text;
            textBox_ExeOption.Text = listView_RunFile.SelectedItems[0].SubItems[3].Text;

        }

        private void button_Edit_Click(object sender, EventArgs e)
        {

            if (listView_RunFile.SelectedItems.Count <= 0)
            {
                return;
            }

            listView_RunFile.SelectedItems[0].SubItems[1].Text = textBox_Itemname.Text;
            listView_RunFile.SelectedItems[0].SubItems[2].Text = textBox_ExeFile.Text;
            listView_RunFile.SelectedItems[0].SubItems[3].Text = textBox_ExeOption.Text;

        }

        private void button_Add_Click(object sender, EventArgs e)
        {


            ListViewItem aItem = new ListViewItem();

            aItem.Text = listView_RunFile.Items.Count.ToString();
            aItem.SubItems.Add(textBox_Itemname.Text);
            aItem.SubItems.Add( textBox_ExeFile.Text);
            aItem.SubItems.Add( textBox_ExeOption.Text);


            if (listView_RunFile.SelectedItems.Count > 0)
            {
                listView_RunFile.Items.Insert(listView_RunFile.SelectedItems[0].Index, aItem);
            }
            else
            {
                listView_RunFile.Items.Add(aItem);
            }
        }

        private void button_SelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog aForm = new OpenFileDialog();
            aForm.CheckFileExists = true;
            aForm.Filter = "Executable files(*.exe;*.bat)|*.*|All files(*.*)|*.*";

            //押されたボタン別の処理 
            if (aForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox_ExeFile.Text = aForm.FileName;
            }

            aForm.Dispose();
        }

        public void SetFormLanguage(Language inLangManager)
        {
            if (inLangManager.Loaded() == true)
            {
                this.Text = inLangManager.SetText("TxtAppEditor", this.Text);
                button_Add.Text = inLangManager.SetText("TxtMenuAdd", button_Add.Text);
                button_Del.Text = inLangManager.SetText("TxtMenuDelete", button_Del.Text);
                button_Update.Text = inLangManager.SetText("TxtUpdate", button_Update.Text);
                button_SelectFile.Text = inLangManager.SetText("TxtOpen", button_SelectFile.Text);

                label_AppName.Text = inLangManager.SetText("TxtAppName", label_AppName.Text);
                label_ExeFile.Text = inLangManager.SetText("TxtAppFile", label_ExeFile.Text);
                label_Option.Text = inLangManager.SetText("TxtAppParam", label_Option.Text);
                groupBox_AppEdit.Text = inLangManager.SetText("TxtAppDetail", groupBox_AppEdit.Text);

                listView_RunFile.Columns[1].Text = inLangManager.SetText("TxtAppName", listView_RunFile.Columns[1].Text);
                listView_RunFile.Columns[2].Text = inLangManager.SetText("TxtAppFile", listView_RunFile.Columns[2].Text);
                listView_RunFile.Columns[3].Text = inLangManager.SetText("TxtAppParam", listView_RunFile.Columns[3].Text);

                button_Ok.Text = inLangManager.SetText("TxtOk", button_Ok.Text);
                button_Cancel.Text = inLangManager.SetText("TxtCancel", button_Cancel.Text);

            }

        }
    }
}

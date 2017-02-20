namespace DesktopStation
{
    partial class ExecuteCfgForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.listView_RunFile = new System.Windows.Forms.ListView();
            this.colNo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAppName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAppFile = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colParam = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.button_Cancel = new System.Windows.Forms.Button();
            this.button_Ok = new System.Windows.Forms.Button();
            this.button_Add = new System.Windows.Forms.Button();
            this.button_Del = new System.Windows.Forms.Button();
            this.button_Update = new System.Windows.Forms.Button();
            this.label_AppName = new System.Windows.Forms.Label();
            this.textBox_Itemname = new System.Windows.Forms.TextBox();
            this.textBox_ExeFile = new System.Windows.Forms.TextBox();
            this.label_ExeFile = new System.Windows.Forms.Label();
            this.textBox_ExeOption = new System.Windows.Forms.TextBox();
            this.label_Option = new System.Windows.Forms.Label();
            this.groupBox_AppEdit = new System.Windows.Forms.GroupBox();
            this.button_SelectFile = new System.Windows.Forms.Button();
            this.groupBox_AppEdit.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView_RunFile
            // 
            this.listView_RunFile.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colNo,
            this.colAppName,
            this.colAppFile,
            this.colParam});
            this.listView_RunFile.FullRowSelect = true;
            this.listView_RunFile.GridLines = true;
            this.listView_RunFile.Location = new System.Drawing.Point(12, 12);
            this.listView_RunFile.MultiSelect = false;
            this.listView_RunFile.Name = "listView_RunFile";
            this.listView_RunFile.Size = new System.Drawing.Size(607, 142);
            this.listView_RunFile.TabIndex = 0;
            this.listView_RunFile.UseCompatibleStateImageBehavior = false;
            this.listView_RunFile.View = System.Windows.Forms.View.Details;
            this.listView_RunFile.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listView_RunFile_ItemSelectionChanged);
            // 
            // colNo
            // 
            this.colNo.Text = "No.";
            this.colNo.Width = 30;
            // 
            // colAppName
            // 
            this.colAppName.Text = "AppName";
            this.colAppName.Width = 100;
            // 
            // colAppFile
            // 
            this.colAppFile.Text = "App File";
            this.colAppFile.Width = 300;
            // 
            // colParam
            // 
            this.colParam.Text = "Parameters";
            this.colParam.Width = 120;
            // 
            // button_Cancel
            // 
            this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Cancel.Location = new System.Drawing.Point(532, 331);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(75, 27);
            this.button_Cancel.TabIndex = 17;
            this.button_Cancel.Text = "&Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            // 
            // button_Ok
            // 
            this.button_Ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_Ok.Location = new System.Drawing.Point(441, 331);
            this.button_Ok.Name = "button_Ok";
            this.button_Ok.Size = new System.Drawing.Size(75, 27);
            this.button_Ok.TabIndex = 16;
            this.button_Ok.Text = "&OK";
            this.button_Ok.UseVisualStyleBackColor = true;
            // 
            // button_Add
            // 
            this.button_Add.Location = new System.Drawing.Point(110, 160);
            this.button_Add.Name = "button_Add";
            this.button_Add.Size = new System.Drawing.Size(75, 23);
            this.button_Add.TabIndex = 18;
            this.button_Add.Text = "Add";
            this.button_Add.UseVisualStyleBackColor = true;
            this.button_Add.Click += new System.EventHandler(this.button_Add_Click);
            // 
            // button_Del
            // 
            this.button_Del.Location = new System.Drawing.Point(535, 160);
            this.button_Del.Name = "button_Del";
            this.button_Del.Size = new System.Drawing.Size(75, 23);
            this.button_Del.TabIndex = 19;
            this.button_Del.Text = "Delete";
            this.button_Del.UseVisualStyleBackColor = true;
            this.button_Del.Click += new System.EventHandler(this.button_Del_Click);
            // 
            // button_Update
            // 
            this.button_Update.Location = new System.Drawing.Point(15, 160);
            this.button_Update.Name = "button_Update";
            this.button_Update.Size = new System.Drawing.Size(75, 23);
            this.button_Update.TabIndex = 20;
            this.button_Update.Text = "Update";
            this.button_Update.UseVisualStyleBackColor = true;
            this.button_Update.Click += new System.EventHandler(this.button_Edit_Click);
            // 
            // label_AppName
            // 
            this.label_AppName.AutoSize = true;
            this.label_AppName.Location = new System.Drawing.Point(15, 28);
            this.label_AppName.Name = "label_AppName";
            this.label_AppName.Size = new System.Drawing.Size(58, 12);
            this.label_AppName.TabIndex = 21;
            this.label_AppName.Text = "App Name";
            // 
            // textBox_Itemname
            // 
            this.textBox_Itemname.Location = new System.Drawing.Point(107, 25);
            this.textBox_Itemname.Name = "textBox_Itemname";
            this.textBox_Itemname.Size = new System.Drawing.Size(394, 19);
            this.textBox_Itemname.TabIndex = 22;
            // 
            // textBox_ExeFile
            // 
            this.textBox_ExeFile.Location = new System.Drawing.Point(107, 50);
            this.textBox_ExeFile.Name = "textBox_ExeFile";
            this.textBox_ExeFile.Size = new System.Drawing.Size(394, 19);
            this.textBox_ExeFile.TabIndex = 24;
            // 
            // label_ExeFile
            // 
            this.label_ExeFile.AutoSize = true;
            this.label_ExeFile.Location = new System.Drawing.Point(15, 53);
            this.label_ExeFile.Name = "label_ExeFile";
            this.label_ExeFile.Size = new System.Drawing.Size(81, 12);
            this.label_ExeFile.TabIndex = 23;
            this.label_ExeFile.Text = "Executable file";
            // 
            // textBox_ExeOption
            // 
            this.textBox_ExeOption.Location = new System.Drawing.Point(107, 78);
            this.textBox_ExeOption.Name = "textBox_ExeOption";
            this.textBox_ExeOption.Size = new System.Drawing.Size(394, 19);
            this.textBox_ExeOption.TabIndex = 26;
            // 
            // label_Option
            // 
            this.label_Option.AutoSize = true;
            this.label_Option.Location = new System.Drawing.Point(15, 81);
            this.label_Option.Name = "label_Option";
            this.label_Option.Size = new System.Drawing.Size(38, 12);
            this.label_Option.TabIndex = 25;
            this.label_Option.Text = "Option";
            // 
            // groupBox_AppEdit
            // 
            this.groupBox_AppEdit.Controls.Add(this.button_SelectFile);
            this.groupBox_AppEdit.Controls.Add(this.label_AppName);
            this.groupBox_AppEdit.Controls.Add(this.textBox_ExeOption);
            this.groupBox_AppEdit.Controls.Add(this.textBox_Itemname);
            this.groupBox_AppEdit.Controls.Add(this.label_Option);
            this.groupBox_AppEdit.Controls.Add(this.label_ExeFile);
            this.groupBox_AppEdit.Controls.Add(this.textBox_ExeFile);
            this.groupBox_AppEdit.Location = new System.Drawing.Point(15, 203);
            this.groupBox_AppEdit.Name = "groupBox_AppEdit";
            this.groupBox_AppEdit.Size = new System.Drawing.Size(595, 113);
            this.groupBox_AppEdit.TabIndex = 27;
            this.groupBox_AppEdit.TabStop = false;
            this.groupBox_AppEdit.Text = "Application detail";
            // 
            // button_SelectFile
            // 
            this.button_SelectFile.Location = new System.Drawing.Point(517, 48);
            this.button_SelectFile.Name = "button_SelectFile";
            this.button_SelectFile.Size = new System.Drawing.Size(61, 23);
            this.button_SelectFile.TabIndex = 27;
            this.button_SelectFile.Text = "Open";
            this.button_SelectFile.UseVisualStyleBackColor = true;
            this.button_SelectFile.Click += new System.EventHandler(this.button_SelectFile_Click);
            // 
            // ExecuteCfgForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(631, 371);
            this.Controls.Add(this.groupBox_AppEdit);
            this.Controls.Add(this.button_Del);
            this.Controls.Add(this.button_Update);
            this.Controls.Add(this.button_Add);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.button_Ok);
            this.Controls.Add(this.listView_RunFile);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExecuteCfgForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Run File Properties";
            this.groupBox_AppEdit.ResumeLayout(false);
            this.groupBox_AppEdit.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ColumnHeader colNo;
        private System.Windows.Forms.ColumnHeader colAppName;
        private System.Windows.Forms.ColumnHeader colAppFile;
        private System.Windows.Forms.ColumnHeader colParam;
        public System.Windows.Forms.ListView listView_RunFile;
        public System.Windows.Forms.Button button_Cancel;
        public System.Windows.Forms.Button button_Ok;
        public System.Windows.Forms.Button button_Add;
        public System.Windows.Forms.Button button_Del;
        public System.Windows.Forms.Button button_Update;
        private System.Windows.Forms.Label label_AppName;
        private System.Windows.Forms.Label label_ExeFile;
        private System.Windows.Forms.Label label_Option;
        private System.Windows.Forms.GroupBox groupBox_AppEdit;
        public System.Windows.Forms.Button button_SelectFile;
        public System.Windows.Forms.TextBox textBox_Itemname;
        public System.Windows.Forms.TextBox textBox_ExeFile;
        public System.Windows.Forms.TextBox textBox_ExeOption;
    }
}
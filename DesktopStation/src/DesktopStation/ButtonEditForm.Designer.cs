namespace DesktopStation
{
    partial class ButtonEditForm
    {
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.cBox_FunctionImage = new System.Windows.Forms.ComboBox();
            this.cBox_FunctionSwitch = new System.Windows.Forms.CheckBox();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.button_Ok = new System.Windows.Forms.Button();
            this.FunctionImageList = new System.Windows.Forms.ImageList(this.components);
            this.gBox_FuncImage = new System.Windows.Forms.GroupBox();
            this.gBox_AssignRunFile = new System.Windows.Forms.GroupBox();
            this.buttonOpen = new System.Windows.Forms.Button();
            this.label_Filename = new System.Windows.Forms.Label();
            this.cBox_FileName = new System.Windows.Forms.ComboBox();
            this.label_AppName = new System.Windows.Forms.Label();
            this.cBox_AppName = new System.Windows.Forms.ComboBox();
            this.gBox_AssignLoc = new System.Windows.Forms.GroupBox();
            this.cBox_FunctionNo = new System.Windows.Forms.ComboBox();
            this.label_FunctionNo = new System.Windows.Forms.Label();
            this.label_LocProtcol = new System.Windows.Forms.Label();
            this.cBox_ProtcolLoc = new System.Windows.Forms.ComboBox();
            this.tBox_Addr = new System.Windows.Forms.TextBox();
            this.label_LocAddr = new System.Windows.Forms.Label();
            this.cBox_Assignment = new System.Windows.Forms.ComboBox();
            this.label_Assignment = new System.Windows.Forms.Label();
            this.gBox_FuncImage.SuspendLayout();
            this.gBox_AssignRunFile.SuspendLayout();
            this.gBox_AssignLoc.SuspendLayout();
            this.SuspendLayout();
            // 
            // cBox_FunctionImage
            // 
            this.cBox_FunctionImage.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cBox_FunctionImage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBox_FunctionImage.Font = new System.Drawing.Font("Arial", 15F);
            this.cBox_FunctionImage.ItemHeight = 28;
            this.cBox_FunctionImage.Items.AddRange(new object[] {
            "None",
            "Speaker",
            "Lamp",
            "Tail Lamp",
            "Slow",
            "5",
            "6",
            "7",
            "8",
            "9"});
            this.cBox_FunctionImage.Location = new System.Drawing.Point(16, 20);
            this.cBox_FunctionImage.MaxDropDownItems = 10;
            this.cBox_FunctionImage.Name = "cBox_FunctionImage";
            this.cBox_FunctionImage.Size = new System.Drawing.Size(93, 34);
            this.cBox_FunctionImage.TabIndex = 2;
            this.cBox_FunctionImage.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cBox_FunctionImage_DrawItem);
            // 
            // cBox_FunctionSwitch
            // 
            this.cBox_FunctionSwitch.AutoSize = true;
            this.cBox_FunctionSwitch.Location = new System.Drawing.Point(28, 22);
            this.cBox_FunctionSwitch.Name = "cBox_FunctionSwitch";
            this.cBox_FunctionSwitch.Size = new System.Drawing.Size(111, 16);
            this.cBox_FunctionSwitch.TabIndex = 1;
            this.cBox_FunctionSwitch.Text = "Momentary mode";
            this.cBox_FunctionSwitch.UseVisualStyleBackColor = true;
            // 
            // button_Cancel
            // 
            this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Cancel.Location = new System.Drawing.Point(421, 216);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(75, 35);
            this.button_Cancel.TabIndex = 12;
            this.button_Cancel.Text = "&Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            // 
            // button_Ok
            // 
            this.button_Ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_Ok.Location = new System.Drawing.Point(328, 216);
            this.button_Ok.Name = "button_Ok";
            this.button_Ok.Size = new System.Drawing.Size(75, 35);
            this.button_Ok.TabIndex = 11;
            this.button_Ok.Text = "&OK";
            this.button_Ok.UseVisualStyleBackColor = true;
            // 
            // FunctionImageList
            // 
            this.FunctionImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.FunctionImageList.ImageSize = new System.Drawing.Size(28, 28);
            this.FunctionImageList.TransparentColor = System.Drawing.Color.Lime;
            // 
            // gBox_FuncImage
            // 
            this.gBox_FuncImage.Controls.Add(this.cBox_FunctionImage);
            this.gBox_FuncImage.Location = new System.Drawing.Point(303, 12);
            this.gBox_FuncImage.Name = "gBox_FuncImage";
            this.gBox_FuncImage.Size = new System.Drawing.Size(127, 65);
            this.gBox_FuncImage.TabIndex = 14;
            this.gBox_FuncImage.TabStop = false;
            this.gBox_FuncImage.Text = "Function image";
            // 
            // gBox_AssignRunFile
            // 
            this.gBox_AssignRunFile.Controls.Add(this.buttonOpen);
            this.gBox_AssignRunFile.Controls.Add(this.label_Filename);
            this.gBox_AssignRunFile.Controls.Add(this.cBox_FileName);
            this.gBox_AssignRunFile.Controls.Add(this.label_AppName);
            this.gBox_AssignRunFile.Controls.Add(this.cBox_AppName);
            this.gBox_AssignRunFile.Location = new System.Drawing.Point(12, 93);
            this.gBox_AssignRunFile.Name = "gBox_AssignRunFile";
            this.gBox_AssignRunFile.Size = new System.Drawing.Size(276, 117);
            this.gBox_AssignRunFile.TabIndex = 15;
            this.gBox_AssignRunFile.TabStop = false;
            this.gBox_AssignRunFile.Text = "Assignment for functions";
            // 
            // buttonOpen
            // 
            this.buttonOpen.Location = new System.Drawing.Point(208, 61);
            this.buttonOpen.Name = "buttonOpen";
            this.buttonOpen.Size = new System.Drawing.Size(54, 23);
            this.buttonOpen.TabIndex = 18;
            this.buttonOpen.Text = "Open";
            this.buttonOpen.UseVisualStyleBackColor = true;
            this.buttonOpen.Click += new System.EventHandler(this.buttonOpen_Click);
            // 
            // label_Filename
            // 
            this.label_Filename.AutoSize = true;
            this.label_Filename.Location = new System.Drawing.Point(14, 72);
            this.label_Filename.Name = "label_Filename";
            this.label_Filename.Size = new System.Drawing.Size(130, 12);
            this.label_Filename.TabIndex = 17;
            this.label_Filename.Text = "File name or parameters";
            // 
            // cBox_FileName
            // 
            this.cBox_FileName.FormattingEnabled = true;
            this.cBox_FileName.Location = new System.Drawing.Point(16, 87);
            this.cBox_FileName.Name = "cBox_FileName";
            this.cBox_FileName.Size = new System.Drawing.Size(246, 20);
            this.cBox_FileName.TabIndex = 16;
            // 
            // label_AppName
            // 
            this.label_AppName.AutoSize = true;
            this.label_AppName.Location = new System.Drawing.Point(14, 27);
            this.label_AppName.Name = "label_AppName";
            this.label_AppName.Size = new System.Drawing.Size(56, 12);
            this.label_AppName.TabIndex = 15;
            this.label_AppName.Text = "App name";
            // 
            // cBox_AppName
            // 
            this.cBox_AppName.FormattingEnabled = true;
            this.cBox_AppName.Location = new System.Drawing.Point(16, 42);
            this.cBox_AppName.Name = "cBox_AppName";
            this.cBox_AppName.Size = new System.Drawing.Size(170, 20);
            this.cBox_AppName.TabIndex = 14;
            // 
            // gBox_AssignLoc
            // 
            this.gBox_AssignLoc.Controls.Add(this.cBox_FunctionNo);
            this.gBox_AssignLoc.Controls.Add(this.label_FunctionNo);
            this.gBox_AssignLoc.Controls.Add(this.label_LocProtcol);
            this.gBox_AssignLoc.Controls.Add(this.cBox_ProtcolLoc);
            this.gBox_AssignLoc.Controls.Add(this.tBox_Addr);
            this.gBox_AssignLoc.Controls.Add(this.label_LocAddr);
            this.gBox_AssignLoc.Location = new System.Drawing.Point(303, 93);
            this.gBox_AssignLoc.Name = "gBox_AssignLoc";
            this.gBox_AssignLoc.Size = new System.Drawing.Size(194, 117);
            this.gBox_AssignLoc.TabIndex = 36;
            this.gBox_AssignLoc.TabStop = false;
            this.gBox_AssignLoc.Text = "Assignment for another loc";
            // 
            // cBox_FunctionNo
            // 
            this.cBox_FunctionNo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBox_FunctionNo.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cBox_FunctionNo.FormattingEnabled = true;
            this.cBox_FunctionNo.Items.AddRange(new object[] {
            "F0",
            "F1",
            "F2",
            "F3",
            "F4",
            "F5",
            "F6",
            "F7",
            "F8",
            "F9",
            "F10",
            "F11",
            "F12",
            "F13",
            "F14",
            "F15",
            "F16",
            "F17",
            "F18",
            "F19",
            "F20",
            "F21",
            "F22",
            "F23",
            "F24",
            "F25",
            "F26",
            "F27",
            "F28"});
            this.cBox_FunctionNo.Location = new System.Drawing.Point(121, 82);
            this.cBox_FunctionNo.Name = "cBox_FunctionNo";
            this.cBox_FunctionNo.Size = new System.Drawing.Size(57, 23);
            this.cBox_FunctionNo.TabIndex = 45;
            // 
            // label_FunctionNo
            // 
            this.label_FunctionNo.Location = new System.Drawing.Point(9, 87);
            this.label_FunctionNo.Name = "label_FunctionNo";
            this.label_FunctionNo.Size = new System.Drawing.Size(106, 14);
            this.label_FunctionNo.TabIndex = 43;
            this.label_FunctionNo.Text = "Function No";
            // 
            // label_LocProtcol
            // 
            this.label_LocProtcol.Location = new System.Drawing.Point(9, 57);
            this.label_LocProtcol.Name = "label_LocProtcol";
            this.label_LocProtcol.Size = new System.Drawing.Size(106, 15);
            this.label_LocProtcol.TabIndex = 42;
            this.label_LocProtcol.Text = "Loc Protcol";
            // 
            // cBox_ProtcolLoc
            // 
            this.cBox_ProtcolLoc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBox_ProtcolLoc.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cBox_ProtcolLoc.FormattingEnabled = true;
            this.cBox_ProtcolLoc.Items.AddRange(new object[] {
            "MM2",
            "mfx",
            "DCC"});
            this.cBox_ProtcolLoc.Location = new System.Drawing.Point(121, 50);
            this.cBox_ProtcolLoc.Name = "cBox_ProtcolLoc";
            this.cBox_ProtcolLoc.Size = new System.Drawing.Size(57, 25);
            this.cBox_ProtcolLoc.TabIndex = 41;
            // 
            // tBox_Addr
            // 
            this.tBox_Addr.Location = new System.Drawing.Point(121, 23);
            this.tBox_Addr.Name = "tBox_Addr";
            this.tBox_Addr.Size = new System.Drawing.Size(57, 21);
            this.tBox_Addr.TabIndex = 17;
            this.tBox_Addr.Text = "0";
            this.tBox_Addr.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label_LocAddr
            // 
            this.label_LocAddr.Location = new System.Drawing.Point(9, 25);
            this.label_LocAddr.Name = "label_LocAddr";
            this.label_LocAddr.Size = new System.Drawing.Size(130, 12);
            this.label_LocAddr.TabIndex = 15;
            this.label_LocAddr.Text = "Loc Address(0-)";
            // 
            // cBox_Assignment
            // 
            this.cBox_Assignment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBox_Assignment.FormattingEnabled = true;
            this.cBox_Assignment.Items.AddRange(new object[] {
            "Default",
            "External App",
            "Another Loc"});
            this.cBox_Assignment.Location = new System.Drawing.Point(153, 57);
            this.cBox_Assignment.Name = "cBox_Assignment";
            this.cBox_Assignment.Size = new System.Drawing.Size(121, 20);
            this.cBox_Assignment.TabIndex = 37;
            this.cBox_Assignment.SelectedIndexChanged += new System.EventHandler(this.cBox_Assignment_SelectedIndexChanged);
            // 
            // label_Assignment
            // 
            this.label_Assignment.AutoSize = true;
            this.label_Assignment.Location = new System.Drawing.Point(26, 60);
            this.label_Assignment.Name = "label_Assignment";
            this.label_Assignment.Size = new System.Drawing.Size(104, 12);
            this.label_Assignment.TabIndex = 38;
            this.label_Assignment.Text = "Assignment funtion";
            // 
            // ButtonEditForm
            // 
            this.AcceptButton = this.button_Ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button_Cancel;
            this.ClientSize = new System.Drawing.Size(512, 262);
            this.Controls.Add(this.label_Assignment);
            this.Controls.Add(this.cBox_Assignment);
            this.Controls.Add(this.gBox_AssignLoc);
            this.Controls.Add(this.gBox_AssignRunFile);
            this.Controls.Add(this.cBox_FunctionSwitch);
            this.Controls.Add(this.gBox_FuncImage);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.button_Ok);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ButtonEditForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit button function";
            this.gBox_FuncImage.ResumeLayout(false);
            this.gBox_AssignRunFile.ResumeLayout(false);
            this.gBox_AssignRunFile.PerformLayout();
            this.gBox_AssignLoc.ResumeLayout(false);
            this.gBox_AssignLoc.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ComboBox cBox_FunctionImage;
        public System.Windows.Forms.CheckBox cBox_FunctionSwitch;
        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.Button button_Ok;
        public System.Windows.Forms.ImageList FunctionImageList;
        private System.Windows.Forms.GroupBox gBox_FuncImage;
        private System.Windows.Forms.GroupBox gBox_AssignRunFile;
        private System.Windows.Forms.Label label_AppName;
        private System.Windows.Forms.Button buttonOpen;
        private System.Windows.Forms.Label label_Filename;
        public System.Windows.Forms.ComboBox cBox_AppName;
        public System.Windows.Forms.ComboBox cBox_FileName;
        private System.Windows.Forms.GroupBox gBox_AssignLoc;
        private System.Windows.Forms.Label label_FunctionNo;
        private System.Windows.Forms.Label label_LocProtcol;
        public System.Windows.Forms.ComboBox cBox_ProtcolLoc;
        public System.Windows.Forms.TextBox tBox_Addr;
        private System.Windows.Forms.Label label_LocAddr;
        public System.Windows.Forms.ComboBox cBox_Assignment;
        private System.Windows.Forms.Label label_Assignment;
        public System.Windows.Forms.ComboBox cBox_FunctionNo;
    }
}
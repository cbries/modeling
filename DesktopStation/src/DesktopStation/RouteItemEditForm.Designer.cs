namespace DesktopStation
{
    partial class RouteItemEditForm
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
            this.button_Cancel = new System.Windows.Forms.Button();
            this.button_Ok = new System.Windows.Forms.Button();
            this.numUpDown_Addr = new System.Windows.Forms.NumericUpDown();
            this.label_Addr = new System.Windows.Forms.Label();
            this.label_Type = new System.Windows.Forms.Label();
            this.comboBox_Type = new System.Windows.Forms.ComboBox();
            this.label_Dir = new System.Windows.Forms.Label();
            this.cBox_SigDir = new System.Windows.Forms.ComboBox();
            this.label_LogicalOpe = new System.Windows.Forms.Label();
            this.cBox_LogicOpe = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown_Addr)).BeginInit();
            this.SuspendLayout();
            // 
            // button_Cancel
            // 
            this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Cancel.Location = new System.Drawing.Point(184, 193);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(75, 31);
            this.button_Cancel.TabIndex = 13;
            this.button_Cancel.Text = "&Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            // 
            // button_Ok
            // 
            this.button_Ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_Ok.Location = new System.Drawing.Point(95, 193);
            this.button_Ok.Name = "button_Ok";
            this.button_Ok.Size = new System.Drawing.Size(75, 31);
            this.button_Ok.TabIndex = 12;
            this.button_Ok.Text = "&OK";
            this.button_Ok.UseVisualStyleBackColor = true;
            // 
            // numUpDown_Addr
            // 
            this.numUpDown_Addr.Font = new System.Drawing.Font("Arial", 15F);
            this.numUpDown_Addr.Location = new System.Drawing.Point(110, 62);
            this.numUpDown_Addr.Maximum = new decimal(new int[] {
            2048,
            0,
            0,
            0});
            this.numUpDown_Addr.Name = "numUpDown_Addr";
            this.numUpDown_Addr.Size = new System.Drawing.Size(149, 30);
            this.numUpDown_Addr.TabIndex = 16;
            this.numUpDown_Addr.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label_Addr
            // 
            this.label_Addr.AutoSize = true;
            this.label_Addr.Location = new System.Drawing.Point(8, 73);
            this.label_Addr.Name = "label_Addr";
            this.label_Addr.Size = new System.Drawing.Size(47, 12);
            this.label_Addr.TabIndex = 17;
            this.label_Addr.Text = "Address";
            // 
            // label_Type
            // 
            this.label_Type.AutoSize = true;
            this.label_Type.Location = new System.Drawing.Point(8, 27);
            this.label_Type.Name = "label_Type";
            this.label_Type.Size = new System.Drawing.Size(30, 12);
            this.label_Type.TabIndex = 15;
            this.label_Type.Text = "Type";
            // 
            // comboBox_Type
            // 
            this.comboBox_Type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Type.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.comboBox_Type.FormattingEnabled = true;
            this.comboBox_Type.Items.AddRange(new object[] {
            "Accessory",
            "S88",
            "Flag"});
            this.comboBox_Type.Location = new System.Drawing.Point(110, 20);
            this.comboBox_Type.Name = "comboBox_Type";
            this.comboBox_Type.Size = new System.Drawing.Size(149, 24);
            this.comboBox_Type.TabIndex = 14;
            this.comboBox_Type.SelectedIndexChanged += new System.EventHandler(this.comboBox_Type_SelectedIndexChanged);
            // 
            // label_Dir
            // 
            this.label_Dir.AutoSize = true;
            this.label_Dir.Location = new System.Drawing.Point(8, 116);
            this.label_Dir.Name = "label_Dir";
            this.label_Dir.Size = new System.Drawing.Size(51, 12);
            this.label_Dir.TabIndex = 22;
            this.label_Dir.Text = "Direction";
            // 
            // cBox_SigDir
            // 
            this.cBox_SigDir.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBox_SigDir.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cBox_SigDir.FormattingEnabled = true;
            this.cBox_SigDir.Items.AddRange(new object[] {
            "Red, Diverse",
            "Green, Straight"});
            this.cBox_SigDir.Location = new System.Drawing.Point(110, 109);
            this.cBox_SigDir.Name = "cBox_SigDir";
            this.cBox_SigDir.Size = new System.Drawing.Size(149, 24);
            this.cBox_SigDir.TabIndex = 21;
            // 
            // label_LogicalOpe
            // 
            this.label_LogicalOpe.AutoSize = true;
            this.label_LogicalOpe.Location = new System.Drawing.Point(8, 158);
            this.label_LogicalOpe.Name = "label_LogicalOpe";
            this.label_LogicalOpe.Size = new System.Drawing.Size(65, 12);
            this.label_LogicalOpe.TabIndex = 24;
            this.label_LogicalOpe.Text = "Logical Ope";
            // 
            // cBox_LogicOpe
            // 
            this.cBox_LogicOpe.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBox_LogicOpe.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cBox_LogicOpe.FormattingEnabled = true;
            this.cBox_LogicOpe.Items.AddRange(new object[] {
            "No Operation",
            "AND",
            "OR"});
            this.cBox_LogicOpe.Location = new System.Drawing.Point(110, 151);
            this.cBox_LogicOpe.Name = "cBox_LogicOpe";
            this.cBox_LogicOpe.Size = new System.Drawing.Size(149, 24);
            this.cBox_LogicOpe.TabIndex = 23;
            // 
            // RouteItemEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(271, 236);
            this.Controls.Add(this.label_LogicalOpe);
            this.Controls.Add(this.cBox_LogicOpe);
            this.Controls.Add(this.label_Dir);
            this.Controls.Add(this.cBox_SigDir);
            this.Controls.Add(this.numUpDown_Addr);
            this.Controls.Add(this.label_Addr);
            this.Controls.Add(this.label_Type);
            this.Controls.Add(this.comboBox_Type);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.button_Ok);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RouteItemEditForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Route item editor";
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown_Addr)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.Button button_Ok;
        public System.Windows.Forms.NumericUpDown numUpDown_Addr;
        public System.Windows.Forms.ComboBox comboBox_Type;
        public System.Windows.Forms.ComboBox cBox_SigDir;
        public System.Windows.Forms.Label label_Addr;
        public System.Windows.Forms.Label label_Dir;
        public System.Windows.Forms.Label label_Type;
        public System.Windows.Forms.Label label_LogicalOpe;
        public System.Windows.Forms.ComboBox cBox_LogicOpe;
    }
}
namespace DesktopStation
{
    partial class SelectAccessoryForm
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
            this.button_Cancel = new System.Windows.Forms.Button();
            this.button_Ok = new System.Windows.Forms.Button();
            this.tBox_AccComment = new System.Windows.Forms.TextBox();
            this.label_Comment = new System.Windows.Forms.Label();
            this.label_AccList = new System.Windows.Forms.Label();
            this.cBox_AccType = new System.Windows.Forms.ComboBox();
            this.cBox_ReverseSignal = new System.Windows.Forms.CheckBox();
            this.cBox_Invisible = new System.Windows.Forms.CheckBox();
            this.cBox_ACCProtocol = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // button_Cancel
            // 
            this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Cancel.Location = new System.Drawing.Point(235, 158);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(75, 32);
            this.button_Cancel.TabIndex = 14;
            this.button_Cancel.Text = "&Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            // 
            // button_Ok
            // 
            this.button_Ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_Ok.Location = new System.Drawing.Point(149, 158);
            this.button_Ok.Name = "button_Ok";
            this.button_Ok.Size = new System.Drawing.Size(75, 32);
            this.button_Ok.TabIndex = 13;
            this.button_Ok.Text = "&OK";
            this.button_Ok.UseVisualStyleBackColor = true;
            // 
            // tBox_AccComment
            // 
            this.tBox_AccComment.Location = new System.Drawing.Point(12, 116);
            this.tBox_AccComment.Name = "tBox_AccComment";
            this.tBox_AccComment.Size = new System.Drawing.Size(298, 21);
            this.tBox_AccComment.TabIndex = 19;
            // 
            // label_Comment
            // 
            this.label_Comment.AutoSize = true;
            this.label_Comment.Location = new System.Drawing.Point(10, 101);
            this.label_Comment.Name = "label_Comment";
            this.label_Comment.Size = new System.Drawing.Size(59, 12);
            this.label_Comment.TabIndex = 18;
            this.label_Comment.Text = "Comments";
            // 
            // label_AccList
            // 
            this.label_AccList.AutoSize = true;
            this.label_AccList.Location = new System.Drawing.Point(10, 13);
            this.label_AccList.Name = "label_AccList";
            this.label_AccList.Size = new System.Drawing.Size(79, 12);
            this.label_AccList.TabIndex = 17;
            this.label_AccList.Text = "Accessory list";
            // 
            // cBox_AccType
            // 
            this.cBox_AccType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cBox_AccType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBox_AccType.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cBox_AccType.FormattingEnabled = true;
            this.cBox_AccType.Items.AddRange(new object[] {
            "Signal",
            "Turnout left",
            "Turnout right",
            "Double slip switch",
            "Three way turnout",
            "Yard signal",
            "Distant signal(76383)",
            "Home signal(76391)",
            "Home signal(76393)",
            "Home signal(76394)"});
            this.cBox_AccType.Location = new System.Drawing.Point(12, 28);
            this.cBox_AccType.Name = "cBox_AccType";
            this.cBox_AccType.Size = new System.Drawing.Size(298, 36);
            this.cBox_AccType.TabIndex = 16;
            this.cBox_AccType.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cBox_AccType_DrawItem);
            this.cBox_AccType.SelectedValueChanged += new System.EventHandler(this.cBox_AccType_SelectedValueChanged);
            // 
            // cBox_ReverseSignal
            // 
            this.cBox_ReverseSignal.AutoSize = true;
            this.cBox_ReverseSignal.Location = new System.Drawing.Point(12, 73);
            this.cBox_ReverseSignal.Name = "cBox_ReverseSignal";
            this.cBox_ReverseSignal.Size = new System.Drawing.Size(100, 16);
            this.cBox_ReverseSignal.TabIndex = 20;
            this.cBox_ReverseSignal.Text = "Reverse signal";
            this.cBox_ReverseSignal.UseVisualStyleBackColor = true;
            // 
            // cBox_Invisible
            // 
            this.cBox_Invisible.AutoSize = true;
            this.cBox_Invisible.Location = new System.Drawing.Point(134, 73);
            this.cBox_Invisible.Name = "cBox_Invisible";
            this.cBox_Invisible.Size = new System.Drawing.Size(66, 16);
            this.cBox_Invisible.TabIndex = 21;
            this.cBox_Invisible.Text = "Invisible";
            this.cBox_Invisible.UseVisualStyleBackColor = true;
            // 
            // cBox_ACCProtocol
            // 
            this.cBox_ACCProtocol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBox_ACCProtocol.FormattingEnabled = true;
            this.cBox_ACCProtocol.Items.AddRange(new object[] {
            "AUTO",
            "DCC",
            "MM2"});
            this.cBox_ACCProtocol.Location = new System.Drawing.Point(227, 71);
            this.cBox_ACCProtocol.Name = "cBox_ACCProtocol";
            this.cBox_ACCProtocol.Size = new System.Drawing.Size(83, 20);
            this.cBox_ACCProtocol.TabIndex = 22;
            // 
            // SelectAccessoryForm
            // 
            this.AcceptButton = this.button_Ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button_Cancel;
            this.ClientSize = new System.Drawing.Size(323, 203);
            this.Controls.Add(this.cBox_ACCProtocol);
            this.Controls.Add(this.cBox_Invisible);
            this.Controls.Add(this.cBox_ReverseSignal);
            this.Controls.Add(this.tBox_AccComment);
            this.Controls.Add(this.label_Comment);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.label_AccList);
            this.Controls.Add(this.cBox_AccType);
            this.Controls.Add(this.button_Ok);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectAccessoryForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select accessory type";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.Button button_Ok;
        private System.Windows.Forms.Label label_Comment;
        private System.Windows.Forms.Label label_AccList;
        public System.Windows.Forms.ComboBox cBox_AccType;
        public System.Windows.Forms.TextBox tBox_AccComment;
        public System.Windows.Forms.CheckBox cBox_ReverseSignal;
        public System.Windows.Forms.CheckBox cBox_Invisible;
        public System.Windows.Forms.ComboBox cBox_ACCProtocol;
    }
}
namespace DesktopStation
{
    partial class SelectLocForm
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
            this.cBox_locomotives = new System.Windows.Forms.ComboBox();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.button_Ok = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cBox_locomotives
            // 
            this.cBox_locomotives.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cBox_locomotives.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBox_locomotives.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cBox_locomotives.FormattingEnabled = true;
            this.cBox_locomotives.ItemHeight = 26;
            this.cBox_locomotives.Location = new System.Drawing.Point(14, 24);
            this.cBox_locomotives.Name = "cBox_locomotives";
            this.cBox_locomotives.Size = new System.Drawing.Size(227, 32);
            this.cBox_locomotives.TabIndex = 13;
            this.cBox_locomotives.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cBox_locomotives_DrawItem);
            // 
            // button_Cancel
            // 
            this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Cancel.Location = new System.Drawing.Point(166, 65);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(75, 37);
            this.button_Cancel.TabIndex = 12;
            this.button_Cancel.Text = "&Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            // 
            // button_Ok
            // 
            this.button_Ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_Ok.Location = new System.Drawing.Point(74, 65);
            this.button_Ok.Name = "button_Ok";
            this.button_Ok.Size = new System.Drawing.Size(75, 37);
            this.button_Ok.TabIndex = 11;
            this.button_Ok.Text = "&OK";
            this.button_Ok.UseVisualStyleBackColor = true;
            // 
            // SelectLocForm
            // 
            this.AcceptButton = this.button_Ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button_Cancel;
            this.ClientSize = new System.Drawing.Size(253, 114);
            this.Controls.Add(this.cBox_locomotives);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.button_Ok);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectLocForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select locomotive";
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.ComboBox cBox_locomotives;
        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.Button button_Ok;
    }
}
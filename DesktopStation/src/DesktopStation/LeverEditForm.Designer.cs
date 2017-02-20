namespace DesktopStation
{
    partial class LeverEditForm
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
            this.gBox_LeverOption = new System.Windows.Forms.GroupBox();
            this.numUpDown_AccGears = new System.Windows.Forms.NumericUpDown();
            this.label_Gears = new System.Windows.Forms.Label();
            this.button_Ok = new System.Windows.Forms.Button();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.cBox_LeverMode = new System.Windows.Forms.ComboBox();
            this.label_ControlMode = new System.Windows.Forms.Label();
            this.gBox_LeverOption.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown_AccGears)).BeginInit();
            this.SuspendLayout();
            // 
            // gBox_LeverOption
            // 
            this.gBox_LeverOption.Controls.Add(this.numUpDown_AccGears);
            this.gBox_LeverOption.Controls.Add(this.label_Gears);
            this.gBox_LeverOption.Location = new System.Drawing.Point(14, 55);
            this.gBox_LeverOption.Name = "gBox_LeverOption";
            this.gBox_LeverOption.Size = new System.Drawing.Size(220, 69);
            this.gBox_LeverOption.TabIndex = 12;
            this.gBox_LeverOption.TabStop = false;
            this.gBox_LeverOption.Text = "Properties for power control mode";
            // 
            // numUpDown_AccGears
            // 
            this.numUpDown_AccGears.Location = new System.Drawing.Point(141, 27);
            this.numUpDown_AccGears.Maximum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.numUpDown_AccGears.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numUpDown_AccGears.Name = "numUpDown_AccGears";
            this.numUpDown_AccGears.Size = new System.Drawing.Size(67, 21);
            this.numUpDown_AccGears.TabIndex = 17;
            this.numUpDown_AccGears.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label_Gears
            // 
            this.label_Gears.AutoSize = true;
            this.label_Gears.Location = new System.Drawing.Point(19, 29);
            this.label_Gears.Name = "label_Gears";
            this.label_Gears.Size = new System.Drawing.Size(95, 12);
            this.label_Gears.TabIndex = 15;
            this.label_Gears.Text = "Acceleration gear";
            // 
            // button_Ok
            // 
            this.button_Ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_Ok.Location = new System.Drawing.Point(78, 139);
            this.button_Ok.Name = "button_Ok";
            this.button_Ok.Size = new System.Drawing.Size(75, 34);
            this.button_Ok.TabIndex = 14;
            this.button_Ok.Text = "&OK";
            this.button_Ok.UseVisualStyleBackColor = true;
            // 
            // button_Cancel
            // 
            this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Cancel.Location = new System.Drawing.Point(159, 139);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(75, 34);
            this.button_Cancel.TabIndex = 15;
            this.button_Cancel.Text = "&Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            // 
            // cBox_LeverMode
            // 
            this.cBox_LeverMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBox_LeverMode.FormattingEnabled = true;
            this.cBox_LeverMode.Items.AddRange(new object[] {
            "Speed control",
            "Power control"});
            this.cBox_LeverMode.Location = new System.Drawing.Point(109, 12);
            this.cBox_LeverMode.Name = "cBox_LeverMode";
            this.cBox_LeverMode.Size = new System.Drawing.Size(126, 20);
            this.cBox_LeverMode.TabIndex = 13;
            // 
            // label_ControlMode
            // 
            this.label_ControlMode.AutoSize = true;
            this.label_ControlMode.Location = new System.Drawing.Point(12, 15);
            this.label_ControlMode.Name = "label_ControlMode";
            this.label_ControlMode.Size = new System.Drawing.Size(73, 12);
            this.label_ControlMode.TabIndex = 16;
            this.label_ControlMode.Text = "Control mode";
            // 
            // LeverEditForm
            // 
            this.AcceptButton = this.button_Cancel;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button_Ok;
            this.ClientSize = new System.Drawing.Size(248, 177);
            this.Controls.Add(this.gBox_LeverOption);
            this.Controls.Add(this.button_Ok);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.cBox_LeverMode);
            this.Controls.Add(this.label_ControlMode);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LeverEditForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Throttle properties";
            this.gBox_LeverOption.ResumeLayout(false);
            this.gBox_LeverOption.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown_AccGears)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gBox_LeverOption;
        public System.Windows.Forms.NumericUpDown numUpDown_AccGears;
        private System.Windows.Forms.Label label_Gears;
        private System.Windows.Forms.Button button_Ok;
        private System.Windows.Forms.Button button_Cancel;
        public System.Windows.Forms.ComboBox cBox_LeverMode;
        private System.Windows.Forms.Label label_ControlMode;
    }
}
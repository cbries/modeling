namespace DesktopStation
{
    partial class VersionForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VersionForm));
            this.button_Ok = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lView_TrackBox = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label_Devices = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // button_Ok
            // 
            this.button_Ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_Ok.Location = new System.Drawing.Point(333, 160);
            this.button_Ok.Name = "button_Ok";
            this.button_Ok.Size = new System.Drawing.Size(75, 33);
            this.button_Ok.TabIndex = 30;
            this.button_Ok.Text = "&OK";
            this.button_Ok.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(265, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(143, 12);
            this.label1.TabIndex = 32;
            this.label1.Text = "Copyright(C)\'2013- Yaasan";
            // 
            // lView_TrackBox
            // 
            this.lView_TrackBox.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.lView_TrackBox.GridLines = true;
            this.lView_TrackBox.Location = new System.Drawing.Point(12, 73);
            this.lView_TrackBox.Name = "lView_TrackBox";
            this.lView_TrackBox.Size = new System.Drawing.Size(396, 72);
            this.lView_TrackBox.TabIndex = 33;
            this.lView_TrackBox.UseCompatibleStateImageBehavior = false;
            this.lView_TrackBox.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "ID";
            this.columnHeader1.Width = 80;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Type";
            this.columnHeader2.Width = 180;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Version";
            this.columnHeader3.Width = 100;
            // 
            // label_Devices
            // 
            this.label_Devices.AutoSize = true;
            this.label_Devices.Location = new System.Drawing.Point(12, 58);
            this.label_Devices.Name = "label_Devices";
            this.label_Devices.Size = new System.Drawing.Size(171, 12);
            this.label_Devices.TabIndex = 34;
            this.label_Devices.Text = "Devices connected to TrackBox:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBox1.Location = new System.Drawing.Point(16, 13);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(243, 32);
            this.pictureBox1.TabIndex = 35;
            this.pictureBox1.TabStop = false;
            // 
            // VersionForm
            // 
            this.AcceptButton = this.button_Ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(420, 206);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label_Devices);
            this.Controls.Add(this.lView_TrackBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_Ok);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VersionForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Information";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_Ok;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        public System.Windows.Forms.ListView lView_TrackBox;
        private System.Windows.Forms.Label label_Devices;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}
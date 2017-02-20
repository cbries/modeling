namespace DesktopStation
{
    partial class ConfigClock
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigClock));
            this.button_Cancel = new System.Windows.Forms.Button();
            this.button_Ok = new System.Windows.Forms.Button();
            this.pBox_Clock = new System.Windows.Forms.PictureBox();
            this.cBox_UseUserClock = new System.Windows.Forms.CheckBox();
            this.gBox_SpecTime = new System.Windows.Forms.GroupBox();
            this.numUpDown_hour = new System.Windows.Forms.NumericUpDown();
            this.label_Hour = new System.Windows.Forms.Label();
            this.numUpDown_min = new System.Windows.Forms.NumericUpDown();
            this.label_Minutes = new System.Windows.Forms.Label();
            this.numUpDown_sec = new System.Windows.Forms.NumericUpDown();
            this.label_Seconds = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pBox_Clock)).BeginInit();
            this.gBox_SpecTime.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown_hour)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown_min)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown_sec)).BeginInit();
            this.SuspendLayout();
            // 
            // button_Cancel
            // 
            this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Cancel.Location = new System.Drawing.Point(240, 163);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(75, 31);
            this.button_Cancel.TabIndex = 13;
            this.button_Cancel.Text = "&Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            // 
            // button_Ok
            // 
            this.button_Ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_Ok.Location = new System.Drawing.Point(143, 163);
            this.button_Ok.Name = "button_Ok";
            this.button_Ok.Size = new System.Drawing.Size(75, 31);
            this.button_Ok.TabIndex = 12;
            this.button_Ok.Text = "&OK";
            this.button_Ok.UseVisualStyleBackColor = true;
            // 
            // pBox_Clock
            // 
            this.pBox_Clock.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pBox_Clock.BackgroundImage")));
            this.pBox_Clock.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pBox_Clock.Location = new System.Drawing.Point(12, 18);
            this.pBox_Clock.Name = "pBox_Clock";
            this.pBox_Clock.Size = new System.Drawing.Size(114, 150);
            this.pBox_Clock.TabIndex = 107;
            this.pBox_Clock.TabStop = false;
            this.pBox_Clock.Paint += new System.Windows.Forms.PaintEventHandler(this.pBox_Clock_Paint);
            // 
            // cBox_UseUserClock
            // 
            this.cBox_UseUserClock.Location = new System.Drawing.Point(143, 18);
            this.cBox_UseUserClock.Name = "cBox_UseUserClock";
            this.cBox_UseUserClock.Size = new System.Drawing.Size(170, 16);
            this.cBox_UseUserClock.TabIndex = 109;
            this.cBox_UseUserClock.Text = "Use the user specified time";
            this.cBox_UseUserClock.UseVisualStyleBackColor = true;
            this.cBox_UseUserClock.CheckedChanged += new System.EventHandler(this.cBox_UseUserClock_CheckedChanged);
            this.cBox_UseUserClock.TextChanged += new System.EventHandler(this.label_Seconds_TextChanged);
            // 
            // gBox_SpecTime
            // 
            this.gBox_SpecTime.Controls.Add(this.numUpDown_hour);
            this.gBox_SpecTime.Controls.Add(this.label_Hour);
            this.gBox_SpecTime.Controls.Add(this.numUpDown_min);
            this.gBox_SpecTime.Controls.Add(this.label_Minutes);
            this.gBox_SpecTime.Controls.Add(this.numUpDown_sec);
            this.gBox_SpecTime.Controls.Add(this.label_Seconds);
            this.gBox_SpecTime.Location = new System.Drawing.Point(143, 48);
            this.gBox_SpecTime.Name = "gBox_SpecTime";
            this.gBox_SpecTime.Size = new System.Drawing.Size(172, 105);
            this.gBox_SpecTime.TabIndex = 110;
            this.gBox_SpecTime.TabStop = false;
            this.gBox_SpecTime.Text = "User specified time";
            this.gBox_SpecTime.TextChanged += new System.EventHandler(this.label_Seconds_TextChanged);
            // 
            // numUpDown_hour
            // 
            this.numUpDown_hour.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.numUpDown_hour.Location = new System.Drawing.Point(90, 18);
            this.numUpDown_hour.Maximum = new decimal(new int[] {
            23,
            0,
            0,
            0});
            this.numUpDown_hour.Name = "numUpDown_hour";
            this.numUpDown_hour.Size = new System.Drawing.Size(56, 26);
            this.numUpDown_hour.TabIndex = 116;
            this.numUpDown_hour.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numUpDown_hour.ValueChanged += new System.EventHandler(this.numUpDown_hour_ValueChanged);
            // 
            // label_Hour
            // 
            this.label_Hour.Location = new System.Drawing.Point(18, 24);
            this.label_Hour.Name = "label_Hour";
            this.label_Hour.Size = new System.Drawing.Size(65, 12);
            this.label_Hour.TabIndex = 115;
            this.label_Hour.Text = "Hours";
            this.label_Hour.TextChanged += new System.EventHandler(this.label_Seconds_TextChanged);
            // 
            // numUpDown_min
            // 
            this.numUpDown_min.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.numUpDown_min.Location = new System.Drawing.Point(90, 47);
            this.numUpDown_min.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.numUpDown_min.Name = "numUpDown_min";
            this.numUpDown_min.Size = new System.Drawing.Size(56, 26);
            this.numUpDown_min.TabIndex = 114;
            this.numUpDown_min.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numUpDown_min.ValueChanged += new System.EventHandler(this.numUpDown_hour_ValueChanged);
            // 
            // label_Minutes
            // 
            this.label_Minutes.Location = new System.Drawing.Point(18, 53);
            this.label_Minutes.Name = "label_Minutes";
            this.label_Minutes.Size = new System.Drawing.Size(65, 12);
            this.label_Minutes.TabIndex = 113;
            this.label_Minutes.Text = "Minutes";
            this.label_Minutes.TextChanged += new System.EventHandler(this.label_Seconds_TextChanged);
            // 
            // numUpDown_sec
            // 
            this.numUpDown_sec.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.numUpDown_sec.Location = new System.Drawing.Point(90, 76);
            this.numUpDown_sec.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.numUpDown_sec.Name = "numUpDown_sec";
            this.numUpDown_sec.Size = new System.Drawing.Size(56, 26);
            this.numUpDown_sec.TabIndex = 112;
            this.numUpDown_sec.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numUpDown_sec.ValueChanged += new System.EventHandler(this.numUpDown_hour_ValueChanged);
            // 
            // label_Seconds
            // 
            this.label_Seconds.Location = new System.Drawing.Point(18, 82);
            this.label_Seconds.Name = "label_Seconds";
            this.label_Seconds.Size = new System.Drawing.Size(65, 12);
            this.label_Seconds.TabIndex = 111;
            this.label_Seconds.Text = "Seconds";
            this.label_Seconds.TextChanged += new System.EventHandler(this.label_Seconds_TextChanged);
            // 
            // ConfigClock
            // 
            this.AcceptButton = this.button_Ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button_Cancel;
            this.ClientSize = new System.Drawing.Size(327, 206);
            this.Controls.Add(this.gBox_SpecTime);
            this.Controls.Add(this.cBox_UseUserClock);
            this.Controls.Add(this.pBox_Clock);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.button_Ok);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigClock";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Clock properties";
            ((System.ComponentModel.ISupportInitialize)(this.pBox_Clock)).EndInit();
            this.gBox_SpecTime.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown_hour)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown_min)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown_sec)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.Button button_Ok;
        private System.Windows.Forms.PictureBox pBox_Clock;
        public System.Windows.Forms.CheckBox cBox_UseUserClock;
        private System.Windows.Forms.GroupBox gBox_SpecTime;
        public System.Windows.Forms.NumericUpDown numUpDown_hour;
        private System.Windows.Forms.Label label_Hour;
        public System.Windows.Forms.NumericUpDown numUpDown_min;
        private System.Windows.Forms.Label label_Minutes;
        public System.Windows.Forms.NumericUpDown numUpDown_sec;
        private System.Windows.Forms.Label label_Seconds;
    }
}
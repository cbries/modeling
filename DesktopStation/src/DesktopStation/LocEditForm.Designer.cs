namespace DesktopStation
{
    partial class LocEditForm
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
            this.numUpDown_LocMaxSpeed = new System.Windows.Forms.NumericUpDown();
            this.label_SpdMaxVal = new System.Windows.Forms.Label();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.tBox_AddrHigh = new System.Windows.Forms.TextBox();
            this.label_SpdMeterMax = new System.Windows.Forms.Label();
            this.numUpDown_ReduceRatio = new System.Windows.Forms.NumericUpDown();
            this.buttonClearLocImage = new System.Windows.Forms.Button();
            this.label_SpdDecRatio = new System.Windows.Forms.Label();
            this.gBox_LocImage = new System.Windows.Forms.GroupBox();
            this.label_LocImageDescription = new System.Windows.Forms.Label();
            this.buttonLoadLocImage = new System.Windows.Forms.Button();
            this.LocImageBox = new System.Windows.Forms.PictureBox();
            this.numUpDown_AccRatio = new System.Windows.Forms.NumericUpDown();
            this.gBox_SpdAdj = new System.Windows.Forms.GroupBox();
            this.label_SpdAccRatio = new System.Windows.Forms.Label();
            this.numUpDown_MaxSpeed = new System.Windows.Forms.NumericUpDown();
            this.gBox_SpdMeter = new System.Windows.Forms.GroupBox();
            this.label_LocAddr2 = new System.Windows.Forms.Label();
            this.gBox_DblHead = new System.Windows.Forms.GroupBox();
            this.cBox_Speedstep2 = new System.Windows.Forms.ComboBox();
            this.label_Speedstep2 = new System.Windows.Forms.Label();
            this.label_LocProtcol2 = new System.Windows.Forms.Label();
            this.cBox_ProtcolLoc2 = new System.Windows.Forms.ComboBox();
            this.tBox_Addr = new System.Windows.Forms.TextBox();
            this.label_ArtNo = new System.Windows.Forms.Label();
            this.tBox_ArtNo = new System.Windows.Forms.TextBox();
            this.label_Manufacture = new System.Windows.Forms.Label();
            this.label_LocName = new System.Windows.Forms.Label();
            this.cBox_LocName = new System.Windows.Forms.ComboBox();
            this.label_LocAddr = new System.Windows.Forms.Label();
            this.cBox_Manufacture = new System.Windows.Forms.ComboBox();
            this.button_Ok = new System.Windows.Forms.Button();
            this.cBox_ProtcolLoc1 = new System.Windows.Forms.ComboBox();
            this.label_LocProtcol = new System.Windows.Forms.Label();
            this.tBox_MfxUID = new System.Windows.Forms.TextBox();
            this.label_MfxDecoderUID = new System.Windows.Forms.Label();
            this.cBox_SpeedStep = new System.Windows.Forms.ComboBox();
            this.label_Speedstep = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown_LocMaxSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown_ReduceRatio)).BeginInit();
            this.gBox_LocImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LocImageBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown_AccRatio)).BeginInit();
            this.gBox_SpdAdj.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown_MaxSpeed)).BeginInit();
            this.gBox_SpdMeter.SuspendLayout();
            this.gBox_DblHead.SuspendLayout();
            this.SuspendLayout();
            // 
            // numUpDown_LocMaxSpeed
            // 
            this.numUpDown_LocMaxSpeed.Location = new System.Drawing.Point(131, 81);
            this.numUpDown_LocMaxSpeed.Maximum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.numUpDown_LocMaxSpeed.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numUpDown_LocMaxSpeed.Name = "numUpDown_LocMaxSpeed";
            this.numUpDown_LocMaxSpeed.Size = new System.Drawing.Size(70, 21);
            this.numUpDown_LocMaxSpeed.TabIndex = 8;
            this.numUpDown_LocMaxSpeed.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // label_SpdMaxVal
            // 
            this.label_SpdMaxVal.Location = new System.Drawing.Point(10, 83);
            this.label_SpdMaxVal.Name = "label_SpdMaxVal";
            this.label_SpdMaxVal.Size = new System.Drawing.Size(115, 12);
            this.label_SpdMaxVal.TabIndex = 7;
            this.label_SpdMaxVal.Text = "Max speed value";
            this.label_SpdMaxVal.TextChanged += new System.EventHandler(this.label_LocAddr_TextChanged);
            // 
            // button_Cancel
            // 
            this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Cancel.Location = new System.Drawing.Point(407, 320);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(75, 33);
            this.button_Cancel.TabIndex = 31;
            this.button_Cancel.Text = "&Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            // 
            // tBox_AddrHigh
            // 
            this.tBox_AddrHigh.Location = new System.Drawing.Point(136, 22);
            this.tBox_AddrHigh.Name = "tBox_AddrHigh";
            this.tBox_AddrHigh.Size = new System.Drawing.Size(65, 21);
            this.tBox_AddrHigh.TabIndex = 17;
            this.tBox_AddrHigh.Text = "0";
            this.tBox_AddrHigh.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label_SpdMeterMax
            // 
            this.label_SpdMeterMax.Location = new System.Drawing.Point(10, 26);
            this.label_SpdMeterMax.Name = "label_SpdMeterMax";
            this.label_SpdMeterMax.Size = new System.Drawing.Size(115, 12);
            this.label_SpdMeterMax.TabIndex = 1;
            this.label_SpdMeterMax.Text = "Max speed(km/h)";
            this.label_SpdMeterMax.TextChanged += new System.EventHandler(this.label_LocAddr_TextChanged);
            // 
            // numUpDown_ReduceRatio
            // 
            this.numUpDown_ReduceRatio.Location = new System.Drawing.Point(131, 50);
            this.numUpDown_ReduceRatio.Maximum = new decimal(new int[] {
            70,
            0,
            0,
            0});
            this.numUpDown_ReduceRatio.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numUpDown_ReduceRatio.Name = "numUpDown_ReduceRatio";
            this.numUpDown_ReduceRatio.Size = new System.Drawing.Size(70, 21);
            this.numUpDown_ReduceRatio.TabIndex = 6;
            this.numUpDown_ReduceRatio.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // buttonClearLocImage
            // 
            this.buttonClearLocImage.Location = new System.Drawing.Point(171, 49);
            this.buttonClearLocImage.Name = "buttonClearLocImage";
            this.buttonClearLocImage.Size = new System.Drawing.Size(55, 23);
            this.buttonClearLocImage.TabIndex = 26;
            this.buttonClearLocImage.Text = "Clear";
            this.buttonClearLocImage.UseVisualStyleBackColor = true;
            this.buttonClearLocImage.Click += new System.EventHandler(this.button2_Click);
            // 
            // label_SpdDecRatio
            // 
            this.label_SpdDecRatio.Location = new System.Drawing.Point(10, 52);
            this.label_SpdDecRatio.Name = "label_SpdDecRatio";
            this.label_SpdDecRatio.Size = new System.Drawing.Size(115, 12);
            this.label_SpdDecRatio.TabIndex = 5;
            this.label_SpdDecRatio.Text = "Deceleration ratio";
            this.label_SpdDecRatio.TextChanged += new System.EventHandler(this.label_LocAddr_TextChanged);
            // 
            // gBox_LocImage
            // 
            this.gBox_LocImage.Controls.Add(this.label_LocImageDescription);
            this.gBox_LocImage.Controls.Add(this.buttonClearLocImage);
            this.gBox_LocImage.Controls.Add(this.buttonLoadLocImage);
            this.gBox_LocImage.Controls.Add(this.LocImageBox);
            this.gBox_LocImage.Location = new System.Drawing.Point(17, 214);
            this.gBox_LocImage.Name = "gBox_LocImage";
            this.gBox_LocImage.Size = new System.Drawing.Size(232, 96);
            this.gBox_LocImage.TabIndex = 39;
            this.gBox_LocImage.TabStop = false;
            this.gBox_LocImage.Text = "Locomotive image";
            this.gBox_LocImage.TextChanged += new System.EventHandler(this.label_LocAddr_TextChanged);
            // 
            // label_LocImageDescription
            // 
            this.label_LocImageDescription.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label_LocImageDescription.Location = new System.Drawing.Point(13, 78);
            this.label_LocImageDescription.Name = "label_LocImageDescription";
            this.label_LocImageDescription.Size = new System.Drawing.Size(200, 11);
            this.label_LocImageDescription.TabIndex = 27;
            this.label_LocImageDescription.Text = "Maximum size should be 160x48pixels";
            this.label_LocImageDescription.TextChanged += new System.EventHandler(this.label_LocAddr_TextChanged);
            // 
            // buttonLoadLocImage
            // 
            this.buttonLoadLocImage.Location = new System.Drawing.Point(171, 24);
            this.buttonLoadLocImage.Name = "buttonLoadLocImage";
            this.buttonLoadLocImage.Size = new System.Drawing.Size(55, 23);
            this.buttonLoadLocImage.TabIndex = 25;
            this.buttonLoadLocImage.Text = "Load";
            this.buttonLoadLocImage.UseVisualStyleBackColor = true;
            this.buttonLoadLocImage.Click += new System.EventHandler(this.buttonLoadLocImage_Click);
            // 
            // LocImageBox
            // 
            this.LocImageBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.LocImageBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LocImageBox.Location = new System.Drawing.Point(7, 24);
            this.LocImageBox.Name = "LocImageBox";
            this.LocImageBox.Size = new System.Drawing.Size(160, 48);
            this.LocImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.LocImageBox.TabIndex = 24;
            this.LocImageBox.TabStop = false;
            // 
            // numUpDown_AccRatio
            // 
            this.numUpDown_AccRatio.Location = new System.Drawing.Point(131, 22);
            this.numUpDown_AccRatio.Maximum = new decimal(new int[] {
            70,
            0,
            0,
            0});
            this.numUpDown_AccRatio.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numUpDown_AccRatio.Name = "numUpDown_AccRatio";
            this.numUpDown_AccRatio.Size = new System.Drawing.Size(70, 21);
            this.numUpDown_AccRatio.TabIndex = 4;
            this.numUpDown_AccRatio.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // gBox_SpdAdj
            // 
            this.gBox_SpdAdj.Controls.Add(this.numUpDown_LocMaxSpeed);
            this.gBox_SpdAdj.Controls.Add(this.label_SpdMaxVal);
            this.gBox_SpdAdj.Controls.Add(this.numUpDown_ReduceRatio);
            this.gBox_SpdAdj.Controls.Add(this.label_SpdDecRatio);
            this.gBox_SpdAdj.Controls.Add(this.numUpDown_AccRatio);
            this.gBox_SpdAdj.Controls.Add(this.label_SpdAccRatio);
            this.gBox_SpdAdj.Location = new System.Drawing.Point(271, 129);
            this.gBox_SpdAdj.Name = "gBox_SpdAdj";
            this.gBox_SpdAdj.Size = new System.Drawing.Size(213, 114);
            this.gBox_SpdAdj.TabIndex = 38;
            this.gBox_SpdAdj.TabStop = false;
            this.gBox_SpdAdj.Text = "Speed adjustment";
            this.gBox_SpdAdj.TextChanged += new System.EventHandler(this.label_LocAddr_TextChanged);
            // 
            // label_SpdAccRatio
            // 
            this.label_SpdAccRatio.Location = new System.Drawing.Point(10, 24);
            this.label_SpdAccRatio.Name = "label_SpdAccRatio";
            this.label_SpdAccRatio.Size = new System.Drawing.Size(115, 12);
            this.label_SpdAccRatio.TabIndex = 3;
            this.label_SpdAccRatio.Text = "Acceleration ratio";
            this.label_SpdAccRatio.TextChanged += new System.EventHandler(this.label_LocAddr_TextChanged);
            // 
            // numUpDown_MaxSpeed
            // 
            this.numUpDown_MaxSpeed.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numUpDown_MaxSpeed.Location = new System.Drawing.Point(132, 24);
            this.numUpDown_MaxSpeed.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.numUpDown_MaxSpeed.Minimum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numUpDown_MaxSpeed.Name = "numUpDown_MaxSpeed";
            this.numUpDown_MaxSpeed.Size = new System.Drawing.Size(70, 21);
            this.numUpDown_MaxSpeed.TabIndex = 2;
            this.numUpDown_MaxSpeed.Value = new decimal(new int[] {
            300,
            0,
            0,
            0});
            // 
            // gBox_SpdMeter
            // 
            this.gBox_SpdMeter.Controls.Add(this.numUpDown_MaxSpeed);
            this.gBox_SpdMeter.Controls.Add(this.label_SpdMeterMax);
            this.gBox_SpdMeter.Location = new System.Drawing.Point(271, 249);
            this.gBox_SpdMeter.Name = "gBox_SpdMeter";
            this.gBox_SpdMeter.Size = new System.Drawing.Size(213, 60);
            this.gBox_SpdMeter.TabIndex = 37;
            this.gBox_SpdMeter.TabStop = false;
            this.gBox_SpdMeter.Text = "Speedometer config";
            this.gBox_SpdMeter.TextChanged += new System.EventHandler(this.label_LocAddr_TextChanged);
            // 
            // label_LocAddr2
            // 
            this.label_LocAddr2.Location = new System.Drawing.Point(9, 25);
            this.label_LocAddr2.Name = "label_LocAddr2";
            this.label_LocAddr2.Size = new System.Drawing.Size(120, 12);
            this.label_LocAddr2.TabIndex = 15;
            this.label_LocAddr2.Text = "2nd Loc Address(0-)";
            this.label_LocAddr2.TextChanged += new System.EventHandler(this.label_LocAddr_TextChanged);
            // 
            // gBox_DblHead
            // 
            this.gBox_DblHead.Controls.Add(this.cBox_Speedstep2);
            this.gBox_DblHead.Controls.Add(this.label_Speedstep2);
            this.gBox_DblHead.Controls.Add(this.label_LocProtcol2);
            this.gBox_DblHead.Controls.Add(this.cBox_ProtcolLoc2);
            this.gBox_DblHead.Controls.Add(this.tBox_AddrHigh);
            this.gBox_DblHead.Controls.Add(this.label_LocAddr2);
            this.gBox_DblHead.Location = new System.Drawing.Point(271, 12);
            this.gBox_DblHead.Name = "gBox_DblHead";
            this.gBox_DblHead.Size = new System.Drawing.Size(212, 104);
            this.gBox_DblHead.TabIndex = 35;
            this.gBox_DblHead.TabStop = false;
            this.gBox_DblHead.Text = "Configure double-heading properties";
            this.gBox_DblHead.TextChanged += new System.EventHandler(this.label_LocAddr_TextChanged);
            // 
            // cBox_Speedstep2
            // 
            this.cBox_Speedstep2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBox_Speedstep2.FormattingEnabled = true;
            this.cBox_Speedstep2.Items.AddRange(new object[] {
            "DCC28",
            "DCC14",
            "DCC128"});
            this.cBox_Speedstep2.Location = new System.Drawing.Point(136, 73);
            this.cBox_Speedstep2.Name = "cBox_Speedstep2";
            this.cBox_Speedstep2.Size = new System.Drawing.Size(65, 20);
            this.cBox_Speedstep2.TabIndex = 45;
            // 
            // label_Speedstep2
            // 
            this.label_Speedstep2.Location = new System.Drawing.Point(9, 76);
            this.label_Speedstep2.Name = "label_Speedstep2";
            this.label_Speedstep2.Size = new System.Drawing.Size(120, 12);
            this.label_Speedstep2.TabIndex = 43;
            this.label_Speedstep2.Text = "2nd Speed step";
            // 
            // label_LocProtcol2
            // 
            this.label_LocProtcol2.Location = new System.Drawing.Point(9, 50);
            this.label_LocProtcol2.Name = "label_LocProtcol2";
            this.label_LocProtcol2.Size = new System.Drawing.Size(120, 12);
            this.label_LocProtcol2.TabIndex = 42;
            this.label_LocProtcol2.Text = "2nd Loc Protcol";
            this.label_LocProtcol2.TextChanged += new System.EventHandler(this.label_LocAddr_TextChanged);
            // 
            // cBox_ProtcolLoc2
            // 
            this.cBox_ProtcolLoc2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBox_ProtcolLoc2.FormattingEnabled = true;
            this.cBox_ProtcolLoc2.Items.AddRange(new object[] {
            "MM2",
            "mfx",
            "DCC"});
            this.cBox_ProtcolLoc2.Location = new System.Drawing.Point(136, 47);
            this.cBox_ProtcolLoc2.Name = "cBox_ProtcolLoc2";
            this.cBox_ProtcolLoc2.Size = new System.Drawing.Size(65, 20);
            this.cBox_ProtcolLoc2.TabIndex = 41;
            this.cBox_ProtcolLoc2.SelectedIndexChanged += new System.EventHandler(this.cBox_ProtcolLoc2_SelectedIndexChanged);
            // 
            // tBox_Addr
            // 
            this.tBox_Addr.Location = new System.Drawing.Point(131, 21);
            this.tBox_Addr.Name = "tBox_Addr";
            this.tBox_Addr.Size = new System.Drawing.Size(121, 21);
            this.tBox_Addr.TabIndex = 25;
            this.tBox_Addr.Text = "0";
            this.tBox_Addr.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label_ArtNo
            // 
            this.label_ArtNo.Location = new System.Drawing.Point(12, 190);
            this.label_ArtNo.Name = "label_ArtNo";
            this.label_ArtNo.Size = new System.Drawing.Size(115, 12);
            this.label_ArtNo.TabIndex = 34;
            this.label_ArtNo.Text = "Item Art No.";
            this.label_ArtNo.TextChanged += new System.EventHandler(this.label_LocAddr_TextChanged);
            // 
            // tBox_ArtNo
            // 
            this.tBox_ArtNo.Location = new System.Drawing.Point(131, 187);
            this.tBox_ArtNo.Name = "tBox_ArtNo";
            this.tBox_ArtNo.Size = new System.Drawing.Size(121, 21);
            this.tBox_ArtNo.TabIndex = 28;
            this.tBox_ArtNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label_Manufacture
            // 
            this.label_Manufacture.Location = new System.Drawing.Point(12, 161);
            this.label_Manufacture.Name = "label_Manufacture";
            this.label_Manufacture.Size = new System.Drawing.Size(115, 12);
            this.label_Manufacture.TabIndex = 33;
            this.label_Manufacture.Text = "Manufacturer";
            this.label_Manufacture.TextChanged += new System.EventHandler(this.label_LocAddr_TextChanged);
            // 
            // label_LocName
            // 
            this.label_LocName.Location = new System.Drawing.Point(12, 132);
            this.label_LocName.Name = "label_LocName";
            this.label_LocName.Size = new System.Drawing.Size(115, 12);
            this.label_LocName.TabIndex = 32;
            this.label_LocName.Text = "Loc Name";
            this.label_LocName.TextChanged += new System.EventHandler(this.label_LocAddr_TextChanged);
            // 
            // cBox_LocName
            // 
            this.cBox_LocName.FormattingEnabled = true;
            this.cBox_LocName.Location = new System.Drawing.Point(131, 129);
            this.cBox_LocName.Name = "cBox_LocName";
            this.cBox_LocName.Size = new System.Drawing.Size(121, 20);
            this.cBox_LocName.TabIndex = 26;
            // 
            // label_LocAddr
            // 
            this.label_LocAddr.Location = new System.Drawing.Point(12, 24);
            this.label_LocAddr.Name = "label_LocAddr";
            this.label_LocAddr.Size = new System.Drawing.Size(115, 12);
            this.label_LocAddr.TabIndex = 30;
            this.label_LocAddr.Text = "Loc Address(1-)";
            this.label_LocAddr.TextChanged += new System.EventHandler(this.label_LocAddr_TextChanged);
            // 
            // cBox_Manufacture
            // 
            this.cBox_Manufacture.FormattingEnabled = true;
            this.cBox_Manufacture.Items.AddRange(new object[] {
            "marklin",
            "PIKO",
            "Roco",
            "Brawa",
            "Fleishmann",
            "KATO",
            "TOMIX",
            "Liliput",
            "Rivarossi",
            "ESU"});
            this.cBox_Manufacture.Location = new System.Drawing.Point(131, 158);
            this.cBox_Manufacture.Name = "cBox_Manufacture";
            this.cBox_Manufacture.Size = new System.Drawing.Size(121, 20);
            this.cBox_Manufacture.TabIndex = 27;
            // 
            // button_Ok
            // 
            this.button_Ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_Ok.Location = new System.Drawing.Point(326, 320);
            this.button_Ok.Name = "button_Ok";
            this.button_Ok.Size = new System.Drawing.Size(75, 33);
            this.button_Ok.TabIndex = 29;
            this.button_Ok.Text = "&OK";
            this.button_Ok.UseVisualStyleBackColor = true;
            // 
            // cBox_ProtcolLoc1
            // 
            this.cBox_ProtcolLoc1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBox_ProtcolLoc1.FormattingEnabled = true;
            this.cBox_ProtcolLoc1.Items.AddRange(new object[] {
            "MM2",
            "mfx",
            "DCC"});
            this.cBox_ProtcolLoc1.Location = new System.Drawing.Point(131, 49);
            this.cBox_ProtcolLoc1.Name = "cBox_ProtcolLoc1";
            this.cBox_ProtcolLoc1.Size = new System.Drawing.Size(121, 20);
            this.cBox_ProtcolLoc1.TabIndex = 40;
            this.cBox_ProtcolLoc1.SelectedIndexChanged += new System.EventHandler(this.cBox_ProtcolLoc1_SelectedIndexChanged);
            // 
            // label_LocProtcol
            // 
            this.label_LocProtcol.Location = new System.Drawing.Point(12, 52);
            this.label_LocProtcol.Name = "label_LocProtcol";
            this.label_LocProtcol.Size = new System.Drawing.Size(115, 12);
            this.label_LocProtcol.TabIndex = 41;
            this.label_LocProtcol.Text = "Loc Protcol";
            this.label_LocProtcol.TextChanged += new System.EventHandler(this.label_LocAddr_TextChanged);
            // 
            // tBox_MfxUID
            // 
            this.tBox_MfxUID.Location = new System.Drawing.Point(131, 101);
            this.tBox_MfxUID.Name = "tBox_MfxUID";
            this.tBox_MfxUID.Size = new System.Drawing.Size(121, 21);
            this.tBox_MfxUID.TabIndex = 42;
            this.tBox_MfxUID.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label_MfxDecoderUID
            // 
            this.label_MfxDecoderUID.Location = new System.Drawing.Point(12, 104);
            this.label_MfxDecoderUID.Name = "label_MfxDecoderUID";
            this.label_MfxDecoderUID.Size = new System.Drawing.Size(115, 12);
            this.label_MfxDecoderUID.TabIndex = 43;
            this.label_MfxDecoderUID.Text = "mfx decoder UID";
            this.label_MfxDecoderUID.TextChanged += new System.EventHandler(this.label_LocAddr_TextChanged);
            // 
            // cBox_SpeedStep
            // 
            this.cBox_SpeedStep.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBox_SpeedStep.FormattingEnabled = true;
            this.cBox_SpeedStep.Items.AddRange(new object[] {
            "DCC 28",
            "DCC 14",
            "DCC128"});
            this.cBox_SpeedStep.Location = new System.Drawing.Point(131, 75);
            this.cBox_SpeedStep.Name = "cBox_SpeedStep";
            this.cBox_SpeedStep.Size = new System.Drawing.Size(121, 20);
            this.cBox_SpeedStep.TabIndex = 44;
            // 
            // label_Speedstep
            // 
            this.label_Speedstep.Location = new System.Drawing.Point(12, 78);
            this.label_Speedstep.Name = "label_Speedstep";
            this.label_Speedstep.Size = new System.Drawing.Size(115, 12);
            this.label_Speedstep.TabIndex = 45;
            this.label_Speedstep.Text = "Speed step";
            // 
            // LocEditForm
            // 
            this.AcceptButton = this.button_Ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button_Cancel;
            this.ClientSize = new System.Drawing.Size(497, 365);
            this.Controls.Add(this.label_Speedstep);
            this.Controls.Add(this.cBox_SpeedStep);
            this.Controls.Add(this.label_MfxDecoderUID);
            this.Controls.Add(this.tBox_MfxUID);
            this.Controls.Add(this.label_LocProtcol);
            this.Controls.Add(this.cBox_ProtcolLoc1);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.gBox_LocImage);
            this.Controls.Add(this.gBox_SpdAdj);
            this.Controls.Add(this.gBox_SpdMeter);
            this.Controls.Add(this.gBox_DblHead);
            this.Controls.Add(this.tBox_Addr);
            this.Controls.Add(this.label_ArtNo);
            this.Controls.Add(this.tBox_ArtNo);
            this.Controls.Add(this.label_Manufacture);
            this.Controls.Add(this.label_LocName);
            this.Controls.Add(this.cBox_LocName);
            this.Controls.Add(this.label_LocAddr);
            this.Controls.Add(this.cBox_Manufacture);
            this.Controls.Add(this.button_Ok);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LocEditForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit locomotive properties";
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown_LocMaxSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown_ReduceRatio)).EndInit();
            this.gBox_LocImage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LocImageBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown_AccRatio)).EndInit();
            this.gBox_SpdAdj.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown_MaxSpeed)).EndInit();
            this.gBox_SpdMeter.ResumeLayout(false);
            this.gBox_DblHead.ResumeLayout(false);
            this.gBox_DblHead.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.NumericUpDown numUpDown_LocMaxSpeed;
        private System.Windows.Forms.Label label_SpdMaxVal;
        private System.Windows.Forms.Button button_Cancel;
        public System.Windows.Forms.TextBox tBox_AddrHigh;
        private System.Windows.Forms.Label label_SpdMeterMax;
        public System.Windows.Forms.NumericUpDown numUpDown_ReduceRatio;
        private System.Windows.Forms.Button buttonClearLocImage;
        private System.Windows.Forms.Label label_SpdDecRatio;
        private System.Windows.Forms.GroupBox gBox_LocImage;
        private System.Windows.Forms.Button buttonLoadLocImage;
        public System.Windows.Forms.PictureBox LocImageBox;
        public System.Windows.Forms.NumericUpDown numUpDown_AccRatio;
        private System.Windows.Forms.GroupBox gBox_SpdAdj;
        private System.Windows.Forms.Label label_SpdAccRatio;
        public System.Windows.Forms.NumericUpDown numUpDown_MaxSpeed;
        private System.Windows.Forms.GroupBox gBox_SpdMeter;
        private System.Windows.Forms.Label label_LocAddr2;
        private System.Windows.Forms.GroupBox gBox_DblHead;
        public System.Windows.Forms.TextBox tBox_Addr;
        private System.Windows.Forms.Label label_ArtNo;
        public System.Windows.Forms.TextBox tBox_ArtNo;
        private System.Windows.Forms.Label label_Manufacture;
        private System.Windows.Forms.Label label_LocName;
        public System.Windows.Forms.ComboBox cBox_LocName;
        private System.Windows.Forms.Label label_LocAddr;
        public System.Windows.Forms.ComboBox cBox_Manufacture;
        private System.Windows.Forms.Button button_Ok;
        public System.Windows.Forms.ComboBox cBox_ProtcolLoc2;
        public System.Windows.Forms.ComboBox cBox_ProtcolLoc1;
        private System.Windows.Forms.Label label_LocProtcol2;
        private System.Windows.Forms.Label label_LocProtcol;
        private System.Windows.Forms.Label label_LocImageDescription;
        public System.Windows.Forms.TextBox tBox_MfxUID;
        private System.Windows.Forms.Label label_MfxDecoderUID;
        public System.Windows.Forms.ComboBox cBox_Speedstep2;
        private System.Windows.Forms.Label label_Speedstep2;
        public System.Windows.Forms.ComboBox cBox_SpeedStep;
        private System.Windows.Forms.Label label_Speedstep;
    }
}
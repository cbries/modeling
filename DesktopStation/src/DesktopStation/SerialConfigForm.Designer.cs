namespace DesktopStation
{
    partial class SerialConfigForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SerialConfigForm));
            this.label_PortNo = new System.Windows.Forms.Label();
            this.label_Baudrate = new System.Windows.Forms.Label();
            this.cBox_SerialPort = new System.Windows.Forms.ComboBox();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.cBox_Baudrate = new System.Windows.Forms.ComboBox();
            this.gBox_Serial = new System.Windows.Forms.GroupBox();
            this.cBox_DtrEnable = new System.Windows.Forms.CheckBox();
            this.button_Ok = new System.Windows.Forms.Button();
            this.gBox_DCC = new System.Windows.Forms.GroupBox();
            this.cBox_DCC = new System.Windows.Forms.CheckBox();
            this.cBox_MfxAutoRegister = new System.Windows.Forms.CheckBox();
            this.gBox_S88 = new System.Windows.Forms.GroupBox();
            this.label_S88SensorNums = new System.Windows.Forms.Label();
            this.numUpDown_S88SensorNums = new System.Windows.Forms.NumericUpDown();
            this.label_S88DetectInterval = new System.Windows.Forms.Label();
            this.numUpDown_S88DetectFreq = new System.Windows.Forms.NumericUpDown();
            this.cBox_S88 = new System.Windows.Forms.CheckBox();
            this.gBox_MultiScreen = new System.Windows.Forms.GroupBox();
            this.label_Screen4 = new System.Windows.Forms.Label();
            this.cBox_Screen4Panel = new System.Windows.Forms.ComboBox();
            this.label_Screen3 = new System.Windows.Forms.Label();
            this.cBox_Screen3Panel = new System.Windows.Forms.ComboBox();
            this.label_Screen2 = new System.Windows.Forms.Label();
            this.cBox_Screen2Panel = new System.Windows.Forms.ComboBox();
            this.label_Screen1 = new System.Windows.Forms.Label();
            this.cBox_Screen1Panel = new System.Windows.Forms.ComboBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label_MSNum = new System.Windows.Forms.Label();
            this.cBox_ScreenNums = new System.Windows.Forms.ComboBox();
            this.gBox_mfx = new System.Windows.Forms.GroupBox();
            this.cBox_MfxAutoUpdate = new System.Windows.Forms.CheckBox();
            this.gBox_AddVisible = new System.Windows.Forms.GroupBox();
            this.numUpDown_WindowZoom = new System.Windows.Forms.NumericUpDown();
            this.label_WindowZoom = new System.Windows.Forms.Label();
            this.label_VisibleBottom = new System.Windows.Forms.Label();
            this.cBox_SideFuncBottom = new System.Windows.Forms.ComboBox();
            this.label_VisibleRight = new System.Windows.Forms.Label();
            this.cBox_SideFuncRight = new System.Windows.Forms.ComboBox();
            this.gBox_Other = new System.Windows.Forms.GroupBox();
            this.cBox_ClearAcc = new System.Windows.Forms.CheckBox();
            this.cBox_AutoCloseSerial = new System.Windows.Forms.CheckBox();
            this.cBox_StopAllLocPon = new System.Windows.Forms.CheckBox();
            this.gBox_Language = new System.Windows.Forms.GroupBox();
            this.label_LanguageDescription = new System.Windows.Forms.Label();
            this.cBox_LanguageFile = new System.Windows.Forms.ComboBox();
            this.cBox_ConnectionType = new System.Windows.Forms.ComboBox();
            this.labelConnectType = new System.Windows.Forms.Label();
            this.gBox_Http = new System.Windows.Forms.GroupBox();
            this.label_IPAddr = new System.Windows.Forms.Label();
            this.cBox_IPAddress = new System.Windows.Forms.ComboBox();
            this.gBox_Serial.SuspendLayout();
            this.gBox_DCC.SuspendLayout();
            this.gBox_S88.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown_S88SensorNums)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown_S88DetectFreq)).BeginInit();
            this.gBox_MultiScreen.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.gBox_mfx.SuspendLayout();
            this.gBox_AddVisible.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown_WindowZoom)).BeginInit();
            this.gBox_Other.SuspendLayout();
            this.gBox_Language.SuspendLayout();
            this.gBox_Http.SuspendLayout();
            this.SuspendLayout();
            // 
            // label_PortNo
            // 
            this.label_PortNo.Location = new System.Drawing.Point(16, 30);
            this.label_PortNo.Name = "label_PortNo";
            this.label_PortNo.Size = new System.Drawing.Size(59, 12);
            this.label_PortNo.TabIndex = 4;
            this.label_PortNo.Text = "Port No";
            this.label_PortNo.TextChanged += new System.EventHandler(this.label_PortNo_TextChanged);
            // 
            // label_Baudrate
            // 
            this.label_Baudrate.Location = new System.Drawing.Point(16, 63);
            this.label_Baudrate.Name = "label_Baudrate";
            this.label_Baudrate.Size = new System.Drawing.Size(59, 12);
            this.label_Baudrate.TabIndex = 2;
            this.label_Baudrate.Text = "Baudrate";
            this.label_Baudrate.TextChanged += new System.EventHandler(this.label_PortNo_TextChanged);
            // 
            // cBox_SerialPort
            // 
            this.cBox_SerialPort.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cBox_SerialPort.FormattingEnabled = true;
            this.cBox_SerialPort.Location = new System.Drawing.Point(81, 26);
            this.cBox_SerialPort.Name = "cBox_SerialPort";
            this.cBox_SerialPort.Size = new System.Drawing.Size(121, 24);
            this.cBox_SerialPort.TabIndex = 1;
            // 
            // button_Cancel
            // 
            this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Cancel.Location = new System.Drawing.Point(641, 406);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(75, 33);
            this.button_Cancel.TabIndex = 7;
            this.button_Cancel.Text = "&Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            // 
            // cBox_Baudrate
            // 
            this.cBox_Baudrate.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cBox_Baudrate.FormattingEnabled = true;
            this.cBox_Baudrate.Items.AddRange(new object[] {
            "9600",
            "19200",
            "38400",
            "57600",
            "115200"});
            this.cBox_Baudrate.Location = new System.Drawing.Point(81, 56);
            this.cBox_Baudrate.Name = "cBox_Baudrate";
            this.cBox_Baudrate.Size = new System.Drawing.Size(121, 24);
            this.cBox_Baudrate.TabIndex = 2;
            this.cBox_Baudrate.Text = "115200";
            // 
            // gBox_Serial
            // 
            this.gBox_Serial.Controls.Add(this.cBox_DtrEnable);
            this.gBox_Serial.Controls.Add(this.label_PortNo);
            this.gBox_Serial.Controls.Add(this.cBox_SerialPort);
            this.gBox_Serial.Controls.Add(this.label_Baudrate);
            this.gBox_Serial.Controls.Add(this.cBox_Baudrate);
            this.gBox_Serial.Location = new System.Drawing.Point(12, 144);
            this.gBox_Serial.Name = "gBox_Serial";
            this.gBox_Serial.Size = new System.Drawing.Size(224, 120);
            this.gBox_Serial.TabIndex = 5;
            this.gBox_Serial.TabStop = false;
            this.gBox_Serial.Text = "Serial port properties";
            this.gBox_Serial.TextChanged += new System.EventHandler(this.label_PortNo_TextChanged);
            // 
            // cBox_DtrEnable
            // 
            this.cBox_DtrEnable.Location = new System.Drawing.Point(18, 91);
            this.cBox_DtrEnable.Name = "cBox_DtrEnable";
            this.cBox_DtrEnable.Size = new System.Drawing.Size(194, 16);
            this.cBox_DtrEnable.TabIndex = 5;
            this.cBox_DtrEnable.Text = "Enable DTR (reset on connect)";
            this.cBox_DtrEnable.UseVisualStyleBackColor = true;
            this.cBox_DtrEnable.TextChanged += new System.EventHandler(this.label_PortNo_TextChanged);
            // 
            // button_Ok
            // 
            this.button_Ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_Ok.Location = new System.Drawing.Point(548, 406);
            this.button_Ok.Name = "button_Ok";
            this.button_Ok.Size = new System.Drawing.Size(75, 33);
            this.button_Ok.TabIndex = 6;
            this.button_Ok.Text = "&OK";
            this.button_Ok.UseVisualStyleBackColor = true;
            // 
            // gBox_DCC
            // 
            this.gBox_DCC.Controls.Add(this.cBox_DCC);
            this.gBox_DCC.Location = new System.Drawing.Point(253, 235);
            this.gBox_DCC.Name = "gBox_DCC";
            this.gBox_DCC.Size = new System.Drawing.Size(224, 45);
            this.gBox_DCC.TabIndex = 8;
            this.gBox_DCC.TabStop = false;
            this.gBox_DCC.Text = "DCC properties";
            this.gBox_DCC.TextChanged += new System.EventHandler(this.label_PortNo_TextChanged);
            // 
            // cBox_DCC
            // 
            this.cBox_DCC.Location = new System.Drawing.Point(15, 23);
            this.cBox_DCC.Name = "cBox_DCC";
            this.cBox_DCC.Size = new System.Drawing.Size(194, 16);
            this.cBox_DCC.TabIndex = 0;
            this.cBox_DCC.Text = "Use DCC accessories";
            this.cBox_DCC.UseVisualStyleBackColor = true;
            this.cBox_DCC.TextChanged += new System.EventHandler(this.label_PortNo_TextChanged);
            // 
            // cBox_MfxAutoRegister
            // 
            this.cBox_MfxAutoRegister.Location = new System.Drawing.Point(15, 26);
            this.cBox_MfxAutoRegister.Name = "cBox_MfxAutoRegister";
            this.cBox_MfxAutoRegister.Size = new System.Drawing.Size(198, 16);
            this.cBox_MfxAutoRegister.TabIndex = 1;
            this.cBox_MfxAutoRegister.Text = "Register mfx locs automatically";
            this.cBox_MfxAutoRegister.UseVisualStyleBackColor = true;
            this.cBox_MfxAutoRegister.TextChanged += new System.EventHandler(this.label_PortNo_TextChanged);
            // 
            // gBox_S88
            // 
            this.gBox_S88.Controls.Add(this.label_S88SensorNums);
            this.gBox_S88.Controls.Add(this.numUpDown_S88SensorNums);
            this.gBox_S88.Controls.Add(this.label_S88DetectInterval);
            this.gBox_S88.Controls.Add(this.numUpDown_S88DetectFreq);
            this.gBox_S88.Controls.Add(this.cBox_S88);
            this.gBox_S88.Location = new System.Drawing.Point(12, 277);
            this.gBox_S88.Name = "gBox_S88";
            this.gBox_S88.Size = new System.Drawing.Size(224, 111);
            this.gBox_S88.TabIndex = 9;
            this.gBox_S88.TabStop = false;
            this.gBox_S88.Text = "S88 sensor config";
            this.gBox_S88.TextChanged += new System.EventHandler(this.label_PortNo_TextChanged);
            // 
            // label_S88SensorNums
            // 
            this.label_S88SensorNums.Location = new System.Drawing.Point(16, 80);
            this.label_S88SensorNums.Name = "label_S88SensorNums";
            this.label_S88SensorNums.Size = new System.Drawing.Size(120, 12);
            this.label_S88SensorNums.TabIndex = 5;
            this.label_S88SensorNums.Text = "Number of connection";
            // 
            // numUpDown_S88SensorNums
            // 
            this.numUpDown_S88SensorNums.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.numUpDown_S88SensorNums.Location = new System.Drawing.Point(150, 75);
            this.numUpDown_S88SensorNums.Maximum = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.numUpDown_S88SensorNums.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numUpDown_S88SensorNums.Name = "numUpDown_S88SensorNums";
            this.numUpDown_S88SensorNums.Size = new System.Drawing.Size(54, 23);
            this.numUpDown_S88SensorNums.TabIndex = 4;
            this.numUpDown_S88SensorNums.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label_S88DetectInterval
            // 
            this.label_S88DetectInterval.Location = new System.Drawing.Point(16, 50);
            this.label_S88DetectInterval.Name = "label_S88DetectInterval";
            this.label_S88DetectInterval.Size = new System.Drawing.Size(120, 12);
            this.label_S88DetectInterval.TabIndex = 3;
            this.label_S88DetectInterval.Text = "Detection interval";
            this.label_S88DetectInterval.TextChanged += new System.EventHandler(this.label_PortNo_TextChanged);
            // 
            // numUpDown_S88DetectFreq
            // 
            this.numUpDown_S88DetectFreq.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.numUpDown_S88DetectFreq.Location = new System.Drawing.Point(150, 45);
            this.numUpDown_S88DetectFreq.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numUpDown_S88DetectFreq.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numUpDown_S88DetectFreq.Name = "numUpDown_S88DetectFreq";
            this.numUpDown_S88DetectFreq.Size = new System.Drawing.Size(54, 23);
            this.numUpDown_S88DetectFreq.TabIndex = 1;
            this.numUpDown_S88DetectFreq.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // cBox_S88
            // 
            this.cBox_S88.Location = new System.Drawing.Point(18, 23);
            this.cBox_S88.Name = "cBox_S88";
            this.cBox_S88.Size = new System.Drawing.Size(194, 16);
            this.cBox_S88.TabIndex = 0;
            this.cBox_S88.Text = "Use S88 detection";
            this.cBox_S88.UseVisualStyleBackColor = true;
            this.cBox_S88.TextChanged += new System.EventHandler(this.label_PortNo_TextChanged);
            // 
            // gBox_MultiScreen
            // 
            this.gBox_MultiScreen.Controls.Add(this.label_Screen4);
            this.gBox_MultiScreen.Controls.Add(this.cBox_Screen4Panel);
            this.gBox_MultiScreen.Controls.Add(this.label_Screen3);
            this.gBox_MultiScreen.Controls.Add(this.cBox_Screen3Panel);
            this.gBox_MultiScreen.Controls.Add(this.label_Screen2);
            this.gBox_MultiScreen.Controls.Add(this.cBox_Screen2Panel);
            this.gBox_MultiScreen.Controls.Add(this.label_Screen1);
            this.gBox_MultiScreen.Controls.Add(this.cBox_Screen1Panel);
            this.gBox_MultiScreen.Controls.Add(this.pictureBox1);
            this.gBox_MultiScreen.Controls.Add(this.label_MSNum);
            this.gBox_MultiScreen.Controls.Add(this.cBox_ScreenNums);
            this.gBox_MultiScreen.Location = new System.Drawing.Point(492, 12);
            this.gBox_MultiScreen.Name = "gBox_MultiScreen";
            this.gBox_MultiScreen.Size = new System.Drawing.Size(224, 258);
            this.gBox_MultiScreen.TabIndex = 10;
            this.gBox_MultiScreen.TabStop = false;
            this.gBox_MultiScreen.Text = "Multiple screens";
            this.gBox_MultiScreen.TextChanged += new System.EventHandler(this.label_PortNo_TextChanged);
            // 
            // label_Screen4
            // 
            this.label_Screen4.Location = new System.Drawing.Point(11, 154);
            this.label_Screen4.Name = "label_Screen4";
            this.label_Screen4.Size = new System.Drawing.Size(90, 12);
            this.label_Screen4.TabIndex = 14;
            this.label_Screen4.Text = "Screen 4";
            this.label_Screen4.TextChanged += new System.EventHandler(this.label_PortNo_TextChanged);
            // 
            // cBox_Screen4Panel
            // 
            this.cBox_Screen4Panel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBox_Screen4Panel.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cBox_Screen4Panel.FormattingEnabled = true;
            this.cBox_Screen4Panel.Items.AddRange(new object[] {
            "None",
            "Locomotive",
            "Multiple Locomotives",
            "6021 keypad",
            "Accessories",
            "Track layout",
            "Learning",
            "Event",
            "Console"});
            this.cBox_Screen4Panel.Location = new System.Drawing.Point(109, 148);
            this.cBox_Screen4Panel.Name = "cBox_Screen4Panel";
            this.cBox_Screen4Panel.Size = new System.Drawing.Size(100, 21);
            this.cBox_Screen4Panel.TabIndex = 13;
            this.cBox_Screen4Panel.SelectedIndexChanged += new System.EventHandler(this.cBox_Screen1Panel_SelectedIndexChanged);
            // 
            // label_Screen3
            // 
            this.label_Screen3.Location = new System.Drawing.Point(11, 121);
            this.label_Screen3.Name = "label_Screen3";
            this.label_Screen3.Size = new System.Drawing.Size(90, 12);
            this.label_Screen3.TabIndex = 12;
            this.label_Screen3.Text = "Screen 3";
            this.label_Screen3.TextChanged += new System.EventHandler(this.label_PortNo_TextChanged);
            // 
            // cBox_Screen3Panel
            // 
            this.cBox_Screen3Panel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBox_Screen3Panel.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cBox_Screen3Panel.FormattingEnabled = true;
            this.cBox_Screen3Panel.Items.AddRange(new object[] {
            "None",
            "Locomotive",
            "Multiple Locomotives",
            "6021 keypad",
            "Accessories",
            "Track layout",
            "Learning",
            "Event",
            "Console"});
            this.cBox_Screen3Panel.Location = new System.Drawing.Point(109, 117);
            this.cBox_Screen3Panel.Name = "cBox_Screen3Panel";
            this.cBox_Screen3Panel.Size = new System.Drawing.Size(100, 21);
            this.cBox_Screen3Panel.TabIndex = 11;
            this.cBox_Screen3Panel.SelectedIndexChanged += new System.EventHandler(this.cBox_Screen1Panel_SelectedIndexChanged);
            // 
            // label_Screen2
            // 
            this.label_Screen2.Location = new System.Drawing.Point(11, 89);
            this.label_Screen2.Name = "label_Screen2";
            this.label_Screen2.Size = new System.Drawing.Size(90, 12);
            this.label_Screen2.TabIndex = 10;
            this.label_Screen2.Text = "Screen 2";
            this.label_Screen2.TextChanged += new System.EventHandler(this.label_PortNo_TextChanged);
            // 
            // cBox_Screen2Panel
            // 
            this.cBox_Screen2Panel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBox_Screen2Panel.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cBox_Screen2Panel.FormattingEnabled = true;
            this.cBox_Screen2Panel.Items.AddRange(new object[] {
            "None",
            "Locomotive",
            "Multiple Locomotives",
            "6021 keypad",
            "Accessories",
            "Track layout",
            "Learning",
            "Event",
            "Console"});
            this.cBox_Screen2Panel.Location = new System.Drawing.Point(109, 85);
            this.cBox_Screen2Panel.Name = "cBox_Screen2Panel";
            this.cBox_Screen2Panel.Size = new System.Drawing.Size(100, 21);
            this.cBox_Screen2Panel.TabIndex = 9;
            this.cBox_Screen2Panel.SelectedIndexChanged += new System.EventHandler(this.cBox_Screen1Panel_SelectedIndexChanged);
            // 
            // label_Screen1
            // 
            this.label_Screen1.Location = new System.Drawing.Point(11, 57);
            this.label_Screen1.Name = "label_Screen1";
            this.label_Screen1.Size = new System.Drawing.Size(90, 12);
            this.label_Screen1.TabIndex = 8;
            this.label_Screen1.Text = "Screen 1";
            this.label_Screen1.TextChanged += new System.EventHandler(this.label_PortNo_TextChanged);
            // 
            // cBox_Screen1Panel
            // 
            this.cBox_Screen1Panel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBox_Screen1Panel.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cBox_Screen1Panel.FormattingEnabled = true;
            this.cBox_Screen1Panel.Items.AddRange(new object[] {
            "None",
            "Locomotive",
            "Multiple Locomotives",
            "6021 keypad",
            "Accessories",
            "Track layout",
            "Learning",
            "Event",
            "Console"});
            this.cBox_Screen1Panel.Location = new System.Drawing.Point(109, 53);
            this.cBox_Screen1Panel.Name = "cBox_Screen1Panel";
            this.cBox_Screen1Panel.Size = new System.Drawing.Size(100, 21);
            this.cBox_Screen1Panel.TabIndex = 7;
            this.cBox_Screen1Panel.SelectedIndexChanged += new System.EventHandler(this.cBox_Screen1Panel_SelectedIndexChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(28, 183);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(160, 62);
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // label_MSNum
            // 
            this.label_MSNum.Location = new System.Drawing.Point(11, 24);
            this.label_MSNum.Name = "label_MSNum";
            this.label_MSNum.Size = new System.Drawing.Size(90, 12);
            this.label_MSNum.TabIndex = 5;
            this.label_MSNum.Text = "Screen numbers";
            this.label_MSNum.TextChanged += new System.EventHandler(this.label_PortNo_TextChanged);
            // 
            // cBox_ScreenNums
            // 
            this.cBox_ScreenNums.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBox_ScreenNums.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cBox_ScreenNums.FormattingEnabled = true;
            this.cBox_ScreenNums.Items.AddRange(new object[] {
            "None",
            "2H",
            "2V",
            "4"});
            this.cBox_ScreenNums.Location = new System.Drawing.Point(109, 20);
            this.cBox_ScreenNums.Name = "cBox_ScreenNums";
            this.cBox_ScreenNums.Size = new System.Drawing.Size(100, 21);
            this.cBox_ScreenNums.TabIndex = 0;
            this.cBox_ScreenNums.SelectedIndexChanged += new System.EventHandler(this.cBox_ScreenNums_SelectedIndexChanged);
            // 
            // gBox_mfx
            // 
            this.gBox_mfx.Controls.Add(this.cBox_MfxAutoUpdate);
            this.gBox_mfx.Controls.Add(this.cBox_MfxAutoRegister);
            this.gBox_mfx.Location = new System.Drawing.Point(253, 150);
            this.gBox_mfx.Name = "gBox_mfx";
            this.gBox_mfx.Size = new System.Drawing.Size(224, 79);
            this.gBox_mfx.TabIndex = 11;
            this.gBox_mfx.TabStop = false;
            this.gBox_mfx.Text = "mfx auto recognization";
            this.gBox_mfx.TextChanged += new System.EventHandler(this.label_PortNo_TextChanged);
            // 
            // cBox_MfxAutoUpdate
            // 
            this.cBox_MfxAutoUpdate.Location = new System.Drawing.Point(15, 50);
            this.cBox_MfxAutoUpdate.Name = "cBox_MfxAutoUpdate";
            this.cBox_MfxAutoUpdate.Size = new System.Drawing.Size(198, 16);
            this.cBox_MfxAutoUpdate.TabIndex = 2;
            this.cBox_MfxAutoUpdate.Text = "Update mfx locs automatically";
            this.cBox_MfxAutoUpdate.UseVisualStyleBackColor = true;
            this.cBox_MfxAutoUpdate.TextChanged += new System.EventHandler(this.label_PortNo_TextChanged);
            // 
            // gBox_AddVisible
            // 
            this.gBox_AddVisible.Controls.Add(this.numUpDown_WindowZoom);
            this.gBox_AddVisible.Controls.Add(this.label_WindowZoom);
            this.gBox_AddVisible.Controls.Add(this.label_VisibleBottom);
            this.gBox_AddVisible.Controls.Add(this.cBox_SideFuncBottom);
            this.gBox_AddVisible.Controls.Add(this.label_VisibleRight);
            this.gBox_AddVisible.Controls.Add(this.cBox_SideFuncRight);
            this.gBox_AddVisible.Location = new System.Drawing.Point(253, 12);
            this.gBox_AddVisible.Name = "gBox_AddVisible";
            this.gBox_AddVisible.Size = new System.Drawing.Size(224, 132);
            this.gBox_AddVisible.TabIndex = 12;
            this.gBox_AddVisible.TabStop = false;
            this.gBox_AddVisible.Text = "Additional window properties";
            this.gBox_AddVisible.TextChanged += new System.EventHandler(this.label_PortNo_TextChanged);
            // 
            // numUpDown_WindowZoom
            // 
            this.numUpDown_WindowZoom.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.numUpDown_WindowZoom.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numUpDown_WindowZoom.Location = new System.Drawing.Point(111, 93);
            this.numUpDown_WindowZoom.Maximum = new decimal(new int[] {
            350,
            0,
            0,
            0});
            this.numUpDown_WindowZoom.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numUpDown_WindowZoom.Name = "numUpDown_WindowZoom";
            this.numUpDown_WindowZoom.Size = new System.Drawing.Size(100, 23);
            this.numUpDown_WindowZoom.TabIndex = 13;
            this.numUpDown_WindowZoom.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label_WindowZoom
            // 
            this.label_WindowZoom.Location = new System.Drawing.Point(13, 98);
            this.label_WindowZoom.Name = "label_WindowZoom";
            this.label_WindowZoom.Size = new System.Drawing.Size(90, 12);
            this.label_WindowZoom.TabIndex = 12;
            this.label_WindowZoom.Text = "Window Zoom";
            // 
            // label_VisibleBottom
            // 
            this.label_VisibleBottom.Location = new System.Drawing.Point(13, 65);
            this.label_VisibleBottom.Name = "label_VisibleBottom";
            this.label_VisibleBottom.Size = new System.Drawing.Size(90, 12);
            this.label_VisibleBottom.TabIndex = 11;
            this.label_VisibleBottom.Text = "Bottom side";
            this.label_VisibleBottom.TextChanged += new System.EventHandler(this.label_PortNo_TextChanged);
            // 
            // cBox_SideFuncBottom
            // 
            this.cBox_SideFuncBottom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBox_SideFuncBottom.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cBox_SideFuncBottom.FormattingEnabled = true;
            this.cBox_SideFuncBottom.Items.AddRange(new object[] {
            "None",
            "Accessories",
            "S88 sensors"});
            this.cBox_SideFuncBottom.Location = new System.Drawing.Point(111, 62);
            this.cBox_SideFuncBottom.Name = "cBox_SideFuncBottom";
            this.cBox_SideFuncBottom.Size = new System.Drawing.Size(100, 21);
            this.cBox_SideFuncBottom.TabIndex = 10;
            // 
            // label_VisibleRight
            // 
            this.label_VisibleRight.Location = new System.Drawing.Point(13, 33);
            this.label_VisibleRight.Name = "label_VisibleRight";
            this.label_VisibleRight.Size = new System.Drawing.Size(90, 12);
            this.label_VisibleRight.TabIndex = 9;
            this.label_VisibleRight.Text = "Right side";
            this.label_VisibleRight.TextChanged += new System.EventHandler(this.label_PortNo_TextChanged);
            // 
            // cBox_SideFuncRight
            // 
            this.cBox_SideFuncRight.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBox_SideFuncRight.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cBox_SideFuncRight.FormattingEnabled = true;
            this.cBox_SideFuncRight.Items.AddRange(new object[] {
            "None",
            "Clock",
            "EMG button"});
            this.cBox_SideFuncRight.Location = new System.Drawing.Point(111, 30);
            this.cBox_SideFuncRight.Name = "cBox_SideFuncRight";
            this.cBox_SideFuncRight.Size = new System.Drawing.Size(100, 21);
            this.cBox_SideFuncRight.TabIndex = 8;
            // 
            // gBox_Other
            // 
            this.gBox_Other.Controls.Add(this.cBox_ClearAcc);
            this.gBox_Other.Controls.Add(this.cBox_AutoCloseSerial);
            this.gBox_Other.Controls.Add(this.cBox_StopAllLocPon);
            this.gBox_Other.Location = new System.Drawing.Point(253, 287);
            this.gBox_Other.Name = "gBox_Other";
            this.gBox_Other.Size = new System.Drawing.Size(224, 101);
            this.gBox_Other.TabIndex = 13;
            this.gBox_Other.TabStop = false;
            this.gBox_Other.Text = "Other properties";
            this.gBox_Other.TextChanged += new System.EventHandler(this.label_PortNo_TextChanged);
            // 
            // cBox_ClearAcc
            // 
            this.cBox_ClearAcc.Location = new System.Drawing.Point(15, 70);
            this.cBox_ClearAcc.Name = "cBox_ClearAcc";
            this.cBox_ClearAcc.Size = new System.Drawing.Size(194, 16);
            this.cBox_ClearAcc.TabIndex = 2;
            this.cBox_ClearAcc.Text = "Clear accessories status on boot";
            this.cBox_ClearAcc.UseVisualStyleBackColor = true;
            // 
            // cBox_AutoCloseSerial
            // 
            this.cBox_AutoCloseSerial.Location = new System.Drawing.Point(15, 48);
            this.cBox_AutoCloseSerial.Name = "cBox_AutoCloseSerial";
            this.cBox_AutoCloseSerial.Size = new System.Drawing.Size(194, 16);
            this.cBox_AutoCloseSerial.TabIndex = 1;
            this.cBox_AutoCloseSerial.Text = "Close serialport in case of STOP";
            this.cBox_AutoCloseSerial.UseVisualStyleBackColor = true;
            // 
            // cBox_StopAllLocPon
            // 
            this.cBox_StopAllLocPon.Location = new System.Drawing.Point(15, 26);
            this.cBox_StopAllLocPon.Name = "cBox_StopAllLocPon";
            this.cBox_StopAllLocPon.Size = new System.Drawing.Size(194, 16);
            this.cBox_StopAllLocPon.TabIndex = 0;
            this.cBox_StopAllLocPon.Text = "Stop all locomotives on power on";
            this.cBox_StopAllLocPon.UseVisualStyleBackColor = true;
            this.cBox_StopAllLocPon.TextChanged += new System.EventHandler(this.label_PortNo_TextChanged);
            // 
            // gBox_Language
            // 
            this.gBox_Language.Controls.Add(this.label_LanguageDescription);
            this.gBox_Language.Controls.Add(this.cBox_LanguageFile);
            this.gBox_Language.Location = new System.Drawing.Point(492, 287);
            this.gBox_Language.Name = "gBox_Language";
            this.gBox_Language.Size = new System.Drawing.Size(224, 101);
            this.gBox_Language.TabIndex = 14;
            this.gBox_Language.TabStop = false;
            this.gBox_Language.Text = "Language properties";
            this.gBox_Language.TextChanged += new System.EventHandler(this.label_PortNo_TextChanged);
            // 
            // label_LanguageDescription
            // 
            this.label_LanguageDescription.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label_LanguageDescription.Location = new System.Drawing.Point(11, 59);
            this.label_LanguageDescription.Name = "label_LanguageDescription";
            this.label_LanguageDescription.Size = new System.Drawing.Size(203, 27);
            this.label_LanguageDescription.TabIndex = 1;
            this.label_LanguageDescription.Text = "Language changes will be applied after the application has been restarted.";
            this.label_LanguageDescription.TextChanged += new System.EventHandler(this.label_PortNo_TextChanged);
            // 
            // cBox_LanguageFile
            // 
            this.cBox_LanguageFile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBox_LanguageFile.FormattingEnabled = true;
            this.cBox_LanguageFile.Location = new System.Drawing.Point(29, 30);
            this.cBox_LanguageFile.Name = "cBox_LanguageFile";
            this.cBox_LanguageFile.Size = new System.Drawing.Size(173, 20);
            this.cBox_LanguageFile.TabIndex = 0;
            // 
            // cBox_ConnectionType
            // 
            this.cBox_ConnectionType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBox_ConnectionType.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cBox_ConnectionType.FormattingEnabled = true;
            this.cBox_ConnectionType.Items.AddRange(new object[] {
            "Serial port",
            "HTTP/Web",
            "Emulation"});
            this.cBox_ConnectionType.Location = new System.Drawing.Point(117, 23);
            this.cBox_ConnectionType.Name = "cBox_ConnectionType";
            this.cBox_ConnectionType.Size = new System.Drawing.Size(107, 21);
            this.cBox_ConnectionType.TabIndex = 9;
            this.cBox_ConnectionType.SelectedIndexChanged += new System.EventHandler(this.cBox_ConnectionType_SelectedIndexChanged);
            // 
            // labelConnectType
            // 
            this.labelConnectType.Location = new System.Drawing.Point(12, 27);
            this.labelConnectType.Name = "labelConnectType";
            this.labelConnectType.Size = new System.Drawing.Size(91, 17);
            this.labelConnectType.TabIndex = 10;
            this.labelConnectType.Text = "Connection type";
            // 
            // gBox_Http
            // 
            this.gBox_Http.Controls.Add(this.label_IPAddr);
            this.gBox_Http.Controls.Add(this.cBox_IPAddress);
            this.gBox_Http.Location = new System.Drawing.Point(12, 61);
            this.gBox_Http.Name = "gBox_Http";
            this.gBox_Http.Size = new System.Drawing.Size(224, 67);
            this.gBox_Http.TabIndex = 15;
            this.gBox_Http.TabStop = false;
            this.gBox_Http.Text = "HTTP/web properties";
            // 
            // label_IPAddr
            // 
            this.label_IPAddr.Location = new System.Drawing.Point(16, 30);
            this.label_IPAddr.Name = "label_IPAddr";
            this.label_IPAddr.Size = new System.Drawing.Size(59, 12);
            this.label_IPAddr.TabIndex = 4;
            this.label_IPAddr.Text = "IP address";
            // 
            // cBox_IPAddress
            // 
            this.cBox_IPAddress.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cBox_IPAddress.FormattingEnabled = true;
            this.cBox_IPAddress.Location = new System.Drawing.Point(81, 26);
            this.cBox_IPAddress.Name = "cBox_IPAddress";
            this.cBox_IPAddress.Size = new System.Drawing.Size(121, 24);
            this.cBox_IPAddress.TabIndex = 1;
            // 
            // SerialConfigForm
            // 
            this.AcceptButton = this.button_Ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button_Cancel;
            this.ClientSize = new System.Drawing.Size(728, 451);
            this.Controls.Add(this.gBox_Http);
            this.Controls.Add(this.labelConnectType);
            this.Controls.Add(this.cBox_ConnectionType);
            this.Controls.Add(this.gBox_Language);
            this.Controls.Add(this.gBox_Other);
            this.Controls.Add(this.gBox_AddVisible);
            this.Controls.Add(this.gBox_mfx);
            this.Controls.Add(this.gBox_MultiScreen);
            this.Controls.Add(this.gBox_S88);
            this.Controls.Add(this.gBox_DCC);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.gBox_Serial);
            this.Controls.Add(this.button_Ok);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SerialConfigForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.gBox_Serial.ResumeLayout(false);
            this.gBox_DCC.ResumeLayout(false);
            this.gBox_S88.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown_S88SensorNums)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown_S88DetectFreq)).EndInit();
            this.gBox_MultiScreen.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.gBox_mfx.ResumeLayout(false);
            this.gBox_AddVisible.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown_WindowZoom)).EndInit();
            this.gBox_Other.ResumeLayout(false);
            this.gBox_Language.ResumeLayout(false);
            this.gBox_Http.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label_PortNo;
        private System.Windows.Forms.Label label_Baudrate;
        public System.Windows.Forms.ComboBox cBox_SerialPort;
        private System.Windows.Forms.Button button_Cancel;
        public System.Windows.Forms.ComboBox cBox_Baudrate;
        private System.Windows.Forms.Button button_Ok;
        private System.Windows.Forms.GroupBox gBox_DCC;
        public System.Windows.Forms.CheckBox cBox_DCC;
        private System.Windows.Forms.GroupBox gBox_S88;
        public System.Windows.Forms.CheckBox cBox_S88;
        private System.Windows.Forms.GroupBox gBox_MultiScreen;
        private System.Windows.Forms.Label label_MSNum;
        public System.Windows.Forms.ComboBox cBox_ScreenNums;
        private System.Windows.Forms.Label label_Screen1;
        public System.Windows.Forms.ComboBox cBox_Screen1Panel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label_Screen4;
        public System.Windows.Forms.ComboBox cBox_Screen4Panel;
        private System.Windows.Forms.Label label_Screen3;
        public System.Windows.Forms.ComboBox cBox_Screen3Panel;
        private System.Windows.Forms.Label label_Screen2;
        public System.Windows.Forms.ComboBox cBox_Screen2Panel;
        public System.Windows.Forms.CheckBox cBox_MfxAutoRegister;
        private System.Windows.Forms.GroupBox gBox_mfx;
        public System.Windows.Forms.CheckBox cBox_MfxAutoUpdate;
        private System.Windows.Forms.Label label_S88DetectInterval;
        public System.Windows.Forms.NumericUpDown numUpDown_S88DetectFreq;
        private System.Windows.Forms.GroupBox gBox_AddVisible;
        private System.Windows.Forms.Label label_VisibleBottom;
        public System.Windows.Forms.ComboBox cBox_SideFuncBottom;
        private System.Windows.Forms.Label label_VisibleRight;
        public System.Windows.Forms.ComboBox cBox_SideFuncRight;
        private System.Windows.Forms.GroupBox gBox_Other;
        public System.Windows.Forms.CheckBox cBox_StopAllLocPon;
        public System.Windows.Forms.CheckBox cBox_DtrEnable;
        private System.Windows.Forms.GroupBox gBox_Language;
        public System.Windows.Forms.ComboBox cBox_LanguageFile;
        private System.Windows.Forms.Label label_LanguageDescription;
        private System.Windows.Forms.Label label_S88SensorNums;
        public System.Windows.Forms.NumericUpDown numUpDown_S88SensorNums;
        public System.Windows.Forms.CheckBox cBox_AutoCloseSerial;
        public System.Windows.Forms.CheckBox cBox_ClearAcc;
        public System.Windows.Forms.ComboBox cBox_ConnectionType;
        private System.Windows.Forms.Label labelConnectType;
        private System.Windows.Forms.Label label_IPAddr;
        public System.Windows.Forms.ComboBox cBox_IPAddress;
        public System.Windows.Forms.GroupBox gBox_Serial;
        public System.Windows.Forms.GroupBox gBox_Http;
        public System.Windows.Forms.NumericUpDown numUpDown_WindowZoom;
        public System.Windows.Forms.Label label_WindowZoom;
    }
}
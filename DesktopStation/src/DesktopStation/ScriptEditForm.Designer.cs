namespace DesktopStation
{
    partial class ScriptEditForm
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
            this.tabLocos = new System.Windows.Forms.TabPage();
            this.panel_ScriptLocSpeed = new System.Windows.Forms.Panel();
            this.numUpDown_TransitionTime = new System.Windows.Forms.NumericUpDown();
            this.label_TransitionTime = new System.Windows.Forms.Label();
            this.cBox_AddrReplacedLocSpd = new System.Windows.Forms.CheckBox();
            this.cBox_ProtcolLocSpd = new System.Windows.Forms.ComboBox();
            this.numericSpeed = new System.Windows.Forms.NumericUpDown();
            this.label_Speed = new System.Windows.Forms.Label();
            this.label_LocAddr = new System.Windows.Forms.Label();
            this.cBox_LocAddressSpd = new System.Windows.Forms.ComboBox();
            this.tabAcc = new System.Windows.Forms.TabPage();
            this.panel_ScriptAcc = new System.Windows.Forms.Panel();
            this.numericAccAddress = new System.Windows.Forms.NumericUpDown();
            this.label_AccSwitch = new System.Windows.Forms.Label();
            this.cBox_AccPower = new System.Windows.Forms.ComboBox();
            this.label_AccAddr = new System.Windows.Forms.Label();
            this.tabWait = new System.Windows.Forms.TabPage();
            this.panel_ScriptWait = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.label_WaitTime = new System.Windows.Forms.Label();
            this.numericWaitTime = new System.Windows.Forms.NumericUpDown();
            this.button_Ok = new System.Windows.Forms.Button();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabLocoFunc = new System.Windows.Forms.TabPage();
            this.panel_ScriptLocFunc = new System.Windows.Forms.Panel();
            this.cBox_AddrReplacedLocFnc = new System.Windows.Forms.CheckBox();
            this.cBox_ProtcolLocFnc = new System.Windows.Forms.ComboBox();
            this.label_FncOnOff = new System.Windows.Forms.Label();
            this.cBox_FunctionPower = new System.Windows.Forms.ComboBox();
            this.label_FncNo = new System.Windows.Forms.Label();
            this.cBox_FunctionNo = new System.Windows.Forms.ComboBox();
            this.label_LocAddr2 = new System.Windows.Forms.Label();
            this.cBox_LocAddressFnc = new System.Windows.Forms.ComboBox();
            this.tabLocoDir = new System.Windows.Forms.TabPage();
            this.panel_ScriptDirect = new System.Windows.Forms.Panel();
            this.cBox_AddrReplacedLocDir = new System.Windows.Forms.CheckBox();
            this.cBox_ProtcolLocDir = new System.Windows.Forms.ComboBox();
            this.cBox_Direction = new System.Windows.Forms.ComboBox();
            this.label_LocDirection = new System.Windows.Forms.Label();
            this.label_LocAddr3 = new System.Windows.Forms.Label();
            this.cBox_LocAddressDir = new System.Windows.Forms.ComboBox();
            this.tabPower = new System.Windows.Forms.TabPage();
            this.panel_ScriptPwr = new System.Windows.Forms.Panel();
            this.label_Power = new System.Windows.Forms.Label();
            this.cBox_Power = new System.Windows.Forms.ComboBox();
            this.tabLine = new System.Windows.Forms.TabPage();
            this.panel_ScriptLine = new System.Windows.Forms.Panel();
            this.cBox_LineNoLabel = new System.Windows.Forms.ComboBox();
            this.label_LineNum = new System.Windows.Forms.Label();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel_LineLabel = new System.Windows.Forms.Panel();
            this.cBox_LineLabel = new System.Windows.Forms.ComboBox();
            this.label_LabelName = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel_Jump = new System.Windows.Forms.Panel();
            this.tBox_JumpFlagNo = new System.Windows.Forms.TextBox();
            this.label_EquivVal = new System.Windows.Forms.Label();
            this.numUpDown_JumpValue = new System.Windows.Forms.NumericUpDown();
            this.label_FlagNo = new System.Windows.Forms.Label();
            this.cBox_JumpLabelName = new System.Windows.Forms.ComboBox();
            this.label_LabelNameJmp = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.panel_SetFlag = new System.Windows.Forms.Panel();
            this.tBox_SetFlagNo = new System.Windows.Forms.TextBox();
            this.label_Value = new System.Windows.Forms.Label();
            this.numUpDown_SetFlagValue = new System.Windows.Forms.NumericUpDown();
            this.label_FlagNo2 = new System.Windows.Forms.Label();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.panel_RunFile = new System.Windows.Forms.Panel();
            this.button_OpenTarget = new System.Windows.Forms.Button();
            this.textBox_TargetFile = new System.Windows.Forms.TextBox();
            this.label_TargetFile = new System.Windows.Forms.Label();
            this.label_AppName = new System.Windows.Forms.Label();
            this.cBox_Appname = new System.Windows.Forms.ComboBox();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.panel_Free = new System.Windows.Forms.Panel();
            this.textBox_FreeCommand = new System.Windows.Forms.TextBox();
            this.label_FreeCommand = new System.Windows.Forms.Label();
            this.label_FreeDescription = new System.Windows.Forms.Label();
            this.tabJumpRun = new System.Windows.Forms.TabPage();
            this.panelJumpRun = new System.Windows.Forms.Panel();
            this.labelJumpRunEqualVal = new System.Windows.Forms.Label();
            this.numUpDownJumpRun = new System.Windows.Forms.NumericUpDown();
            this.cBox_JumpRunLabel = new System.Windows.Forms.ComboBox();
            this.labelJumpRunlabel = new System.Windows.Forms.Label();
            this.cBox_AddrReplacedLocJump = new System.Windows.Forms.CheckBox();
            this.cBox_JumpRunLocProt = new System.Windows.Forms.ComboBox();
            this.labelJumpRunLocAddr = new System.Windows.Forms.Label();
            this.cBox_JumpRunLocAddr = new System.Windows.Forms.ComboBox();
            this.tabRoute = new System.Windows.Forms.TabPage();
            this.panelRoute = new System.Windows.Forms.Panel();
            this.cBoxRouteState = new System.Windows.Forms.ComboBox();
            this.label_RuteState = new System.Windows.Forms.Label();
            this.cBox_Route = new System.Windows.Forms.ComboBox();
            this.labelRoutename = new System.Windows.Forms.Label();
            this.cBox_Routelabel = new System.Windows.Forms.ComboBox();
            this.labeRouteLabel = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.comboBox5 = new System.Windows.Forms.ComboBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.comboBox6 = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.comboBox7 = new System.Windows.Forms.ComboBox();
            this.comboBox8 = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.comboBox9 = new System.Windows.Forms.ComboBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.comboBox10 = new System.Windows.Forms.ComboBox();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.label15 = new System.Windows.Forms.Label();
            this.comboBox11 = new System.Windows.Forms.ComboBox();
            this.label16 = new System.Windows.Forms.Label();
            this.comboBox12 = new System.Windows.Forms.ComboBox();
            this.comboBox13 = new System.Windows.Forms.ComboBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.comboBox14 = new System.Windows.Forms.ComboBox();
            this.panel_ScriptOuter = new System.Windows.Forms.Panel();
            this.tabLocos.SuspendLayout();
            this.panel_ScriptLocSpeed.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown_TransitionTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericSpeed)).BeginInit();
            this.tabAcc.SuspendLayout();
            this.panel_ScriptAcc.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericAccAddress)).BeginInit();
            this.tabWait.SuspendLayout();
            this.panel_ScriptWait.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericWaitTime)).BeginInit();
            this.tabControl.SuspendLayout();
            this.tabLocoFunc.SuspendLayout();
            this.panel_ScriptLocFunc.SuspendLayout();
            this.tabLocoDir.SuspendLayout();
            this.panel_ScriptDirect.SuspendLayout();
            this.tabPower.SuspendLayout();
            this.panel_ScriptPwr.SuspendLayout();
            this.tabLine.SuspendLayout();
            this.panel_ScriptLine.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel_LineLabel.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel_Jump.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown_JumpValue)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.panel_SetFlag.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown_SetFlagValue)).BeginInit();
            this.tabPage4.SuspendLayout();
            this.panel_RunFile.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.panel_Free.SuspendLayout();
            this.tabJumpRun.SuspendLayout();
            this.panelJumpRun.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownJumpRun)).BeginInit();
            this.tabRoute.SuspendLayout();
            this.panelRoute.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            this.SuspendLayout();
            // 
            // button_Cancel
            // 
            this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Cancel.Location = new System.Drawing.Point(212, 164);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(75, 27);
            this.button_Cancel.TabIndex = 15;
            this.button_Cancel.Text = "&Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            // 
            // tabLocos
            // 
            this.tabLocos.Controls.Add(this.panel_ScriptLocSpeed);
            this.tabLocos.Location = new System.Drawing.Point(4, 22);
            this.tabLocos.Name = "tabLocos";
            this.tabLocos.Padding = new System.Windows.Forms.Padding(3);
            this.tabLocos.Size = new System.Drawing.Size(285, 146);
            this.tabLocos.TabIndex = 1;
            this.tabLocos.Text = "LocoSpeed";
            this.tabLocos.UseVisualStyleBackColor = true;
            // 
            // panel_ScriptLocSpeed
            // 
            this.panel_ScriptLocSpeed.Controls.Add(this.numUpDown_TransitionTime);
            this.panel_ScriptLocSpeed.Controls.Add(this.label_TransitionTime);
            this.panel_ScriptLocSpeed.Controls.Add(this.cBox_AddrReplacedLocSpd);
            this.panel_ScriptLocSpeed.Controls.Add(this.cBox_ProtcolLocSpd);
            this.panel_ScriptLocSpeed.Controls.Add(this.numericSpeed);
            this.panel_ScriptLocSpeed.Controls.Add(this.label_Speed);
            this.panel_ScriptLocSpeed.Controls.Add(this.label_LocAddr);
            this.panel_ScriptLocSpeed.Controls.Add(this.cBox_LocAddressSpd);
            this.panel_ScriptLocSpeed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_ScriptLocSpeed.Location = new System.Drawing.Point(3, 3);
            this.panel_ScriptLocSpeed.Name = "panel_ScriptLocSpeed";
            this.panel_ScriptLocSpeed.Size = new System.Drawing.Size(279, 140);
            this.panel_ScriptLocSpeed.TabIndex = 42;
            // 
            // numUpDown_TransitionTime
            // 
            this.numUpDown_TransitionTime.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.numUpDown_TransitionTime.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numUpDown_TransitionTime.Location = new System.Drawing.Point(134, 103);
            this.numUpDown_TransitionTime.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.numUpDown_TransitionTime.Name = "numUpDown_TransitionTime";
            this.numUpDown_TransitionTime.Size = new System.Drawing.Size(134, 24);
            this.numUpDown_TransitionTime.TabIndex = 49;
            // 
            // label_TransitionTime
            // 
            this.label_TransitionTime.AutoSize = true;
            this.label_TransitionTime.Location = new System.Drawing.Point(8, 108);
            this.label_TransitionTime.Name = "label_TransitionTime";
            this.label_TransitionTime.Size = new System.Drawing.Size(113, 12);
            this.label_TransitionTime.TabIndex = 48;
            this.label_TransitionTime.Text = "Transition Time[0.1s]";
            // 
            // cBox_AddrReplacedLocSpd
            // 
            this.cBox_AddrReplacedLocSpd.AutoSize = true;
            this.cBox_AddrReplacedLocSpd.Location = new System.Drawing.Point(134, 46);
            this.cBox_AddrReplacedLocSpd.Name = "cBox_AddrReplacedLocSpd";
            this.cBox_AddrReplacedLocSpd.Size = new System.Drawing.Size(71, 16);
            this.cBox_AddrReplacedLocSpd.TabIndex = 47;
            this.cBox_AddrReplacedLocSpd.Text = "Replaced";
            this.cBox_AddrReplacedLocSpd.UseVisualStyleBackColor = true;
            this.cBox_AddrReplacedLocSpd.CheckedChanged += new System.EventHandler(this.cBox_AddrDefinedLocSpd_CheckedChanged);
            // 
            // cBox_ProtcolLocSpd
            // 
            this.cBox_ProtcolLocSpd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBox_ProtcolLocSpd.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cBox_ProtcolLocSpd.FormattingEnabled = true;
            this.cBox_ProtcolLocSpd.Items.AddRange(new object[] {
            "MM2",
            "mfx",
            "DCC"});
            this.cBox_ProtcolLocSpd.Location = new System.Drawing.Point(211, 41);
            this.cBox_ProtcolLocSpd.Name = "cBox_ProtcolLocSpd";
            this.cBox_ProtcolLocSpd.Size = new System.Drawing.Size(57, 23);
            this.cBox_ProtcolLocSpd.TabIndex = 46;
            // 
            // numericSpeed
            // 
            this.numericSpeed.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.numericSpeed.Increment = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.numericSpeed.Location = new System.Drawing.Point(134, 71);
            this.numericSpeed.Maximum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.numericSpeed.Name = "numericSpeed";
            this.numericSpeed.Size = new System.Drawing.Size(134, 24);
            this.numericSpeed.TabIndex = 45;
            // 
            // label_Speed
            // 
            this.label_Speed.AutoSize = true;
            this.label_Speed.Location = new System.Drawing.Point(8, 76);
            this.label_Speed.Name = "label_Speed";
            this.label_Speed.Size = new System.Drawing.Size(80, 12);
            this.label_Speed.TabIndex = 44;
            this.label_Speed.Text = "Speed(0-1024)";
            // 
            // label_LocAddr
            // 
            this.label_LocAddr.AutoSize = true;
            this.label_LocAddr.Location = new System.Drawing.Point(8, 18);
            this.label_LocAddr.Name = "label_LocAddr";
            this.label_LocAddr.Size = new System.Drawing.Size(69, 12);
            this.label_LocAddr.TabIndex = 43;
            this.label_LocAddr.Text = "Loc Address";
            // 
            // cBox_LocAddressSpd
            // 
            this.cBox_LocAddressSpd.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cBox_LocAddressSpd.FormattingEnabled = true;
            this.cBox_LocAddressSpd.Items.AddRange(new object[] {
            "SLOT.A",
            "SLOT.B",
            "SLOT.C",
            "SLOT.D",
            "SLOT.E",
            "SLOT.F",
            "SLOT.G",
            "SLOT.H"});
            this.cBox_LocAddressSpd.Location = new System.Drawing.Point(134, 12);
            this.cBox_LocAddressSpd.Name = "cBox_LocAddressSpd";
            this.cBox_LocAddressSpd.Size = new System.Drawing.Size(134, 23);
            this.cBox_LocAddressSpd.TabIndex = 42;
            // 
            // tabAcc
            // 
            this.tabAcc.Controls.Add(this.panel_ScriptAcc);
            this.tabAcc.Location = new System.Drawing.Point(4, 22);
            this.tabAcc.Name = "tabAcc";
            this.tabAcc.Size = new System.Drawing.Size(285, 146);
            this.tabAcc.TabIndex = 3;
            this.tabAcc.Text = "Accessory";
            this.tabAcc.UseVisualStyleBackColor = true;
            // 
            // panel_ScriptAcc
            // 
            this.panel_ScriptAcc.Controls.Add(this.numericAccAddress);
            this.panel_ScriptAcc.Controls.Add(this.label_AccSwitch);
            this.panel_ScriptAcc.Controls.Add(this.cBox_AccPower);
            this.panel_ScriptAcc.Controls.Add(this.label_AccAddr);
            this.panel_ScriptAcc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_ScriptAcc.Location = new System.Drawing.Point(0, 0);
            this.panel_ScriptAcc.Name = "panel_ScriptAcc";
            this.panel_ScriptAcc.Size = new System.Drawing.Size(285, 146);
            this.panel_ScriptAcc.TabIndex = 33;
            // 
            // numericAccAddress
            // 
            this.numericAccAddress.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.numericAccAddress.Location = new System.Drawing.Point(109, 18);
            this.numericAccAddress.Maximum = new decimal(new int[] {
            320,
            0,
            0,
            0});
            this.numericAccAddress.Name = "numericAccAddress";
            this.numericAccAddress.Size = new System.Drawing.Size(120, 24);
            this.numericAccAddress.TabIndex = 36;
            // 
            // label_AccSwitch
            // 
            this.label_AccSwitch.AutoSize = true;
            this.label_AccSwitch.Location = new System.Drawing.Point(17, 62);
            this.label_AccSwitch.Name = "label_AccSwitch";
            this.label_AccSwitch.Size = new System.Drawing.Size(39, 12);
            this.label_AccSwitch.TabIndex = 35;
            this.label_AccSwitch.Text = "Switch";
            // 
            // cBox_AccPower
            // 
            this.cBox_AccPower.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBox_AccPower.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cBox_AccPower.FormattingEnabled = true;
            this.cBox_AccPower.Items.AddRange(new object[] {
            "RED",
            "GREEN"});
            this.cBox_AccPower.Location = new System.Drawing.Point(109, 56);
            this.cBox_AccPower.Name = "cBox_AccPower";
            this.cBox_AccPower.Size = new System.Drawing.Size(120, 23);
            this.cBox_AccPower.TabIndex = 34;
            // 
            // label_AccAddr
            // 
            this.label_AccAddr.AutoSize = true;
            this.label_AccAddr.Location = new System.Drawing.Point(17, 23);
            this.label_AccAddr.Name = "label_AccAddr";
            this.label_AccAddr.Size = new System.Drawing.Size(47, 12);
            this.label_AccAddr.TabIndex = 33;
            this.label_AccAddr.Text = "Address";
            // 
            // tabWait
            // 
            this.tabWait.Controls.Add(this.panel_ScriptWait);
            this.tabWait.Location = new System.Drawing.Point(4, 22);
            this.tabWait.Name = "tabWait";
            this.tabWait.Padding = new System.Windows.Forms.Padding(3);
            this.tabWait.Size = new System.Drawing.Size(285, 146);
            this.tabWait.TabIndex = 5;
            this.tabWait.Text = "Wait";
            this.tabWait.UseVisualStyleBackColor = true;
            // 
            // panel_ScriptWait
            // 
            this.panel_ScriptWait.Controls.Add(this.label10);
            this.panel_ScriptWait.Controls.Add(this.label_WaitTime);
            this.panel_ScriptWait.Controls.Add(this.numericWaitTime);
            this.panel_ScriptWait.Location = new System.Drawing.Point(3, 3);
            this.panel_ScriptWait.Name = "panel_ScriptWait";
            this.panel_ScriptWait.Size = new System.Drawing.Size(303, 149);
            this.panel_ScriptWait.TabIndex = 3;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(159, 51);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(77, 12);
            this.label10.TabIndex = 5;
            this.label10.Text = "10[d]=1.0[sec]";
            // 
            // label_WaitTime
            // 
            this.label_WaitTime.AutoSize = true;
            this.label_WaitTime.Location = new System.Drawing.Point(21, 23);
            this.label_WaitTime.Name = "label_WaitTime";
            this.label_WaitTime.Size = new System.Drawing.Size(53, 12);
            this.label_WaitTime.TabIndex = 4;
            this.label_WaitTime.Text = "Wait time";
            // 
            // numericWaitTime
            // 
            this.numericWaitTime.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.numericWaitTime.Location = new System.Drawing.Point(128, 18);
            this.numericWaitTime.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numericWaitTime.Name = "numericWaitTime";
            this.numericWaitTime.Size = new System.Drawing.Size(120, 24);
            this.numericWaitTime.TabIndex = 3;
            // 
            // button_Ok
            // 
            this.button_Ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_Ok.Location = new System.Drawing.Point(121, 164);
            this.button_Ok.Name = "button_Ok";
            this.button_Ok.Size = new System.Drawing.Size(75, 27);
            this.button_Ok.TabIndex = 14;
            this.button_Ok.Text = "&OK";
            this.button_Ok.UseVisualStyleBackColor = true;
            this.button_Ok.Click += new System.EventHandler(this.button_Ok_Click);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabLocos);
            this.tabControl.Controls.Add(this.tabLocoFunc);
            this.tabControl.Controls.Add(this.tabLocoDir);
            this.tabControl.Controls.Add(this.tabAcc);
            this.tabControl.Controls.Add(this.tabPower);
            this.tabControl.Controls.Add(this.tabWait);
            this.tabControl.Controls.Add(this.tabLine);
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Controls.Add(this.tabPage2);
            this.tabControl.Controls.Add(this.tabPage3);
            this.tabControl.Controls.Add(this.tabPage4);
            this.tabControl.Controls.Add(this.tabPage5);
            this.tabControl.Controls.Add(this.tabJumpRun);
            this.tabControl.Controls.Add(this.tabRoute);
            this.tabControl.Location = new System.Drawing.Point(12, 6);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(293, 172);
            this.tabControl.TabIndex = 16;
            // 
            // tabLocoFunc
            // 
            this.tabLocoFunc.Controls.Add(this.panel_ScriptLocFunc);
            this.tabLocoFunc.Location = new System.Drawing.Point(4, 22);
            this.tabLocoFunc.Name = "tabLocoFunc";
            this.tabLocoFunc.Padding = new System.Windows.Forms.Padding(3);
            this.tabLocoFunc.Size = new System.Drawing.Size(285, 146);
            this.tabLocoFunc.TabIndex = 7;
            this.tabLocoFunc.Text = "LocFunc";
            this.tabLocoFunc.UseVisualStyleBackColor = true;
            // 
            // panel_ScriptLocFunc
            // 
            this.panel_ScriptLocFunc.Controls.Add(this.cBox_AddrReplacedLocFnc);
            this.panel_ScriptLocFunc.Controls.Add(this.cBox_ProtcolLocFnc);
            this.panel_ScriptLocFunc.Controls.Add(this.label_FncOnOff);
            this.panel_ScriptLocFunc.Controls.Add(this.cBox_FunctionPower);
            this.panel_ScriptLocFunc.Controls.Add(this.label_FncNo);
            this.panel_ScriptLocFunc.Controls.Add(this.cBox_FunctionNo);
            this.panel_ScriptLocFunc.Controls.Add(this.label_LocAddr2);
            this.panel_ScriptLocFunc.Controls.Add(this.cBox_LocAddressFnc);
            this.panel_ScriptLocFunc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_ScriptLocFunc.Location = new System.Drawing.Point(3, 3);
            this.panel_ScriptLocFunc.Name = "panel_ScriptLocFunc";
            this.panel_ScriptLocFunc.Size = new System.Drawing.Size(279, 140);
            this.panel_ScriptLocFunc.TabIndex = 0;
            // 
            // cBox_AddrReplacedLocFnc
            // 
            this.cBox_AddrReplacedLocFnc.AutoSize = true;
            this.cBox_AddrReplacedLocFnc.Location = new System.Drawing.Point(134, 48);
            this.cBox_AddrReplacedLocFnc.Name = "cBox_AddrReplacedLocFnc";
            this.cBox_AddrReplacedLocFnc.Size = new System.Drawing.Size(71, 16);
            this.cBox_AddrReplacedLocFnc.TabIndex = 49;
            this.cBox_AddrReplacedLocFnc.Text = "Replaced";
            this.cBox_AddrReplacedLocFnc.UseVisualStyleBackColor = true;
            this.cBox_AddrReplacedLocFnc.CheckedChanged += new System.EventHandler(this.cBox_AddrDefinedLocFnc_CheckedChanged);
            // 
            // cBox_ProtcolLocFnc
            // 
            this.cBox_ProtcolLocFnc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBox_ProtcolLocFnc.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cBox_ProtcolLocFnc.FormattingEnabled = true;
            this.cBox_ProtcolLocFnc.Items.AddRange(new object[] {
            "MM2",
            "mfx",
            "DCC"});
            this.cBox_ProtcolLocFnc.Location = new System.Drawing.Point(211, 43);
            this.cBox_ProtcolLocFnc.Name = "cBox_ProtcolLocFnc";
            this.cBox_ProtcolLocFnc.Size = new System.Drawing.Size(57, 23);
            this.cBox_ProtcolLocFnc.TabIndex = 48;
            // 
            // label_FncOnOff
            // 
            this.label_FncOnOff.AutoSize = true;
            this.label_FncOnOff.Location = new System.Drawing.Point(8, 118);
            this.label_FncOnOff.Name = "label_FncOnOff";
            this.label_FncOnOff.Size = new System.Drawing.Size(93, 12);
            this.label_FncOnOff.TabIndex = 47;
            this.label_FncOnOff.Text = "Function  On/Off";
            // 
            // cBox_FunctionPower
            // 
            this.cBox_FunctionPower.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBox_FunctionPower.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cBox_FunctionPower.FormattingEnabled = true;
            this.cBox_FunctionPower.Items.AddRange(new object[] {
            "OFF",
            "ON"});
            this.cBox_FunctionPower.Location = new System.Drawing.Point(130, 112);
            this.cBox_FunctionPower.Name = "cBox_FunctionPower";
            this.cBox_FunctionPower.Size = new System.Drawing.Size(138, 23);
            this.cBox_FunctionPower.TabIndex = 46;
            // 
            // label_FncNo
            // 
            this.label_FncNo.AutoSize = true;
            this.label_FncNo.Location = new System.Drawing.Point(8, 82);
            this.label_FncNo.Name = "label_FncNo";
            this.label_FncNo.Size = new System.Drawing.Size(67, 12);
            this.label_FncNo.TabIndex = 45;
            this.label_FncNo.Text = "Function No";
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
            this.cBox_FunctionNo.Location = new System.Drawing.Point(130, 76);
            this.cBox_FunctionNo.Name = "cBox_FunctionNo";
            this.cBox_FunctionNo.Size = new System.Drawing.Size(138, 23);
            this.cBox_FunctionNo.TabIndex = 44;
            // 
            // label_LocAddr2
            // 
            this.label_LocAddr2.AutoSize = true;
            this.label_LocAddr2.Location = new System.Drawing.Point(6, 20);
            this.label_LocAddr2.Name = "label_LocAddr2";
            this.label_LocAddr2.Size = new System.Drawing.Size(69, 12);
            this.label_LocAddr2.TabIndex = 43;
            this.label_LocAddr2.Text = "Loc Address";
            // 
            // cBox_LocAddressFnc
            // 
            this.cBox_LocAddressFnc.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cBox_LocAddressFnc.FormattingEnabled = true;
            this.cBox_LocAddressFnc.Items.AddRange(new object[] {
            "SLOT.A",
            "SLOT.B",
            "SLOT.C",
            "SLOT.D",
            "SLOT.E",
            "SLOT.F",
            "SLOT.G",
            "SLOT.H"});
            this.cBox_LocAddressFnc.Location = new System.Drawing.Point(130, 14);
            this.cBox_LocAddressFnc.Name = "cBox_LocAddressFnc";
            this.cBox_LocAddressFnc.Size = new System.Drawing.Size(138, 23);
            this.cBox_LocAddressFnc.TabIndex = 42;
            // 
            // tabLocoDir
            // 
            this.tabLocoDir.Controls.Add(this.panel_ScriptDirect);
            this.tabLocoDir.Location = new System.Drawing.Point(4, 22);
            this.tabLocoDir.Name = "tabLocoDir";
            this.tabLocoDir.Padding = new System.Windows.Forms.Padding(3);
            this.tabLocoDir.Size = new System.Drawing.Size(285, 146);
            this.tabLocoDir.TabIndex = 8;
            this.tabLocoDir.Text = "LocDirect";
            this.tabLocoDir.UseVisualStyleBackColor = true;
            // 
            // panel_ScriptDirect
            // 
            this.panel_ScriptDirect.Controls.Add(this.cBox_AddrReplacedLocDir);
            this.panel_ScriptDirect.Controls.Add(this.cBox_ProtcolLocDir);
            this.panel_ScriptDirect.Controls.Add(this.cBox_Direction);
            this.panel_ScriptDirect.Controls.Add(this.label_LocDirection);
            this.panel_ScriptDirect.Controls.Add(this.label_LocAddr3);
            this.panel_ScriptDirect.Controls.Add(this.cBox_LocAddressDir);
            this.panel_ScriptDirect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_ScriptDirect.Location = new System.Drawing.Point(3, 3);
            this.panel_ScriptDirect.Name = "panel_ScriptDirect";
            this.panel_ScriptDirect.Size = new System.Drawing.Size(279, 140);
            this.panel_ScriptDirect.TabIndex = 0;
            // 
            // cBox_AddrReplacedLocDir
            // 
            this.cBox_AddrReplacedLocDir.AutoSize = true;
            this.cBox_AddrReplacedLocDir.Location = new System.Drawing.Point(123, 54);
            this.cBox_AddrReplacedLocDir.Name = "cBox_AddrReplacedLocDir";
            this.cBox_AddrReplacedLocDir.Size = new System.Drawing.Size(71, 16);
            this.cBox_AddrReplacedLocDir.TabIndex = 48;
            this.cBox_AddrReplacedLocDir.Text = "Replaced";
            this.cBox_AddrReplacedLocDir.UseVisualStyleBackColor = true;
            this.cBox_AddrReplacedLocDir.CheckedChanged += new System.EventHandler(this.cBox_AddrDefinedLocDir_CheckedChanged);
            // 
            // cBox_ProtcolLocDir
            // 
            this.cBox_ProtcolLocDir.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBox_ProtcolLocDir.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cBox_ProtcolLocDir.FormattingEnabled = true;
            this.cBox_ProtcolLocDir.Items.AddRange(new object[] {
            "MM2",
            "mfx",
            "DCC"});
            this.cBox_ProtcolLocDir.Location = new System.Drawing.Point(200, 49);
            this.cBox_ProtcolLocDir.Name = "cBox_ProtcolLocDir";
            this.cBox_ProtcolLocDir.Size = new System.Drawing.Size(57, 23);
            this.cBox_ProtcolLocDir.TabIndex = 46;
            // 
            // cBox_Direction
            // 
            this.cBox_Direction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBox_Direction.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cBox_Direction.FormattingEnabled = true;
            this.cBox_Direction.Items.AddRange(new object[] {
            "Foward",
            "Reverse"});
            this.cBox_Direction.Location = new System.Drawing.Point(119, 85);
            this.cBox_Direction.Name = "cBox_Direction";
            this.cBox_Direction.Size = new System.Drawing.Size(138, 23);
            this.cBox_Direction.TabIndex = 45;
            // 
            // label_LocDirection
            // 
            this.label_LocDirection.AutoSize = true;
            this.label_LocDirection.Location = new System.Drawing.Point(12, 89);
            this.label_LocDirection.Name = "label_LocDirection";
            this.label_LocDirection.Size = new System.Drawing.Size(51, 12);
            this.label_LocDirection.TabIndex = 44;
            this.label_LocDirection.Text = "Direction";
            // 
            // label_LocAddr3
            // 
            this.label_LocAddr3.AutoSize = true;
            this.label_LocAddr3.Location = new System.Drawing.Point(12, 26);
            this.label_LocAddr3.Name = "label_LocAddr3";
            this.label_LocAddr3.Size = new System.Drawing.Size(69, 12);
            this.label_LocAddr3.TabIndex = 43;
            this.label_LocAddr3.Text = "Loc Address";
            // 
            // cBox_LocAddressDir
            // 
            this.cBox_LocAddressDir.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cBox_LocAddressDir.FormattingEnabled = true;
            this.cBox_LocAddressDir.Items.AddRange(new object[] {
            "SLOT.A",
            "SLOT.B",
            "SLOT.C",
            "SLOT.D",
            "SLOT.E",
            "SLOT.F",
            "SLOT.G",
            "SLOT.H"});
            this.cBox_LocAddressDir.Location = new System.Drawing.Point(119, 20);
            this.cBox_LocAddressDir.Name = "cBox_LocAddressDir";
            this.cBox_LocAddressDir.Size = new System.Drawing.Size(138, 23);
            this.cBox_LocAddressDir.TabIndex = 42;
            // 
            // tabPower
            // 
            this.tabPower.Controls.Add(this.panel_ScriptPwr);
            this.tabPower.Location = new System.Drawing.Point(4, 22);
            this.tabPower.Name = "tabPower";
            this.tabPower.Size = new System.Drawing.Size(285, 146);
            this.tabPower.TabIndex = 4;
            this.tabPower.Text = "Power";
            this.tabPower.UseVisualStyleBackColor = true;
            // 
            // panel_ScriptPwr
            // 
            this.panel_ScriptPwr.Controls.Add(this.label_Power);
            this.panel_ScriptPwr.Controls.Add(this.cBox_Power);
            this.panel_ScriptPwr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_ScriptPwr.Location = new System.Drawing.Point(0, 0);
            this.panel_ScriptPwr.Name = "panel_ScriptPwr";
            this.panel_ScriptPwr.Size = new System.Drawing.Size(285, 146);
            this.panel_ScriptPwr.TabIndex = 25;
            // 
            // label_Power
            // 
            this.label_Power.AutoSize = true;
            this.label_Power.Location = new System.Drawing.Point(16, 19);
            this.label_Power.Name = "label_Power";
            this.label_Power.Size = new System.Drawing.Size(36, 12);
            this.label_Power.TabIndex = 26;
            this.label_Power.Text = "Power";
            // 
            // cBox_Power
            // 
            this.cBox_Power.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBox_Power.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cBox_Power.FormattingEnabled = true;
            this.cBox_Power.Items.AddRange(new object[] {
            "Power off",
            "Power on"});
            this.cBox_Power.Location = new System.Drawing.Point(103, 15);
            this.cBox_Power.Name = "cBox_Power";
            this.cBox_Power.Size = new System.Drawing.Size(163, 23);
            this.cBox_Power.TabIndex = 25;
            // 
            // tabLine
            // 
            this.tabLine.Controls.Add(this.panel_ScriptLine);
            this.tabLine.Location = new System.Drawing.Point(4, 22);
            this.tabLine.Name = "tabLine";
            this.tabLine.Padding = new System.Windows.Forms.Padding(3);
            this.tabLine.Size = new System.Drawing.Size(285, 146);
            this.tabLine.TabIndex = 9;
            this.tabLine.Text = "Line";
            this.tabLine.UseVisualStyleBackColor = true;
            // 
            // panel_ScriptLine
            // 
            this.panel_ScriptLine.Controls.Add(this.cBox_LineNoLabel);
            this.panel_ScriptLine.Controls.Add(this.label_LineNum);
            this.panel_ScriptLine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_ScriptLine.Location = new System.Drawing.Point(3, 3);
            this.panel_ScriptLine.Name = "panel_ScriptLine";
            this.panel_ScriptLine.Size = new System.Drawing.Size(279, 140);
            this.panel_ScriptLine.TabIndex = 0;
            // 
            // cBox_LineNoLabel
            // 
            this.cBox_LineNoLabel.FormattingEnabled = true;
            this.cBox_LineNoLabel.Location = new System.Drawing.Point(121, 22);
            this.cBox_LineNoLabel.Name = "cBox_LineNoLabel";
            this.cBox_LineNoLabel.Size = new System.Drawing.Size(121, 20);
            this.cBox_LineNoLabel.TabIndex = 9;
            // 
            // label_LineNum
            // 
            this.label_LineNum.AutoSize = true;
            this.label_LineNum.Location = new System.Drawing.Point(27, 25);
            this.label_LineNum.Name = "label_LineNum";
            this.label_LineNum.Size = new System.Drawing.Size(77, 12);
            this.label_LineNum.TabIndex = 6;
            this.label_LineNum.Text = "Line No/Label";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel_LineLabel);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(285, 146);
            this.tabPage1.TabIndex = 10;
            this.tabPage1.Text = "LineLabel";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // panel_LineLabel
            // 
            this.panel_LineLabel.Controls.Add(this.cBox_LineLabel);
            this.panel_LineLabel.Controls.Add(this.label_LabelName);
            this.panel_LineLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_LineLabel.Location = new System.Drawing.Point(3, 3);
            this.panel_LineLabel.Name = "panel_LineLabel";
            this.panel_LineLabel.Size = new System.Drawing.Size(279, 140);
            this.panel_LineLabel.TabIndex = 0;
            // 
            // cBox_LineLabel
            // 
            this.cBox_LineLabel.FormattingEnabled = true;
            this.cBox_LineLabel.Location = new System.Drawing.Point(120, 21);
            this.cBox_LineLabel.Name = "cBox_LineLabel";
            this.cBox_LineLabel.Size = new System.Drawing.Size(121, 20);
            this.cBox_LineLabel.TabIndex = 8;
            // 
            // label_LabelName
            // 
            this.label_LabelName.AutoSize = true;
            this.label_LabelName.Location = new System.Drawing.Point(21, 24);
            this.label_LabelName.Name = "label_LabelName";
            this.label_LabelName.Size = new System.Drawing.Size(65, 12);
            this.label_LabelName.TabIndex = 7;
            this.label_LabelName.Text = "Label Name";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.panel_Jump);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(285, 146);
            this.tabPage2.TabIndex = 11;
            this.tabPage2.Text = "Jump";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // panel_Jump
            // 
            this.panel_Jump.Controls.Add(this.tBox_JumpFlagNo);
            this.panel_Jump.Controls.Add(this.label_EquivVal);
            this.panel_Jump.Controls.Add(this.numUpDown_JumpValue);
            this.panel_Jump.Controls.Add(this.label_FlagNo);
            this.panel_Jump.Controls.Add(this.cBox_JumpLabelName);
            this.panel_Jump.Controls.Add(this.label_LabelNameJmp);
            this.panel_Jump.Location = new System.Drawing.Point(6, 6);
            this.panel_Jump.Name = "panel_Jump";
            this.panel_Jump.Size = new System.Drawing.Size(276, 132);
            this.panel_Jump.TabIndex = 0;
            // 
            // tBox_JumpFlagNo
            // 
            this.tBox_JumpFlagNo.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tBox_JumpFlagNo.Location = new System.Drawing.Point(133, 58);
            this.tBox_JumpFlagNo.Name = "tBox_JumpFlagNo";
            this.tBox_JumpFlagNo.Size = new System.Drawing.Size(121, 24);
            this.tBox_JumpFlagNo.TabIndex = 15;
            // 
            // label_EquivVal
            // 
            this.label_EquivVal.AutoSize = true;
            this.label_EquivVal.Location = new System.Drawing.Point(25, 105);
            this.label_EquivVal.Name = "label_EquivVal";
            this.label_EquivVal.Size = new System.Drawing.Size(89, 12);
            this.label_EquivVal.TabIndex = 14;
            this.label_EquivVal.Text = "Equivalent value";
            // 
            // numUpDown_JumpValue
            // 
            this.numUpDown_JumpValue.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.numUpDown_JumpValue.Location = new System.Drawing.Point(133, 100);
            this.numUpDown_JumpValue.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.numUpDown_JumpValue.Name = "numUpDown_JumpValue";
            this.numUpDown_JumpValue.Size = new System.Drawing.Size(121, 24);
            this.numUpDown_JumpValue.TabIndex = 13;
            // 
            // label_FlagNo
            // 
            this.label_FlagNo.AutoSize = true;
            this.label_FlagNo.Location = new System.Drawing.Point(25, 64);
            this.label_FlagNo.Name = "label_FlagNo";
            this.label_FlagNo.Size = new System.Drawing.Size(81, 12);
            this.label_FlagNo.TabIndex = 12;
            this.label_FlagNo.Text = "Flag No (0-49)";
            // 
            // cBox_JumpLabelName
            // 
            this.cBox_JumpLabelName.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cBox_JumpLabelName.FormattingEnabled = true;
            this.cBox_JumpLabelName.Location = new System.Drawing.Point(133, 18);
            this.cBox_JumpLabelName.Name = "cBox_JumpLabelName";
            this.cBox_JumpLabelName.Size = new System.Drawing.Size(121, 23);
            this.cBox_JumpLabelName.TabIndex = 10;
            // 
            // label_LabelNameJmp
            // 
            this.label_LabelNameJmp.AutoSize = true;
            this.label_LabelNameJmp.Location = new System.Drawing.Point(25, 24);
            this.label_LabelNameJmp.Name = "label_LabelNameJmp";
            this.label_LabelNameJmp.Size = new System.Drawing.Size(65, 12);
            this.label_LabelNameJmp.TabIndex = 9;
            this.label_LabelNameJmp.Text = "Label Name";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.panel_SetFlag);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(285, 146);
            this.tabPage3.TabIndex = 12;
            this.tabPage3.Text = "SetFlag";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // panel_SetFlag
            // 
            this.panel_SetFlag.Controls.Add(this.tBox_SetFlagNo);
            this.panel_SetFlag.Controls.Add(this.label_Value);
            this.panel_SetFlag.Controls.Add(this.numUpDown_SetFlagValue);
            this.panel_SetFlag.Controls.Add(this.label_FlagNo2);
            this.panel_SetFlag.Location = new System.Drawing.Point(6, 6);
            this.panel_SetFlag.Name = "panel_SetFlag";
            this.panel_SetFlag.Size = new System.Drawing.Size(273, 132);
            this.panel_SetFlag.TabIndex = 0;
            // 
            // tBox_SetFlagNo
            // 
            this.tBox_SetFlagNo.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tBox_SetFlagNo.Location = new System.Drawing.Point(134, 15);
            this.tBox_SetFlagNo.Name = "tBox_SetFlagNo";
            this.tBox_SetFlagNo.Size = new System.Drawing.Size(121, 24);
            this.tBox_SetFlagNo.TabIndex = 19;
            // 
            // label_Value
            // 
            this.label_Value.AutoSize = true;
            this.label_Value.Location = new System.Drawing.Point(26, 62);
            this.label_Value.Name = "label_Value";
            this.label_Value.Size = new System.Drawing.Size(34, 12);
            this.label_Value.TabIndex = 18;
            this.label_Value.Text = "Value";
            // 
            // numUpDown_SetFlagValue
            // 
            this.numUpDown_SetFlagValue.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.numUpDown_SetFlagValue.Location = new System.Drawing.Point(134, 57);
            this.numUpDown_SetFlagValue.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.numUpDown_SetFlagValue.Minimum = new decimal(new int[] {
            65536,
            0,
            0,
            -2147483648});
            this.numUpDown_SetFlagValue.Name = "numUpDown_SetFlagValue";
            this.numUpDown_SetFlagValue.Size = new System.Drawing.Size(121, 24);
            this.numUpDown_SetFlagValue.TabIndex = 17;
            // 
            // label_FlagNo2
            // 
            this.label_FlagNo2.AutoSize = true;
            this.label_FlagNo2.Location = new System.Drawing.Point(26, 24);
            this.label_FlagNo2.Name = "label_FlagNo2";
            this.label_FlagNo2.Size = new System.Drawing.Size(81, 12);
            this.label_FlagNo2.TabIndex = 16;
            this.label_FlagNo2.Text = "Flag No (0-49)";
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.panel_RunFile);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(285, 146);
            this.tabPage4.TabIndex = 13;
            this.tabPage4.Text = "RunFile";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // panel_RunFile
            // 
            this.panel_RunFile.Controls.Add(this.button_OpenTarget);
            this.panel_RunFile.Controls.Add(this.textBox_TargetFile);
            this.panel_RunFile.Controls.Add(this.label_TargetFile);
            this.panel_RunFile.Controls.Add(this.label_AppName);
            this.panel_RunFile.Controls.Add(this.cBox_Appname);
            this.panel_RunFile.Location = new System.Drawing.Point(6, 7);
            this.panel_RunFile.Name = "panel_RunFile";
            this.panel_RunFile.Size = new System.Drawing.Size(273, 132);
            this.panel_RunFile.TabIndex = 1;
            // 
            // button_OpenTarget
            // 
            this.button_OpenTarget.Location = new System.Drawing.Point(180, 100);
            this.button_OpenTarget.Name = "button_OpenTarget";
            this.button_OpenTarget.Size = new System.Drawing.Size(75, 23);
            this.button_OpenTarget.TabIndex = 20;
            this.button_OpenTarget.Text = "Open";
            this.button_OpenTarget.UseVisualStyleBackColor = true;
            this.button_OpenTarget.Click += new System.EventHandler(this.button_OpenTarget_Click);
            // 
            // textBox_TargetFile
            // 
            this.textBox_TargetFile.Location = new System.Drawing.Point(28, 77);
            this.textBox_TargetFile.Name = "textBox_TargetFile";
            this.textBox_TargetFile.Size = new System.Drawing.Size(227, 21);
            this.textBox_TargetFile.TabIndex = 19;
            // 
            // label_TargetFile
            // 
            this.label_TargetFile.AutoSize = true;
            this.label_TargetFile.Location = new System.Drawing.Point(26, 62);
            this.label_TargetFile.Name = "label_TargetFile";
            this.label_TargetFile.Size = new System.Drawing.Size(61, 12);
            this.label_TargetFile.TabIndex = 18;
            this.label_TargetFile.Text = "Target File";
            // 
            // label_AppName
            // 
            this.label_AppName.AutoSize = true;
            this.label_AppName.Location = new System.Drawing.Point(26, 24);
            this.label_AppName.Name = "label_AppName";
            this.label_AppName.Size = new System.Drawing.Size(95, 12);
            this.label_AppName.TabIndex = 16;
            this.label_AppName.Text = "Application Name";
            // 
            // cBox_Appname
            // 
            this.cBox_Appname.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cBox_Appname.FormattingEnabled = true;
            this.cBox_Appname.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9"});
            this.cBox_Appname.Location = new System.Drawing.Point(134, 18);
            this.cBox_Appname.Name = "cBox_Appname";
            this.cBox_Appname.Size = new System.Drawing.Size(121, 23);
            this.cBox_Appname.TabIndex = 15;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.panel_Free);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(285, 146);
            this.tabPage5.TabIndex = 14;
            this.tabPage5.Text = "Free";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // panel_Free
            // 
            this.panel_Free.Controls.Add(this.textBox_FreeCommand);
            this.panel_Free.Controls.Add(this.label_FreeCommand);
            this.panel_Free.Controls.Add(this.label_FreeDescription);
            this.panel_Free.Location = new System.Drawing.Point(6, 7);
            this.panel_Free.Name = "panel_Free";
            this.panel_Free.Size = new System.Drawing.Size(273, 132);
            this.panel_Free.TabIndex = 2;
            // 
            // textBox_FreeCommand
            // 
            this.textBox_FreeCommand.Location = new System.Drawing.Point(15, 23);
            this.textBox_FreeCommand.Name = "textBox_FreeCommand";
            this.textBox_FreeCommand.Size = new System.Drawing.Size(250, 21);
            this.textBox_FreeCommand.TabIndex = 19;
            // 
            // label_FreeCommand
            // 
            this.label_FreeCommand.AutoSize = true;
            this.label_FreeCommand.Location = new System.Drawing.Point(13, 8);
            this.label_FreeCommand.Name = "label_FreeCommand";
            this.label_FreeCommand.Size = new System.Drawing.Size(55, 12);
            this.label_FreeCommand.TabIndex = 18;
            this.label_FreeCommand.Text = "Command";
            // 
            // label_FreeDescription
            // 
            this.label_FreeDescription.Location = new System.Drawing.Point(13, 56);
            this.label_FreeDescription.Name = "label_FreeDescription";
            this.label_FreeDescription.Size = new System.Drawing.Size(252, 55);
            this.label_FreeDescription.TabIndex = 16;
            this.label_FreeDescription.Text = "Description";
            // 
            // tabJumpRun
            // 
            this.tabJumpRun.Controls.Add(this.panelJumpRun);
            this.tabJumpRun.Location = new System.Drawing.Point(4, 22);
            this.tabJumpRun.Name = "tabJumpRun";
            this.tabJumpRun.Padding = new System.Windows.Forms.Padding(3);
            this.tabJumpRun.Size = new System.Drawing.Size(285, 146);
            this.tabJumpRun.TabIndex = 15;
            this.tabJumpRun.Text = "JumpRun";
            this.tabJumpRun.UseVisualStyleBackColor = true;
            // 
            // panelJumpRun
            // 
            this.panelJumpRun.Controls.Add(this.labelJumpRunEqualVal);
            this.panelJumpRun.Controls.Add(this.numUpDownJumpRun);
            this.panelJumpRun.Controls.Add(this.cBox_JumpRunLabel);
            this.panelJumpRun.Controls.Add(this.labelJumpRunlabel);
            this.panelJumpRun.Controls.Add(this.cBox_AddrReplacedLocJump);
            this.panelJumpRun.Controls.Add(this.cBox_JumpRunLocProt);
            this.panelJumpRun.Controls.Add(this.labelJumpRunLocAddr);
            this.panelJumpRun.Controls.Add(this.cBox_JumpRunLocAddr);
            this.panelJumpRun.Location = new System.Drawing.Point(6, 6);
            this.panelJumpRun.Name = "panelJumpRun";
            this.panelJumpRun.Size = new System.Drawing.Size(273, 134);
            this.panelJumpRun.TabIndex = 0;
            // 
            // labelJumpRunEqualVal
            // 
            this.labelJumpRunEqualVal.AutoSize = true;
            this.labelJumpRunEqualVal.Location = new System.Drawing.Point(22, 106);
            this.labelJumpRunEqualVal.Name = "labelJumpRunEqualVal";
            this.labelJumpRunEqualVal.Size = new System.Drawing.Size(89, 12);
            this.labelJumpRunEqualVal.TabIndex = 55;
            this.labelJumpRunEqualVal.Text = "Equivalent value";
            // 
            // numUpDownJumpRun
            // 
            this.numUpDownJumpRun.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.numUpDownJumpRun.Location = new System.Drawing.Point(129, 101);
            this.numUpDownJumpRun.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.numUpDownJumpRun.Name = "numUpDownJumpRun";
            this.numUpDownJumpRun.Size = new System.Drawing.Size(121, 24);
            this.numUpDownJumpRun.TabIndex = 54;
            // 
            // cBox_JumpRunLabel
            // 
            this.cBox_JumpRunLabel.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cBox_JumpRunLabel.FormattingEnabled = true;
            this.cBox_JumpRunLabel.Location = new System.Drawing.Point(129, 72);
            this.cBox_JumpRunLabel.Name = "cBox_JumpRunLabel";
            this.cBox_JumpRunLabel.Size = new System.Drawing.Size(121, 23);
            this.cBox_JumpRunLabel.TabIndex = 53;
            // 
            // labelJumpRunlabel
            // 
            this.labelJumpRunlabel.AutoSize = true;
            this.labelJumpRunlabel.Location = new System.Drawing.Point(21, 78);
            this.labelJumpRunlabel.Name = "labelJumpRunlabel";
            this.labelJumpRunlabel.Size = new System.Drawing.Size(65, 12);
            this.labelJumpRunlabel.TabIndex = 52;
            this.labelJumpRunlabel.Text = "Label Name";
            // 
            // cBox_AddrReplacedLocJump
            // 
            this.cBox_AddrReplacedLocJump.AutoSize = true;
            this.cBox_AddrReplacedLocJump.Location = new System.Drawing.Point(116, 46);
            this.cBox_AddrReplacedLocJump.Name = "cBox_AddrReplacedLocJump";
            this.cBox_AddrReplacedLocJump.Size = new System.Drawing.Size(71, 16);
            this.cBox_AddrReplacedLocJump.TabIndex = 51;
            this.cBox_AddrReplacedLocJump.Text = "Replaced";
            this.cBox_AddrReplacedLocJump.UseVisualStyleBackColor = true;
            this.cBox_AddrReplacedLocJump.CheckedChanged += new System.EventHandler(this.cBox_AddrReplacedLocJump_CheckedChanged);
            // 
            // cBox_JumpRunLocProt
            // 
            this.cBox_JumpRunLocProt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBox_JumpRunLocProt.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cBox_JumpRunLocProt.FormattingEnabled = true;
            this.cBox_JumpRunLocProt.Items.AddRange(new object[] {
            "MM2",
            "mfx",
            "DCC"});
            this.cBox_JumpRunLocProt.Location = new System.Drawing.Point(193, 41);
            this.cBox_JumpRunLocProt.Name = "cBox_JumpRunLocProt";
            this.cBox_JumpRunLocProt.Size = new System.Drawing.Size(57, 23);
            this.cBox_JumpRunLocProt.TabIndex = 50;
            // 
            // labelJumpRunLocAddr
            // 
            this.labelJumpRunLocAddr.AutoSize = true;
            this.labelJumpRunLocAddr.Location = new System.Drawing.Point(21, 18);
            this.labelJumpRunLocAddr.Name = "labelJumpRunLocAddr";
            this.labelJumpRunLocAddr.Size = new System.Drawing.Size(69, 12);
            this.labelJumpRunLocAddr.TabIndex = 49;
            this.labelJumpRunLocAddr.Text = "Loc Address";
            // 
            // cBox_JumpRunLocAddr
            // 
            this.cBox_JumpRunLocAddr.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cBox_JumpRunLocAddr.FormattingEnabled = true;
            this.cBox_JumpRunLocAddr.Location = new System.Drawing.Point(116, 12);
            this.cBox_JumpRunLocAddr.Name = "cBox_JumpRunLocAddr";
            this.cBox_JumpRunLocAddr.Size = new System.Drawing.Size(134, 23);
            this.cBox_JumpRunLocAddr.TabIndex = 48;
            // 
            // tabRoute
            // 
            this.tabRoute.Controls.Add(this.panelRoute);
            this.tabRoute.Location = new System.Drawing.Point(4, 22);
            this.tabRoute.Name = "tabRoute";
            this.tabRoute.Padding = new System.Windows.Forms.Padding(3);
            this.tabRoute.Size = new System.Drawing.Size(285, 146);
            this.tabRoute.TabIndex = 16;
            this.tabRoute.Text = "Route";
            this.tabRoute.UseVisualStyleBackColor = true;
            // 
            // panelRoute
            // 
            this.panelRoute.Controls.Add(this.cBoxRouteState);
            this.panelRoute.Controls.Add(this.label_RuteState);
            this.panelRoute.Controls.Add(this.cBox_Route);
            this.panelRoute.Controls.Add(this.labelRoutename);
            this.panelRoute.Controls.Add(this.cBox_Routelabel);
            this.panelRoute.Controls.Add(this.labeRouteLabel);
            this.panelRoute.Location = new System.Drawing.Point(6, 6);
            this.panelRoute.Name = "panelRoute";
            this.panelRoute.Size = new System.Drawing.Size(273, 134);
            this.panelRoute.TabIndex = 58;
            // 
            // cBoxRouteState
            // 
            this.cBoxRouteState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBoxRouteState.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cBoxRouteState.FormattingEnabled = true;
            this.cBoxRouteState.Items.AddRange(new object[] {
            "Opening",
            "Closing"});
            this.cBoxRouteState.Location = new System.Drawing.Point(129, 91);
            this.cBoxRouteState.Name = "cBoxRouteState";
            this.cBoxRouteState.Size = new System.Drawing.Size(121, 23);
            this.cBoxRouteState.TabIndex = 63;
            // 
            // label_RuteState
            // 
            this.label_RuteState.AutoSize = true;
            this.label_RuteState.Location = new System.Drawing.Point(21, 97);
            this.label_RuteState.Name = "label_RuteState";
            this.label_RuteState.Size = new System.Drawing.Size(32, 12);
            this.label_RuteState.TabIndex = 62;
            this.label_RuteState.Text = "State";
            // 
            // cBox_Route
            // 
            this.cBox_Route.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBox_Route.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cBox_Route.FormattingEnabled = true;
            this.cBox_Route.Location = new System.Drawing.Point(129, 13);
            this.cBox_Route.Name = "cBox_Route";
            this.cBox_Route.Size = new System.Drawing.Size(121, 23);
            this.cBox_Route.TabIndex = 61;
            // 
            // labelRoutename
            // 
            this.labelRoutename.AutoSize = true;
            this.labelRoutename.Location = new System.Drawing.Point(21, 19);
            this.labelRoutename.Name = "labelRoutename";
            this.labelRoutename.Size = new System.Drawing.Size(35, 12);
            this.labelRoutename.TabIndex = 60;
            this.labelRoutename.Text = "Route";
            // 
            // cBox_Routelabel
            // 
            this.cBox_Routelabel.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cBox_Routelabel.FormattingEnabled = true;
            this.cBox_Routelabel.Location = new System.Drawing.Point(129, 53);
            this.cBox_Routelabel.Name = "cBox_Routelabel";
            this.cBox_Routelabel.Size = new System.Drawing.Size(121, 23);
            this.cBox_Routelabel.TabIndex = 59;
            // 
            // labeRouteLabel
            // 
            this.labeRouteLabel.AutoSize = true;
            this.labeRouteLabel.Location = new System.Drawing.Point(21, 59);
            this.labeRouteLabel.Name = "labeRouteLabel";
            this.labeRouteLabel.Size = new System.Drawing.Size(65, 12);
            this.labeRouteLabel.TabIndex = 58;
            this.labeRouteLabel.Text = "Label Name";
            // 
            // panel2
            // 
            this.panel2.Location = new System.Drawing.Point(79, 170);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(200, 100);
            this.panel2.TabIndex = 42;
            // 
            // comboBox5
            // 
            this.comboBox5.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox5.FormattingEnabled = true;
            this.comboBox5.Items.AddRange(new object[] {
            "MM2",
            "mfx",
            "DCC"});
            this.comboBox5.Location = new System.Drawing.Point(216, 38);
            this.comboBox5.Name = "comboBox5";
            this.comboBox5.Size = new System.Drawing.Size(57, 20);
            this.comboBox5.TabIndex = 41;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Increment = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.numericUpDown1.Location = new System.Drawing.Point(238, 70);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(135, 19);
            this.numericUpDown1.TabIndex = 32;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(23, 124);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(93, 12);
            this.label11.TabIndex = 31;
            this.label11.Text = "Function  On/Off";
            // 
            // comboBox6
            // 
            this.comboBox6.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox6.FormattingEnabled = true;
            this.comboBox6.Items.AddRange(new object[] {
            "OFF",
            "ON"});
            this.comboBox6.Location = new System.Drawing.Point(135, 121);
            this.comboBox6.Name = "comboBox6";
            this.comboBox6.Size = new System.Drawing.Size(138, 20);
            this.comboBox6.TabIndex = 30;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(23, 98);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(67, 12);
            this.label12.TabIndex = 29;
            this.label12.Text = "Function No";
            // 
            // comboBox7
            // 
            this.comboBox7.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox7.FormattingEnabled = true;
            this.comboBox7.Items.AddRange(new object[] {
            "None",
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
            "F16"});
            this.comboBox7.Location = new System.Drawing.Point(135, 95);
            this.comboBox7.Name = "comboBox7";
            this.comboBox7.Size = new System.Drawing.Size(138, 20);
            this.comboBox7.TabIndex = 28;
            // 
            // comboBox8
            // 
            this.comboBox8.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox8.FormattingEnabled = true;
            this.comboBox8.Items.AddRange(new object[] {
            "Foward",
            "Reverse"});
            this.comboBox8.Location = new System.Drawing.Point(135, 61);
            this.comboBox8.Name = "comboBox8";
            this.comboBox8.Size = new System.Drawing.Size(138, 20);
            this.comboBox8.TabIndex = 26;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(23, 61);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(36, 12);
            this.label13.TabIndex = 25;
            this.label13.Text = "Speed";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(23, 14);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(69, 12);
            this.label14.TabIndex = 22;
            this.label14.Text = "Loc Address";
            // 
            // comboBox9
            // 
            this.comboBox9.FormattingEnabled = true;
            this.comboBox9.Location = new System.Drawing.Point(135, 12);
            this.comboBox9.Name = "comboBox9";
            this.comboBox9.Size = new System.Drawing.Size(138, 20);
            this.comboBox9.TabIndex = 21;
            // 
            // panel3
            // 
            this.panel3.Location = new System.Drawing.Point(79, 170);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(200, 100);
            this.panel3.TabIndex = 42;
            // 
            // comboBox10
            // 
            this.comboBox10.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox10.FormattingEnabled = true;
            this.comboBox10.Items.AddRange(new object[] {
            "MM2",
            "mfx",
            "DCC"});
            this.comboBox10.Location = new System.Drawing.Point(216, 38);
            this.comboBox10.Name = "comboBox10";
            this.comboBox10.Size = new System.Drawing.Size(57, 20);
            this.comboBox10.TabIndex = 41;
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Increment = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.numericUpDown2.Location = new System.Drawing.Point(238, 70);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(135, 19);
            this.numericUpDown2.TabIndex = 32;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(23, 124);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(93, 12);
            this.label15.TabIndex = 31;
            this.label15.Text = "Function  On/Off";
            // 
            // comboBox11
            // 
            this.comboBox11.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox11.FormattingEnabled = true;
            this.comboBox11.Items.AddRange(new object[] {
            "OFF",
            "ON"});
            this.comboBox11.Location = new System.Drawing.Point(135, 121);
            this.comboBox11.Name = "comboBox11";
            this.comboBox11.Size = new System.Drawing.Size(138, 20);
            this.comboBox11.TabIndex = 30;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(23, 98);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(67, 12);
            this.label16.TabIndex = 29;
            this.label16.Text = "Function No";
            // 
            // comboBox12
            // 
            this.comboBox12.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox12.FormattingEnabled = true;
            this.comboBox12.Items.AddRange(new object[] {
            "None",
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
            "F16"});
            this.comboBox12.Location = new System.Drawing.Point(135, 95);
            this.comboBox12.Name = "comboBox12";
            this.comboBox12.Size = new System.Drawing.Size(138, 20);
            this.comboBox12.TabIndex = 28;
            // 
            // comboBox13
            // 
            this.comboBox13.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox13.FormattingEnabled = true;
            this.comboBox13.Items.AddRange(new object[] {
            "Foward",
            "Reverse"});
            this.comboBox13.Location = new System.Drawing.Point(135, 61);
            this.comboBox13.Name = "comboBox13";
            this.comboBox13.Size = new System.Drawing.Size(138, 20);
            this.comboBox13.TabIndex = 26;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(23, 61);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(36, 12);
            this.label17.TabIndex = 25;
            this.label17.Text = "Speed";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(23, 14);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(69, 12);
            this.label18.TabIndex = 22;
            this.label18.Text = "Loc Address";
            // 
            // comboBox14
            // 
            this.comboBox14.FormattingEnabled = true;
            this.comboBox14.Location = new System.Drawing.Point(135, 12);
            this.comboBox14.Name = "comboBox14";
            this.comboBox14.Size = new System.Drawing.Size(138, 20);
            this.comboBox14.TabIndex = 21;
            // 
            // panel_ScriptOuter
            // 
            this.panel_ScriptOuter.Location = new System.Drawing.Point(3, 7);
            this.panel_ScriptOuter.Name = "panel_ScriptOuter";
            this.panel_ScriptOuter.Size = new System.Drawing.Size(302, 171);
            this.panel_ScriptOuter.TabIndex = 17;
            // 
            // ScriptEditForm
            // 
            this.AcceptButton = this.button_Ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button_Cancel;
            this.ClientSize = new System.Drawing.Size(308, 206);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.button_Ok);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.panel_ScriptOuter);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ScriptEditForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Script Line Editor";
            this.tabLocos.ResumeLayout(false);
            this.panel_ScriptLocSpeed.ResumeLayout(false);
            this.panel_ScriptLocSpeed.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown_TransitionTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericSpeed)).EndInit();
            this.tabAcc.ResumeLayout(false);
            this.panel_ScriptAcc.ResumeLayout(false);
            this.panel_ScriptAcc.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericAccAddress)).EndInit();
            this.tabWait.ResumeLayout(false);
            this.panel_ScriptWait.ResumeLayout(false);
            this.panel_ScriptWait.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericWaitTime)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.tabLocoFunc.ResumeLayout(false);
            this.panel_ScriptLocFunc.ResumeLayout(false);
            this.panel_ScriptLocFunc.PerformLayout();
            this.tabLocoDir.ResumeLayout(false);
            this.panel_ScriptDirect.ResumeLayout(false);
            this.panel_ScriptDirect.PerformLayout();
            this.tabPower.ResumeLayout(false);
            this.panel_ScriptPwr.ResumeLayout(false);
            this.panel_ScriptPwr.PerformLayout();
            this.tabLine.ResumeLayout(false);
            this.panel_ScriptLine.ResumeLayout(false);
            this.panel_ScriptLine.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.panel_LineLabel.ResumeLayout(false);
            this.panel_LineLabel.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.panel_Jump.ResumeLayout(false);
            this.panel_Jump.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown_JumpValue)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.panel_SetFlag.ResumeLayout(false);
            this.panel_SetFlag.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown_SetFlagValue)).EndInit();
            this.tabPage4.ResumeLayout(false);
            this.panel_RunFile.ResumeLayout(false);
            this.panel_RunFile.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.panel_Free.ResumeLayout(false);
            this.panel_Free.PerformLayout();
            this.tabJumpRun.ResumeLayout(false);
            this.panelJumpRun.ResumeLayout(false);
            this.panelJumpRun.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownJumpRun)).EndInit();
            this.tabRoute.ResumeLayout(false);
            this.panelRoute.ResumeLayout(false);
            this.panelRoute.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_Cancel;
        public System.Windows.Forms.TabPage tabLocos;
        public System.Windows.Forms.TabPage tabAcc;
        public System.Windows.Forms.TabPage tabWait;
        private System.Windows.Forms.Button button_Ok;
        public System.Windows.Forms.TabControl tabControl;
        public System.Windows.Forms.TabPage tabPower;
        private System.Windows.Forms.TabPage tabLocoFunc;
        public System.Windows.Forms.ComboBox cBox_ProtcolLocFnc;
        public System.Windows.Forms.Label label_FncOnOff;
        public System.Windows.Forms.ComboBox cBox_FunctionPower;
        public System.Windows.Forms.Label label_FncNo;
        public System.Windows.Forms.ComboBox cBox_FunctionNo;
        private System.Windows.Forms.Label label_LocAddr2;
        public System.Windows.Forms.ComboBox cBox_LocAddressFnc;
        private System.Windows.Forms.TabPage tabLocoDir;
        public System.Windows.Forms.ComboBox cBox_ProtcolLocDir;
        public System.Windows.Forms.ComboBox cBox_Direction;
        public System.Windows.Forms.Label label_LocDirection;
        private System.Windows.Forms.Label label_LocAddr3;
        public System.Windows.Forms.ComboBox cBox_LocAddressDir;
        private System.Windows.Forms.Panel panel2;
        public System.Windows.Forms.ComboBox comboBox5;
        public System.Windows.Forms.NumericUpDown numericUpDown1;
        public System.Windows.Forms.Label label11;
        public System.Windows.Forms.ComboBox comboBox6;
        public System.Windows.Forms.Label label12;
        public System.Windows.Forms.ComboBox comboBox7;
        public System.Windows.Forms.ComboBox comboBox8;
        public System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        public System.Windows.Forms.ComboBox comboBox9;
        private System.Windows.Forms.Panel panel3;
        public System.Windows.Forms.ComboBox comboBox10;
        public System.Windows.Forms.NumericUpDown numericUpDown2;
        public System.Windows.Forms.Label label15;
        public System.Windows.Forms.ComboBox comboBox11;
        public System.Windows.Forms.Label label16;
        public System.Windows.Forms.ComboBox comboBox12;
        public System.Windows.Forms.ComboBox comboBox13;
        public System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        public System.Windows.Forms.ComboBox comboBox14;
        public System.Windows.Forms.ComboBox cBox_ProtcolLocSpd;
        public System.Windows.Forms.NumericUpDown numericSpeed;
        public System.Windows.Forms.Label label_Speed;
        private System.Windows.Forms.Label label_LocAddr;
        public System.Windows.Forms.ComboBox cBox_LocAddressSpd;
        public System.Windows.Forms.NumericUpDown numericAccAddress;
        private System.Windows.Forms.Label label_AccSwitch;
        public System.Windows.Forms.ComboBox cBox_AccPower;
        private System.Windows.Forms.Label label_AccAddr;
        private System.Windows.Forms.Label label_Power;
        public System.Windows.Forms.ComboBox cBox_Power;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label_WaitTime;
        public System.Windows.Forms.NumericUpDown numericWaitTime;
        private System.Windows.Forms.Panel panel_ScriptOuter;
        public System.Windows.Forms.Panel panel_ScriptLocSpeed;
        public System.Windows.Forms.Panel panel_ScriptLocFunc;
        public System.Windows.Forms.Panel panel_ScriptDirect;
        public System.Windows.Forms.Panel panel_ScriptAcc;
        public System.Windows.Forms.Panel panel_ScriptPwr;
        public System.Windows.Forms.Panel panel_ScriptWait;
        private System.Windows.Forms.TabPage tabLine;
        public System.Windows.Forms.Panel panel_ScriptLine;
        private System.Windows.Forms.Label label_LineNum;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label_LabelName;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label_LabelNameJmp;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label label_Value;
        private System.Windows.Forms.Label label_FlagNo2;
        public System.Windows.Forms.Panel panel_Jump;
        public System.Windows.Forms.Panel panel_SetFlag;
        public System.Windows.Forms.Panel panel_LineLabel;
        public System.Windows.Forms.ComboBox cBox_LineLabel;
        public System.Windows.Forms.ComboBox cBox_JumpLabelName;
        public System.Windows.Forms.NumericUpDown numUpDown_JumpValue;
        public System.Windows.Forms.NumericUpDown numUpDown_SetFlagValue;
        public System.Windows.Forms.CheckBox cBox_AddrReplacedLocDir;
        public System.Windows.Forms.CheckBox cBox_AddrReplacedLocSpd;
        public System.Windows.Forms.CheckBox cBox_AddrReplacedLocFnc;
        private System.Windows.Forms.TabPage tabPage4;
        public System.Windows.Forms.Panel panel_RunFile;
        private System.Windows.Forms.Button button_OpenTarget;
        public System.Windows.Forms.TextBox textBox_TargetFile;
        private System.Windows.Forms.Label label_TargetFile;
        private System.Windows.Forms.Label label_AppName;
        public System.Windows.Forms.ComboBox cBox_Appname;
        private System.Windows.Forms.TabPage tabPage5;
        public System.Windows.Forms.Panel panel_Free;
        public System.Windows.Forms.TextBox textBox_FreeCommand;
        private System.Windows.Forms.Label label_FreeCommand;
        public System.Windows.Forms.Label label_FreeDescription;
        public System.Windows.Forms.NumericUpDown numUpDown_TransitionTime;
        public System.Windows.Forms.Label label_TransitionTime;
        public System.Windows.Forms.TextBox tBox_JumpFlagNo;
        public System.Windows.Forms.TextBox tBox_SetFlagNo;
        public System.Windows.Forms.Label label_FlagNo;
        public System.Windows.Forms.TabPage tabJumpRun;
        private System.Windows.Forms.Label labelJumpRunEqualVal;
        public System.Windows.Forms.NumericUpDown numUpDownJumpRun;
        public System.Windows.Forms.ComboBox cBox_JumpRunLabel;
        private System.Windows.Forms.Label labelJumpRunlabel;
        public System.Windows.Forms.CheckBox cBox_AddrReplacedLocJump;
        public System.Windows.Forms.ComboBox cBox_JumpRunLocProt;
        private System.Windows.Forms.Label labelJumpRunLocAddr;
        public System.Windows.Forms.ComboBox cBox_JumpRunLocAddr;
        private System.Windows.Forms.TabPage tabRoute;
        public System.Windows.Forms.Panel panelJumpRun;
        public System.Windows.Forms.Panel panelRoute;
        public System.Windows.Forms.ComboBox cBox_Route;
        public System.Windows.Forms.Label labelRoutename;
        public System.Windows.Forms.ComboBox cBox_Routelabel;
        public System.Windows.Forms.Label labeRouteLabel;
        public System.Windows.Forms.ComboBox cBoxRouteState;
        public System.Windows.Forms.Label label_RuteState;
        public System.Windows.Forms.ComboBox cBox_LineNoLabel;
        public System.Windows.Forms.Label label_EquivVal;
    }
}
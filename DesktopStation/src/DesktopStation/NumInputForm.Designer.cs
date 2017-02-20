namespace DesktopStation
{
    partial class NumInputForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NumInputForm));
            this.button_Cancel = new System.Windows.Forms.Button();
            this.button_Ok = new System.Windows.Forms.Button();
            this.label_Description = new System.Windows.Forms.Label();
            this.label_AccAddr = new System.Windows.Forms.Label();
            this.addressUpDown = new System.Windows.Forms.NumericUpDown();
            this.label_S88Addr = new System.Windows.Forms.Label();
            this.S88sensorUpDown = new System.Windows.Forms.NumericUpDown();
            this.gBox_addr = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabRoute = new System.Windows.Forms.TabPage();
            this.buttonRouteDelete = new System.Windows.Forms.Button();
            this.label_SelRoute = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.gBox_Detail = new System.Windows.Forms.GroupBox();
            this.button4 = new System.Windows.Forms.Button();
            this.button_Up = new System.Windows.Forms.Button();
            this.labelSigDir = new System.Windows.Forms.Label();
            this.cBox_SigDir = new System.Windows.Forms.ComboBox();
            this.buttonEditRouteItem = new System.Windows.Forms.Button();
            this.buttonDelRouteItem = new System.Windows.Forms.Button();
            this.buttonAddRouteItem = new System.Windows.Forms.Button();
            this.listView_RouteAccs = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderOp = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cBox_IgnoreOpenState = new System.Windows.Forms.CheckBox();
            this.numUpDown_SigAddr = new System.Windows.Forms.NumericUpDown();
            this.labelSigAddr = new System.Windows.Forms.Label();
            this.textBox_RouteName = new System.Windows.Forms.TextBox();
            this.labelName = new System.Windows.Forms.Label();
            this.comboBox_Routes = new System.Windows.Forms.ComboBox();
            this.tabDeco = new System.Windows.Forms.TabPage();
            this.buttonOpenDecoImage = new System.Windows.Forms.Button();
            this.labelDecoImageFile = new System.Windows.Forms.Label();
            this.numUpDownY = new System.Windows.Forms.NumericUpDown();
            this.labelDecoY = new System.Windows.Forms.Label();
            this.numUpDownX = new System.Windows.Forms.NumericUpDown();
            this.labelDecoX = new System.Windows.Forms.Label();
            this.picDecoImage = new System.Windows.Forms.PictureBox();
            this.textBoxDecoUserText = new System.Windows.Forms.TextBox();
            this.labelDecoText = new System.Windows.Forms.Label();
            this.cBox_UseDeco = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.addressUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.S88sensorUpDown)).BeginInit();
            this.gBox_addr.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabRoute.SuspendLayout();
            this.gBox_Detail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown_SigAddr)).BeginInit();
            this.tabDeco.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDecoImage)).BeginInit();
            this.SuspendLayout();
            // 
            // button_Cancel
            // 
            this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Cancel.Location = new System.Drawing.Point(523, 425);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(75, 31);
            this.button_Cancel.TabIndex = 11;
            this.button_Cancel.Text = "&Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            // 
            // button_Ok
            // 
            this.button_Ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_Ok.Location = new System.Drawing.Point(417, 425);
            this.button_Ok.Name = "button_Ok";
            this.button_Ok.Size = new System.Drawing.Size(75, 31);
            this.button_Ok.TabIndex = 10;
            this.button_Ok.Text = "&OK";
            this.button_Ok.UseVisualStyleBackColor = true;
            // 
            // label_Description
            // 
            this.label_Description.AutoSize = true;
            this.label_Description.Location = new System.Drawing.Point(370, 62);
            this.label_Description.Name = "label_Description";
            this.label_Description.Size = new System.Drawing.Size(171, 12);
            this.label_Description.TabIndex = 9;
            this.label_Description.Text = "*Address 0 means not assigned.";
            // 
            // label_AccAddr
            // 
            this.label_AccAddr.AutoSize = true;
            this.label_AccAddr.Location = new System.Drawing.Point(23, 31);
            this.label_AccAddr.Name = "label_AccAddr";
            this.label_AccAddr.Size = new System.Drawing.Size(103, 12);
            this.label_AccAddr.TabIndex = 8;
            this.label_AccAddr.Text = "Accessory address";
            // 
            // addressUpDown
            // 
            this.addressUpDown.Font = new System.Drawing.Font("Arial", 15F);
            this.addressUpDown.Location = new System.Drawing.Point(170, 20);
            this.addressUpDown.Maximum = new decimal(new int[] {
            2048,
            0,
            0,
            0});
            this.addressUpDown.Name = "addressUpDown";
            this.addressUpDown.Size = new System.Drawing.Size(120, 30);
            this.addressUpDown.TabIndex = 7;
            this.addressUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label_S88Addr
            // 
            this.label_S88Addr.AutoSize = true;
            this.label_S88Addr.Location = new System.Drawing.Point(317, 31);
            this.label_S88Addr.Name = "label_S88Addr";
            this.label_S88Addr.Size = new System.Drawing.Size(106, 12);
            this.label_S88Addr.TabIndex = 13;
            this.label_S88Addr.Text = "S88 sensor address";
            // 
            // S88sensorUpDown
            // 
            this.S88sensorUpDown.Font = new System.Drawing.Font("Arial", 15F);
            this.S88sensorUpDown.Location = new System.Drawing.Point(439, 20);
            this.S88sensorUpDown.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.S88sensorUpDown.Name = "S88sensorUpDown";
            this.S88sensorUpDown.Size = new System.Drawing.Size(120, 30);
            this.S88sensorUpDown.TabIndex = 12;
            this.S88sensorUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // gBox_addr
            // 
            this.gBox_addr.Controls.Add(this.addressUpDown);
            this.gBox_addr.Controls.Add(this.label_AccAddr);
            this.gBox_addr.Controls.Add(this.label_S88Addr);
            this.gBox_addr.Controls.Add(this.label_Description);
            this.gBox_addr.Controls.Add(this.S88sensorUpDown);
            this.gBox_addr.Location = new System.Drawing.Point(22, 12);
            this.gBox_addr.Name = "gBox_addr";
            this.gBox_addr.Size = new System.Drawing.Size(577, 84);
            this.gBox_addr.TabIndex = 14;
            this.gBox_addr.TabStop = false;
            this.gBox_addr.Text = "Address Assignment";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabRoute);
            this.tabControl1.Controls.Add(this.tabDeco);
            this.tabControl1.Location = new System.Drawing.Point(12, 113);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(597, 300);
            this.tabControl1.TabIndex = 16;
            // 
            // tabRoute
            // 
            this.tabRoute.Controls.Add(this.buttonRouteDelete);
            this.tabRoute.Controls.Add(this.label_SelRoute);
            this.tabRoute.Controls.Add(this.button1);
            this.tabRoute.Controls.Add(this.gBox_Detail);
            this.tabRoute.Controls.Add(this.comboBox_Routes);
            this.tabRoute.Location = new System.Drawing.Point(4, 22);
            this.tabRoute.Name = "tabRoute";
            this.tabRoute.Padding = new System.Windows.Forms.Padding(3);
            this.tabRoute.Size = new System.Drawing.Size(589, 274);
            this.tabRoute.TabIndex = 0;
            this.tabRoute.Text = "Routes";
            this.tabRoute.UseVisualStyleBackColor = true;
            // 
            // buttonRouteDelete
            // 
            this.buttonRouteDelete.Location = new System.Drawing.Point(474, 10);
            this.buttonRouteDelete.Name = "buttonRouteDelete";
            this.buttonRouteDelete.Size = new System.Drawing.Size(75, 35);
            this.buttonRouteDelete.TabIndex = 3;
            this.buttonRouteDelete.Text = "Delete";
            this.buttonRouteDelete.UseVisualStyleBackColor = true;
            this.buttonRouteDelete.Click += new System.EventHandler(this.buttonRouteDelete_Click);
            // 
            // label_SelRoute
            // 
            this.label_SelRoute.AutoSize = true;
            this.label_SelRoute.Location = new System.Drawing.Point(17, 21);
            this.label_SelRoute.Name = "label_SelRoute";
            this.label_SelRoute.Size = new System.Drawing.Size(83, 12);
            this.label_SelRoute.TabIndex = 10;
            this.label_SelRoute.Text = "Selected Route";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(383, 10);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 35);
            this.button1.TabIndex = 2;
            this.button1.Text = "Add";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // gBox_Detail
            // 
            this.gBox_Detail.Controls.Add(this.button4);
            this.gBox_Detail.Controls.Add(this.button_Up);
            this.gBox_Detail.Controls.Add(this.labelSigDir);
            this.gBox_Detail.Controls.Add(this.cBox_SigDir);
            this.gBox_Detail.Controls.Add(this.buttonEditRouteItem);
            this.gBox_Detail.Controls.Add(this.buttonDelRouteItem);
            this.gBox_Detail.Controls.Add(this.buttonAddRouteItem);
            this.gBox_Detail.Controls.Add(this.listView_RouteAccs);
            this.gBox_Detail.Controls.Add(this.cBox_IgnoreOpenState);
            this.gBox_Detail.Controls.Add(this.numUpDown_SigAddr);
            this.gBox_Detail.Controls.Add(this.labelSigAddr);
            this.gBox_Detail.Controls.Add(this.textBox_RouteName);
            this.gBox_Detail.Controls.Add(this.labelName);
            this.gBox_Detail.Location = new System.Drawing.Point(6, 51);
            this.gBox_Detail.Name = "gBox_Detail";
            this.gBox_Detail.Size = new System.Drawing.Size(576, 212);
            this.gBox_Detail.TabIndex = 1;
            this.gBox_Detail.TabStop = false;
            this.gBox_Detail.Text = "Route detail";
            // 
            // button4
            // 
            this.button4.Image = ((System.Drawing.Image)(resources.GetObject("button4.Image")));
            this.button4.Location = new System.Drawing.Point(547, 49);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(23, 24);
            this.button4.TabIndex = 22;
            this.button4.Tag = "2";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button_Up_Click);
            // 
            // button_Up
            // 
            this.button_Up.Image = ((System.Drawing.Image)(resources.GetObject("button_Up.Image")));
            this.button_Up.Location = new System.Drawing.Point(547, 20);
            this.button_Up.Name = "button_Up";
            this.button_Up.Size = new System.Drawing.Size(23, 23);
            this.button_Up.TabIndex = 21;
            this.button_Up.Tag = "1";
            this.button_Up.UseVisualStyleBackColor = true;
            this.button_Up.Click += new System.EventHandler(this.button_Up_Click);
            // 
            // labelSigDir
            // 
            this.labelSigDir.AutoSize = true;
            this.labelSigDir.Location = new System.Drawing.Point(15, 117);
            this.labelSigDir.Name = "labelSigDir";
            this.labelSigDir.Size = new System.Drawing.Size(86, 12);
            this.labelSigDir.TabIndex = 20;
            this.labelSigDir.Text = "Signal Direction";
            // 
            // cBox_SigDir
            // 
            this.cBox_SigDir.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBox_SigDir.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cBox_SigDir.FormattingEnabled = true;
            this.cBox_SigDir.Items.AddRange(new object[] {
            "Red, Diverse",
            "Green, Straight"});
            this.cBox_SigDir.Location = new System.Drawing.Point(111, 110);
            this.cBox_SigDir.Name = "cBox_SigDir";
            this.cBox_SigDir.Size = new System.Drawing.Size(148, 24);
            this.cBox_SigDir.TabIndex = 19;
            this.cBox_SigDir.SelectedIndexChanged += new System.EventHandler(this.cBox_SigDir_SelectedIndexChanged);
            // 
            // buttonEditRouteItem
            // 
            this.buttonEditRouteItem.Location = new System.Drawing.Point(367, 171);
            this.buttonEditRouteItem.Name = "buttonEditRouteItem";
            this.buttonEditRouteItem.Size = new System.Drawing.Size(87, 23);
            this.buttonEditRouteItem.TabIndex = 18;
            this.buttonEditRouteItem.Text = "Edit";
            this.buttonEditRouteItem.UseVisualStyleBackColor = true;
            this.buttonEditRouteItem.Click += new System.EventHandler(this.buttonEditRouteItem_Click);
            // 
            // buttonDelRouteItem
            // 
            this.buttonDelRouteItem.Location = new System.Drawing.Point(466, 171);
            this.buttonDelRouteItem.Name = "buttonDelRouteItem";
            this.buttonDelRouteItem.Size = new System.Drawing.Size(75, 23);
            this.buttonDelRouteItem.TabIndex = 17;
            this.buttonDelRouteItem.Text = "Delete";
            this.buttonDelRouteItem.UseVisualStyleBackColor = true;
            this.buttonDelRouteItem.Click += new System.EventHandler(this.buttonDelRouteItem_Click_1);
            // 
            // buttonAddRouteItem
            // 
            this.buttonAddRouteItem.Location = new System.Drawing.Point(281, 171);
            this.buttonAddRouteItem.Name = "buttonAddRouteItem";
            this.buttonAddRouteItem.Size = new System.Drawing.Size(75, 23);
            this.buttonAddRouteItem.TabIndex = 16;
            this.buttonAddRouteItem.Text = "Add";
            this.buttonAddRouteItem.UseVisualStyleBackColor = true;
            this.buttonAddRouteItem.Click += new System.EventHandler(this.buttonAddRouteItem_Click);
            // 
            // listView_RouteAccs
            // 
            this.listView_RouteAccs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeaderOp,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.listView_RouteAccs.FullRowSelect = true;
            this.listView_RouteAccs.GridLines = true;
            this.listView_RouteAccs.Location = new System.Drawing.Point(281, 20);
            this.listView_RouteAccs.Name = "listView_RouteAccs";
            this.listView_RouteAccs.Size = new System.Drawing.Size(260, 145);
            this.listView_RouteAccs.TabIndex = 15;
            this.listView_RouteAccs.UseCompatibleStateImageBehavior = false;
            this.listView_RouteAccs.View = System.Windows.Forms.View.Details;
            this.listView_RouteAccs.DoubleClick += new System.EventHandler(this.buttonEditRouteItem_Click);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "No";
            this.columnHeader1.Width = 37;
            // 
            // columnHeaderOp
            // 
            this.columnHeaderOp.Text = "Op";
            this.columnHeaderOp.Width = 40;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Type";
            this.columnHeader2.Width = 40;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Addr";
            this.columnHeader3.Width = 50;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Dir";
            // 
            // cBox_IgnoreOpenState
            // 
            this.cBox_IgnoreOpenState.AutoSize = true;
            this.cBox_IgnoreOpenState.Location = new System.Drawing.Point(111, 149);
            this.cBox_IgnoreOpenState.Name = "cBox_IgnoreOpenState";
            this.cBox_IgnoreOpenState.Size = new System.Drawing.Size(113, 16);
            this.cBox_IgnoreOpenState.TabIndex = 14;
            this.cBox_IgnoreOpenState.Text = "Ignore open state";
            this.cBox_IgnoreOpenState.UseVisualStyleBackColor = true;
            this.cBox_IgnoreOpenState.CheckedChanged += new System.EventHandler(this.cBox_IgnoreOpenState_CheckedChanged);
            // 
            // numUpDown_SigAddr
            // 
            this.numUpDown_SigAddr.Font = new System.Drawing.Font("Arial", 15F);
            this.numUpDown_SigAddr.Location = new System.Drawing.Point(111, 71);
            this.numUpDown_SigAddr.Maximum = new decimal(new int[] {
            2048,
            0,
            0,
            0});
            this.numUpDown_SigAddr.Name = "numUpDown_SigAddr";
            this.numUpDown_SigAddr.Size = new System.Drawing.Size(148, 30);
            this.numUpDown_SigAddr.TabIndex = 11;
            this.numUpDown_SigAddr.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numUpDown_SigAddr.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // labelSigAddr
            // 
            this.labelSigAddr.AutoSize = true;
            this.labelSigAddr.Location = new System.Drawing.Point(15, 82);
            this.labelSigAddr.Name = "labelSigAddr";
            this.labelSigAddr.Size = new System.Drawing.Size(80, 12);
            this.labelSigAddr.TabIndex = 12;
            this.labelSigAddr.Text = "Signal address";
            // 
            // textBox_RouteName
            // 
            this.textBox_RouteName.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBox_RouteName.Location = new System.Drawing.Point(111, 31);
            this.textBox_RouteName.Name = "textBox_RouteName";
            this.textBox_RouteName.Size = new System.Drawing.Size(148, 24);
            this.textBox_RouteName.TabIndex = 10;
            this.textBox_RouteName.TextChanged += new System.EventHandler(this.textBox_RouteName_TextChanged);
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(15, 37);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(68, 12);
            this.labelName.TabIndex = 9;
            this.labelName.Text = "Route Name";
            // 
            // comboBox_Routes
            // 
            this.comboBox_Routes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Routes.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.comboBox_Routes.FormattingEnabled = true;
            this.comboBox_Routes.Location = new System.Drawing.Point(128, 12);
            this.comboBox_Routes.Name = "comboBox_Routes";
            this.comboBox_Routes.Size = new System.Drawing.Size(230, 27);
            this.comboBox_Routes.TabIndex = 4;
            this.comboBox_Routes.SelectedIndexChanged += new System.EventHandler(this.comboBox_Routes_SelectedIndexChanged);
            // 
            // tabDeco
            // 
            this.tabDeco.Controls.Add(this.buttonOpenDecoImage);
            this.tabDeco.Controls.Add(this.labelDecoImageFile);
            this.tabDeco.Controls.Add(this.numUpDownY);
            this.tabDeco.Controls.Add(this.labelDecoY);
            this.tabDeco.Controls.Add(this.numUpDownX);
            this.tabDeco.Controls.Add(this.labelDecoX);
            this.tabDeco.Controls.Add(this.picDecoImage);
            this.tabDeco.Controls.Add(this.textBoxDecoUserText);
            this.tabDeco.Controls.Add(this.labelDecoText);
            this.tabDeco.Controls.Add(this.cBox_UseDeco);
            this.tabDeco.Location = new System.Drawing.Point(4, 22);
            this.tabDeco.Name = "tabDeco";
            this.tabDeco.Padding = new System.Windows.Forms.Padding(3);
            this.tabDeco.Size = new System.Drawing.Size(589, 274);
            this.tabDeco.TabIndex = 1;
            this.tabDeco.Text = "Decoration";
            this.tabDeco.UseVisualStyleBackColor = true;
            // 
            // buttonOpenDecoImage
            // 
            this.buttonOpenDecoImage.Enabled = false;
            this.buttonOpenDecoImage.Location = new System.Drawing.Point(497, 216);
            this.buttonOpenDecoImage.Name = "buttonOpenDecoImage";
            this.buttonOpenDecoImage.Size = new System.Drawing.Size(75, 23);
            this.buttonOpenDecoImage.TabIndex = 19;
            this.buttonOpenDecoImage.Text = "Open Image";
            this.buttonOpenDecoImage.UseVisualStyleBackColor = true;
            // 
            // labelDecoImageFile
            // 
            this.labelDecoImageFile.AutoSize = true;
            this.labelDecoImageFile.Location = new System.Drawing.Point(342, 188);
            this.labelDecoImageFile.Name = "labelDecoImageFile";
            this.labelDecoImageFile.Size = new System.Drawing.Size(28, 12);
            this.labelDecoImageFile.TabIndex = 18;
            this.labelDecoImageFile.Text = "Text";
            // 
            // numUpDownY
            // 
            this.numUpDownY.Font = new System.Drawing.Font("Arial", 15F);
            this.numUpDownY.Location = new System.Drawing.Point(92, 101);
            this.numUpDownY.Maximum = new decimal(new int[] {
            2048,
            0,
            0,
            0});
            this.numUpDownY.Name = "numUpDownY";
            this.numUpDownY.Size = new System.Drawing.Size(104, 30);
            this.numUpDownY.TabIndex = 16;
            this.numUpDownY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelDecoY
            // 
            this.labelDecoY.AutoSize = true;
            this.labelDecoY.Location = new System.Drawing.Point(17, 112);
            this.labelDecoY.Name = "labelDecoY";
            this.labelDecoY.Size = new System.Drawing.Size(57, 12);
            this.labelDecoY.TabIndex = 17;
            this.labelDecoY.Text = "Position Y";
            // 
            // numUpDownX
            // 
            this.numUpDownX.Font = new System.Drawing.Font("Arial", 15F);
            this.numUpDownX.Location = new System.Drawing.Point(92, 54);
            this.numUpDownX.Maximum = new decimal(new int[] {
            2048,
            0,
            0,
            0});
            this.numUpDownX.Name = "numUpDownX";
            this.numUpDownX.Size = new System.Drawing.Size(104, 30);
            this.numUpDownX.TabIndex = 14;
            this.numUpDownX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelDecoX
            // 
            this.labelDecoX.AutoSize = true;
            this.labelDecoX.Location = new System.Drawing.Point(17, 65);
            this.labelDecoX.Name = "labelDecoX";
            this.labelDecoX.Size = new System.Drawing.Size(57, 12);
            this.labelDecoX.TabIndex = 15;
            this.labelDecoX.Text = "Position X";
            // 
            // picDecoImage
            // 
            this.picDecoImage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picDecoImage.Location = new System.Drawing.Point(344, 22);
            this.picDecoImage.Name = "picDecoImage";
            this.picDecoImage.Size = new System.Drawing.Size(228, 163);
            this.picDecoImage.TabIndex = 13;
            this.picDecoImage.TabStop = false;
            // 
            // textBoxDecoUserText
            // 
            this.textBoxDecoUserText.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBoxDecoUserText.Location = new System.Drawing.Point(92, 149);
            this.textBoxDecoUserText.Multiline = true;
            this.textBoxDecoUserText.Name = "textBoxDecoUserText";
            this.textBoxDecoUserText.Size = new System.Drawing.Size(226, 71);
            this.textBoxDecoUserText.TabIndex = 12;
            // 
            // labelDecoText
            // 
            this.labelDecoText.AutoSize = true;
            this.labelDecoText.Location = new System.Drawing.Point(17, 155);
            this.labelDecoText.Name = "labelDecoText";
            this.labelDecoText.Size = new System.Drawing.Size(28, 12);
            this.labelDecoText.TabIndex = 11;
            this.labelDecoText.Text = "Text";
            // 
            // cBox_UseDeco
            // 
            this.cBox_UseDeco.AutoSize = true;
            this.cBox_UseDeco.Location = new System.Drawing.Point(19, 23);
            this.cBox_UseDeco.Name = "cBox_UseDeco";
            this.cBox_UseDeco.Size = new System.Drawing.Size(101, 16);
            this.cBox_UseDeco.TabIndex = 0;
            this.cBox_UseDeco.Text = "Use decoration";
            this.cBox_UseDeco.UseVisualStyleBackColor = true;
            // 
            // NumInputForm
            // 
            this.AcceptButton = this.button_Ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button_Cancel;
            this.ClientSize = new System.Drawing.Size(618, 468);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.gBox_addr);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.button_Ok);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NumInputForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Assigning properties";
            ((System.ComponentModel.ISupportInitialize)(this.addressUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.S88sensorUpDown)).EndInit();
            this.gBox_addr.ResumeLayout(false);
            this.gBox_addr.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabRoute.ResumeLayout(false);
            this.tabRoute.PerformLayout();
            this.gBox_Detail.ResumeLayout(false);
            this.gBox_Detail.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown_SigAddr)).EndInit();
            this.tabDeco.ResumeLayout(false);
            this.tabDeco.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDecoImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.Button button_Ok;
        private System.Windows.Forms.Label label_Description;
        private System.Windows.Forms.Label label_AccAddr;
        public System.Windows.Forms.NumericUpDown addressUpDown;
        private System.Windows.Forms.Label label_S88Addr;
        public System.Windows.Forms.NumericUpDown S88sensorUpDown;
        public System.Windows.Forms.GroupBox gBox_addr;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.Button buttonRouteDelete;
        private System.Windows.Forms.Label label_SelRoute;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox gBox_Detail;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button_Up;
        private System.Windows.Forms.Label labelSigDir;
        public System.Windows.Forms.ComboBox cBox_SigDir;
        private System.Windows.Forms.Button buttonEditRouteItem;
        private System.Windows.Forms.Button buttonDelRouteItem;
        private System.Windows.Forms.Button buttonAddRouteItem;
        private System.Windows.Forms.ListView listView_RouteAccs;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeaderOp;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        public System.Windows.Forms.CheckBox cBox_IgnoreOpenState;
        public System.Windows.Forms.NumericUpDown numUpDown_SigAddr;
        private System.Windows.Forms.Label labelSigAddr;
        private System.Windows.Forms.TextBox textBox_RouteName;
        private System.Windows.Forms.Label labelName;
        public System.Windows.Forms.ComboBox comboBox_Routes;
        private System.Windows.Forms.Button buttonOpenDecoImage;
        public System.Windows.Forms.NumericUpDown numUpDownY;
        private System.Windows.Forms.Label labelDecoY;
        public System.Windows.Forms.NumericUpDown numUpDownX;
        private System.Windows.Forms.Label labelDecoX;
        private System.Windows.Forms.Label labelDecoText;
        public System.Windows.Forms.TabPage tabRoute;
        public System.Windows.Forms.TabPage tabDeco;
        public System.Windows.Forms.PictureBox picDecoImage;
        public System.Windows.Forms.Label labelDecoImageFile;
        public System.Windows.Forms.TextBox textBoxDecoUserText;
        public System.Windows.Forms.CheckBox cBox_UseDeco;
    }
}
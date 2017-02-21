using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;
using System.Xml.Serialization;
using System.IO;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using CameraS88;


namespace DesktopStation
{
    public delegate bool UpdateAccList_delegate(int inAccAddr, int inDirection);
    public delegate bool UpdateLocList_delegate(int inLocAddr, int inSpeed, int inDirection, int inFunctionNo, int inFunctionValue);
    public delegate void SetS88RunText_delegate(string inText);
    public delegate void runFile_delegate(string inAppName, string inFileName);
    public delegate void setTransitionSpeed_delegate(int inAddress, int inTargetSpeed, int inTotalTime);
    public delegate LocData getLocItemFromIndex_delegate(int inLocIndex, int inSlotIndex, int inAddress);
    public delegate int getS88Value_delegate(int inNo);
    public delegate int getAccValue_delegate(int inNo);
    public delegate bool getDCCmodee_delegate(int inIndex);

    public partial class MainForm : Form
    {
        delegate void SetRecvText_delegate(string inText);
        delegate void SetRecvOk_delegate(bool inOn);
        delegate void SetRecvAnotherStation_delegate(string inText);

        private MeterDrawing MeterDrawer;
        private AppSetting gAppSettings;
        private LocomotiveDB LocDB;
        private CVManager CVList;
        private TrackBoxManager TBoxManager;
        private List<ScriptData> ScriptList;
        private List<string> SendingList;
        private List<string> CheckingList;
        private Language LangManager;
        private DSServer AppServer;
        private RouteManager Routes;

        private int gTypedAddr;
        private int LeverValue;
        private int Dial6021Value;
        private int SelectedTileIndex;
        private int SelectedLayoutIndex;
        private int SelectedCabAccIndex;
        private int SelectedCabS88Index;
        private TrackDiagramManager LayoutMapData;
        private bool NextAutoSpeedSet6021;
        private int NextAutoSpeedAddr6021;
        private int gControlMode = 0;
        private int ScriptCurrentLine;
        private int ScriptWaitTime;
        private int ScriptMode;
        private int SelectedAccessoryIndex = 0;
        private int DisplayedAccessoryTipIndex = -1;
        private int SelectedLocIndex = 0;
        private Bitmap MeterBoxChacheBitmap;
        private Bitmap LocIconBitmap;
        private int[] S88Flags;
        private RailuinoSerial SerialCmd;
        private S88EventManager S88Manager;
        private int CheckRailuinoResponse;
        private int Counter_ComErr;
        private int Counter_S88;
        private int Counter_Ping;
        private int Counter_Timeout;
        private int Counter_GatewayError;
        private int flagUpdateByS88Sign;
        private bool flagUpdateS88SensorView;
        public DateTime timeBoot;
        private int CurrentSelectedMultiLocIndex;
        private string SerialBuftext;
        public ExecuteManager ExecuteManager;
        public CraneControl CraneController;
        public DsWebCtrl WebControl;
        private IconList iconList;
        private int ScaleRation = 100;//120;
        private bool SerialCommunicated = false;
        private bool gProcessingWebCmd = false;//Webコマンド処理で時間がかかる場合があるためのフラグ
        private bool Flag_AutoStart = false;

        private IpcRemoteObject m_msg;


        public MainForm()
        {
            InitializeComponent();

            /* ホイールを取得 */
            //ホイールイベントの追加  
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseWheel); 

            /* スリープを検出する */
            Microsoft.Win32.SystemEvents.PowerModeChanged += new Microsoft.Win32.PowerModeChangedEventHandler(this.MainForm_PowerModeChanged);

            /* パネルの親をタブからメインフォームのものに切り替え */
            panel_AccList.Parent = OuterPanel;
            panel_Layout.Parent = OuterPanel;
            panel_Loc.Parent = OuterPanel;
            panel6021.Parent = OuterPanel;
            panel_Sequence.Parent = OuterPanel;
            panel_SerialConsole.Parent = OuterPanel;
            panel_CVEditor.Parent = OuterPanel;
            panel_MultiLocs.Parent = OuterPanel;
            panel_S88.Parent = OuterPanel;
            panel_Crane.Parent = OuterPanel;

            tabPanels.Visible = false;

            /* 処理クラス */
            MeterDrawer = new MeterDrawing();
            LangManager = new Language();

            /* スクリプト機能 */
            ScriptCurrentLine = 0;
            ScriptWaitTime = 0;
            ScriptMode = 0;

            gTypedAddr = 0;
            LeverValue = 0;
            Dial6021Value = 0;
            SelectedTileIndex = 0;
            SelectedLayoutIndex = 0;
            LocDB = new LocomotiveDB();
            CVList = new CVManager();
            ScriptList = new List<ScriptData>();
            LayoutMapData = new TrackDiagramManager();
            MeterBoxChacheBitmap = new Bitmap(400, 400);
            LocIconBitmap = new Bitmap(160, 48);
            S88Flags = new int[Program.S88_MAX];
            SerialCmd = new RailuinoSerial(SendCommand, SetScriptData, SetRecvText, getDCCmode);
            TBoxManager = new TrackBoxManager();
            ExecuteManager = new ExecuteManager();
            CraneController = new CraneControl(SerialCmd);
            WebControl = new DsWebCtrl(HttpUploadCompleted);
            iconList = new IconList();
            S88Manager = new S88EventManager(SerialCmd, UpdateLocList, UpdateAccList, SetS88RunText, RunFile, SetTransition, getLocItemFromIndex, checkRoute_bridge, setRoute_bridge, getAccItem);
            AppServer = new DSServer(GetWebReceivedSerialCommand, GetWebStatus, 1192);
            AppServer.ExePath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);

            /* シリアルの送信周期制御 */
            SendingList = new List<string>();

            /* Web経由命令表示処理用 */
            CheckingList = new List<string>();

            /* シリアル受信バッファ */
            SerialBuftext = "";

            /* Railuinoの応答信号をチェックして動作が正常か確認する */
            CheckRailuinoResponse = 2;

            /* エラーカウンター */
            Counter_ComErr = 0; /* 通信エラーカウンター */
            Counter_Ping = 0; /* Pingカウンター */
            Counter_S88 = 0;  /* S88 */
            Counter_Timeout = 0; /* 予約送信のタイムアウトカウンター */
            Counter_GatewayError = 0; /* ゲートウェイ側のエラー */

            /* S88で自動的に更新される際のフラグ */
            flagUpdateByS88Sign = Program.PANELUPDATE_NONE;

            /* S88センサ状態表示の更新 */
            flagUpdateS88SensorView = false;

            /* 6021指令のセット */
            NextAutoSpeedAddr6021 = 0;
            NextAutoSpeedSet6021 = false;

            /* 複数画面の選択番号 */
            CurrentSelectedMultiLocIndex = -1;

            /* Cab画面用変数の初期化 */
            SelectedCabAccIndex = 0;
            SelectedCabS88Index = 0;

        }


        private void MainForm_PowerModeChanged(object sender, Microsoft.Win32.PowerModeChangedEventArgs e)
        {

            switch (e.Mode)
            {

                case Microsoft.Win32.PowerModes.Suspend:

                    /* Sleepに入った時 */
                    TrackPowerOff(true);

                    /* シリアルモードの場合、シリアルポートを閉じる */
                    if (gAppSettings.mSendMode == 0)
                    {
                        //シリアル通信が全て終わるまで待つ
                        DSCommon.WaitSleepTime(2);

                        //自動的にシリアルポートを閉じる
                        serialPort.Close();
                    }
                    break;

                case Microsoft.Win32.PowerModes.Resume:

                    break;
            }

        }

        private bool getDCCmode(int inIndex)
        {
            bool aResult = true;

            //範囲チェック
            if (inIndex <= 0)
            {
                return true;
            }

           AccessoryData aData = gAppSettings.mAccList[inIndex - 1];

           switch (aData.mProtocol)
           {

               case 0:
                    //AUTO
                    aResult = gAppSettings.mDCCMode;
                   break;

               case 1:
                   //DCC
                   aResult = true;
                   break;

               case 2:
                   aResult = false;
                   break;
           }


           return aResult;

        }

        private int getAccItem(int inNo)
        {

            //範囲チェック
            if (inNo <= 0)
            {
                return 0;
            }

            AccessoryData aData = gAppSettings.mAccList[inNo - 1];



            return aData.mDirection;

        }

        // マウスホイールイベント  
        private void MainForm_MouseWheel(object sender, MouseEventArgs e)
        {
            /* 目盛り数を取得 */
            if ((gControlMode == Program.POWER_OFF) || (panel_Loc.Visible == false))
            {
                return;
            }

            if (e.Delta >= 0)
            {
                KeyDownLocPanel(Program.KEYMAP_UP);
            }
            else
            {
                KeyDownLocPanel(Program.KEYMAP_DOWN);
            }


        }

        public LocData getLocItemFromIndex(int inLocIndex, int inSlotIndex, int inAddress)
        {

            if(( inLocIndex < 0) && (inSlotIndex > 0))
            {
                //スロットから取得
                int aSlotInLocIndex = gAppSettings.mLocCtrlList[inSlotIndex - 1] - 1;

                if ((aSlotInLocIndex >= 0) && (aSlotInLocIndex < LocDB.Items.Count))
                {
                    return LocDB.Items[aSlotInLocIndex];
                }
                else
                {
                    return null;
                }

            }
            else if ((inLocIndex >= 0) && (inLocIndex < LocDB.Items.Count))
            {
                //インデックスから取得
                return LocDB.Items[inLocIndex];
            }
            else if (inAddress > 0)
            {
                int aIndex = LocDB.SearchLoc(inAddress);

                if (aIndex >= 0)
                {
                    return LocDB.Items[aIndex];
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }

        }


        private void RefreshMultiLocSpeedDir(int inNo)
        {
            switch (inNo)
            {
                case 0:
                    pBox_DirectBox0.Refresh();
                    pBox_MLocName0.Refresh();
                    SpeedCtrlBox0.Refresh();
                    break;

                case 1:
                    pBox_DirectBox1.Refresh();
                    pBox_MLocName1.Refresh();
                    SpeedCtrlBox1.Refresh();
                    break;

                case 2:
                    pBox_DirectBox2.Refresh();
                    pBox_MLocName2.Refresh();
                    SpeedCtrlBox2.Refresh();
                    break;

                case 3:
                    pBox_DirectBox3.Refresh();
                    pBox_MLocName3.Refresh();
                    SpeedCtrlBox3.Refresh();
                    break;

                case 4:
                    pBox_DirectBox4.Refresh();
                    pBox_MLocName4.Refresh();
                    SpeedCtrlBox4.Refresh();
                    break;

                case 5:
                    pBox_DirectBox5.Refresh();
                    pBox_MLocName5.Refresh();
                    SpeedCtrlBox5.Refresh();
                    break;

                case 6:
                    pBox_DirectBox6.Refresh();
                    pBox_MLocName6.Refresh();
                    SpeedCtrlBox6.Refresh();
                    break;

                case 7:
                    pBox_DirectBox7.Refresh();
                    pBox_MLocName7.Refresh();
                    SpeedCtrlBox7.Refresh();
                    break;
            }
        }


        private void SButton_Close_Click(object sender, EventArgs e)
        {
            Close();

        }

        private void SetTransition(int inAddress, int inTargetSpeed, int inTotalTime)
        {
            int inIndex;

            if (inAddress <= 0)
            {
                return;
            }
            
            inIndex = LocDB.SearchLoc(inAddress);

            if (inIndex >= 0)
            {
                /* 速度自動変化処理の登録 */
                LocDB.Items[inIndex].SetTransitionSpeed(inTargetSpeed, inTotalTime);
                LocDB.Transitioning = true;
            }

        }

        private void UpdateTransition()
        {
            int aTransitionCount = 0;

            if (LocDB.Transitioning == false)
            {
                return;
            }


            for (int i = 0; i < LocDB.Items.Count; i++)
            {
                if (LocDB.Items[i].IsTrasitioning() == true)
                {
                    LocDB.Items[i].IncTransitionRemain();
                    
                    if (LocDB.Items[i].TransitionSpeed() == true)
                    {
                        /* 速度変化したので速度指令送信 */
                        LocDB.Items[i].SetUpdateNextInterval();

                        /* リスト更新 */
                        UpdateLocList(LocDB.Items[i].mLocAddr, -1, -1, -1, -1);

                    }

                    if (LocDB.Items[i].IsFinishedTransition() == true)
                    {
                        LocDB.Items[i].ResetTransitionSpeed();

                    }

                    aTransitionCount++;
                }

            }

            if (aTransitionCount == 0)
            {
                LocDB.Transitioning = false;
            }
            else
            {
                /* 描画予約 */
                flagUpdateByS88Sign = flagUpdateByS88Sign | Program.PANELUPDATE_LOC;

                /* すぐに描画させる */
                IntervalDrawAllPanel();
            }


        }

        private void SetRecvText(string inText)
        {
            listBox_Serial.BeginUpdate();
            listBox_Serial.Items.Add(inText + "\n");

            if (listBox_Serial.Items.Count > 100)
            {
                listBox_Serial.Items.RemoveAt(0);
            }

            listBox_Serial.TopIndex = listBox_Serial.Items.Count - 1;
            listBox_Serial.EndUpdate();
        }

        private void SetS88RunText(string inText)
        {
            listBox_S88Console.BeginUpdate();
            listBox_S88Console.Items.Add(inText + "\n");

            if (listBox_S88Console.Items.Count > 100)
            {
                listBox_S88Console.Items.RemoveAt(0);
            }

            listBox_S88Console.TopIndex = listBox_S88Console.Items.Count - 1;
            listBox_S88Console.EndUpdate();
        }

        private void SetRecvConnectCheck(string inText)
        {
            if (inText.IndexOf("200 Ok") >= 0)
            {
                CheckRailuinoResponse = Program.REPLY_OK;

                if (SerialCommunicated == false)
                {
                    SerialCommunicated = true;
                }

                ConnectedStatusLabel.Text = LangManager.SetText("TxtMsgReceived", "Received");

            }
            else if ((inText.IndexOf("300 Command error") >= 0) || (inText.IndexOf("301 Syntax error") >= 0) || (inText.IndexOf("302 receive timeout") >= 0) || (inText.IndexOf("303 Unknown error") >= 0))
            {
                // エラー受信
                CheckRailuinoResponse = Program.REPLY_ERROR;

                /* エラーカウンタを回す */
                Counter_GatewayError++;

                if (Counter_GatewayError > 255)
                {
                    Counter_GatewayError = 255;
                }

                ConnectedStatusLabel.Text = LangManager.SetText("TxtMsgError", "Error");
            }
            else if ((inText.IndexOf("100 Ready") >= 0) || (inText.IndexOf("101 Ready") >= 0))
            {
                //リセット完了
                CheckRailuinoResponse = Program.REPLY_READY;

                ConnectedStatusLabel.Text = LangManager.SetText("TxtMsgReset", "Reset");

            }
        }

        private void SetRecvAnotherStation(string inText)
        {
            string[] aParameters;
            int aAddress;
            uint aMFXUID;
            int aSpeed;
            int aCalcAddress;

            if (inText == "")
            {
                return;
            }

            aParameters = inText.Split(',');

            if (aParameters[0] == "@PWR")
            {
                if (aParameters[1] == "0")
                {
                    /* STOPが押された状態と同じにする（電源オフ） */
                    TrackPowerOff(false);
                }
                else if (aParameters[1] == "1")
                {
                    /* STOPが押された状態と同じにする（電源オフ） */
                    TrackPowerOn(false);
                }

            }
            else if (aParameters[0] == "@SPD")
            {
                aAddress = (DSCommon.ParseStrToIntHex(aParameters[1]) << 8) + DSCommon.ParseStrToIntHex(aParameters[2]);
                aSpeed = (DSCommon.ParseStrToIntHex(aParameters[3]) << 8) + DSCommon.ParseStrToIntHex(aParameters[4]);
  
                /* 更新 */
                UpdateLocList(aAddress, aSpeed, -1, -1, -1);

            }
            else if (aParameters[0] == "@DIR")
            {
                aAddress = (DSCommon.ParseStrToIntHex(aParameters[1]) << 8) + DSCommon.ParseStrToIntHex(aParameters[2]);
                
                /* 更新 */
                UpdateLocList(aAddress, -1, DSCommon.ParseStrToInt(aParameters[3]), -1, -1);

            }
            else if (aParameters[0] == "@FNC")
            {
                aAddress = (DSCommon.ParseStrToIntHex(aParameters[1]) << 8) + DSCommon.ParseStrToIntHex(aParameters[2]);

                /* 更新 */
                UpdateLocList(aAddress, -1, -1, DSCommon.ParseStrToInt(aParameters[3]), DSCommon.ParseStrToInt(aParameters[4]));

            }
            else if (aParameters[0] == "@ACC")
            {
                aAddress = (DSCommon.ParseStrToIntHex(aParameters[1]) << 8) + DSCommon.ParseStrToIntHex(aParameters[2]);

                if (gAppSettings.mDCCMode == true)
                {
                    //DCC Accessories
                    aCalcAddress = aAddress - Program.DCCACCADDRESS;
                }
                else
                {
                    //MM2 Accessories
                    aCalcAddress = aAddress - Program.MM2ACCADDRESS;
                }

                /* 更新 */
                UpdateAccList(aCalcAddress, DSCommon.ParseStrToInt(aParameters[3]));

            }
            else if ((aParameters[0] == "@MFX") && (gAppSettings.mMfxAutoRegister == true))
            {

                aMFXUID = (DSCommon.ParseStrToUInt32Hex(aParameters[1]) << 24) + (DSCommon.ParseStrToUInt32Hex(aParameters[2]) << 16) + (DSCommon.ParseStrToUInt32Hex(aParameters[3]) << 8) + DSCommon.ParseStrToUInt32Hex(aParameters[4]);

                /* UID検索 */
                RegisterMfxLoc(aMFXUID);
            }
            else if ((aParameters[0] == "@MFXBIND") && (gAppSettings.mMfxAutoUpdate == true))
            {

                aMFXUID = (DSCommon.ParseStrToUInt32Hex(aParameters[1]) << 24) + (DSCommon.ParseStrToUInt32Hex(aParameters[2]) << 16) + (DSCommon.ParseStrToUInt32Hex(aParameters[3]) << 8) + DSCommon.ParseStrToUInt32Hex(aParameters[4]);
                aAddress = (DSCommon.ParseStrToIntHex(aParameters[5]) << 8) + DSCommon.ParseStrToIntHex(aParameters[6]);

                /* UID検索 */
                UpdateMfxLoc(aMFXUID, aAddress);
            }
            else if (aParameters[0] == "@PING")
            {
                uint aTBoxUID;
                uint aTBoxType;
                uint aTBoxVersion;

                aTBoxUID = (DSCommon.ParseStrToUInt32Hex(aParameters[1]) << 24) + (DSCommon.ParseStrToUInt32Hex(aParameters[2]) << 16) + (DSCommon.ParseStrToUInt32Hex(aParameters[3]) << 8) + DSCommon.ParseStrToUInt32Hex(aParameters[4]);
                aTBoxVersion = (DSCommon.ParseStrToUInt32Hex(aParameters[5]) << 8) + DSCommon.ParseStrToUInt32Hex(aParameters[6]);
                aTBoxType = (DSCommon.ParseStrToUInt32Hex(aParameters[7]) << 8) + DSCommon.ParseStrToUInt32Hex(aParameters[8]);

                /* UID検索 */
                TBoxManager.SearchAdd(aTBoxUID, aTBoxType, aTBoxVersion);
            }
            else if (aParameters[0] == "@DSG")
            {
                /* Desktop Station Gateway */
                TBoxManager.SearchAdd(9999, 9999, 0);
            }  
            else if (aParameters[0] == "@CV")
            {
                aAddress = (DSCommon.ParseStrToIntHex(aParameters[1]) << 8) + DSCommon.ParseStrToIntHex(aParameters[2]);
                int aCVNo = DSCommon.ParseStrToInt(aParameters[2]);
                int aCVValue = DSCommon.ParseStrToInt(aParameters[3]);

                CVList.Add(aCVNo, aCVValue);

                listBox_CVHistory.Items.Add("Read : No=" + aCVNo + " ,Val=" + aCVValue);

            }           

        }

        private void RegisterMfxLoc(uint inMFXUID)
        {

            int aLocAddress;

            /* 既存のmfx機関車を検索。存在しない場合、-1が返る */
            aLocAddress = LocDB.SearchExistsMfxLoc(inMFXUID);


            if (aLocAddress < 0)
            {
                /* 無ければDBに追加。UID登録。LocAddress新規発行。 */
                LocData aItem = new LocData();
                aItem.mMFXUID = inMFXUID;
                aLocAddress = LocDB.GetNewMfxAdress();
                aItem.mLocAddr = aLocAddress;
                aItem.mLocName = "Unidentified mfx loc";
                aItem.mFunctionImageTable = new int[Program.MAX_FUNCTIONNUM];
                aItem.mFunctionStatus = new int[Program.MAX_FUNCTIONNUM];
                aItem.mExFunctionCommand = new string[Program.MAX_FUNCTIONNUM + 1];
                aItem.mExFunctionData = new string[Program.MAX_FUNCTIONNUM + 1];
                aItem.mFunctionExMethod = new int[Program.MAX_FUNCTIONNUM + 1];
                aItem.mFunctionExAddress = new int[Program.MAX_FUNCTIONNUM + 1];
                aItem.mFunctionExFuncNo = new int[Program.MAX_FUNCTIONNUM + 1];
                aItem.mDisplayMaxSpeed = 180;
                aItem.mSpeedAccRatio = 3;
                aItem.mSpeedRedRatio = 3;
                aItem.mLocMaxSpeed = Program.SPEED_MAX;
                LocDB.Items.Add(aItem);

                /* リスト更新 */
                UpdateLocDisplay();

                /* メッセージ表示 */
                StatusLabel.Text = "Registered new mfx loco (UID: " + inMFXUID.ToString("X8") + ", ADDR: " + aLocAddress.ToString("X4") + ")";
            }

            /* MS2が繋がっているときは、応答を返さないようにする */
            if (TBoxManager.CheckMS2() == false)
            {
                /* 認識処理実行用のコマンド送信 */
                SerialCmd.SetMfxBind(inMFXUID, LocDB.GetAddress(aLocAddress));
                SerialCmd.SetMfxVerify(inMFXUID, LocDB.GetAddress(aLocAddress));
            }
        }

        private void UpdateMfxLoc(uint inMFXUID, int inLocSID)
        {

            int aLocAddress = LocDB.AssignAddressProtcol(Program.PROTCOL_MFX, inLocSID);

            if (LocDB.UpdateExistsMfxLoc(inMFXUID, aLocAddress) == true)
            {
                /* リスト更新 */
                UpdateLocDisplay();
                
                /* メッセージ表示 */
                StatusLabel.Text = "Update mfx loco address (UID: " + inMFXUID.ToString("X8") + ", ADDR: " + aLocAddress.ToString("X4") + ")";

            }
        }
   

        private void SetRecvS88Datas(string inText)
        {
            string aFunctionName;
            string aParameters;
            string aParameter;
            int i;

            if (inText == "")
            {
                return;
            }

            if (inText[0] == '@')
            {
                aFunctionName = inText.Substring(1, inText.IndexOf(',') - 1);
                aParameters = inText.Substring(inText.IndexOf(',') + 1);

                if (aFunctionName == "S88")
                {
                    for (i = 0; i < Program.S88_MAX; i++)
                    {
                        if (aParameters.IndexOf(',') <= 0)
                        {
                            break;
                        }
                        //S88のデータを１台毎(16bit幅)に取得
                        aParameter = aParameters.Substring(0, aParameters.IndexOf(','));
                        aParameters = aParameters.Substring(aParameters.IndexOf(',') + 1);

                        if (aParameter.Length > 0)
                        {
                            S88Flags[i] = DSCommon.ParseStrToIntHex(aParameter);
                        }
                        else
                        {
                            break;
                        }
                    }


                    // S88センサ状態変化チェック
                    if (S88Manager.UpdateCheck(S88Flags) == true)
                    {
                        flagUpdateS88SensorView = true;
                    }
                    
                    // イベント状態変化チェック・実行許可
                    if (S88Manager.IntervalCheck(S88Flags) == true)
                    {
                        flagUpdateS88SensorView = true;
                    }

                }
            }
        }

        private bool GetS88Flag(int inS88No)
        {
            bool aResult;
            int aFlagData;
            int aX, aY;

            aX = inS88No / 16;
            aY = inS88No - (aX * 16);

            aFlagData = (S88Flags[inS88No / 16] >> aY) & 0x01;

            if (aFlagData == 0x01)
            {
                aResult = true;
            }
            else
            {
                aResult = false;
            }

            return aResult;
        }

        private void SendCommand(string inCommandText)
        {
            /* 送信パケット詰まり防止のため、送信予約リストにコマンドを登録。 */

            SendingList.Insert(0, inCommandText);

        }

        void SetScriptData(string inCommand, int inParam1, int inParam2, int inParam3, int inParam4)
        {
            if (ScriptMode != Program.SCRIPTMODE_TEACH)
            {
                return;
            }

            if ((ScriptWaitTime > 0) && (ScriptList.Count > 0))
            {

                ScriptData aItem2 = new ScriptData();
                aItem2.mCommand = Program.SCRIPTCMD_WAIT;
                aItem2.mParam1 = ScriptWaitTime.ToString();
                aItem2.mParam2 = "";
                aItem2.mParam3 = 0;
                aItem2.mParam4 = 0;

                ScriptList.Add(aItem2);

            }

            ScriptData aItem = new ScriptData();
            aItem.mCommand = inCommand;
            aItem.mParam1 = inParam1.ToString();
            aItem.mParam2 = inParam2.ToString();
            aItem.mParam3 = inParam3;
            aItem.mParam4 = inParam4;

            ScriptList.Add(aItem);

            /* 時刻を初期化 */
            ScriptWaitTime = 0;

        }

        public bool ReadXMLSetting(string inFileName)
        {
            bool retVal = false;

            /* 存在しない場合は生成 */
            if (gAppSettings == null)
            {
                gAppSettings = new AppSetting();
                gAppSettings.mLocCtrlList = new int[Program.MULTICONTROL_MAX];
                gAppSettings.mAccList = new List<AccessoryData>();
            }

            //設定ファイルがあるときのみ読み込みします。
            if (File.Exists(inFileName))
            {
                //XmlSerializerを呼び出す
                XmlSerializer serializer = new XmlSerializer(typeof(AppSetting));
                //ファイルを開く
                FileStream fs = new FileStream(inFileName, FileMode.Open);

                //読み込み
                try
                {
                    gAppSettings = (AppSetting)serializer.Deserialize(fs);
                }
                catch (System.Exception ex)
                {
                    //ファイルを閉じる
                    fs.Close();

                    //メッセージを表示する
                    MessageBox.Show("Unknown file. (" + ex.Message + ")", "Event File Open Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return false;
                }


                //ファイルを閉じる
                fs.Close();

                gAppSettings.Check();

                retVal = true;
            }
            else
            {
                gAppSettings.ClearDefault();

            }

            gAppSettings.ClearAccessories(gAppSettings.mClearAccessories);

            return retVal;
        }


        private void SaveToFile_Script(string inFileName)
        {
            int i;

            //ファイルを作る
            StreamWriter aStrWriter = new StreamWriter(inFileName, false);

            for (i = 0; i < ScriptList.Count; i++)
            {
                /* 書き込み */
                aStrWriter.WriteLine("{0},{1},{2},{3},{4}", ScriptList[i].mCommand, ScriptList[i].mParam1, ScriptList[i].mParam2, ScriptList[i].mParam3, ScriptList[i].mParam4);

            }

            aStrWriter.Close();
        }

        private void ReadFromFile_Script(string inFileName)
        {

            string[] aFields;

            ScriptList.Clear();

            //設定ファイルがあるときのみ読み込みします。
            if (File.Exists(inFileName))
            {

                TextFieldParser aParser = new TextFieldParser(inFileName);
                aParser.TextFieldType = FieldType.Delimited;
                aParser.SetDelimiters(",");

                while (aParser.EndOfData == false)
                {

                    /* 分析処理する */
                    ScriptData aItem = new ScriptData();

                    aFields = aParser.ReadFields();

                    aItem.mCommand = DSCommon.GetCSVFieldString(aFields, 0, Program.SCRIPTCMD_EXIT);
                    aItem.mParam1 = DSCommon.GetCSVFieldString(aFields, 1, "0");
                    aItem.mParam2 = DSCommon.GetCSVFieldString(aFields, 2, "0");
                    aItem.mParam3 = DSCommon.GetCSVFieldInt(aFields, 3, 0);

                    /* 追加 */
                    ScriptList.Add(aItem);
                }

                aParser.Close();

            }
        }

        private void RunScriptMain()
        {
            bool aBreak;

            if (ScriptMode != Program.SCRIPTMODE_RUN)
            {
                return;
            }

            if (ScriptWaitTime <= 0)
            {
                while (ScriptCurrentLine < ScriptList.Count)
                {

                    aBreak = ProcessScript(ScriptList[ScriptCurrentLine]);

                    /* 次の行へ */
                    ScriptCurrentLine = ScriptCurrentLine + 1;

                    if (aBreak == true)
                    {
                        break;
                    }
                }

                if (ScriptMode == Program.SCRIPTMODE_RUN)
                {
                    StatusLabel.Text = LangManager.SetText("TxtRunning", "Running.") + " " + LangManager.SetText("TxtLine", "Line") + ": " + ScriptCurrentLine.ToString() + ".";
                }

            }
            else
            {

                /* 待機する */
                ScriptWaitTime = ScriptWaitTime - 1;

            }
        }

        private void ExitScript()
        {
            /* スクリプト処理停止 */
            timerScript.Enabled = false;

            ScriptMode = Program.SCRIPTMODE_STOP;

            buttonScriptRun.Text = LangManager.SetText("TxtRun", "Run");
            buttonScriptRun.Enabled = true;

            buttonScriptTeach.Text = LangManager.SetText("TxtTeach", "Learning Mode");
            buttonScriptTeach.Enabled = true;
            buttonScriptLoad.Enabled = true;
            buttonScriptSave.Enabled = true;

            listScript.BackColor = SystemColors.Window;
            StatusLabel.Text = LangManager.SetText("TxtFinish", "Done.");

        }

        private bool ProcessScript(ScriptData inData)
        {
            bool aResult = false;

            if (inData.mCommand == Program.SCRIPTCMD_WAIT)
            {
                ScriptWaitTime = DSCommon.ParseStrToInt(inData.mParam1);
                aResult = true;
            }

            if (inData.mCommand == Program.SCRIPTCMD_SPEED)
            {
                SerialCmd.SetLocoSpeed(DSCommon.ParseStrToInt(inData.mParam1), DSCommon.ParseStrToInt(inData.mParam2), 0);

                /* 管理リストに反映 */
                UpdateLocList(DSCommon.ParseStrToInt(inData.mParam1), DSCommon.ParseStrToInt(inData.mParam2), -1, -1, -1);
            }

            if (inData.mCommand == Program.SCRIPTCMD_DIRECTION)
            {
                SerialCmd.SetLocoDirection(DSCommon.ParseStrToInt(inData.mParam1), DSCommon.ParseStrToInt(inData.mParam2));

                /* 管理リストに反映 */
                UpdateLocList(DSCommon.ParseStrToInt(inData.mParam1), -1, DSCommon.ParseStrToInt(inData.mParam2), -1, -1);
            }

            if (inData.mCommand == Program.SCRIPTCMD_FUNCTION)
            {
                SerialCmd.SetLocoFunction(DSCommon.ParseStrToInt(inData.mParam1), DSCommon.ParseStrToInt(inData.mParam2), inData.mParam3);

                /* 管理リストに反映 */
                UpdateLocList(DSCommon.ParseStrToInt(inData.mParam1), -1, -1, DSCommon.ParseStrToInt(inData.mParam2), inData.mParam3);
            }

            if (inData.mCommand == Program.SCRIPTCMD_ACCESSORY)
            {
                SerialCmd.SetTurnout(DSCommon.ParseStrToInt(inData.mParam1), DSCommon.ParseStrToInt(inData.mParam2));

                /* 管理リストに反映 */
                UpdateAccList(DSCommon.ParseStrToInt(inData.mParam1), DSCommon.ParseStrToInt(inData.mParam2));

            }

            if (inData.mCommand == Program.SCRIPTCMD_POWER)
            {
                gControlMode = SerialCmd.SetPower(DSCommon.ParseStrToInt(inData.mParam1));
            }

            if (inData.mCommand == Program.SCRIPTCMD_EXIT)
            {
                ExitScript();
            }

            if (inData.mCommand == Program.SCRIPTCMD_GOTO)
            {
                ScriptCurrentLine = DSCommon.ParseStrToInt(inData.mParam1) - 1;
                aResult = true;
            }

            return aResult;
        }

        private void UpdateScriptDisplay()
        {
            int i;
            string aParam1, aParam2, aParam3;

            listScript.Items.Clear();

            for (i = 0; i < ScriptList.Count; i++)
            {
                ListViewItem aItem = new ListViewItem();
                aItem.Text = i.ToString();

                //表示変換処理
                getScriptParamName(ScriptList[i], out aParam1, out aParam2, out aParam3);

                aItem.SubItems.Add(ScriptList[i].mCommand);
                aItem.SubItems.Add(aParam1);
                aItem.SubItems.Add(aParam2);
                aItem.SubItems.Add(aParam3);

                listScript.Items.Add(aItem);

            }

        }


        private bool UpdateLocList(int inLocAddr, int inSpeed, int inDirection, int inFunctionNo, int inFunctionValue)
        {
            bool aResult = false;
            int aUpdateRef = Program.PANELUPDATE_NONE;
            int aIndex;
            int i, j;

            for (i = 0; i < LocDB.Items.Count; i++)
            {
                /* 一致した機関車アドレスのデータを取得する */
                if (LocDB.Items[i].mLocAddr == inLocAddr)
                {
                    if (inSpeed >= 0)
                    {
                        LocDB.Items[i].mCurrentSpeed = inSpeed;
                        LocDB.CheckCurrentSpeed(i);

                    }

                    if (inDirection >= 0)
                    {
                        LocDB.Items[i].mCurrentDirection = inDirection;
                    }

                    if (inFunctionNo >= 0)
                    {
                        LocDB.Items[i].mFunctionStatus[inFunctionNo] = inFunctionValue;
                    }

                    /* 表示中のものと一致する場合 */
                    if (SelectedLocIndex == i)
                    {
                        aUpdateRef = Program.PANELUPDATE_LOC;
                    }

                    for (j = 0; j < Program.MULTICONTROL_MAX; j++)
                    {
                        aIndex = gAppSettings.mLocCtrlList[j] - 1;

                        if (aIndex >= 0)
                        {

                            if (LocDB.Items[aIndex].mLocAddr == inLocAddr)
                            {
                                aUpdateRef = Program.PANELUPDATE_LOC;

                                break;
                            }
                        }

                    }

                    /* 画面更新処理を実行 */
                    flagUpdateByS88Sign = flagUpdateByS88Sign | aUpdateRef;

                    aResult = true;

                    break;
                }
            }

            return aResult;
        }

        private bool UpdateAccList(int inAccAddr, int inDirection)
        {
            bool aResult = false;

            if ((inDirection >= 0) && (inAccAddr > 0) && (inAccAddr <= Program.ACCESSORIES_MAX))
            {
                gAppSettings.mAccList[inAccAddr - 1].SetAccDirection(inDirection);

                /*画面更新用*/
                flagUpdateByS88Sign = flagUpdateByS88Sign | Program.PANELUPDATE_ACC;
                
                aResult = true;
            }

            return aResult;
        }


        private void UpdateLocDisplay()
        {
            int i;
            int aSelectIndex;

            aSelectIndex = SelectedLocIndex;

            cBox_LocChange.Items.Clear();

            for (i = 0; i < LocDB.Items.Count; i++)
            {
                /* 入れるテキストは何でも良い。表示側で自動変更する。 */
                cBox_LocChange.Items.Add(LocDB.Items[i].mLocName);
            }

            //不正な位置を修正（マイナス側）
            if (aSelectIndex < 0)
            {
                aSelectIndex = 0;
            }

            //不正な位置を修正(リスト末尾)
            if (aSelectIndex >= cBox_LocChange.Items.Count)
            {
                aSelectIndex = cBox_LocChange.Items.Count - 1;
            }

            SelectedLocIndex = aSelectIndex;
            cBox_LocChange.SelectedIndex = aSelectIndex;

        }


        private void ControlAvailability(bool inAvailable)
        {

            button_F1.Enabled = inAvailable;
            button_F2.Enabled = inAvailable;
            button_F3.Enabled = inAvailable;
            button_F4.Enabled = inAvailable;
            button_F5.Enabled = inAvailable;
            button_F6.Enabled = inAvailable;
            button_F7.Enabled = inAvailable;
            button_F8.Enabled = inAvailable;
            button_F9.Enabled = inAvailable;
            button_F10.Enabled = inAvailable;
            button_F11.Enabled = inAvailable;
            button_F12.Enabled = inAvailable;
            button_F13.Enabled = inAvailable;
            button_F14.Enabled = inAvailable;
            button_F15.Enabled = inAvailable;
            button_F16.Enabled = inAvailable;
            button_FWD.Enabled = inAvailable;
            button_REV.Enabled = inAvailable;

            //button_LocDBLoad.Enabled = inAvailable;
            //button_LocDBSave.Enabled = inAvailable;
            //cBox_LocChange.Enabled = inAvailable;
            //button_NewLoc.Enabled = inAvailable;
            //button_DelLoc.Enabled = inAvailable;
            //button_AnyLocs.Enabled = inAvailable;
            
            cBox_AccFilter.Enabled = inAvailable;
            timerSpeed.Enabled = inAvailable;
            timerDraw.Enabled = inAvailable;
            timerUpdateAll.Enabled = inAvailable;

            //SButton_Loc.Enabled = inAvailable;
            SButton_Acc.Enabled = inAvailable;
            SButton_KPAD.Enabled = inAvailable;
            //SButton_Layout.Enabled = inAvailable;
            SButton_Sequence.Enabled = inAvailable;
            SButton_CVEditor.Enabled = inAvailable;
            SButton_MULTILOCS.Enabled = inAvailable;
            //SButton_S88.Enabled = inAvailable;
            SButton_Crane.Enabled = inAvailable;
            SButton_MScreen.Enabled = inAvailable & ((gAppSettings.mScreenNums > 0) ? true : false) & (ScaleRation == 100);
            LeverBox.Enabled = inAvailable;

            /* ボトムバーのボタンの表示可否をモードや電源状態で制御する */
            button_MultiFunc0.Enabled = inAvailable;
            button_MultiFunc1.Enabled = inAvailable;

            if ((gControlMode == Program.POWER_OFF) || (gAppSettings.mSideFuncBottom == Program.BOTTOMBAR_NONE))
            {
                button_MultiFunc0.Visible = false;
                button_MultiFunc1.Visible = false;
            }
            else
            {
                button_MultiFunc0.Visible = true;
                button_MultiFunc1.Visible = true;
            }


            /* 6021KeyPad */
            button6021_0.Enabled = inAvailable;
            button6021_1.Enabled = inAvailable;
            button6021_2.Enabled = inAvailable;
            button6021_3.Enabled = inAvailable;
            button6021_4.Enabled = inAvailable;
            button6021_5.Enabled = inAvailable;
            button6021_6.Enabled = inAvailable;
            button6021_7.Enabled = inAvailable;
            button6021_8.Enabled = inAvailable;
            button6021_9.Enabled = inAvailable;
            button6021_F1.Enabled = inAvailable;
            button6021_F2.Enabled = inAvailable;
            button6021_F3.Enabled = inAvailable;
            button6021_F4.Enabled = inAvailable;
            button6021_FWD.Enabled = inAvailable;
            button6021_REV.Enabled = inAvailable;
            button6021_OFF.Enabled = inAvailable;
            cBox_6021Protcol.Enabled = inAvailable;

            /* レイアウト */
            //button_LayoutNew.Enabled = inAvailable;
            //button_LayoutLoad.Enabled = inAvailable;
            //button_LayoutSave.Enabled = inAvailable;

            /* シーケンス */
            buttonScriptLoad.Enabled = inAvailable;
            buttonScriptRun.Enabled = inAvailable;
            buttonScriptSave.Enabled = inAvailable;
            buttonScriptTeach.Enabled = inAvailable;
            listScript.Enabled = inAvailable;

            /* アクセサリ制御 */
            buttonEditAccOption.Enabled = inAvailable;
            AccScrollBar.Enabled = inAvailable;

            /* 複数台制御 */
            panel_MultiLocs.SuspendLayout();
            buttonMultiF1.Enabled = inAvailable;
            buttonMultiF2.Enabled = inAvailable;
            buttonMultiF3.Enabled = inAvailable;
            buttonMultiF4.Enabled = inAvailable;
            buttonMultiF5.Enabled = inAvailable;
            buttonMultiF6.Enabled = inAvailable;
            buttonMultiF7.Enabled = inAvailable;
            buttonMultiF8.Enabled = inAvailable;
            buttonMultiF9.Enabled = inAvailable;
            buttonMultiF10.Enabled = inAvailable;
            buttonMultiF11.Enabled = inAvailable;
            buttonMultiF12.Enabled = inAvailable;
            buttonMultiF13.Enabled = inAvailable;
            buttonMultiF14.Enabled = inAvailable;
            buttonMultiF15.Enabled = inAvailable;
            buttonMultiF16.Enabled = inAvailable;
            buttonMLoc_Select0.Enabled = inAvailable;
            buttonMLoc_Select1.Enabled = inAvailable;
            buttonMLoc_Select2.Enabled = inAvailable;
            buttonMLoc_Select3.Enabled = inAvailable;
            buttonMLoc_Select4.Enabled = inAvailable;
            buttonMLoc_Select5.Enabled = inAvailable;
            buttonMLoc_Select6.Enabled = inAvailable;
            buttonMLoc_Select7.Enabled = inAvailable;

            /* S88センサイベント */
            //lBox_S88Events.Enabled = inAvailable;
            //tabS88Events.Enabled = inAvailable;
            //button_S88EventsImport.Enabled = inAvailable;
            //button_S88EventsExport.Enabled = inAvailable;

            /* CV読み書き */
            cBox_CVProtcol.Enabled = inAvailable;
            buttonCVRead.Enabled = inAvailable;
            buttonCVWrite.Enabled = inAvailable;
            groupBox_CVEditor.Enabled = inAvailable;
            panel_CVEditor.Enabled = inAvailable;


            /* Crane */
            EnableCranePanelUI(cBox_CraneType.SelectedIndex, inAvailable, true);


            panel_MultiLocs.ResumeLayout(false);

        }


        private void SetNextSpeed(int inIndex)
        {
            int aHighAddressExists;
            int aLocSpeed;

            /* 重連のアドレスを取得する */
            aHighAddressExists = LocDB.GetAddress(LocDB.Items[inIndex].mLocAddr_dbl);

            aLocSpeed = LocDB.Items[inIndex].CurrentSpeed();

            /* 速度セット */
            SerialCmd.SetLocoSpeed(LocDB.Items[inIndex].mLocAddr, aLocSpeed, LocDB.Items[inIndex].mLocSpeedstep);

            if (aHighAddressExists > 0)
            {
                SerialCmd.SetLocoSpeed(LocDB.Items[inIndex].mLocAddr_dbl, aLocSpeed, LocDB.Items[inIndex].mDoubleLoc[0].LocSpeedstep);
            }

        }

        private bool ReserveSpeed(int inIndex, int inNextSpeed)
        {
            int aSpeed;

            aSpeed = inNextSpeed;


            if (aSpeed <= 0)
            {
                aSpeed = 0;
            }

            if (aSpeed >= LocDB.Items[inIndex].mLocMaxSpeed)
            {
                aSpeed = LocDB.Items[inIndex].mLocMaxSpeed;
            }

            /*　前回値と異なる場合  */
            if (LocDB.Items[inIndex].mCurrentSpeed != aSpeed)
            {

                LocDB.Items[inIndex].mCurrentSpeed = aSpeed;

                /* 速度反映予約 */
                LocDB.Items[inIndex].SetUpdateNextInterval();

                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IncrementSpeedLever(int inIndex)
        {
            int aNextSpeed;

            aNextSpeed = LocDB.Items[inIndex].CurrentSpeed() + Program.SPEED_INCREMENTAL;

            return ReserveSpeed(inIndex, aNextSpeed);
        }

        private bool DecrementSpeedLever(int inIndex)
        {
            int aNextSpeed;

            aNextSpeed = LocDB.Items[inIndex].CurrentSpeed() - Program.SPEED_INCREMENTAL;

            return ReserveSpeed(inIndex, aNextSpeed);

        }

        private void IncrementPowerLever()
        {
            if (LeverValue < gAppSettings.mSpeedGears)
            {
                LeverValue = LeverValue + 1;
            }

            /* レバーを描画 */
            LeverBox.Refresh();
        }

        private void DecrementPowerLever()
        {
            if (LeverValue > (-1 * gAppSettings.mSpeedGears))
            {
                LeverValue = LeverValue - 1;
            }

            LeverBox.Refresh();
        }

        private void UpdatePowerLever(int inValue)
        {
            LeverValue = inValue;

            LeverBox.Refresh();
        }

        private void UpdateFunctionButtons(int inLocIndex, bool inShifted)
        {
            int aShiftIndex = 0;

            if (inLocIndex < 0)
            {
                return;
            }


            if (inShifted == true)
            {
                aShiftIndex = 16;

            }

            /* 画像情報 */
            button_F1.ImageIndex = (LocDB.Items[inLocIndex].mFunctionImageTable[0 + aShiftIndex] >> 1);
            button_F2.ImageIndex = (LocDB.Items[inLocIndex].mFunctionImageTable[1 + aShiftIndex] >> 1);
            button_F3.ImageIndex = (LocDB.Items[inLocIndex].mFunctionImageTable[2 + aShiftIndex] >> 1);
            button_F4.ImageIndex = (LocDB.Items[inLocIndex].mFunctionImageTable[3 + aShiftIndex] >> 1);
            button_F5.ImageIndex = (LocDB.Items[inLocIndex].mFunctionImageTable[4 + aShiftIndex] >> 1);
            button_F6.ImageIndex = (LocDB.Items[inLocIndex].mFunctionImageTable[5 + aShiftIndex] >> 1);
            button_F7.ImageIndex = (LocDB.Items[inLocIndex].mFunctionImageTable[6 + aShiftIndex] >> 1);
            button_F8.ImageIndex = (LocDB.Items[inLocIndex].mFunctionImageTable[7 + aShiftIndex] >> 1);
            button_F9.ImageIndex = (LocDB.Items[inLocIndex].mFunctionImageTable[8 + aShiftIndex] >> 1);
            button_F10.ImageIndex = (LocDB.Items[inLocIndex].mFunctionImageTable[9 + aShiftIndex] >> 1);
            button_F11.ImageIndex = (LocDB.Items[inLocIndex].mFunctionImageTable[10 + aShiftIndex] >> 1);
            button_F12.ImageIndex = (LocDB.Items[inLocIndex].mFunctionImageTable[11 + aShiftIndex] >> 1);
            button_F13.ImageIndex = (LocDB.Items[inLocIndex].mFunctionImageTable[12 + aShiftIndex] >> 1);

            if (inShifted == false)
            {
                button_F14.ImageIndex = (LocDB.Items[inLocIndex].mFunctionImageTable[13] >> 1);
                button_F15.ImageIndex = (LocDB.Items[inLocIndex].mFunctionImageTable[14] >> 1);
                button_F16.ImageIndex = (LocDB.Items[inLocIndex].mFunctionImageTable[15] >> 1);
            }
            else
            {
                button_F14.ImageIndex = 0;
                button_F15.ImageIndex = 0;
                button_F16.ImageIndex = 0;
            }

            button_F1.Checked = LocDB.Items[inLocIndex].mFunctionStatus[0 + aShiftIndex] > 0 ? (true) : (false);
            button_F2.Checked = LocDB.Items[inLocIndex].mFunctionStatus[1 + aShiftIndex] > 0 ? (true) : (false);
            button_F3.Checked = LocDB.Items[inLocIndex].mFunctionStatus[2 + aShiftIndex] > 0 ? (true) : (false);
            button_F4.Checked = LocDB.Items[inLocIndex].mFunctionStatus[3 + aShiftIndex] > 0 ? (true) : (false);
            button_F5.Checked = LocDB.Items[inLocIndex].mFunctionStatus[4 + aShiftIndex] > 0 ? (true) : (false);
            button_F6.Checked = LocDB.Items[inLocIndex].mFunctionStatus[5 + aShiftIndex] > 0 ? (true) : (false);
            button_F7.Checked = LocDB.Items[inLocIndex].mFunctionStatus[6 + aShiftIndex] > 0 ? (true) : (false);
            button_F8.Checked = LocDB.Items[inLocIndex].mFunctionStatus[7 + aShiftIndex] > 0 ? (true) : (false);
            button_F9.Checked = LocDB.Items[inLocIndex].mFunctionStatus[8 + aShiftIndex] > 0 ? (true) : (false);
            button_F10.Checked = LocDB.Items[inLocIndex].mFunctionStatus[9 + aShiftIndex] > 0 ? (true) : (false);
            button_F11.Checked = LocDB.Items[inLocIndex].mFunctionStatus[10 + aShiftIndex] > 0 ? (true) : (false);
            button_F12.Checked = LocDB.Items[inLocIndex].mFunctionStatus[11 + aShiftIndex] > 0 ? (true) : (false);
            button_F13.Checked = LocDB.Items[inLocIndex].mFunctionStatus[12 + aShiftIndex] > 0 ? (true) : (false);

            if (inShifted == false)
            {
                button_F14.Checked = LocDB.Items[inLocIndex].mFunctionStatus[13] > 0 ? (true) : (false);
                button_F15.Checked = LocDB.Items[inLocIndex].mFunctionStatus[14] > 0 ? (true) : (false);
                button_F16.Checked = LocDB.Items[inLocIndex].mFunctionStatus[15] > 0 ? (true) : (false);
            }
            else
            {
                button_F14.Checked = false;
                button_F15.Checked = false;
                button_F16.Checked = false;
            }
        }

        private void UpdateFunctionButtons_MultiLocos(int inLocIndex, bool inShifted)
        {
            int aShiftIndex = 0;

            if (inLocIndex < 0)
            {
                return;
            }

            if (inShifted == true)
            {
                aShiftIndex = 16;

            }

            /* 画像情報 */
            buttonMultiF1.ImageIndex = (LocDB.Items[inLocIndex].mFunctionImageTable[0 + aShiftIndex] >> 1);
            buttonMultiF2.ImageIndex = (LocDB.Items[inLocIndex].mFunctionImageTable[1 + aShiftIndex] >> 1);
            buttonMultiF3.ImageIndex = (LocDB.Items[inLocIndex].mFunctionImageTable[2 + aShiftIndex] >> 1);
            buttonMultiF4.ImageIndex = (LocDB.Items[inLocIndex].mFunctionImageTable[3 + aShiftIndex] >> 1);
            buttonMultiF5.ImageIndex = (LocDB.Items[inLocIndex].mFunctionImageTable[4 + aShiftIndex] >> 1);
            buttonMultiF6.ImageIndex = (LocDB.Items[inLocIndex].mFunctionImageTable[5 + aShiftIndex] >> 1);
            buttonMultiF7.ImageIndex = (LocDB.Items[inLocIndex].mFunctionImageTable[6 + aShiftIndex] >> 1);
            buttonMultiF8.ImageIndex = (LocDB.Items[inLocIndex].mFunctionImageTable[7 + aShiftIndex] >> 1);
            buttonMultiF9.ImageIndex = (LocDB.Items[inLocIndex].mFunctionImageTable[8 + aShiftIndex] >> 1);
            buttonMultiF10.ImageIndex = (LocDB.Items[inLocIndex].mFunctionImageTable[9 + aShiftIndex] >> 1);
            buttonMultiF11.ImageIndex = (LocDB.Items[inLocIndex].mFunctionImageTable[10 + aShiftIndex] >> 1);
            buttonMultiF12.ImageIndex = (LocDB.Items[inLocIndex].mFunctionImageTable[11 + aShiftIndex] >> 1);
            buttonMultiF13.ImageIndex = (LocDB.Items[inLocIndex].mFunctionImageTable[12 + aShiftIndex] >> 1);

            if (inShifted == false)
            {
                buttonMultiF14.ImageIndex = (LocDB.Items[inLocIndex].mFunctionImageTable[13] >> 1);
                buttonMultiF15.ImageIndex = (LocDB.Items[inLocIndex].mFunctionImageTable[14] >> 1);
                buttonMultiF16.ImageIndex = (LocDB.Items[inLocIndex].mFunctionImageTable[15] >> 1);
            }
            else
            {
                buttonMultiF14.ImageIndex = 0;
                buttonMultiF15.ImageIndex = 0;
                buttonMultiF16.ImageIndex = 0;
            }

            buttonMultiF1.Checked = LocDB.Items[inLocIndex].mFunctionStatus[0 + aShiftIndex] > 0 ? (true) : (false);
            buttonMultiF2.Checked = LocDB.Items[inLocIndex].mFunctionStatus[1 + aShiftIndex] > 0 ? (true) : (false);
            buttonMultiF3.Checked = LocDB.Items[inLocIndex].mFunctionStatus[2 + aShiftIndex] > 0 ? (true) : (false);
            buttonMultiF4.Checked = LocDB.Items[inLocIndex].mFunctionStatus[3 + aShiftIndex] > 0 ? (true) : (false);
            buttonMultiF5.Checked = LocDB.Items[inLocIndex].mFunctionStatus[4 + aShiftIndex] > 0 ? (true) : (false);
            buttonMultiF6.Checked = LocDB.Items[inLocIndex].mFunctionStatus[5 + aShiftIndex] > 0 ? (true) : (false);
            buttonMultiF7.Checked = LocDB.Items[inLocIndex].mFunctionStatus[6 + aShiftIndex] > 0 ? (true) : (false);
            buttonMultiF8.Checked = LocDB.Items[inLocIndex].mFunctionStatus[7 + aShiftIndex] > 0 ? (true) : (false);
            buttonMultiF9.Checked = LocDB.Items[inLocIndex].mFunctionStatus[8 + aShiftIndex] > 0 ? (true) : (false);
            buttonMultiF10.Checked = LocDB.Items[inLocIndex].mFunctionStatus[9 + aShiftIndex] > 0 ? (true) : (false);
            buttonMultiF11.Checked = LocDB.Items[inLocIndex].mFunctionStatus[10 + aShiftIndex] > 0 ? (true) : (false);
            buttonMultiF12.Checked = LocDB.Items[inLocIndex].mFunctionStatus[11 + aShiftIndex] > 0 ? (true) : (false);
            buttonMultiF13.Checked = LocDB.Items[inLocIndex].mFunctionStatus[12 + aShiftIndex] > 0 ? (true) : (false);

            if (inShifted == false)
            {
                buttonMultiF14.Checked = LocDB.Items[inLocIndex].mFunctionStatus[13] > 0 ? (true) : (false);
                buttonMultiF15.Checked = LocDB.Items[inLocIndex].mFunctionStatus[14] > 0 ? (true) : (false);
                buttonMultiF16.Checked = LocDB.Items[inLocIndex].mFunctionStatus[15] > 0 ? (true) : (false);
            }
            else
            {
                buttonMultiF14.Checked = false;
                buttonMultiF15.Checked = false;
                buttonMultiF16.Checked = false;
            }
        }

        private void DrawLeverBoxSpeed(Graphics inCanvas)
        {
            int aLocMaxSpeed;
            int aLocCurrentSpeed;

            /* 最高速度値の基準 */
            if (SelectedLocIndex < 0)
            {
                aLocMaxSpeed = Program.SPEED_MAX;
                aLocCurrentSpeed = 0;
            }
            else
            {
                aLocMaxSpeed = LocDB.Items[SelectedLocIndex].mLocMaxSpeed;
                aLocCurrentSpeed = LocDB.Items[SelectedLocIndex].CurrentSpeed();
            }

            MeterDrawer.DrawLeverBox_Speed(inCanvas, aLocCurrentSpeed, aLocMaxSpeed);

        }



        private Point GetDialPoints(int inCx, int inCy, int inRadius, int inRadius2, float inTheta)
        {
            double aX1, aY1;
            double aX2, aY2;

            aX1 = inCx + inRadius * Math.Cos(inTheta);
            aY1 = inCy + inRadius * Math.Sin(inTheta);

            aX2 = aX1 + inRadius2 * Math.Cos(inTheta - Math.PI / 2);
            aY2 = aY1 + inRadius2 * Math.Sin(inTheta - Math.PI / 2);

            return new Point((int)aX2, (int)aY2);
        }

        private string GetLocTitle(int inIndex)
        {
            int aLowAddress;
            int aHighAddress;
            string aText;

            if (inIndex >= LocDB.Items.Count)
            {
                return "";
            }

            /* 重連のアドレスを取得する */
            aLowAddress = LocDB.GetAddress(LocDB.Items[inIndex].mLocAddr);
            aHighAddress = LocDB.GetAddress(LocDB.Items[inIndex].mLocAddr_dbl);

            /* 重連か否かで表示を変える */
            if (aHighAddress == 0)
            {
                aText = aLowAddress.ToString() + ":" + LocDB.Items[inIndex].mLocName;
            }
            else
            {
                aText = aLowAddress.ToString() + "/" + aHighAddress.ToString() + ":" + LocDB.Items[inIndex].mLocName;
            }

            return aText;

        }

        private void SetPictureBox(PictureBox inPicBox, string inFilename, bool inDefault)
        {
            if (File.Exists(inFilename) == true)
            {
                inPicBox.ImageLocation = inFilename;

            }
            else
            {
                inPicBox.ImageLocation = "";

                if (inDefault == true)
                {
                    //inPicBox.Image = Properties.Resources.InitialLocIcon;
                    inPicBox.Image = LocImageBox0.InitialImage;
                }
            }
        }

        private void UpdateLocMultiCtrl(int inCtrlNo)
        {
            int aIndex;

            aIndex = gAppSettings.mLocCtrlList[inCtrlNo] - 1;

            switch (inCtrlNo)
            {
                case 0:
                    pBox_MLocName0.Refresh();
                    if ((aIndex < 0) || (aIndex >= LocDB.Items.Count))
                    {
                        SetPictureBox(LocImageBox0, "", false);
                    }
                    else
                    {
                        SetPictureBox(LocImageBox0, DSCommon.SetOmitImageFilePathAndName(LocDB.Items[aIndex].mIconFile,Application.StartupPath + "\\images"), true);
                    }
                    break;

                case 1:
                    pBox_MLocName1.Refresh();
                    if ((aIndex < 0) || (aIndex >= LocDB.Items.Count))
                    {
                        SetPictureBox(LocImageBox1, "", false);
                    }
                    else
                    {
                        SetPictureBox(LocImageBox1, DSCommon.SetOmitImageFilePathAndName(LocDB.Items[aIndex].mIconFile, Application.StartupPath + "\\images"), true);
                    }
                    break;

                case 2:
                    pBox_MLocName2.Refresh();
                    if ((aIndex < 0) || (aIndex >= LocDB.Items.Count))
                    {
                        SetPictureBox(LocImageBox2, "", false);
                    }
                    else
                    {
                        SetPictureBox(LocImageBox2, DSCommon.SetOmitImageFilePathAndName(LocDB.Items[aIndex].mIconFile, Application.StartupPath + "\\images"), true);
                    }
                    break;

                case 3:
                    pBox_MLocName3.Refresh();
                    if ((aIndex < 0) || (aIndex >= LocDB.Items.Count))
                    {
                        SetPictureBox(LocImageBox3, "", false);
                    }
                    else
                    {
                        SetPictureBox(LocImageBox3, DSCommon.SetOmitImageFilePathAndName(LocDB.Items[aIndex].mIconFile, Application.StartupPath + "\\images"), true);
                    }
                    break;

                case 4:
                    pBox_MLocName4.Refresh();
                    if ((aIndex < 0) || (aIndex >= LocDB.Items.Count))
                    {
                        SetPictureBox(LocImageBox4, "", false);
                    }
                    else
                    {
                        SetPictureBox(LocImageBox4, DSCommon.SetOmitImageFilePathAndName(LocDB.Items[aIndex].mIconFile, Application.StartupPath + "\\images"), true);
                    }
                    break;

                case 5:
                    pBox_MLocName5.Refresh();
                    if ((aIndex < 0) || (aIndex >= LocDB.Items.Count))
                    {
                        SetPictureBox(LocImageBox5, "", false);
                    }
                    else
                    {
                        SetPictureBox(LocImageBox5, DSCommon.SetOmitImageFilePathAndName(LocDB.Items[aIndex].mIconFile, Application.StartupPath + "\\images"), true);
                    }
                    break;

                case 6:
                    pBox_MLocName6.Refresh();
                    if ((aIndex < 0) || (aIndex >= LocDB.Items.Count))
                    {
                        SetPictureBox(LocImageBox6, "", false);
                    }
                    else
                    {
                        SetPictureBox(LocImageBox6, DSCommon.SetOmitImageFilePathAndName(LocDB.Items[aIndex].mIconFile, Application.StartupPath + "\\images"), true);
                    }
                    break;

                case 7:
                    pBox_MLocName7.Refresh();
                    if ((aIndex < 0) || (aIndex >= LocDB.Items.Count))
                    {
                        SetPictureBox(LocImageBox7, "", false);
                    }
                    else
                    {
                        SetPictureBox(LocImageBox7, DSCommon.SetOmitImageFilePathAndName(LocDB.Items[aIndex].mIconFile, Application.StartupPath + "\\images"), true);
                    }
                    break;
            }

        }


        private void DrawMultiLocSpeedCtrlBoxSpeed(Graphics inCanvas, int inIndex, int inScaleRatio)
        {
            int aX, aY, bX, bY;
            int i;
            int aLocMaxSpeed;
            int aLocCurrentSpeed;
            int aLocSpeedDisplay;

            /* 最高速度値の基準 */
            if ((inIndex < 0) || (inIndex >= LocDB.Items.Count))
            {
                aLocMaxSpeed = Program.SPEED_MAX;
                aLocCurrentSpeed = 0;
                aLocSpeedDisplay = 0;
            }
            else
            {
                aLocMaxSpeed = LocDB.Items[inIndex].mLocMaxSpeed;
                aLocCurrentSpeed = (LocDB.Items[inIndex].CurrentSpeed() * 49 / aLocMaxSpeed);
                aLocSpeedDisplay = (LocDB.Items[inIndex].CurrentSpeed() * LocDB.Items[inIndex].mDisplayMaxSpeed / aLocMaxSpeed);
            }

            //(アンチエイリアス処理されたレタリング)を指定する
            inCanvas.SmoothingMode = SmoothingMode.AntiAlias;
            inCanvas.TextRenderingHint = TextRenderingHint.AntiAlias;

            //ペンオブジェクトの作成
            Pen aPen = new Pen(Color.LightGray);
            aPen.Width = Program.PEN_WIDTH * inScaleRatio / 100;

            /* ハンドル稼動部を書く */
            for (i = 0; i < 48; i++)
            {

                aY = 57 - i;
                aX = i * 5;
                bY = 57;
                bX = aX;

                aPen.Width = 2;

                if (aLocCurrentSpeed > i)
                {
                    aPen.Color = Color.FromArgb(127 + i * 128 / 48, 255, 0, 0);
                }
                else
                {
                    aPen.Color = Color.FromArgb(50 + i * 205 / 48, 90, 90, 90);
                }

                inCanvas.DrawLine(aPen, aX * inScaleRatio / 100, aY * inScaleRatio / 100, bX * inScaleRatio / 100, bY * inScaleRatio / 100);
            }

            /* 文字系 */
            System.Drawing.Font aDrawFont = new System.Drawing.Font("Arial", 10 * inScaleRatio / 100, FontStyle.Bold);
            StringFormat aDrawFormat = new StringFormat();


            inCanvas.DrawString(aLocSpeedDisplay.ToString(), aDrawFont, Brushes.Black, (30 * inScaleRatio / 100) - inCanvas.MeasureString(aLocSpeedDisplay.ToString(), aDrawFont).Width, 5);
            inCanvas.DrawString("km/h", aDrawFont, Brushes.Black, 30 * inScaleRatio / 100, 5);

            aPen.Dispose();
            aDrawFont.Dispose();
            aDrawFormat.Dispose();

        }

        private void button_DelLoc_Click(object sender, EventArgs e)
        {
            if (cBox_LocChange.Items.Count <= 1)
            {

                MessageBox.Show(LangManager.SetText("TxtMsgBoxCantDeleteEvent", "Unable to delete chosen loc. You need to have at least 1 entry in the database"), LangManager.SetText("TxtMsgBoxAttention", "Attention"), MessageBoxButtons.OK);
            }
            else
            {
                /* 削除可能な場合 */

                if (MessageBox.Show(LangManager.SetText("TxtMsgBoxDeleteLoc","Do you want to delete current loc data?"), LangManager.SetText("TxtMsgBoxAttention", "Attention"), MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    LocDB.Items.RemoveAt(SelectedLocIndex);

                    /* リスト更新 */
                    UpdateLocDisplay();
                }
            }
        }

        private void button_NewLoc_Click(object sender, EventArgs e)
        {
            int aLowNewAddress;
            int aHighNewAddress;

            /* 機関車設定フォームを開く */
            LocEditForm aForm = new LocEditForm();

            aForm.SetFormLanguage(LangManager);

            aForm.Text = LangManager.SetText("TxtCfgLocNew","New locomotive properties");
            aForm.cBox_LocName.Text = "";
            aForm.tBox_ArtNo.Text = "";
            aForm.cBox_Manufacture.Text = "";
            aForm.tBox_Addr.Text = "0";
            aForm.tBox_AddrHigh.Text = "0";
            aForm.numUpDown_MaxSpeed.Value = 180;
            aForm.numUpDown_AccRatio.Value = 3;
            aForm.numUpDown_ReduceRatio.Value = 3;
            aForm.numUpDown_LocMaxSpeed.Value = Program.SPEED_MAX;
            aForm.LocImageBox.ImageLocation = "";
            aForm.cBox_ProtcolLoc1.SelectedIndex = 0;
            aForm.cBox_ProtcolLoc2.SelectedIndex = 0;
            aForm.cBox_SpeedStep.SelectedIndex = 0;
            aForm.cBox_Speedstep2.SelectedIndex = 0;

            aForm.ShowDialog(this);

            //押されたボタン別の処理 
            if (aForm.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                LocData aItem = new LocData();

                aItem.mLocName = aForm.cBox_LocName.Text;
                aItem.mLocItemNo = DSCommon.ParseStrToInt(aForm.tBox_ArtNo.Text);
                aItem.mLocManufacture = aForm.cBox_Manufacture.Text;
                aItem.mFunctionImageTable = new int[Program.MAX_FUNCTIONNUM];
                aItem.mFunctionStatus = new int[Program.MAX_FUNCTIONNUM];
                aItem.mExFunctionCommand = new string[Program.MAX_FUNCTIONNUM + 1];
                aItem.mExFunctionData = new string[Program.MAX_FUNCTIONNUM + 1];
                aItem.mFunctionExMethod = new int[Program.MAX_FUNCTIONNUM + 1];
                aItem.mFunctionExAddress = new int[Program.MAX_FUNCTIONNUM + 1];
                aItem.mFunctionExFuncNo = new int[Program.MAX_FUNCTIONNUM + 1];


                aLowNewAddress = DSCommon.ParseStrToInt(aForm.tBox_Addr.Text);
                aHighNewAddress = DSCommon.ParseStrToInt(aForm.tBox_AddrHigh.Text);

                aItem.mLocAddr = LocDB.AssignAddressProtcol(aForm.cBox_ProtcolLoc1.SelectedIndex, aLowNewAddress);
                aItem.mLocAddr_dbl = LocDB.AssignAddressProtcol(aForm.cBox_ProtcolLoc2.SelectedIndex, aHighNewAddress);
                aItem.mDisplayMaxSpeed = decimal.ToInt32(aForm.numUpDown_MaxSpeed.Value);
                aItem.mSpeedAccRatio = decimal.ToInt32(aForm.numUpDown_AccRatio.Value);
                aItem.mSpeedRedRatio = decimal.ToInt32(aForm.numUpDown_ReduceRatio.Value);
                aItem.mLocMaxSpeed = decimal.ToInt32(aForm.numUpDown_LocMaxSpeed.Value);
                aItem.mIconFile = DSCommon.GetOmitImageFileName(aForm.LocImageBox.ImageLocation, Application.StartupPath + "\\images");
                aItem.mMFXUID = DSCommon.ParseStrToUInt32Hex(aForm.tBox_MfxUID.Text);
                aItem.mLocSpeedstep = aForm.cBox_SpeedStep.SelectedIndex;
                aItem.mDoubleLoc[0].LocSpeedstep = aForm.cBox_Speedstep2.SelectedIndex;

                LocDB.Items.Add(aItem);

            }

            /* リスト更新 */
            UpdateLocDisplay();

            //フォームの解放 
            aForm.Dispose();
        }

        private void button_AnyLocs_Click(object sender, EventArgs e)
        {
            int aLowAddress;
            int aHighAddress;
            int aLowLocoProtcolcheck;
            int aHighLocoProtcolcheck;
            int aLowNewAddress;
            int aHighNewAddress;

            /* 重連のアドレスを取得する */
            aLowAddress = LocDB.GetAddress(LocDB.Items[SelectedLocIndex].mLocAddr);
            aHighAddress = LocDB.GetAddress(LocDB.Items[SelectedLocIndex].mLocAddr_dbl);
            aLowLocoProtcolcheck = LocDB.GetAddressLocProtcol(LocDB.Items[SelectedLocIndex].mLocAddr);
            aHighLocoProtcolcheck = LocDB.GetAddressLocProtcol(LocDB.Items[SelectedLocIndex].mLocAddr_dbl);

            /* 機関車設定フォームを開く */
            LocEditForm aForm = new LocEditForm();

            aForm.SetFormLanguage(LangManager);

            aForm.cBox_LocName.Text = LocDB.Items[SelectedLocIndex].mLocName;
            aForm.tBox_ArtNo.Text = LocDB.Items[SelectedLocIndex].mLocItemNo.ToString();
            aForm.cBox_Manufacture.Text = LocDB.Items[SelectedLocIndex].mLocManufacture;
            aForm.tBox_Addr.Text = aLowAddress.ToString();
            aForm.tBox_AddrHigh.Text = aHighAddress.ToString();
            aForm.cBox_ProtcolLoc1.SelectedIndex = aLowLocoProtcolcheck;
            aForm.cBox_ProtcolLoc2.SelectedIndex = aHighLocoProtcolcheck;
            aForm.numUpDown_MaxSpeed.Value = LocDB.Items[SelectedLocIndex].mDisplayMaxSpeed;
            aForm.numUpDown_AccRatio.Value = LocDB.Items[SelectedLocIndex].mSpeedAccRatio;
            aForm.numUpDown_ReduceRatio.Value = LocDB.Items[SelectedLocIndex].mSpeedRedRatio;
            aForm.numUpDown_LocMaxSpeed.Value = Math.Min(LocDB.Items[SelectedLocIndex].mLocMaxSpeed, Program.SPEED_MAX);
            aForm.LocImageBox.ImageLocation = DSCommon.SetOmitImageFilePathAndName(LocDB.Items[SelectedLocIndex].mIconFile, Application.StartupPath + "\\images");

            aForm.tBox_MfxUID.Text = LocDB.Items[SelectedLocIndex].mMFXUID.ToString("X8");
            aForm.cBox_SpeedStep.SelectedIndex = LocDB.Items[SelectedLocIndex].mLocSpeedstep;
            aForm.cBox_Speedstep2.SelectedIndex = LocDB.Items[SelectedLocIndex].mDoubleLoc[0].LocSpeedstep;


            aForm.ShowDialog(this);

            //押されたボタン別の処理 
            if (aForm.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                LocDB.Items[SelectedLocIndex].mLocName = aForm.cBox_LocName.Text;
                LocDB.Items[SelectedLocIndex].mLocItemNo = DSCommon.ParseStrToInt(aForm.tBox_ArtNo.Text);
                LocDB.Items[SelectedLocIndex].mLocManufacture = aForm.cBox_Manufacture.Text;

                aLowNewAddress = DSCommon.ParseStrToInt(aForm.tBox_Addr.Text);
                aHighNewAddress = DSCommon.ParseStrToInt(aForm.tBox_AddrHigh.Text);

                LocDB.Items[SelectedLocIndex].mLocAddr = LocDB.AssignAddressProtcol(aForm.cBox_ProtcolLoc1.SelectedIndex, aLowNewAddress);
                LocDB.Items[SelectedLocIndex].mLocAddr_dbl = LocDB.AssignAddressProtcol(aForm.cBox_ProtcolLoc2.SelectedIndex, aHighNewAddress);
                LocDB.Items[SelectedLocIndex].mDisplayMaxSpeed = decimal.ToInt32(aForm.numUpDown_MaxSpeed.Value);
                LocDB.Items[SelectedLocIndex].mSpeedAccRatio = decimal.ToInt32(aForm.numUpDown_AccRatio.Value);
                LocDB.Items[SelectedLocIndex].mSpeedRedRatio = decimal.ToInt32(aForm.numUpDown_ReduceRatio.Value);
                LocDB.Items[SelectedLocIndex].mLocMaxSpeed = decimal.ToInt32(aForm.numUpDown_LocMaxSpeed.Value);
                LocDB.Items[SelectedLocIndex].mIconFile = DSCommon.GetOmitImageFileName(aForm.LocImageBox.ImageLocation, Application.StartupPath + "\\images");
                LocDB.Items[SelectedLocIndex].mMFXUID = DSCommon.ParseStrToUInt32Hex(aForm.tBox_MfxUID.Text);
                LocDB.Items[SelectedLocIndex].mLocSpeedstep = aForm.cBox_SpeedStep.SelectedIndex;
                LocDB.Items[SelectedLocIndex].mDoubleLoc[0].LocSpeedstep = aForm.cBox_Speedstep2.SelectedIndex;

                /* 現在走行中の場合を想定してMAX超えの場合は自動的に変更する */
                LocDB.CheckCurrentSpeed(SelectedLocIndex);

            }

            /* リスト更新 */
            UpdateLocDisplay();

            //フォームの解放 
            aForm.Dispose();
        }

        private void cBox_LocChange_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedLocIndex = cBox_LocChange.SelectedIndex;

            /* スピードレバーを0の位置にする */
            UpdatePowerLever(0);

            UpdatePanelLocomotiveControl();


            /* 適当なところにフォーカスを逃がす */
            MeterBox.Focus();

        }

        private void UpdatePanelLocomotiveControl()
        {
            SetLocIconFileOnCab(DSCommon.SetOmitImageFilePathAndName(LocDB.Items[SelectedLocIndex].mIconFile, Application.StartupPath + "\\images"));

            /* ボタン画像更新 */
            UpdateFunctionButtons(SelectedLocIndex, false);

            /* メーター更新 */
            UpdateMeterBoxCache();
            MeterBox.Refresh();
            MultiFunctionBox.Refresh();
            /* レバー更新 */
            LeverBox.Refresh();
        }

        private void SetLocIconFileOnCab(string inFileName)
        {
            Image aImage;

            Graphics aCanvas = Graphics.FromImage(LocIconBitmap);
            aCanvas.Clear(Color.Transparent);

            /* 機関車イメージの指定 */
            if (File.Exists(inFileName))
            {
                aImage = Image.FromFile( inFileName);

            }
            else
            {
                aImage = Properties.Resources.InitialLocIcon;

            }
            
            aCanvas.DrawImage(aImage, (LocIconBitmap.Width - aImage.Width) / 2, (LocIconBitmap.Height - aImage.Height) / 2, aImage.Width, aImage.Height);

            aCanvas.Dispose();

            LocIconBitmap.MakeTransparent();

        }

        private void listView1_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            Graphics aCanvas = e.Graphics;


            if (e.Item.Checked)
            {
                checkimgList.Draw(aCanvas, new Point(e.Bounds.Left, e.Bounds.Top + 1), 0);
            }
            else
            {
                checkimgList.Draw(aCanvas, new Point(e.Bounds.Left, e.Bounds.Top + 1), 1);
            }

            int textOffset = 16;
            Rectangle rText = e.Bounds;
            rText.Offset(textOffset, 0);
            rText.Width -= textOffset;

            e.Graphics.DrawString(e.Item.Text, e.Item.Font, Brushes.Black, rText);
        }

        private void timerDraw_Tick(object sender, EventArgs e)
        {
            IntervalDrawLocPanel();

        }

        private void IntervalDrawAllPanel()
        {

            /* 電源オフ時は動かさない */
            if (gControlMode == Program.POWER_OFF)
            {
                return;
            }
                
            if(flagUpdateByS88Sign == Program.PANELUPDATE_NONE)
            {
                return;
            }

            if ((panel_Layout.Visible == true) && (((flagUpdateByS88Sign & Program.PANELUPDATE_LOC) > 0) || ((flagUpdateByS88Sign & Program.PANELUPDATE_ACC) > 0)))
            {

                LayoutBox.Refresh();

                /* フラグの除去 */
                if((flagUpdateByS88Sign & Program.PANELUPDATE_LOC) > 0)
                {
                    flagUpdateByS88Sign = flagUpdateByS88Sign & (3 - Program.PANELUPDATE_LOC);
                }

                if((flagUpdateByS88Sign & Program.PANELUPDATE_ACC) > 0)
                {
                    flagUpdateByS88Sign = flagUpdateByS88Sign & (3 - Program.PANELUPDATE_ACC);
                }

            }
            else if ((panel_Loc.Visible == true) && (((flagUpdateByS88Sign & Program.PANELUPDATE_LOC) > 0) || ((flagUpdateByS88Sign & Program.PANELUPDATE_ACC) > 0)))
            {
                UpdatePanelLocomotiveControl();

                flagUpdateByS88Sign = flagUpdateByS88Sign & (3 - Program.PANELUPDATE_LOC);
            }
            else if ((panel_MultiLocs.Visible == true) && ((flagUpdateByS88Sign & Program.PANELUPDATE_LOC) > 0))
            {

                for (int i = 0; i < 8; i++)
                {
                    RefreshMultiLocSpeedDir(i);
                }

                flagUpdateByS88Sign = flagUpdateByS88Sign & (3 - Program.PANELUPDATE_LOC);

            }
            else if ((panel_AccList.Visible == true) && ((flagUpdateByS88Sign & Program.PANELUPDATE_ACC) > 0))
            {
                pBox_AccList.Refresh();
                
                flagUpdateByS88Sign = flagUpdateByS88Sign & (3 - Program.PANELUPDATE_ACC);
            }



        }

        private void IntervalDrawLocPanel()
        {

            double aLastSpeed;
            double aNextSpeed;
            int aIndex;
            bool aDrawFlag = false;

            /* 電源オフまたはスピード直接指定時は動かさない */
            if ((gControlMode == Program.POWER_OFF) || (gAppSettings.mSpeedLeverMode == 0))
            {
                return;
            }

            aIndex = SelectedLocIndex;

            aLastSpeed = LocDB.Items[aIndex].mCurrentSpeed;
            aNextSpeed = LocDB.Items[aIndex].mCurrentSpeed;

            /* 新しい速度の演算 */
            if (LeverValue < 0)
            {
                /* 減速 */
                aNextSpeed = aNextSpeed + ((double)LeverValue * (10.0 + (double)LocDB.Items[aIndex].mSpeedRedRatio)) / 25.0;
            }
            else
            {
                /* 加速 */
                aNextSpeed = aNextSpeed + ((double)LeverValue * (10.0 + (double)LocDB.Items[aIndex].mSpeedAccRatio)) / 25.0;
            }

            if (aNextSpeed > LocDB.Items[aIndex].mLocMaxSpeed)
            {
                aNextSpeed = LocDB.Items[aIndex].mLocMaxSpeed;
            }
            else if (aNextSpeed > Program.SPEED_MAX)
            {
                aNextSpeed = Program.SPEED_MAX;
            }
            else if (aNextSpeed < 0)
            {
                aNextSpeed = 0;
            }

            if (aNextSpeed != aLastSpeed)
            {
                LocDB.Items[aIndex].mCurrentSpeed = aNextSpeed;
                LocDB.Items[aIndex].SetUpdateNextInterval();

                aDrawFlag = true;
            }

            if( aDrawFlag == true)
            {
                /* メーター更新 */
                MeterBox.Refresh();
            }
        }

        private void SpeedCtrlBox0_Paint(object sender, PaintEventArgs e)
        {
            int aIndex;
            int aTag;

            /* スピードバーの表示 */
            PictureBox aPBox = sender as PictureBox;
            aTag = Convert.ToInt32(aPBox.Tag);

            /* インデックス取得。-1でもOK。 */
            aIndex = gAppSettings.mLocCtrlList[aTag] - 1;

            /* キャンバスを取得 */
            Graphics aCanvas = e.Graphics;

            //(アンチエイリアス処理されたレタリング)を指定する
            aCanvas.SmoothingMode = SmoothingMode.AntiAlias;


            DrawMultiLocSpeedCtrlBoxSpeed(aCanvas, aIndex, ScaleRation);
        }

        private void SpeedCtrlBox0_MouseDown(object sender, MouseEventArgs e)
        {
            int aIndex;
            int aTag;
            int aSpeed;

            if ((gControlMode == Program.POWER_OFF) || (e.Button != System.Windows.Forms.MouseButtons.Left))
            {
                return;
            }

            /* スピードバーの表示 */
            PictureBox aPBox = sender as PictureBox;
            aTag = Convert.ToInt32(aPBox.Tag);

            if (gAppSettings.mLocCtrlList[aTag] <= 0)
            {
                return;
            }

            /* ボタン切り替え */
            ChangeButtonMultiLocs(aTag);

            aIndex = gAppSettings.mLocCtrlList[aTag] - 1;

            /* 速度調整 */

            aSpeed = e.X * LocDB.Items[aIndex].mLocMaxSpeed / aPBox.Width;
            aSpeed = (aSpeed / 16) * 16;

            if (aSpeed <= 0)
            {
                aSpeed = 0;
            }

            if (aSpeed >= LocDB.Items[aIndex].mLocMaxSpeed)
            {
                aSpeed = LocDB.Items[aIndex].mLocMaxSpeed;
            }

            LocDB.Items[aIndex].mCurrentSpeed = aSpeed;

            /* 速度反映予約 */
            LocDB.Items[aIndex].SetUpdateNextInterval();

            /* メータ更新 */
            aPBox.Refresh();
        }


        private void ChangeButtonMultiLocs(int inTag)
        {
            int i;
            int aIndex;

            if (gAppSettings.mLocCtrlList[inTag] <= 0)
            {
                return;
            }

            aIndex = gAppSettings.mLocCtrlList[inTag] - 1;

            UpdateFunctionButtons_MultiLocos(aIndex, false );

            CurrentSelectedMultiLocIndex = inTag;

            for (i = 0; i < Program.MULTICONTROL_MAX; i++)
            {
                RefreshMultiLocSpeedDir(i);
            }

        }

        private void buttonMultiF8_Click(object sender, EventArgs e)
        {
            CheckBox aButton = sender as CheckBox;
            int aTag = -1;
            int aFunctionNo;
            int aIndex = 0;

            aFunctionNo = int.Parse(aButton.Tag.ToString());

            aTag = CurrentSelectedMultiLocIndex;

            /* 該当なしのときの処理 */
            if (aTag < 0)
            {
                return;
            }

            /* 機関車データベースの番号を取得 */
            aIndex = gAppSettings.mLocCtrlList[aTag] - 1;

            /* ファンクション実行処理 */
            RunFunctionButton(aFunctionNo, aIndex, aButton);
        }

        private void UpdateSelectedMultiLocs()
        {

            /* 速度計を更新 */

            switch (CurrentSelectedMultiLocIndex)
            {
                case 0:
                    SpeedCtrlBox0.Refresh();
                    break;
                case 1:
                    SpeedCtrlBox1.Refresh();
                    break;
                case 2:
                    SpeedCtrlBox2.Refresh();
                    break;
                case 3:
                    SpeedCtrlBox3.Refresh();
                    break;
                case 4:
                    SpeedCtrlBox4.Refresh();
                    break;
                case 5:
                    SpeedCtrlBox5.Refresh();
                    break;
                case 6:
                    SpeedCtrlBox6.Refresh();
                    break;
                case 7:
                    SpeedCtrlBox7.Refresh();
                    break;
            }

        }

        private void buttonMLoc_REV0_Click(object sender, EventArgs e)
        {
            /* 進行方向調整 */
            int aTag;
            int aDirection;
            int aIndex;

            

            Button aButton = sender as Button;
            aTag = Convert.ToInt32(aButton.Tag) / 10;

            if (gAppSettings.mLocCtrlList[aTag] <= 0)
            {
                return;
            }

            aDirection = Convert.ToInt32(aButton.Tag) % 10;
            aIndex = gAppSettings.mLocCtrlList[aTag] - 1;

            /* 進行方向をセット */
            SetLocDirection(aIndex, aDirection);

            /* 画面を更新 */
            ChangeButtonMultiLocs(aTag);
        }

        private void RunFunctionButton(int inTag, int inIndex, CheckBox inCheckBox)
        {

            int aSwitchType;
            int aPower;
            int aLocAddress;
            int aFuncNo;
            int aShiftedTagNo = 0;

            if (inIndex < 0)
            {
                return;
            }

            if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
            {
                aShiftedTagNo = 16 + inTag;

                if (aShiftedTagNo > Program.MAX_FUNCTIONNUM)
                {
                    aShiftedTagNo = Program.MAX_FUNCTIONNUM;
                }

            }
            else
            {
                aShiftedTagNo = inTag;
            }

            switch (LocDB.Items[inIndex].mFunctionExMethod[aShiftedTagNo])
            {
                case 1:
                    //擬似ファンクション機能
                    RunFile(LocDB.Items[inIndex].mExFunctionCommand[aShiftedTagNo], LocDB.Items[inIndex].mExFunctionData[aShiftedTagNo]);
                    //擬似ファンクションでは常にMomentary(On/Offの区別無し）
                    inCheckBox.Checked = false;
                    break;

                case 0:
                case 2:

                    aSwitchType = LocDB.Items[inIndex].mFunctionImageTable[aShiftedTagNo] & 0x01;

                    /* 機関車アドレスとファンクションのボタン種別を取得する */
                    if (LocDB.Items[inIndex].mFunctionExMethod[aShiftedTagNo] == 0)
                    {
                        aLocAddress = LocDB.Items[inIndex].mLocAddr;
                        aFuncNo = aShiftedTagNo;
                    }
                    else
                    {
                        aLocAddress = LocDB.Items[inIndex].mFunctionExAddress[aShiftedTagNo];
                        aFuncNo = LocDB.Items[inIndex].mFunctionExFuncNo[aShiftedTagNo];
                    }



                    if (aSwitchType == 0)
                    {
                        /* 値を取得 */
                        aPower = LocDB.Items[inIndex].mFunctionStatus[aShiftedTagNo];

                        /* 反転 */
                        aPower = Math.Abs(aPower - 1);

                        /* モーメンタリ・ファンクション */
                        SerialCmd.SetLocoFunction(aLocAddress, aFuncNo, aPower);

                        /* リストに反映 */
                        LocDB.Items[inIndex].mFunctionStatus[aShiftedTagNo] = aPower;

                        /* ボタンをへこますかどうか */
                        if (aPower == 0)
                        {
                            inCheckBox.Checked = false;
                        }
                        else
                        {
                            inCheckBox.Checked = true;
                        }
                    }
                    else
                    {
                        /* オルタネタリ・ファンクション */
                        SerialCmd.SetLocoFunction(aLocAddress, aFuncNo, 1);

                        //待ち
                        DSCommon.WaitSleepTime(5);

                        /* すぐに切る */
                        SerialCmd.SetLocoFunction(aLocAddress, aFuncNo, 0);

                        inCheckBox.Checked = false;

                    }
                    break;
            }

        }


        private void SetLocDirection(int inIndex, int inDirection)
        {
            int aLowAddress;
            int aHighAddress;
            int aHighAddressExists;
            int aLastLocDirection;

            /* 重連のアドレスを取得する */
            aLowAddress = LocDB.Items[inIndex].mLocAddr;
            aHighAddress = LocDB.Items[inIndex].mLocAddr_dbl;
            aHighAddressExists = LocDB.GetAddress(LocDB.Items[inIndex].mLocAddr_dbl);

            /* 前回値をいったん保存 */
            aLastLocDirection = LocDB.Items[inIndex].mCurrentDirection;

            /* 進行方向 */
            switch (inDirection)
            {
                case 0:
                    SerialCmd.SetLocoDirection(aLowAddress, Program.DIRECTION_FWD);
                    LocDB.Items[inIndex].mCurrentDirection = Program.DIRECTION_FWD;
                    break;
                case 1:
                    SerialCmd.SetLocoDirection(aLowAddress, Program.DIRECTION_REV);
                    LocDB.Items[inIndex].mCurrentDirection = Program.DIRECTION_REV;
                    break;
            }

            /* 重連用(2機目) */
            if (aHighAddressExists > 0)
            {
                switch (inDirection)
                {
                    case 0: SerialCmd.SetLocoDirection(aHighAddress, Program.DIRECTION_FWD); break;
                    case 1: SerialCmd.SetLocoDirection(aHighAddress, Program.DIRECTION_REV); break;
                }
            }

            /* 前回値と異なるときは速度をゼロにセットする */
            if (aLastLocDirection != LocDB.Items[inIndex].mCurrentDirection)
            {
                /* 進行方向が変わるときは速度はゼロになる */
                LocDB.Items[inIndex].mCurrentSpeed = 0;

                /* パワーコントロール時はNに戻す */
                UpdatePowerLever(0);

            }
            else
            {
                /* 何もしない */
            }

        }

        private void ToggleLocDirection(int inIndex)
        {
            int aDirection;

            if (LocDB.Items[inIndex].mCurrentDirection == Program.DIRECTION_FWD)
            {
                aDirection = 1;
            }
            else
            {
                aDirection = 0;
            }

            /* 進行方向セット */
            SetLocDirection(inIndex, aDirection);

        }

        private void label_MLocName0_Click(object sender, EventArgs e)
        {
            int aTag;

            PictureBox aPanel = sender as PictureBox;
            aTag = Convert.ToInt32(aPanel.Tag);

            ChangeButtonMultiLocs(aTag);
        }

        private void buttonMLoc_Select0_Click(object sender, EventArgs e)
        {
            int aTag;
            int i;
            int aTempIndex;
            Button aButton = sender as Button;
            aTag = Convert.ToInt32(aButton.Tag);


            /* 機関車設定フォームを開く */
            SelectLocForm aForm = new SelectLocForm(LocDB);

            aForm.SetFormLanguage(LangManager);

            aForm.cBox_locomotives.Items.Clear();
            aForm.cBox_locomotives.Items.Add(LangManager.SetText("TxtNotSelected", "Not selected"));
            

            for (i = 0; i < LocDB.Items.Count; i++)
            {
                aForm.cBox_locomotives.Items.Add(GetLocTitle(i));
            }

            /* 選択中の機関車のDB番号を取得(+1分オフセットされているので注意) */
            aTempIndex = gAppSettings.mLocCtrlList[aTag];

            /* エラーが無いかチェックする */
            if ((aTempIndex < 0) || (aTempIndex > LocDB.Items.Count))
            {
                aTempIndex = 0;
            }

            aForm.cBox_locomotives.SelectedIndex = aTempIndex;

            /* 変更画面を表示する */
            aForm.ShowDialog(this);

            //押されたボタン別の処理 
            if (aForm.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                gAppSettings.mLocCtrlList[aTag] = aForm.cBox_locomotives.SelectedIndex;

                UpdateLocMultiCtrl(aTag);
            }

            aForm.Dispose();
        }

        private void buttonCVRead_Click(object sender, EventArgs e)
        {
            /* CV 読み書き */

            Button aButton = sender as Button;
            int aTag;
            int aLocAddress;
            int aCVValue = 0;
            int aCVNo = 0;
            int aAddrOffset = 0;

            aTag = int.Parse(aButton.Tag.ToString());

            aCVNo = decimal.ToInt32(numUpDown_CVNo.Value);
            aCVValue = decimal.ToInt32(numUpDown_CVValue.Value);

            aLocAddress = LocDB.AssignAddressProtcol(cBox_CVProtcol.SelectedIndex, 0);

            if ((cBox_DecoderManufacture.SelectedIndex == 1) && (cBox_CVProtcol.SelectedIndex == 2))
            {
                //DS激安DCCデコーダの特殊モード移行用
                aAddrOffset = 0x4000;
            }
            else
            {
                aAddrOffset = 0;
            }

            /* 読み書き処理方向 */
            switch (aTag)
            {
                case 1:
                    /* CV Write */

                    /* 警告表示 */
                    if (MessageBox.Show(LangManager.SetText("TxtCVAttention", "Are you sure you wish to write this CV to the loc decoder?"), LangManager.SetText("TxtCVAttentionTitle", "Attention - Read/Write CV"), MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {

                        if (cBox_DecoderManufacture.SelectedIndex == 2)
                        {
                            /* コマンド送信  */
                            SerialCmd.SetLocoConfigEx(aLocAddress + decimal.ToInt32(numUpDown_CVLocAddr.Value), aCVNo + aAddrOffset, aCVValue, 1);
                        }
                        else
                        {
                            /* コマンド送信  */
                            SerialCmd.SetLocoConfig(aLocAddress, aCVNo + aAddrOffset, aCVValue);
                        }
                    }
                    break;

                case 2:
                    /* CV Read */
                    SerialCmd.GetLocoConfig(aLocAddress, aCVNo);

                    break;
            }
        }

        private void button6021_OFF_Click(object sender, EventArgs e)
        {
            if (button6021_F1.Checked)
            {
                button6021_F1.BackColor = Color.Green;
                button6021_F1.Checked = false;
                SerialCmd.SetLocoFunction(gTypedAddr, 1, 0);
            }

            if (button6021_F2.Checked)
            {
                button6021_F2.BackColor = Color.Green;
                button6021_F2.Checked = false;
                SerialCmd.SetLocoFunction(gTypedAddr, 2, 0);
            }

            if (button6021_F3.Checked)
            {
                button6021_F3.BackColor = Color.Green;
                button6021_F3.Checked = false;
                SerialCmd.SetLocoFunction(gTypedAddr, 3, 0);
            }

            if (button6021_F4.Checked)
            {
                button6021_F4.BackColor = Color.Green;
                button6021_F4.Checked = false;
                SerialCmd.SetLocoFunction(gTypedAddr, 4, 0);
            }
        }

        private void button6021_FWD_Click(object sender, EventArgs e)
        {
            Button aButton = sender as Button;
            int aTag;

            aTag = int.Parse(aButton.Tag.ToString());

            /* 進行方向 */
            if (aTag == 1)
            {
                SerialCmd.SetLocoDirection(gTypedAddr, Program.DIRECTION_FWD);
                label_6021Direct.Text = "FWD";
            }
            else
            {
                SerialCmd.SetLocoDirection(gTypedAddr, Program.DIRECTION_REV);
                label_6021Direct.Text = "REV";
            }
        }

        private void button6021_1_Click(object sender, EventArgs e)
        {
            /* アドレス入力 */
            Button aButton = sender as Button;
            int aTag;

            aTag = int.Parse(aButton.Tag.ToString());

            Update6021LocNum(aTag);

        }

        private void Update6021LocNum(int inTag)
        {
            /* KeyPad */
            int aNumber;
            int aRefAddress;


            aNumber = LocDB.GetAddress(gTypedAddr);


            /* アドレス更新 */
            aRefAddress = (aNumber - ((aNumber / 1000) * 1000)) * 10 + inTag;

            Update6021DisplayAddress(aRefAddress);

        }

        private void Update6021DisplayAddress(int inNewAddress)
        {

            /* プロトコルのアドレス切り替え */
            gTypedAddr = LocDB.AssignAddressProtcol(cBox_6021Protcol.SelectedIndex, inNewAddress);

            /* 表示 */
            label_Addr.Text = inNewAddress.ToString("D4");
            label_6021Direct.Text = "-";
        }

        private void button6021_F1_Click(object sender, EventArgs e)
        {
            CheckBox aButton = (CheckBox)sender;
            int aTag;
            int aPower;

            aTag = int.Parse(aButton.Tag.ToString());


            if (aButton.Checked == true)
            {
                aPower = 1;
                aButton.BackColor = Color.Red;
            }
            else
            {
                aPower = 0;
                aButton.BackColor = Color.Green;
            }

            /* ファンクション */
            SerialCmd.SetLocoFunction(gTypedAddr, aTag, aPower);
        }

        private void Dial6021Box_Paint(object sender, PaintEventArgs e)
        {
            int cx, cy;
            float ax, ay, bx, by;
            int aR = 80;
            float aTheta;
            Point[] pt = new Point[4];
            int i;

            int aWidth = 240;
            int aHeight = 240;


            cx = aWidth / 2;
            cy = aHeight / 2;

            ax = (float)(aWidth - (aR * 2)) / 2;
            ay = (float)(aHeight - (aR * 2)) / 2;
            bx = (float)(aR * 2);
            by = (float)(aR * 2);

            RectangleF aGaugeRect = new RectangleF(ax, ay, bx, by);

            /* 描画 */
            Graphics aCanvas = e.Graphics;

            //(アンチエイリアス処理されたレタリング)を指定する
            aCanvas.SmoothingMode = SmoothingMode.AntiAlias;
            aCanvas.TextRenderingHint = TextRenderingHint.AntiAlias;

            /* 外 */
            LinearGradientBrush aGBrush = new LinearGradientBrush(aGaugeRect, Color.DarkRed, Color.LightSalmon, LinearGradientMode.ForwardDiagonal);
            aCanvas.FillEllipse(aGBrush, aGaugeRect);


            /* 内側1 */
            aGaugeRect.Inflate(-1, -1);

            LinearGradientBrush aGBrush2 = new LinearGradientBrush(aGaugeRect, Color.Salmon, Color.Red, LinearGradientMode.ForwardDiagonal);
            aCanvas.FillEllipse(aGBrush2, aGaugeRect);

            //ペンオブジェクトの作成
            Pen aPen = new Pen(Color.Black);
            aPen.Width = Program.PEN_WIDTH_DBL;

            /* 細い線を描画 */
            aPen.Width = 2;
            for (i = 0; i <= 20; i++)
            {
                aTheta = (float)i * 6 + 30;
                ax = (float)cx + ((aR + Program.DIAL_RADIUS_MIN) * (float)Math.Cos((aTheta - 180) * 2 * (float)Math.PI / 360));
                ay = (float)cy + ((aR + Program.DIAL_RADIUS_MIN) * (float)Math.Sin((aTheta - 180) * 2 * (float)Math.PI / 360));
                bx = (float)cx + ((aR + Program.DIAL_RADIUS_MAX) * (float)Math.Cos((aTheta - 180) * (float)Math.PI / 180));
                by = (float)cy + ((aR + Program.DIAL_RADIUS_MAX) * (float)Math.Sin((aTheta - 180) * (float)Math.PI / 180));

                //直線を描画
                aCanvas.DrawLine(aPen, ax, ay, bx, by);
            }

            /* 太い線を描画 */
            aPen.Width = 4;
            for (i = 0; i <= 5; i++)
            {
                aTheta = (float)i * 24 + 30;
                ax = (float)cx + ((aR + Program.DIAL_RADIUS_MIN) * (float)Math.Cos((aTheta - 180) * 2 * (float)Math.PI / 360));
                ay = (float)cy + ((aR + Program.DIAL_RADIUS_MIN) * (float)Math.Sin((aTheta - 180) * 2 * (float)Math.PI / 360));
                bx = (float)cx + ((aR + Program.DIAL_RADIUS_MAX) * (float)Math.Cos((aTheta - 180) * (float)Math.PI / 180));
                by = (float)cy + ((aR + Program.DIAL_RADIUS_MAX) * (float)Math.Sin((aTheta - 180) * (float)Math.PI / 180));

                //直線を描画
                aCanvas.DrawLine(aPen, ax, ay, bx, by);

            }

            /* 文字系 */
            System.Drawing.Font aDrawFont = new System.Drawing.Font("Arial", 12, FontStyle.Bold);
            StringFormat aDrawFormat = new StringFormat();

            aCanvas.DrawString("0", aDrawFont, Brushes.Black, 15, 55);
            aCanvas.DrawString("50", aDrawFont, Brushes.Black, 45, 18);
            aCanvas.DrawString("100", aDrawFont, Brushes.Black, 80, 0);
            aCanvas.DrawString("150", aDrawFont, Brushes.Black, 130, 0);
            aCanvas.DrawString("200", aDrawFont, Brushes.Black, 175, 18);
            aCanvas.DrawString("250", aDrawFont, Brushes.Black, 213, 55);

            /* 針の描画 */
            aTheta = (float)Math.PI * (30 + Dial6021Value * 120 / 1024) / 180;

            pt[0] = GetDialPoints(cx, cy, 100, 10, aTheta);
            pt[1] = GetDialPoints(cx, cy, 100, -10, aTheta);
            pt[2] = GetDialPoints(cx, cy, 85, 2, (float)Math.PI + aTheta);
            pt[3] = GetDialPoints(cx, cy, 85, -2, (float)Math.PI + aTheta);
            aCanvas.FillPolygon(Brushes.Red, pt);

            //ペンオブジェクトの作成
            Pen aPen2 = new Pen(Color.DarkRed);
            aPen2.Width = 1;
            aCanvas.DrawPolygon(aPen2, pt);


            aPen.Dispose();
            aPen2.Dispose();
            aDrawFont.Dispose();
            aDrawFormat.Dispose();
            aGBrush.Dispose();
            aGBrush2.Dispose();
        }

        private void Dial6021Box_MouseDown(object sender, MouseEventArgs e)
        {
            /* 6021ダイヤル値取得  */

            int aDialValue = 0;
            int cx, cy;
            int ax, ay;
            int rx, ry;
            float aTheta;
            int aTheta_r;
            int aWidth = 240;
            int aHeight = 240;




            if ((gControlMode == Program.POWER_OFF) || ((e.Button != System.Windows.Forms.MouseButtons.Left)))
            {
                return;
            }

            ax = e.X;
            ay = e.Y;

            cx = aWidth / 2;
            cy = aHeight / 2;

            if (ay > cy)
            {
                /* 下側は無視する */
                return;
            }


            rx = ax - cx;
            ry = ay - cy;

            if (Math.Sqrt(rx * rx + ry * ry) < 10)
            {
                /* 円の内側は無視する */
                return;
            }

            aTheta = (float)Math.Atan2(ry, rx) + (float)Math.PI;

            aTheta_r = (int)(aTheta * 180 / Math.PI);

            /* 10deg以上ずれている場合は無視する処理とする */
            if ((aTheta_r < 20) || (aTheta_r > 160))
            {
                return;
            }

            /* 最小最大の調整 */
            if (aTheta_r < 30)
            {
                aTheta_r = 30;
            }

            if (aTheta_r > 150)
            {
                aTheta_r = 150;
            }

            /* 角度から速度値に換算（精度を32dずつにわざと落として処理軽量化） */
            aDialValue = ((aTheta_r - 30) * 32 / 120) * 32;

            if (aDialValue != Dial6021Value)
            {
                /* 速度値を確定 */
                Dial6021Value = aDialValue;
                NextAutoSpeedSet6021 = true;
                NextAutoSpeedAddr6021 = gTypedAddr;

                /* 再描画 */
                Dial6021Box.Refresh();

            }
        }

        private void LayoutBox_MouseMove(object sender, MouseEventArgs e)
        {
            int aTileIndex;

            if ((gControlMode == Program.POWER_OFF) || ((e.Button != System.Windows.Forms.MouseButtons.Left)))
            {
                return;
            }

            /* MouseMove時はマイナスや範囲外も発生するので無視させる */
            if ((e.X < 0) || (e.X >= LayoutBox.Width) || (e.Y < 0) || (e.Y >= LayoutBox.Height))
            {
                return;
            }


            /* インデックスを取得 */
            SelectedLayoutIndex = LayoutMapData.GetLayoutMapIndex(e.X, e.Y, ScaleRation);

            if (SelectedTileIndex >= 2)
            {
                /* タイルを代入 */
                aTileIndex = SelectedTileIndex;

                if (aTileIndex == 2)
                {
                    aTileIndex = 0;
                }

                LayoutMapData.UpdateLayoutTile(SelectedLayoutIndex, aTileIndex);

                /* レイアウトボックスを再描画 */
                LayoutBox.Refresh();

            }
        }

        private void LayoutBox_Paint(object sender, PaintEventArgs e)
        {
            int aX, aY;
            int bX, bY;
            int i, j;
            int aPicIndex;
            int aPicIndex_raw;
            int aAccAddr;
            int aS88Addr;
            int aRouteNo;
            int aTileSize = Program.TILE32_SIZE * ScaleRation / 100;
            int aTileCnt = LayoutMapData.GetMaxCount();

            /* キャンバスを取得 */
            Graphics aCanvas = e.Graphics;

            /* アンチエイリアス有効 */
            aCanvas.SmoothingMode = SmoothingMode.AntiAlias;
            aCanvas.TextRenderingHint = TextRenderingHint.AntiAlias;

            //ペンオブジェクトの作成
            Pen aPenSelect = new Pen(Color.Red);
            aPenSelect.Width = 1;

            /* 文字系 */
            System.Drawing.Font aDrawFont = new System.Drawing.Font("Arial", 12 * ScaleRation / 100, FontStyle.Bold);
            StringFormat aDrawFormat = new StringFormat();
            aDrawFormat.Alignment = StringAlignment.Near;

            /* データを描画 */
            for (i = 0; i < aTileCnt; i++)
            {
                aX = (i % LayoutMapData.Width) * aTileSize;
                aY = (i / LayoutMapData.Width) * aTileSize;

                aPicIndex = LayoutMapData.GetLayoutTile(i);
                aPicIndex_raw = aPicIndex;
                aAccAddr = LayoutMapData.GetLayoutAccNo(i);
                aS88Addr = LayoutMapData.GetLayoutS88No(i);
                aRouteNo = LayoutMapData.GetLayoutRouteNo(i);

                /* アクセサリのとき */
                if ((SelectedTileIndex == 0) && (aAccAddr != 0) && (aPicIndex >= 10))
                {

                    /* 分岐の場合は分岐を描画するように、インデックスを変更する（現在状態を加味） */
                    if (gAppSettings.mAccList[aAccAddr - 1].mDirection == 1)
                    {
                        aPicIndex = aPicIndex + (aPicIndex >= 20 ? 24 : 26);
                    }
                    else
                    {
                        aPicIndex = aPicIndex + (aPicIndex >= 20 ? 24 : 26) + 24;
                    }

                    /* ストレート・カーブの場合は何もしない（後で信号機を描画する） */
                    
                }
                else if ((SelectedTileIndex == 0) && (aRouteNo > 0) && (aPicIndex >= 2))
                {
                    if (Routes.RoutesList.CheckRoute(aRouteNo - 1, true) == true)
                    {
                        if (aPicIndex > 10)
                        {
                            aPicIndex = aPicIndex + 24;
                        }
                    }
                    else
                    {
                        if (aPicIndex > 10)
                        {
                            aPicIndex = aPicIndex + 24 + 24;
                        }
                    }

                }


                /* 線路を描画する */
                if ((aPicIndex >= 2) && (aPicIndex < 128))
                {
                    LayoutMapData.DrawRail(aCanvas, aPicIndex, aX, aY, aTileSize, aTileSize);
                    //aCanvas.DrawImage(TileImgList.Images[aPicIndex], aX, aY, aTileSize, aTileSize);
                }
                else
                {
                    aCanvas.FillRectangle(Brushes.Black, aX, aY, aTileSize, aTileSize);
                }

                /* 信号機を描画する */
                if ((SelectedTileIndex == 0) && (aAccAddr > 0) && (aPicIndex >= 4) && (aPicIndex < 10))
                {

                    /* 分岐の場合は分岐を描画するように、インデックスを変更する（現在状態を加味） */
                    MeterDrawer.DrawSignal(aCanvas, aX, aY, aPicIndex, gAppSettings.mAccList[aAccAddr - 1].mDirection);
                }

                /* S88センサを描画する */
                if ((SelectedTileIndex == 0) && (aS88Addr > 0) && (gAppSettings.mS88Sensor == true))
                {

                    /* 分岐の場合は分岐を描画するように、インデックスを変更する（現在状態を加味） */
                    MeterDrawer.DrawS88Sensor(aCanvas, aX, aY, GetS88Flag(aS88Addr - 1), aPicIndex, ScaleRation);
                }

                /* Route番号を記載する */
                if ((SelectedTileIndex <= 1) && (aRouteNo > 0) && (aPicIndex_raw >= 24) && (aPicIndex_raw < 28))
                {
                    System.Drawing.Font aDrawFont_RT = new System.Drawing.Font("Arial", 6 * ScaleRation / 100, FontStyle.Bold);

                    aCanvas.DrawString("R" + (aRouteNo - 1).ToString(), aDrawFont_RT, Brushes.Red, (float)aX, (float)aY, aDrawFormat);

                }

                /* 編集モードのとき */
                if (SelectedTileIndex == 1)
                {
                    /* 背景を上書きする */
                    if ((aAccAddr >= 1) || ((aS88Addr >= 1) && (gAppSettings.mS88Sensor == true)))
                    {
                        Brush aBrush = new SolidBrush(Color.FromArgb(128, Color.White));
                        aCanvas.FillRectangle(aBrush, aX, aY, aTileSize, aTileSize);
                        aBrush.Dispose();
                    }

                    /* 有効なアドレスのとき */
                    if (aAccAddr >= 1)
                    {
                        /* アドレスを描画する */
                        aCanvas.DrawString(aAccAddr.ToString(), aDrawFont, Brushes.Red, (float)aX, (float)aY, aDrawFormat);

                    }

                    if ((aS88Addr >= 1) && (gAppSettings.mS88Sensor == true))
                    {
                        /* アドレスを描画する */
                        aCanvas.DrawString(aS88Addr.ToString(), aDrawFont, Brushes.Blue, (float)aX, (float)aY + (aTileSize >> 1), aDrawFormat);

                    }
                }

                /* 選択しているとき */
                if ((SelectedLayoutIndex == i) && (SelectedTileIndex != 0))
                {
                    aCanvas.DrawRectangle(aPenSelect, aX + 1, aY + 1, aTileSize - 2, aTileSize - 2);

                }
            }

            /* 分岐はどちらが選択されているか判定する */

            //ペンオブジェクトの作成
            Pen aPen = new Pen(Color.LightGray);
            aPen.Width = 1;

            //(アンチエイリアス処理されたレタリング)を指定する
            aCanvas.SmoothingMode = SmoothingMode.AntiAlias;


            /* グリッドを描画 */

            for (i = 0; i <= LayoutMapData.Height; i++)
            {
                aX = 0;
                aY = i * aTileSize;
                bX = LayoutMapData.Width * aTileSize;
                bY = aY;

                //直線を描画
                aCanvas.DrawLine(aPen, aX, aY, bX, bY);

            }

            for (j = 0; j <= LayoutMapData.Width; j++)
            {
                aX = j * aTileSize;
                aY = 0;
                bX = aX;
                bY = LayoutMapData.Height * aTileSize;

                //直線を描画
                aCanvas.DrawLine(aPen, aX, aY, bX, bY);
            }

            /* デコレーションの描画 */


            for ( i = 0; i < LayoutMapData.SpecialItems.Count; i++)
            {
                SpecialData aData = LayoutMapData.SpecialItems[i];

                //編集モードの時にマークを表示
                if (SelectedTileIndex == 1)
                {
                    aX = aData.X * aTileSize + 1;
                    aY = aData.Y * aTileSize + 1;
                    bX = aX + 8;
                    bY = aY + 8;

                    //直線を描画
                    aCanvas.DrawRectangle(aPenSelect, aX, aY, 8, 8);
                    aCanvas.DrawLine(aPenSelect, aX, aY, bX, bY);
                }

                if (aData.Enable == true)
                {
                    if (aData.Text != "")
                    {
                        float aYPos = (aTileSize - aCanvas.MeasureString(aData.Text, aDrawFont).Height) / 2;



                        aCanvas.DrawString(aData.Text, aDrawFont, Brushes.White, aData.X * aTileSize, aData.Y * aTileSize + aYPos, aDrawFormat);
                    }

                }

            }



            aPen.Dispose();
            aPenSelect.Dispose();
            aDrawFormat.Dispose();
            aDrawFont.Dispose();

        }

        private void TileBox_Paint(object sender, PaintEventArgs e)
        {
            int aX, aY;
            int i;
            int aTileSize = Program.TILE32_SIZE * ScaleRation / 100;
            int aCntMax = LayoutMapData.GetMaxCount();

            /* キャンバスを取得 */
            Graphics aCanvas = e.Graphics;

            //ペンオブジェクトの作成
            Pen aPen = new Pen(Color.Red);
            aPen.Width = 2;

            

            for (i = 0; i < aCntMax; i++)
            {
                aX = (i % 2) * aTileSize;
                aY = (i / 2) * aTileSize;

                //タイル画像を描画
                LayoutMapData.DrawRail(aCanvas, i, aX, aY, aTileSize, aTileSize);
                //aCanvas.DrawImage(TileImgList.Images[i], aX, aY, aTileSize, aTileSize);

                if (SelectedTileIndex == i)
                {
                    aCanvas.DrawRectangle(aPen, aX + 1, aY + 1, aTileSize - 2, aTileSize - 2);

                }
            }

            aPen.Dispose();
        }

        private void TileBox_MouseDown(object sender, MouseEventArgs e)
        {
            int aX, aY;
            int aTileSize = Program.TILE32_SIZE * ScaleRation / 100;

            /*
            if (gControlMode == Program.POWER_OFF)
            {
                return;
            }
             * */

            /* XYのインデックス番号に変換 */
            aX = e.X / aTileSize;
            aY = e.Y / aTileSize;

            /* 1次元のインデックス番号を確定 */
            SelectedTileIndex = aY * 2 + aX;

            /* タイルボックスを再描画 */
            TileBox.Refresh();
            LayoutBox.Refresh();
        }

        private void LayoutBox_MouseDown(object sender, MouseEventArgs e)
        {
            int aAccAddr;
            int aTileIndex;

            /* インデックスを取得 */
            SelectedLayoutIndex = LayoutMapData.GetLayoutMapIndex(e.X, e.Y, ScaleRation);

            if ((SelectedTileIndex == 0) && (gControlMode == Program.POWER_ON))
            {

                switch (LayoutMapData.GetLayoutTile(SelectedLayoutIndex))
                {
                    case 24:
                    case 25:
                    case 26:
                    case 27:
                        /* ルート切り替え */
                        int aRouteNo = LayoutMapData.GetLayoutRouteNo(SelectedLayoutIndex);
                        if (aRouteNo > 0)
                        {
                            Routes.RoutesList.SetRoute(aRouteNo - 1, true);
                        }
                        break;

                    default:

                        /* プレイモード, 電源ON時のみ動作 */
                        aAccAddr = LayoutMapData.GetLayoutAccNo(SelectedLayoutIndex);

                        /* 分岐を動かす（） */
                        if ((aAccAddr > 0) && (gAppSettings.mAccList.Count >= aAccAddr))
                        {
                            gAppSettings.mAccList[aAccAddr - 1].ReverserDirection();
                            SerialCmd.SetTurnout(aAccAddr, gAppSettings.mAccList[aAccAddr - 1].GetAccDirection());
                        }
                        break;
                }

            }
            else if (SelectedTileIndex == 1)
            {
                /* 編集モード(何もしない) */

            }
            else if ((SelectedTileIndex >= 2) && (e.Button == System.Windows.Forms.MouseButtons.Left))
            {
                /* タイルを代入 */
                aTileIndex = SelectedTileIndex;

                if (aTileIndex == 2)
                {
                    aTileIndex = 0;
                }

                LayoutMapData.UpdateLayoutTile(SelectedLayoutIndex, aTileIndex);

            }

            /* レイアウトボックスを再描画 */
            LayoutBox.Refresh();
        }

        private void LayoutBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int aAccAddr;
            int aS88Addr;
            int aX, aY;

            SelectedLayoutIndex = LayoutMapData.GetLayoutMapIndex(e.X, e.Y, ScaleRation);
            LayoutMapData.GetLayoutMapPos(e.X, e.Y, ScaleRation, out aX, out aY);

            if (SelectedTileIndex == 1)
            {
                /* 編集モード */
                aAccAddr = LayoutMapData.GetLayoutAccNo(SelectedLayoutIndex);
                aS88Addr = LayoutMapData.GetLayoutS88No(SelectedLayoutIndex);

                /* 設定フォームを開く */
                NumInputForm aForm = new NumInputForm(Routes.RoutesList);

                aForm.SetFormLanguage(LangManager);

                /* デコレーションデータ確認 */
                int aDecoIndex = LayoutMapData.GetSpecialItemNo(SelectedLayoutIndex);
                if (aDecoIndex >= 0)
                {
                    aForm.textBoxDecoUserText.Text = LayoutMapData.SpecialItems[aDecoIndex].Text;
                    aForm.numUpDownX.Value = LayoutMapData.SpecialItems[aDecoIndex].X;
                    aForm.numUpDownY.Value = LayoutMapData.SpecialItems[aDecoIndex].Y;
                    aForm.cBox_UseDeco.Checked = LayoutMapData.SpecialItems[aDecoIndex].Enable;
                    aForm.labelDecoImageFile.Text = LayoutMapData.SpecialItems[aDecoIndex].ImageFile;
                }
                else
                {
                    //新規追加
                    aForm.cBox_UseDeco.Checked = false;
                    aForm.numUpDownX.Value = aX;
                    aForm.numUpDownY.Value = aY;
                    aForm.labelDecoImageFile.Text = "";
                    aForm.textBoxDecoUserText.Text = "";
                }


                //コンボボックスへのルートの登録
                aForm.AddRoutes();
                int aRouteNo = LayoutMapData.GetLayoutRouteNo(SelectedLayoutIndex);

                if (aRouteNo < 0)
                {
                    aRouteNo = 0;
                }

                if (aRouteNo > Routes.RoutesList.ListItems.Count)
                {
                    aRouteNo = 0;
                }

                aForm.comboBox_Routes.SelectedIndex = aRouteNo;


                if ((LayoutMapData.GetLayoutTile(SelectedLayoutIndex) >= 24) && (LayoutMapData.GetLayoutTile(SelectedLayoutIndex) <= 27))
                {
                    //ルート編集モード
                    aForm.tabRoute.Enabled = true;
                    aForm.gBox_addr.Enabled = false;



                    aForm.ShowDialog(this);

                    //押されたボタン別の処理 
                    if (aForm.DialogResult == System.Windows.Forms.DialogResult.OK)
                    {
                        LayoutMapData.SetLayoutMapRouteNo(SelectedLayoutIndex, aForm.comboBox_Routes.SelectedIndex);

                    }

                }
                else
                {
                    //通常編集モード
                    aForm.tabRoute.Enabled = true;
                    aForm.gBox_addr.Enabled = true;

                    aForm.addressUpDown.Value = aAccAddr;
                    aForm.S88sensorUpDown.Value = aS88Addr;

                    /* S88を使用しない場合は編集不可にする */
                    aForm.S88sensorUpDown.Enabled = gAppSettings.mS88Sensor;

                    aForm.ShowDialog(this);

                    //押されたボタン別の処理 
                    if (aForm.DialogResult == System.Windows.Forms.DialogResult.OK)
                    {
                        aAccAddr = decimal.ToInt32(aForm.addressUpDown.Value);

                        //データを格納
                        LayoutMapData.UpdateLayoutAccNo(SelectedLayoutIndex, aAccAddr);
                        LayoutMapData.SetLayoutMapRouteNo(SelectedLayoutIndex, aForm.comboBox_Routes.SelectedIndex);

                        if (gAppSettings.mS88Sensor == true)
                        {
                            aS88Addr = decimal.ToInt32(aForm.S88sensorUpDown.Value);
                            LayoutMapData.UpdateLayoutS88Addr(SelectedLayoutIndex, aS88Addr);
                        }

                        //デコレーションファイル保存

                        if ((aDecoIndex < 0) && (aForm.cBox_UseDeco.Checked == true))
                        {
                            SpecialData aNewData = new SpecialData();
                            aNewData.Enable = aForm.cBox_UseDeco.Checked;
                            aNewData.X = aX;
                            aNewData.Y = aY;
                            aNewData.Text = aForm.textBoxDecoUserText.Text;
                            aNewData.ImageFile = aForm.labelDecoImageFile.Text;

                            LayoutMapData.SpecialItems.Add(aNewData);
                        }
                        else if (aDecoIndex >= 0)
                        {
                            LayoutMapData.SpecialItems[aDecoIndex].Enable = aForm.cBox_UseDeco.Checked;
                            LayoutMapData.SpecialItems[aDecoIndex].X = decimal.ToInt32(aForm.numUpDownX.Value);
                            LayoutMapData.SpecialItems[aDecoIndex].Y = decimal.ToInt32(aForm.numUpDownY.Value);
                            LayoutMapData.SpecialItems[aDecoIndex].Text = aForm.textBoxDecoUserText.Text;
                            LayoutMapData.SpecialItems[aDecoIndex].ImageFile = aForm.labelDecoImageFile.Text;
                        }
                        else
                        {
                            /* 何もしない */

                        }


                    }


                }

                //フォームの解放 
                aForm.Close();

            }

            /* レイアウトボックスを再描画 */
            LayoutBox.Refresh();
        }

        private void button_LayoutLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog aForm = new OpenFileDialog();
            aForm.CheckFileExists = true;
            aForm.Filter = "Layout map file(*.map)|*.map|All files(*.*)|*.*";

            //押されたボタン別の処理 
            if (aForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                LayoutMapData.LoadFromFile(aForm.FileName);

                string aTempRouteFile = System.IO.Path.ChangeExtension(aForm.FileName, "rte");

                if (System.IO.File.Exists(aTempRouteFile) == true)
                {
                    Routes.LoadFromFile(aTempRouteFile);
                }

                /* レイアウト更新 */
                LayoutBox.Width = LayoutMapData.Width * (Program.TILE32_SIZE * ScaleRation / 100) + 1;
                LayoutBox.Height = LayoutMapData.Height * (Program.TILE32_SIZE * ScaleRation / 100) + 1;

                LayoutBox.Refresh();
            }

            aForm.Dispose();
        }

        private void button_LayoutSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog aForm = new SaveFileDialog();
            aForm.OverwritePrompt = true;
            aForm.Filter = "Layout map file(*.map)|*.map|All files(*.*)|*.*";

            //押されたボタン別の処理 
            if (aForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                LayoutMapData.SaveToFile(aForm.FileName);

                string aTempRouteFile = System.IO.Path.ChangeExtension(aForm.FileName, "rte");

                Routes.SaveToFile(aTempRouteFile);
            }

            aForm.Dispose();
        }

        private void button_LayoutNew_Click(object sender, EventArgs e)
        {

            /* たずねる */
            if (MessageBox.Show(LangManager.SetText("TxtMsgBoxClearLayout", "Would you like to clear current layout?"), LangManager.SetText("TxtMsgBoxAttention", "Attention"), MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {

                SizeInput aForm = new SizeInput();

                aForm.numUpDown_Width.Value = 26;
                aForm.numUpDown_Height.Value = 14;

                if (aForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {

                    LayoutMapData.Width = decimal.ToInt32(aForm.numUpDown_Width.Value);
                    LayoutMapData.Height = decimal.ToInt32(aForm.numUpDown_Height.Value);
                    LayoutMapData.Clear();

                    Routes.Clear();

                    /* レイアウト更新 */
                    LayoutBox.Width = LayoutMapData.Width * Program.TILE32_SIZE * ScaleRation / 100;
                    LayoutBox.Height = LayoutMapData.Height * Program.TILE32_SIZE * ScaleRation / 100;
                    LayoutBox.Refresh();
                }
            }

        }

        private void LeverBox_Paint(object sender, PaintEventArgs e)
        {
            /* キャンバスを取得 */
            Graphics aCanvas = e.Graphics;

            //(アンチエイリアス処理されたレタリング)を指定する
            aCanvas.SmoothingMode = SmoothingMode.AntiAlias;
            aCanvas.TextRenderingHint = TextRenderingHint.AntiAlias;


            if (gAppSettings.mSpeedLeverMode == 0)
            {
                DrawLeverBoxSpeed(aCanvas);

            }
            else if (gAppSettings.mSpeedLeverMode == 1)
            {
                MeterDrawer.DrawLeverBoxPower(aCanvas, gAppSettings.mSpeedGears, LeverValue, gControlMode, ScaleRation);

            }
        }

        private void LeverBox_MouseDown(object sender, MouseEventArgs e)
        {
            int aIndex;
            int aMax;
            int aMin;
            int aCount;

            /* 目盛り数を取得 */
            if ((gControlMode == Program.POWER_OFF) || (e.Button != System.Windows.Forms.MouseButtons.Left))
            {
                return;
            }

            if (gAppSettings.mSpeedLeverMode == 0)
            {
                /* 速度指定モード時 */
                if (e.X >= Program.LEVER_PIN_LEFT)
                {

                    aMax = LocDB.Items[SelectedLocIndex].mLocMaxSpeed;


                    if (e.Y < Program.LEVER_BAR_TOP)
                    {
                        aCount = 0;
                    }
                    else if (e.Y > (Program.LEVER_HEIGHT + Program.LEVER_BAR_TOP))
                    {
                        aCount = aMax;

                    }
                    else
                    {
                        aCount = (e.Y - Program.LEVER_BAR_TOP) * aMax / (Program.LEVER_HEIGHT);


                    }

                    /* 速度のセット（タイマーで自動で反映される） */
                    LocDB.Items[SelectedLocIndex].mCurrentSpeed = (aMax - aCount);
                    LocDB.Items[SelectedLocIndex].SetUpdateNextInterval();

                    /* レバー更新 */
                    UpdatePowerLever(0);

                    /* メーター更新 */
                    MeterBox.Refresh();
                }

            }
            else
            {
                /* パワー制御モード時 */
                if ((e.X >= Program.LEVER_PIN_LEFT) && (e.Y >= 0) && (e.Y < LeverBox.Height))
                {
                    aMin = gAppSettings.mSpeedGears;
                    aMax = gAppSettings.mSpeedGears;

                    aCount = aMax + aMin + 1;

                    aIndex = e.Y * aCount / LeverBox.Height;

                    UpdatePowerLever(aMax - aIndex);

                    /* メーター更新 */
                    MeterBox.Refresh();

                }
            }

        }

        private void timerScript_Tick(object sender, EventArgs e)
        {
            switch (ScriptMode)
            {
                case Program.SCRIPTMODE_RUN: RunScriptMain();
                    break;
                case Program.SCRIPTMODE_TEACH:
                    ScriptWaitTime = ScriptWaitTime + 1;
                    break;
            }
        }

        private void buttonScriptRun_Click(object sender, EventArgs e)
        {
            if (ScriptMode == Program.SCRIPTMODE_STOP)
            {
                /* スクリプト実行 */

                buttonScriptRun.Text = LangManager.SetText("TxtStop", "STOP");

                ScriptWaitTime = 0;
                ScriptCurrentLine = 0;
                ScriptMode = Program.SCRIPTMODE_RUN;

                buttonScriptTeach.Enabled = false;
                buttonScriptLoad.Enabled = false;
                buttonScriptSave.Enabled = false;

                listScript.BackColor = Color.AliceBlue;

                /* スクリプト処理開始 */
                timerScript.Enabled = true;
            }
            else
            {
                /* スクリプト停止 */

                ExitScript();

            }
        }

        private void buttonScriptTeach_Click(object sender, EventArgs e)
        {
            if (ScriptMode == Program.SCRIPTMODE_STOP)
            {
                /* スクリプト実行 */

                if (MessageBox.Show(LangManager.SetText("TxtMsgBoxTeachingMode", "Would you like to initiate the learning mode?"), LangManager.SetText("TxtMsgBoxAttention", "Attention"), MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {

                    buttonScriptTeach.Text = LangManager.SetText("TxtStop", "STOP");

                    ScriptWaitTime = 0;
                    ScriptMode = Program.SCRIPTMODE_TEACH;
                    ScriptList.Clear();

                    buttonScriptRun.Enabled = false;
                    buttonScriptLoad.Enabled = false;
                    buttonScriptSave.Enabled = false;

                    StatusLabel.Text = LangManager.SetText("TxtTeach", "Learning Mode");

                    /* スクリプト処理開始 */
                    timerScript.Enabled = true;
                }
            }
            else
            {
                /* スクリプト停止 */

                /* 終了コマンドをセット */
                ScriptData aItem = new ScriptData();
                aItem.mCommand = "EXIT";
                ScriptList.Add(aItem);

                /* スクリプト処理停止 */
                ExitScript();

                /* 表示 */
                UpdateScriptDisplay();
            }
        }

        private void buttonScriptLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog aForm = new OpenFileDialog();
            aForm.CheckFileExists = true;
            aForm.Filter = "Script file(*.csv)|*.csv|All files(*.*)|*.*";

            //押されたボタン別の処理 
            if (aForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ReadFromFile_Script(aForm.FileName);

                /* 表示 */
                UpdateScriptDisplay();
            }

            aForm.Dispose();
        }

        private void buttonScriptSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog aForm = new SaveFileDialog();
            aForm.OverwritePrompt = true;
            aForm.Filter = "Script file(*.csv)|*.csv|All files(*.*)|*.*";

            //押されたボタン別の処理 
            if (aForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SaveToFile_Script(aForm.FileName);
            }

            aForm.Dispose();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i;
            int aDeleteItemCount;

            aDeleteItemCount = listScript.SelectedItems.Count;

            for (i = aDeleteItemCount - 1; i >= 0; i--)
            {
                ScriptList.RemoveAt(listScript.SelectedItems[i].Index);
                listScript.Items.Remove(listScript.SelectedItems[i]);
            }

        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int aIndex;

            if (listScript.SelectedItems.Count <= 0)
            {
                return;
            }

            aIndex = listScript.SelectedItems[0].Index;

            if (OpenScriptEdit(ScriptList[aIndex], true) == true)
            {

                /* スクリプト登録 */
                UpdateScriptDisplay();

            }

        }

        private bool OpenScriptEdit(ScriptData inData, bool inLocAddrNotAutoReplaced)
        {
            bool aResult = false;
            string aCommand;
            int i;

            aCommand = inData.mCommand;

            /* スクリプト編集 */
            ScriptEditForm aForm = new ScriptEditForm();

            aForm.SetFormLanguage(LangManager);

            aForm.panel_ScriptAcc.Visible = false;
            aForm.panel_ScriptDirect.Visible = false;
            aForm.panel_ScriptLocFunc.Visible = false;
            aForm.panel_ScriptLocSpeed.Visible = false;
            aForm.panel_ScriptPwr.Visible = false;
            aForm.panel_ScriptWait.Visible = false;
            aForm.panel_ScriptLine.Visible = false;
            aForm.panel_Jump.Visible = false;
            aForm.panel_LineLabel.Visible = false;
            aForm.panel_SetFlag.Visible = false;
            aForm.panel_Free.Visible = false;
            aForm.panel_RunFile.Visible = false;
            aForm.panelRoute.Visible = false;
            aForm.panelJumpRun.Visible = false;

            aForm.cBox_AddrReplacedLocSpd.Enabled = !inLocAddrNotAutoReplaced;
            aForm.cBox_AddrReplacedLocFnc.Enabled = !inLocAddrNotAutoReplaced;
            aForm.cBox_AddrReplacedLocDir.Enabled = !inLocAddrNotAutoReplaced;
            aForm.cBox_AddrReplacedLocJump.Enabled = !inLocAddrNotAutoReplaced;

            if (aCommand == Program.SCRIPTCMD_SPEED)
            {
                aForm.panel_ScriptLocSpeed.Visible = true;

                if ((inLocAddrNotAutoReplaced == false) && (LocDB.GetAddress(DSCommon.ParseStrToInt(inData.mParam1)) <= 0) && (inData.mParam1.Contains("SLOT.") == false))
                {
                    aForm.cBox_AddrReplacedLocSpd.Checked = true;
                }
                else
                {
                    aForm.cBox_AddrReplacedLocSpd.Checked = false;
                }

                aForm.cBox_LocAddressSpd.Enabled = !aForm.cBox_AddrReplacedLocSpd.Checked;
                aForm.cBox_ProtcolLocSpd.Enabled = !aForm.cBox_AddrReplacedLocSpd.Checked;

                if (inData.mParam1.Contains("SLOT.") == true)
                {
                    aForm.cBox_LocAddressSpd.Text = inData.mParam1;
                }
                else
                {
                    aForm.cBox_LocAddressSpd.Text = LocDB.GetAddress(DSCommon.ParseStrToInt(inData.mParam1)).ToString();
                    aForm.cBox_ProtcolLocSpd.SelectedIndex = LocDB.GetAddressLocProtcol(DSCommon.ParseStrToInt(inData.mParam1));
                }

                aForm.numericSpeed.Value = DSCommon.ParseStrToInt(inData.mParam2);
                aForm.numUpDown_TransitionTime.Value = inData.mParam3;
            }

            if (aCommand == Program.SCRIPTCMD_DIRECTION)
            {
                aForm.panel_ScriptDirect.Visible = true;

                if ((inLocAddrNotAutoReplaced == false) && (LocDB.GetAddress(DSCommon.ParseStrToInt(inData.mParam1)) <= 0) && (inData.mParam1.Contains("SLOT.") == false))
                {
                    aForm.cBox_AddrReplacedLocDir.Checked = true;
                }
                else
                {
                    aForm.cBox_AddrReplacedLocDir.Checked = false;
                }

                aForm.cBox_LocAddressDir.Enabled = !aForm.cBox_AddrReplacedLocDir.Checked;
                aForm.cBox_ProtcolLocDir.Enabled = !aForm.cBox_AddrReplacedLocDir.Checked;

                if (inData.mParam1.Contains("SLOT.") == true)
                {
                    aForm.cBox_LocAddressDir.Text = inData.mParam1;
                }
                else
                {
                    aForm.cBox_LocAddressDir.Text = LocDB.GetAddress(DSCommon.ParseStrToInt(inData.mParam1)).ToString();
                    aForm.cBox_ProtcolLocDir.SelectedIndex = LocDB.GetAddressLocProtcol(DSCommon.ParseStrToInt(inData.mParam1));
                }


                switch (DSCommon.ParseStrToInt(inData.mParam2))
                {
                    case Program.DIRECTION_FWD: aForm.cBox_Direction.SelectedIndex = 0; break;
                    case Program.DIRECTION_REV: aForm.cBox_Direction.SelectedIndex = 1; break;
                    default:
                        aForm.cBox_Direction.SelectedIndex = 0;
                        break;
                }
            }

            if (aCommand == Program.SCRIPTCMD_FUNCTION)
            {
                aForm.panel_ScriptLocFunc.Visible = true;

                if ((inLocAddrNotAutoReplaced == false) && (LocDB.GetAddress(DSCommon.ParseStrToInt(inData.mParam1)) <= 0) && (inData.mParam1.Contains("SLOT.") == false))
                {
                    aForm.cBox_AddrReplacedLocFnc.Checked = true;
                }
                else
                {
                    aForm.cBox_AddrReplacedLocFnc.Checked = false;
                }

                aForm.cBox_LocAddressFnc.Enabled = !aForm.cBox_AddrReplacedLocFnc.Checked;
                aForm.cBox_ProtcolLocFnc.Enabled = !aForm.cBox_AddrReplacedLocFnc.Checked;

                if (inData.mParam1.Contains("SLOT.") == true)
                {
                    aForm.cBox_LocAddressFnc.Text = inData.mParam1;
                }
                else
                {
                    aForm.cBox_LocAddressFnc.Text = LocDB.GetAddress(DSCommon.ParseStrToInt(inData.mParam1)).ToString();
                    aForm.cBox_ProtcolLocFnc.SelectedIndex = LocDB.GetAddressLocProtcol(DSCommon.ParseStrToInt(inData.mParam1));
                }

                aForm.cBox_FunctionNo.SelectedIndex = DSCommon.ParseStrToInt(inData.mParam2);
                aForm.cBox_FunctionPower.SelectedIndex = inData.mParam3;
            }

            if (aCommand == Program.SCRIPTCMD_ACCESSORY)
            {
                aForm.panel_ScriptAcc.Visible = true;

                aForm.numericAccAddress.Value = DSCommon.ParseStrToInt(inData.mParam1);
                aForm.cBox_AccPower.SelectedIndex = DSCommon.ParseStrToInt(inData.mParam2);
            }

            if (aCommand == Program.SCRIPTCMD_POWER)
            {
                aForm.panel_ScriptPwr.Visible = true;

                aForm.cBox_Power.SelectedIndex = DSCommon.ParseStrToInt(inData.mParam1);
            }

            if ((aCommand == Program.SCRIPTCMD_WAIT) || (aCommand == Program.SCRIPTCMD_WAITRND))
            {
                aForm.panel_ScriptWait.Visible = true;

                aForm.numericWaitTime.Value = DSCommon.ParseStrToInt(inData.mParam1);
            }

            if (aCommand == Program.SCRIPTCMD_WAITIF)
            {
                aForm.panel_Free.Visible = true;
                aForm.label_FreeDescription.Text = "IF command example:\nFLAG.1==10\nS88.12!=0";

                aForm.textBox_FreeCommand.Text = inData.mParam1;
            }

            if (aCommand == Program.SCRIPTCMD_EXIT)
            {
                //MessageBox.Show("This Function doesn't have a parameter.", "Information", MessageBoxButtons.OK);

                return false;
            }

            if (aCommand == Program.SCRIPTCMD_GOTO)
            {
                aForm.panel_ScriptLine.Visible = true;

                aForm.cBox_LineNoLabel.Text = inData.mParam1;
            }

            if (aCommand == Program.SCRIPTCMD_GOTOIF)
            {
                aForm.panel_Jump.Visible = true;

                aForm.cBox_JumpLabelName.Text = inData.mParam1;
                aForm.tBox_JumpFlagNo.Text = inData.mParam2;

                //ラベルを書き換える
                aForm.label_FlagNo.Text = "IF Condition";
                aForm.numUpDown_JumpValue.Visible = false;
                aForm.label_EquivVal.Visible = false;

            }



            if (aCommand == Program.SCRIPTCMD_JUMP)
            {
                aForm.panel_Jump.Visible = true;

                aForm.cBox_JumpLabelName.Text = inData.mParam1;
                aForm.tBox_JumpFlagNo.Text = inData.mParam2;
                aForm.numUpDown_JumpValue.Value = inData.mParam3;
            }
            if (aCommand == Program.SCRIPTCMD_JUMPS88)
            {
                aForm.panel_Jump.Visible = true;

                aForm.cBox_JumpLabelName.Text = inData.mParam1;
                aForm.tBox_JumpFlagNo.Text = (DSCommon.ParseStrToInt(inData.mParam2) + 1).ToString();
                aForm.numUpDown_JumpValue.Value = inData.mParam3;

                //ラベルを書き換える
                aForm.label_FlagNo.Text = "S88 No.(1-256)";
                aForm.numUpDown_JumpValue.Maximum = 1;

            }



            if ((aCommand == Program.SCRIPTCMD_SETFLAG) || (aCommand == Program.SCRIPTCMD_INCFLAG))
            {
                aForm.panel_SetFlag.Visible = true;

                aForm.tBox_SetFlagNo.Text = inData.mParam1;
                aForm.numUpDown_SetFlagValue.Value = DSCommon.ParseStrToInt(inData.mParam2);
            }

            if (aCommand == Program.SCRIPTCMD_LABEL)
            {
                aForm.panel_LineLabel.Visible = true;

                aForm.cBox_LineLabel.Text = inData.mParam1;
            }

            if (aCommand == Program.SCRIPTCMD_RUNFILE)
            {
                aForm.panel_RunFile.Visible = true;

                aForm.cBox_Appname.Items.Clear();

                /* アプリを登録 */
                for (i = 0; i < ExecuteManager.Items.Count; i++)
                {
                    aForm.cBox_Appname.Items.Add(ExecuteManager.Items[i].ItemName);
                }

                aForm.cBox_Appname.Text = inData.mParam1;
                aForm.textBox_TargetFile.Text = inData.mParam2;

            }


            if (aCommand == Program.SCRIPTCMD_JUMPROUTE)
            {
                aForm.panelRoute.Visible = true;


                for (int j = 0; j < Routes.RoutesList.ListItems.Count; j++)
                {
                    aForm.cBox_Route.Items.Add("R" + j.ToString() + ":" + Routes.RoutesList.ListItems[j].NameText);
                }

                aForm.cBox_Route.SelectedIndex = DSCommon.ParseStrToInt(inData.mParam2);
                aForm.cBoxRouteState.SelectedIndex = inData.mParam3;

                //フリー編集画面を開く
                aForm.cBox_Routelabel.Text = inData.mParam1;

            }

            if (aCommand == Program.SCRIPTCMD_SETROUTE)
            {
                aForm.panelRoute.Visible = true;

                for (int j= 0; j < Routes.RoutesList.ListItems.Count; j++)
                {
                    aForm.cBox_Route.Items.Add("R" + j.ToString() + ":" + Routes.RoutesList.ListItems[j].NameText);
                }

                aForm.cBox_Route.SelectedIndex = DSCommon.ParseStrToInt(inData.mParam1);

                //フリー編集画面を開く
                aForm.cBox_Routelabel.Visible = false;
                aForm.labeRouteLabel.Visible = false;
                aForm.cBoxRouteState.Visible = false;
                aForm.label_RuteState.Visible = false;
            }

            if ((aCommand == Program.SCRIPTCMD_JUMPSTOP) || (aCommand == Program.SCRIPTCMD_JUMPRUN))
            {
                aForm.panelJumpRun.Visible = true;

                aForm.cBox_JumpRunLabel.Text = inData.mParam1;
                aForm.numUpDownJumpRun.Value = inData.mParam3;

                if ((inLocAddrNotAutoReplaced == false) && (LocDB.GetAddress(DSCommon.ParseStrToInt(inData.mParam2)) <= 0))
                {
                    aForm.cBox_AddrReplacedLocJump.Checked = true;
                }
                else
                {
                    aForm.cBox_AddrReplacedLocJump.Checked = false;
                }

                aForm.cBox_JumpRunLocAddr.Enabled = !aForm.cBox_AddrReplacedLocJump.Checked;
                aForm.cBox_JumpRunLocProt.Enabled = !aForm.cBox_AddrReplacedLocJump.Checked;

                aForm.cBox_JumpRunLocAddr.Text = LocDB.GetAddress(DSCommon.ParseStrToInt(inData.mParam2)).ToString();
                aForm.cBox_JumpRunLocProt.SelectedIndex = LocDB.GetAddressLocProtcol(DSCommon.ParseStrToInt(inData.mParam2));


            }


            /* フォームを開く */
            if (aForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                /* Script Listに反映 */

                if (aCommand == Program.SCRIPTCMD_SPEED)
                {

                    if (aForm.cBox_LocAddressSpd.Text.Contains("SLOT.") == true)
                    {
                        inData.mParam1 = aForm.cBox_LocAddressSpd.Text;
                    }
                    else
                    {
                        inData.mParam1 = GetLocAddressFromDialog(aForm.cBox_LocAddressSpd.Text, aForm.cBox_ProtcolLocSpd.SelectedIndex, aForm.cBox_AddrReplacedLocSpd.Checked).ToString();
                    }                    
                    
                    inData.mParam2 = decimal.ToInt32(aForm.numericSpeed.Value).ToString();
                    inData.mParam3 = decimal.ToInt32(aForm.numUpDown_TransitionTime.Value);

                }

                if (aCommand == Program.SCRIPTCMD_DIRECTION)
                {
                    if (aForm.cBox_LocAddressDir.Text.Contains("SLOT.") == true)
                    {
                        inData.mParam1 = aForm.cBox_LocAddressDir.Text;
                    }
                    else
                    {
                        inData.mParam1 = GetLocAddressFromDialog(aForm.cBox_LocAddressDir.Text, aForm.cBox_ProtcolLocDir.SelectedIndex, aForm.cBox_AddrReplacedLocDir.Checked).ToString();
                    }

                    switch (aForm.cBox_Direction.SelectedIndex)
                    {
                        case 0: inData.mParam2 = Program.DIRECTION_FWD.ToString(); break;
                        case 1: inData.mParam2 = Program.DIRECTION_REV.ToString(); break;
                        default:
                            inData.mParam2 = Program.DIRECTION_FWD.ToString();
                            break;
                    }
                
                }

                if (aCommand == Program.SCRIPTCMD_FUNCTION)
                {
                    if (aForm.cBox_LocAddressFnc.Text.Contains("SLOT.") == true)
                    {
                        inData.mParam1 = aForm.cBox_LocAddressFnc.Text;
                    }
                    else
                    {
                        inData.mParam1 = GetLocAddressFromDialog(aForm.cBox_LocAddressFnc.Text, aForm.cBox_ProtcolLocFnc.SelectedIndex, aForm.cBox_AddrReplacedLocFnc.Checked).ToString();
                    }
                        
                    inData.mParam2 = aForm.cBox_FunctionNo.SelectedIndex.ToString();
                    inData.mParam3 = aForm.cBox_FunctionPower.SelectedIndex;
                }

                if (aCommand == Program.SCRIPTCMD_ACCESSORY)
                {

                    inData.mParam1 = decimal.ToInt32(aForm.numericAccAddress.Value).ToString();
                    inData.mParam2 = aForm.cBox_AccPower.SelectedIndex.ToString();
                }

                if (aCommand == Program.SCRIPTCMD_POWER)
                {

                    inData.mParam1 = aForm.cBox_Power.SelectedIndex.ToString();
                }

                if ( (aCommand == Program.SCRIPTCMD_WAIT) || (aCommand == Program.SCRIPTCMD_WAITRND))
                {

                    inData.mParam1 = decimal.ToInt32(aForm.numericWaitTime.Value).ToString();
                }

                if (aCommand == Program.SCRIPTCMD_WAITIF)
                {

                    inData.mParam1 = aForm.textBox_FreeCommand.Text;
                }

                if (aCommand == Program.SCRIPTCMD_GOTO)
                {

                    inData.mParam1 = aForm.cBox_LineNoLabel.Text;
                }

                if (aCommand == Program.SCRIPTCMD_GOTOIF)
                {
                    inData.mParam1 = aForm.cBox_JumpLabelName.Text;
                    inData.mParam2 = aForm.tBox_JumpFlagNo.Text;
                }

                if (aCommand == Program.SCRIPTCMD_JUMP)
                {
                    inData.mParam1 = aForm.cBox_JumpLabelName.Text;
                    inData.mParam2 = aForm.tBox_JumpFlagNo.Text;
                    inData.mParam3 = decimal.ToInt32(aForm.numUpDown_JumpValue.Value);
                }


                if (aCommand == Program.SCRIPTCMD_JUMPS88)
                {
                    inData.mParam1 = aForm.cBox_JumpLabelName.Text;
                    inData.mParam2 = (DSCommon.ParseStrToInt(aForm.tBox_JumpFlagNo.Text) - 1).ToString();
                    inData.mParam3 = decimal.ToInt32(aForm.numUpDown_JumpValue.Value);
                }                

                if ((aCommand == Program.SCRIPTCMD_SETFLAG) || (aCommand == Program.SCRIPTCMD_INCFLAG))
                {
                    inData.mParam1 = aForm.tBox_SetFlagNo.Text;
                    inData.mParam2 = aForm.numUpDown_SetFlagValue.Value.ToString();
                }

                if (aCommand == Program.SCRIPTCMD_LABEL)
                {
                    inData.mParam1 = aForm.cBox_LineLabel.Text;
                }

                if (aCommand == Program.SCRIPTCMD_RUNFILE)
                {

                    inData.mParam1 = aForm.cBox_Appname.Text;
                    inData.mParam2 = aForm.textBox_TargetFile.Text;

                }

                if (aCommand == Program.SCRIPTCMD_SETROUTE)
                {
                    inData.mParam1 = aForm.cBox_Route.SelectedIndex.ToString();

                }

                if (aCommand == Program.SCRIPTCMD_JUMPROUTE)
                {
                    inData.mParam2 = aForm.cBox_Route.SelectedIndex.ToString();
                    inData.mParam1 = aForm.cBox_Routelabel.Text;
                    inData.mParam3 = aForm.cBoxRouteState.SelectedIndex;


                }

                if ((aCommand == Program.SCRIPTCMD_JUMPSTOP) || (aCommand == Program.SCRIPTCMD_JUMPRUN))
                {
                    inData.mParam1 = aForm.cBox_JumpRunLabel.Text;
                    inData.mParam2 = GetLocAddressFromDialog(aForm.cBox_JumpRunLocAddr.Text, aForm.cBox_JumpRunLocProt.SelectedIndex, aForm.cBox_AddrReplacedLocJump.Checked).ToString();
                    inData.mParam3 = decimal.ToInt32(aForm.numUpDownJumpRun.Value);
                }

                aResult = true;

            }

            aForm.Dispose();

            return aResult;
        }


        private int GetLocAddressFromDialog(string inLocAddress, int inProtocolIndex, bool inAutoReplaced)
        {
            int aLocNewAddress;

            if ((inLocAddress != "") && (inAutoReplaced == false))
            {
                aLocNewAddress = LocDB.AssignAddressProtcol(inProtocolIndex, DSCommon.ParseStrToInt(inLocAddress));
            }
            else
            {
                aLocNewAddress = 0;
            }

            return aLocNewAddress;

        }

        private void sPEEDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /* 追加 */
            int aIndex;

            ToolStripMenuItem aItem = sender as ToolStripMenuItem;

            ScriptData aData = new ScriptData();

            aData.mParam1 = "";
            aData.mParam2 = "";

            switch (DSCommon.ParseStrToInt((string)aItem.Tag))
            {
                case 1: aData.mCommand = Program.SCRIPTCMD_SPEED;
                    break;
                case 2: aData.mCommand = Program.SCRIPTCMD_DIRECTION;
                    break;
                case 3: aData.mCommand = Program.SCRIPTCMD_FUNCTION;
                    break;
                case 4: aData.mCommand = Program.SCRIPTCMD_ACCESSORY;
                    break;
                case 5: aData.mCommand = Program.SCRIPTCMD_WAIT;
                    break;
                case 6: aData.mCommand = Program.SCRIPTCMD_POWER;
                    break;
                case 7: aData.mCommand = Program.SCRIPTCMD_EXIT;
                    break;
                case 8: aData.mCommand = Program.SCRIPTCMD_GOTO;
                    break;
            }

            aIndex = 0;

            if (listScript.SelectedItems.Count > 0)
            {

                aIndex = listScript.SelectedItems[0].Index;
                ScriptList.Insert(aIndex + 1, aData);
            }
            else
            {
                ScriptList.Add(aData);
            }

            UpdateScriptDisplay();


            if (OpenScriptEdit(ScriptList[aIndex], true) == true)
            {

                /* スクリプト登録 */
                UpdateScriptDisplay();

            }
        }

        private void editLeverOptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int aAccGears;

            LeverEditForm aForm = new LeverEditForm();

            aForm.SetFormLanguage(LangManager);

            aAccGears = gAppSettings.mSpeedGears;

            /* 最大最小チェック */
            if (aAccGears <= decimal.ToInt32(aForm.numUpDown_AccGears.Minimum))
            {
                aAccGears = decimal.ToInt32(aForm.numUpDown_AccGears.Minimum);
            }
            if (aAccGears >= decimal.ToInt32(aForm.numUpDown_AccGears.Maximum))
            {
                aAccGears = decimal.ToInt32(aForm.numUpDown_AccGears.Maximum);
            }

            aForm.cBox_LeverMode.SelectedIndex = gAppSettings.mSpeedLeverMode;
            aForm.numUpDown_AccGears.Value = aAccGears;

            if (aForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                gAppSettings.mSpeedLeverMode = aForm.cBox_LeverMode.SelectedIndex;
                gAppSettings.mSpeedGears = decimal.ToInt32(aForm.numUpDown_AccGears.Value);

                /* 位置を初期化 */
                LeverValue = 0;

                /* 画面再描画 */
                LeverBox.Refresh();
                MeterBox.Refresh();
            }

            aForm.Dispose();
        }

        private void button_ConsoleLogClear_Click(object sender, EventArgs e)
        {
            listBox_Serial.Items.Clear();
        }

        private void serialPort_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            string aRecvText = "";
            string aSerialText = "";
            int i;

            System.IO.Ports.SerialPort sp = (System.IO.Ports.SerialPort)sender;
            try
            {
                SerialBuftext = SerialBuftext + sp.ReadExisting();
            }
            catch (InvalidOperationException ex)
            {
                aRecvText = "[ERR]" + ex.Message;
                /* エラーカウンター */
                Counter_ComErr = Counter_ComErr + 1;

            }
            catch (TimeoutException ex)
            {
                aRecvText = "[ERR]" + ex.Message;

                /* エラーカウンター */
                Counter_ComErr = Counter_ComErr + 1;
            }
            catch (Exception ex)
            {
                aRecvText = "[ERR]" + ex.Message;

                /* エラーカウンター */
                Counter_ComErr = Counter_ComErr + 1;
            }

            SetRecvText_delegate aFunc = new SetRecvText_delegate(SetRecvText);

            if (aRecvText.Length > 0)
            {
                listBox_Serial.BeginInvoke(aFunc, aRecvText);
            }

            /* データ分解 */

            i = 0;

            while(true)
            {


                if (SerialBuftext == "")
                {
                    //何もしない
                    break;
                }
                else if (SerialBuftext[i] == '\n')
                {
                    aSerialText = SerialBuftext.Substring(0, i);
                    SerialBuftext = SerialBuftext.Substring(i + 1, SerialBuftext.Length - i - 1);
                    i = 0;

                    if (aSerialText.Length > 0)
                    {

                        /* 末尾のCRを削除する */
                        if (aSerialText[aSerialText.Length - 1] == '\r')
                        {
                            aSerialText = aSerialText.Substring(0, aSerialText.Length - 1);
                        }

                        aRecvText = "[RECV]" + aSerialText;

                        listBox_Serial.BeginInvoke(aFunc, aRecvText);

                        SetRecvConnectCheck(aSerialText);

                        /* 外部コマンドステーションの応答を取得 */
                        this.BeginInvoke(new SetRecvAnotherStation_delegate(SetRecvAnotherStation), new object[] { aSerialText });

                        /* S88受信データを保存 */
                        SetRecvS88Datas(aSerialText);
                    }
                }
                else
                {
                    i = i + 1;
                }

                if (i >= SerialBuftext.Length)
                {
                    break;
                }
            }
        }

        private void editFunctionButtonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int aPower;
            int aTag;
            int aFunctionData;
            int aImageIndex;
            int i;

            /* ボタンの編集画面を出す */
            ToolStripDropDownItem aStripItem = sender as ToolStripDropDownItem;
            ContextMenuStrip aMenu = aStripItem.Owner as ContextMenuStrip;

            aTag = Convert.ToInt32(aMenu.SourceControl.Tag);

            if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
            {
                aTag = aTag + 16;
            }


            if (aTag < Program.MAX_FUNCTIONNUM)
            {
                ButtonEditForm aForm = new ButtonEditForm();
                aFunctionData = LocDB.Items[SelectedLocIndex].mFunctionImageTable[aTag];
                aPower = aFunctionData & 0x01;
                aImageIndex = aFunctionData >> 1;

                /* 画像インデックスの範囲チェック */
                if (aImageIndex >= FunctionImageList.Images.Count)
                {
                    aImageIndex = FunctionImageList.Images.Count - 1;
                }

                /* ON状態かOFF状態か */
                if (aPower == 0)
                {
                    aForm.cBox_FunctionSwitch.Checked = false;
                }
                else
                {
                    aForm.cBox_FunctionSwitch.Checked = true;
                }

                /* RunFile関連(ファンクションエミュレート) */

                //外部アプリ名の一覧を列挙
                for( i = 0; i < ExecuteManager.Items.Count; i++)
                {
                    aForm.cBox_AppName.Items.Add(ExecuteManager.Items[i].ItemName);
                }

                aForm.cBox_AppName.Text = LocDB.Items[SelectedLocIndex].mExFunctionCommand[aTag];
                aForm.cBox_Assignment.SelectedIndex = LocDB.Items[SelectedLocIndex].mFunctionExMethod[aTag];
                aForm.cBox_FileName.Text = LocDB.Items[SelectedLocIndex].mExFunctionData[aTag];
                aForm.SetStateAssignRunFile(aForm.cBox_Assignment.SelectedIndex);
                aForm.tBox_Addr.Text = LocDB.GetAddress(LocDB.Items[SelectedLocIndex].mFunctionExAddress[aTag]).ToString();
                aForm.cBox_ProtcolLoc.SelectedIndex = LocDB.GetAddressLocProtcol(LocDB.Items[SelectedLocIndex].mFunctionExAddress[aTag]);
                aForm.cBox_FunctionNo.SelectedIndex = LocDB.Items[SelectedLocIndex].mFunctionExFuncNo[aTag];

                /* イメージリストを登録する */
                aForm.FunctionImageList.Images.Clear();
                aForm.cBox_FunctionImage.Items.Clear();

                for (i = 0; i < FunctionImageList.Images.Count; i++)
                {
                    aForm.FunctionImageList.Images.Add(FunctionImageList.Images[i]);
                    aForm.cBox_FunctionImage.Items.Add(i.ToString());
                }

                aForm.cBox_FunctionImage.SelectedIndex = aImageIndex;

                /* 言語設定 */
                aForm.SetFormLanguage(LangManager);

                aForm.Text = aForm.Text + " (F" + (aTag).ToString() + ")";


                if (aForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    /* 設定を反映するとき */

                    if (aForm.cBox_FunctionSwitch.Checked == true)
                    {
                        aPower = 1;
                    }
                    else
                    {
                        aPower = 0;
                    }

                    aImageIndex = aForm.cBox_FunctionImage.SelectedIndex;

                    aFunctionData = aPower + (aImageIndex << 1);

                    LocDB.Items[SelectedLocIndex].mFunctionImageTable[aTag] = aFunctionData;

                    LocDB.Items[SelectedLocIndex].mExFunctionCommand[aTag] = aForm.cBox_AppName.Text;
                    LocDB.Items[SelectedLocIndex].mFunctionExMethod[aTag] = aForm.cBox_Assignment.SelectedIndex;
                    LocDB.Items[SelectedLocIndex].mExFunctionData[aTag] = aForm.cBox_FileName.Text;
                    LocDB.Items[SelectedLocIndex].mFunctionExAddress[aTag] = LocDB.AssignAddressProtcol(aForm.cBox_ProtcolLoc.SelectedIndex, DSCommon.ParseStrToInt(aForm.tBox_Addr.Text));
                    LocDB.Items[SelectedLocIndex].mFunctionExFuncNo[aTag] = aForm.cBox_FunctionNo.SelectedIndex;



                    UpdateFunctionButtons(SelectedLocIndex, false);
                }

                aForm.Dispose();

            }

        }

        private void SButton_Loc_Click(object sender, EventArgs e)
        {

            ToolStripButton aButton = sender as ToolStripButton;
            int aTag;

            aTag = int.Parse(aButton.Tag.ToString());

            /* 複数画面表示以外は位置を修正する */
            if (aTag != 10)
            {
                UpdatePanelAllPosition();
            }

            switch (aTag)
            {
                case 1:
                    //メイン画面
                    panel_AccList.Visible = false;
                    panel_Layout.Visible = false;
                    panel_Loc.Visible = true;
                    panel6021.Visible = false;
                    panel_Sequence.Visible = false;
                    panel_SerialConsole.Visible = false;
                    panel_CVEditor.Visible = false;
                    panel_MultiLocs.Visible = false;
                    panel_S88.Visible = false;
                    panel_Crane.Visible = false;

                    //フォームにフォーカスを移す(Wheel用)
                    this.Focus();

                    /* 画面更新 */
                    UpdatePanelLocomotiveControl();
                    break;

                case 2:
                    //アクセサリを表示
                    panel_AccList.Visible = true;
                    panel_Layout.Visible = false;
                    panel_Loc.Visible = false;
                    panel6021.Visible = false;
                    panel_Sequence.Visible = false;
                    panel_SerialConsole.Visible = false;
                    panel_CVEditor.Visible = false;
                    panel_MultiLocs.Visible = false;
                    panel_S88.Visible = false;
                    panel_Crane.Visible = false;

                    break;

                case 3:
                    //6021を表示
                    panel_AccList.Visible = false;
                    panel_Layout.Visible = false;
                    panel_Loc.Visible = false;
                    panel6021.Visible = true;
                    panel_Sequence.Visible = false;
                    panel_SerialConsole.Visible = false;
                    panel_CVEditor.Visible = false;
                    panel_MultiLocs.Visible = false;
                    panel_S88.Visible = false;
                    panel_Crane.Visible = false;

                    break;

                case 4:
                    //レイアウトを表示
                    panel_AccList.Visible = false;
                    panel_Layout.Visible = true;
                    panel_Loc.Visible = false;
                    panel6021.Visible = false;
                    panel_Sequence.Visible = false;
                    panel_SerialConsole.Visible = false;
                    panel_CVEditor.Visible = false;
                    panel_MultiLocs.Visible = false;
                    panel_S88.Visible = false;
                    panel_Crane.Visible = false;

                    break;

                case 5:
                    //シーケンスを表示
                    panel_AccList.Visible = false;
                    panel_Layout.Visible = false;
                    panel_Loc.Visible = false;
                    panel6021.Visible = false;
                    panel_Sequence.Visible = true;
                    panel_SerialConsole.Visible = false;
                    panel_CVEditor.Visible = false;
                    panel_MultiLocs.Visible = false;
                    panel_S88.Visible = false;
                    panel_Crane.Visible = false;

                    break;

                case 6:
                    //コンソールを表示
                    panel_AccList.Visible = false;
                    panel_Layout.Visible = false;
                    panel_Loc.Visible = false;
                    panel6021.Visible = false;
                    panel_Sequence.Visible = false;
                    panel_SerialConsole.Visible = true;
                    panel_CVEditor.Visible = false;
                    panel_MultiLocs.Visible = false;
                    panel_S88.Visible = false;
                    panel_Crane.Visible = false;

                    break;

                case 7:
                    //CVエディタを表示
                    panel_AccList.Visible = false;
                    panel_Layout.Visible = false;
                    panel_Loc.Visible = false;
                    panel6021.Visible = false;
                    panel_Sequence.Visible = false;
                    panel_SerialConsole.Visible = false;
                    panel_CVEditor.Visible = true;
                    panel_MultiLocs.Visible = false;
                    panel_S88.Visible = false;
                    panel_Crane.Visible = false;

                    break;

                case 8:
                    //複数台同時制御画面を表示
                    panel_AccList.Visible = false;
                    panel_Layout.Visible = false;
                    panel_Loc.Visible = false;
                    panel6021.Visible = false;
                    panel_Sequence.Visible = false;
                    panel_SerialConsole.Visible = false;
                    panel_CVEditor.Visible = false;
                    panel_MultiLocs.Visible = true;
                    panel_S88.Visible = false;
                    panel_Crane.Visible = false;


                    UpdatePanelMultiLocomotivePanel();

                    break;

                case 9:
                    /* S88センサー */
                    panel_AccList.Visible = false;
                    panel_Layout.Visible = false;
                    panel_Loc.Visible = false;
                    panel6021.Visible = false;
                    panel_Sequence.Visible = false;
                    panel_SerialConsole.Visible = false;
                    panel_CVEditor.Visible = false;
                    panel_MultiLocs.Visible = false;
                    panel_S88.Visible = true;
                    panel_Crane.Visible = false;


                    /* 機関車選択ボックス用 */





                    if (cBox_LocChangeEV.Items.Count != (cBox_LocChange.Items.Count + 1))
                    {

                        int aSelectedIndex = cBox_LocChangeEV.SelectedIndex;
                        cBox_LocChangeEV.Items.Clear();
                        cBox_LocChangeEV.Items.Add(LangManager.SetText("TxtNotSelected", "Not selected"));

                        for (int i = 0; i < cBox_LocChange.Items.Count; i++)
                        {
                            cBox_LocChangeEV.Items.Add(cBox_LocChange.Items[i]);
                        }

                        /* インデックスを復帰 */
                        if (cBox_LocChangeEV.Items.Count > aSelectedIndex)
                        {
                            cBox_LocChangeEV.SelectedIndex = aSelectedIndex;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < cBox_LocChange.Items.Count; i++)
                        {
                            if (cBox_LocChangeEV.Items[i + 1] != cBox_LocChange.Items[i])
                            {
                                cBox_LocChangeEV.Items[i + 1] = cBox_LocChange.Items[i];
                            }
                        }
                    }

                    //イベント未選択
                    if (lBox_S88Events.SelectedItem == null)
                    {
                        lBox_S88Events.SelectedIndex = 0;
                    }

                    break;

                case 10:
                    /* 複数画面表示 */
                    panel_AccList.Visible = false;
                    panel_Layout.Visible = false;
                    panel_Loc.Visible = false;
                    panel6021.Visible = false;
                    panel_Sequence.Visible = false;
                    panel_SerialConsole.Visible = false;
                    panel_CVEditor.Visible = false;
                    panel_MultiLocs.Visible = false;
                    panel_S88.Visible = false;
                    panel_Crane.Visible = false;


                    /* 表示と位置設定・割り当て */
                    UpdateMultiPanels();


                    break;

                case 11:
                    /* クレーン */
                    panel_AccList.Visible = false;
                    panel_Layout.Visible = false;
                    panel_Loc.Visible = false;
                    panel6021.Visible = false;
                    panel_Sequence.Visible = false;
                    panel_SerialConsole.Visible = false;
                    panel_CVEditor.Visible = false;
                    panel_MultiLocs.Visible = false;
                    panel_S88.Visible = false;
                    panel_Crane.Visible = true;
                    break;

            }

        }

        private void UpdateFormSize(int inClientWidth, int inClientHeight)
        {
            int aWidth;
            int aHeight;

            aWidth = inClientWidth;
            aHeight = inClientHeight + statusStrip1.Height + toolStrip.Height;
            this.ClientSize = new System.Drawing.Size(aWidth, aHeight);


        }

        private void UpdateMultiPanels()
        {

            switch (gAppSettings.mScreenNums)
            {
                case 1:
                    /* ２画面横 */
                    UpdateFormSize(Program.SCREEN_SIZE2_WIDTH, Program.SCREEN_SIZE1_HEIGHT);

                    /* パネル */
                    UpdatePanelOnMultiPanels(gAppSettings.mScreen1Panel, 0, 0);
                    UpdatePanelOnMultiPanels(gAppSettings.mScreen2Panel, 925, 0);

                    break;
                case 2:
                    /* ２画面縦 */
                    UpdateFormSize(Program.SCREEN_SIZE1_WIDTH, Program.SCREEN_SIZE2_HEIGHT);

                    /* パネル */
                    UpdatePanelOnMultiPanels(gAppSettings.mScreen1Panel, 0, 0);
                    UpdatePanelOnMultiPanels(gAppSettings.mScreen3Panel, 0, 470);

                    break;
                case 3:
                    /* ４画面 */
                    UpdateFormSize(Program.SCREEN_SIZE2_WIDTH, Program.SCREEN_SIZE2_HEIGHT);

                    /* パネル */
                    UpdatePanelOnMultiPanels(gAppSettings.mScreen1Panel, 0, 0);
                    UpdatePanelOnMultiPanels(gAppSettings.mScreen2Panel, 925, 0);
                    UpdatePanelOnMultiPanels(gAppSettings.mScreen3Panel, 0, 470);
                    UpdatePanelOnMultiPanels(gAppSettings.mScreen4Panel, 925, 470);
                    
                    break;
                default:
                    return;
            }

        }

        private void UpdatePanelOnMultiPanels(int inNum, int inX, int inY)
        {

            switch( inNum)
            {
                case 1:
                    panel_Loc.Top = inY;
                    panel_Loc.Left = inX;
                    panel_Loc.BorderStyle = BorderStyle.FixedSingle;
                    panel_Loc.Visible = true;
                    break;
                case 2:
                    panel_MultiLocs.Top = inY;
                    panel_MultiLocs.Left = inX;
                    panel_MultiLocs.BorderStyle = BorderStyle.FixedSingle;
                    panel_MultiLocs.Visible = true;
                    break;
                case 3:
                    panel6021.Top = inY;
                    panel6021.Left = inX;
                    panel6021.BorderStyle = BorderStyle.FixedSingle;
                    panel6021.Visible = true;
                    break;
                case 4:
                    panel_AccList.Top = inY;
                    panel_AccList.Left = inX;
                    panel_AccList.BorderStyle = BorderStyle.FixedSingle;
                    panel_AccList.Visible = true;
                    break;
                case 5:
                    panel_Layout.Top = inY;
                    panel_Layout.Left = inX;
                    panel_Layout.BorderStyle = BorderStyle.FixedSingle;
                    panel_Layout.Visible = true;
                    break;
                case 6:
                    panel_Sequence.Top = inY;
                    panel_Sequence.Left = inX;
                    panel_Sequence.BorderStyle = BorderStyle.FixedSingle;
                    panel_Sequence.Visible = true;
                    break;
                case 7:
                    panel_S88.Top = inY;
                    panel_S88.Left = inX;
                    panel_S88.BorderStyle = BorderStyle.FixedSingle;
                    panel_S88.Visible = true;
                    break;
                case 8:
                    panel_SerialConsole.Top = inY;
                    panel_SerialConsole.Left = inX;
                    panel_SerialConsole.BorderStyle = BorderStyle.FixedSingle;
                    panel_SerialConsole.Visible = true;
                    break;
            }


        }

        private void UpdatePanelAllPosition()
        {
            //スケールが100%のままのとき
            if (ScaleRation == 100)
            {
                //複数表示画面になっているとき
                if ((this.ClientSize.Width > Program.SCREEN_SIZE1_WIDTH) || (this.ClientSize.Height > Program.SCREEN_SIZE1_HEIGHT))
                {
                    UpdateFormSize(Program.SCREEN_SIZE1_WIDTH, Program.SCREEN_SIZE1_HEIGHT);
                }
            }

            panel_AccList.Top = 0;
            panel_AccList.Left = 0;
            panel_AccList.BorderStyle = BorderStyle.None;

            panel_Layout.Top = 0;
            panel_Layout.Left = 0;
            panel_Layout.BorderStyle = BorderStyle.None;

            panel_Loc.Top = 0;
            panel_Loc.Left = 0;
            panel_Loc.BorderStyle = BorderStyle.None;

            panel6021.Top = 0;
            panel6021.Left = 0;
            panel6021.BorderStyle = BorderStyle.None;

            panel_Sequence.Top = 0;
            panel_Sequence.Left = 0;
            panel_Sequence.BorderStyle = BorderStyle.None;

            panel_SerialConsole.Top = 0;
            panel_SerialConsole.Left = 0;
            panel_SerialConsole.BorderStyle = BorderStyle.None;

            panel_CVEditor.Top = 0;
            panel_CVEditor.Left = 0;
            panel_CVEditor.BorderStyle = BorderStyle.None;

            panel_MultiLocs.Top = 0;
            panel_MultiLocs.Left = 0;
            panel_MultiLocs.BorderStyle = BorderStyle.None;

            panel_S88.Top = 0;
            panel_S88.Left = 0;
            panel_S88.BorderStyle = BorderStyle.None;



        }

        private void UpdatePanelMultiLocomotivePanel()
        {
            int aSelectedNo;
            int aIndex;
            int i;

            //画面状態の更新処理
            for (i = 0; i < Program.MULTICONTROL_MAX; i++)
            {
                UpdateLocMultiCtrl(i);
            }

            aSelectedNo = CurrentSelectedMultiLocIndex;

            /* 該当なしのときの処理 */
            if (aSelectedNo < 0)
            {
                return;
            }

            /* 機関車データベースの番号を取得 */
            aIndex = gAppSettings.mLocCtrlList[aSelectedNo] - 1;

            /* 選択機関車のボタンを更新 */
            UpdateFunctionButtons_MultiLocos(aIndex, false);
        }

        private void ResetSpeedOnLocPanel(int inIndex)
        {

            int aLowAddress;
            int aHighAddress;

            /* 重連のアドレスを取得する */
            aLowAddress = LocDB.Items[inIndex].mLocAddr;
            aHighAddress = LocDB.Items[inIndex].mLocAddr_dbl;

            /* リストに反映 */
            LocDB.Items[inIndex].mCurrentSpeed = 0;

            /* 速度セット */
            if (LocDB.GetAddress(aLowAddress) > 0)
            {
                SerialCmd.SetLocoSpeed(aLowAddress, 0, LocDB.Items[inIndex].mLocSpeedstep);
            }

            if (LocDB.GetAddress(aHighAddress) > 0)
            {
                SerialCmd.SetLocoSpeed(aHighAddress, 0, LocDB.Items[inIndex].mLocSpeedstep);
            }


        }

        private void button_SpeedReset_Click(object sender, EventArgs e)
        {
            /* clicked Emergency button */
            ResetSpeedOnLocPanel(SelectedLocIndex);

            UpdatePowerLever(0);

            /* メーター更新 */
            MeterBox.Refresh();

            /* レバー更新 */
            LeverBox.Refresh();

        }
        private void timerSpeed_Tick(object sender, EventArgs e)
        {

            DateTime aTime;

            if (gControlMode == Program.POWER_OFF)
            {
                return;
            }

            /* 次の速度をセット */
            for (int i = 0; i < LocDB.Items.Count; i++)
            {
                if (LocDB.Items[i].IsUpdateNextInterval() == true)
                {
                    SetNextSpeed(i);
                    LocDB.Items[i].ResetUpdateNextInterval();
                }
            }

            /* 6021の指令のセット */
            if (NextAutoSpeedSet6021 == true)
            {
                /* 速度セット */
                SerialCmd.SetLocoSpeed(NextAutoSpeedAddr6021, Dial6021Value, 0);

                NextAutoSpeedAddr6021 = 0;
                NextAutoSpeedSet6021 = false;
            }

            /* Ping送信周期 */
            if (gAppSettings.mPingSend == false)
            {
                if (Counter_Ping >= Program.COUNTTIMER_PING)
                {
                    SerialCmd.SetPing();
                    Counter_Ping = 0;
                }
                else
                {
                    if (SendingList.Count == 0)
                    {
                        /* 何も送信しない状況があるとき */
                        Counter_Ping = Counter_Ping + 1;
                    }
                    else
                    {
                        /* 何か信号は送っているのでPINGカウンタはリセットする */
                        Counter_Ping = 0;
                    }
                }
            }

            /* 時間の決定 */
            if (gAppSettings.mUseVirtualClock == true)
            {
                aTime = gAppSettings.mVirtualClock + (DateTime.Now - timeBoot);

            }
            else
            {
                aTime = DateTime.Now;
            }

            // イベントスクリプト状態変化チェック・実行許可(時間実行、在線チェック)
            if (S88Manager.IntervalCheckTime(S88Flags, aTime, LocDB, Routes.RoutesList) == true)
            {
                flagUpdateS88SensorView = true;
            }

        }

        private void UpdateMeterBoxCache()
        {
            int cX, cY;
            int aR;
            int aSpeedDisplay;

            /* 最高速度値の基準 */
            if (gControlMode == Program.POWER_OFF)
            {
                aSpeedDisplay = 300;
            }
            else
            {
                aSpeedDisplay = LocDB.Items[SelectedLocIndex].mDisplayMaxSpeed;
            }

            /* 画像の原点・半径を規定 */
            cX = (200 * ScaleRation) / 100;
            cY = cX;
            aR = cX - 10;

            Graphics aBackGroundBmp = Graphics.FromImage(MeterBoxChacheBitmap);

            //(アンチエイリアス処理されたレタリング)を指定する
            aBackGroundBmp.SmoothingMode = SmoothingMode.AntiAlias;
            aBackGroundBmp.TextRenderingHint = TextRenderingHint.AntiAlias;

            /* メータ背景描画処理 */
            MeterDrawer.DrawMeterBox_Background(aBackGroundBmp, cX, cY, aR, aSpeedDisplay, this.BackColor);


            /* 機関車アイコンの背景描画 */
            DrawIconRoundRectangle(aBackGroundBmp, ((400 - LocIconBitmap.Width) / 2 - 1) * ScaleRation / 100, (310 - 1) * ScaleRation / 100, (160 + 2) * ScaleRation / 100, (48 + 2) * ScaleRation / 100, 2, false, Color.LightGray, Color.Gray, Color.DimGray);

            /* 機関車アイコン */
            aBackGroundBmp.DrawImage(LocIconBitmap, ((400 - LocIconBitmap.Width) / 2) * ScaleRation / 100, 310 * ScaleRation / 100, LocIconBitmap.Width * ScaleRation / 100, LocIconBitmap.Height * ScaleRation / 100);

            /* 文字系 */
            System.Drawing.Font aDrawFont = new System.Drawing.Font("Arial", 18);
            StringFormat aDrawFormat = new StringFormat();

            aDrawFormat.Alignment = StringAlignment.Center;
            aBackGroundBmp.DrawString("km/h", aDrawFont, Brushes.White, 200 * ScaleRation / 100, 270 * ScaleRation / 100, aDrawFormat);

            aBackGroundBmp.Dispose();
            aDrawFormat.Dispose();
            aDrawFont.Dispose();

        }

        private void MeterBox_Paint(object sender, PaintEventArgs e)
        {
            string aTextNotch;
            int aCurrentSpeed;
            int aLocMaxSpeed;
            Point[] pt = new Point[3];
            int cX, cY;
            int aR;
            int aSpeedDisplay;

            /* 最高速度値の基準 */
            if (gControlMode == Program.POWER_OFF)
            {
                aLocMaxSpeed = Program.SPEED_MAX;
                aCurrentSpeed = 0;
                aSpeedDisplay = 300;
            }
            else
            {
                aLocMaxSpeed = LocDB.Items[SelectedLocIndex].mLocMaxSpeed;
                aCurrentSpeed = LocDB.Items[SelectedLocIndex].CurrentSpeed();
                aSpeedDisplay = LocDB.Items[SelectedLocIndex].mDisplayMaxSpeed;
            }

            /* 画像の原点・半径を規定 */
            cX = (200 * ScaleRation) / 100;
            cY = cX;
            aR = cX - 10;

            /* 描画 */
            Graphics aCanvas = e.Graphics;

            //(アンチエイリアス処理されたレタリング)を指定する
            aCanvas.SmoothingMode = SmoothingMode.AntiAlias;
            aCanvas.TextRenderingHint = TextRenderingHint.AntiAlias;

            //キャッシュから復元
            aCanvas.DrawImage(MeterBoxChacheBitmap, 0, 0);

            //針の描画
            MeterDrawer.DrawMeterBox_Pin(aCanvas, cX, cY, aR, aLocMaxSpeed, aCurrentSpeed);

            /* 文字系 */
            System.Drawing.Font aDrawFont = new System.Drawing.Font("Arial", 20 * ScaleRation / 100);
            StringFormat aDrawFormat = new StringFormat();
            aDrawFormat.Alignment = StringAlignment.Center;

            /* 速度表示 */
            aTextNotch = (aCurrentSpeed * aSpeedDisplay / aLocMaxSpeed).ToString() + "";
            aCanvas.DrawString(aTextNotch, aDrawFont, Brushes.White, 200 * ScaleRation / 100, 240 * ScaleRation / 100, aDrawFormat);


            /* 進行方向描画 */
            if (gControlMode == Program.POWER_ON)
            {
                switch (LocDB.Items[SelectedLocIndex].mCurrentDirection)
                {
                    case 0:
                    case Program.DIRECTION_FWD:
                        pt[0] = new Point(290 * ScaleRation / 100, 324 * ScaleRation / 100);
                        pt[1] = new Point(305 * ScaleRation / 100, 334 * ScaleRation / 100);
                        pt[2] = new Point(290 * ScaleRation / 100, 344 * ScaleRation / 100);
                        break;
                    case Program.DIRECTION_REV:
                        pt[0] = new Point(110 * ScaleRation / 100, 324 * ScaleRation / 100);
                        pt[1] = new Point(95 * ScaleRation / 100, 334 * ScaleRation / 100);
                        pt[2] = new Point(110 * ScaleRation / 100, 344 * ScaleRation / 100);
                        break;
                }

                aCanvas.FillPolygon(Brushes.White, pt);
                
            }

            aDrawFont.Dispose();
            aDrawFormat.Dispose();
            
        }

        private void DrawIconRoundRectangle(Graphics g, float x, float y, float w, float h, float r, bool inFrame, Color inBrush1, Color inBrush2, Color inFrameColor)
        {
            float a = (float)(4 * (1.41421356 - 1) / 3 * r);
            RectangleF aGaugeRect = new RectangleF(x, y, w, h);

            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            path.AddBezier(x, y + r, x, y + r - a, x + r - a, y, x + r, y); // 左上
            path.AddBezier(x + w - r, y, x + w - r + a, y, x + w, y + r - a, x + w, y + r); // 右上
            path.AddBezier(x + w, y + h - r, x + w, y + h - r + a, x + w - r + a, y + h, x + w - r, y + h); // 右下
            path.AddBezier(x + r, y + h, x + r - a, y + h, x, y + h - r + a, x, y + h - r); // 左下
            path.CloseFigure();


            LinearGradientBrush aBrush = new LinearGradientBrush(aGaugeRect, inBrush1, inBrush2, LinearGradientMode.ForwardDiagonal);
            g.FillPath(aBrush, path);

            if (inFrame == true)
            {
                Pen aPen = new Pen(inFrameColor, 1);
                g.DrawPath(aPen, path);
                aPen.Dispose();
            }


            aBrush.Dispose();
            path.Dispose();
        }

        private void button_FWD_Click(object sender, EventArgs e)
        {
            Button aButton = sender as Button;
            int aTag;

            /* ボタンに割り振られた進行方向種別を取得 */
            aTag = int.Parse(aButton.Tag.ToString());

            /* 進行方向をセット */
            SetLocDirection(SelectedLocIndex, aTag);

            /* メータ再描画 */
            MeterBox.Refresh();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            string aConfigFilePath = Application.StartupPath + "\\Resources\\config";

            /* 設定ファイルフォルダの存在チェック */
            if (Directory.Exists(aConfigFilePath) == false)
            {
                Directory.CreateDirectory(aConfigFilePath);
            }

            /* 存在しない場合は生成 */
            if (gAppSettings == null)
            {
                gAppSettings = new AppSetting();
            }

            /* 設定を格納 */
            gAppSettings.mBaudrate = serialPort.BaudRate;
            gAppSettings.mPortName = serialPort.PortName;
            gAppSettings.mDtrEnable = serialPort.DtrEnable;

            /* CV関連 */
            gAppSettings.mSelectDCCDecoder = cBox_DecoderManufacture.SelectedIndex;


            /* S88イベント設定を保存 */
            S88Manager.SaveToFile(aConfigFilePath + Program.FILE_S88EVENT);

            /* XML保存 */
            gAppSettings.SaveToFile(aConfigFilePath + "\\settings.xml");
            LocDB.SaveToFile_New(aConfigFilePath + "\\LocDB.xml");
            LayoutMapData.SaveToFile(aConfigFilePath + Program.FILE_LAYOUT);
            ExecuteManager.SaveToFile(aConfigFilePath + Program.FILE_EXECUTE);
            Routes.SaveToFile(aConfigFilePath + "\\Routes.xml");

            try
            {

                if (IsOpenSendManager())
                {
                    if (gControlMode == Program.POWER_ON)
                    {
                        if (ScriptMode != Program.SCRIPTMODE_STOP)
                        {
                            ExitScript();
                        }

                        gControlMode = SerialCmd.SetPower(Program.POWER_OFF);
                        gControlMode = SerialCmd.SetPower(Program.POWER_OFF);

                        DSCommon.WaitSleepTime(1);
                    }
                }

                AppServer.Stop();

                serialPort.Close();
            }
            catch (Exception)
            {
                return;
            }
        }

        private void button_F16_Click(object sender, EventArgs e)
        {
            CheckBox aButton = (CheckBox)sender;
            int aTag;

            aTag = int.Parse(aButton.Tag.ToString());

            RunFunctionButton(aTag, SelectedLocIndex, aButton);

            MeterBox.Focus();
        }

        private bool IsNumeric(string inNumText)
        {
            int j;

            bool aResult = int.TryParse(inNumText, out j);

            return aResult;
        }

        private void SButton_CfgSerial_Click(object sender, EventArgs e)
        {

            bool aVisibleOption = gControlMode == Program.POWER_OFF ? true : false;

            string[] aPorts = System.IO.Ports.SerialPort.GetPortNames();
            int j;

            /* Microsoftによるポート名の後に不正文字がつくバグ対策 */
            for (j = 0; j < aPorts.Length; j++)
            {
                if (IsNumeric(aPorts[j].Substring(aPorts[j].Length - 1, 1)) == false)
                {
                    aPorts[j] = aPorts[j].Substring(0, aPorts[j].Length - 1);
                }
            }

            /* 設定フォームを開く */
            SerialConfigForm aForm = new SerialConfigForm();

            /* 言語切り替え */
            aForm.SetFormLanguage(LangManager);

            /* 表示オプション(変更不可項目指定) */
            aForm.NonVisibleOption = aVisibleOption;
            //aForm.gBox_Http.Enabled = aVisibleOption;
            //aForm.gBox_Serial.Enabled = aVisibleOption;
            aForm.cBox_ConnectionType.Enabled = aVisibleOption;

            /* 設定項目反映 */
            aForm.cBox_SerialPort.Items.Clear();
            aForm.cBox_SerialPort.Items.AddRange(aPorts);
            aForm.cBox_SerialPort.Text = serialPort.PortName;
            aForm.cBox_DtrEnable.Checked = serialPort.DtrEnable;
            aForm.cBox_Baudrate.Text = serialPort.BaudRate.ToString();
            aForm.cBox_ConnectionType.SelectedIndex = gAppSettings.mSendMode;
            aForm.cBox_IPAddress.Text = gAppSettings.mIPAddress;


            aForm.cBox_DCC.Checked = gAppSettings.mDCCMode;
            aForm.cBox_MfxAutoRegister.Checked = gAppSettings.mMfxAutoRegister;
            aForm.cBox_MfxAutoUpdate.Checked = gAppSettings.mMfxAutoUpdate;
            aForm.cBox_S88.Checked = gAppSettings.mS88Sensor;
            aForm.numUpDown_S88DetectFreq.Value = DSCommon.MaxMin(2, 10, gAppSettings.mS88SendInterval);
            aForm.numUpDown_S88SensorNums.Value = DSCommon.MaxMin(1, 32, gAppSettings.mS88NumOfConnection);
            aForm.cBox_StopAllLocPon.Checked = gAppSettings.mStopAllLocWhenPowerOn;
            aForm.cBox_AutoCloseSerial.Checked = gAppSettings.mAutoCloseSerialport;
            aForm.cBox_ClearAcc.Checked = gAppSettings.mClearAccessories;

            aForm.cBox_SideFuncRight.SelectedIndex = gAppSettings.mSideFuncRight;
            aForm.cBox_SideFuncBottom.SelectedIndex = gAppSettings.mSideFuncBottom;
            aForm.numUpDown_WindowZoom.Value = DSCommon.MaxMin(100, 350, gAppSettings.mWindowZoom);


            /* 複数スクリーン */
            aForm.cBox_ScreenNums.Enabled = aVisibleOption;
            aForm.cBox_ScreenNums.SelectedIndex = gAppSettings.mScreenNums;
            aForm.cBox_Screen1Panel.SelectedIndex = gAppSettings.mScreen1Panel;
            aForm.cBox_Screen2Panel.SelectedIndex = gAppSettings.mScreen2Panel;
            aForm.cBox_Screen3Panel.SelectedIndex = gAppSettings.mScreen3Panel;
            aForm.cBox_Screen4Panel.SelectedIndex = gAppSettings.mScreen4Panel;


            /* ファイル検索 */
            aForm.cBox_LanguageFile.Items.Add("English (Default)");

            int aLanguageIndex = 0;

            if (Directory.Exists(Application.StartupPath + "\\language\\") == true)
            {

                string[] files = Directory.GetFiles(Application.StartupPath + "\\language\\");

                for (int i = 0; i < files.Length; i++)
                {
                    string aFilename = Path.GetFileName(files[i]);
                    aForm.cBox_LanguageFile.Items.Add(aFilename);

                    if (aFilename == gAppSettings.mLanguageFile)
                    {
                        aLanguageIndex = i + 1;
                    }
                }
            }

            aForm.cBox_LanguageFile.SelectedIndex = aLanguageIndex;
            

            /* 設定フォームを開く */
            aForm.ShowDialog(this);

            //押されたボタン別の処理 
            if (aForm.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                gAppSettings.mSendMode = aForm.cBox_ConnectionType.SelectedIndex;
                gAppSettings.mIPAddress = aForm.cBox_IPAddress.Text;

                /* パワーオン中は操作不可 */
                if (aVisibleOption == true )
                {

                    //if ((IsOpenSendManager() == true) && (gAppSettings.mSendMode == 0))
                    //{
                        /* 開いていたら閉じる */
                        if (serialPort.IsOpen == true)
                        {
                            serialPort.Close();
                        }
                    //}

                    serialPort.PortName = aForm.cBox_SerialPort.Text;
                    serialPort.BaudRate = int.Parse(aForm.cBox_Baudrate.Text);
                    serialPort.DtrEnable = aForm.cBox_DtrEnable.Checked;
                    serialPort.RtsEnable = aForm.cBox_DtrEnable.Checked;
                }



                gAppSettings.mDCCMode = aForm.cBox_DCC.Checked;
                gAppSettings.mMfxAutoRegister = aForm.cBox_MfxAutoRegister.Checked;
                gAppSettings.mMfxAutoUpdate = aForm.cBox_MfxAutoUpdate.Checked;
                gAppSettings.mS88Sensor = aForm.cBox_S88.Checked;
                gAppSettings.mS88SendInterval = decimal.ToInt32(aForm.numUpDown_S88DetectFreq.Value);
                gAppSettings.mS88NumOfConnection = decimal.ToInt32(aForm.numUpDown_S88SensorNums.Value);
                gAppSettings.mSideFuncRight = aForm.cBox_SideFuncRight.SelectedIndex;
                gAppSettings.mSideFuncBottom = aForm.cBox_SideFuncBottom.SelectedIndex;
                gAppSettings.mWindowZoom = decimal.ToInt32(aForm.numUpDown_WindowZoom.Value);

                gAppSettings.mStopAllLocWhenPowerOn = aForm.cBox_StopAllLocPon.Checked;
                gAppSettings.mAutoCloseSerialport = aForm.cBox_AutoCloseSerial.Checked;
                gAppSettings.mClearAccessories = aForm.cBox_ClearAcc.Checked;

                /* 複数スクリーン */
                gAppSettings.mScreenNums = aForm.cBox_ScreenNums.SelectedIndex;
                gAppSettings.mScreen1Panel = aForm.cBox_Screen1Panel.SelectedIndex;
                gAppSettings.mScreen2Panel = aForm.cBox_Screen2Panel.SelectedIndex;
                gAppSettings.mScreen3Panel = aForm.cBox_Screen3Panel.SelectedIndex;
                gAppSettings.mScreen4Panel = aForm.cBox_Screen4Panel.SelectedIndex;

                /* ランゲージファイル */
                if (aForm.cBox_LanguageFile.SelectedIndex == 0)
                {
                    gAppSettings.mLanguageFile = "";
                }
                else
                {
                    gAppSettings.mLanguageFile = aForm.cBox_LanguageFile.Items[aForm.cBox_LanguageFile.SelectedIndex].ToString();
                }

                /* 表示系の更新 */
                MultiFunctionBox.Refresh();

            }

            //フォームの解放 
            aForm.Dispose();
        }

        public bool checkRoute_bridge(int inRouteNo, bool inAutoUpdate)
        {
            return Routes.RoutesList.CheckRoute(inRouteNo, inAutoUpdate);

        }

        public bool setRoute_bridge(int inRouteNo, bool inDirCheck)
        {
            return Routes.RoutesList.SetRoute(inRouteNo, inDirCheck);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            int i;
            string aConfigFilePath = Application.StartupPath + "\\config";

            /* 設定ファイルフォルダの存在チェック */
            if (Directory.Exists(aConfigFilePath) == false)
            {
                aConfigFilePath = Application.StartupPath;
            }

            /* XMLから設定を読み込み */
            ReadXMLSetting(aConfigFilePath + "\\settings.xml");

            /* ルートマネージャ生成 */
            Routes = new RouteManager(SerialCmd, S88Manager.GetS88Values, S88Manager.GetScriptValues , gAppSettings.mAccList, UpdateAccList);
            Routes.LoadFromFile(aConfigFilePath + "\\Routes.xml");


            /* 機関車DBを読み込み */
            if (File.Exists(aConfigFilePath + "\\LocDB.xml") == true)
            {
                LocDB.LoadFromFile_New(aConfigFilePath + "\\LocDB.xml");
            }
            else
            {
                LocDB.LoadFromFile(aConfigFilePath + "\\LocDB.dat");
            }

            /* マップを読み込み */
            LayoutMapData.LoadFromFile(aConfigFilePath + Program.FILE_LAYOUT);
            LayoutMapData.LoadRailMap(Application.StartupPath + "\\Resources\\Images\\railmap\\Railmap.png");

            /* イベント設定を読み込み */
            S88Manager.LoadFromFile(aConfigFilePath + Program.FILE_S88EVENT);
            UpdateS88Events();


            /* 実行ファイル登録の読込 */
            ExecuteManager.LoadFromFile(aConfigFilePath + Program.FILE_EXECUTE);

            /* アイコンファイル読込 */
            iconList.LoadFromFolder(Application.StartupPath + Program.FOLDER_ICON, FunctionImageList);

            /* ランゲージファイル読み込み */
            LangManager.LoadFromFile(Application.StartupPath + Program.FOLDER_LANGUAGE + gAppSettings.mLanguageFile);

            /* 言語ファイルの反映 */
            SetMainFormLanguages(LangManager);

            /* 設定を格納 */
            serialPort.BaudRate = gAppSettings.mBaudrate;
            serialPort.PortName = gAppSettings.mPortName;
            serialPort.DtrEnable = gAppSettings.mDtrEnable;
            serialPort.RtsEnable = gAppSettings.mDtrEnable;

            /* スケールファクタを変更 */
            if (gAppSettings.mWindowZoom > 100)
            {
                ScaleRation = gAppSettings.mWindowZoom;

                ResizeWindow();
            }


            /* レイアウトボックスのサイズを修正 */
            LayoutBox.Width = LayoutMapData.Width * (Program.TILE32_SIZE * ScaleRation / 100) + 1;
            LayoutBox.Height = LayoutMapData.Height * (Program.TILE32_SIZE * ScaleRation / 100) + 1;

            /* フォームサイズを直す */
            panel_AccList.Visible = false;
            panel_Layout.Visible = false;
            panel_Loc.Visible = true;
            panel6021.Visible = false;
            panel_Crane.Visible = false;

            /* パワーオフ */
            gControlMode = Program.POWER_OFF;

            cBox_6021Protcol.SelectedIndex = 0;

            /* 初期は操作できないようにする */
            ControlAvailability(false);

            /* 機関車イメージをセット(無効なファイル名を指定して内部画像を表示） */
            SetLocIconFileOnCab("");

            /* メーターボックスの背景を事前描画 */
            UpdateMeterBoxCache();

            /* 複数機関車制御パネルの設定 */
            for (i = 0; i < Program.MULTICONTROL_MAX; i++)
            {
                UpdateLocMultiCtrl(i);
            }

            //Cab
            UpdateLocDisplay();


            /* CV一覧 */
            CVList.Clear();
            cBox_CVProtcol.SelectedIndex = 2; //DCCをデフォルトにする
            cBox_DecoderManufacture.SelectedIndex = gAppSettings.mSelectDCCDecoder;
            chkListBox_CV29Calc.SetItemChecked(1, true);

            /* クレーン画面 */
            cBox_CraneProtcol.SelectedIndex = LocDB.GetAddressLocProtcol(CraneController.CraneLocAddress);
            numUpDownCraneAddress.Value = LocDB.GetAddress(CraneController.CraneLocAddress);
            cBox_CraneType.SelectedIndex = 0;

            /* 起動時の時刻をセット */
            timeBoot = DateTime.Now;


            /* CameraS88とのIPCの処理 */
            OpenIPCChannel();

            /*タイマ起動　*/
            timerSerialSend.Enabled = true;

            /* コマンドパラメータ取得 */

            //コマンドライン引数を配列で取得する
            string[] cmds = System.Environment.GetCommandLineArgs();

            //コマンドライン引数を列挙する
            foreach (string cmd in cmds)
            {
                if (cmd.ToLower() == "autostart")
                {
                    Flag_AutoStart = true;

                }
            }           

            /* サーバー開始 */
            AppServer.Start();

            /* 自動スタート */
            if (Flag_AutoStart == true)
            {
                ClickPower();
            }
            
            //イベント監視実行
            buttonEventInProcess.Text = LangManager.SetText("TxtEventInProgress", "In process");
            S88Manager.InProgress = true;

        }



        private void OpenIPCChannel()
        {
            // サーバーチャンネルの生成
            IpcServerChannel channel = new IpcServerChannel("cameras88");

            // チャンネルを登録
            ChannelServices.RegisterChannel(channel, true);

            // リモートオブジェクトを生成して公開
            m_msg = new IpcRemoteObject();

            m_msg.OnTrance += new IpcRemoteObject.CallEventHandler(m_msg_OnTrance);

            RemotingServices.Marshal(m_msg, "s88data", typeof(IpcRemoteObject));


        }

        //クライアントから転送されてきた情報をS88リストに反映表示
        void m_msg_OnTrance(IpcRemoteObject.RemoteObjectEventArg e)
        {
            if((e.Address <= 0) || (e.Address > 256))
            {
                return;
            }

            int aIndex = (e.Address - 1) >> 4;
            int aShift = (e.Address - 1) % 16;
            
            if( e.Status == 0)
            {
                S88Flags[aIndex] = S88Flags[aIndex] & (~(1 << aShift));
            }
            else
            {
                S88Flags[aIndex] = S88Flags[aIndex] | (1 << aShift);
            }

            // S88センサ状態変化チェック
            if (S88Manager.UpdateCheck(S88Flags) == true)
            {
                flagUpdateS88SensorView = true;
            }
                    
            // イベント状態変化チェック・実行許可
            if (S88Manager.IntervalCheck(S88Flags) == true)
            {
                flagUpdateS88SensorView = true;
            }
        }

        private void SetMainFormLanguages(Language inLangManager)
        {
            if (inLangManager.Loaded() == true)
            {

                //Menu Button

                SButton_Close.ToolTipText = LangManager.SetText("TxtIconClose", SButton_Close.ToolTipText);
                SButton_Loc.ToolTipText = LangManager.SetText("TxtIconCab", SButton_Loc.ToolTipText);
                SButton_MULTILOCS.ToolTipText = LangManager.SetText("TxtIconMLoc", SButton_MULTILOCS.ToolTipText);
                SButton_Acc.ToolTipText = LangManager.SetText("TxtIconAcc", SButton_Acc.ToolTipText);
                SButton_KPAD.ToolTipText = LangManager.SetText("TxtIconEasy", SButton_KPAD.ToolTipText);
                SButton_Layout.ToolTipText = LangManager.SetText("TxtIconLayout", SButton_Layout.ToolTipText);
                SButton_Sequence.ToolTipText = LangManager.SetText("TxtIconTeach", SButton_Sequence.ToolTipText);
                SButton_CVEditor.ToolTipText = LangManager.SetText("TxtIconCV", SButton_CVEditor.ToolTipText);
                SButton_S88.ToolTipText = LangManager.SetText("TxtIconEV", SButton_S88.ToolTipText);
                SButton_Crane.ToolTipText = LangManager.SetText("TxtIconCrane", SButton_Crane.ToolTipText);
                SButton_MScreen.ToolTipText = LangManager.SetText("TxtIconScreen", SButton_MScreen.ToolTipText);
                SButton_Console.ToolTipText = LangManager.SetText("TxtIconLog", SButton_Console.ToolTipText);
                SButton_CfgSerial.ToolTipText = LangManager.SetText("TxtIconCfg", SButton_CfgSerial.ToolTipText);
                SButton_RunFile.ToolTipText = LangManager.SetText("TxtIconEApp", SButton_RunFile.ToolTipText);
                SButton_VerInfo.ToolTipText = LangManager.SetText("TxtIconInfo", SButton_VerInfo.ToolTipText);
                SButton_Power.ToolTipText = LangManager.SetText("TxtIconPower", SButton_Power.ToolTipText);


                //Cab
                button_LocDBLoad.Text = inLangManager.SetText("TxtLoad", button_LocDBLoad.Text);
                button_LocDBSave.Text = inLangManager.SetText("TxtSave", button_LocDBSave.Text);

                Button_WebApp.Text = LangManager.SetText("TxtWebApp", Button_WebApp.Text);

                //Layout
                button_LayoutNew.Text = inLangManager.SetText("TxtNew", button_LayoutNew.Text);
                button_LayoutLoad.Text = inLangManager.SetText("TxtLoad", button_LayoutLoad.Text);
                button_LayoutSave.Text = inLangManager.SetText("TxtSave", button_LayoutSave.Text);
                button_LayoutCfg.Text = inLangManager.SetText("TxtCfg", button_LayoutCfg.Text);

                buttonEditAccOption.Text = inLangManager.SetText("TxtEditOption", buttonEditAccOption.Text);

                /* Sequence */
                buttonScriptLoad.Text = inLangManager.SetText("TxtImport", buttonScriptLoad.Text);
                buttonScriptSave.Text = inLangManager.SetText("TxtExport", buttonScriptSave.Text);
                buttonScriptTeach.Text = inLangManager.SetText("TxtTeach", buttonScriptTeach.Text);
                button_SeqScriptUp.Text = inLangManager.SetText("TxtItemUp", button_SeqScriptUp.Text);
                button_SeqScriptDown.Text = inLangManager.SetText("TxtItemDown", button_SeqScriptDown.Text);
                buttonScriptRun.Text = inLangManager.SetText("TxtRun", buttonScriptRun.Text);

                listScript.Columns[0].Text = inLangManager.SetText("TxtLine", listScript.Columns[0].Text);
                listScript.Columns[1].Text = inLangManager.SetText("TxtCommand", listScript.Columns[1].Text);
                listScript.Columns[2].Text = inLangManager.SetText("TxtParam", "Param") + "1";
                listScript.Columns[3].Text = inLangManager.SetText("TxtParam", "Param") + "2";
                listScript.Columns[4].Text = inLangManager.SetText("TxtParam", "Param") + "3";

                /* S88 */
                lView_S88Script.Columns[0].Text = inLangManager.SetText("TxtLine", lView_S88Script.Columns[0].Text);
                lView_S88Script.Columns[1].Text = inLangManager.SetText("TxtCommand", lView_S88Script.Columns[1].Text);
                lView_S88Script.Columns[2].Text = inLangManager.SetText("TxtParam", "Param") + "1";
                lView_S88Script.Columns[3].Text = inLangManager.SetText("TxtParam", "Param") + "2";
                lView_S88Script.Columns[4].Text = inLangManager.SetText("TxtParam", "Param") + "3";

                button_S88ScriptUp.Text = inLangManager.SetText("TxtItemUp", button_S88ScriptUp.Text);
                button_S88ScriptDown.Text = inLangManager.SetText("TxtItemDown", button_S88ScriptDown.Text);
                button_S88EventsImport.Text = inLangManager.SetText("TxtImport", button_S88EventsImport.Text);
                button_S88EventsExport.Text = inLangManager.SetText("TxtExport", button_S88EventsExport.Text);
                cBox_S88EventAvailable.Text = inLangManager.SetText("TxtS88Available", cBox_S88EventAvailable.Text);
                label_S88EventName.Text = inLangManager.SetText("TxtS88EventName", label_S88EventName.Text);
                label_S88StartTrigger.Text = inLangManager.SetText("TxtS88StartTrig", label_S88StartTrigger.Text);
                label_S88LocAddr.Text = inLangManager.SetText("TxtLocAddr", label_S88LocAddr.Text);
                label_S88EventScript.Text = inLangManager.SetText("TxtS88EventScript", label_S88EventScript.Text);
                tabS88_EventEdit.Text = inLangManager.SetText("TxtS88EventEditor", tabS88_EventEdit.Text);
                tabS88_Sensor.Text = inLangManager.SetText("TxtS88SensorInfo", tabS88_Sensor.Text);
                tabS88_ScriptInfo.Text = inLangManager.SetText("TxtS88ScriptInfo", tabS88_ScriptInfo.Text);
                gBox_S88Flag.Text = inLangManager.SetText("TxtS88FlagView", gBox_S88Flag.Text);
                gBox_S88RunChk.Text = inLangManager.SetText("TxtS88RunCheck", gBox_S88RunChk.Text);
                gBox_S88Log.Text = inLangManager.SetText("TxtS88ProcessLog", gBox_S88Log.Text);
                button_S88ConsoleClear.Text = inLangManager.SetText("TxtClear", button_S88ConsoleClear.Text);
                button_S88ScriptViewUpdate.Text = inLangManager.SetText("TxtUpdate", button_S88ScriptViewUpdate.Text);
                button_S88EventClear.Text = inLangManager.SetText("TxtS88ClearEvent", button_S88EventClear.Text);
                label_S88TriggerCondition.Text = inLangManager.SetText("TxtTriggerCondition", label_S88TriggerCondition.Text);
                button_TriggerProperty.Text = inLangManager.SetText("TxtTriggerOption", button_TriggerProperty.Text);
                gBox_EventClock.Text = inLangManager.SetText("TxtItemClock", gBox_EventClock.Text);
                button_S88ScriptReset.Text = inLangManager.SetText("TxtMsgReset", button_S88ScriptReset.Text);

                cBox_S88StartTrigger.Items[0] = inLangManager.SetText("TxtCondItemEntering", cBox_S88StartTrigger.Items[0].ToString());
                cBox_S88StartTrigger.Items[1] = inLangManager.SetText("TxtCondItemLeaving", cBox_S88StartTrigger.Items[1].ToString());
                cBox_S88StartTrigger.Items[2] = inLangManager.SetText("TxtCondItemExisting", cBox_S88StartTrigger.Items[2].ToString());
                cBox_S88StartTrigger.Items[3] = inLangManager.SetText("TxtCondItemNotExisting", cBox_S88StartTrigger.Items[3].ToString());
                cBox_S88StartTrigger.Items[4] = inLangManager.SetText("TxtCondItemInterval", cBox_S88StartTrigger.Items[4].ToString());
                cBox_S88StartTrigger.Items[5] = inLangManager.SetText("TxtCondItemClock", cBox_S88StartTrigger.Items[5].ToString());
                cBox_S88StartTrigger.Items[6] = inLangManager.SetText("TxtCondItemRandom", cBox_S88StartTrigger.Items[6].ToString());
                cBox_S88StartTrigger.Items[7] = inLangManager.SetText("TxtCondItemRunning", cBox_S88StartTrigger.Items[7].ToString());
                cBox_S88StartTrigger.Items[8] = inLangManager.SetText("TxtCondItemStopping", cBox_S88StartTrigger.Items[8].ToString());
                cBox_S88StartTrigger.Items[9] = inLangManager.SetText("TxtCondItemFlag", cBox_S88StartTrigger.Items[9].ToString());
                cBox_S88StartTrigger.Items[10] = inLangManager.SetText("TxtCondItemRoute", cBox_S88StartTrigger.Items[10].ToString());
                cBox_S88StartTrigger.Items[11] = inLangManager.SetText("TxtCondItemStartup", cBox_S88StartTrigger.Items[11].ToString());

                /* コンソール */
                button_ConsoleLogClear.Text = inLangManager.SetText("TxtClear", button_ConsoleLogClear.Text);
                button_CopySerialConsole.Text = inLangManager.SetText("TxtCopy", button_CopySerialConsole.Text);


                StatusLabel.Text = inLangManager.SetText("TxtMainWelcome", StatusLabel.Text);

                button_FWD.Text = inLangManager.SetText("TxtMainCabFwd", button_FWD.Text);
                button_REV.Text = inLangManager.SetText("TxtMainCabRev", button_REV.Text);

                SButton_Power.Text = inLangManager.SetText("TxtStart", SButton_Power.Text);
                this.Text = inLangManager.SetText("TxtMainTitle", this.Text);

                editFunctionButtonToolStripMenuItem.Text = inLangManager.SetText("TxtMainCabEditFunc", editFunctionButtonToolStripMenuItem.Text);
                editLeverOptionsToolStripMenuItem.Text = inLangManager.SetText("TxtMainCabEditLever", editLeverOptionsToolStripMenuItem.Text);

                /* コンテキストメニュー */
                MenuItemS88_Add.Text = inLangManager.SetText("TxtMenuAdd", MenuItemS88_Add.Text);
                MenuItemS88_Edit.Text = inLangManager.SetText("TxtMenuEdit", MenuItemS88_Edit.Text);
                MenuItemS88_Del.Text = inLangManager.SetText("TxtMenuDelete", MenuItemS88_Del.Text);
                MenuItemS88_Copy.Text = inLangManager.SetText("TxtMenuCopy", MenuItemS88_Copy.Text);
                MenuItemS88_Paste.Text = inLangManager.SetText("TxtMenuPaste", MenuItemS88_Paste.Text);
                MenuItem_Add.Text = inLangManager.SetText("TxtMenuAdd", MenuItem_Add.Text);
                MenuItem_Edit.Text = inLangManager.SetText("TxtMenuEdit", MenuItem_Edit.Text);
                MenuItem_Del.Text = inLangManager.SetText("TxtMenuDelete", MenuItem_Del.Text);
                MenuItem_Copy.Text = inLangManager.SetText("TxtMenuCopy", MenuItem_Copy.Text);
                MenuItem_Paste.Text = inLangManager.SetText("TxtMenuPaste", MenuItem_Paste.Text);

                /* 6021 */
                button6021_OFF.Text = inLangManager.SetText("Txt6021Off", button6021_OFF.Text);

                /* CV */
                label_CVProtcol.Text = inLangManager.SetText("TxtLocProtcol", label_CVProtcol.Text);
                label_CVLocAdr.Text = inLangManager.SetText("TxtLocAddr", label_CVLocAdr.Text);
                gBox_CVLocAdrRelated.Text = inLangManager.SetText("TxtCVLocAdrRelated", gBox_CVLocAdrRelated.Text);
                label_CV29Calc.Text = inLangManager.SetText("TxtCV29Calc", label_CV29Calc.Text);
                groupBox_CVEditor.Text = inLangManager.SetText("TxtCVManualReadWrite", groupBox_CVEditor.Text);
                button_DCCCVAdrRead.Text = inLangManager.SetText("TxtRead", button_DCCCVAdrRead.Text);
                button_CVDCCAdrWrite.Text = inLangManager.SetText("TxtWrite", button_CVDCCAdrWrite.Text);
                label_CVInfo.Text = inLangManager.SetText("TxtCVInfo", label_CVInfo.Text);
                label_CVNo.Text = inLangManager.SetText("TxtCVNo", label_CVNo.Text);
                label_CVValue.Text = inLangManager.SetText("TxtValue", label_CVValue.Text);
                buttonCVRead.Text = inLangManager.SetText("TxtRead", buttonCVRead.Text);
                buttonCVWrite.Text = inLangManager.SetText("TxtWrite", buttonCVWrite.Text);
                button_CVGenerate.Text = inLangManager.SetText("TxtGenerate", button_CVGenerate.Text);
                gBox_CVDCCAdrGen.Text = inLangManager.SetText("TxtCVDCCLocAdrTitle", gBox_CVDCCAdrGen.Text);
                label_CVManufacture.Text = inLangManager.SetText("TxtCVDecodermanufacture", label_CVManufacture.Text);
                

                chkListBox_CV29Calc.Items[0] = inLangManager.SetText("TxtCV29_Bit0", chkListBox_CV29Calc.Items[0].ToString());
                chkListBox_CV29Calc.Items[1] = inLangManager.SetText("TxtCV29_Bit1", chkListBox_CV29Calc.Items[1].ToString());
                chkListBox_CV29Calc.Items[2] = inLangManager.SetText("TxtCV29_Bit2", chkListBox_CV29Calc.Items[2].ToString());
                chkListBox_CV29Calc.Items[3] = inLangManager.SetText("TxtCV29_Bit3", chkListBox_CV29Calc.Items[3].ToString());
                chkListBox_CV29Calc.Items[4] = inLangManager.SetText("TxtCV29_Bit4", chkListBox_CV29Calc.Items[4].ToString());
                chkListBox_CV29Calc.Items[5] = inLangManager.SetText("TxtCV29_Bit5", chkListBox_CV29Calc.Items[5].ToString());
                chkListBox_CV29Calc.Items[6] = inLangManager.SetText("TxtCV29_Bit6", chkListBox_CV29Calc.Items[6].ToString());
                chkListBox_CV29Calc.Items[7] = inLangManager.SetText("TxtCV29_Bit7", chkListBox_CV29Calc.Items[7].ToString());

                /* Crane */
                gBox_CraneLoc.Text = inLangManager.SetText("TxtLocAddr", gBox_CraneLoc.Text);
                labelCraneProtcol.Text = inLangManager.SetText("TxtLocProtcol", labelCraneProtcol.Text);
                labelCraneLocAddr.Text = inLangManager.SetText("TxtLocAddr", labelCraneLocAddr.Text);
                gBoxCraneType.Text = inLangManager.SetText("TxtCraneType", gBoxCraneType.Text);

            }

        }

        private string getSerialPortName()
        {
            string aResult;
            aResult = serialPort.PortName;
            return aResult;
        }

        private void SButton_Power_Click(object sender, EventArgs e)
        {
            ClickPower();
        }

        private void ClickPower()
        {
            string[] aSerialPorts;
            bool aPortNameCheckOk;
            int i;
            bool aPortOpenCheck = false;

            aSerialPorts = System.IO.Ports.SerialPort.GetPortNames();

            /* Check available ports */
            switch (gAppSettings.mSendMode)
            {
                case 0:
                    //Serial
                    aPortOpenCheck = (aSerialPorts.Length == 0) ? false : true;
                    break;

                case 1:
                    //Http
                    aPortOpenCheck = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
                    break;

                case 2:
                    //Emulation
                    aPortOpenCheck = true;
                    break;
            }

            if (aPortOpenCheck == false)
            {
                StatusLabel.Text = LangManager.SetText("TxtMsgPortNotFound", "Unable to locate gateway port.");
                return;
            }

            /* Check port names */
            aPortNameCheckOk = false;

            for (i = 0; i < aSerialPorts.Length; i++)
            {
                if (aSerialPorts[i].IndexOf(serialPort.PortName) >= 0)
                {
                    aPortNameCheckOk = true;
                }
            }

            if ((aPortNameCheckOk == false) && (gAppSettings.mSendMode == 0))
            {

                StatusLabel.Text = getSerialPortName() + LangManager.SetText("TxtMsgPortNotAvailable", " is not available.");

                return;
            }

            /* Check port opened */
            if (IsOpenSendManager() == false)
            {
                try
                {
                    switch (gAppSettings.mSendMode)
                    {
                        case 0:
                            serialPort.Open();
                            break;

                        case 1:
                        case 2:
                        default:
                            //何もしない
                            break;
                    }

                }
                catch (ArgumentException ex)
                {
                    StatusLabel.Text = ex.Message;
                    return;
                }
            }
            
            if (IsOpenSendManager())
            {
                if (gControlMode == Program.POWER_OFF)
                {
                    StatusLabel.Text = LangManager.SetText("TxtMsgPortWait", "Please wait until the serial port has been opened.");

                    if (TrackPowerOn(true) == true)
                    {
                        StatusLabel.Text = getSerialPortName() + " " + LangManager.SetText("TxtMsgPortOpened", "has been opened.");
                    }
                    else
                    {
                        StatusLabel.Text = getSerialPortName() + LangManager.SetText("TxtMsgPortNotAvailable", " is not available.");
                    }

                }
                else
                {
                    /* 停止。 */
                    TrackPowerOff(true);

                    /* シリアルポートを閉じる(閉じるとMS2から制御不能になるので) */
                    if ((gAppSettings.mAutoCloseSerialport == true) && (gAppSettings.mSendMode == 0))
                    {
                        //シリアル通信が全て終わるまで待つ
                        DSCommon.WaitSleepTime(2);

                        //自動的にシリアルポートを閉じる
                        serialPort.Close();
                    }

                    //イベントを全て強制終了
                    S88Manager.ResetEvents();


                }
            }
            else
            {
                StatusLabel.Text = getSerialPortName() + LangManager.SetText("TxtMsgPortUnknownError", " unknown error.");
            }

        }

        private bool TrackPowerOn(bool inSendSerialCmd)
        {
            int aTimeoutCount = 0;

            SButton_Power.Enabled = false;
            SButton_Close.Enabled = false;

            S88Manager.Clear();

            CheckRailuinoResponse = 2;
            ConnectedStatusLabel.Text = "-";

            TBoxManager.Clear();

            if (inSendSerialCmd == true)
            {
                SerialCmd.SetPing();

                DSCommon.WaitSleepTime(2);

                gControlMode = SerialCmd.SetPower(Program.POWER_ON);

                StatusLabel.Text = StatusLabel.Text + " ";
                SerialCommunicated = false;

                while (SerialCommunicated == false)
                {
                    DSCommon.WaitSleepTime(2);
                    StatusLabel.Text = StatusLabel.Text + ".";
                    aTimeoutCount++;

                    // 5sec timeout
                    if (aTimeoutCount > 50)
                    {
                        SButton_Power.Enabled = true;
                        SButton_Close.Enabled = true;

                      
                        try
                        {
                            serialPort.DiscardInBuffer();
                            serialPort.Close();
                        }
                        catch (ArgumentException ex)
                        {
                            StatusLabel.Text = ex.Message;
                        }

                        gControlMode = Program.POWER_OFF;

                        return false;
                    }

                }
            }
            else
            {
                gControlMode = Program.POWER_ON;
            }

            SButton_Power.Enabled = true;
            SButton_Close.Enabled = true;
            SButton_Power.Text = LangManager.SetText("TxtStop", "STOP");
            ControlAvailability(true);

            UpdateLocDisplay();

            if ((gAppSettings.mStopAllLocWhenPowerOn == true) && (inSendSerialCmd == true))
            {
                StopAllLocs(false);
            }

            S88Manager.StartupRun();

            return true;
        }


        private void TrackPowerOff(bool inSendSerialCmd)
        {

            //スクリプト関連実行を停止する
            if (ScriptMode != Program.SCRIPTMODE_STOP)
            {
                ExitScript();
            }

            /* レールへの電源供給停止 */
            if (inSendSerialCmd == true)
            {
                gControlMode = SerialCmd.SetPower(Program.POWER_OFF);
            }
            else
            {
                gControlMode = Program.POWER_OFF;
            }

            /* 表示系の切り替え */
            SButton_Power.Text = LangManager.SetText("TxtStart", "START");
            ControlAvailability(false);
            //StatusLabel.Text = getSerialPortName() + LangManager.SetText("TxtMsgPortClosed", " has been closed.");
            /* 影響のあるところはすぐに更新 */
            MultiFunctionBox.Refresh();


            /* 応答チェックを初期化 */
            CheckRailuinoResponse = 2;
            //ConnectedStatusLabel.Text = "-";

        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Space)
            {
                /* 電源トグル切り替え */
                SButton_Power_Click(sender, e);

                return;

            }

            /* 電源断のときは無効化処理 */
            if (gControlMode == Program.POWER_OFF)
            {
                return;

            }

            /* パワーオフへの指令かどうかチェック */
            if (e.KeyCode == Keys.Escape)
            {
                /* 全列車停止(電源断はしない) */
                runEmergencyFunction();

                /* 描画予約 */
                flagUpdateByS88Sign = flagUpdateByS88Sign | Program.PANELUPDATE_LOC;

                /* すぐに描画させる */
                IntervalDrawAllPanel();

            }
            else if ((e.KeyCode == Keys.F1) && (e.Control == true))
            {
                typeAccFunction(0);
            }
            else if ((e.KeyCode == Keys.F2) && (e.Control == true))
            {
                typeAccFunction(1);
            }
            else if ((e.KeyCode == Keys.F3) && (e.Control == true))
            {
                typeAccFunction(2);
            }
            else if ((e.KeyCode == Keys.F4) && (e.Control == true))
            {
                typeAccFunction(3);
            }
            else if ((e.KeyCode == Keys.F5) && (e.Control == true))
            {
                typeAccFunction(4);
            }
            else if ((e.KeyCode == Keys.F6) && (e.Control == true))
            {
                typeAccFunction(5);
            }
            else if ((e.KeyCode == Keys.F7) && (e.Control == true))
            {
                typeAccFunction(6);
            }
            else if ((e.KeyCode == Keys.F8) && (e.Control == true))
            {
                typeAccFunction(7);
            }
            else if ((e.KeyCode == Keys.F9) && (e.Control == true))
            {
                typeAccFunction(8);
            }
            else if ((e.KeyCode == Keys.F10) && (e.Control == true))
            {
                typeAccFunction(9);
            }
            else if ((e.KeyCode == Keys.F11) && (e.Control == true))
            {
                typeAccFunction(10);
            }
            else if ((e.KeyCode == Keys.F12) && (e.Control == true))
            {
                typeAccFunction(11);
            }
            else
            {
                /* 機関車制御メイン画面以外はキー操作禁止 */
                if (panel_Loc.Visible == true)
                {
                    KeyDownLocPanel((int)e.KeyCode);
                }
                else if (panel_MultiLocs.Visible == true)
                {
                    KeyDownMultiCtrlPanel((int)e.KeyCode);
                }
                else if (panel6021.Visible == true)
                {
                    KeyDown6021Panel((int)e.KeyCode);
                }
                else if (panel_Layout.Visible == true)
                {
                    KeyDownLayoutPanel((int)e.KeyCode);
                }
            }

        }

        private void KeyDownLayoutPanel(int inKeyCode)
        {
            if (inKeyCode == (int)Keys.Delete)
            {
                if (SelectedTileIndex == 1)
                {
                    /* 割り当てS88・アクセサリアドレスの削除処理 */
                    LayoutMapData.UpdateLayoutS88Addr(SelectedLayoutIndex, 0);
                    LayoutMapData.UpdateLayoutAccNo(SelectedLayoutIndex, 0);

                    /* 結果を再描画 */
                    LayoutBox.Refresh();
                }
                else if (SelectedTileIndex >= 2)
                {
                    /* タイルを初期化する */
                    LayoutMapData.UpdateLayoutTile(SelectedLayoutIndex, 0);

                    /* 結果を再描画 */
                    LayoutBox.Refresh();
                }
            }
            else if ((inKeyCode == (int)Keys.Up) || (inKeyCode == (int)Keys.Down) || (inKeyCode == (int)Keys.Left) || (inKeyCode == (int)Keys.Right))
            {
                /* プレイモードではないときのみ動作 */
                if (SelectedTileIndex >= 1)
                {
                    /* 一時的にindexを移す */
                    int aTempIndex = SelectedLayoutIndex;


                    switch (inKeyCode)
                    {
                        case (int)Keys.Up:
                            aTempIndex = aTempIndex - LayoutMapData.Width;
                            break;
                        case (int)Keys.Down:
                            aTempIndex = aTempIndex + LayoutMapData.Width;
                            break;
                        case (int)Keys.Left:
                            aTempIndex = aTempIndex - 1;
                            break;
                        case (int)Keys.Right:
                            aTempIndex = aTempIndex + 1;
                            break;
                        default:
                            break;
                    }

                    /* 範囲内なら代入する */
                    if ((aTempIndex >= 0) && (aTempIndex < LayoutMapData.GetMaxCount()))
                    {
                        SelectedLayoutIndex = aTempIndex;
                    }

                    /* 結果を再描画 */
                    LayoutBox.Refresh();
                }
            }
        }

        private void KeyDown6021Panel(int inKeyCode)
        {


            /* ファンクション */
            if ((inKeyCode == (int)Keys.NumPad0) || (inKeyCode == (int)Keys.D0))
            {
                Update6021LocNum(0);
            }
            else if ((inKeyCode == (int)Keys.NumPad1) || (inKeyCode == (int)Keys.D1))
            {
                Update6021LocNum(1);
            }
            else if ((inKeyCode == (int)Keys.NumPad2) || (inKeyCode == (int)Keys.D2))
            {
                Update6021LocNum(2);
            }
            else if ((inKeyCode == (int)Keys.NumPad3) || (inKeyCode == (int)Keys.D3))
            {
                Update6021LocNum(3);
            }
            else if ((inKeyCode == (int)Keys.NumPad4) || (inKeyCode == (int)Keys.D4))
            {
                Update6021LocNum(4);
            }
            else if ((inKeyCode == (int)Keys.NumPad5) || (inKeyCode == (int)Keys.D5))
            {
                Update6021LocNum(5);
            }
            else if ((inKeyCode == (int)Keys.NumPad6) || (inKeyCode == (int)Keys.D6))
            {
                Update6021LocNum(6);
            }
            else if ((inKeyCode == (int)Keys.NumPad7) || (inKeyCode == (int)Keys.D7))
            {
                Update6021LocNum(7);
            }
            else if ((inKeyCode == (int)Keys.NumPad8) || (inKeyCode == (int)Keys.D8))
            {
                Update6021LocNum(8);
            }
            else if ((inKeyCode == (int)Keys.NumPad9) || (inKeyCode == (int)Keys.D9))
            {
                Update6021LocNum(9);
            }
            else if ((inKeyCode == (int)Keys.Delete) || (inKeyCode == (int)Keys.Back))
            {
                /* アドレスクリア */
                Update6021DisplayAddress(0);
            }
            else if ((inKeyCode == (int)Keys.Up) || (inKeyCode == (int)Keys.Right))
            {
                KeyDown6021Panel_Speed(32);
            }
            else if ((inKeyCode == (int)Keys.Down) || (inKeyCode == (int)Keys.Left))
            {
                KeyDown6021Panel_Speed(-32);
            }

        }

        private void KeyDown6021Panel_Speed(int inDelta)
        {

            int aDialValue = Dial6021Value + inDelta;

            if (aDialValue <= 0)
            {
                aDialValue = 0;
            }
            
            else if( aDialValue > 1024)
            {
                aDialValue = 1024;
            }


            if (aDialValue != Dial6021Value)
            {
                /* 速度値を確定 */
                Dial6021Value = aDialValue;
                NextAutoSpeedSet6021 = true;
                NextAutoSpeedAddr6021 = gTypedAddr;

                /* 再描画 */
                Dial6021Box.Refresh();

            }


        }

        private void KeyDownMultiCtrlPanel(int inKeyCode)
        {
            int aTag = -1;
            int aIndex = 0;

            aTag = CurrentSelectedMultiLocIndex;

            /* 該当なしのときの処理 */
            if (aTag < 0)
            {
                return;
            }

            /* 機関車データベースの番号を取得 */
            aIndex = gAppSettings.mLocCtrlList[aTag] - 1;

            if (inKeyCode == (int)Keys.ShiftKey)
            {
                /* 表示の更新 */
                DisplayExFuncButtonOnMultiPanel(true);
                UpdateFunctionButtons_MultiLocos(aIndex, true);

            }


            /* スピード制御モード */
            if ((inKeyCode == Program.KEYMAP_PPLUS) || (inKeyCode == Program.KEYMAP_UP))
            {
                if (IncrementSpeedLever(aIndex) == true)
                {
                    UpdateSelectedMultiLocs();
                }

            }
            else if ((inKeyCode == Program.KEYMAP_PMINUS) || (inKeyCode == Program.KEYMAP_DOWN))
            {
                if (DecrementSpeedLever(aIndex) == true)
                {
                    UpdateSelectedMultiLocs();
                }
            }

            /* 進行方向 */
            if (inKeyCode == Program.KEYMAP_TOGGLEDIRECTION)
            {
                ToggleLocDirection(aIndex);
            }

            /* ファンクション */
            if (inKeyCode == Program.KEYMAP_FUNC01)
            {
                RunFunctionButton(0, aIndex, buttonMultiF1);
            }
            else if (inKeyCode == Program.KEYMAP_FUNC02)
            {
                RunFunctionButton(1, aIndex, buttonMultiF2);
            }
            else if (inKeyCode == Program.KEYMAP_FUNC03)
            {
                RunFunctionButton(2, aIndex, buttonMultiF3);
            }
            else if (inKeyCode == Program.KEYMAP_FUNC04)
            {
                RunFunctionButton(3, aIndex, buttonMultiF4);
            }
            else if (inKeyCode == Program.KEYMAP_FUNC05)
            {
                RunFunctionButton(4, aIndex, buttonMultiF5);
            }
            else if (inKeyCode == Program.KEYMAP_FUNC06)
            {
                RunFunctionButton(5, aIndex, buttonMultiF6);
            }
            else if (inKeyCode == Program.KEYMAP_FUNC07)
            {
                RunFunctionButton(6, aIndex, buttonMultiF7);
            }
            else if (inKeyCode == Program.KEYMAP_FUNC08)
            {
                RunFunctionButton(7, aIndex, buttonMultiF8);
            }
            else if (inKeyCode == Program.KEYMAP_FUNC09)
            {
                RunFunctionButton(8, aIndex, buttonMultiF9);
            }
            else if (inKeyCode == Program.KEYMAP_FUNC10)
            {
                RunFunctionButton(9, aIndex, buttonMultiF10);
            }
            else if (inKeyCode == Program.KEYMAP_FUNC11)
            {
                RunFunctionButton(10, aIndex, buttonMultiF11);
            }
            else if (inKeyCode == Program.KEYMAP_FUNC12)
            {
                RunFunctionButton(11, aIndex, buttonMultiF12);
            }
        }

        private void DisplayExFuncButtonOnLocPanel(bool inShow)
        {
            if (inShow == true)
            {
                button_F1.Text = "F16";
                button_F2.Text = "F17";
                button_F3.Text = "F18";
                button_F4.Text = "F19";
                button_F5.Text = "F20";
                button_F6.Text = "F21";
                button_F7.Text = "F22";
                button_F8.Text = "F23";
                button_F9.Text = "F24";
                button_F10.Text = "F25";
                button_F11.Text = "F26";
                button_F12.Text = "F27";
                button_F13.Text = "F28";
                button_F14.Text = "";
                button_F15.Text = "";
                button_F16.Text = "";
                button_F14.Enabled = false;
                button_F15.Enabled = false;
                button_F16.Enabled = false;
            }
            else
            {
                button_F1.Text = "F0";
                button_F2.Text = "F1";
                button_F3.Text = "F2";
                button_F4.Text = "F3";
                button_F5.Text = "F4";
                button_F6.Text = "F5";
                button_F7.Text = "F6";
                button_F8.Text = "F7";
                button_F9.Text = "F8";
                button_F10.Text = "F9";
                button_F11.Text = "F10";
                button_F12.Text = "F11";
                button_F13.Text = "F12";
                button_F14.Text = "F13";
                button_F15.Text = "F14";
                button_F16.Text = "F15";
                button_F14.Enabled = true;
                button_F15.Enabled = true;
                button_F16.Enabled = true;
            }


        }


        private void DisplayExFuncButtonOnMultiPanel(bool inShow)
        {
            if (inShow == true)
            {
                buttonMultiF1.Text = "F16";
                buttonMultiF2.Text = "F17";
                buttonMultiF3.Text = "F18";
                buttonMultiF4.Text = "F19";
                buttonMultiF5.Text = "F20";
                buttonMultiF6.Text = "F21";
                buttonMultiF7.Text = "F22";
                buttonMultiF8.Text = "F23";
                buttonMultiF9.Text = "F24";
                buttonMultiF10.Text = "F25";
                buttonMultiF11.Text = "F26";
                buttonMultiF12.Text = "F27";
                buttonMultiF13.Text = "F28";
                buttonMultiF14.Text = "";
                buttonMultiF15.Text = "";
                buttonMultiF16.Text = "";
                buttonMultiF14.Enabled = false;
                buttonMultiF15.Enabled = false;
                buttonMultiF16.Enabled = false;
            }
            else
            {
                buttonMultiF1.Text = "F0";
                buttonMultiF2.Text = "F1";
                buttonMultiF3.Text = "F2";
                buttonMultiF4.Text = "F3";
                buttonMultiF5.Text = "F4";
                buttonMultiF6.Text = "F5";
                buttonMultiF7.Text = "F6";
                buttonMultiF8.Text = "F7";
                buttonMultiF9.Text = "F8";
                buttonMultiF10.Text = "F9";
                buttonMultiF11.Text = "F10";
                buttonMultiF12.Text = "F11";
                buttonMultiF13.Text = "F12";
                buttonMultiF14.Text = "F13";
                buttonMultiF15.Text = "F14";
                buttonMultiF16.Text = "F15";
                buttonMultiF14.Enabled = true;
                buttonMultiF15.Enabled = true;
                buttonMultiF16.Enabled = true;
            }


        }

        private void KeyUpLocPanel(int inKeyCode)
        {
            if (inKeyCode == (int)Keys.ShiftKey)
            {
                /* 表示の更新 */
                DisplayExFuncButtonOnLocPanel(false);
                UpdateFunctionButtons(SelectedLocIndex, false);

            }
        }

        private void KeyUpMultiPanel(int inKeyCode)
        {
            int aTag = -1;
            int aIndex = 0;

            aTag = CurrentSelectedMultiLocIndex;

            /* 該当なしのときの処理 */
            if (aTag < 0)
            {
                return;
            }

            /* 機関車データベースの番号を取得 */
            aIndex = gAppSettings.mLocCtrlList[aTag] - 1;


            if (inKeyCode == (int)Keys.ShiftKey)
            {
                /* 表示の更新 */
                DisplayExFuncButtonOnMultiPanel(false);
                UpdateFunctionButtons_MultiLocos(aIndex, false);

            }
        }

        private void KeyDownLocPanel(int inKeyCode)
        {
            if (inKeyCode == (int)Keys.ShiftKey)
            {
                /* 表示の更新 */
                DisplayExFuncButtonOnLocPanel(true);
                UpdateFunctionButtons(SelectedLocIndex, true);

            }


            if (gAppSettings.mSpeedLeverMode == 0)
            {
                /* スピード制御モード */
                if ((inKeyCode == Program.KEYMAP_PPLUS) || (inKeyCode == Program.KEYMAP_UP))
                {
                    if (IncrementSpeedLever(SelectedLocIndex) == true)
                    {
                        LeverBox.Refresh();
                        MeterBox.Refresh();
                    }
                }
                else if ((inKeyCode == Program.KEYMAP_PMINUS) || (inKeyCode == Program.KEYMAP_DOWN))
                {
                    if (DecrementSpeedLever(SelectedLocIndex) == true)
                    {
                        LeverBox.Refresh();
                        MeterBox.Refresh();
                    }
                }
            }
            else
            {
                /* パワー制御モード */


                if (((inKeyCode == Program.KEYMAP_PPLUS) || (inKeyCode == Program.KEYMAP_UP)) && (LeverValue >= 0))
                {
                    /* 加速(ノッチ進) */
                    IncrementPowerLever();
                }
                else if (((inKeyCode == Program.KEYMAP_PMINUS) || (inKeyCode == Program.KEYMAP_DOWN)) && (LeverValue > 0))
                {
                    /* 加速(ノッチ戻) */
                    DecrementPowerLever();
                }
                else if ((inKeyCode == Program.KEYMAP_FREE))
                {
                    /* ノッチフリーラン */
                    UpdatePowerLever(0);
                }
                else if (((inKeyCode == Program.KEYMAP_BMINUS) || (inKeyCode == Program.KEYMAP_UP)) && (LeverValue < 0))
                {
                    /* 減速(ノッチ進) */
                    IncrementPowerLever();
                }
                else if (((inKeyCode == Program.KEYMAP_BPLUS) || (inKeyCode == Program.KEYMAP_DOWN)) && (LeverValue <= 0))
                {
                    /* 減速(ノッチ戻) */
                    DecrementPowerLever();
                }
            }

            /* 非常停止 */
            if ((inKeyCode == Program.KEYMAP_EMERGENCY))
            {
                ResetSpeedOnLocPanel(SelectedLocIndex);

                UpdatePowerLever(0);

                /* メーター更新 */
                MeterBox.Refresh();

                /* レバー更新 */
                LeverBox.Refresh();
            }

            /* 進行方向 */
            if (inKeyCode == Program.KEYMAP_TOGGLEDIRECTION)
            {
                ToggleLocDirection(SelectedLocIndex);
                MeterBox.Refresh();
            }

            /* ファンクション */
            if (inKeyCode == Program.KEYMAP_FUNC01)
            {
                RunFunctionButton(0, SelectedLocIndex, button_F1);
            }
            else if (inKeyCode == Program.KEYMAP_FUNC02)
            {
                RunFunctionButton(1, SelectedLocIndex, button_F2);
            }
            else if (inKeyCode == Program.KEYMAP_FUNC03)
            {
                RunFunctionButton(2, SelectedLocIndex, button_F3);
            }
            else if (inKeyCode == Program.KEYMAP_FUNC04)
            {
                RunFunctionButton(3, SelectedLocIndex, button_F4);
            }
            else if (inKeyCode == Program.KEYMAP_FUNC05)
            {
                RunFunctionButton(4, SelectedLocIndex, button_F5);
            }
            else if (inKeyCode == Program.KEYMAP_FUNC06)
            {
                RunFunctionButton(5, SelectedLocIndex, button_F6);
            }
            else if (inKeyCode == Program.KEYMAP_FUNC07)
            {
                RunFunctionButton(6, SelectedLocIndex, button_F7);
            }
            else if (inKeyCode == Program.KEYMAP_FUNC08)
            {
                RunFunctionButton(7, SelectedLocIndex, button_F8);
            }
            else if (inKeyCode == Program.KEYMAP_FUNC09)
            {
                RunFunctionButton(8, SelectedLocIndex, button_F9);
            }
            else if (inKeyCode == Program.KEYMAP_FUNC10)
            {
                RunFunctionButton(9, SelectedLocIndex, button_F10);
            }
            else if (inKeyCode == Program.KEYMAP_FUNC11)
            {
                RunFunctionButton(10, SelectedLocIndex, button_F11);
            }
            else if (inKeyCode == Program.KEYMAP_FUNC12)
            {
                RunFunctionButton(11, SelectedLocIndex, button_F12);
            }
        }




        private void panelLocCtrl0_Click(object sender, EventArgs e)
        {
            /* 複数機関車制御の選択 */
            int aTag;

            Panel aPanel = sender as Panel;
            aTag = Convert.ToInt32(aPanel.Tag);

            ChangeButtonMultiLocs(aTag);
        }

        private void LocImageBox0_Click(object sender, EventArgs e)
        {
            /* 複数機関車制御の選択 */
            int aTag;

            PictureBox aPanel = sender as PictureBox;
            aTag = Convert.ToInt32(aPanel.Tag);

            ChangeButtonMultiLocs(aTag);
        }

        private bool getAccListDoubleType(int inIndex)
        {
            bool aResult = false;

            if( inIndex >= 0)
            {

                switch (gAppSettings.mAccList[inIndex].mType)
                {
                    case 4:
                    case 6:
                    case 8:
                    case 9:
                        aResult = true;
                        break;
                }
            }

            return aResult;
        }

        private bool getAccListDoubleType_3max(int inIndex)
        {
            bool aResult = false;

            if (inIndex >= 0)
            {

                switch (gAppSettings.mAccList[inIndex].mType)
                {
                    case 4: //(4パターンあり)
                    case 6:
                    case 8:
                        aResult = true;
                        break;
                }
            }

            return aResult;
        }

        private void pBox_AccList_Paint(object sender, PaintEventArgs e)
        {
            int i;
            int aTopIndex;
            int aImageNo;
            int ax, ay;
            int aCurrentIndex;
            int aCurrentImageIndex;

            /* キャンバスを取得 */
            Graphics aCanvas = e.Graphics;

            /* アンチエイリアス有効 */
            aCanvas.SmoothingMode = SmoothingMode.AntiAlias;
            aCanvas.TextRenderingHint = TextRenderingHint.AntiAlias;

            /* 文字系 */
            System.Drawing.Font aDrawFont = new System.Drawing.Font("Arial", 15 * ScaleRation / 100, FontStyle.Bold);

            aTopIndex = AccScrollBar.Value * 40;

            for (i = 0; i < 40; i++)
            {
                aCurrentIndex = i + aTopIndex;

                if (getAccListDoubleType(aCurrentIndex) == true)
                {
                    //複数のアドレスを使用するアクセサリ
                    aImageNo = gAppSettings.mAccList[aCurrentIndex].mType * 10 + gAppSettings.mAccList[aCurrentIndex].mDirection + gAppSettings.mAccList[aCurrentIndex + 1].mDirection * 2;
                }
                else
                {
                    //複数のアドレスを使用しないアクセサリ
                    aImageNo = gAppSettings.mAccList[aCurrentIndex].mType * 10 + gAppSettings.mAccList[aCurrentIndex].mDirection;
                }

                aCurrentImageIndex = GetAccTileImageIndex(aImageNo);

                /* 位置演算 */
                ax = (i % Program.ACCESSORIES_IDXWIDTH) * Program.ACCESSORIES_ITEMWIDTH;
                ay = (i / Program.ACCESSORIES_IDXWIDTH) * Program.ACCESSORIES_ITEMHEIGHT;


                /* 一つ前が２重のアドレス利用の場合は描画しない */
                if (getAccListDoubleType(aCurrentIndex - 1) == false)
                {
                    int aTextX = (ax + 32 + 5) * ScaleRation / 100;
                    int aTextY = (ay + 10) * ScaleRation / 100;
                    float aRectX = ax * ScaleRation / 100;
                    float aRectY = ay * ScaleRation / 100;
                    float aRectW = 96 * ScaleRation / 100;
                    float aRectH = 74 * ScaleRation / 100;


                    /* 選択中なら枠を描画 */
                    if (SelectedAccessoryIndex == aCurrentIndex)
                    {
                        DrawIconRoundRectangle(aCanvas, aRectX, aRectY, aRectW, aRectH, 3, true, Color.FromArgb(240, 240, 240), Color.Silver, Color.Red);
                        aCanvas.DrawString(string.Format("{0, 3}", aCurrentIndex + 1), aDrawFont, Brushes.Red, aTextX, aTextY);
                    }
                    else
                    {
                        DrawIconRoundRectangle(aCanvas, aRectX, aRectY, aRectW, aRectH, 3, true, Color.FromArgb(240, 240, 240), Color.Silver, Color.DimGray);
                        aCanvas.DrawString(string.Format("{0, 3}", aCurrentIndex + 1), aDrawFont, Brushes.Black, aTextX, aTextY);
                    }

                    /* 描画 */
                    aCanvas.DrawImage(AccessoryImageList.Images[aCurrentImageIndex], (ax + 5) * ScaleRation / 100, (ay + 5) * ScaleRation / 100, 32 * ScaleRation / 100, 64 * ScaleRation / 100);

                    /* 非表示描画 */
                    if (gAppSettings.mAccList[aCurrentIndex].mInvisible == true)
                    {
                        Brush aBrush = new SolidBrush(Color.FromArgb(200, SystemColors.Control));

                        /* 薄く描画する */
                        aCanvas.FillRectangle(aBrush, aRectX, aRectY, aRectW, aRectH);

                    }

                }

            }
        }

        private int GetAccTileImageIndex(int inImageNo)
        {
            int aCurrentImageIndex;

            switch (inImageNo)
            {
                case 0: aCurrentImageIndex = 0;
                    break;
                case 1: aCurrentImageIndex = 1;
                    break;
                case 10: aCurrentImageIndex = 8;
                    break;
                case 11: aCurrentImageIndex = 7;
                    break;
                case 20: aCurrentImageIndex = 11;
                    break;
                case 21: aCurrentImageIndex = 10;
                    break;
                case 30: aCurrentImageIndex = 12;
                    break;
                case 31: aCurrentImageIndex = 13;
                    break;
                case 40: aCurrentImageIndex = 3;
                    break;
                case 41: aCurrentImageIndex = 4;
                    break;
                case 42: aCurrentImageIndex = 5;
                    break;
                case 43: aCurrentImageIndex = 2;
                    break;
                //Yard Signal
                case 50: aCurrentImageIndex = 14;
                    break;
                case 51: aCurrentImageIndex = 15;
                    break;
                //Distant Signal(76383)
                case 60: aCurrentImageIndex =  16;
                    break;
                case 61: aCurrentImageIndex =  17;
                    break;
                case 62: aCurrentImageIndex =  18;
                    break;
                //Home Signal(76391)
                case 70: aCurrentImageIndex =  19;
                    break;
                case 71: aCurrentImageIndex =  20;
                    break;
                //Home Signal(76393)
                case 80: aCurrentImageIndex =  21;
                    break;
                case 81: aCurrentImageIndex =  22;
                    break;
                case 82: aCurrentImageIndex =  23;
                    break;
                //Home Signal(76394)
                case 90: aCurrentImageIndex =  24;
                    break;
                case 91: aCurrentImageIndex =  25;
                    break;
                case 92: aCurrentImageIndex =  26;
                    break;
                case 93: aCurrentImageIndex = 27;
                    break;
    
                default:
                    aCurrentImageIndex = 0;
                    break;
            }

            return aCurrentImageIndex;
   
        }


        private void AccScrollBar_ValueChanged(object sender, EventArgs e)
        {

            pBox_AccList.Refresh();
        }

        private void pBox_AccList_MouseDown(object sender, MouseEventArgs e)
        {
            int ax, ay;
            int bx, by;
            int aIndex;
            int aTopIndex;
            bool aRefresh = false;

            if (gControlMode == Program.POWER_OFF)
            {
                return;
            }

            ax = e.X / (Program.ACCESSORIES_ITEMWIDTH * ScaleRation / 100);
            ay = e.Y / (Program.ACCESSORIES_ITEMHEIGHT * ScaleRation / 100);
            bx = e.X % (Program.ACCESSORIES_ITEMWIDTH * ScaleRation / 100);
            by = e.Y % (Program.ACCESSORIES_ITEMHEIGHT * ScaleRation / 100);

            /* 最大値チェック */
            if ((ax > Program.ACCESSORIES_IDXWIDTH) || (ax < 0) || (ay > Program.ACCESSORIES_IDXHEIGHT) || (ay < 0))
            {
                return;
            }

            /* 順番を算出する */
            aTopIndex = AccScrollBar.Value * 40;
            aIndex = ay * Program.ACCESSORIES_IDXWIDTH + ax + aTopIndex;

            /* 一つ前がアドレスを二つ使うタイプの場合 */
            if (getAccListDoubleType(aIndex - 1) == true)
            {
                return;
            }

            /* 前回値と選択アドレスが異なる場合 */
            if (SelectedAccessoryIndex != aIndex)
            {
                aRefresh = true;
                SelectedAccessoryIndex = aIndex;
            }

            if (e.Button == MouseButtons.Left)
            {
                //枠内をクリックか
                if ((bx <= (32 * ScaleRation / 100)) && (by <= (64 * ScaleRation / 100)))
                {
                    //非表示設定になっていないか
                    if (gAppSettings.mAccList[aIndex].mInvisible == false)
                    {
                        SetAccCommand(aIndex);

                        aRefresh = true;
                    }
                }

                SetAccTips(aIndex);

            }

            if (aRefresh == true)
            {
                pBox_AccList.Refresh();
            }

        }

        private void typeAccFunction(int inIndex)
        {
            int aIndex = inIndex;


            /* 一つ前がアドレスを二つ使うタイプの場合 */
            if (getAccListDoubleType(aIndex - 1) == true)
            {
                return;
            }

            //非表示設定になっていないか
            if (gAppSettings.mAccList[aIndex].mInvisible == false)
            {
                SetAccCommand(aIndex);

                /* 描画予約 */
                flagUpdateByS88Sign = flagUpdateByS88Sign | Program.PANELUPDATE_ACC;

                /* すぐに描画させる */
                IntervalDrawAllPanel();

            }

        }



        private void SetAccTips(int inIndex)
        {
            if (inIndex != DisplayedAccessoryTipIndex)
            {
                InfoTip.SetToolTip(pBox_AccList, gAppSettings.mAccList[inIndex].mComment);
                DisplayedAccessoryTipIndex = inIndex;
            }
        }

        private void OpenFormSelectAccessoryType(int inIndex)
        {
            SelectAccessoryForm aForm = new SelectAccessoryForm(AccessoryImageList);

            aForm.SetFormLanguage(LangManager);

            aForm.cBox_AccType.SelectedIndex = gAppSettings.mAccList[inIndex].mType;
            aForm.Text = aForm.Text+ " (No." + (inIndex + 1).ToString() + ")";
            aForm.tBox_AccComment.Text = gAppSettings.mAccList[inIndex].mComment;
            aForm.cBox_ReverseSignal.Checked = gAppSettings.mAccList[inIndex].mReverse;
            aForm.cBox_Invisible.Checked = gAppSettings.mAccList[inIndex].mInvisible;
            aForm.cBox_ACCProtocol.SelectedIndex = gAppSettings.mAccList[inIndex].mProtocol;

            aForm.SelectedIndex = inIndex;

            aForm.ShowDialog(this);

            //押されたボタン別の処理 
            if (aForm.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                gAppSettings.mAccList[inIndex].mType = aForm.cBox_AccType.SelectedIndex;
                gAppSettings.mAccList[inIndex].mComment = aForm.tBox_AccComment.Text;
                gAppSettings.mAccList[inIndex].mReverse = aForm.cBox_ReverseSignal.Checked;
                gAppSettings.mAccList[inIndex].mInvisible = aForm.cBox_Invisible.Checked;
                gAppSettings.mAccList[inIndex].mProtocol = aForm.cBox_ACCProtocol.SelectedIndex;

                pBox_AccList.Refresh();
            }

            //フォームの解放 
            aForm.Dispose();
        }

        private void buttonEditAccOption_Click(object sender, EventArgs e)
        {
            OpenFormSelectAccessoryType(SelectedAccessoryIndex);
        }

        private void cBox_6021Protcol_SelectedIndexChanged(object sender, EventArgs e)
        {
            int aRefAddress;

            /* アドレス更新 */
            aRefAddress = LocDB.GetAddress(gTypedAddr);

            gTypedAddr = LocDB.AssignAddressProtcol(cBox_6021Protcol.SelectedIndex, aRefAddress);
            label_6021Protcol.Text = LocDB.GetProtcolName(cBox_6021Protcol.SelectedIndex);

            /* 適当なところにフォーカスを逃がす */
            label_Addr.Focus();

        }


        private void pBox_S88SensorDisplay_Paint(object sender, PaintEventArgs e)
        {
            /* 描画処理 */

            /* キャンバスを取得 */
            Graphics aCanvas = e.Graphics;

            /* アンチエイリアス有効 */
            aCanvas.SmoothingMode = SmoothingMode.AntiAlias;
            aCanvas.TextRenderingHint = TextRenderingHint.AntiAlias;

            MeterDrawer.DrawS88SensorDisplay(aCanvas, S88Flags, ScaleRation);


        }


        private void timerS88_Tick(object sender, EventArgs e)
        {
            if (gControlMode == Program.POWER_OFF)
            {
                return;
            }

            if (flagUpdateS88SensorView == true)
            {
                /* Layout画面でS88センサ状況を表示している場合は更新 */

                if ((panel_Layout.Visible == true) && (LayoutMapData.UsingS88 == true))
                {
                    LayoutBox.Refresh();
                }

                /* S88画面表示中は更新 */
                if ((panel_Loc.Visible == true) && (gAppSettings.mSideFuncBottom == Program.BOTTOMBAR_S88))
                {
                    MultiFunctionBox.Refresh();
                }

                /* S88画面表示中は更新 */
                if (panel_S88.Visible == true)
                {
                    pBox_S88SensorDisplay.Refresh();
                }
                
                flagUpdateS88SensorView = false;
            }

            /* S88の実行 */
            S88Manager.IntervalRun();
            /* 速度遷移処理の実行 */
            UpdateTransition();

            /* S88データ取得周期 */
            if (gAppSettings.mS88Sensor == true)
            {
                if (Counter_S88 >= gAppSettings.mS88SendInterval)
                {
                    SendCommand(Program.SERIALCMD_GETS88 + "(" + gAppSettings.mS88NumOfConnection.ToString() + ")");

                    Counter_S88 = 0;
                }
                else
                {
                    Counter_S88 = Counter_S88 + 1;
                }
            }


        }

        private void lBox_S88Events_SelectedIndexChanged(object sender, EventArgs e)
        {

            int aIndex;

            /* 選択番号変更 */
            aIndex = lBox_S88Events.SelectedIndex;

            if ((aIndex < 0) || (aIndex >= Program.S88EVENT_MAX))
            {
                return;
            }

            /* 設定反映 */
            cBox_S88EventAvailable.Checked = S88Manager.mEvents[aIndex].mAvailable;
            tBox_S88EventName.Text = S88Manager.mEvents[aIndex].EventName;

            if (S88Manager.mEvents[aIndex].mSlotIndex == 0)
            {
                if (S88Manager.mEvents[aIndex].mLocIndex < cBox_LocChangeEV.Items.Count)
                {
                    cBox_LocChangeEV.SelectedIndex = S88Manager.mEvents[aIndex].mLocIndex + 1;
                }
                else
                {
                    cBox_LocChangeEV.SelectedIndex = cBox_LocChangeEV.Items.Count;
                }
            }
            else
            {
                cBox_LocChangeEV.SelectedIndex = 0;
                cBox_SlotChangeEV.SelectedIndex = S88Manager.mEvents[aIndex].mSlotIndex;
            }


            cBox_S88StartTrigger.SelectedIndex = S88Manager.mEvents[aIndex].TriggerType;  
            
            /* トリガオプションの表示変更 */
            UpdateS88ScriptTriggerOption();

            /* スクリプト登録 */
            UpdateS88ScriptDisplay();
 
        }

        private void tBox_S88EventName_TextChanged(object sender, EventArgs e)
        {
            int aIndex;

            /* 選択番号変更 */
            aIndex = lBox_S88Events.SelectedIndex;

            if ((aIndex < 0) || (aIndex >= Program.S88EVENT_MAX))
            {
                return;
            }

            S88Manager.mEvents[aIndex].EventName = tBox_S88EventName.Text;
            /* 一覧も更新する */
            lBox_S88Events.Items[aIndex] = GetS88EventName(aIndex, S88Manager.mEvents[aIndex].EventName);
        }

        private void cBox_S88EventAvailable_CheckedChanged(object sender, EventArgs e)
        {
            int aIndex;

            /* 選択番号変更 */
            aIndex = lBox_S88Events.SelectedIndex;

            if ((aIndex < 0) || (aIndex >= Program.S88EVENT_MAX))
            {
                return;
            }

            S88Manager.mEvents[aIndex].mAvailable = cBox_S88EventAvailable.Checked;
        }


        static string ToAlphabet(int inNumber)
        {
            int aNumber;

            aNumber = inNumber + 1;

            if (aNumber <= 0)
            {
                return "";
            }

            int n = aNumber % 26;

            n = (n == 0) ? 26 : n;

            string s = ((char)(n + 64)).ToString();

            if (aNumber == n)
            {
                return s;
            }
            else
            {
                return s;
            }
        }

        private string GetS88EventName(int inIndex, string inNameText)
        {
            return ToAlphabet(inIndex) + ": " + inNameText;
        }

        private void UpdateS88Events()
        {
            int i;

            for (i = 0; i < Program.S88EVENT_MAX; i++)
            {
                lBox_S88Events.Items[i] = GetS88EventName(i, S88Manager.mEvents[i].EventName);

            }

        }

        private void button_S88EventClear_Click(object sender, EventArgs e)
        {

            int aIndex;

            /* 選択番号変更 */
            aIndex = lBox_S88Events.SelectedIndex;

            if ((aIndex < 0) || (aIndex >= Program.S88EVENT_MAX))
            {
                return;
            }

            if (MessageBox.Show(LangManager.SetText("TxtMsgBoxDeleteEvent","Do you want to delete this event?"), LangManager.SetText("TxtMsgBoxAttention", "Attention"), MessageBoxButtons.YesNo) != System.Windows.Forms.DialogResult.Yes)
            {
                return;
            }

            /* イベント初期化 */
            S88Manager.mEvents[aIndex].mSensorAddress = 0;
            S88Manager.mEvents[aIndex].mAvailable = false;
            S88Manager.mEvents[aIndex].EventName = "";
            S88Manager.mEvents[aIndex].mLocIndex = -1;
            S88Manager.mEvents[aIndex].mIntervalTime = 0;
            S88Manager.mEvents[aIndex].mTriggerTime = 0;
            S88Manager.mEvents[aIndex].TriggerType = 0;
            S88Manager.mEvents[aIndex].Items.Clear();

            cBox_S88EventAvailable.Checked = false;
            cBox_LocChangeEV.SelectedIndex = 0;
            tBox_S88EventName.Text = "";
            lView_S88Script.Items.Clear();
            cBox_S88StartTrigger.SelectedIndex = 0;

            /* 一覧も更新する */
            lBox_S88Events.Items[aIndex] = GetS88EventName(aIndex, S88Manager.mEvents[aIndex].EventName);


        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            /* 追加 */
            int aIndex;
            int aEventIndex;

            /* 選択番号変更 */
            aEventIndex = lBox_S88Events.SelectedIndex;

            if ((aEventIndex < 0) || (aEventIndex >= Program.S88EVENT_MAX))
            {
                return;
            }

            ToolStripMenuItem aItem = sender as ToolStripMenuItem;

            ScriptData aData = new ScriptData();

            aData.mParam2 = "";
            aData.mParam1 = "";

            switch (DSCommon.ParseStrToInt((string)aItem.Tag))
            {
                case 1: aData.mCommand = Program.SCRIPTCMD_SPEED;
                    break;
                case 2: aData.mCommand = Program.SCRIPTCMD_DIRECTION;
                    break;
                case 3: aData.mCommand = Program.SCRIPTCMD_FUNCTION;
                    break;
                case 4: aData.mCommand = Program.SCRIPTCMD_ACCESSORY;
                    break;
                case 5: aData.mCommand = Program.SCRIPTCMD_WAIT;
                    break;
                case 6: aData.mCommand = Program.SCRIPTCMD_POWER;
                    break;
                case 7: aData.mCommand = Program.SCRIPTCMD_EXIT;
                    break;
                case 8: aData.mCommand = Program.SCRIPTCMD_GOTO;
                    break;
                case 9: aData.mCommand = Program.SCRIPTCMD_JUMP;
                    break;
                case 10: aData.mCommand = Program.SCRIPTCMD_SETFLAG;
                    break;
                case 11: aData.mCommand = Program.SCRIPTCMD_LABEL;
                    break;
                case 12: aData.mCommand = Program.SCRIPTCMD_INCFLAG;
                    break;
                case 13: aData.mCommand = Program.SCRIPTCMD_RUNFILE;
                    break;
                case 14: aData.mCommand = Program.SCRIPTCMD_JUMPRUN;
                    break;
                case 15: aData.mCommand = Program.SCRIPTCMD_JUMPSTOP;
                    break;
                case 16: aData.mCommand = Program.SCRIPTCMD_JUMPROUTE;
                    break;
                case 17: aData.mCommand = Program.SCRIPTCMD_SETROUTE;
                    break;
                case 18: aData.mCommand = Program.SCRIPTCMD_JUMPS88;
                    break;
                case 19: aData.mCommand = Program.SCRIPTCMD_WAITRND;
                    break;
                case 20: aData.mCommand = Program.SCRIPTCMD_WAITIF;
                    break;
                case 21: aData.mCommand = Program.SCRIPTCMD_GOTOIF;
                    break;
            }

            aIndex = 0;

            if (lView_S88Script.SelectedItems.Count > 0)
            {
                aIndex = lView_S88Script.SelectedItems[0].Index;
                S88Manager.mEvents[aEventIndex].Items.Insert(aIndex + 1, aData);
                
                //編集用にインデックスを追加した位置にする
                aIndex = aIndex + 1;
            }
            else
            {
                S88Manager.mEvents[aEventIndex].Items.Add(aData);

                //編集用にインデックスを追加した位置にする
                aIndex = S88Manager.mEvents[aEventIndex].Items.Count - 1;
            }

            /* スクリプト登録 */
            UpdateS88ScriptDisplay();

            /* 編集画面を開く */
            if (OpenScriptEdit(S88Manager.mEvents[aEventIndex].Items[aIndex], false) == true)
            {
                /* スクリプト登録 */
                UpdateS88ScriptDisplay();
            }

        }

        private void toolStripMenuItem_EditS88Event_Click(object sender, EventArgs e)
        {
            int aIndex;
            int aEventIndex;

            /* 選択番号変更 */
            aEventIndex = lBox_S88Events.SelectedIndex;

            if ((aEventIndex < 0) || (aEventIndex >= Program.S88EVENT_MAX))
            {
                return;
            }

            if (lView_S88Script.SelectedItems.Count <= 0)
            {
                return;
            }

            aIndex = lView_S88Script.SelectedItems[0].Index;

            if (OpenScriptEdit(S88Manager.mEvents[aEventIndex].Items[aIndex], false) == true)
            {

                /* スクリプト登録 */
                UpdateS88ScriptDisplay();
            }
        }

        private void toolStripMenuItem_S88EventDel_Click(object sender, EventArgs e)
        {
            DeleteEventEditorItem();
        }

        private void DeleteEventEditorItem()
        {
            int i;
            int aDeleteItemCount;
            int aEventIndex;

            /* 選択番号変更 */
            aEventIndex = lBox_S88Events.SelectedIndex;

            if ((aEventIndex < 0) || (aEventIndex >= Program.S88EVENT_MAX))
            {
                return;
            }

            aDeleteItemCount = lView_S88Script.SelectedItems.Count;

            for (i = aDeleteItemCount - 1; i >= 0; i--)
            {
                S88Manager.mEvents[aEventIndex].Items.RemoveAt(lView_S88Script.SelectedItems[i].Index);
                lView_S88Script.Items.Remove(lView_S88Script.SelectedItems[i]);
            }

        }


        private void UpdateS88ScriptDisplay()
        {
            int i;
            int aIndex;
            string aParam1, aParam2,aParam3;

            aIndex = lBox_S88Events.SelectedIndex;

            if ((aIndex < 0) || (aIndex >= Program.S88EVENT_MAX))
            {
                return;
            }

            lView_S88Script.Items.Clear();

            for (i = 0; i < S88Manager.mEvents[aIndex].Items.Count; i++)
            {
                ListViewItem aItem = new ListViewItem();
                aItem.Text = i.ToString();

                /* パラメータ変換処理 */

                getScriptParamName(S88Manager.mEvents[aIndex].Items[i], out aParam1, out aParam2, out aParam3);

                aItem.SubItems.Add(S88Manager.mEvents[aIndex].Items[i].mCommand);
                aItem.SubItems.Add(aParam1);
                aItem.SubItems.Add(aParam2);
                aItem.SubItems.Add(aParam3);

                lView_S88Script.Items.Add(aItem);
            }

        }

        private void getScriptParamName(ScriptData inData, out string outParam1, out string outParam2, out string outParam3)
        {
            outParam1 = inData.mParam1;
            outParam2 = inData.mParam2;
            outParam3 = inData.mParam3.ToString();


            if (inData.mCommand == Program.SCRIPTCMD_POWER)
            {
                if (inData.mParam1 == "0")
                {
                    outParam1 = "POWER OFF";
                }
                else if (inData.mParam1 == "1")
                {
                    outParam1 = "POWER ON";
                }
                outParam2 = "";
                outParam3 = "";

            }
            else if (inData.mCommand == Program.SCRIPTCMD_SPEED)
            {
                if (inData.mParam1.Contains("SLOT.") == false)
                {
                    outParam1 = LocDB.GetProtcolAddressDescription(DSCommon.ParseStrToInt(inData.mParam1));
                }

                outParam3 = inData.mParam3.ToString() + " [0.1sec]";

            }
            else if (inData.mCommand == Program.SCRIPTCMD_ACCESSORY)
            {
                if (inData.mParam2 == "0")
                {
                    outParam2 = "RED,/";
                }
                else if (inData.mParam2 == "1")
                {
                    outParam2 = "GREEN,|";
                }
                outParam3 = "";
            }
            else if (inData.mCommand == Program.SCRIPTCMD_FUNCTION)
            {
                if (inData.mParam1 != null)
                {
                    if (inData.mParam1.Contains("SLOT.") == false)
                    {
                        outParam1 = LocDB.GetProtcolAddressDescription(DSCommon.ParseStrToInt(inData.mParam1));
                    }
                }
                else
                {
                    outParam2 = "AUTO";
                }

                outParam2 = "F" + (DSCommon.ParseStrToInt(inData.mParam2)).ToString();

                if (inData.mParam3 == 0)
                {
                    outParam3 = "OFF";
                }
                else if (inData.mParam3 == 1)
                {
                    outParam3 = "ON";
                }
            }
            else if (inData.mCommand == Program.SCRIPTCMD_DIRECTION)
            {
                if (inData.mParam1 != null)
                {
                    if (inData.mParam1.Contains("SLOT.") == false)
                    {
                        outParam1 = LocDB.GetProtcolAddressDescription(DSCommon.ParseStrToInt(inData.mParam1));
                    }
                }
                else
                {
                    outParam2 = "AUTO";
                }


                if (inData.mParam2 == "1")
                {
                    outParam2 = "FWD";
                }
                else if (inData.mParam2 == "2")
                {
                    outParam2 = "REV";
                }

                outParam3 = "";
            }
            else if (inData.mCommand == Program.SCRIPTCMD_EXIT)
            {
                outParam1 = "";
                outParam2 = "";
                outParam3 = "";
            }
            else if (inData.mCommand == Program.SCRIPTCMD_LABEL)
            {
                outParam2 = "";
                outParam3 = "";
            }
            else if (inData.mCommand == Program.SCRIPTCMD_RUNFILE)
            {
                outParam3 = "";
            }
            else if (inData.mCommand == Program.SCRIPTCMD_SETFLAG)
            {
                outParam1 = "Flag." + outParam1;
                outParam3 = "";
            }
            else if (inData.mCommand == Program.SCRIPTCMD_INCFLAG)
            {
                outParam1 = "Flag." + outParam1;
                outParam3 = "";
            }
            else if (inData.mCommand == Program.SCRIPTCMD_GOTO)
            {
                outParam1 = "Line." + outParam1;
                outParam2 = "";
                outParam3 = "";
            }
            else if (inData.mCommand == Program.SCRIPTCMD_GOTOIF)
            {
                outParam1 = "Line." + outParam1;
                outParam3 = "";
            }

            else if (inData.mCommand == Program.SCRIPTCMD_WAIT)
            {
                outParam1 = outParam1 + " [0.1sec]";
                outParam2 = "";
                outParam3 = "";
            }
            else if (inData.mCommand == Program.SCRIPTCMD_WAITRND)
            {
                outParam1 = outParam1 + " [0.1sec]";
                outParam2 = "";
                outParam3 = "";
            }
            else if (inData.mCommand == Program.SCRIPTCMD_WAITIF)
            {
                outParam1 = outParam1 + "";
                outParam2 = "";
                outParam3 = "";
            }
            else if (inData.mCommand == Program.SCRIPTCMD_JUMP)
            {
                outParam2 = "Flag." + outParam2;
            }
            else if (inData.mCommand == Program.SCRIPTCMD_JUMPS88)
            {
                outParam2 = "S88." + (DSCommon.ParseStrToInt( outParam2) + 1).ToString();
            }
            else if ((inData.mCommand == Program.SCRIPTCMD_JUMPRUN) || (inData.mCommand == Program.SCRIPTCMD_JUMPSTOP))
            {
                outParam2 = "Loc." + (DSCommon.ParseStrToInt(outParam2)).ToString();
                outParam3 = "Speed." + outParam3;
            }
            else if (inData.mCommand == Program.SCRIPTCMD_JUMPROUTE)
            {
                outParam2 = "Route." + (DSCommon.ParseStrToInt(outParam2)).ToString();
                outParam3 = (outParam3 == "0") ? "Opening" : "Closing";

            }
            else if (inData.mCommand == Program.SCRIPTCMD_SETROUTE)
            {
                outParam1 = "Route." + (DSCommon.ParseStrToInt(outParam1)).ToString();
                outParam2 = "";
                outParam3 = "";
            }
        }


        private void button_S88EventsImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog aForm = new OpenFileDialog();
            aForm.CheckFileExists = true;
            aForm.Filter = "S88 Event file(*.xml)|*.xml|All files(*.*)|*.*";

            //押されたボタン別の処理 
            if (aForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                S88Manager.LoadFromFile(aForm.FileName);

                /* 更新 */
                UpdateS88Events();
            }

            aForm.Dispose();
        }

        private void button_S88EventsExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog aForm = new SaveFileDialog();
            aForm.OverwritePrompt = true;
            aForm.Filter = "S88 Event file(*.xml)|*.xml|All files(*.*)|*.*";

            //押されたボタン別の処理 
            if (aForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                S88Manager.SaveToFile(aForm.FileName);
            }

            aForm.Dispose();
        }

        private void button_S88ScriptDown_Click(object sender, EventArgs e)
        {
            int aTag;
            int i;
            int aIndex;
            int aEventIndex;

            if (lView_S88Script.SelectedItems.Count <= 0)
            {
                return;
            }


            Button aButton = sender as Button;

            aTag = int.Parse(aButton.Tag.ToString());

            aEventIndex = lBox_S88Events.SelectedIndex;

            if ((aEventIndex < 0) || (aEventIndex >= Program.S88EVENT_MAX))
            {
                return;
            }


                switch (aTag)
                {
                    case 1:
                        for (i = 0; i < lView_S88Script.SelectedItems.Count; i++)
                        {
                            aIndex = lView_S88Script.SelectedItems[i].Index;

                            if (aIndex > 0)
                            {
                                /* 表示側 */
                                ListViewItem aItem = lView_S88Script.Items[aIndex].Clone() as ListViewItem;

                                lView_S88Script.Items.RemoveAt(aIndex);
                                aItem.Selected = true;
                                lView_S88Script.Items.Insert(aIndex - 1, aItem);

                                lView_S88Script.Items[aIndex - 1].Text = (aIndex - 1).ToString();
                                lView_S88Script.Items[aIndex].Text = aIndex.ToString();

                                /* データ側 */
                                ScriptData aData = new ScriptData();

                                aData.mCommand = S88Manager.mEvents[aEventIndex].Items[aIndex].mCommand;
                                aData.mParam1 = S88Manager.mEvents[aEventIndex].Items[aIndex].mParam1;
                                aData.mParam2 = S88Manager.mEvents[aEventIndex].Items[aIndex].mParam2;
                                aData.mParam3 = S88Manager.mEvents[aEventIndex].Items[aIndex].mParam3;
                                aData.mParam4 = S88Manager.mEvents[aEventIndex].Items[aIndex].mParam4;

                                S88Manager.mEvents[aEventIndex].Items.RemoveAt(aIndex);
                                S88Manager.mEvents[aEventIndex].Items.Insert(aIndex - 1, aData);
                        
                            }
                        }

                        break;

                    case 2:
                        for (i = lView_S88Script.SelectedItems.Count -1 ; i >= 0; i--)
                        {
                            aIndex = lView_S88Script.SelectedItems[i].Index;

                            if (aIndex >= 0 && (aIndex < (lView_S88Script.Items.Count - 1)))
                            {
                                /* 表示側 */
                                ListViewItem aItem = lView_S88Script.Items[aIndex].Clone() as ListViewItem;

                                lView_S88Script.Items.RemoveAt(aIndex);
                                lView_S88Script.Items.Insert(aIndex + 1, aItem);

                                lView_S88Script.Items[aIndex].Text = aIndex.ToString();
                                lView_S88Script.Items[aIndex + 1].Text = (aIndex + 1).ToString();
                                lView_S88Script.Items[aIndex + 1].Selected = true;


                                /* データ側 */
                                ScriptData aData = new ScriptData();

                                aData.mCommand = S88Manager.mEvents[aEventIndex].Items[aIndex].mCommand;
                                aData.mParam1 = S88Manager.mEvents[aEventIndex].Items[aIndex].mParam1;
                                aData.mParam2 = S88Manager.mEvents[aEventIndex].Items[aIndex].mParam2;
                                aData.mParam3 = S88Manager.mEvents[aEventIndex].Items[aIndex].mParam3;
                                aData.mParam4 = S88Manager.mEvents[aEventIndex].Items[aIndex].mParam4;

                                S88Manager.mEvents[aEventIndex].Items.RemoveAt(aIndex);
                                S88Manager.mEvents[aEventIndex].Items.Insert(aIndex + 1, aData);
                            }
                        }

                        break;
                }

        }

        private void timerSerialSend_Tick(object sender, EventArgs e)
        {
            int aIndex;

           //DEBUG  this.Text = SendingList.Count.ToString() + " / " + CheckRailuinoResponse.ToString();

            if ((CheckingList.Count > 0) && (gProcessingWebCmd == false))
            {
                gProcessingWebCmd = true;

                /* Webからの指令を表示系に反映する処理(スレッドセーフ回避のため） */
                SetRecvAnotherStationWeb(CheckingList[CheckingList.Count - 1]);
                /* 予約リストから削除 */
                CheckingList.RemoveAt(CheckingList.Count - 1);

                gProcessingWebCmd = false;

            }           

            /* スケジュールされていなければ完了 */
            if (SendingList.Count <= 0)
            {
                return;
            }

            /* 応答なしの監視 */
            if (Counter_Timeout > 3000)
            {
                //タイムアウトエラーにする
                CheckRailuinoResponse = Program.REPLY_TIMEOUT;
            }

            /* ゲートウェイ側のエラーの監視 */
            if (Counter_GatewayError > 10)
            {
                SetRecvText("[ERR] GATEWAY ERROR");
                SendingList.Clear();
                return;
            }

            /* 応答のチェック */
            if (CheckRailuinoResponse < 0)
            {
                Counter_Timeout = Counter_Timeout + 1;

                return;
            }

            /* 古いものから送信 */
            aIndex = SendingList.Count - 1;

            /* エラーで無い場合またはPING or S88(定期的に送るもの)のとき、前回送信したものを削除する */
            if ((CheckRailuinoResponse == Program.REPLY_OK) || (SendingList[aIndex] == Program.SERIALCMD_GETS88) || (SendingList[aIndex] == Program.SERIALCMD_PING))
            {
                /* 応答フラグを初期化 */
                CheckRailuinoResponse = Program.REPLY_READY;

                /* 予約リストから削除 */
                SendingList.RemoveAt(aIndex);

                //インデックスを更新
                aIndex = SendingList.Count - 1;

                /* 応答があるのでエラーカウンタ初期化 */
                Counter_GatewayError = 0;
            }

            //再チェック
            if (SendingList.Count <= 0)
            {
                return;
            }
            else if (CheckRailuinoResponse == Program.REPLY_TIMEOUT)
            {
                SetRecvText("[ERR] TIMEOUT");
                SendingList.Clear();
                return;
            }

            /* 送信する */
            if (IsOpenSendManager())
            {
                CheckRailuinoResponse = Program.REPLY_WAITING;
                Counter_Timeout = 0;
                Counter_ComErr  = 0;

                SendToGateway(SendingList[aIndex]);
                

                SetRecvText("[SEND] " + SendingList[aIndex]);
            }
            else
            {
                if (gControlMode == Program.POWER_ON)
                {
                    if (Counter_ComErr <= 5)
                    {
                        SetRecvText("[ERR] Error of serial port occured. Cannot send command.");
                    }
                    Counter_ComErr++;

                }
                else
                {


                }
            }
        }

        private bool IsOpenSendManager()
        {
            bool aReturn = false;

            switch (gAppSettings.mSendMode)
            {
            case    0:
                    /* シリアルポート */
                    aReturn = serialPort.IsOpen;
                    break;
            case    1:
                    /* HTTP Post送信 */
                    aReturn = true;
                    break;
            case    2:
                default:
                    /* エミュレーション */
                    aReturn = true;
                    break;

            }

            return aReturn;

        }

        private void SendToGateway(string inCommand)
        {
            string aReplyText = "";

            switch (gAppSettings.mSendMode)
            {
            case    0:
                    /* シリアルポート */
                    serialPort.WriteLine(inCommand);
                    break;
            case    1:
                    /* HTTP Post送信 */
                    WebControl.SendWithPost(gAppSettings.mIPAddress, inCommand);
                    break;
            case    2:
                    /* エミュレーション */
                    aReplyText = "200 Ok";
                    break;
            }

            /* 応答に合わせて処理を行う。シリアルポートは別。 */
            if (aReplyText != "")
            {
                SetRecvText("[RECV]" + aReplyText);
                SetRecvConnectCheck(aReplyText);

                /* 外部コマンドステーションの応答を取得 */
                this.BeginInvoke(new SetRecvAnotherStation_delegate(SetRecvAnotherStation), new object[] { aReplyText });

                /* S88受信データを保存 */
                SetRecvS88Datas(aReplyText);
            }

        }

        private void HttpUploadCompleted(object sender,	System.Net.DownloadStringCompletedEventArgs e)
        {

            try
            {

                string aReplyText = (string)e.Result;

                /* タグ除去 */
                int aIdx1 = aReplyText.IndexOf("<!--");
                int aIdx2 = aReplyText.IndexOf("-->");

                if (aIdx2 - aIdx1 > 0)
                {
                    string aReplyData = aReplyText.Substring(aIdx1 + 4, aIdx2 - aIdx1 - 4);

                    SetRecvText("[RECV]" + aReplyData);
                    SetRecvConnectCheck(aReplyData);

                    /* 外部コマンドステーションの応答を取得 */
                    this.BeginInvoke(new SetRecvAnotherStation_delegate(SetRecvAnotherStation), new object[] { aReplyData });

                    /* S88受信データを保存 */
                    SetRecvS88Datas(aReplyData);
                }
                else
                {
                    return;
                }

            }
            catch (Exception)
            {
                SetRecvText("[ERR] Http exception : " + e.Error.Message);

            }

        }

        private void updateS88ScriptView()
        {
            int i;
            string aText;

            /* Flag 表示 */

            aText = "";

            for (i = 0; i < Program.SCRIPTVALUE_MAX; i++)
            {
                aText = aText + "No." + i.ToString() + " : " + S88Manager.GetScriptValues(i).ToString() + "\r\n";
            }

            label_S88ScrInfoFlagView.Text = aText;

            /* Run check 表示 */

            aText = "";

            for (i = 0; i < Program.S88EVENT_MAX; i++)
            {

                if (S88Manager.mEvents[i].EventName == "")
                {
                    aText = aText + "Event " + ToAlphabet(i) + ": " + (S88Manager.mEvents[i].Run == true ? "ON" : "OFF") + "\r\n";
                }
                else
                {
                    aText = aText + "(" + ToAlphabet(i) + ")" + S88Manager.mEvents[i].EventName + ": " + (S88Manager.mEvents[i].Run == true ? "ON":"OFF" ) + "\r\n";
                }
            }

            label_S88ScrInfoRunChk.Text = aText;

        }


        private void button_S88ScriptViewUpdate_Click(object sender, EventArgs e)
        {
            updateS88ScriptView();

        }

        private void tabPage10_Paint(object sender, PaintEventArgs e)
        {
            updateS88ScriptView();
        }

        private void cBox_S88StartTrigger_SelectedIndexChanged(object sender, EventArgs e)
        {
            int aIndex;

            /* 選択番号変更 */
            aIndex = lBox_S88Events.SelectedIndex;

            if ((aIndex < 0) || (aIndex >= Program.S88EVENT_MAX))
            {
                return;
            }

            /* トリガの種類切り替え */
            S88Manager.mEvents[aIndex].TriggerType = cBox_S88StartTrigger.SelectedIndex;

            UpdateS88ScriptTriggerOption();

        }

        private void MeterBox_MouseDown(object sender, MouseEventArgs e)
        {


            /* メーターをマウス操作できるようにする */

            int aDialValue = 0;
            int cx, cy;
            int ax, ay;
            int rx, ry;
            float aTheta;
            int aTheta_r;
            double aR;

            
            if ((gControlMode == Program.POWER_OFF) || ((e.Button != System.Windows.Forms.MouseButtons.Left)))
            {
                return;
            }

            ax = e.X;
            ay = e.Y;

            cx = MeterBox.Width / 2;
            cy = MeterBox.Height / 2;


            rx = ax - cx;
            ry = ay - cy;

            aR = Math.Sqrt(rx * rx + ry * ry);

            if ((aR < 10) || (aR > (cx + 10)))
            {
                /* 円の内側および外側は無視する */
                return;
            }

            aTheta = (float)Math.Atan2(ry, rx) + (float)Math.PI;

            aTheta_r = (int)(aTheta * 180 / Math.PI);

            /* 10deg以上ずれている場合は無視する処理とする */
            if ((aTheta_r <= 320) && (aTheta_r > 220))
            {
                return;
            }

            /* 許容範囲の調整 */
            if ((aTheta_r <= 330) && (aTheta_r >= 320))
            {
                aTheta_r = 330;
            }

            if ((aTheta_r >= 210) && (aTheta_r <= 220))
            {
                aTheta_r = 210;
            }

            /* 正規化 */
            if (aTheta_r > 300)
            {
                aTheta_r = aTheta_r - 360;

            }

            /* 角度から速度値に換算（精度を32dずつにわざと落として処理軽量化） */
            aDialValue = ((aTheta_r + 30) * LocDB.Items[SelectedLocIndex].mLocMaxSpeed / 240);


            if (aDialValue != LocDB.Items[SelectedLocIndex].mCurrentSpeed)
            { 

                /* 速度のセット（タイマーで自動で反映される） */
                LocDB.Items[SelectedLocIndex].mCurrentSpeed = aDialValue;
                LocDB.Items[SelectedLocIndex].SetUpdateNextInterval();

                /* レバー更新 */
                UpdatePowerLever(0);

                /* メーター更新 */
                MeterBox.Refresh();

            }

        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /* S88のコピーアンドペースト */

            int i;
            int aEventIndex;
            int aSelectedIndex = 0;
            string aText = "";

            ToolStripMenuItem aItem = sender as ToolStripMenuItem;

            /* 選択番号変更 */
            aEventIndex = lBox_S88Events.SelectedIndex;

            if ((aEventIndex < 0) || (aEventIndex >= Program.S88EVENT_MAX))
            {
                return;
            }


            if (int.Parse(aItem.Tag as string) == 1)
            {
                aText = "DESKTOPSTATION,SCRIPT,2013\r\n";

                /* インデックスを取得する */
                if (lView_S88Script.SelectedItems.Count > 0)
                {

                    for (i = 0; i < lView_S88Script.SelectedItems.Count; i++)
                    {
                        aSelectedIndex = lView_S88Script.SelectedItems[i].Index;

                        aText = aText + S88Manager.mEvents[aEventIndex].Items[aSelectedIndex].mCommand + "," + S88Manager.mEvents[aEventIndex].Items[aSelectedIndex].mParam1 + "," + S88Manager.mEvents[aEventIndex].Items[aSelectedIndex].mParam2 + "," + S88Manager.mEvents[aEventIndex].Items[aSelectedIndex].mParam3.ToString() + "\r\n";
                    }                
                }
                else
                {
                    /* 無選択の場合は全てコピー */
                    aSelectedIndex = 0;
                    
                    for (i = aSelectedIndex; i < S88Manager.mEvents[aEventIndex].Items.Count; i++)
                    {
                        aText = aText + S88Manager.mEvents[aEventIndex].Items[i].mCommand + "," + S88Manager.mEvents[aEventIndex].Items[i].mParam1 + "," + S88Manager.mEvents[aEventIndex].Items[i].mParam2 + "," + S88Manager.mEvents[aEventIndex].Items[i].mParam3.ToString() + "\r\n";
                    }
                
                }



                /* クリップボードへ転送 */
                Clipboard.SetDataObject(aText);

            }
            else if (int.Parse(aItem.Tag as string) == 2)
            {

                /* クリップボードからデータ取得 */
                IDataObject data = Clipboard.GetDataObject();

                /* 種類がテキスト以外は何もしない */
                if (data.GetDataPresent(DataFormats.Text))
                {
                    aText = (string)data.GetData(DataFormats.Text);
                }
                else
                {
                    return;
                }

                /* データが無いときは終了 */
                if ((aText == "") || (aText.IndexOf("DESKTOPSTATION,SCRIPT,2013") < 0))
                {
                    return;
                }

                /* インデックスを取得する */
                if (lView_S88Script.SelectedItems.Count > 0)
                {
                    aSelectedIndex = lView_S88Script.SelectedItems[0].Index;
                }
                else
                {
                    aSelectedIndex = 0;
                }

                /* 書き込む */
                string[] aTextArray = aText.Split(new[] {"\r\n"}, StringSplitOptions.None);

                for( i = 1; i < aTextArray.Length; i++)
                {
                    string[] aParamArray = aTextArray[i].Split(',');

                    if (aParamArray.Length >= 2)
                    {

                        ScriptData aNewItem = new ScriptData();

                        aNewItem.mCommand = aParamArray[0];

                        if (aParamArray.Length > 1)
                        {
                            aNewItem.mParam1 = aParamArray[1];
                        }

                        if (aParamArray.Length > 2)
                        {
                            aNewItem.mParam2 = aParamArray[2];
                        }

                        if (aParamArray.Length > 3)
                        {
                            aNewItem.mParam3 = DSCommon.ParseStrToInt(aParamArray[3]);
                        }

                        if (aParamArray.Length > 4)
                        {
                            aNewItem.mParam4 = DSCommon.ParseStrToInt(aParamArray[4]);
                        }

                        if (aSelectedIndex + i < S88Manager.mEvents[aEventIndex].Items.Count)
                        {
                            S88Manager.mEvents[aEventIndex].Items.Insert(aSelectedIndex + i, aNewItem);
                        }
                        else
                        {
                            S88Manager.mEvents[aEventIndex].Items.Add(aNewItem);
                        }

                    }

                }

                UpdateS88ScriptDisplay();

            }

        }

        private void contextMenu_S88Event_Opening(object sender, CancelEventArgs e)
        {
            MenuItemS88_Paste.Enabled = Clipboard.ContainsText();
        }

        private void button_CopySerialConsole_Click(object sender, EventArgs e)
        {
            /* クリップボードにコピー */
            int i;
            string aText = "";

            for (i = 0; i < listBox_Serial.Items.Count; i++)
            {
                aText = aText + listBox_Serial.Items[i];
            }
            

            /* クリップボードへ転送 */
            Clipboard.SetDataObject(aText);


        }

        private void SButton_VerInfo_Click(object sender, EventArgs e)
        {
            int i;

            VersionForm aForm = new VersionForm();

            aForm.SetFormLanguage(LangManager);

            for (i = 0; i < TBoxManager.Items.Count; i++)
            {
                ListViewItem aItem = new ListViewItem();
                aItem.Text = TBoxManager.Items[i].mTextTrackBoxUID;
                aItem.SubItems.Add( TBoxManager.Items[i].mTextTypes);
                aItem.SubItems.Add( TBoxManager.Items[i].mTextVersion);

                aForm.lView_TrackBox.Items.Add(aItem);

            }

            aForm.ShowDialog(this);

        }

        private void buttonMfxRecognization_Click(object sender, EventArgs e)
        {

        }

        private void pBox_Clock_Paint(object sender, PaintEventArgs e)
        {
            DateTime aTime;

            if (gAppSettings.mSideFuncRight == Program.RIGHTBAR_NONE)
            {
                return;
            }

            /* 描画 */
            Graphics aCanvas = e.Graphics;

            //(アンチエイリアス処理されたレタリング)を指定する
            aCanvas.SmoothingMode = SmoothingMode.AntiAlias;
            aCanvas.TextRenderingHint = TextRenderingHint.AntiAlias;

            switch (gAppSettings.mSideFuncRight)
            {
                    /* Clock */
                case Program.RIGHTBAR_CLOCK:

                    aCanvas.DrawImage(Properties.Resources.Clock, 0, 0, Properties.Resources.Clock.Width * ScaleRation / 100, Properties.Resources.Clock.Height * ScaleRation / 100);


                    /* 時間の決定 */
                    if (gAppSettings.mUseVirtualClock == true)
                    {
                        aTime = gAppSettings.mVirtualClock + (DateTime.Now - timeBoot);

                    }
                    else
                    {
                        aTime = DateTime.Now;
                    }

                    /* 時計の描画 */
                    MeterDrawer.DrawClockBox(aCanvas, aTime, ScaleRation);

                    break;

                    /* 非常停止ボタン */
                case Program.RIGHTBAR_EMG:

                    aCanvas.DrawImage(Properties.Resources.EmergencyButton, 0, 0, Properties.Resources.EmergencyButton.Width * ScaleRation / 100, Properties.Resources.EmergencyButton.Height * ScaleRation / 100);

                    string aEmgText = LangManager.SetText("TxtEmergency", "EMERGENCY STOP");
                    Font aFont = new Font("Arial", 14, FontStyle.Bold);

                    float aX = (pBox_Clock.Width - aCanvas.MeasureString(aEmgText, aFont).Width) / 2;

                    /* サイズオーバー補正 */
                    if (aX < 0)
                    {
                        aFont.Dispose();
                        aFont = new Font("Arial", 11, FontStyle.Bold);
                        aX = (pBox_Clock.Width - aCanvas.MeasureString(aEmgText, aFont).Width) / 2;

                        if (aX < 0)
                        {
                            aFont.Dispose();
                            aFont = new Font("Arial", 8, FontStyle.Bold);
                            aX = (pBox_Clock.Width - aCanvas.MeasureString(aEmgText, aFont).Width) / 2;

                        }

                    }

                    /* テキストを描画 */
                    aCanvas.DrawString(aEmgText, aFont, Brushes.Black, aX, 125 * ScaleRation / 100);

                    aFont.Dispose();

                    break;
            }
        }

        private void timerClock_Tick(object sender, EventArgs e)
        {
            /* 時計の更新 */
            pBox_Clock.Refresh();

            pBox_EventClock.Refresh();


            /* 電源オフ時は動かさない */
            if (gControlMode == Program.POWER_ON)
            {
                //ルートのチェック処理を動かす
                Routes.RoutesList.CheckRouteAll(true);
            }

        }

        private void pBox_Clock_Click(object sender, EventArgs e)
        {
            switch (gAppSettings.mSideFuncRight)
            {
                /* Clock */
                case Program.RIGHTBAR_CLOCK:
                    openClockConfigForm();

                    break;

                /* 非常停止ボタン */
                case Program.RIGHTBAR_EMG:

                    /* 非常停止処理 */
                    runEmergencyFunction();

                    /* スピードレバーを0の位置にする */
                    UpdatePowerLever(0);

                    /* メーター更新 */
                    MeterBox.Refresh();

                    /* レバー更新 */
                    LeverBox.Refresh();                    

                    break;
            }
        }

        private void runEmergencyFunction()
        {

            if (gControlMode == Program.POWER_OFF)
            {
                return;
            }

            /* 命令スタックリストを初期化 */
            SendingList.Clear();
            CheckRailuinoResponse = 2;

            /* 走行中の機関車を停止 */
            StopAllLocs(true);

        }

        private void StopAllLocs(bool inStopRunningOnly)
        {

            for (int i = 0; i < cBox_LocChange.Items.Count; i++)
            {
                /* 走行している場合 */
                if ((LocDB.Items[i].mCurrentSpeed > 0) || (inStopRunningOnly == false))
                {
                    /* 指定列車を緊急停止 */
                    ResetSpeedOnLocPanel(i);
                }
            }
        }

        private void openClockConfigForm()
        {

            /* 時計設定フォームを開く */
            ConfigClock aForm = new ConfigClock();

            aForm.numUpDown_hour.Value = gAppSettings.mVirtualClock.Hour;
            aForm.numUpDown_min.Value = gAppSettings.mVirtualClock.Minute;
            aForm.numUpDown_sec.Value = gAppSettings.mVirtualClock.Second;
            aForm.cBox_UseUserClock.Checked = gAppSettings.mUseVirtualClock;

            aForm.SetFormLanguage(LangManager);

            aForm.ShowDialog(this);

            //押されたボタン別の処理 
            if (aForm.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                /* 時刻の算出 */
                DateTime aTime = new DateTime(2013, 1, 1, decimal.ToInt32(aForm.numUpDown_hour.Value), decimal.ToInt32(aForm.numUpDown_min.Value), decimal.ToInt32(aForm.numUpDown_sec.Value));

                gAppSettings.mVirtualClock = aTime;
                gAppSettings.mUseVirtualClock = aForm.cBox_UseUserClock.Checked;

                /* 経過時刻を更新 */
                timeBoot = DateTime.Now;

                /* 時計更新 */
                pBox_Clock.Refresh();
            }

            aForm.Dispose();

        }

        private void button_SeqScriptDown_Click(object sender, EventArgs e)
        {
            /* Up/down */
            int aTag;
            int i;
            int aIndex;

            if (listScript.SelectedItems.Count <= 0)
            {
                return;
            }


            Button aButton = sender as Button;

            aTag = int.Parse(aButton.Tag.ToString());

            switch (aTag)
            {
                case 1:
                    for (i = 0; i < listScript.SelectedItems.Count; i++)
                    {
                        aIndex = listScript.SelectedItems[i].Index;

                        if (aIndex > 0)
                        {
                            /* 表示側 */
                            ListViewItem aItem = listScript.Items[aIndex].Clone() as ListViewItem;

                            listScript.Items.RemoveAt(aIndex);
                            aItem.Selected = true;
                            listScript.Items.Insert(aIndex - 1, aItem);

                            listScript.Items[aIndex - 1].Text = (aIndex - 1).ToString();
                            listScript.Items[aIndex].Text = aIndex.ToString();

                            /* データ側 */
                            ScriptData aData = new ScriptData();

                            aData.mCommand = ScriptList[aIndex].mCommand;
                            aData.mParam1 = ScriptList[aIndex].mParam1;
                            aData.mParam2 = ScriptList[aIndex].mParam2;
                            aData.mParam3 = ScriptList[aIndex].mParam3;
                            aData.mParam4 = ScriptList[aIndex].mParam4;

                            ScriptList.RemoveAt(aIndex);
                            ScriptList.Insert(aIndex - 1, aData);

                        }
                    }

                    break;

                case 2:
                    for (i = listScript.SelectedItems.Count - 1; i >= 0; i--)
                    {
                        aIndex = listScript.SelectedItems[i].Index;

                        if (aIndex >= 0 && (aIndex < (listScript.Items.Count - 1)))
                        {
                            /* 表示側 */
                            ListViewItem aItem = listScript.Items[aIndex].Clone() as ListViewItem;

                            listScript.Items.RemoveAt(aIndex);
                            listScript.Items.Insert(aIndex + 1, aItem);

                            listScript.Items[aIndex].Text = aIndex.ToString();
                            listScript.Items[aIndex + 1].Text = (aIndex + 1).ToString();
                            listScript.Items[aIndex + 1].Selected = true;


                            /* データ側 */
                            ScriptData aData = new ScriptData();

                            aData.mCommand = ScriptList[aIndex].mCommand;
                            aData.mParam1 = ScriptList[aIndex].mParam1;
                            aData.mParam2 = ScriptList[aIndex].mParam2;
                            aData.mParam3 = ScriptList[aIndex].mParam3;
                            aData.mParam4 = ScriptList[aIndex].mParam4;

                            ScriptList.RemoveAt(aIndex);
                            ScriptList.Insert(aIndex + 1, aData);
                        }
                    }

                    break;
            }

        }

        private void toolStripMenuItem12_Click(object sender, EventArgs e)
        {
            /* シーケンススクリプトのコピーアンドペースト */

            int i;
            int aSelectedIndex = 0;
            string aText = "";

            ToolStripMenuItem aItem = sender as ToolStripMenuItem;

            if (int.Parse(aItem.Tag as string) == 1)
            {
                aText = "DESKTOPSTATION,SCRIPT,2013\r\n";

                for (i = 0; i < ScriptList.Count; i++)
                {
                    aText = aText + ScriptList[i].mCommand + "," + ScriptList[i].mParam1 + "," + ScriptList[i].mParam2 + "," + ScriptList[i].mParam3.ToString() + "\r\n";
                }

                /* クリップボードへ転送 */
                Clipboard.SetDataObject(aText);

            }
            else if (int.Parse(aItem.Tag as string) == 2)
            {

                /* クリップボードからデータ取得 */
                IDataObject data = Clipboard.GetDataObject();

                /* 種類がテキスト以外は何もしない */
                if (data.GetDataPresent(DataFormats.Text))
                {
                    aText = (string)data.GetData(DataFormats.Text);
                }
                else
                {
                    return;
                }

                /* データが無いときは終了 */
                if ((aText == "") || (aText.IndexOf("DESKTOPSTATION,SCRIPT,2013") < 0))
                {
                    return;
                }

                /* インデックスを取得する */
                if (listScript.SelectedItems.Count > 0)
                {
                    aSelectedIndex = listScript.SelectedItems[0].Index;
                }
                else
                {
                    aSelectedIndex = 0;
                }

                /* 書き込む */
                string[] aTextArray = aText.Split(new[] { "\r\n" }, StringSplitOptions.None);

                for (i = 1; i < aTextArray.Length; i++)
                {
                    string[] aParamArray = aTextArray[i].Split(',');

                    if (aParamArray.Length >= 2)
                    {

                        ScriptData aNewItem = new ScriptData();

                        aNewItem.mCommand = aParamArray[0];

                        if (aParamArray.Length > 1)
                        {
                            aNewItem.mParam1 = aParamArray[1];
                        }

                        if (aParamArray.Length > 2)
                        {
                            aNewItem.mParam2 = aParamArray[2];
                        }

                        if (aParamArray.Length > 3)
                        {
                            aNewItem.mParam3 = DSCommon.ParseStrToInt(aParamArray[3]);
                        }

                        if (aParamArray.Length > 4)
                        {
                            aNewItem.mParam4 = DSCommon.ParseStrToInt(aParamArray[4]);
                        }

                        /* 登録が無いときとあるときで処理が変わる */
                        if (aSelectedIndex + i < ScriptList.Count)
                        {
                            ScriptList.Insert(aSelectedIndex + i, aNewItem);
                        }
                        else
                        {
                            ScriptList.Add(aNewItem);
                        }
                    }

                }

                UpdateScriptDisplay();

            }
        }

        private void button_S88ConsoleClear_Click(object sender, EventArgs e)
        {
            /* S88実行ログのリスト初期化 */
            listBox_S88Console.Items.Clear();
        }

        private void cBox_LocChange_DrawItem(object sender, DrawItemEventArgs e)
        {
            /* 描画アイテム */

            e.DrawBackground();

            if( (e.Index < 0) || (LocDB.Items.Count <= 0))
            {
                return;
            }

            MeterDrawer.DrawLocLabel(e.Graphics, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height, e.Index, Brushes.Black, LocDB, "", ScaleRation);

            /* 選択枠描画 */
            e.DrawFocusRectangle();
        }

        private void pBox_MLocName1_Paint(object sender, PaintEventArgs e)
        {
            PictureBox aPBox;
            int aIndex = 0;
            int aTag;
            
            aPBox = sender as PictureBox;
            aTag = Convert.ToInt32(aPBox.Tag);
            aIndex = gAppSettings.mLocCtrlList[aTag] - 1;

            if (aTag == CurrentSelectedMultiLocIndex)
            {
                MeterDrawer.DrawLocLabel(e.Graphics, 0, 0, aPBox.Width, aPBox.Height, aIndex, Brushes.Red, LocDB, "", ScaleRation);
            }
            else
            {
                MeterDrawer.DrawLocLabel(e.Graphics, 0, 0, aPBox.Width, aPBox.Height, aIndex, Brushes.Black, LocDB, "", ScaleRation);
            }

            
        }

        private void MultiFunctionBox_Paint(object sender, PaintEventArgs e)
        {
            /* キャンバスを取得 */
            Graphics aCanvas = e.Graphics;

            /* アンチエイリアス有効 */
            aCanvas.SmoothingMode = SmoothingMode.AntiAlias;
            aCanvas.TextRenderingHint = TextRenderingHint.AntiAlias;

            /* 背景の描画 */
            DrawIconRoundRectangle(aCanvas, 0, 0, MultiFunctionBox.Width - 1, MultiFunctionBox.Height - 1, 5, true, Color.FromArgb(240, 240, 240), Color.Silver, Color.DimGray);

            /* 文字系 */
            System.Drawing.Font aDrawFont = new System.Drawing.Font("Arial", 12, FontStyle.Bold);

            if ((gControlMode == Program.POWER_OFF) || (gAppSettings.mSideFuncBottom == Program.BOTTOMBAR_NONE))
            {
                string aWelcomeText = LangManager.SetText("TxtMainWelcome", "Welcome aboard!");

                aCanvas.DrawString(aWelcomeText, aDrawFont, Brushes.Black, (MultiFunctionBox.Width - aCanvas.MeasureString(aWelcomeText, aDrawFont).Width) / 2, (MultiFunctionBox.Height - aCanvas.MeasureString(aWelcomeText, aDrawFont).Height) / 2);

                return;
            }


            switch (gAppSettings.mSideFuncBottom)
            {
                /* Accessories */
                case Program.BOTTOMBAR_ACC:
                    DrawBottomBarAccessories(aCanvas, ScaleRation);
                    break;

                /* S88 */
                case Program.BOTTOMBAR_S88:
                    MeterDrawer.DrawBottomoBarS88Sensors(aCanvas, S88Flags, SelectedCabS88Index, ScaleRation);
                    break;

                default:
                    break;
            }

            aDrawFont.Dispose();
        }

        private void DrawBottomBarAccessories(Graphics inCanvas, int inScaleRatio)
        {
            int i;
            int aTopIndex;
            int aImageNo;
            int ax, ay;
            int aCurrentIndex;
            int aCurrentImageIndex;

            /* 文字系 */
            System.Drawing.Font aDrawFont = new System.Drawing.Font("Arial", 12 * inScaleRatio / 100, FontStyle.Bold);

            /* 矢印ボタンを描画 */

            aTopIndex = SelectedCabAccIndex * 8;

            for (i = 0; i < 8; i++)
            {
                aCurrentIndex = i + aTopIndex;

                if (getAccListDoubleType(aCurrentIndex) == true)
                {
                    //複数のアドレスを使用するアクセサリ
                    aImageNo = gAppSettings.mAccList[aCurrentIndex].mType * 10 + gAppSettings.mAccList[aCurrentIndex].mDirection + gAppSettings.mAccList[aCurrentIndex + 1].mDirection * 2;
                }
                else
                {
                    //複数のアドレスを使用しないアクセサリ
                    aImageNo = gAppSettings.mAccList[aCurrentIndex].mType * 10 + gAppSettings.mAccList[aCurrentIndex].mDirection;
                }

                aCurrentImageIndex = GetAccTileImageIndex(aImageNo);

                /* 位置演算 */
                ax = i * Program.MULTIFNC_ACC_WIDTH + Program.MULTIFNC_ACC_LEFT;
                ay = Program.MULTIFNC_ACC_TOP;


                /* 一つ前が２重のアドレス利用の場合は描画しない */
                if (getAccListDoubleType(aCurrentIndex - 1) == false)
                {
                    /* 非表示以外は描画 */
                    if (gAppSettings.mAccList[aCurrentIndex].mInvisible == false)
                    {
                        int aIx = ax + (Program.MULTIFNC_ACC_WIDTH - Program.MULTIFNC_ACC_ITEMWIDTH) / 2;
                        int aIy = ay;

                        float aTx = ax * inScaleRatio / 100 + (Program.MULTIFNC_ACC_WIDTH * inScaleRatio / 100 - inCanvas.MeasureString((aCurrentIndex + 1).ToString(), aDrawFont).Width) / 2;
                        float aTy = ay + Program.MULTIFNC_ACC_ITEMHEIGHT;

                        /* 描画 */
                        inCanvas.DrawImage(AccessoryImageList.Images[aCurrentImageIndex], aIx * inScaleRatio / 100, aIy * inScaleRatio / 100, Program.MULTIFNC_ACC_ITEMWIDTH * inScaleRatio / 100, Program.MULTIFNC_ACC_ITEMHEIGHT * inScaleRatio / 100);

                        inCanvas.DrawString((aCurrentIndex + 1).ToString(), aDrawFont, Brushes.Black, aTx, aTy * inScaleRatio / 100);
                    }

                }

            }

            aDrawFont.Dispose();

        }

        private void MultiFunctionBox_MouseDown(object sender, MouseEventArgs e)
        {

            int ax, ay;
            int aIndex;
            int aTopIndex;
            bool aRefresh = false;

            int aAccLeft = Program.MULTIFNC_ACC_LEFT * ScaleRation / 100;
            int aAccTop = Program.MULTIFNC_ACC_TOP * ScaleRation / 100;
            int aAccWidth = Program.MULTIFNC_ACC_WIDTH * ScaleRation / 100;
            int aAccHeight = Program.ACCESSORIES_ITEMHEIGHT * ScaleRation / 100;
            int aAccIdxHeight = Program.ACCESSORIES_IDXHEIGHT;
            int aAccIdxWidth = Program.ACCESSORIES_IDXWIDTH;



            if ((gControlMode == Program.POWER_OFF) || (gAppSettings.mSideFuncBottom != Program.BOTTOMBAR_ACC))
            {
                return;
            }

            if ((e.X < aAccLeft) || (e.Y < aAccTop))
            {
                return;
            }

            ax = (e.X - aAccLeft) / aAccWidth;
            ay = (e.Y - aAccTop) / aAccHeight;

            /* 最大値チェック */
            if ((ax >= 8) || (ax < 0) || (ay > aAccIdxHeight) || (ay < 0))
            {
                return;
            }

            /* 順番を算出する */
            aTopIndex = SelectedCabAccIndex * aAccIdxWidth;
            aIndex = ax + aTopIndex;

            /* 一つ前がアドレスを二つ使うタイプの場合 */
            if (getAccListDoubleType(aIndex - 1) == true)
            {
                return;
            }

            if (e.Button == MouseButtons.Left)
            {
                /* 非表示になっていなければ処理 */
                if (gAppSettings.mAccList[aIndex].mInvisible == false)
                {
                    SetAccCommand(aIndex);

                    aRefresh = true;
                }
            }

            if (aRefresh == true)
            {
                MultiFunctionBox.Refresh();

            }

        }

        private void SetAccCommand(int inIndex)
        {

            if (getAccListDoubleType(inIndex) == true)
            {
                switch (gAppSettings.mAccList[inIndex].mDirection + gAppSettings.mAccList[inIndex + 1].mDirection * 2)
                {
                    case 0:
                        gAppSettings.mAccList[inIndex].mDirection = 1;
                        gAppSettings.mAccList[inIndex + 1].mDirection = 0;
                        break;
                    case 1:
                        gAppSettings.mAccList[inIndex].mDirection = 0;
                        gAppSettings.mAccList[inIndex + 1].mDirection = 1;
                        break;
                    case 2:
                        if (getAccListDoubleType_3max(inIndex) == true)
                        {
                            /* 3種類しかない場合は、4つめに行かせない */
                            gAppSettings.mAccList[inIndex].mDirection = 0;
                            gAppSettings.mAccList[inIndex + 1].mDirection = 0;
                        }
                        else
                        {
                            gAppSettings.mAccList[inIndex].mDirection = 1;
                            gAppSettings.mAccList[inIndex + 1].mDirection = 1;
                        }
                        break;
                    case 3:
                        gAppSettings.mAccList[inIndex].mDirection = 0;
                        gAppSettings.mAccList[inIndex + 1].mDirection = 0;
                        break;
                }
                SerialCmd.SetTurnout(inIndex + 1, gAppSettings.mAccList[inIndex].GetAccDirection());
                SerialCmd.SetTurnout(inIndex + 2, gAppSettings.mAccList[inIndex + 1].GetAccDirection());
            }
            else
            {
                //信号を逆にする（RED⇔GREEN)
                gAppSettings.mAccList[inIndex].ReverserDirection();
                //シリアルで出力
                SerialCmd.SetTurnout(inIndex + 1, gAppSettings.mAccList[inIndex].GetAccDirection());
            }
        }


        private void button_MultiFunc0_Click(object sender, EventArgs e)
        {

            Button aButton = sender as Button;
            int aTag;
            int aTempIndex;
            int aMax;

            aTag = int.Parse(aButton.Tag.ToString());

            if (gAppSettings.mSideFuncBottom == Program.BOTTOMBAR_NONE)
            {
                return;
            }

            switch (gAppSettings.mSideFuncBottom)
            {
                case Program.BOTTOMBAR_ACC:
                    aMax = 40;
                    aTempIndex = SelectedCabAccIndex;
                    break;
                case Program.BOTTOMBAR_S88:
                    aMax = 16;
                    aTempIndex = SelectedCabS88Index;
                    break;
                default:
                    aMax = 40;
                    aTempIndex = SelectedCabAccIndex;
                    break;
            }



            if (aTag == 0)
            {
                aTempIndex = aTempIndex - 1;
            }
            else
            {
                aTempIndex = aTempIndex + 1;
            }

            if (aTempIndex < 0)
            {
                aTempIndex = aMax - 1;
            }
            else if (aTempIndex >= aMax)
            {
                aTempIndex = 0;
            }
            else
            {
            }

            switch (gAppSettings.mSideFuncBottom)
            {
                case Program.BOTTOMBAR_ACC:
                    SelectedCabAccIndex = aTempIndex;
                    break;
                case Program.BOTTOMBAR_S88:
                    SelectedCabS88Index = aTempIndex;
                    break;
            }
            

            MultiFunctionBox.Refresh();


        }

        private void pBox_DirectBox0_Paint(object sender, PaintEventArgs e)
        {
            /* 進行方向表示・変更ボタン（複数機関車制御） */
            int aTag;
            int aIndex;
            Point[] pt = new Point[3];

            if (gControlMode == Program.POWER_OFF)
            {
                return;
            }

            PictureBox aPBox = sender as PictureBox;
            aTag = Convert.ToInt32(aPBox.Tag);

            if (gAppSettings.mLocCtrlList[aTag] <= 0)
            {
                return;
            }

            aIndex = gAppSettings.mLocCtrlList[aTag] - 1;

            if (aIndex > LocDB.Items.Count)
            {
                //indexが機関車DBの全体数よりも多い場合
                return;
            }

            /* アンチエイリアス有効 */
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            /* 背景の描画 */
            DrawIconRoundRectangle(e.Graphics, 0, 0, (40 - 1) * ScaleRation / 100, (26 - 1) * ScaleRation / 100, 5, true, Color.FromArgb(240, 240, 240), Color.Silver, Color.DimGray);
            DrawIconRoundRectangle(e.Graphics, 41 * ScaleRation / 100, 0, (40 - 1) * ScaleRation / 100, (26 - 1) * ScaleRation / 100, 5, true, Color.FromArgb(240, 240, 240), Color.Silver, Color.DimGray);


            /* 表示 */
            switch (LocDB.Items[aIndex].mCurrentDirection)
            {
                case 0:
                case Program.DIRECTION_FWD:
                    pt[0] = new Point((41 + 10 + 0) * ScaleRation / 100, (0 + 3) * ScaleRation / 100);
                    pt[1] = new Point((41 + 10 + 20) * ScaleRation / 100, (10 + 3) * ScaleRation / 100);
                    pt[2] = new Point((41 + 10 + 0) * ScaleRation / 100, (20 + 3) * ScaleRation / 100);
                    break;
                case Program.DIRECTION_REV:
                    pt[0] = new Point((10 + 20) * ScaleRation / 100, (0 + 3) * ScaleRation / 100);
                    pt[1] = new Point((10 + 0) * ScaleRation / 100, (10 + 3) * ScaleRation / 100);
                    pt[2] = new Point((10 + 20) * ScaleRation / 100, (20 + 3) * ScaleRation / 100);
                    break;
            }

            e.Graphics.FillPolygon(Brushes.Black, pt);

            switch (LocDB.Items[aIndex].mCurrentDirection)
            {
                case 0:
                case Program.DIRECTION_FWD:
                    pt[0] = new Point((10 + 20) * ScaleRation / 100, (0 + 3) * ScaleRation / 100);
                    pt[1] = new Point((10 + 0) * ScaleRation / 100, (10 + 3) * ScaleRation / 100);
                    pt[2] = new Point((10 + 20) * ScaleRation / 100, (20 + 3) * ScaleRation / 100);
                    break;
                case Program.DIRECTION_REV:
                    pt[0] = new Point((41 + 10 + 0) * ScaleRation / 100, (0 + 3) * ScaleRation / 100);
                    pt[1] = new Point((41 + 10 + 20) * ScaleRation / 100, (10 + 3) * ScaleRation / 100);
                    pt[2] = new Point((41 + 10 + 0) * ScaleRation / 100, (20 + 3) * ScaleRation / 100);
                    break;
            }

            e.Graphics.FillPolygon(Brushes.Gray, pt);

        }

        private void pBox_DirectBox0_MouseClick(object sender, MouseEventArgs e)
        {
            /* 進行方向調整 */
            int aTag;
            int aDirection;
            int aIndex;
            int aTemp;

            if (gControlMode == Program.POWER_OFF)
            {
                return;
            }

            PictureBox aPBox = sender as PictureBox;
            aTag = Convert.ToInt32(aPBox.Tag);

            if (gAppSettings.mLocCtrlList[aTag] <= 0)
            {
                return;
            }

            aIndex = gAppSettings.mLocCtrlList[aTag] - 1;

            /* 進行方向調整 */
            aTemp = e.X / (aPBox.Width / 2);

            /* 範囲調整 */
            if ((aTemp < 0) || (aTemp > 1))
            {
                return;
            }

            /* 反転させる（表示が逆なため） */
            switch (aTemp)
            {
                case 0:
                    aDirection = 1;
                    break;
                case 1:
                    aDirection = 0;
                    break;
                default:
                    aDirection = 0;
                    break;
            }

            /* 進行方向をセット */
            SetLocDirection(aIndex, aDirection);

            /* 画面を更新 */
            ChangeButtonMultiLocs(aTag);
        }

        private void label_S88EventName_TextChanged(object sender, EventArgs e)
        {
            /* 現状サイズが大きい場合は、フォントサイズを自動調整する */

            Control aUIParts = sender as Control;

            Size stringSize = TextRenderer.MeasureText(aUIParts.Text, aUIParts.Font);

            if (aUIParts.Size.Width < stringSize.Width)
            {
                Font aFont = new Font("Arial", 8);
                aUIParts.Font = aFont;
            }
        }

        private void button_TriggerProperty_Click(object sender, EventArgs e)
        {
            /* トリガオプションの設定 */
            
            int aIndex;

            /* 選択番号変更 */
            aIndex = lBox_S88Events.SelectedIndex;

            if ((aIndex < 0) || (aIndex >= Program.S88EVENT_MAX))
            {
                return;
            }

            //Startup event has no properties
            if (cBox_S88StartTrigger.SelectedIndex == 11)
            {
                MessageBox.Show("Startup event has no properties.");
                return;
            }

            /* フォームを開く */
            ScriptTriggerForm aForm = new ScriptTriggerForm();

            aForm.SetFormLanguage(LangManager);

            /* セット */
            switch (cBox_S88StartTrigger.SelectedIndex)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    /* S88センサ関係 */
                    aForm.numBox_S88SensorAddr.Value = S88Manager.mEvents[aIndex].mSensorAddress;
                    aForm.numUpDown_hour.Enabled = false;
                    aForm.numUpDown_min.Enabled = false;
                    aForm.numUpDown_sec.Enabled = false;
                    aForm.gBox_Speed.Enabled = false;
                    aForm.gBox_Flag.Enabled = false;
                    aForm.gBox_Route.Enabled = false;
                    break;
                case 4:
                    /* Interval */
                    aForm.numUpDown_hour.Value = S88Manager.mEvents[aIndex].mIntervalTime / (60 * 60);
                    aForm.numUpDown_min.Value = (S88Manager.mEvents[aIndex].mIntervalTime / 60) % 60;
                    aForm.numUpDown_sec.Value = S88Manager.mEvents[aIndex].mIntervalTime % 60;

                    aForm.numBox_S88SensorAddr.Enabled = false;
                    aForm.gBox_Speed.Enabled = false;
                    aForm.gBox_Flag.Enabled = false;
                    aForm.gBox_Route.Enabled = false;
                    aForm.gBox_SpecTime.Text = LangManager.SetText("TxtTriggerIntervalTitle", "Interval time");
                    break;

                case 5:
                    /* Clock */
                    aForm.numUpDown_hour.Value = S88Manager.mEvents[aIndex].mTriggerTime / (60 * 60);
                    aForm.numUpDown_min.Value = (S88Manager.mEvents[aIndex].mTriggerTime / 60) % 60;
                    aForm.numUpDown_sec.Value = S88Manager.mEvents[aIndex].mTriggerTime % 60;

                    aForm.numBox_S88SensorAddr.Enabled = false;
                    aForm.gBox_Speed.Enabled = false;
                    aForm.gBox_Flag.Enabled = false;
                    aForm.gBox_Route.Enabled = false;
                    aForm.gBox_SpecTime.Text = LangManager.SetText("TxtTriggerClockTitle", "Clock time");
                    break;
                case 6:
                    /* Interval */
                    aForm.numUpDown_hour.Value = S88Manager.mEvents[aIndex].mIntervalTime / (60 * 60);
                    aForm.numUpDown_min.Value = (S88Manager.mEvents[aIndex].mIntervalTime / 60) % 60;
                    aForm.numUpDown_sec.Value = S88Manager.mEvents[aIndex].mIntervalTime % 60;

                    aForm.numBox_S88SensorAddr.Enabled = false;
                    aForm.gBox_Speed.Enabled = false;
                    aForm.gBox_Flag.Enabled = false;
                    aForm.gBox_Route.Enabled = false;
                    aForm.gBox_SpecTime.Text = LangManager.SetText("TxtTriggerRandomTitle", "Random time");
                    break;


                case 7:
                    //走行中
                    aForm.numBox_S88SensorAddr.Enabled = false;
                    aForm.numUpDown_hour.Enabled = false;
                    aForm.numUpDown_min.Enabled = false;
                    aForm.numUpDown_sec.Enabled = false;
                    aForm.numUpDown_StopSpeed.Enabled = false;

                    aForm.numUpDown_RunSpeed.Value = S88Manager.mEvents[aIndex].mTriggerSpeed_Run;
                    aForm.gBox_Flag.Enabled = false;
                    aForm.gBox_Route.Enabled = false;

                    break;
                case 8:
                    //停止中
                    aForm.numBox_S88SensorAddr.Enabled = false;
                    aForm.numUpDown_hour.Enabled = false;
                    aForm.numUpDown_min.Enabled = false;
                    aForm.numUpDown_sec.Enabled = false;
                    aForm.numUpDown_RunSpeed.Enabled = false;

                    aForm.numUpDown_StopSpeed.Value = S88Manager.mEvents[aIndex].mTriggerSpeed_Stop;
                    aForm.gBox_Flag.Enabled = false;
                    aForm.gBox_Route.Enabled = false;
                    break;

                case 9:
                    //フラグ
                    aForm.numBox_S88SensorAddr.Enabled = false;
                    aForm.gBox_SpecTime.Enabled = false;
                    aForm.gBox_Speed.Enabled = false;
                    aForm.gBox_Route.Enabled = false;

                    aForm.nuUpDown_FlagNo.Value = S88Manager.mEvents[aIndex].mFlagNo;
                    aForm.nuUpDown_FlagVal.Value = S88Manager.mEvents[aIndex].mFlagValue;

                    break;
                case 10:
                    //ルート
                    aForm.numBox_S88SensorAddr.Enabled = false;
                    aForm.gBox_SpecTime.Enabled = false;
                    aForm.gBox_Speed.Enabled = false;
                    aForm.gBox_Flag.Enabled = false;

                    aForm.comboBox_Routes.Items.Clear();

                    if (Routes.RoutesList.ListItems.Count <= 0)
                    {

                        aForm.comboBox_Routes.Items.Add("Not selected");
                        aForm.comboBox_Routes.SelectedIndex = 0;
                    }
                    else
                    {

                        for (int i = 0; i < Routes.RoutesList.ListItems.Count; i++)
                        {
                            aForm.comboBox_Routes.Items.Add(i.ToString() + ":" + Routes.RoutesList.ListItems[i].NameText);

                        }

                        if (S88Manager.mEvents[aIndex].mRouteNo < Routes.RoutesList.ListItems.Count)
                        {
                            aForm.comboBox_Routes.SelectedIndex = S88Manager.mEvents[aIndex].mRouteNo;
                        }
                        else
                        {
                            aForm.comboBox_Routes.SelectedIndex = Routes.RoutesList.ListItems.Count - 1;
                        }
                    }


                    break;
                case 11:
                    //線路電源投入時
                    //return;
                    break;
            } 
           

            aForm.ShowDialog(this);

            //押されたボタン別の処理 
            if (aForm.DialogResult == System.Windows.Forms.DialogResult.OK)
            {

                switch(cBox_S88StartTrigger.SelectedIndex)
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                       S88Manager.mEvents[aIndex].mSensorAddress = decimal.ToInt32(aForm.numBox_S88SensorAddr.Value);
                        break;
                    case 4:
                        /* Interval */
                        S88Manager.mEvents[aIndex].mIntervalTime =  decimal.ToInt32(aForm.numUpDown_hour.Value * (60 * 60) + (aForm.numUpDown_min.Value * 60) + aForm.numUpDown_sec.Value);
                        break;

                    case 5:
                        /* Clock */
                        S88Manager.mEvents[aIndex].mTriggerTime =  decimal.ToInt32(aForm.numUpDown_hour.Value * (60 * 60) + (aForm.numUpDown_min.Value * 60) + aForm.numUpDown_sec.Value);

                        break;
                    case 6:
                        /* Interval */
                        S88Manager.mEvents[aIndex].mIntervalTime = decimal.ToInt32(aForm.numUpDown_hour.Value * (60 * 60) + (aForm.numUpDown_min.Value * 60) + aForm.numUpDown_sec.Value);
                        break;
                    case 7:
                        S88Manager.mEvents[aIndex].mTriggerSpeed_Run = decimal.ToInt32(aForm.numUpDown_RunSpeed.Value);
                        break;
                    case 8:
                        S88Manager.mEvents[aIndex].mTriggerSpeed_Stop = decimal.ToInt32(aForm.numUpDown_StopSpeed.Value);
                        break;

                    case 9:
                        S88Manager.mEvents[aIndex].mFlagNo = decimal.ToInt32(aForm.nuUpDown_FlagNo.Value);
                        S88Manager.mEvents[aIndex].mFlagValue = decimal.ToInt32(aForm.nuUpDown_FlagVal.Value);

                        break;

                    case 10:
                        S88Manager.mEvents[aIndex].mRouteNo = aForm.comboBox_Routes.SelectedIndex;

                        break;

                }

                /* トリガオプションの表示変更 */
                UpdateS88ScriptTriggerOption();

            }
        }

        private void UpdateS88ScriptTriggerOption()
        {

            int aIndex;

            /* 選択番号変更 */
            aIndex = lBox_S88Events.SelectedIndex;

            if ((aIndex < 0) || (aIndex >= Program.S88EVENT_MAX))
            {
                return;
            }

            switch (cBox_S88StartTrigger.SelectedIndex)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    /* S88センサ関係 */
                    label_S88TriggerData.Text = LangManager.SetText("TxtS88SensorAddr", "Sensor address") + ": " + S88Manager.mEvents[aIndex].mSensorAddress.ToString();
                    break;

                case 4:
                    /* Interval */
                    label_S88TriggerData.Text = LangManager.SetText("TxtTriggerIntervalTitle", "Interval time") + ": " + (S88Manager.mEvents[aIndex].mIntervalTime / (60 * 60)).ToString("D2") + ":" + ((S88Manager.mEvents[aIndex].mIntervalTime / 60) % 60).ToString("D2") + ":" + (S88Manager.mEvents[aIndex].mIntervalTime % 60).ToString("D2");
                    break;

                case 5:
                    /* Clock */
                    label_S88TriggerData.Text = LangManager.SetText("TxtTriggerClockTitle", "Clock time") + ": " + (S88Manager.mEvents[aIndex].mTriggerTime / (60 * 60)).ToString("D2") + ":" + ((S88Manager.mEvents[aIndex].mTriggerTime / 60) % 60).ToString("D2") + ":" + (S88Manager.mEvents[aIndex].mTriggerTime % 60).ToString("D2");
                    break;

                case 6:
                    /* Random Interval */
                    label_S88TriggerData.Text = LangManager.SetText("TxtTriggerRandomTitle", "Random time") + ": " + (S88Manager.mEvents[aIndex].mIntervalTime / (60 * 60)).ToString("D2") + ":" + ((S88Manager.mEvents[aIndex].mIntervalTime / 60) % 60).ToString("D2") + ":" + (S88Manager.mEvents[aIndex].mIntervalTime % 60).ToString("D2");
                    break;

                case 7:
                    label_S88TriggerData.Text = LangManager.SetText("TxtSpeedThreshold", "Threshold speed") + ": " + S88Manager.mEvents[aIndex].mTriggerSpeed_Run.ToString() + "(" + (S88Manager.mEvents[aIndex].mTriggerSpeed_Run * 100 / 1024).ToString("0.0") + "%)";
                    break;

                case 8:
                    label_S88TriggerData.Text = LangManager.SetText("TxtSpeedThreshold", "Threshold speed") + ": " + S88Manager.mEvents[aIndex].mTriggerSpeed_Stop.ToString() + "(" + (S88Manager.mEvents[aIndex].mTriggerSpeed_Stop * 100 / 1024).ToString("0.0") + "%)";
                    break;

                case 9:
                    label_S88TriggerData.Text = "FlagNo." + S88Manager.mEvents[aIndex].mFlagNo.ToString() + " == " + S88Manager.mEvents[aIndex].mFlagValue.ToString(); 
                    break;

                case 10:
                    label_S88TriggerData.Text = "RouteNo." + S88Manager.mEvents[aIndex].mRouteNo.ToString() + " is opening.";
                    break;

                case 11:
                    label_S88TriggerData.Text = "Start up";
                    break;
            }
        }

        private void RunFile(string inAppName, string inFileName)
        {
            ExecuteManager.Run(inAppName, inFileName);
        }

        private void SButton_RunFile_Click(object sender, EventArgs e)
        {
            int i;

            /* 設定フォームを開く */
            ExecuteCfgForm aForm = new ExecuteCfgForm();

            /* 言語切り替え */
            aForm.SetFormLanguage(LangManager);

            /* 設定項目反映 */
            aForm.listView_RunFile.Items.Clear();

            for ( i = 0; i < ExecuteManager.Items.Count; i++)
            {
                ListViewItem aItem = new ListViewItem();
                aItem.Text = i.ToString();

                aItem.SubItems.Add(ExecuteManager.Items[i].ItemName);
                aItem.SubItems.Add(ExecuteManager.Items[i].FileName);
                aItem.SubItems.Add(ExecuteManager.Items[i].Option);

                aForm.listView_RunFile.Items.Add(aItem);
            }


            /* 設定フォームを開く */
            aForm.ShowDialog(this);

            //押されたボタン別の処理 
            if (aForm.DialogResult == System.Windows.Forms.DialogResult.OK)
            {

                /* 保存処理 */
                ExecuteManager.Clear();

                for (i = 0; i < aForm.listView_RunFile.Items.Count ; i++)
                {

                    ExecuteManager.Add(aForm.listView_RunFile.Items[i].SubItems[1].Text, aForm.listView_RunFile.Items[i].SubItems[2].Text, aForm.listView_RunFile.Items[i].SubItems[3].Text);

                }


            }

            //フォームの解放 
            aForm.Dispose();
        }

        private void pBox_AccList_MouseMove(object sender, MouseEventArgs e)
        {
            int ax, ay;
            int bx, by;
            int aIndex;
            int aTopIndex;

            if (gControlMode == Program.POWER_OFF)
            {
                return;
            }

            ax = e.X / Program.ACCESSORIES_ITEMWIDTH;
            ay = e.Y / Program.ACCESSORIES_ITEMHEIGHT;
            bx = e.X % Program.ACCESSORIES_ITEMWIDTH;
            by = e.Y % Program.ACCESSORIES_ITEMHEIGHT;

            /* 最大値チェック */
            if ((ax > Program.ACCESSORIES_IDXWIDTH) || (ax < 0) || (ay > Program.ACCESSORIES_IDXHEIGHT) || (ay < 0))
            {
                return;
            }

            /* 順番を算出する */
            aTopIndex = AccScrollBar.Value * 40;
            aIndex = ay * Program.ACCESSORIES_IDXWIDTH + ax + aTopIndex;

            /* 一つ前がアドレスを二つ使うタイプの場合 */
            if (getAccListDoubleType(aIndex - 1) == true)
            {
                aIndex = aIndex - 1;
            }

            SetAccTips(aIndex);

        }

        private void pBox_EventClock_Click(object sender, EventArgs e)
        {
            openClockConfigForm();
        }

        private void pBox_EventClock_Paint(object sender, PaintEventArgs e)
        {
            DateTime aTime;

            /* 描画 */
            Graphics aCanvas = e.Graphics;

            aCanvas.DrawImage(Properties.Resources.Clock, 0, 0, Properties.Resources.Clock.Width * ScaleRation / 100, Properties.Resources.Clock.Height * ScaleRation / 100);

            //(アンチエイリアス処理されたレタリング)を指定する
            aCanvas.SmoothingMode = SmoothingMode.AntiAlias;

            /* 時間の決定 */
            if (gAppSettings.mUseVirtualClock == true)
            {
                aTime = gAppSettings.mVirtualClock + (DateTime.Now - timeBoot);

            }
            else
            {
                aTime = DateTime.Now;
            }

            /* 時計の描画 */
            MeterDrawer.DrawClockBox(aCanvas, aTime, ScaleRation);
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {

            if (gControlMode == Program.POWER_OFF)
            {
                return;

            }

            /* 機関車制御メイン画面以外はキー操作禁止 */
            if (panel_Loc.Visible == true)
            {
                KeyUpLocPanel((int)e.KeyCode);
            }
            else if (panel_MultiLocs.Visible == true)
            {
                KeyUpMultiPanel((int)e.KeyCode);
            }
            else if (panel6021.Visible == true)
            {
                    
            }
            else if (panel_Layout.Visible == true)
            {
                    
            }


        }

        private void button_CLR_Click(object sender, EventArgs e)
        {
            /* アドレスクリア */
            Update6021DisplayAddress(0);
        }

        private void button_FWD_TextChanged(object sender, EventArgs e)
        {
            /* 現状サイズが大きい場合は、フォントサイズを自動調整する */

            Control aUIParts = sender as Control;

            Size stringSize = TextRenderer.MeasureText(aUIParts.Text, aUIParts.Font);

            if (aUIParts.Size.Width < stringSize.Width + 32)
            {
                Font aFont = new Font("Arial", 10);
                aUIParts.Font = aFont;
            }
        }

        private void button_ConsoleLogClear_TextChanged(object sender, EventArgs e)
        {
            /* 現状サイズが大きい場合は、フォントサイズを自動調整する */

            Control aUIParts = sender as Control;

            Size stringSize = TextRenderer.MeasureText(aUIParts.Text, aUIParts.Font);

            if (aUIParts.Size.Width < stringSize.Width)
            {
                Font aFont = new Font("Arial", 9);
                aUIParts.Font = aFont;
            }
        }

        private void button_CVGenerate_Click(object sender, EventArgs e)
        {
            UpdateDCCCVLocAddr();

        }

        private void UpdateDCCCVLocAddr()
        {
            int aLocAddress = decimal.ToInt32(numUpDown_CVLocAddress.Value);

            if (aLocAddress <= 127)
            {
                label_CV_01.Text = "CV01: " + aLocAddress.ToString();
                label_CV_17.Text = "CV17: -";
                label_CV_18.Text = "CV18: -";

            }
            else
            {
                label_CV_01.Text = "CV01: -";
                label_CV_17.Text = "CV17: " + ((aLocAddress >> 8) | 0xC0).ToString();
                label_CV_18.Text = "CV18: " + (aLocAddress & 0xFF).ToString();

            }

            label_CV_29.Text = "CV29: " + GenerateCV29().ToString();
        }

        private int GenerateCV29()
        {
            int aCV29 = 0;

            //CV29の生成

            for( int i = 0; i < 8; i++)
            {
                if (chkListBox_CV29Calc.GetItemChecked(i) == true)
                {
                    aCV29 = aCV29 | (1 << i);
                }
                else
                {
                    //何もしない
                }

            }

            return aCV29;
        }

        private void cBox_CVProtcol_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox aItem = sender as ComboBox;

            if (aItem.SelectedIndex == 2)
            {
                gBox_CVDCCAdrGen.Enabled = true;
                cBox_DecoderManufacture.Enabled = true;
                numUpDown_CVLocAddr.Enabled = (cBox_DecoderManufacture.SelectedIndex == 2) ? true : false;

            }
            else
            {
                gBox_CVDCCAdrGen.Enabled = false;
                cBox_DecoderManufacture.Enabled = false;
                numUpDown_CVLocAddr.Enabled = false;
            }
        }

        private void button_DCCCVAdrRead_Click(object sender, EventArgs e)
        {

            //DCC専用のアドレス指定機能

            Button aButton = sender as Button;
            int aTag;
            int aLocAddress;
            int aDCCAddress;
            int aAddrOffset = 0;

            aTag = int.Parse(aButton.Tag.ToString());

            /*表示を更新しておく*/
            UpdateDCCCVLocAddr();

            aLocAddress = LocDB.AssignAddressProtcol(cBox_CVProtcol.SelectedIndex, 0);

            if ((cBox_DecoderManufacture.SelectedIndex == 1) && (cBox_CVProtcol.SelectedIndex == 2))
            {
                //DS激安DCCデコーダの特殊モード移行用
                aAddrOffset = 0x4000;
            }
            else
            {
                aAddrOffset = 0;
            }


            /* 読み書き処理方向 */
            switch (aTag)
            {
                case 1:
                    /* CV Write */

                    /* 警告表示 */
                    if (MessageBox.Show(LangManager.SetText("TxtCVAttention", "Write CV to your locomotive. Are you sure?"), LangManager.SetText("TxtCVAttentionTitle", "CV write attention"), MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        aDCCAddress = decimal.ToInt32(numUpDown_CVLocAddress.Value);

                        SerialCmd.SetLocoConfig(aLocAddress, 29 + aAddrOffset, GenerateCV29());
                        DSCommon.WaitSleepTime(30);

                        /* コマンド送信  */
                        if (aDCCAddress <= 127)
                        {
                            SerialCmd.SetLocoConfig(aLocAddress, 1 + aAddrOffset, aDCCAddress);

                        }
                        else
                        {
                            SerialCmd.SetLocoConfig(aLocAddress, 17 + aAddrOffset, (aDCCAddress >> 8) | 0xC0);
                            DSCommon.WaitSleepTime(30);
                            SerialCmd.SetLocoConfig(aLocAddress, 18 + aAddrOffset, aDCCAddress & 0xFF);
                        }



                    }
                    break;

                case 2:
                    /* CV Read */

                    SerialCmd.GetLocoConfig(aLocAddress, 1);
                    DSCommon.WaitSleepTime(80);
                    SerialCmd.GetLocoConfig(aLocAddress, 17);
                    DSCommon.WaitSleepTime(80);
                    SerialCmd.GetLocoConfig(aLocAddress, 18);
                    DSCommon.WaitSleepTime(80);
                    SerialCmd.GetLocoConfig(aLocAddress, 29);


                    break;
            }
        }


        private void numUpDown_CVLocAddress_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown aNumUpDown = sender as NumericUpDown;

            if (aNumUpDown.Value > 127)
            {
                chkListBox_CV29Calc.SetItemChecked(5, true);
            }
            else
            {
                chkListBox_CV29Calc.SetItemChecked(5, false);
            }
        }

        private void MainForm_Activated(object sender, EventArgs e)
        {

        }

        private void Button_CraneBoomUp_Click(object sender, EventArgs e)
        {
            NotSelectableCheckBox aButton = sender as NotSelectableCheckBox;
            int aTag;

            aTag = int.Parse(aButton.Tag.ToString());

            switch (aTag)
            {
            case 99:
                   CraneController.CraneSpeed = 128;
                   tBar_CraneSpeed.Value = 128;
            break;

            case 0:
                Button_CraneBoomUp.Checked = false;
                Button_CraneBoomDown.Checked = false;
                Button_CraneHookUp.Checked = false;
                Button_CraneHookDown.Checked = false;
                Button_CraneLeft.Checked = false;
                Button_CraneRight.Checked = false;

            break;

               default:
                Button_CraneBoomUp.Checked = false;
                Button_CraneBoomDown.Checked = false;
                Button_CraneHookUp.Checked = false;
                Button_CraneHookDown.Checked = false;
                Button_CraneLeft.Checked = false;
                Button_CraneRight.Checked = false;
                

                break;



            }


            if (CraneController.DoButtonControl(aTag) && (aTag >= 1) && (aTag < 99))
            {
                aButton.Checked = true;
            }

            

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            int aLocAddress = LocDB.GetAddress(CraneController.CraneLocAddress);

            CraneController.CraneLocAddress = LocDB.AssignAddressProtcol(cBox_CraneProtcol.SelectedIndex, aLocAddress);

        }

        private void numUpDownCraneAddress_ValueChanged(object sender, EventArgs e)
        {
            int aLocProtcol = LocDB.GetAddressLocProtcol(CraneController.CraneLocAddress);

            CraneController.CraneLocAddress = LocDB.AssignAddressProtcol(aLocProtcol, decimal.ToInt32(numUpDownCraneAddress.Value));

        }

        private void tBar_CraneSpeed_ValueChanged(object sender, EventArgs e)
        {
            /* 速度の更新 */
            CraneController.CraneSpeed = tBar_CraneSpeed.Value;
            CraneController.DoRun();
        }

        private void cBox_CraneType_SelectedIndexChanged(object sender, EventArgs e)
        {

            EnableCranePanelUI(cBox_CraneType.SelectedIndex, true, false);

        }

        private void EnableCranePanelUI(int inIndex, bool inAvailable, bool inAll)
        {
            if (inAll == true)
            {
                Button_CraneBoomDown.Enabled = inAvailable;
                Button_CraneBoomUp.Enabled = inAvailable;
                Button_CraneBoomDown.Enabled = inAvailable;
                Button_CraneHookDown.Enabled = inAvailable;
                Button_CraneHookUp.Enabled = inAvailable;
                Button_CraneLeft.Enabled = inAvailable;
                Button_CraneRight.Enabled = inAvailable;
                Button_CraneRabbit.Enabled = inAvailable;
                Button_CraneStop.Enabled = inAvailable;
                cBox_CraneProtcol.Enabled = inAvailable;
                numUpDownCraneAddress.Enabled = inAvailable;
                cBox_CraneType.Enabled = inAvailable;
                tBar_CraneSpeed.Enabled = inAvailable;

            }


            switch (inIndex)
            {
                case 0:
                    //Trix 23591, Marklin33951,4995x
                    Button_CraneRunLeft.Enabled = false & inAvailable;
                    Button_CraneRunRight.Enabled = false & inAvailable;
                    Button_CraneCabLeft.Enabled = false & inAvailable;
                    Button_CraneCabRight.Enabled = false & inAvailable;


                    break;

                case 1:
                    // Marklin 76515
                    Button_CraneRunLeft.Enabled = false & inAvailable;
                    Button_CraneRunRight.Enabled = false & inAvailable;
                    Button_CraneCabLeft.Enabled = false & inAvailable;
                    Button_CraneCabRight.Enabled = false & inAvailable;
                    break;

                case 2:
                    // Marklin 76501
                    Button_CraneRunLeft.Enabled = true & inAvailable;
                    Button_CraneRunRight.Enabled = true & inAvailable;
                    Button_CraneCabLeft.Enabled = true & inAvailable;
                    Button_CraneCabRight.Enabled = true & inAvailable;
                    break;

            }


        }

        private void timerUpdateAll_Tick(object sender, EventArgs e)
        {
            IntervalDrawAllPanel();
        }

        private void cBox_LocChangeEV_DrawItem(object sender, DrawItemEventArgs e)
        {
            int aIndex = e.Index - 1;

            ComboBox aCBox = sender as ComboBox;

            /* 描画アイテム */

            e.DrawBackground();

            if (LocDB.Items.Count <= 0)
            {
                return;
            }

            if (aIndex < 0)
            {
                /* NotSelectedを表示する（選択しない条件を設定できるようにするため） */
                MeterDrawer.DrawLocLabel(e.Graphics, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height, LocDB.Items.Count, Brushes.Black, LocDB, aCBox.Items[0].ToString(), ScaleRation);
            }
            else
            {
                /* 通常の機関車データを表示 */
                MeterDrawer.DrawLocLabel(e.Graphics, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height, aIndex, Brushes.Black, LocDB, aCBox.Items[0].ToString(), ScaleRation);
            }

            /* 選択枠描画 */
            e.DrawFocusRectangle();
        }

        private void cBox_LocChangeEV_SelectedIndexChanged(object sender, EventArgs e)
        {
            int aIndex;

            ComboBox aCBox = sender as ComboBox;

            /* 選択番号変更 */
            aIndex = lBox_S88Events.SelectedIndex;

            if ((aIndex < 0) || (aIndex >= Program.S88EVENT_MAX))
            {
                return;
            }

            if (aCBox.SelectedIndex == 0)
            {
                return;
            }
            else
            {

                S88Manager.mEvents[aIndex].mLocIndex = aCBox.SelectedIndex - 1;
                S88Manager.mEvents[aIndex].mSlotIndex = 0;

                //スロット選択無効
                cBox_SlotChangeEV.SelectedIndex = 0;
            }
        }

        private void SButton_Joystick_Click(object sender, EventArgs e)
        {

        }


        private void ResizeWindow()
        {
            int aScale = ScaleRation;

            int aWidth;
            int aHeight;

            aWidth = (Program.SCREEN_SIZE1_WIDTH * aScale / 100);
            aHeight = (Program.SCREEN_SIZE1_HEIGHT * aScale / 100);
            this.ClientSize = new System.Drawing.Size(aWidth, aHeight + statusStrip1.Height + toolStrip.Height);

            panel_Loc.ClientSize = new System.Drawing.Size(aWidth, aHeight);
            panel_Crane.ClientSize = new System.Drawing.Size(aWidth, aHeight);
            panel_AccList.ClientSize = new System.Drawing.Size(aWidth, aHeight);
            panel_CVEditor.ClientSize = new System.Drawing.Size(aWidth, aHeight);
            panel_Layout.ClientSize = new System.Drawing.Size(aWidth, aHeight);
            panel_MultiLocs.ClientSize = new System.Drawing.Size(aWidth, aHeight);
            panel_Sequence.ClientSize = new System.Drawing.Size(aWidth, aHeight);
            panel6021.ClientSize = new System.Drawing.Size(aWidth, aHeight);
            panel_S88.ClientSize = new System.Drawing.Size(aWidth, aHeight);
            panel_SerialConsole.ClientSize = new System.Drawing.Size(aWidth, aHeight);


            /* 各UIの位置とサイズのスケーリング調整 */

            //Cab panel
            SetScaleItems(cBox_LocChange, aScale);
            SetScaleItems(button_AnyLocs, aScale);
            SetScaleItems(button_NewLoc, aScale);
            SetScaleItems(button_DelLoc, aScale);
            SetScaleItems(button_FWD, aScale);
            SetScaleItems(button_REV, aScale);
            SetScaleItems(button_F1, aScale);
            SetScaleItems(button_F2, aScale);
            SetScaleItems(button_F3, aScale);
            SetScaleItems(button_F4, aScale);
            SetScaleItems(button_F5, aScale);
            SetScaleItems(button_F6, aScale);
            SetScaleItems(button_F7, aScale);
            SetScaleItems(button_F8, aScale);
            SetScaleItems(button_F9, aScale);
            SetScaleItems(button_F10, aScale);
            SetScaleItems(button_F11, aScale);
            SetScaleItems(button_F12, aScale);
            SetScaleItems(button_F13, aScale);
            SetScaleItems(button_F14, aScale);
            SetScaleItems(button_F15, aScale);
            SetScaleItems(button_F16, aScale);
            SetScaleItems(pBox_Clock, aScale);
            SetScaleItems(button_MultiFunc0, aScale);
            SetScaleItems(button_MultiFunc1, aScale);
            SetScaleItems(MeterBox, aScale);
            SetScaleItems(LeverBox, aScale);
            SetScaleItems(MultiFunctionBox, aScale);
            SetScaleItems(Button_WebApp, aScale);
            SetScaleItems(button_LocDBLoad, aScale);
            SetScaleItems(button_LocDBSave, aScale);
            

            //Accessory
            //SetScaleItems(cBox_AccFilter, aScale);
            SetScaleItems(buttonEditAccOption, aScale);
            SetScaleItems(pBox_AccList, aScale);
            SetScaleItems(AccScrollBar, aScale);


            //Multi Locs
            SetScaleItems(buttonMultiF1, aScale);
            SetScaleItems(buttonMultiF2, aScale);
            SetScaleItems(buttonMultiF3, aScale);
            SetScaleItems(buttonMultiF4, aScale);
            SetScaleItems(buttonMultiF5, aScale);
            SetScaleItems(buttonMultiF6, aScale);
            SetScaleItems(buttonMultiF7, aScale);
            SetScaleItems(buttonMultiF8, aScale);
            SetScaleItems(buttonMultiF9, aScale);
            SetScaleItems(buttonMultiF10, aScale);
            SetScaleItems(buttonMultiF11, aScale);
            SetScaleItems(buttonMultiF12, aScale);
            SetScaleItems(buttonMultiF13, aScale);
            SetScaleItems(buttonMultiF14, aScale);
            SetScaleItems(buttonMultiF15, aScale);
            SetScaleItems(buttonMultiF16, aScale);
            SetScaleItems(buttonMLoc_Select0, aScale);
            SetScaleItems(buttonMLoc_Select1, aScale);
            SetScaleItems(buttonMLoc_Select2, aScale);
            SetScaleItems(buttonMLoc_Select3, aScale);
            SetScaleItems(buttonMLoc_Select4, aScale);
            SetScaleItems(buttonMLoc_Select5, aScale);
            SetScaleItems(buttonMLoc_Select6, aScale);
            SetScaleItems(buttonMLoc_Select7, aScale);
            SetScaleItems(panelLocCtrl0, aScale);
            SetScaleItems(panelLocCtrl1, aScale);
            SetScaleItems(panelLocCtrl2, aScale);
            SetScaleItems(panelLocCtrl3, aScale);
            SetScaleItems(panelLocCtrl4, aScale);
            SetScaleItems(panelLocCtrl5, aScale);
            SetScaleItems(panelLocCtrl6, aScale);
            SetScaleItems(panelLocCtrl7, aScale);
            SetScaleItems(SpeedCtrlBox0, aScale);
            SetScaleItems(SpeedCtrlBox1, aScale);
            SetScaleItems(SpeedCtrlBox2, aScale);
            SetScaleItems(SpeedCtrlBox3, aScale);
            SetScaleItems(SpeedCtrlBox4, aScale);
            SetScaleItems(SpeedCtrlBox5, aScale);
            SetScaleItems(SpeedCtrlBox6, aScale);
            SetScaleItems(SpeedCtrlBox7, aScale);
            SetScaleItems(LocImageBox0, aScale);
            SetScaleItems(LocImageBox1, aScale);
            SetScaleItems(LocImageBox2, aScale);
            SetScaleItems(LocImageBox3, aScale);
            SetScaleItems(LocImageBox4, aScale);
            SetScaleItems(LocImageBox5, aScale);
            SetScaleItems(LocImageBox6, aScale);
            SetScaleItems(LocImageBox7, aScale);
            SetScaleItems(pBox_DirectBox0, aScale);
            SetScaleItems(pBox_DirectBox1, aScale);
            SetScaleItems(pBox_DirectBox2, aScale);
            SetScaleItems(pBox_DirectBox3, aScale);
            SetScaleItems(pBox_DirectBox4, aScale);
            SetScaleItems(pBox_DirectBox5, aScale);
            SetScaleItems(pBox_DirectBox6, aScale);
            SetScaleItems(pBox_DirectBox7, aScale);
            SetScaleItems(pBox_MLocName0, aScale);
            SetScaleItems(pBox_MLocName1, aScale);
            SetScaleItems(pBox_MLocName2, aScale);
            SetScaleItems(pBox_MLocName3, aScale);
            SetScaleItems(pBox_MLocName4, aScale);
            SetScaleItems(pBox_MLocName5, aScale);
            SetScaleItems(pBox_MLocName6, aScale);
            SetScaleItems(pBox_MLocName7, aScale);
            SetScaleItems(label_slotA, aScale);
            SetScaleItems(label_slotB, aScale);
            SetScaleItems(label_slotC, aScale);
            SetScaleItems(label_slotD, aScale);
            SetScaleItems(label_slotE, aScale);
            SetScaleItems(label_slotF, aScale);
            SetScaleItems(label_slotG, aScale);
            SetScaleItems(label_slotH, aScale);

            //Sequence
            SetScaleItems(buttonScriptLoad, aScale);
            SetScaleItems(listScript, aScale);
            SetScaleItems(buttonScriptSave, aScale);
            SetScaleItems(buttonScriptRun, aScale);
            SetScaleItems(buttonScriptTeach, aScale);
            SetScaleItems(button_SeqScriptUp, aScale);
            SetScaleItems(button_SeqScriptDown, aScale);
            
            for (int i = 0; i < listScript.Columns.Count; i++)
            {
                listScript.Columns[i].Width = listScript.Columns[i].Width * aScale / 100;
            }

            //6021
            SetScaleItems(panel2, aScale);
            SetScaleItems(button6021_0, aScale);
            SetScaleItems(button6021_1, aScale);
            SetScaleItems(button6021_2, aScale);
            SetScaleItems(button6021_3, aScale);
            SetScaleItems(button6021_4, aScale);
            SetScaleItems(button6021_5, aScale);
            SetScaleItems(button6021_6, aScale);
            SetScaleItems(button6021_7, aScale);
            SetScaleItems(button6021_8, aScale);
            SetScaleItems(button6021_9, aScale);
            SetScaleItems(button6021_F1, aScale);
            SetScaleItems(button6021_F2, aScale);
            SetScaleItems(button6021_F3, aScale);
            SetScaleItems(button6021_F4, aScale);
            SetScaleItems(button6021_FWD, aScale);
            SetScaleItems(button6021_REV, aScale);
            SetScaleItems(button6021_OFF, aScale);
            SetScaleItems(button_CLR, aScale);
            SetScaleItems(Dial6021Box, aScale);
            SetScaleItems(cBox_6021Protcol, aScale);
            SetScaleItems(label_6021Direct, aScale);
            SetScaleItems(label_6021Protcol, aScale);
            SetScaleItems(label_Addr, aScale);

            //Crane
            SetScaleItems(Button_CraneBoomDown, aScale);
            SetScaleItems(Button_CraneBoomUp, aScale);
            SetScaleItems(Button_CraneCabLeft, aScale);
            SetScaleItems(Button_CraneCabRight, aScale);
            SetScaleItems(Button_CraneHookDown, aScale);
            SetScaleItems(Button_CraneHookUp, aScale);
            SetScaleItems(Button_CraneLeft, aScale);
            SetScaleItems(Button_CraneRabbit, aScale);
            SetScaleItems(Button_CraneRight, aScale);
            SetScaleItems(Button_CraneRunLeft, aScale);
            SetScaleItems(Button_CraneRunRight, aScale);
            SetScaleItems(Button_CraneStop, aScale);
            SetScaleItems(cBox_CraneProtcol, aScale);
            SetScaleItems(cBox_CraneType, aScale);
            SetScaleItems(labelCraneLocAddr, aScale);
            SetScaleItems(labelCraneProtcol, aScale);
            SetScaleItems(numUpDownCraneAddress, aScale);
            SetScaleItems(tBar_CraneSpeed, aScale);
            SetScaleItems(label_CraneSpeedMax, aScale);
            SetScaleItems(label_CraneSpeedMin, aScale);
            SetScaleItems(gBox_CraneLoc, aScale);
            SetScaleItems(gBoxCraneType, aScale);

            //Console
            SetScaleItems(button_ConsoleLogClear, aScale);
            SetScaleItems(button_CopySerialConsole, aScale);
            SetScaleItems(listBox_Serial, aScale);

            //Event

            SetScaleItems(panel_EVFlagStatus, aScale);
            SetScaleItems(panel_EVRunStatus, aScale);
            SetScaleItems(lBox_S88Events, aScale);
            SetScaleItems(button_S88EventsImport, aScale);
            SetScaleItems(button_S88EventsExport, aScale);
            SetScaleItems(button_S88EventClear, aScale);
            SetScaleItems(lView_S88Script, aScale);
            SetScaleItems(button_S88ScriptDown, aScale);
            SetScaleItems(button_S88ScriptUp, aScale);
            SetScaleItems(label_S88EventScript, aScale);
            SetScaleItems(label_S88LocAddr, aScale);
            SetScaleItems(cBox_LocChangeEV, aScale);
            SetScaleItems(label_S88TriggerCondition, aScale);
            SetScaleItems(label_S88TriggerData, aScale);
            SetScaleItems(label_S88EventName, aScale);
            SetScaleItems(tBox_S88EventName, aScale);
            SetScaleItems(label_S88StartTrigger, aScale);
            SetScaleItems(cBox_S88StartTrigger, aScale);
            SetScaleItems(button_TriggerProperty, aScale);
            SetScaleItems(cBox_S88EventAvailable, aScale);
            SetScaleItems(pBox_S88SensorDisplay, aScale);
            SetScaleItems(button_S88ScriptViewUpdate, aScale);
            SetScaleItems(label_S88ScrInfoFlagView, aScale);
            SetScaleItems(pBox_EventClock, aScale);
            SetScaleItems(label_S88ScrInfoRunChk, aScale);
            SetScaleItems(button_S88ConsoleClear, aScale);
            SetScaleItems(listBox_S88Console, aScale);
            SetScaleItems(gBox_S88Flag, aScale);
            SetScaleItems(gBox_S88Log, aScale);
            SetScaleItems(gBox_S88RunChk, aScale);
            SetScaleItems(gBox_EventClock, aScale);
            SetScaleItems(tabS88Events, aScale);
            SetScaleItems(cBox_SlotChangeEV, aScale);
            SetScaleItems(buttonEventInProcess, aScale);
            SetScaleItems(button_S88ScriptReset, aScale);
            

            
            for(int i = 0; i < lView_S88Script.Columns.Count; i++)
            {
                lView_S88Script.Columns[i].Width = lView_S88Script.Columns[i].Width * aScale / 100;
            }

            //CV
            SetScaleItems(button_CVDCCAdrWrite, aScale);
            SetScaleItems(button_CVGenerate, aScale);
            SetScaleItems(button_DCCCVAdrRead, aScale);
            SetScaleItems(cBox_CVProtcol, aScale);
            SetScaleItems(label_CV_01, aScale);
            SetScaleItems(label_CV_17, aScale);
            SetScaleItems(label_CV_18, aScale);
            SetScaleItems(label_CV_29, aScale);
            SetScaleItems(label_CV29Calc, aScale);
            SetScaleItems(label_CVInfo, aScale);
            SetScaleItems(label_CVLocAdr, aScale);
            SetScaleItems(label_CVNo, aScale);
            SetScaleItems(label_CVProtcol, aScale);
            SetScaleItems(label_CVValue, aScale);
            SetScaleItems(label_CVLocAddr, aScale);
            SetScaleItems(listBox_CVHistory, aScale);
            SetScaleItems(chkListBox_CV29Calc, aScale);
            SetScaleItems(numUpDown_CVNo, aScale);
            SetScaleItems(numUpDown_CVValue, aScale);
            SetScaleItems(buttonCVRead, aScale);
            SetScaleItems(buttonCVWrite, aScale);
            SetScaleItems(numUpDown_CVLocAddress, aScale);
            SetScaleItems(label_CVManufacture, aScale);
            SetScaleItems(cBox_DecoderManufacture, aScale);
            SetScaleItems(gBox_CVLocAdrRelated, aScale);
            SetScaleItems(gBox_CVDCCAdrGen, aScale);
            SetScaleItems(groupBox_CVEditor, aScale);
            SetScaleItems(numUpDown_CVLocAddr, aScale);

            

            //Layout
            SetScaleItems(panelLayoutTool, aScale);
            panelLayoutTool.Width = panelLayoutTool.Width + (aScale - 100) / 2;

            SetScaleItems(button_LayoutNew, aScale);
            SetScaleItems(button_LayoutLoad, aScale);
            SetScaleItems(button_LayoutSave, aScale);
            SetScaleItems(button_LayoutCfg, aScale);
            
            SetScaleItems(TileBox, aScale);
            SetScaleItems(LayoutBox, aScale);
            SetScaleItems(panelLayout, aScale);
            

            //影響を受けるコンポーネントを調整
            MeterBoxChacheBitmap.Dispose();
            MeterBoxChacheBitmap = new Bitmap(400 * aScale / 100, 400 * aScale / 100);


            this.Left = (Screen.PrimaryScreen.Bounds.Width - this.Width) / 2;

            this.Top = (Screen.PrimaryScreen.Bounds.Height - this.Height) / 2;

        }

        private void SetScaleItems(Control inItem, int inScale)
        {
            inItem.Location = GetScaleLocation(inItem, inScale);
            inItem.ClientSize = GetScaleSize(inItem, inScale);
            inItem.Font = GetFontSize(inItem, inScale);

        }

        private void SetScaleWidth(Control inItem, int inScale)
        {
            inItem.Width = inItem.Width * inScale / 100;

        }

        private void SetScaleItemsWithoutFont(Control inItem, int inScale)
        {
            inItem.Location = GetScaleLocation(inItem, inScale);
            inItem.ClientSize = GetScaleSize(inItem, inScale);
        }

        private Point GetScaleLocation(Control inItem, int inScale)
        {
            return new Point((inItem.Location.X * inScale) / 100, (inItem.Location.Y * inScale) / 100);
        }

        private Size GetScaleSize(Control inItem, int inScale)
        {
            return new Size((inItem.ClientSize.Width * inScale) / 100, (inItem.ClientSize.Height * inScale) / 100);
        }

        private Font GetFontSize(Control inItem, int inScale)
        {
            return new Font("Arial",(float)(inItem.Font.Size * inScale / 100), FontStyle.Regular);
        }

        private void serialPort_ErrorReceived(object sender, System.IO.Ports.SerialErrorReceivedEventArgs e)
        {
            /* シリアル通信エラー */

            StatusLabel.Text = "Seirial error";


        }

        private void cBox_DecoderManufacture_SelectedIndexChanged(object sender, EventArgs e)
        {

            numUpDown_CVLocAddr.Enabled = (cBox_DecoderManufacture.SelectedIndex == 2) ? true : false;

        }

        private void cBox_SlotChangeEV_SelectedIndexChanged(object sender, EventArgs e)
        {
            int aIndex;

            ComboBox aCBox = sender as ComboBox;

            /* 選択番号変更 */
            aIndex = lBox_S88Events.SelectedIndex;

            if ((aIndex < 0) || (aIndex >= Program.S88EVENT_MAX))
            {
                return;
            }

            //初期化時は何もしない
            if (aCBox.SelectedIndex == 0)
            {
                return;
            }
            else
            {

                S88Manager.mEvents[aIndex].mLocIndex = -1;
                S88Manager.mEvents[aIndex].mSlotIndex = aCBox.SelectedIndex;

                //スロット選択無効
                cBox_LocChangeEV.SelectedIndex = 0;
            }

        }

        private void cBox_SlotChangeEV_DrawItem(object sender, DrawItemEventArgs e)
        {

        }

        private void GetWebReceivedSerialCommand(string inCommandText)
        {
            SendingList.Insert(0, inCommandText);
            CheckingList.Insert(0, inCommandText);

        }

        private string GetWebStatus(int inMode)
        {
            /* Web経由で、状態をテキストにして応答するための処理(ajax用) */
            string aRet = "";

            if (inMode == 0)
            {
                //1sec間隔

                /* 電源状態 */
                if (gControlMode == Program.POWER_OFF)
                {
                    aRet = aRet + "POWER_OFF," + "\r\n";
                }
                else
                {
                    aRet = aRet + "POWER_ON," + "\r\n";
                }

                /* 機関車状態 */
                for (int i = 0; i < LocDB.Items.Count; i++)
                {
                    long aFuncStatus = 0;

                    aRet = aRet + "LOC," + LocDB.Items[i].mLocAddr.ToString() + "," +
                        LocDB.Items[i].mCurrentDirection.ToString() + "," +
                        LocDB.Items[i].mCurrentSpeed.ToString() + "," +
                        LocDB.Items[i].mDisplayMaxSpeed.ToString() + ",";


                    for (int j = 0; j < Program.MAX_FUNCTIONNUM; j++)
                    {
                        aFuncStatus = aFuncStatus + (LocDB.Items[i].mFunctionStatus[j] << j);
                    }



                    aRet = aRet + aFuncStatus.ToString() + "\r\n";
                }


                /* アクセサリ状態 */
                int aCount = 0;
                long aAccData = 0;
                aRet = aRet + "ACC,";

                for (int i = 0; i < gAppSettings.mAccList.Count; i++)
                {
                    aAccData = aAccData + (gAppSettings.mAccList[i].mDirection << aCount);
                    aCount++;

                    if (aCount >= 8)
                    {
                        aCount = 0;
                        aRet = aRet + aAccData.ToString() + ",";
                        aAccData = 0;
                    }

                }
                aRet = aRet + "\r\n";

                //S88
                if (gAppSettings.mS88Sensor == true)
                {
                    aRet = aRet + "S88,";

                    for (int i = 0; i < Program.S88_MAX; i++)
                    {
                        aRet = aRet + S88Flags[i].ToString() + ",";
                    }

                    aRet = aRet + "\r\n";
                }


            }
            else if (inMode == 1)
            {
                //起動時、設定変更時

                //Acc
                aRet = aRet + "CFG_ACC_TYPE,";

                for (int i = 0; i < gAppSettings.mAccList.Count; i++)
                {
                    aRet = aRet + gAppSettings.mAccList[i].mType.ToString() + ",";
                }

                aRet = aRet + "\r\n";

                //MAP Size
                aRet = aRet + "CFG_MAP_SIZE," + LayoutMapData.Width.ToString() + "," + LayoutMapData.Height.ToString() + ",\r\n";

                //MAP Image
                aRet = aRet + "CFG_MAP_IMG,";

                for (int i = 0; i < LayoutMapData.GetMaxCount(); i++)
                {
                    aRet = aRet + LayoutMapData.GetLayoutTile(i).ToString() + ",";
                }
                aRet = aRet + "\r\n";

                //MAP Accessory
                aRet = aRet + "CFG_MAP_ACC,";

                for (int i = 0; i < LayoutMapData.GetMaxCount(); i++)
                {
                    aRet = aRet + LayoutMapData.GetLayoutAccNo(i).ToString() + ",";
                }
                aRet = aRet + "\r\n";

                //MAP S88
                aRet = aRet + "CFG_MAP_S88,";

                for (int i = 0; i < LayoutMapData.GetMaxCount(); i++)
                {
                    aRet = aRet + LayoutMapData.GetLayoutS88No(i).ToString() + ",";
                }

                aRet = aRet + "\r\n";

            }
            else
            {
                //何もしない
            }


            return aRet;
        }



        private void SetRecvAnotherStationWeb(string inCommandText)
            {
            /* Web経由で得た指令 */

            string[] aParameters;
            int aAddress;
            string[] aDelimiter = { "(", ",", ")" };
            int aSpeed;
            int aCalcAddress;

            if (inCommandText == "")
            {
                return;
            }

            aParameters = inCommandText.Split(aDelimiter, StringSplitOptions.None);

            if (aParameters == null)
            {
                return;
            }

            if (aParameters[0] == "setPower")
            {
                if (((aParameters[1] == "0") && (gControlMode == Program.POWER_ON)) ||
                    ((aParameters[1] == "1") && (gControlMode == Program.POWER_OFF)) )
                {
                    /* STOPが押された状態と同じにする（電源オンのとき） */
                    /* STARTが押された状態と同じにする（電源オフのとき） */
                    ClickPower();
                }

            }
            else if (aParameters[0] == "setLocoSpeed")
            {
                aAddress = DSCommon.ParseStrToInt(aParameters[1]);
                aSpeed = DSCommon.ParseStrToInt(aParameters[2]);

                /* 更新 */
                UpdateLocList(aAddress, aSpeed, -1, -1, -1);

            }
            else if (aParameters[0] == "setLocoDirection")
            {
                aAddress = DSCommon.ParseStrToInt(aParameters[1]);

                /* 更新 */
                UpdateLocList(aAddress, -1, DSCommon.ParseStrToInt(aParameters[2]), -1, -1);

            }
            else if (aParameters[0] == "setLocoFunction")
            {
                aAddress = DSCommon.ParseStrToInt(aParameters[1]);

                /* 更新 */
                UpdateLocList(aAddress, -1, -1, DSCommon.ParseStrToInt(aParameters[2]), DSCommon.ParseStrToInt(aParameters[3]));

            }
            else if (aParameters[0] == "setTurnout")
            {
                aAddress = DSCommon.ParseStrToInt(aParameters[1]);

                if (gAppSettings.mDCCMode == true)
                {
                    //DCC Accessories
                    aCalcAddress = aAddress - Program.DCCACCADDRESS;
                }
                else
                {
                    //MM2 Accessories
                    aCalcAddress = aAddress - Program.MM2ACCADDRESS;
                }

                /* 更新 */
                UpdateAccList(aCalcAddress, DSCommon.ParseStrToInt(aParameters[2]));

            }            

        }

        private void linkLabel_Url_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {


        }

        private void Button_WebApp_Click(object sender, EventArgs e)
        {

            //Web app
            UrlForm aForm = new UrlForm();

            aForm.Text = LangManager.SetText("TxtWebApp", "Web App") + " URL";

            aForm.linkLabel1.Text = AppServer.UrlPath;
            aForm.linkLabel2.Text = AppServer.HostnamePath;


            aForm.ShowDialog(this);

            //押されたボタン別の処理 
            if (aForm.DialogResult == System.Windows.Forms.DialogResult.OK)
            {

            }

            //フォームの解放 
            aForm.Dispose();


        }

        private void button_LocDBLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog aForm = new OpenFileDialog();
            aForm.CheckFileExists = true;
            aForm.Filter = "Locomotive database file(*.xml)|*.xml|All files(*.*)|*.*";

            //押されたボタン別の処理 
            if (aForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                LocDB.LoadFromFile_New(aForm.FileName);

                UpdateLocDisplay();

            }

            aForm.Dispose();
        }

        private void button_LocDBSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog aForm = new SaveFileDialog();
            aForm.OverwritePrompt = true;
            aForm.Filter = "Locomotive database file(*.xml)|*.xml|All files(*.*)|*.*";

            //押されたボタン別の処理 
            if (aForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                LocDB.SaveToFile_New(aForm.FileName);
            }

            aForm.Dispose();
        }

        private void buttonEventInProcess_Click(object sender, EventArgs e)
        {
            //イベント監視　中止・実行の切り替え

            if (S88Manager.InProgress == true)
            {
                //実行中の時は停止

                buttonEventInProcess.Text = LangManager.SetText("TxtEventSuspended", "Suspended");
                S88Manager.InProgress = false;

            }
            else
            {
                //停止中の時は実行
                buttonEventInProcess.Text = LangManager.SetText("TxtEventInProgress", "In process");
                S88Manager.InProgress = true;
            }



        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            /* スリープイベントを削除 */
            Microsoft.Win32.SystemEvents.PowerModeChanged -= new Microsoft.Win32.PowerModeChangedEventHandler(this.MainForm_PowerModeChanged);

        }

        private void tabPanels_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tabS88Events_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button_LayoutCfg_Click(object sender, EventArgs e)
        {
            SizeInput aForm = new SizeInput();

            aForm.numUpDown_Width.Value = LayoutMapData.Width;
            aForm.numUpDown_Height.Value = LayoutMapData.Height;

            if (aForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                LayoutMapData.Resize(decimal.ToInt32(aForm.numUpDown_Width.Value), decimal.ToInt32(aForm.numUpDown_Height.Value));

                /* レイアウト更新 */
                LayoutBox.Width = LayoutMapData.Width * Program.TILE32_SIZE * ScaleRation / 100;
                LayoutBox.Height = LayoutMapData.Height * Program.TILE32_SIZE * ScaleRation / 100;
                LayoutBox.Refresh();
            }



        }

        private void button_S88ScriptReset_Click(object sender, EventArgs e)
        {
            //
            S88Manager.ResetEvents();

            MessageBox.Show("All events are resetted.");

        }

    }
    

}

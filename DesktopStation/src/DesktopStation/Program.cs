using System;
using System.Windows.Forms;

namespace DesktopStation
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            bool createdNew;

            System.Threading.Mutex mutex =
                new System.Threading.Mutex(true, "DesktopStaion20130501", out createdNew);
            if (createdNew == false)
            {
                MessageBox.Show("Do not run multiple starts!");
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());

            mutex.ReleaseMutex();
        }


        public const int POWER_ON = 1;
        public const int POWER_OFF = 0;
        public const int ACCESSORIES_MAX = 320 + 1;
        public const int ACCESSORIES_MAX_DCC = 2048 + 1;

        public const int ACCESSORIES_IDXWIDTH = 8;
        public const int ACCESSORIES_IDXHEIGHT = 5;
        public const int ACCESSORIES_ITEMWIDTH = 110;
        public const int ACCESSORIES_ITEMHEIGHT = 84;

        public const int REPLY_WAITING = -1;
        public const int REPLY_OK = 0;
        public const int REPLY_ERROR = 1;
        public const int REPLY_READY = 2;
        public const int REPLY_TIMEOUT = 3;
        public const int REPLY_GWERROR = 4;


        public const int MFXADDRESS = 0x4000;
        public const int DCCADDRESS = 0xC000;
        public const int MM2ACCADDRESS = 0x2FFF;
        public const int DCCACCADDRESS = 0x37FF;

        public const int PROTCOL_MM2 = 0;
        public const int PROTCOL_MFX = 1;
        public const int PROTCOL_DCC = 2;

        public const int SPEED_MAX = 1024;
        public const int SPEED_INCREMENTAL = 8;
        public const int SPEED_MULVAL = 3;
        public const int LOC_MAX = 50;
        public const int S88_MAX = 16;
        public const int S88EVENT_MAX = 26;
        public const int DIRECTION_FWD = 1;//1
        public const int DIRECTION_REV = 2;//2
        public const int PEN_WIDTH = 4;
        public const int PEN_WIDTH_SLM = 2;
        public const int PEN_WIDTH_DBL = 6;
        public const int METER_LINEWIDTH0 = 12;
        public const int METER_LINEWIDTH1 = 20;
        public const int METER_LINEWIDTH2 = 28;
        public const int MAX_FUNCTIONNUM = 28 + 1;
        public const int LEVER_BAR_TOP = 15;
        public const int LEVER_BAR_LEFT = 30;
        public const int LEVER_BAR_WIDTH = 20;
        public const int LEVER_HEIGHT = 280;
        public const int LEVER_PIN_LEFT = 35;
        public const int LEVER_PIN_WIDTH = 48;
        public const int LEVER_PIN_PENWIDTH = 20;
        public const int LEVER_STEP_WIDTH = 34;
        public const int DIAL_RADIUS_MAX = 22;
        public const int DIAL_RADIUS_MIN = 5;

        /* Cabのマルチファンクション・アクセサリ操作用 */
        public const int MULTIFNC_ACC_TOP = 6;
        public const int MULTIFNC_ACC_LEFT = 8;
        public const int MULTIFNC_ACC_WIDTH = 40;
        public const int MULTIFNC_ACC_HEIGHT = 32;
        public const int MULTIFNC_ACC_ITEMWIDTH = 16;
        public const int MULTIFNC_ACC_ITEMHEIGHT = 32;

        public const int RIGHTBAR_NONE = 0;
        public const int RIGHTBAR_CLOCK = 1;
        public const int RIGHTBAR_EMG = 2;
        public const int BOTTOMBAR_NONE = 0;
        public const int BOTTOMBAR_ACC = 1;
        public const int BOTTOMBAR_S88 = 2;

 
        public const int KEYMAP_PPLUS = (int)Keys.Z;
        public const int KEYMAP_PMINUS = (int)Keys.A;
        public const int KEYMAP_FREE = (int)Keys.X;
        public const int KEYMAP_BPLUS = (int)Keys.OemPeriod;
        public const int KEYMAP_BMINUS = (int)Keys.Oemcomma;
        public const int KEYMAP_EMERGENCY = (int)Keys.OemQuestion;
        public const int KEYMAP_UP = (int)Keys.PageUp;
        public const int KEYMAP_DOWN = (int)Keys.PageDown;
        public const int KEYMAP_TOGGLEDIRECTION = (int)Keys.Home;
        public const int KEYMAP_FUNC01 = (int)Keys.F1;
        public const int KEYMAP_FUNC02 = (int)Keys.F2;
        public const int KEYMAP_FUNC03 = (int)Keys.F3;
        public const int KEYMAP_FUNC04 = (int)Keys.F4;
        public const int KEYMAP_FUNC05 = (int)Keys.F5;
        public const int KEYMAP_FUNC06 = (int)Keys.F6;
        public const int KEYMAP_FUNC07 = (int)Keys.F7;
        public const int KEYMAP_FUNC08 = (int)Keys.F8;
        public const int KEYMAP_FUNC09 = (int)Keys.F9;
        public const int KEYMAP_FUNC10 = (int)Keys.F10;
        public const int KEYMAP_FUNC11 = (int)Keys.F11;
        public const int KEYMAP_FUNC12 = (int)Keys.F12;

        public const int SCREEN_SIZE1_WIDTH = 930;
        public const int SCREEN_SIZE1_HEIGHT = 474;
        public const int SCREEN_SIZE2_WIDTH = (SCREEN_SIZE1_WIDTH * 2);
        public const int SCREEN_SIZE2_HEIGHT = (SCREEN_SIZE1_HEIGHT * 2);

        public const int TILE32_SIZE = 32;
        public const int TILE32_LAYOUT_WIDTH = 26;
        public const int TILE32_LAYOUT_HEIGHT = 14;
        public const int TILE32_DIPLAYMAX = 28;
        public const int SCRIPTMODE_RUN = 1;
        public const int SCRIPTMODE_STOP = 0;
        public const int SCRIPTMODE_TEACH = 2;
        public const String SCRIPTCMD_WAIT = "WAIT";
        public const String SCRIPTCMD_WAITIF = "WAITIF";
        public const String SCRIPTCMD_WAITRND = "WAITRND";
        public const String SCRIPTCMD_SPEED = "SPEED";
        public const String SCRIPTCMD_DIRECTION = "DIRECTION";
        public const String SCRIPTCMD_FUNCTION = "FUNCTION";
        public const String SCRIPTCMD_ACCESSORY = "ACCESSORY";
        public const String SCRIPTCMD_POWER = "POWER";
        public const String SCRIPTCMD_EXIT = "EXIT";
        public const String SCRIPTCMD_GOTO = "GOTO";
        public const String SCRIPTCMD_SETFLAG = "SETFLAG";
        public const String SCRIPTCMD_LABEL = "LABEL";
        public const String SCRIPTCMD_INCFLAG = "INCFLAG";
        public const String SCRIPTCMD_RUNFILE = "RUNFILE";
        public const String SCRIPTCMD_SETROUTE = "SETROUTE";
        public const String SCRIPTCMD_GOTOIF = "GOTOIF";

        //今後、削除予定
        public const String SCRIPTCMD_JUMPS88 = "JUMPS88";
        public const String SCRIPTCMD_JUMPRUN = "JUMPRUN";
        public const String SCRIPTCMD_JUMPSTOP = "JUMPSTOP";
        public const String SCRIPTCMD_JUMPROUTE = "JUMPROUTE";
        public const String SCRIPTCMD_JUMP = "JUMP";


        public const String SERIALCMD_LOCSPEED = "setLocoSpeed";
        public const String SERIALCMD_LOCDIRECTION = "setLocoDirection";
        public const String SERIALCMD_LOCFUNCTION = "setLocoFunction";
        public const String SERIALCMD_LOCACCEL = "accelerateLoco";
        public const String SERIALCMD_LOCDEACCEL = "decelerateLoco";
        public const String SERIALCMD_TGLDIRECTION = "toggleLocoDirection";
        public const String SERIALCMD_TGLFUNCTION = "toggleLocoFunction";
        public const String SERIALCMD_TURNOUT = "setTurnout";
        public const String SERIALCMD_PING = "setPing";
        public const String SERIALCMD_POWER = "setPower";
        public const String SERIALCMD_SETCV = "setLocoConfig";
        public const String SERIALCMD_GETCV = "getLocoConfig";
        public const String SERIALCMD_GETS88 = "getS88";
        public const String SERIALCMD_MFXDISCOVERY = "mfxDiscovery";
        public const String SERIALCMD_MFXBIND = "mfxBind";
        public const String SERIALCMD_MFXVERIFY = "mfxVerify";        


        public const int MULTICONTROL_MAX = 8;

        public const int DOUBLEHEADING_MAX = 4;

        public const int COUNTTIMER_PING = 50;
        public const int COUNTTIMER_S88 = 2;
        public const int SCRIPTVALUE_MAX = 50;

        public const int PANELUPDATE_NONE = 0;
        public const int PANELUPDATE_LOC = 1;
        public const int PANELUPDATE_ACC = 2;

        public const String FILE_S88EVENT = "\\Resources\\S88Events.xml";
        public const String FILE_LAYOUT = "\\Resources\\Default.map";
        public const String FILE_EXECUTE = "\\Resources\\Execute.dat";
        public const String FOLDER_ICON = "\\Resources\\Icons\\";
        public const String FOLDER_LANGUAGE = "\\Resources\\language\\";

        

        

    }
}

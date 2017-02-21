using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System.Windows.Forms;

namespace DesktopStation
{
    public delegate int getScriptValue_delegate(int inNo);

    public class S88EventWaitItem
    {
        public bool Enable;
        public String Expression;

        public String LeftExpText;
        public String RightExpText;
        public String CenterExpText;
        
        public int LeftExp;
        public int RightExp;
        public int CenterExp;

        public int LeftExpType;
        public int RightExpType;
        public int CenterExpType;

        public S88EventWaitItem()
        {
            Enable = false;
            Expression = "";
        }

        public void SetExpression(String inExp)
        {
            Expression = inExp;
            String[] aTexts = inExp.Split(new string[] { "==", "!=", ">=", "<=", ">", "<" }, StringSplitOptions.RemoveEmptyEntries);
            if (aTexts.Length < 2)
            {
                MessageBox.Show("Error! Argument number is wrong. " + inExp);
                Enable = false;
                return;
            }

            LeftExpText = aTexts[0];
            CenterExpText = GetExpCondition(inExp, out CenterExp);
            RightExpText = aTexts[1];

            GetType(LeftExpText, out LeftExpType, out LeftExp);
            GetType(RightExpText, out RightExpType, out RightExp);
        }

        public String GetExpCondition(String inExp, out int outExpCondition)
        {
            if (inExp.Contains("==") == true)
            {
                outExpCondition = 1;
                return "==";
            }
            else if (inExp.Contains("!=") == true)
            {
                outExpCondition = 2;
                return "!=";
            }
            else if (inExp.Contains(">=") == true)
            {
                outExpCondition = 3;
                return ">=";
            }
            else if (inExp.Contains("<=") == true)
            {
                outExpCondition = 4;
                return "<=";
            }
            else if (inExp.Contains(">") == true)
            {
                outExpCondition = 5;
                return ">";
            }
            else if (inExp.Contains("<") == true)
            {
                outExpCondition = 6;
                return "<";
            }
            else
            {
                outExpCondition = 0;
                MessageBox.Show("Error! Argument condition is wrong. " + inExp);
                Enable = false;
                return "";
            }
        }

        public void GetType(String inCmd, out int outType, out int outNo)
        {
            int aRet = 0;

            String[] aTexts = inCmd.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);

            if (aTexts.Length <= 1)
            {
                outType = 100;
                Int32.TryParse(inCmd, out outNo);
            }
            else
            {
                if (aTexts[0] == "FLAG")
                {
                    aRet = 1;
                }
                else if (aTexts[0] == "LSPD")
                {
                    aRet = 2;
                }
                else if (aTexts[0] == "LDIR")
                {
                    aRet = 3;
                }
                else if (aTexts[0] == "ACC")
                {
                    aRet = 4;
                }
                else if (aTexts[0] == "ROUTE")
                {
                    aRet = 5;
                }
                else if (aTexts[0] == "S88")
                {
                    aRet = 6;
                }
                else if (aTexts[0] == "SSPD")
                {
                    aRet = 7;
                    aTexts[1] = ConvertSlotNo(aTexts[1]);
                }
                else if (aTexts[0] == "SDIR")
                {
                    aRet = 8;
                    aTexts[1] = ConvertSlotNo(aTexts[1]);
                }
                else
                {
                    MessageBox.Show("Wrong command: " + aTexts[0]);
                    aRet = 0;
                    Enable = false;
                }

                outType = aRet;

                Int32.TryParse(aTexts[1], out outNo);
            }
        }


        public String ConvertSlotNo(String inText)
        {
            String aText = "0";

            if (inText.ToUpper() == "A")
            {
                aText = "1";
            }
            else if (inText.ToUpper() == "B")
            {
                aText = "2";
            }
            else if (inText.ToUpper() == "C")
            {
                aText = "3";
            }
            else if (inText.ToUpper() == "D")
            {
                aText = "4";
            }
            else if (inText.ToUpper() == "E")
            {
                aText = "5";
            }
            else if (inText.ToUpper() == "F")
            {
                aText = "6";
            }
            else if (inText.ToUpper() == "G")
            {
                aText = "7";
            }
            else if (inText.ToUpper() == "H")
            {
                aText = "8";
            }

            return aText;
        }

        public void Interval()
        {
			// ignore
        }
    }

    public class S88EventCfgItem
    {
        public string mAreaName;
        public int mSensorAddress;
        //public int mLocAddress;
        public List<ScriptData> Items;
        public bool mAvailable;
        public int TriggerType;
        public int mIntervalTime;
        public int mTriggerTime;
        public int mLocIndex;
        public int mSlotIndex;
        public int mTriggerSpeed_Stop;
        public int mTriggerSpeed_Run;
        public int mFlagNo;
        public int mFlagValue;
        public int mFlagOperator;
        public int mRouteNo;

        public S88EventCfgItem()
        {
            mAreaName = "";
            mSensorAddress = 0;
            //mLocAddress = 0;
            mAvailable = false;
            TriggerType = 0;
            mIntervalTime = 0;
            mTriggerTime = 0;
            mLocIndex = -1;
            mSlotIndex = 0;
            mFlagNo = 0;
            mFlagValue = 0;
            mFlagOperator = 0;
            mRouteNo = 0;

            Items = new List<ScriptData>();
        }
    } /* S88EventCfgItem */

    public class S88EventCfg
    {
        public S88EventCfgItem[] Events;

        public S88EventCfg()
        {
            int i;
            Events = new S88EventCfgItem[Program.S88EVENT_MAX];
            for (i = 0; i < Program.S88EVENT_MAX; i++)
                Events[i] = new S88EventCfgItem();
        }
    } /* S88EventCfg */

    public class S88Event
    {
        public delegate void setScriptValues_delegate(int inNo, int inValue);
        public delegate void writeS88ProcessLog_delegate(String inEventName, String inTrigger, int inLineNo, String inLog);
        
        public string EventName;
        public int mSensorAddress;
        public List<ScriptData> Items;
        //public bool mDCCMode;
        public bool mAvailable;
        public int ScriptMode;
        public bool Run;
        public int TriggerType;
        public int mIntervalTime;
        public int mTriggerTime;
        public int mRandomTime;
        public DateTime mPreviousTime;
        public int mTriggerSpeed_Stop;
        public int mTriggerSpeed_Run;
        public int mRouteNo;
        public int mFlagNo;
        public int mFlagValue;
        public int mFlagOperator;
        public int mLocIndex;
        public int mSlotIndex;

        private int lineNo;
        private int waitTime;

        private S88EventWaitItem waitFunc;

        private RailuinoSerial serialCmd;
        private getScriptValue_delegate getScriptValue;
        private setScriptValues_delegate setScriptValues;
        private UpdateLocList_delegate updateLocList;
        private UpdateAccList_delegate updateAccList;
        private writeS88ProcessLog_delegate writeS88ProcessLog;
        private runFile_delegate runFile;
        private setTransitionSpeed_delegate setTransitionSpeed;
        private getLocItemFromIndex_delegate getLocItemFromIndex;
        private getS88Value_delegate getS88Value;
        private checkRoute_delegate checkRoute;
        private setRoute_delegate setRoute;
        private getAccValue_delegate getAccItem;

        public S88Event(RailuinoSerial inSerialCmd, getScriptValue_delegate inGetScriptValue, setScriptValues_delegate inSetScriptValue, 
            UpdateLocList_delegate inUpdateLocList, UpdateAccList_delegate inUpdateAccList, writeS88ProcessLog_delegate inWriteS88ProcessLog, 
            runFile_delegate inRunFile, setTransitionSpeed_delegate inSetTransitionSpeed, getLocItemFromIndex_delegate inGetLocItemFromIndex, 
            getS88Value_delegate inGetS88Value, checkRoute_delegate inCheckRoute, setRoute_delegate inSetRoute, getAccValue_delegate inGetAccItem)
        {
            EventName = "";
            mSensorAddress = 0;
            mLocIndex = -1;
            mSlotIndex = 0;
            mAvailable = false;
            Items = new List<ScriptData>();
            mTriggerSpeed_Stop = 0;
            mTriggerSpeed_Run = 512;
            waitFunc = new S88EventWaitItem();

            mRouteNo = 0;
            mFlagNo = 0;
            mFlagValue = 0;
            mFlagOperator = 0;

            serialCmd = inSerialCmd;
            getScriptValue = inGetScriptValue;
            setScriptValues = inSetScriptValue;
            updateLocList = inUpdateLocList;
            updateAccList = inUpdateAccList;
            writeS88ProcessLog = inWriteS88ProcessLog;
            runFile = inRunFile;
            setTransitionSpeed = inSetTransitionSpeed;
            getLocItemFromIndex = inGetLocItemFromIndex;
            getS88Value = inGetS88Value;

            checkRoute = inCheckRoute;
            setRoute = inSetRoute;

            getAccItem = inGetAccItem;
               

            ScriptMode = Program.SCRIPTMODE_STOP;
            Run = false;

        }

        public void StartScript()
        {
            /* スクリプト処理開始 */
            ScriptMode = Program.SCRIPTMODE_RUN;
            waitTime = 0;
            lineNo = 0;
            waitFunc.Enable = false;
        }

        public void ExitScript()
        {
            /* スクリプト処理停止 */
            ScriptMode = Program.SCRIPTMODE_STOP;

            lineNo = 0;
            Run = false;

        }

        /* 定周期でループ動作する処理 */
        public void RunScriptMain()
        {
            bool aBreak;

            //強制停止用
            if (Run == false)
            {
                return;
            }

            if (ScriptMode != Program.SCRIPTMODE_RUN)
            {
                return;
            }

            if ((waitTime <= 0) && (waitFunc.Enable == false))
            {
                while (lineNo < Items.Count)
                {

                    aBreak = ProcessScript(Items[lineNo]);

                    /* 次の行へ */
                    lineNo = lineNo + 1;

                    if (aBreak == true)
                    {
                        break;
                    }
                }

                if( ScriptMode == Program.SCRIPTMODE_RUN)
                {
                }

                if ((Items.Count <= lineNo) && (Items.Count > 0 ))
                {
                    if (Items[Items.Count - 1].mCommand != Program.SCRIPTCMD_EXIT)
                    {
                        ExitScript();

                        /* ログ */
                        writeS88ProcessLog(EventName, getTriggerText(), 9999, "Force Exit");
                    }

                    ExitScript();
                }

            }
            else
            {
                if (waitFunc.Enable == true)
                {
                    IntervalWaitIf();

                }
                else
                {
                    /* 待機する */
                    waitTime = waitTime - 1;
                }
            }
        }

        private int GetWaitIfValue(int inType, int inNo)
        {
            int aRet = 0;
            LocData aLocData;

            switch( inType)
            {
                case 1:
                    //FLAG
                    aRet = getScriptValue(inNo);
                    break;

                case 2:
                    //LSPD
                    aLocData = getLocItemFromIndex(-1, -1, inNo);

                    if (aLocData != null)
                    {
                        aRet = (int)aLocData.mCurrentSpeed;
                    }
                    else
                    {
                        aRet = 0;
                    }

                    break;

                case 3:
                    //LDIR
                    aLocData = getLocItemFromIndex(-1, -1, inNo);

                    if (aLocData != null)
                    {
                        aRet = (int)aLocData.mCurrentDirection;
                    }
                    else
                    {
                        aRet = 0;
                    }
                    break;

                case 4:
                    //ACC
                    aRet = getAccItem(inNo);
                    break;

                case 5:
                    //ROUTE
                    aRet = (checkRoute(inNo, false) == true) ? 1 : 0;
                    break;

                case 6:
                    //S88
                    aRet = getS88Value(inNo);
                    break;

                case 7:
                    //SSPD
                    aLocData = getLocItemFromIndex(-1, inNo, -1);

                    if (aLocData != null)
                    {
                        aRet = (int)aLocData.mCurrentSpeed;
                    }
                    else
                    {
                        aRet = 0;
                    }

                    break;

                case 8:
                    //SDIR
                    aLocData = getLocItemFromIndex(-1, inNo, -1);

                    if (aLocData != null)
                    {
                        aRet = (int)aLocData.mCurrentDirection;
                    }
                    else
                    {
                        aRet = 0;
                    }
                    break;
                case 100:
                    //生値
                    aRet = inNo;
                    break;

            }


            return aRet;
        }

        private bool GetIfExpression(S88EventWaitItem inExprItem)
        {
            /* 条件成立までウェイトする処理の監視 */

            int aLeftResult = 0;
            int aRightResult = 0;
            bool aResult = false;

            //左側の数字を出す
            aLeftResult = GetWaitIfValue(inExprItem.LeftExpType, inExprItem.LeftExp);

            //右側の数字を出す
            aRightResult = GetWaitIfValue(inExprItem.RightExpType, inExprItem.RightExp);



            //左右を指定方式で比較する
            switch (inExprItem.CenterExp)
            {
                case 1:
                    // ==

                    if (aLeftResult == aRightResult)
                    {
                        aResult = true;
                    }
                    break;

                case 2:
                    // !=

                    if (aLeftResult != aRightResult)
                    {
                        aResult = true;
                    }
                    break;

                case 3:
                    // >=

                    if (aLeftResult >= aRightResult)
                    {
                        aResult = true;
                    }
                    break;

                case 4:
                    // <=

                    if (aLeftResult <= aRightResult)
                    {
                        aResult = true;
                    }
                    break;

                case 5:
                    // >

                    if (aLeftResult > aRightResult)
                    {
                        aResult = true;
                    }
                    break;

                case 6:
                    // <

                    if (aLeftResult < aRightResult)
                    {
                        aResult = true;
                    }
                    break;
            }

            return aResult;
        }

        private void IntervalWaitIf()
        {

            bool aResult = GetIfExpression(waitFunc);

            //最終処理
            if (aResult == true)
            {
                //条件成立。ウェイトを終了する。
                waitFunc.Enable = false;
            }

        }

        private String getTriggerText()
        {
            String aText = "";

            switch (TriggerType)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    aText = "SAd." + mSensorAddress.ToString();
                    break;
                case 4:
                    aText = "Int." + (mIntervalTime / (60 * 60)).ToString("D2") + ":" + ((mIntervalTime / 60) % 60).ToString("D2") + ":" + (mIntervalTime % 60).ToString("D2");
                    break;
                case 5:
                    aText = "Clk." + (mTriggerTime / (60 * 60)).ToString("D2") + ":" + ((mTriggerTime / 60) % 60).ToString("D2") + ":" + (mTriggerTime % 60).ToString("D2");
                    break;
                case 6:
                    aText = "Rand.";
                    break;

                case 7:
                    aText = "SpdTh(Run) " + mTriggerSpeed_Run.ToString();
                    break;
                case 8:
                    aText = "SpdTh(Stop) " + mTriggerSpeed_Stop.ToString();
                    break;
                case 9:
                    aText = "Flag " + mFlagNo.ToString() + "==" + mFlagValue.ToString();
                    break;
                case 10:
                    aText = "Route " + mRouteNo.ToString();
                    break;
                case 11:
                    aText = "Startup";
                    break;
            }

            return aText;
        }

        

        private int checkSlot(String inText)
        {

            if (inText.Contains("SLOT.A") == true)
            {
                return 1;

            }
            else if (inText.Contains("SLOT.B") == true)
            {
                return 2;

            }
            else if (inText.Contains("SLOT.C") == true)
            {
                return 3;

            }
            else if (inText.Contains("SLOT.D") == true)
            {
                return 4;

            }
            else if (inText.Contains("SLOT.E") == true)
            {
                return 5;

            }
            else if (inText.Contains("SLOT.F") == true)
            {
                return 6;

            }
            else if (inText.Contains("SLOT.G") == true)
            {
                return 7;

            }
            else if (inText.Contains("SLOT.H") == true)
            {
                return 8;

            }
            else
            {
                return -1;
             }



        }

        private void setLocAddress(String inAddrText, out int outAddress, out int outProtcol, out int outSpeedRatio, out LocData outLocData)
        {
            int aSlotCheck;
            int aLocAddress;
            int aProtocol = 0;
            int aSpeedRatio = 1024;
            LocData aLocData;


            aSlotCheck = checkSlot(inAddrText);

            if (aSlotCheck < 0)
            {
                aLocAddress = convertParam(inAddrText);

                /* 選択された機関車を使用する */
                if (aLocAddress <= 0)
                {
                    aLocData = getLocItemFromIndex(mLocIndex, mSlotIndex, -1);
                    if (aLocData != null)
                    {
                        aLocAddress = aLocData.mLocAddr;
                        aProtocol = aLocData.mLocSpeedstep;
                        aSpeedRatio = aLocData.mLocMaxSpeed + 1;
                    }
                }
                else
                {
                    aLocData = getLocItemFromIndex(-1, -1, aLocAddress);

                    if (aLocData != null)
                    {
                        aProtocol = aLocData.mLocSpeedstep;
                    }
                    else
                    {
                        aProtocol = 0;//速度ステッププロトコル固定(DCC28 or MM2-14)
                    }
                }
            }
            else
            {
                /* スロット指定の場合 */
                aLocData = getLocItemFromIndex(-1, aSlotCheck, -1);

                if (aLocData != null)
                {
                    aLocAddress = aLocData.mLocAddr;
                    aProtocol = aLocData.mLocSpeedstep;
                    aSpeedRatio = aLocData.mLocMaxSpeed + 1;
                }
                else
                {
                    aLocAddress = 0;
                }
            }

            outAddress = aLocAddress;
            outLocData = aLocData;
            outProtcol = aProtocol;
            outSpeedRatio = aSpeedRatio;                


        }

        private bool ProcessScript(ScriptData inData)
        {
            bool aResult = false;
            int aTemp;
            int aLocAddress;
            int aLineNo;
            int aProtocol = 0;
            int aSpeedRatio = 1024;
            int aFound = 0;
            LocData aLocData;



            if (inData.mCommand == Program.SCRIPTCMD_WAIT)
            {
                waitTime = convertParam(inData.mParam1);
                aResult = true;

                /* ログ */
                writeS88ProcessLog(EventName, getTriggerText(), lineNo, inData.mCommand + "(" + inData.mParam1 + ")");
            }


            if (inData.mCommand == Program.SCRIPTCMD_WAITRND)
            {
                Random cRandom = new System.Random();

                //ランダムウェイト
                waitTime = cRandom.Next(convertParam(inData.mParam1));

                aResult = true;

                /* ログ */
                writeS88ProcessLog(EventName, getTriggerText(), lineNo, inData.mCommand + "(" + inData.mParam1 + ")");
            }

            if (inData.mCommand == Program.SCRIPTCMD_WAITIF)
            {
                waitTime = 1;
                waitFunc.Enable = true;
                waitFunc.SetExpression(inData.mParam1);

                aResult = true;

                /* ログ */
                writeS88ProcessLog(EventName, getTriggerText(), lineNo, inData.mCommand + "(" + inData.mParam1 + ")");
            }


            

            if (inData.mCommand == Program.SCRIPTCMD_SPEED)
            {

                setLocAddress(inData.mParam1, out aLocAddress, out aProtocol, out aSpeedRatio, out aLocData);

                if ((aLocData == null) && (aLocAddress <= 0))
                {
                    /* 異常系 */
                    /* ログ */
                    writeS88ProcessLog(EventName, getTriggerText(), lineNo, inData.mCommand + "(Addr Err," + inData.mParam2 + "," + inData.mParam3.ToString() + ")");

                }
                else
                {
                    if ((inData.mParam3 == 0) || (aLocData == null))
                    {
                        serialCmd.SetLocoSpeed(aLocAddress, (convertParam(inData.mParam2) * aSpeedRatio) >> 10, aProtocol);

                        /* 管理リストに反映 */
                        updateLocList(aLocAddress, (convertParam(inData.mParam2) * aSpeedRatio) >> 10, -1, -1, -1);

                        /* 重連運転想定 */
                        if (aLocData != null)
                        {
                            if (getAddress(aLocData.mLocAddr_dbl) > 0)
                            {
                                serialCmd.SetLocoSpeed(aLocData.mLocAddr_dbl, (convertParam(inData.mParam2) * aSpeedRatio) >> 10, aLocData.mDoubleLoc[0].mLocSpeedstep);
                                /* 管理リストに反映 */
                                updateLocList(aLocData.mLocAddr_dbl, (convertParam(inData.mParam2) * aSpeedRatio) >> 10, -1, -1, -1);

                            }
                        }
                    }
                    else
                    {
                        waitTime = inData.mParam3;

                        setTransitionSpeed(aLocAddress, (convertParam(inData.mParam2) * aSpeedRatio) >> 10, waitTime);

                        /* 管理リストには自動で反映 */
                    }

                    /* ログ */
                    writeS88ProcessLog(EventName, getTriggerText(), lineNo, inData.mCommand + "(" + aLocAddress.ToString() + "," + inData.mParam2 + "," + inData.mParam3.ToString() + ")");
                }

                aResult = true;

            }

            if (inData.mCommand == Program.SCRIPTCMD_DIRECTION)
            {
                setLocAddress(inData.mParam1, out aLocAddress, out aProtocol, out aSpeedRatio, out aLocData);

                if ((aLocData == null) && (aLocAddress <= 0))
                {
                    /* 異常系 */
                    /* ログ */
                    writeS88ProcessLog(EventName, getTriggerText(), lineNo, inData.mCommand + "(Addr Err," + inData.mParam2 + ")");
                }
                else
                {
                    serialCmd.SetLocoDirection(aLocAddress, convertParam(inData.mParam2));
                    /* 管理リストに反映 */
                    updateLocList(aLocAddress, -1, convertParam(inData.mParam2), -1, -1);


                    /* 重連運転想定 */
                    if (aLocData != null)
                    {
                        if (getAddress(aLocData.mLocAddr_dbl) > 0)
                        {
                            serialCmd.SetLocoDirection(aLocData.mLocAddr_dbl, convertParam(inData.mParam2));
                            /* 管理リストに反映 */
                            updateLocList(aLocData.mLocAddr_dbl, -1, convertParam(inData.mParam2), -1, -1);
                        }
                    }

                    aResult = true;

                    /* ログ */
                    writeS88ProcessLog(EventName, getTriggerText(), lineNo, inData.mCommand + "(" + aLocAddress.ToString() + "," + inData.mParam2 + ")");
                }
            }

            if (inData.mCommand == Program.SCRIPTCMD_FUNCTION)
            {
                setLocAddress(inData.mParam1, out aLocAddress, out aProtocol, out aSpeedRatio, out aLocData);

                aResult = true;

                if ((aLocData == null) && (aLocAddress <= 0))
                {
                    /* 異常系 */

                    /* ログ */
                    writeS88ProcessLog(EventName, getTriggerText(), lineNo, inData.mCommand + "(Addr Err," + inData.mParam2 + "," + inData.mParam3 + ")");

                }
                else
                {
                    /* 正常系 */
                    serialCmd.SetLocoFunction(aLocAddress, convertParam(inData.mParam2), inData.mParam3);

                    /* 管理リストに反映 */
                    updateLocList(aLocAddress, -1, -1, convertParam(inData.mParam2), inData.mParam3);
                    /* ログ */
                    writeS88ProcessLog(EventName, getTriggerText(), lineNo, inData.mCommand + "(" + aLocAddress.ToString() + "," + inData.mParam2 + "," + inData.mParam3 + ")");
                }

            }

            if (inData.mCommand == Program.SCRIPTCMD_ACCESSORY)
            {
                serialCmd.SetTurnout(convertParam(inData.mParam1), convertParam(inData.mParam2));

                /* 管理リストに反映 */
                updateAccList(convertParam(inData.mParam1), convertParam(inData.mParam2));

                aResult = true;

                /* ログ */
                writeS88ProcessLog(EventName, getTriggerText(), lineNo, inData.mCommand + "(" + inData.mParam1 + "," + inData.mParam2 + ")");
            }

            if (inData.mCommand == Program.SCRIPTCMD_POWER)
            {
                serialCmd.SetPower(convertParam(inData.mParam1));

                /* ログ */
                writeS88ProcessLog(EventName, getTriggerText(), lineNo, inData.mCommand + "(" + inData.mParam1 + ")");
            }

            if (inData.mCommand == Program.SCRIPTCMD_EXIT)
            {
                /* ログ */
                writeS88ProcessLog(EventName, getTriggerText(), lineNo, inData.mCommand + "()");

                //スクリプト終了
                ExitScript();

                /* 処理中断 */
                aResult = true;

            }

            if (inData.mCommand == Program.SCRIPTCMD_GOTO)
            {
                /* 行番号を退避 */
                aLineNo = lineNo;

                if (DSCommon.IsNumeric(inData.mParam1) == true)
                {

                    lineNo = convertParam(inData.mParam1) - 1;

                    /* ログ */
                    writeS88ProcessLog(EventName, getTriggerText(), aLineNo, inData.mCommand + "(" + inData.mParam1 + ")");
                }
                else
                {
                    /* 指定ラベルへジャンプする */
                    aTemp = searchLabel(inData.mParam1);
                    if (aTemp >= 0)
                    {
                        lineNo = aTemp - 1;
                    }

                    /* ログ */
                    writeS88ProcessLog(EventName, getTriggerText(), aLineNo, inData.mCommand + "(" + inData.mParam1 + " -> " + lineNo.ToString() + ")");

                }


            }

            if (inData.mCommand == Program.SCRIPTCMD_SETFLAG)
            {
                setScriptValues(convertParam(inData.mParam1), convertParam(inData.mParam2));
                
                /* ログ */
                writeS88ProcessLog(EventName, getTriggerText(), lineNo, inData.mCommand + "(" + inData.mParam1 + "," + inData.mParam2 + ")");
            }

            /* フラグの値を加算（減算）する */
            if (inData.mCommand == Program.SCRIPTCMD_INCFLAG)
            {
                aTemp = getScriptValue(convertParam(inData.mParam1));

                if (convertParam(inData.mParam2) == 0)
                {
                    aTemp = aTemp + 1;
                }
                else
                {
                    aTemp = aTemp + convertParam(inData.mParam2);
                }

                setScriptValues(convertParam(inData.mParam1), aTemp);
                
                /* ログ */
                writeS88ProcessLog(EventName, getTriggerText(), lineNo, inData.mCommand + "(" + inData.mParam1 + "," + inData.mParam2 + " -> " + aTemp.ToString() + ")");
            }

            if (inData.mCommand == Program.SCRIPTCMD_GOTOIF)
            {
                /* 行番号を退避 */
                aLineNo = lineNo;

                S88EventWaitItem aExpr = new S88EventWaitItem();
                aExpr.SetExpression(inData.mParam2);


                /* If構文の状態に応じて指定ラベルへジャンプする */
                if (GetIfExpression(aExpr) == true)
                {
                    aTemp = searchLabel(inData.mParam1);

                    if (aTemp >= 0)
                    {
                        lineNo = aTemp;
                    }

                    /* ログ */
                    writeS88ProcessLog(EventName, getTriggerText(), aLineNo, inData.mCommand + "(" + inData.mParam1 + "," + inData.mParam2 + ")" + " -> Line." + aTemp.ToString());
                }
                else
                {
                    /* ログ */
                    writeS88ProcessLog(EventName, getTriggerText(), aLineNo, inData.mCommand + "(" + inData.mParam1 + "," + inData.mParam2 + ")");
                }               
                

            }


            if (inData.mCommand == Program.SCRIPTCMD_JUMP)
            {
                /* 行番号を退避 */
                aLineNo = lineNo;

                /* FLAGの状態に応じて指定ラベルへジャンプする */
                if (getScriptValue(convertParam(inData.mParam2)) == inData.mParam3)
                {
                    aTemp = searchLabel(inData.mParam1);

                    if (aTemp >= 0)
                    {
                        lineNo = aTemp;
                    }

                    /* ログ */
                    writeS88ProcessLog(EventName, getTriggerText(), aLineNo, inData.mCommand + "(" + inData.mParam1 + "," + inData.mParam2 + "," + inData.mParam3 + ")" + " -> Line." + aTemp.ToString());
                }
                else
                {
                    /* ログ */
                    writeS88ProcessLog(EventName, getTriggerText(), aLineNo, inData.mCommand + "(" + inData.mParam1 + "," + inData.mParam2 + "," + inData.mParam3 + ")");
                }

            }

            if (inData.mCommand == Program.SCRIPTCMD_JUMPS88)
            {
                /* 行番号を退避 */
                aLineNo = lineNo;

                int aS88status_jump = getS88Value(convertParam(inData.mParam2));

                /* FLAGの状態に応じて指定ラベルへジャンプする */
                if (aS88status_jump == inData.mParam3)
                {
                    aTemp = searchLabel(inData.mParam1);

                    if (aTemp >= 0)
                    {
                        lineNo = aTemp;
                    }

                    /* ログ */
                    writeS88ProcessLog(EventName, getTriggerText(), aLineNo, inData.mCommand + "(" + inData.mParam1 + "," + inData.mParam2 + "," + inData.mParam3 + ")" + " -> Line." + aTemp.ToString());
                }
                else
                {
                    /* ログ */
                    writeS88ProcessLog(EventName, getTriggerText(), aLineNo, inData.mCommand + "(" + inData.mParam1 + "," + inData.mParam2 + "," + inData.mParam3 + ")");
                }

            }

            if (inData.mCommand == Program.SCRIPTCMD_JUMPRUN)
            {
                /* 行番号を退避 */
                aLineNo = lineNo;

                aLocAddress = convertParam(inData.mParam2);

                /* 選択された機関車を使用する */
                aLocData = getLocItemFromIndex(mLocIndex, mSlotIndex, aLocAddress);

                if (aLocData != null)
                {
                    if (aLocData.CurrentSpeed() >= inData.mParam3)
                    {
                        aTemp = searchLabel(inData.mParam1);

                        if (aTemp >= 0)
                        {
                            lineNo = aTemp;
                        }

                        aFound = 1;
                    }

                }


                /* FLAGの状態に応じて指定ラベルへジャンプする */
                if (aFound == 1)
                {
                    /* ログ */
                    writeS88ProcessLog(EventName, getTriggerText(), aLineNo, inData.mCommand + "(" + inData.mParam1 + "," + inData.mParam2 + "," + inData.mParam3 + ")" + " -> Line." + lineNo.ToString());
                }
                else
                {
                    /* ログ */
                    writeS88ProcessLog(EventName, getTriggerText(), aLineNo, inData.mCommand + "(" + inData.mParam1 + "," + inData.mParam2 + "," + inData.mParam3 + ")");
                }

            }

            if (inData.mCommand == Program.SCRIPTCMD_JUMPSTOP)
            {
                /* 行番号を退避 */
                aLineNo = lineNo;

                aLocAddress = convertParam(inData.mParam2);

                /* 選択された機関車を使用する */
                aLocData = getLocItemFromIndex(mLocIndex, mSlotIndex, aLocAddress);

                if (aLocData != null)
                {
                    if (aLocData.CurrentSpeed() < inData.mParam3)
                    {
                        aTemp = searchLabel(inData.mParam1);

                        if (aTemp >= 0)
                        {
                            lineNo = aTemp;
                        }

                        aFound = 1;
                    }

                }


                /* FLAGの状態に応じて指定ラベルへジャンプする */
                if (aFound == 1)
                {
                    /* ログ */
                    writeS88ProcessLog(EventName, getTriggerText(), aLineNo, inData.mCommand + "(" + inData.mParam1 + "," + inData.mParam2 + "," + inData.mParam3 + ")" + " -> Line." + lineNo.ToString());
                }
                else
                {
                    /* ログ */
                    writeS88ProcessLog(EventName, getTriggerText(), aLineNo, inData.mCommand + "(" + inData.mParam1 + "," + inData.mParam2 + "," + inData.mParam3 + ")");
                }

            }

            if (inData.mCommand == Program.SCRIPTCMD_JUMPROUTE)
            {
                /* 行番号を退避 */
                aLineNo = lineNo;

                bool aState = (inData.mParam3 == 0) ? false : true;


                /* 設定したルートと合致する場合 */
                if (checkRoute(convertParam(inData.mParam2), false) == aState)
                {

                    aTemp = searchLabel(inData.mParam1);

                    if (aTemp >= 0)
                    {
                        lineNo = aTemp;
                    }

                    /* ログ */
                    writeS88ProcessLog(EventName, getTriggerText(), aLineNo, inData.mCommand + "(" + inData.mParam1 + "," + inData.mParam2 + "," + inData.mParam3 + ")" + " -> Line." + lineNo.ToString());

                }
                else
                {
                    /* ログ */
                    writeS88ProcessLog(EventName, getTriggerText(), aLineNo, inData.mCommand + "(" + inData.mParam1 + "," + inData.mParam2 + "," + inData.mParam3 + ")");
                }
            }

            if (inData.mCommand == Program.SCRIPTCMD_SETROUTE)
            {

                setRoute(convertParam(inData.mParam1), true);

                /* ログ */
                writeS88ProcessLog(EventName, getTriggerText(), lineNo, inData.mCommand + "(" + inData.mParam1 + ")");
            }


            if (inData.mCommand == Program.SCRIPTCMD_LABEL)
            {
                /* ラベルなので何もしない */

                /* 無限ループ防止用でいったん抜ける */
                aResult = true;
            }

            if (inData.mCommand == Program.SCRIPTCMD_RUNFILE)
            {
                /* ファイル実行 */
                runFile(inData.mParam1, inData.mParam2);

                /* ログ */
                writeS88ProcessLog(EventName, getTriggerText(), lineNo, inData.mCommand + "(" + inData.mParam1 + "," + inData.mParam2 + ")");
            } 


            return aResult;
        }

        private void setLocAddress(string p, out int aLocAddress, out int aProtocol, out int aSpeedRatio)
        {
            throw new NotImplementedException();
        }

        private int convertParam(String inText)
        {
            int aResult = 0;

            if (inText == "")
            {
                return 0;
            }

            Int32.TryParse(inText, out aResult);

            return aResult;
        }

        private int searchLabel(String inLabelName)
        {
            int aResult = -1;
            int i;

            for (i = 0; i < Items.Count; i++)
            {
                if (Items[i].mCommand == Program.SCRIPTCMD_LABEL)
                {
                    if (Items[i].mParam1 == inLabelName)
                    {
                        aResult = i;
                        break;
                    }
                }
            }


            return aResult;
        }

        public int getAddress(int inAddress)
        {
            int aUpdatedAddr;


            if (inAddress >= Program.DCCADDRESS)
            {
                aUpdatedAddr = inAddress - Program.DCCADDRESS;

            }
            else if (inAddress >= Program.MFXADDRESS)
            {
                aUpdatedAddr = inAddress - Program.MFXADDRESS;

            }
            else
            {
                aUpdatedAddr = inAddress;

            }

            /* 16bit分アドレス(MFXフラグを除く)を取得 */
            return aUpdatedAddr;
        }



    } /* S88Event */

    public class S88EventManager
    {
        public S88Event[] mEvents;

        public int Mode;
        public bool InProgress;
        private int[] lastFlags;
        private RailuinoSerial serialCmd;
        private int[] valuesScript;

        private getLocItemFromIndex_delegate getLocItemFromIndex;
        private SetS88RunText_delegate setS88RunText;
        Random RandomFunc;

        public S88EventManager(RailuinoSerial inSerialCmd, UpdateLocList_delegate inUpdateLocList, UpdateAccList_delegate inUpdateAccList, SetS88RunText_delegate inSetS88RunText, 
            runFile_delegate inRunFile, setTransitionSpeed_delegate inSetTransitionSpeed, getLocItemFromIndex_delegate inGetLocItemFromIndex,
            checkRoute_delegate inCheckRoute, setRoute_delegate inSetRoute, getAccValue_delegate inGetAccItem)
        {
            int i;

            mEvents = new S88Event[Program.S88EVENT_MAX];
            lastFlags = new int[Program.S88_MAX];

            valuesScript = new int[Program.SCRIPTVALUE_MAX];
            /* Init values on script.*/
            for (i = 0; i < Program.SCRIPTVALUE_MAX; i++)
            {
                valuesScript[i] = 0;
            }
            
            serialCmd = inSerialCmd;
            setS88RunText = inSetS88RunText;
            getLocItemFromIndex = inGetLocItemFromIndex;

            /* Generate class */
            for (i = 0; i < Program.S88EVENT_MAX; i++)
            {
                mEvents[i] = new S88Event(inSerialCmd, GetScriptValues, setScriptValues, inUpdateLocList, inUpdateAccList, writeS88ProcessLog, inRunFile, inSetTransitionSpeed, inGetLocItemFromIndex, GetS88Values, inCheckRoute, inSetRoute, inGetAccItem);
            }

            RandomFunc = new System.Random();
            InProgress = true;

        }

        public void Clear()
        {
            int i;

            /* Init values on script.*/
            for (i = 0; i < Program.SCRIPTVALUE_MAX; i++)
            {
                valuesScript[i] = 0;
            }

            /* S88センサのバッファも初期化 */
            for (i = 0; i < Program.S88_MAX; i++)
            {
                /* 最初からセンサ上に機関車がいる場合を想定して初期値は0にしない。 */
                lastFlags[i] = -1;
            }

        }

        public void ResetEvents()
        {
            Clear();

            for (int i = 0; i < Program.S88EVENT_MAX; i++)
            {
                //実行中の時
                if (mEvents[i].Run == true)
                {
                    //停止処理
                    mEvents[i].Run = false;

                    //スクリプト強制終了
                    mEvents[i].ExitScript();
                }

            }

        }

        public int GetSensorLastFlag(int inNo)
        {
            return lastFlags[inNo];
        }

        public int GetScriptValues(int inNo)
        {
            if ((inNo < Program.SCRIPTVALUE_MAX) && (inNo >= 0))
            {
                return valuesScript[inNo];
            }
            else
            {
                return 0;
            }
        }

        private void setScriptValues(int inNo, int inValue)
        {
            if ((inNo < Program.SCRIPTVALUE_MAX) && (inNo >= 0))
            {
                valuesScript[inNo] = inValue;
            } 

        }

        public int GetS88Values(int inNo)
        {
            int aFlagHigh;
            int aFlagLow;
            
            if ((inNo < (Program.S88_MAX * 16)) && (inNo >= 0))
            {
                aFlagHigh = inNo / 16;
                aFlagLow = inNo % 16;

                if (lastFlags[aFlagHigh] == -1)
                {
                    return 0;
                }
                else
                {
                    return (lastFlags[aFlagHigh] >> aFlagLow) & 0x01;
                }
            }
            else
            {
                return 0;
            }
        }
      
        public bool UpdateCheck(int[] inFlags)
        {
            int i;
            int aUpdateNums = 0;
 

            for (i = 0; i < Program.S88_MAX; i++)
            {
                /* 前回値を保存する */
                if ((lastFlags[i] != inFlags[i]) && (lastFlags[i] >= 0))
                {
                    aUpdateNums = aUpdateNums + 1;
                }
            }

            return (aUpdateNums > 0 ? true : false);
        }
       
        public bool IntervalCheck(int[] inFlags)
        {
            int i;
            int aCheckFlag;
            int aUpdateNums = 0;
            int aFlagHigh;
            int aFlagLow;
            int aFlagLast;
            int aFlagCurrent;

            for (i = 0; i < Program.S88EVENT_MAX; i++)
            {
                /* センサアドレスが有効で、イベント動作フラグONのとき */
                if ((mEvents[i].mSensorAddress > 0) && (mEvents[i].mAvailable == true) && (mEvents[i].Run == false))
                {
                    int aSensorAddress = mEvents[i].mSensorAddress - 1;

                    /* センサアドレスから、センサスレーブNoとビット位置に換算する */
                    aFlagHigh = aSensorAddress / 16;
                    aFlagLow = aSensorAddress % 16;

                    /* 前回値が-1(初期値)ではないとき */
                    if (lastFlags[aFlagHigh] >= 0)
                    {
                        /* 新旧のフラグ値のみを取得 */
                        aFlagLast = (lastFlags[aFlagHigh] >> aFlagLow) & 0x01;
                        aFlagCurrent = (inFlags[aFlagHigh] >> aFlagLow) & 0x01;

                        aCheckFlag = aFlagCurrent - aFlagLast;

                        // 進入のとき
                        if ((aCheckFlag > 0) && (mEvents[i].TriggerType == 0))
                        {
                            mEvents[i].StartScript();
                            mEvents[i].Run = true;

                        }
                        else if ((aCheckFlag < 0) && (mEvents[i].TriggerType == 1))
                        {
                            // Negative Edgeのとき
                            mEvents[i].StartScript();
                            mEvents[i].Run = true;

                        }

                        /* 変化チェック用（表示関連） */
                        if (aCheckFlag != 0)
                        {
                            aUpdateNums = aUpdateNums + 1;
                        }
                    }
                }
            }

            for (i = 0; i < Program.S88_MAX; i++)
            {
                /* 前回値を保存する */
                lastFlags[i] = inFlags[i];
            }

            /* 返す値は変化のあったイベントの数 */
            return (aUpdateNums > 0 ? true : false);
        }
      
        public bool IntervalCheckTime(int[] inFlags, DateTime inCurrentTime, LocomotiveDB inLocDB, RouteList inRouteList)
        {
            int i;
            int aUpdateNums = 0;
            int aFlagHigh;
            int aFlagLow;
            int aFlagCurrent;
            int aLocIndex;
            double aCurrentSpeed = 0;
            TimeSpan aDifference;


            for (i = 0; i < Program.S88EVENT_MAX; i++)
            {
                /* センサアドレスが有効で、イベント動作フラグONのとき */
                if ((mEvents[i].TriggerType >= 2) && (mEvents[i].mAvailable == true) && (mEvents[i].Run == false))
                {

                    switch (mEvents[i].TriggerType)
                    {

                        case 2:
                        case 3:
                            /* 在線チェック */

                            /* センサアドレスから、センサスレーブNoとビット位置に換算する */
                            aFlagHigh = mEvents[i].mSensorAddress / 16;
                            aFlagLow = mEvents[i].mSensorAddress - aFlagHigh * 16 - 1;
                            aFlagCurrent = (inFlags[aFlagHigh] >> aFlagLow) & 0x01;

                            if( (mEvents[i].TriggerType == 2) && (aFlagCurrent == 1) )
                            {
                                /* 在線しているとき */
                                mEvents[i].StartScript();
                                mEvents[i].Run = true;

                                /* 変化チェック用（表示関連） */
                                aUpdateNums = aUpdateNums + 1;
                            }
                            else if ((mEvents[i].TriggerType == 3) && (aFlagCurrent == 0) )
                            {
                                /* 在線していないとき */
                                mEvents[i].StartScript();
                                mEvents[i].Run = true;

                                /* 変化チェック用（表示関連） */
                                aUpdateNums = aUpdateNums + 1;
                            }
     
                            break;

                        case 4:
                            /* Intervalチェック */

                            if (mEvents[i].mPreviousTime.CompareTo(DateTime.MinValue) == 0)
                            {
                                mEvents[i].mPreviousTime = inCurrentTime;
                            }

                            aDifference = inCurrentTime - mEvents[i].mPreviousTime;

                            if (aDifference.TotalSeconds >= mEvents[i].mIntervalTime)
                            {
                                mEvents[i].mPreviousTime = inCurrentTime;

                                mEvents[i].StartScript();
                                mEvents[i].Run = true;

                                /* 変化チェック用（表示関連） */
                                aUpdateNums = aUpdateNums + 1;
                            }
                            break;

                        case 5:
                            /* Clockチェック */

                            int aTemp = (inCurrentTime.Hour * 60 * 60) + (inCurrentTime.Minute * 60) + (inCurrentTime.Second);

                            if (Math.Abs(mEvents[i].mTriggerTime - aTemp) <= 0)
                            {
                                mEvents[i].StartScript();
                                mEvents[i].Run = true;
                            }

                            /* 変化チェック用（表示関連） */
                            aUpdateNums = aUpdateNums + 1;

                            break;
                        case 6:
                            /* Random Intervalチェック */

                            if (mEvents[i].mPreviousTime.CompareTo(DateTime.MinValue) == 0)
                            {
                                mEvents[i].mPreviousTime = inCurrentTime;
                                mEvents[i].mRandomTime = RandomFunc.Next(mEvents[i].mIntervalTime);
                                
                            }

                            aDifference = inCurrentTime - mEvents[i].mPreviousTime;

                            if (aDifference.TotalSeconds >= mEvents[i].mRandomTime)
                            {
                                mEvents[i].StartScript();
                                mEvents[i].Run = true;
                                mEvents[i].mPreviousTime = inCurrentTime;
                                mEvents[i].mRandomTime = RandomFunc.Next(mEvents[i].mIntervalTime);

                                /* 変化チェック用（表示関連） */
                                aUpdateNums = aUpdateNums + 1;


                            }


                            break;

                        case 7:
                        case 8:
                            /* 車両運転状況チェック（走行時） */

                            aLocIndex = mEvents[i].mLocIndex;
                            aCurrentSpeed = -1;

                            if ((aLocIndex < 0) && (mEvents[i].mSlotIndex > 0))
                            {
                                //スロットから取得
                                LocData aLocData = getLocItemFromIndex(mEvents[i].mLocIndex, mEvents[i].mSlotIndex, -1);

                                if (aLocData != null)
                                {
                                    aCurrentSpeed = aLocData.mCurrentSpeed;
                                }
                                else
                                {
                                    aCurrentSpeed = -1;
                                }
                            }
                            else if (aLocIndex >= 0)
                            {
                                //スロットを設定していない時
                                aCurrentSpeed = inLocDB.Items[aLocIndex].mCurrentSpeed;
                            }
                            else
                            {
                                //スロットも機関車設定もしていない時
                                aCurrentSpeed = -1;
                            }

                            if (aCurrentSpeed >= 0)
                            {
                                /*速度監視*/
                                if ( ((aCurrentSpeed >= mEvents[i].mTriggerSpeed_Run) && (mEvents[i].TriggerType == 7)) ||
                                    ((aCurrentSpeed <= mEvents[i].mTriggerSpeed_Stop) && (mEvents[i].TriggerType == 8)))

                                {
                                    //イベント実行
                                    mEvents[i].StartScript();
                                    mEvents[i].Run = true;
                                    /* 変化チェック用（表示関連） */
                                    aUpdateNums = aUpdateNums + 1;
                                }


                            }
                            break;
                    
                        case 9:
                            //Flagチェック

                            if (mEvents[i].mFlagValue == GetScriptValues(mEvents[i].mFlagNo))
                            {
                                //イベント実行
                                mEvents[i].StartScript();
                                mEvents[i].Run = true;
                                /* 変化チェック用（表示関連） */
                                aUpdateNums = aUpdateNums + 1;
                            }

                            break;

                        case 10:
                            //ルートチェック
                            if( inRouteList.CheckRoute(mEvents[i].mRouteNo, false) == true)
                            {
                                //イベント実行
                                mEvents[i].StartScript();
                                mEvents[i].Run = true;


                                /* 変化チェック用（表示関連） */
                                aUpdateNums = aUpdateNums + 1;
                            }


                            break;


                        case 11:
                            //スタート時
                            //ここには記載しない
                            break;

                    }

                }

            }

            /* 返す値は変化のあったイベントの数 */
            return (aUpdateNums > 0 ? true : false);
        }

        public void StartupRun()
        {
            int i;

            for (i = 0; i < Program.S88EVENT_MAX; i++)
            {
                /* センサアドレスが有効で、イベント動作フラグONのとき */
                if ((mEvents[i].TriggerType >= 2) && (mEvents[i].mAvailable == true) && (mEvents[i].Run == false))
                {
                    switch (mEvents[i].TriggerType)
                    {
                        case 11:
                            //スタート時

                            //イベント実行
                            mEvents[i].StartScript();
                            mEvents[i].Run = true;

                            break;

                    }
                }
            }

        }

        /// <summary> 100ms 周期スクリプト実行 </summary>
        public void IntervalRun()
        {
            int i;

            /* 実行許可を確認 */
            if (InProgress == false)
            {
                for (i = 0; i < Program.S88EVENT_MAX; i++)
                {
                    //実行中の時
                    if (mEvents[i].Run == true)
                    {
                        //停止処理
                        mEvents[i].Run = false;

                        //スクリプト強制終了
                        mEvents[i].ExitScript();
                    }
                }
                
                return;
            }

            /* 実行処理 */
            for (i = 0; i < Program.S88EVENT_MAX; i++)
            {
                /* センサアドレスが有効で、イベント動作フラグONのとき */
                if (mEvents[i].Run == true)
                {

                    mEvents[i].RunScriptMain();

                    /* STOP check */
                    if (mEvents[i].ScriptMode == Program.SCRIPTMODE_STOP)
                    {
                        mEvents[i].Run = false;
                        mEvents[i].ExitScript();
                    }
                 }
            }
        }

        public void SaveToFile(String inFilename)
        {
            int i, j;

            //いったん保存領域に展開する
            S88EventCfg aTempCfg = new S88EventCfg();

            for (i = 0; i < Program.S88EVENT_MAX; i++)
            {
                aTempCfg.Events[i].mSensorAddress = mEvents[i].mSensorAddress;
                aTempCfg.Events[i].mAreaName = mEvents[i].EventName;
                aTempCfg.Events[i].mAvailable = mEvents[i].mAvailable;
                aTempCfg.Events[i].mLocIndex = mEvents[i].mLocIndex;
                aTempCfg.Events[i].mSlotIndex = mEvents[i].mSlotIndex;

                aTempCfg.Events[i].TriggerType = mEvents[i].TriggerType;
                aTempCfg.Events[i].mIntervalTime = mEvents[i].mIntervalTime;
                aTempCfg.Events[i].mTriggerTime = mEvents[i].mTriggerTime;
                aTempCfg.Events[i].mTriggerSpeed_Run = mEvents[i].mTriggerSpeed_Run;
                aTempCfg.Events[i].mTriggerSpeed_Stop = mEvents[i].mTriggerSpeed_Stop;
                aTempCfg.Events[i].mFlagNo = mEvents[i].mFlagNo;
                aTempCfg.Events[i].mFlagValue = mEvents[i].mFlagValue;
                aTempCfg.Events[i].mFlagOperator = mEvents[i].mFlagOperator;
                aTempCfg.Events[i].mRouteNo = mEvents[i].mRouteNo;
               

                for (j = 0; j < mEvents[i].Items.Count; j++)
                {
                    ScriptData aItem = new ScriptData();
                    aItem.mCommand = mEvents[i].Items[j].mCommand;
                    aItem.mParam1 = mEvents[i].Items[j].mParam1;
                    aItem.mParam2 = mEvents[i].Items[j].mParam2;
                    aItem.mParam3 = mEvents[i].Items[j].mParam3;
                    aItem.mParam4 = mEvents[i].Items[j].mParam4;
                    aTempCfg.Events[i].Items.Add(aItem);

                }
            }

            //XmlSerializerを呼び出す
            XmlSerializer serializer = new XmlSerializer(typeof(S88EventCfg));
            //ファイルを作る
            FileStream fs = new FileStream(inFilename, FileMode.Create);
            //書き込み
            serializer.Serialize(fs, aTempCfg);  //sclsはSampleClassのインスタンス名
            //ファイルを閉じる
            fs.Close();
        }

        public bool LoadFromFile(String inFilename)
        {
            bool retVal = false;
            int i, j;

            /* 初期化 */
            for (i = 0; i < Program.S88EVENT_MAX; i++)
            {
                mEvents[i].Items.Clear();
                mEvents[i].EventName = "";
                mEvents[i].mSensorAddress = 0;
                mEvents[i].mLocIndex = -1;
                mEvents[i].mSlotIndex = 0;
                mEvents[i].mAvailable = false;
                mEvents[i].mTriggerSpeed_Run = 102;
                mEvents[i].mTriggerSpeed_Stop = 102;
                mEvents[i].mRouteNo = 0;
                mEvents[i].mFlagNo = 0;
                mEvents[i].mFlagOperator = 0;
                mEvents[i].mFlagValue = 0;

            }


            //設定ファイルがあるときのみ読み込みします。
            if (File.Exists(inFilename))
            {
                //XmlSerializerを呼び出す
                XmlSerializer serializer = new XmlSerializer(typeof(S88EventCfg));
                //ファイルを開く
                FileStream fs = new FileStream(inFilename, FileMode.Open);

                //読み込み
                S88EventCfg aTempCfg = new S88EventCfg();
                
                try
                {
                    aTempCfg = (S88EventCfg)serializer.Deserialize(fs);
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

                //反映
                for (i = 0; i < Program.S88EVENT_MAX; i++)
                {
                    mEvents[i].mSensorAddress = aTempCfg.Events[i].mSensorAddress;
                    mEvents[i].EventName = aTempCfg.Events[i].mAreaName;
                    mEvents[i].mAvailable = aTempCfg.Events[i].mAvailable;
                    mEvents[i].mLocIndex = aTempCfg.Events[i].mLocIndex;
                    mEvents[i].mSlotIndex = aTempCfg.Events[i].mSlotIndex;
                    mEvents[i].TriggerType = aTempCfg.Events[i].TriggerType;
                    mEvents[i].mIntervalTime = aTempCfg.Events[i].mIntervalTime;
                    mEvents[i].mTriggerTime = aTempCfg.Events[i].mTriggerTime;
                    mEvents[i].mTriggerSpeed_Run = aTempCfg.Events[i].mTriggerSpeed_Run;
                    mEvents[i].mTriggerSpeed_Stop = aTempCfg.Events[i].mTriggerSpeed_Stop;
                    mEvents[i].mFlagNo = aTempCfg.Events[i].mFlagNo;
                    mEvents[i].mFlagValue = aTempCfg.Events[i].mFlagValue;
                    mEvents[i].mFlagOperator = aTempCfg.Events[i].mFlagOperator;
                    mEvents[i].mRouteNo = aTempCfg.Events[i].mRouteNo;


                    for (j = 0; j < aTempCfg.Events[i].Items.Count; j++)
                    {
                        ScriptData aItem = new ScriptData();
                        aItem.mCommand = aTempCfg.Events[i].Items[j].mCommand;
                        aItem.mParam1 = aTempCfg.Events[i].Items[j].mParam1;
                        aItem.mParam2 = aTempCfg.Events[i].Items[j].mParam2;
                        aItem.mParam3 = aTempCfg.Events[i].Items[j].mParam3;
                        aItem.mParam4 = aTempCfg.Events[i].Items[j].mParam4;
                        mEvents[i].Items.Add(aItem);

                    }
                }


                retVal = true;
            }
            else
            {
                /* 初期化は完了済 */
            }


            return retVal;
        }

        private void writeS88ProcessLog(String inEventName, String inTrigger, int inLineNo, String inLog)
        {
            setS88RunText("[" + inEventName + "(" + inTrigger + "), " + inLineNo.ToString() + "] " + inLog);
        }
    } /* S88EventManager */

}

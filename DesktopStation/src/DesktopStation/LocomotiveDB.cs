using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualBasic.FileIO;
using System.IO;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace DesktopStation
{
    public class LocomotiveDB
    {
        public List<LocData> Items;

        public bool Transitioning;


        public LocomotiveDB()
        {
            Transitioning = false;


            Items = new List<LocData>();
        }

        public void LoadFromFile_New(String inFileName)
        {

            //設定ファイルがあるときのみ読み込みします。
            if (File.Exists(inFileName))
            {
                //XmlSerializerを呼び出す
                XmlSerializer serializer = new XmlSerializer(typeof(List<LocData>));
                //ファイルを開く
                FileStream fs = new FileStream(inFileName, FileMode.Open);

                //読み込み
                List<LocData> aTempList = new List<LocData>();

                try
                {
                    aTempList = (List<LocData>)serializer.Deserialize(fs);
                }
                catch (System.Exception ex)
                {
                    //ファイルを閉じる
                    fs.Close();

                    //メッセージを表示する
                    MessageBox.Show("Unknown file. (" + ex.Message + ")", "Event File Open Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }


                //ファイルを閉じる
                fs.Close();

                Items.Clear();

                for (int i = 0; i < aTempList.Count; i++)
                {
                    /* NULLチェック */
                    if (aTempList[i].mExFunctionCommand == null)
                    {
                        aTempList[i].mExFunctionCommand = new String[Program.MAX_FUNCTIONNUM + 1];
                    }
                    if (aTempList[i].mExFunctionData == null)
                    {
                        aTempList[i].mExFunctionData = new String[Program.MAX_FUNCTIONNUM + 1];
                    }
                    if (aTempList[i].mFunctionExMethod == null)
                    {
                        aTempList[i].mFunctionExMethod = new int[Program.MAX_FUNCTIONNUM + 1];
                    }
                    if (aTempList[i].mFunctionExAddress == null)
                    {
                        aTempList[i].mFunctionExAddress = new int[Program.MAX_FUNCTIONNUM + 1];
                    }
                    if (aTempList[i].mFunctionExFuncNo == null)
                    {
                        aTempList[i].mFunctionExFuncNo = new int[Program.MAX_FUNCTIONNUM + 1];
                    }

                    aTempList[i].mCurrentDirection = Program.DIRECTION_FWD;
                    aTempList[i].mCurrentSpeed = 0;

                    for(int j = 0; j < Program.MAX_FUNCTIONNUM; j++)
                    {
                        aTempList[i].mFunctionStatus[j] = 0;

                    }

                    if (aTempList[i].mDoubleLoc == null)
                    {
                        aTempList[i].mDoubleLoc = new DoubleHeadingLocUnit[Program.DOUBLEHEADING_MAX];

                        for (int k = 0; k < Program.DOUBLEHEADING_MAX; k++)
                        {
                            aTempList[i].mDoubleLoc[k] = new DoubleHeadingLocUnit(); 
                        }

                    }


                    Items.Add(aTempList[i]);
                }

            }

            /* 何も登録されていないときはダミーデータを登録する */
            if (Items.Count == 0)
            {
                LocData aItem = new LocData();
                aItem.mLocAddr = 3;
                aItem.mLocName = "DB ICE3";
                aItem.mLocItemNo = 57305;
                aItem.mLocManufacture = "PIKO";
                aItem.mDisplayMaxSpeed = 240;
                aItem.mSpeedAccRatio = 3;
                aItem.mSpeedRedRatio = 3;
                aItem.mFunctionImageTable = new int[Program.MAX_FUNCTIONNUM];
                aItem.mFunctionStatus = new int[Program.MAX_FUNCTIONNUM];
                aItem.mExFunctionData = new String[Program.MAX_FUNCTIONNUM + 1];
                aItem.mExFunctionCommand = new String[Program.MAX_FUNCTIONNUM + 1];
                aItem.mFunctionExMethod = new int[Program.MAX_FUNCTIONNUM + 1];
                aItem.mFunctionExAddress = new int[Program.MAX_FUNCTIONNUM + 1];
                aItem.mFunctionExFuncNo = new int[Program.MAX_FUNCTIONNUM + 1];
                aItem.mLocMaxSpeed = Program.SPEED_MAX;
                aItem.mComment = "";

                /* 追加 */
                Items.Add(aItem);
            }

        }

        public void SaveToFile_New(String inFilename)
        {

            //いったん保存領域に展開する
            S88EventCfg aTempCfg = new S88EventCfg();

            //XmlSerializerを呼び出す
            XmlSerializer serializer = new XmlSerializer(typeof(List<LocData>));
            //ファイルを作る
            FileStream fs = new FileStream(inFilename, FileMode.Create);
            //書き込み
            serializer.Serialize(fs, Items);  //sclsはSampleClassのインスタンス名
            //ファイルを閉じる
            fs.Close();
        }

        public void LoadFromFile(String inFileName)
        {

            String[] aFields;
            int i, j;
            String aGotFncTbl;
            String aNum;

            //設定ファイルがあるときのみ読み込みします。
            if (File.Exists(inFileName))
            {

                TextFieldParser aParser = new TextFieldParser(inFileName);
                aParser.TextFieldType = FieldType.Delimited;
                aParser.SetDelimiters(",");

                while (aParser.EndOfData == false)
                {

                    /* 分析処理する */
                    LocData aItem = new LocData();

                    aFields = aParser.ReadFields();

                    aItem.mLocAddr = GetCSVFieldInt(aFields, 0, 0);
                    aItem.mLocAddr_dbl = GetCSVFieldInt(aFields, 1, 0);
                    aItem.mLocName = GetCSVFieldString(aFields, 2, "");
                    aItem.mLocItemNo = GetCSVFieldInt(aFields, 3, 0);
                    aItem.mLocManufacture = GetCSVFieldString(aFields, 4, "");
                    aGotFncTbl = GetCSVFieldString(aFields, 5, "");
                    aItem.mDisplayMaxSpeed = GetCSVFieldInt(aFields, 6, 300);
                    aItem.mSpeedAccRatio = GetCSVFieldInt(aFields, 7, 3);
                    aItem.mSpeedRedRatio = GetCSVFieldInt(aFields, 8, 3);
                    aItem.mLocMaxSpeed = GetCSVFieldInt(aFields, 9, Program.SPEED_MAX);
                    aItem.mComment = GetCSVFieldString(aFields, 10, "");
                    aItem.mIconFile = GetCSVFieldString(aFields, 11, "");
                    aItem.mMFXUID = (UInt32)GetCSVFieldUInt(aFields, 12, 0);

                    aItem.mFunctionImageTable = new int[Program.MAX_FUNCTIONNUM + 1];
                    aItem.mFunctionStatus = new int[Program.MAX_FUNCTIONNUM + 1];
                    aItem.mExFunctionData = new String[Program.MAX_FUNCTIONNUM + 1];
                    aItem.mExFunctionCommand = new String[Program.MAX_FUNCTIONNUM + 1];
                    aItem.mFunctionExMethod = new int[Program.MAX_FUNCTIONNUM + 1];
                    aItem.mFunctionExAddress = new int[Program.MAX_FUNCTIONNUM + 1];
                    aItem.mFunctionExFuncNo = new int[Program.MAX_FUNCTIONNUM + 1];

                    if (aGotFncTbl != "")
                    {
                        j = 0;
                        i = 0;
                        aNum = "";

                        while (j < aGotFncTbl.Length)
                        {
                            if (aGotFncTbl[j] == '.')
                            {
                                aItem.mFunctionImageTable[i] = ParseStrToInt(aNum);
                                aNum = "";
                                i = i + 1;
                            }
                            else
                            {
                                aNum = aNum + aGotFncTbl[j];
                            }

                            /* 1文字進める */
                            j = j + 1;

                        }
                    }

                    /* 追加 */
                    Items.Add(aItem);
                }

                aParser.Close();

            }

            /* 何も登録されていないときはダミーデータを登録する */
            if (Items.Count == 0)
            {
                LocData aItem = new LocData();
                aItem.mLocAddr = 3 + Program.DCCADDRESS;
                aItem.mLocName = "DB ICE3";
                aItem.mLocItemNo = 57305;
                aItem.mLocManufacture = "PIKO";
                aItem.mDisplayMaxSpeed = 240;
                aItem.mLocSpeedstep = 0;
                aItem.mSpeedAccRatio = 3;
                aItem.mSpeedRedRatio = 3;
                aItem.mFunctionImageTable = new int[Program.MAX_FUNCTIONNUM];
                aItem.mFunctionStatus = new int[Program.MAX_FUNCTIONNUM];
                aItem.mExFunctionData = new String[Program.MAX_FUNCTIONNUM + 1];
                aItem.mExFunctionCommand = new String[Program.MAX_FUNCTIONNUM + 1];
                aItem.mFunctionExMethod = new int[Program.MAX_FUNCTIONNUM + 1];
                aItem.mFunctionExAddress = new int[Program.MAX_FUNCTIONNUM + 1];
                aItem.mFunctionExFuncNo = new int[Program.MAX_FUNCTIONNUM + 1];
                aItem.mLocMaxSpeed = Program.SPEED_MAX;
                aItem.mComment = "";

                /* 追加 */
                Items.Add(aItem);
            }
        }

        public void SaveToFile(String inFileName)
        {
            int i;
            int j;
            String aFncTbl;

            //ファイルを作る
            StreamWriter aStrWriter = new StreamWriter(inFileName, false);

            for (i = 0; i < Items.Count; i++)
            {
                aFncTbl = "";

                /* Function Table生成 */
                for (j = 0; j < Program.MAX_FUNCTIONNUM; j++)
                {
                    aFncTbl = aFncTbl + Items[i].mFunctionImageTable[j].ToString() + ".";
                }

                /* 書き込み */
                aStrWriter.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},\"{10}\",\"{11}\",{12}", Items[i].mLocAddr, Items[i].mLocAddr_dbl, Items[i].mLocName, Items[i].mLocItemNo, Items[i].mLocManufacture, aFncTbl, Items[i].mDisplayMaxSpeed, Items[i].mSpeedAccRatio, Items[i].mSpeedRedRatio, Items[i].mLocMaxSpeed, Items[i].mComment, Items[i].mIconFile, Items[i].mMFXUID);
            }

            aStrWriter.Close();
        }


        private int ParseStrToInt(String aText)
        {
            int aRet;

            Int32.TryParse(aText, out aRet);

            return aRet;

        }

        private UInt32 ParseStrToUInt32(String aText)
        {
            UInt32 aRet;

            UInt32.TryParse(aText, out aRet);

            return aRet;

        }

        private int GetCSVFieldInt(String[] inFields, int inIndex, int inDefault)
        {
            int aResult;

            if (inFields.Length > inIndex)
            {
                aResult = ParseStrToInt(inFields[inIndex]);
            }
            else
            {
                aResult = inDefault;
            }
            return aResult;
        }

        private UInt32 GetCSVFieldUInt(String[] inFields, int inIndex, UInt32 inDefault)
        {
            UInt32 aResult;

            if (inFields.Length > inIndex)
            {
                aResult = ParseStrToUInt32(inFields[inIndex]);
            }
            else
            {
                aResult = inDefault;
            }
            return aResult;
        }

        private String GetCSVFieldString(String[] inFields, int inIndex, String inDefault)
        {
            String aResult;

            if (inFields.Length > inIndex)
            {
                aResult = inFields[inIndex];
            }
            else
            {
                aResult = inDefault;
            }
            return aResult;
        }

        public int SearchLoc(int inLocAddress)
        {
            int i;
            int aIndex = -1;

            for (i = 0; i < Items.Count; i++)
            {
                if (Items[i].mLocAddr == inLocAddress)
                {
                    aIndex = i;

                    break;
                }
            }

            return aIndex;
        }


        public int GetNewMfxAdress()
        {
            int aAddress = 0;
            int i;

            for (i = 0; i < Items.Count; i++)
            {
                if ((GetAddressLocProtcol(Items[i].mLocAddr) == Program.PROTCOL_MFX) && (GetAddress(Items[i].mLocAddr) >= aAddress))
                {
                    aAddress = GetAddress(Items[i].mLocAddr) + 1;
                }
            }

            return Program.MFXADDRESS + aAddress;
        }

        public int SearchExistsMfxLoc(long inMFXUID)
        {
            int i;
            int aLocAddress = -1;

            for (i = 0; i < Items.Count; i++)
            {
                if ((GetAddressLocProtcol(Items[i].mLocAddr) == Program.PROTCOL_MFX) && (Items[i].mMFXUID == inMFXUID))
                {
                    aLocAddress = Items[i].mLocAddr;

                    break;
                }
            }

            return aLocAddress;
        }



        public bool UpdateExistsMfxLoc(long inMFXUID, int inLocAddress)
        {
            int i;
            bool aResult = false;

            for (i = 0; i < Items.Count; i++)
            {
                if ((GetAddressLocProtcol(Items[i].mLocAddr) == Program.PROTCOL_MFX) && (Items[i].mMFXUID == inMFXUID))
                {
                    if (Items[i].mLocAddr != inLocAddress)
                    {
                        Items[i].mLocAddr = inLocAddress;

                        aResult = true;
                    }

                    break;
                }
            }

            return aResult;
        }

        public int GetAddress(int inAddress)
        {
            int aUpdatedAddr;
            int aShiftAddr = inAddress >> 8;


            if (aShiftAddr < 0x04)
	        {
                aUpdatedAddr = inAddress;
            }
            else if ((aShiftAddr >= 0x30) && (aShiftAddr <= 0x33))
	        {
                aUpdatedAddr = inAddress - Program.MM2ACCADDRESS;
            }
            else if ((aShiftAddr >= 0x38) && (aShiftAddr <= 0x3F))
	        {
                aUpdatedAddr = inAddress - Program.DCCACCADDRESS;
            }
            else if ((aShiftAddr >= 0x40) && (aShiftAddr <= 0x70))
	        {
                aUpdatedAddr = inAddress - Program.MFXADDRESS;
            }
            else if ((aShiftAddr >= 0xC0) && (aShiftAddr <= 0xFF))
	        {
                aUpdatedAddr = inAddress - Program.DCCADDRESS;
            }
	        else
	        {
		        return 0;
	        }

            /* 16bit分アドレス(MFXフラグを除く)を取得 */
            return aUpdatedAddr;
        }

        public int GetAddressLocProtcol(int inAddress)
        {

            if (inAddress >= Program.DCCADDRESS)
            {
                return 2;
            }
            else if (inAddress >= Program.MFXADDRESS)
            {
                return 1;
            }
            else
            {
                return 0;
            }

        }

        public int AssignAddressProtcol(int inProtcol, int inTargetAddress)
        {
            int aAddress;

            switch (inProtcol)
            {
                case Program.PROTCOL_MM2:
                    aAddress = inTargetAddress;
                    break;
                case Program.PROTCOL_MFX:
                    aAddress = inTargetAddress + Program.MFXADDRESS;
                    break;
                case Program.PROTCOL_DCC:
                    aAddress = inTargetAddress + Program.DCCADDRESS;
                    break;
                default:
                    aAddress = inTargetAddress;
                    break;
            }

            return aAddress;

        }

        public String GetProtcolName(int inProtcol)
        {
            String aName;

            switch (inProtcol)
            {
                case Program.PROTCOL_MM2:
                    aName = "MM2";
                    break;
                case Program.PROTCOL_MFX:
                    aName = "mfx";
                    break;
                case Program.PROTCOL_DCC:
                    aName = "DCC";
                    break;
                default:
                    aName = "MM2";
                    break;
            }

            return aName;

        }

        public String GetProtcolAddressDescription(int inAddress)
        {
            String aName;

            if (inAddress == 0)
            {
                aName = "AUTO";
            }
            else
            {
                switch (GetAddressLocProtcol(inAddress))
                {
                    case Program.PROTCOL_DCC:
                        aName = GetAddress(inAddress) + " (DCC)";
                        break;
                    case Program.PROTCOL_MM2:
                        aName = GetAddress(inAddress) + " (MM2)";
                        break;
                    case Program.PROTCOL_MFX:
                        aName = GetAddress(inAddress) + " (mfx)";
                        break;
                    default:
                        aName = GetAddress(inAddress) + " (?)";
                        break;
                }
            }

            return aName;
        }



        public bool CheckCurrentSpeed(int inIndex)
        {

            if (inIndex < Items.Count)
            {

                if (Items[inIndex].mCurrentSpeed > Items[inIndex].mLocMaxSpeed)
                {
                    Items[inIndex].mCurrentSpeed = Items[inIndex].mLocMaxSpeed;
                }

                return true;
            }
            else
            {
                return false;
            }

        }

    }

}

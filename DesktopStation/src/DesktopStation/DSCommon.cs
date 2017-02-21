﻿using System;
using System.Windows.Forms;
using System.IO;

namespace DesktopStation
{
    class NotSelectableButton : System.Windows.Forms.Button
    {
        public NotSelectableButton()
        {
            this.SetStyle(ControlStyles.Selectable, false);
        }
    }

    class NotSelectableComboBox : System.Windows.Forms.ComboBox
    {
        public NotSelectableComboBox()
        {
            this.SetStyle(ControlStyles.Selectable, false);
        }
    }

    class NotSelectableCheckBox : System.Windows.Forms.CheckBox
    {
        public NotSelectableCheckBox()
        {
            this.SetStyle(ControlStyles.Selectable, false);
        }
    }

    public static class DSCommon
    {

        public static string ConvertSlotNo(string inText)
        {
            string aText = "0";

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

        public static bool IsNumeric(string inNumText)
        {
            int j;

            bool aResult = int.TryParse(inNumText, out j);

            return aResult;
        }

        public static int ParseStrToIntHex(string inHexText)
        {
            int aNum;

            if (int.TryParse(inHexText, System.Globalization.NumberStyles.HexNumber, null, out aNum) == true)
            {
                return aNum;
            }
            else
            {
                return 0;
            }
        }

        public static uint ParseStrToUInt32Hex(string inHexText)
        {
            uint aNum;

            if (uint.TryParse(inHexText, System.Globalization.NumberStyles.HexNumber, null, out aNum) == true)
            {
                return aNum;
            }
            else
            {
                return 0;
            }
        }

        public static void WaitSleepTime(int in100ms)
        {
            // 時間のかかる処理
            for (int i = 0; i < in100ms; i++)
            {
                // 何らかの処理
                System.Threading.Thread.Sleep(100);

                // メッセージ・キューにあるWindowsメッセージをすべて処理する
                Application.DoEvents();
            }
        }

        public static int ParseStrToInt(string aText)
        {
            int aRet;

            int.TryParse(aText, out aRet);

            return aRet;

        }

        public static int GetCSVFieldInt(string[] inFields, int inIndex, int inDefault)
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

        public static string GetCSVFieldString(string[] inFields, int inIndex, string inDefault)
        {
            string aResult;

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

        public static int MaxMin(int inMin, int inMax, int inValue)
        {
            if (inValue <= inMin)
            {
                return inMin;
            }
            else if (inValue >= inMax)
            {
                return inMax;
            }
            else
            {
                return inValue;
            }
        }

        public static string CopyImageFile(string inFileName, string inStoredPath)
        {
            string aNewFileName = inStoredPath + "\\" + Path.GetFileName(inFileName);

            if (File.Exists(aNewFileName) == true)
            {
                return "";
            }
            else
            {

                if (Directory.Exists(inStoredPath) == false)
                {
                    Directory.CreateDirectory(inStoredPath);
                }

                File.Copy(inFileName, aNewFileName);

                return aNewFileName;
            }

        }

        public static string SetOmitImageFilePathAndName(string inFileName, string inStoredPath)
        {
            if (inFileName == "")
            {
                return "";
            }
            
            if (Path.GetDirectoryName(inFileName) == "")
            {
                return inStoredPath + "\\" + inFileName;
            }
            else
            {
                return inFileName;
            }
        }

        public static string GetOmitImageFileName(string inFileName, string inOmitPath)
        {
            if (inFileName == "")
            {
                return "";
            }

            if (Path.GetDirectoryName(inFileName) == inOmitPath)
            {
                return Path.GetFileName(inFileName);
            }
            else
            {
                return inFileName;
            }
        }

        public static string ConvertToSerialCommand(string inText, bool inDCCMode)
        {
            string[] aParameters;
            int aAddress;
            int aSpeed;
            int aCalcAddress;
            string aResult = "";
            int aErrored = 0;

            if (inText == "")
            {
                return "";
            }

            aParameters = inText.Split(',');

            if (aParameters[0] == "@PWR")
            {
                aResult = Program.SERIALCMD_POWER + "(" + aParameters[1] + ")";
            }
            else if (aParameters[0] == "@SPD")
            {
                if (aParameters.Length >= 5)
                {

                    aAddress = (DSCommon.ParseStrToIntHex(aParameters[1]) << 8) + DSCommon.ParseStrToIntHex(aParameters[2]);
                    aSpeed = (DSCommon.ParseStrToIntHex(aParameters[3]) << 8) + DSCommon.ParseStrToIntHex(aParameters[4]);

                    /* 変換 */
                    aResult = Program.SERIALCMD_LOCSPEED + "(" + aAddress.ToString() + "," + aSpeed.ToString() + ")";
                }
                else
                {
                    aErrored = 1;
                }

            }
            else if (aParameters[0] == "@DIR")
            {
                aAddress = (DSCommon.ParseStrToIntHex(aParameters[1]) << 8) + DSCommon.ParseStrToIntHex(aParameters[2]);

                /* 変換 */
                aResult = Program.SERIALCMD_LOCDIRECTION + "(" + aAddress.ToString() + "," + aParameters[3] + ")";

            }
            else if (aParameters[0] == "@FNC")
            {
                aAddress = (DSCommon.ParseStrToIntHex(aParameters[1]) << 8) + DSCommon.ParseStrToIntHex(aParameters[2]);

                /* 変換 */
                aResult = Program.SERIALCMD_LOCFUNCTION + "(" + aAddress.ToString() + "," + aParameters[3] + "," + aParameters[4] + ")";

            }
            else if (aParameters[0] == "@ACC")
            {
                aAddress = (DSCommon.ParseStrToIntHex(aParameters[1]) << 8) + DSCommon.ParseStrToIntHex(aParameters[2]);

                if (inDCCMode == true)
                {
                    //DCC Accessories
                    aCalcAddress = aAddress - Program.DCCACCADDRESS;
                }
                else
                {
                    //MM2 Accessories
                    aCalcAddress = aAddress - Program.MM2ACCADDRESS;
                }

                /* 変換 */
                aResult = Program.SERIALCMD_TURNOUT + "(" + aCalcAddress.ToString() + "," + aParameters[3] + ")";

            }

            //エラーチェック
            if (aErrored == 1)
            {
                /* エラー */
                MessageBox.Show("Command Error from external application/station");
            }

            return aResult;

        }

    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;

namespace DesktopStation
{

    public class RailuinoSerial
    {
        public delegate void TSetScriptData(String inCommand, int inParam1, int inParam2, int inParam3, int inParam4);
        public delegate void TSetRecvText(String inText);
        public delegate void TSendCommand(String inCommandText);
        TSetScriptData SetScriptData;
        TSetRecvText SetRecvText;
        TSendCommand SendCommand;
        getDCCmodee_delegate getDCCmode;


        public RailuinoSerial(TSendCommand inFuncSend, TSetScriptData inFuncSet, TSetRecvText inFuncRecv, getDCCmodee_delegate inDCCmode)
        {
            SendCommand = new TSendCommand(inFuncSend);
            SetScriptData = new TSetScriptData(inFuncSet);
            SetRecvText = new TSetRecvText(inFuncRecv);
            getDCCmode = inDCCmode;
        }

        public void SetLocoSpeed(int inAddress, int inSpeed, int inSpeedstep)
        {
            String aCommandText;

            int aSpeed = inSpeed;

            if (aSpeed > 1023)
            {
                aSpeed = 1023;
            }
            else if (aSpeed < 0)
            {
                aSpeed = 0;
            }

            if (CheckLocAddress(inAddress))
            {
                aCommandText = Program.SERIALCMD_LOCSPEED + "(" + inAddress.ToString() + "," + aSpeed.ToString() + "," + (inAddress < 0xFF ? 0 : inSpeedstep).ToString() + ")";

                SendCommand(aCommandText);

                //MM2は強制的に速度ステップは無効にする。
                SetScriptData(Program.SCRIPTCMD_SPEED, inAddress, inSpeed, inAddress < 0xFF ? 0 : inSpeedstep, 0);
            }
            else
            {
                //エラー
                Console.Write("Address is out of range (Address0).\n");

            }

        }

        private bool CheckLocAddress(int inAddress)
        {
            int aUpdatedAddr;
            int aShiftAddr = inAddress >> 8;


            if (aShiftAddr < 0x04)
            {
                aUpdatedAddr = inAddress;
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
                aUpdatedAddr = 0;
            }


            return (aUpdatedAddr <= 0) ? false : true;

        }

        public void AccelerateLoco(int inAddress)
        {
            String aCommandText;

            aCommandText = Program.SERIALCMD_LOCACCEL + "(" + inAddress.ToString() + ")";

            SendCommand(aCommandText);
        }

        public void DecelerateLoco(int inAddress)
        {
            String aCommandText;

            aCommandText = Program.SERIALCMD_LOCDEACCEL + "(" + inAddress.ToString() + ")";

            SendCommand(aCommandText);
        }

        public void ToggleLocoDirection(int inAddress)
        {
            String aCommandText;

            aCommandText = Program.SERIALCMD_TGLDIRECTION + "(" + inAddress.ToString() + ")";

            SendCommand(aCommandText);
        }

        public void ToggleLocoFunction(int inAddress, int inFunction)
        {
            String aCommandText;

            aCommandText = Program.SERIALCMD_TGLFUNCTION + "(" + inAddress.ToString() + "," + inFunction.ToString() + ")";

            SendCommand(aCommandText);
        }

        public void SetLocoFunction(int inAddress, int inFunction, int inPower)
        {
            String aCommandText;

            if (CheckLocAddress(inAddress))
            {
                aCommandText = Program.SERIALCMD_LOCFUNCTION + "(" + inAddress.ToString() + "," + inFunction.ToString() + "," + inPower.ToString() + ")";

                SendCommand(aCommandText);
                SetScriptData(Program.SCRIPTCMD_FUNCTION, inAddress, inFunction, inPower, 0);
            }
            else
            {
                //エラー
                Console.Write("Address is out of range (Address0).\n");

            }
        }

        public void SetLocoDirection(int inAddress, int inDirection)
        {
            String aCommandText;

            if (CheckLocAddress(inAddress))
            {
                aCommandText = Program.SERIALCMD_LOCDIRECTION + "(" + inAddress.ToString() + "," + inDirection.ToString() + ")";

                SendCommand(aCommandText);
                SetScriptData(Program.SCRIPTCMD_DIRECTION, inAddress, inDirection, 0, 0);
            }
            else
            {
                //エラー
                Console.Write("Address is out of range (Address0).\n");
            }

        }

        public void SetTurnout(int inAddress, int inDirection)
        {
            String aCommandText;
            int aCalcAddress;

            //プロトコルをAccリストから参照して決定する
            if (getDCCmode(inAddress) == true)
            {
                //DCC Accessories
                aCalcAddress = Program.DCCACCADDRESS + inAddress;
            }
            else
            {
                //MM2 Accessories
                aCalcAddress = Program.MM2ACCADDRESS + inAddress;
            }



            aCommandText = Program.SERIALCMD_TURNOUT + "(" + aCalcAddress.ToString() + "," + inDirection.ToString() + ")";

            SendCommand(aCommandText);
            SetScriptData(Program.SCRIPTCMD_ACCESSORY, inAddress, inDirection, 0, 0);

        }

        /*
        public void GetS88()
        {
            String aCommandText;

            aCommandText = "getS88()";

            SendCommand(aCommandText);

        }
         * */

        public void SetPing()
        {
            String aCommandText;

            aCommandText = Program.SERIALCMD_PING + "()";

            SendCommand(aCommandText);

        }

        public int SetPower(int inPower)
        {
            String aCommandText;

            aCommandText = Program.SERIALCMD_POWER + "(" + inPower.ToString() + ")";

            SendCommand(aCommandText);

            SetScriptData(Program.SCRIPTCMD_POWER, inPower, 0, 0, 0);

            return inPower;

        }

        public void SetLocoConfig(int inAddress, int inCVNo, int inCVValue)
        {
            String aCommandText;

            aCommandText = Program.SERIALCMD_SETCV + "(" + inAddress.ToString() + "," + inCVNo.ToString() + "," + inCVValue.ToString() + ")";

            SendCommand(aCommandText);
        }

        public void SetLocoConfigEx(int inAddress, int inCVNo, int inCVValue, int inMode)
        {
            String aCommandText;

            aCommandText = Program.SERIALCMD_SETCV + "(" + inAddress.ToString() + "," + inCVNo.ToString() + "," + inCVValue.ToString() + "," + inMode.ToString() + ")";

            SendCommand(aCommandText);
        }

        public int GetLocoConfig(int inAddress, int inCVNo)
        {
            String aCommandText;
            int aResult = 0;

            aCommandText = Program.SERIALCMD_GETCV + "(" + inAddress.ToString() + "," + inCVNo.ToString() + ")";

            SendCommand(aCommandText);

            /* 取得待ち */



            /* 取得値を返す */
            return aResult;
        }

        
        public void SetMfxDiscovery()
        {
            String aCommandText;

            aCommandText = Program.SERIALCMD_MFXDISCOVERY + "()";

            SendCommand(aCommandText);

        }

        public void SetMfxBind(UInt32 inUID, int inLocAddress)
        {
            String aCommandText;

            aCommandText = Program.SERIALCMD_MFXBIND + "(" + ((inUID >> 16) & 0xFFFF).ToString() + "," + (inUID & 0xFFFF).ToString() + "," + inLocAddress.ToString() + ")";

            SendCommand(aCommandText);

        }

        public void SetMfxVerify(UInt32 inUID, int inLocAddress)
        {
            String aCommandText;

            aCommandText = Program.SERIALCMD_MFXVERIFY + "(" + ((inUID >> 16) & 0xFFFF).ToString() + "," + (inUID & 0xFFFF).ToString() + "," + inLocAddress.ToString() + ")";

            SendCommand(aCommandText);

        }

    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace DesktopStation
{
    public class CraneControl
    {
        public int CraneLocAddress;
        public int CraneSpeed;
        public int Mode;

        private int PreviousTag;
        private RailuinoSerial serialCmd;
        private int[] functionList;

        public CraneControl(RailuinoSerial inSerialCmd)
        {
            CraneLocAddress = 77; /* LocAddressは77(Trix23951ベース) */

            CraneSpeed = 128;
            PreviousTag = 0;
            Mode = 0;

            serialCmd = inSerialCmd;

            functionList = new int[5];

            for (int i = 0; i < 5; i++)
            {
                functionList[i] = 0;
            }


        }

        public void StopAll()
        {
            serialCmd.SetLocoSpeed(CraneLocAddress, 0, 0);

            if (Mode == 1)
            {
                serialCmd.SetLocoSpeed(CraneLocAddress + 1, 0, 0);
            }

            for (int i = 2; i <= 4; i++)
            {
                if (functionList[i] == 1)
                {
                    serialCmd.SetLocoFunction(CraneLocAddress, i, 0);
                    functionList[i] = 0;
                }
            }
        }

        public void Stop(int inFlag)
        {

            if (((inFlag & 1) > 0) && (functionList[2] == 1))
            {
               serialCmd.SetLocoFunction(CraneLocAddress, 2, 0);
               functionList[2] = 0;
            }

            if (((inFlag & 2) > 0) && (functionList[3] == 1))
            {
                serialCmd.SetLocoFunction(CraneLocAddress, 3, 0);
                functionList[3] = 0;
            }

            if (((inFlag & 4) > 0) && (functionList[4] == 1))
            {
                serialCmd.SetLocoFunction(CraneLocAddress, 4, 0);
                functionList[4] = 0;
            }

            if (((inFlag & 8) > 0) && (functionList[1] == 1) && (Mode == 1))
            {
                serialCmd.SetLocoFunction(CraneLocAddress, 1, 0);
                functionList[1] = 0;
            }
        }

        public void DoCab()
        {
            /* フック */
            serialCmd.SetLocoFunction(CraneLocAddress, 1, 1);
            functionList[1] = 1;

        }

        public void DoHook()
        {
            /* フック */
            serialCmd.SetLocoFunction(CraneLocAddress, 3, 1);
            functionList[3] = 1;

        }

        public void DoBoom()
        {
            /* ブーム */
            serialCmd.SetLocoFunction(CraneLocAddress, 2, 1);
            functionList[2] = 1;

        }

        public void DoRotation()
        {
            /* 旋回 */
            serialCmd.SetLocoFunction(CraneLocAddress, 4, 1);
            functionList[4] = 1;

        }


        public void DoUp()
        {
            /* 上昇 */
            serialCmd.SetLocoDirection(CraneLocAddress, Program.DIRECTION_FWD);
            DSCommon.WaitSleepTime(1);
            serialCmd.SetLocoDirection(CraneLocAddress, Program.DIRECTION_FWD);

        }

        public void DoDown()
        {
            /* 下降 */
            serialCmd.SetLocoDirection(CraneLocAddress, Program.DIRECTION_REV);
            DSCommon.WaitSleepTime(1);
            serialCmd.SetLocoDirection(CraneLocAddress, Program.DIRECTION_REV);

        }

        public void DoRun()
        {
            serialCmd.SetLocoSpeed(CraneLocAddress, CraneSpeed, 0);

        }

        public bool DoButtonControl(int inTag)
        {
            bool aReturn = true;

            if ((inTag > 0) && (inTag < 99) && (inTag == PreviousTag))
            {
                aReturn = false;
                StopAll();
                PreviousTag = 0;


            }
            else
            {
                aReturn = true;
                PreviousTag = inTag;

                switch (inTag)
                {
                    case 0:
                        /* STOP */
                        StopAll();

                        break;

                    case 99:
                        /* RABIT */


                        break;

                    case 1:
                        Stop(3);

                        /* LEFT */
                        DoRotation();
                        DoDown();
                        DoRun();

                        break;

                    case 2:
                        Stop(3);

                        /* RIGHT */
                        DoRotation();
                        DoUp();
                        DoRun();

                        break;
                    case 3:
                        Stop(5);
                    
                        /* HOOKUP */
                        DoHook();
                        DoUp();
                        DoRun();

                        break;

                    case 4:
                        Stop(5);
                
                       /* HOOKDOWN */
                        DoHook();
                        DoDown();
                        DoRun();

                        break;
                    case 5:
                        Stop(6);

                        /* BOOMUP */
                        DoBoom();
                        DoUp();
                        DoRun();

                        break;

                    case 6:
                        Stop(6);

                        /* BOOMDOWN */
                        DoBoom();
                        DoDown();
                        DoRun();

                        break;

                    case 10:

                        /* クレーン車前進 */
                        serialCmd.SetLocoDirection(CraneLocAddress, Program.DIRECTION_FWD);
                        DSCommon.WaitSleepTime(1);
                        serialCmd.SetLocoDirection(CraneLocAddress, Program.DIRECTION_FWD);

                        serialCmd.SetLocoSpeed(CraneLocAddress + 1, CraneSpeed, 0);

                        break;

                    case 11:

                        /* クレーン車後退 */
                        serialCmd.SetLocoDirection(CraneLocAddress, Program.DIRECTION_REV);
                        DSCommon.WaitSleepTime(1);
                        serialCmd.SetLocoDirection(CraneLocAddress, Program.DIRECTION_REV);

                        serialCmd.SetLocoSpeed(CraneLocAddress + 1, CraneSpeed, 0);


                        break;

                    case 12:
                        Stop(7);

                        /* CAB Run left */
                        DoCab();
                        DoUp();
                        DoRun();

                        break;

                    case 13:
                        Stop(7);

                        /* Cab Run Right */
                        DoCab();
                        DoDown();
                        DoRun();

                        break;

                }
            }

            return aReturn;
        }


    }
}

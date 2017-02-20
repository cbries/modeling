using System;
using System.Collections.Generic;
using System.Text;

namespace DesktopStation
{
    public class ATBlockInfo
    {
        public Int64 StartPosition;
        public Int64 EndPosition;
        public Int64 Distance;
        public int BlockNo;
        public int SignalStatus;

    }

    public class ATLocomotive
    {
        public bool Enable;
        public Int64 Position;
        public int[] SpeedCurve;
        public int LocomotiveNo;
        public bool AutomaticMode;

        /* HLR用 */
        public int SpeedPatternNo;
        public int TargetSpeed;
        public int PreviousSpeed;
        public int SpeedAdjustCount;


        /**
         * シグモイド関数を計算する
         * @param[in] gain ゲイン(標準シグモイド関数の場合は1を指定)
         * @param[in] x 値
         * @return シグモイド関数の計算値
         */

        double sigmoid(double gain, double x)
        {
            return 1.0 / (1.0 + Math.Exp(-gain * x));
        }

        public void NotifySpeed(int inCurrentSpeed, int MaxPosition)
        {
            Position = Position + inCurrentSpeed / 2;

            if (MaxPosition < Position)
            {
                Position = 0;
            }
        }

        public void SetHLRCount(int inCount)
        {
            SpeedAdjustCount = inCount;
        }

        public int GetHLRSpeed()
        {
            if (TargetSpeed > PreviousSpeed)
            {
                /* 加速 */
                return PreviousSpeed + (((TargetSpeed - PreviousSpeed) * SpeedAdjustCount) >> 10);

            }
            else
            {
                /* 減速 */
                return TargetSpeed + (((TargetSpeed - PreviousSpeed) * SpeedAdjustCount) >> 10);
            }
        }



    }

    public class ATModelInfo
    {
        public Int64 Distance;
        public String ModelName;
        public int ModelNo;
        public String FileName;
        public bool Calibrated;

        private LocomotiveDB locDB;

        public ATModelInfo(LocomotiveDB inLocDB)
        {
            locDB = inLocDB;
            Distance = 0;
            ModelName = "Unknown";
            ModelNo = 0;
            

        }

        public void CalibrationStart(ATLocomotive inLocData)
        {

            /* 運転させる */




        }

        public void Interval()
        {


        }

    }


    public class Automatica
    {

        public ATModelInfo ModelInfo;
        public List<ATLocomotive> LocList;
        public List<ATBlockInfo> BlockList;

        private bool runStatus;
        private LocomotiveDB locDB;

        public Automatica(LocomotiveDB inLocDB)
        {
            runStatus = false;
            locDB = inLocDB;

            ModelInfo = new ATModelInfo(inLocDB);

        }


        public bool Start()
        {
            /* キャリブレーション済みかチェック */
            if (ModelInfo.Calibrated == false)
            {
                /* キャリブレーションしていない */


                return false;
            }




            runStatus = true;

            return true;
        }

        public void End()
        {
            runStatus = false;



        }

        public void Calibration()
        {


        }


        public void IntervalRun()
        {
            /* 定周期動作 */

            ModelInfo.Interval();

        }


    }
}

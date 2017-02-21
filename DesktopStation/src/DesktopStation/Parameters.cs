using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System.Windows.Forms;

namespace DesktopStation
{
    public class DoubleHeadingLocUnit
    {
        public int LocAddr { get; set; }
        public int LocSpeedstep { get; set; }

        public void Initialize()
        {
            LocAddr = 0;
            LocSpeedstep = 0;
        }
    }

    public class LocData
    {
        public int mLocItemNo;
        public int mLocAddr;
        public int mLocAddr_dbl;
        public string mLocName;
        public int[] mFunctionImageTable;
        public int[] mFunctionStatus;
        public int[] mFunctionExMethod;
        public int[] mFunctionExAddress;
        public int[] mFunctionExFuncNo;
        public string[] mExFunctionData;
        public string[] mExFunctionCommand;
        public string mLocManufacture;
        public double mCurrentSpeed;
        public int mCurrentDirection;
        public int mDisplayMaxSpeed;
        public int mSpeedAccRatio;
        public int mSpeedRedRatio;
        public string mIconFile;
        public string mComment;
        public int mLocMaxSpeed;
        public uint mMFXUID;
        public int mLocSpeedstep;
        public DoubleHeadingLocUnit[] mDoubleLoc;

        private double mTargetSpeed;
        private double mPreviousSpeed;
        private int mTransitionTime;
        private int mRemainTime;

        private bool mUpdateNextInterval;

        public LocData()
        {
            mDoubleLoc = new DoubleHeadingLocUnit[Program.DOUBLEHEADING_MAX];

            for (int k = 0; k < Program.DOUBLEHEADING_MAX; k++)
            {
                mDoubleLoc[k] = new DoubleHeadingLocUnit();
            }
        }

        public void Initialize()
        {

            mLocItemNo = 0;
            mLocAddr = 0;
            mLocAddr_dbl = 0;
            mLocName = "";
            mFunctionImageTable = null;
            mFunctionStatus = null;
            mFunctionExMethod = null;
            mExFunctionData = null;
            mExFunctionCommand = null;
            mFunctionExAddress = null;
            mFunctionExFuncNo = null;
            mLocManufacture = "";
            mCurrentSpeed = 0;
            mCurrentDirection = 0;
            mDisplayMaxSpeed = 0;
            mSpeedAccRatio = 0;
            mSpeedRedRatio = 0;
            mIconFile = "";
            mComment = "";
            mLocMaxSpeed = 0;
            mMFXUID = 0;
            mLocSpeedstep = 0;
        }

        public int CurrentSpeed()
        {
            return Convert.ToInt32(mCurrentSpeed);

        }

        public bool IsTrasitioning()
        {
            return (mTransitionTime > 0) ? true : false;
        }

        public void SetTransitionSpeed(int inTargetSpeed, int inTotalTime)
        {
            mPreviousSpeed = mCurrentSpeed;
            mTargetSpeed = inTargetSpeed;
            mTransitionTime = inTotalTime;
            mRemainTime = 0;
        }

        public void ResetTransitionSpeed()
        {
            mPreviousSpeed = 0;
            mTargetSpeed = 0;
            mTransitionTime = 0;
            mRemainTime = 0;
        }

        public void UpdateTransitionRemain(int inRemainTime)
        {
            mRemainTime = inRemainTime;
        }

        public void IncTransitionRemain()
        {
            if (mRemainTime < mTransitionTime)
            {

                mRemainTime++;
            }
            else
            {
                mRemainTime = mTransitionTime;
            }
        }

        public bool IsFinishedTransition()
        {
            return (mRemainTime == mTransitionTime) ? true : false;
        }


        public int GetTransitionRemain()
        {
            return mRemainTime;
        }

        public bool TransitionSpeed()
        {
            bool aReturn = false;
            double aSpeed;

            if (mTransitionTime > 0)
            {
                //y=ax+bの線形式
               aSpeed = mPreviousSpeed + ((mTargetSpeed - mPreviousSpeed) * mRemainTime) / mTransitionTime;

                //最高速度超のときは、リミットする。
               if (aSpeed >= mLocMaxSpeed)
               {
                   aSpeed = mLocMaxSpeed;
               }
                
            }
            else
            {
                aSpeed = mTargetSpeed;
            }

            if (aSpeed != mCurrentSpeed)
            {
                mCurrentSpeed = aSpeed;
                aReturn = true;
            }

            return aReturn;

        }

        public void ResetUpdateNextInterval()
        {
            mUpdateNextInterval = false;
        }

        public void SetUpdateNextInterval()
        {
            mUpdateNextInterval = true;
        }

        public bool IsUpdateNextInterval()
        {
            return mUpdateNextInterval;
        }


    };

    public class ScriptData
    {
        public string mCommand;
        public string mParam1;
        public string mParam2;
        public int mParam3;
        public int mParam4;
        public void Initialize()
        {
            mCommand = "";
            mParam1 = "";
            mParam2 = "";
            mParam3 = 0;
            mParam4 = 0;
        }

    };

    public class AccessoryData
    {
        public string mComment;
        public int mType;
        public int mDirection;
        public bool mReverse;
        public bool mInvisible;
        public int mProtocol;

        public void Initialize()
        {
            mComment = "";
            mType = 0;
            mDirection = 0;
            mReverse = false;
            mInvisible = false;
        }


        public int GetAccDirection()
        {
            if (mReverse == false)
            {
                return mDirection;
            }
            else
            {
                return GetReverserDirection();
            }

        }

        public void SetAccDirection(int inDirection)
        {
            mDirection = inDirection;

            /* リバース設定のとき、反転する */
            if (mReverse == true)
            {
                ReverserDirection();
            }

        }

        public void ReverserDirection()
        {
            mDirection = GetReverserDirection();
        }

        public int GetReverserDirection()
        {
            int aRet = 0;

            switch (mDirection)
            {
                case 0: aRet = 1; break;// 分岐→直進
                case 1: aRet = 0; break;// 直進→分岐
            }

            return aRet;
        }

    };


    public class AppSetting
    {
        public string mPortName;
        public int mBaudrate;
        public bool mDtrEnable;
        public string mKeyPowerOn;
        public string mKeyPowerOff;
        public string mKeyBreakOn;
        public string mKeyBreakOff;
        public string mKeyFreerun;
        public string mKeyEmergency;
        public int mSpeedGears;
        public int mSpeedLeverMode;
        public int[] mLocCtrlList;
        public List<AccessoryData> mAccList;
        public bool mDCCMode;
        public bool mMfxAutoRegister;
        public bool mMfxAutoUpdate;
        public bool mS88Sensor;
        public int mS88SendInterval;
        public bool mPingSend;
        public int mScreenNums;
        public int mScreen1Panel;
        public int mScreen2Panel;
        public int mScreen3Panel;
        public int mScreen4Panel;
        public DateTime mVirtualClock;
        public bool mUseVirtualClock;
        public int mSideFuncRight;
        public int mSideFuncBottom;
        public bool mStopAllLocWhenPowerOn;
        public string mLanguageFile;
        public int mS88NumOfConnection;
        public bool mAutoCloseSerialport;
        public bool mClearAccessories;
        public string mIPAddress;
        public int mSendMode;
        public int mSelectDCCDecoder;
        public int mWindowZoom;

        public void Initialize()
        {
            mPortName = "";
            mBaudrate = 0;
            mDtrEnable = true;
            mKeyPowerOn = "";
            mKeyPowerOff = "";
            mKeyBreakOn = "";
            mKeyBreakOff = "";
            mKeyFreerun = "";
            mKeyEmergency = "";
            mSpeedGears = 5;
            mSpeedLeverMode = 0;
            mLocCtrlList = null;
            mAccList = null;
            mDCCMode = true;
            mMfxAutoRegister = true;
            mMfxAutoUpdate = true;
            mS88Sensor = false;
            mS88NumOfConnection = 1;
            mS88SendInterval = 0;
            mPingSend = false;
            mScreenNums = 2;
            mScreen1Panel = 0;
            mScreen2Panel = 0;
            mScreen3Panel = 0;
            mScreen4Panel = 0;
            mSideFuncRight = 1;
            mSideFuncBottom = 1;
            mStopAllLocWhenPowerOn = true;
            mLanguageFile = "";
            mAutoCloseSerialport = false;
            mClearAccessories = false;

            mUseVirtualClock = false;
            mIPAddress = "192.168.0.177";
            mSendMode = 0;
            mSelectDCCDecoder = 0;
            mWindowZoom = 100;

        }

        public void SaveToFile(string inFileName)
        {
            //XmlSerializerを呼び出す
            XmlSerializer serializer = new XmlSerializer(typeof(AppSetting));
            //ファイルを作る
            FileStream fs = new FileStream(inFileName, FileMode.Create);
            //書き込み
            serializer.Serialize(fs, this); 
            //ファイルを閉じる
            fs.Close();
        }

 
        public void ClearDefault()
        {
            int i;

            this.mPortName = "COM1";
            this.mBaudrate = 115200;
            this.mSpeedGears = 5;
            this.mSpeedLeverMode = 0;
            this.mS88SendInterval = Program.COUNTTIMER_S88;
            this.mMfxAutoRegister = true;
            this.mMfxAutoUpdate = true;
            this.mS88Sensor = false;
            this.mDCCMode = true;
            this.mDtrEnable = true;

            for (i = 0; i < Program.MULTICONTROL_MAX; i++)
            {
                this.mLocCtrlList[i] = 0;
            }


            string aEngName = System.Globalization.CultureInfo.CurrentCulture.EnglishName.ToLower();

            if (aEngName.Contains("japanese"))
            {
                mLanguageFile = "Japanese.csv";
            }


        }

        public void Check()
        {

            if (this.mSpeedGears <= 0)
            {
                this.mSpeedGears = 5;
            }


        }

        public void ClearAccessories(bool inClear)
        {
            int i;

            if (this.mAccList.Count == 0)
            {
                for (i = 0; i < Program.ACCESSORIES_MAX; i++)
                {
                    this.mAccList.Add(new AccessoryData());
                }
            }
            else if (inClear == true)
            {
                for (i = 0; i < Program.ACCESSORIES_MAX; i++)
                {
                    this.mAccList[i].mDirection = 0;
                }
            }
            else
            {
                /* 何もしない（以前の状態を使用する） */
            }

            }


    }

    public class IconList
    {

        public IconList()
        {


        }

        public void LoadFromFolder(string inFolderName, ImageList inImgList)
        {

            if (System.IO.Directory.Exists(inFolderName) == false)
            {
                return;
            }

            string[] files = System.IO.Directory.GetFiles(inFolderName, "*", System.IO.SearchOption.AllDirectories);

            foreach( string aFile in files )
            {
                string aExt = System.IO.Path.GetExtension(aFile);

                if ((aExt == ".jpg") || (aExt == ".png") || (aExt == ".gif") || (aExt == ".bmp"))
                {
                    System.Drawing.Image aImage = System.Drawing.Image.FromFile(aFile);

                    if ((aImage.Width > 28) || (aImage.Height > 28))
                    {
                        //リサイズ

                    }


                    inImgList.Images.Add(aImage);

                }


            }


        }


    }


}

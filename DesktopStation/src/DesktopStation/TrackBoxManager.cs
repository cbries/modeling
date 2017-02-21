using System;
using System.Collections.Generic;

namespace DesktopStation
{
    public class TrackBoxItem
    {
        public string mTextTypes;
        public string mTextVersion;
        public string mTextTrackBoxUID;
        public uint mTypes;
        public uint mVersion;
        public uint mTrackBoxUID;

        public void Initialize()
        {
            mTrackBoxUID = 0;
            mVersion = 0;
            mTypes = 0;
            mTextVersion = "";
            mTextTypes = "";
            mTextTrackBoxUID = "";
        }
    };

    class TrackBoxManager
    {
        public List<TrackBoxItem> Items;


        public TrackBoxManager()
        {



            Items = new List<TrackBoxItem>();
        }

        public void Clear()
        {
            Items.Clear();

        }

        public bool CheckMS2()
        {

            int i;
            bool aResult = false;

            /* 既に登録されていないか検索する */
            for (i = 0; i < Items.Count; i++)
            {
                if ((Items[i].mTypes & 0xFFF0) == 0x0030)
                {
                    aResult = true;
                    break;
                }
            }

            return aResult;
        }

        public void SearchAdd(uint inUID, uint inType, uint inVersion)
        {
            int i;
            bool aResult = false;

            /* 既に登録されていないか検索する */
            for (i = 0; i < Items.Count; i++)
            {
                if (Items[i].mTrackBoxUID == inUID)
                {
                    aResult = true;
                    break;
                }
            }

            if (aResult == false)
            {
                /* 無ければ追加する */
                Add(inUID, inType, inVersion);

            }

        }

        public void Add(uint inUID, uint inType, uint inVersion)
        {
            TrackBoxItem aItem = new TrackBoxItem();

            aItem.mTrackBoxUID = inUID;
            aItem.mTypes = inType;
            aItem.mVersion = inVersion;
            aItem.mTextTrackBoxUID = inUID.ToString("X8");
            aItem.mTextVersion = ((inVersion >> 8) & 0xFF).ToString() + "." + (inVersion & 0xFF).ToString();
            aItem.mTextTypes = getTypes(inType);

            Items.Add(aItem);
        }

        private string getTypes(uint inType)
        {
            string aResult = "";

            switch( inType & 0xFFF0)
            {
                case 0x0000:
                    aResult = "Track Format Processor(60213,60214) / Booster(60173, 60174)";
                    break;
                case 0x0010:
                    aResult = "Trackbox(60112, 60113)";
                    break;
                case 0x0020:
                    aResult = "Connect 6021(60218)";
                    break;
                case 0x0030:
                    aResult = "MS2 60653";
                    break;
                case 0xFFE0:
                    aResult = "Wireless Devices";
                    break;
                case 0xFFFF:
                    aResult = "CS2";
                    break;
                case 0x2700:
                    aResult = "DesktopStation Gateway";
                    break;
            }

            return aResult;

        }

    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace DesktopStation
{
    class CVItem
    {
        public int Number;
        public int Value;

        public CVItem()
        {
            Number = 0;
            Value = 0;
        }

    }

    class CVManager
    {
        public int CurrentLoc;
        public List<CVItem> Items;

        public CVManager()
        {

            CurrentLoc = 0;

            Items = new List<CVItem>();

        }

        public void Clear()
        {
            int i;

            Items.Clear();
            CurrentLoc = 0;

            for (i = 1; i < 80; i++)
            {
                Add(i, 0);
            }

        }

        public void SetCurrentLoc(int inLocAddr)
        {
            CurrentLoc = inLocAddr;
        }

        public bool CheckCurrentLoc(int inLocAddr)
        {
            return CurrentLoc == inLocAddr ? true : false ;
        }

        public void Add(int inCVNo, int inValue)
        {
            int i;
            int aExist = -1;

            for (i = 0; i < Items.Count; i++)
            {
                if (Items[i].Number == inCVNo)
                {
                    aExist = i;
                    break;
                }

            }

            if (aExist < 0)
            {

                CVItem aItem = new CVItem();

                aItem.Number = inCVNo;
                aItem.Value = inValue;

                Items.Add(aItem);
            }
            else
            {
                Items[aExist].Value = inValue;

            }

        }

        public String GetCVName(int inCVNo, int inProtcol)
        {
            String aResult;

            switch (inProtcol)
            {
                case Program.PROTCOL_MM2:
                    aResult = getCVName_MM2(inCVNo);
                    break;

                case Program.PROTCOL_MFX:
                    aResult = getCVName_mfx(inCVNo);
                    break;

                case Program.PROTCOL_DCC:
                    aResult = getCVName_DCC(inCVNo);
                    break;
                default:
                    aResult = "";
                    break;

            }

            return aResult;

        }

        private String getCVName_MM2(int inCVNo)
        {
            String aResult;

            switch (inCVNo)
            {
                case 1:
                    aResult = "Address";
                    break;
                case 2:
                    aResult = "Min. Voltage";
                    break;
                case 3:
                    aResult = "Accel. time";
                    break;
                case 4:
                    aResult = "Decel. time";
                    break;
                case 5:
                    aResult = "Max. speed";
                    break;
                case 6:
                    aResult = "Avg. speed";
                    break;
                case 7:
                    aResult = "Version";
                    break;
                case 8:
                    aResult = "Manufacture";
                    break;
                default:
                    aResult = "";
                    break;
            }

            return aResult;

        }

        private String getCVName_mfx(int inCVNo)
        {
            String aResult;

            switch (inCVNo)
            {
                case 1:
                    aResult = "Address";
                    break;
                case 2:
                    aResult = "Min. Voltage";
                    break;
                case 3:
                    aResult = "Accel. time";
                    break;
                case 4:
                    aResult = "Decel. time";
                    break;
                case 5:
                    aResult = "Max. speed";
                    break;
                case 6:
                    aResult = "Avg. speed";
                    break;
                case 7:
                    aResult = "Version";
                    break;
                case 8:
                    aResult = "Manufacture";
                    break;
                default:
                    aResult = "";
                    break;
            }

            return aResult;
        }

        private String getCVName_DCC(int inCVNo)
        {
            String aResult;
            
            switch (inCVNo)
            {
                case 1:
                    aResult = "Address";
                    break;
                case 2:
                    aResult = "Min. Voltage";
                   break;
                case 3:
                   aResult = "Accel. time";
                   break;
                case 4:
                   aResult = "Decel. time";
                   break;
                case 5:
                   aResult = "Max. speed";
                   break;
                case 6:
                   aResult = "Avg. speed";
                   break;
                case 7:
                   aResult = "Version";
                   break;
                case 8:
                   aResult = "Manufacture";
                   break;
                default:
                   aResult = "";
                   break;
            }

            return aResult;

        }

        public void UpdateListView(ListView inListView, int inLocProtcol)
        {
            int i;
            int j = 0;
            int aExist;

            for (i = 0; i < Items.Count; i++)
            {
                aExist = -1;

                for (j = 0; j < inListView.Items.Count; j++)
                {
                    if (inListView.Items[j].Text == Items[i].Number.ToString())
                    {
                        if (inListView.Items[j].SubItems[1].Text != Items[i].Value.ToString())
                        {
                            inListView.Items[j].SubItems[1].Text = Items[i].Value.ToString();
                        }

                        aExist = j;
                        break;
                    }
                }

                if (aExist < 0)
                {
                    ListViewItem aItem = new ListViewItem();
                    aItem.Text = Items[i].Number.ToString();
                    aItem.SubItems.Add(GetCVName(Items[i].Value, inLocProtcol));
                    aItem.SubItems.Add(Items[i].Value.ToString());

                    inListView.Items.Add(aItem);
                }

            }

        }


    }
}

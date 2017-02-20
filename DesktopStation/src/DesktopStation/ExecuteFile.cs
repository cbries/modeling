using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.VisualBasic.FileIO;
using System.Xml.Serialization;
using System.IO;

namespace DesktopStation
{

    public class ExecuteFileItem
    {
        public String ItemName;
        public String FileName;
        public String Option;

        public ExecuteFileItem()
        {
            ItemName = "";
            FileName = "";
            Option = "";
        }

    }


    public class ExecuteManager
    {
        public List<ExecuteFileItem> Items;

        public ExecuteManager()
        {
            Items = new List<ExecuteFileItem>();
        }

        public void Run(String inItemName, String inRunFile)
        {

            int aIndex = GetIndex(inItemName);

            if (aIndex >= 0)
            {
                if (File.Exists(Items[aIndex].FileName) == false)
                {
                    return;
                }

                Process.Start(Items[aIndex].FileName, Items[aIndex].Option + " \"" + inRunFile + "\"");
            }
         }


        public void Add(String inItemName, String inFileName, String inOption)
        {
            ExecuteFileItem aItem = new ExecuteFileItem();

            aItem.ItemName = inItemName;
            aItem.FileName = inFileName;
            aItem.Option = inOption;

            Items.Add(aItem);

        }

        public bool Delete(String inItemName)
        {
            bool aResult = false;

            int aIndex = GetIndex(inItemName);

            if (aIndex >= 0)
            {
                Items.RemoveAt(aIndex);
                aResult = true;
            }

            return aResult;

        }



        public int GetIndex(String inItemName)
        {
            int aResult = -1;

            for (int i = 0; i < Items.Count; i++)
            {
                if (inItemName == Items[i].ItemName)
                {
                    aResult = i;
                    break;
                }
            }

            return aResult;

        }

        public void Clear()
        {
            Items.Clear();
        }

        public void SaveToFile(String inFileName)
        {
            int i;

            //ファイルを作る
            StreamWriter aStrWriter = new StreamWriter(inFileName, false);

            for (i = 0; i < Items.Count; i++)
            {
                /* 書き込み */
                aStrWriter.WriteLine("{0},{1},{2}", Items[i].ItemName, Items[i].FileName, Items[i].Option);
            }

            aStrWriter.Close();
        }

        public bool LoadFromFile(String inFileName)
        {
            bool retVal = false;
            String[] aFields;


            //設定ファイルがあるときのみ読み込みします。
            if (File.Exists(inFileName))
            {
                Clear();

                TextFieldParser aParser = new TextFieldParser(inFileName);
                aParser.TextFieldType = FieldType.Delimited;
                aParser.SetDelimiters(",");

                while (aParser.EndOfData == false)
                {

                    /* 分析処理する */
                    ExecuteFileItem aItem = new ExecuteFileItem();

                    aFields = aParser.ReadFields();

                    aItem.ItemName = aFields[0];
                    aItem.FileName = aFields[1];
                    aItem.Option = aFields[2];

                    /* 追加 */
                    Items.Add(aItem);
                }

                aParser.Close();

                retVal = true;
            }
            else
            {
                Clear();
            }

            if (Items.Count <= 0)
            {
                Add("PLAYSOUND", System.Windows.Forms.Application.StartupPath + "\\SoundPlay.exe", "");

            }


            return retVal;
        }



    }
}

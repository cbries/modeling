using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualBasic.FileIO;
using System.IO;

namespace DesktopStation
{
    public class ExecuteFileItem
    {
        public string ItemName;
        public string FileName;
        public string Option;

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

        public void Run(string inItemName, string inRunFile)
        {
            int aIndex = GetIndex(inItemName);
            if (aIndex >= 0)
            {
                if (File.Exists(Items[aIndex].FileName) == false)
                    return;

                Process.Start(Items[aIndex].FileName, Items[aIndex].Option + " \"" + inRunFile + "\"");
            }
         }

        public void Add(string inItemName, string inFileName, string inOption)
        {
            ExecuteFileItem aItem = new ExecuteFileItem();

            aItem.ItemName = inItemName;
            aItem.FileName = inFileName;
            aItem.Option = inOption;

            Items.Add(aItem);
        }

        public bool Delete(string inItemName)
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

        public int GetIndex(string inItemName)
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

        public void SaveToFile(string inFileName)
        {
            int i;
            StreamWriter aStrWriter = new StreamWriter(inFileName, false);
            for (i = 0; i < Items.Count; i++)
            {
                aStrWriter.WriteLine("{0},{1},{2}", Items[i].ItemName, Items[i].FileName, Items[i].Option);
            }
            aStrWriter.Close();
        }

        public bool LoadFromFile(string inFileName)
        {
            bool retVal = false;
            string[] aFields;

            if (File.Exists(inFileName))
            {
                Clear();

                TextFieldParser aParser = new TextFieldParser(inFileName);
                aParser.TextFieldType = FieldType.Delimited;
                aParser.SetDelimiters(",");

                while (aParser.EndOfData == false)
                {
                    ExecuteFileItem aItem = new ExecuteFileItem();
                    aFields = aParser.ReadFields();
                    aItem.ItemName = aFields[0];
                    aItem.FileName = aFields[1];
                    aItem.Option = aFields[2];
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

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualBasic.FileIO;
using System.IO;

namespace DesktopStation
{
    public class LanguageItem
    {
        public String Label;
        public String Text;

        public LanguageItem()
        {
            Label = "";
            Text = "";
        }


    }


    public class Language
    {
        public List<LanguageItem> Items;

        public Language()
        {
            Items = new List<LanguageItem>();

            Clear();

        }


        public void Clear()
        {
            Items.Clear();
        }

        public String Find(String inLabel)
        {
            int i = 0;
            String aResult = "";

            for (i = 0; i < Items.Count; i++)
            {
                if (inLabel == Items[i].Label)
                {
                    aResult = Items[i].Text;
                    break;
                }
            }

            return aResult;

        }


        public static String GetCSVFieldString(String[] inFields, int inIndex, String inDefault)
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

        public bool Loaded()
        {
            return Items.Count > 0 ? true : false;
        }

        public String SetText(String inText, String inDefault)
        {

            if ((inText == "") || (inText == null))
            {
                return inDefault;
            }
            else
            {
                String aFindData = Find(inText);

                if (aFindData == "")
                {
                    return inDefault;
                }
                else
                {
                    return aFindData;
                }
            }
        }



        public void LoadFromFile(String inFileName)
        {
            String[] aFields;

            //設定ファイルがあるときのみ読み込みします。
            if (File.Exists(inFileName))
            {

                TextFieldParser aParser = new TextFieldParser(inFileName);
                aParser.TextFieldType = FieldType.Delimited;
                aParser.SetDelimiters(",");

                while (aParser.EndOfData == false)
                {

                    /* 分析処理する */
                    ScriptData aItem = new ScriptData();

                    aFields = aParser.ReadFields();

                    String aTitle = GetCSVFieldString(aFields, 0, "");
                    String aData = GetCSVFieldString(aFields, 1, "");

                    SetLanguageItem(aTitle, aData);

                }

                aParser.Close();
            }
            else
            {
                Clear();
            }
        }

        private void SetLanguageItem(String inTitle, String inData)
        {

            LanguageItem aItem = new LanguageItem();

            aItem.Label = inTitle;
            aItem.Text = inData;

            Items.Add(aItem);
        }

    }
}

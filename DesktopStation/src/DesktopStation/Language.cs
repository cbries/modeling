using System;
using System.Collections.Generic;
using Microsoft.VisualBasic.FileIO;
using System.IO;

namespace DesktopStation
{
    public class LanguageItem
    {
        public string Label;
        public string Text;

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

        public string Find(string inLabel)
        {
            int i = 0;
            string aResult = "";

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

        public bool Loaded()
        {
            return Items.Count > 0 ? true : false;
        }

        public string SetText(string inText, string inDefault)
        {

            if ((inText == "") || (inText == null))
            {
                return inDefault;
            }
            else
            {
                string aFindData = Find(inText);

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

        public void LoadFromFile(string inFileName)
        {
            string[] aFields;

            if (File.Exists(inFileName))
            {
                TextFieldParser aParser = new TextFieldParser(inFileName)
                {
                    TextFieldType = FieldType.Delimited
                };
                aParser.SetDelimiters(",");

                while (aParser.EndOfData == false)
                {
                    aFields = aParser.ReadFields();
                    string aTitle = GetCSVFieldString(aFields, 0, "");
                    string aData = GetCSVFieldString(aFields, 1, "");

                    SetLanguageItem(aTitle, aData);
                }

                aParser.Close();
            }
            else
            {
                Clear();
            }
        }

        private void SetLanguageItem(string inTitle, string inData)
        {
            LanguageItem aItem = new LanguageItem();
            aItem.Label = inTitle;
            aItem.Text = inData;
            Items.Add(aItem);
        }
    }
}

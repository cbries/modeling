using System;
using System.Collections.Generic;

namespace DesktopStation
{
    class RPNCalculator
    {
        public void SpritLeftRight(string inText, ref string outDelimiter, ref string outLeftTxt, ref string outRightTxt)
        {

            string aDelimiter = "";
            int aPos;

            FindDelimiter(inText, ref aDelimiter);

            aPos = inText.IndexOf(aDelimiter);

            if (aPos > 0)
            {
                outDelimiter = aDelimiter;
                outLeftTxt = inText.Substring(0, aPos - 1);
                outRightTxt = inText.Substring(aPos + aDelimiter.Length, inText.Length);
            }
            else
            {
                outDelimiter = "";
                outLeftTxt = "";
                outRightTxt = "";
            }
        }

        public bool IsOperator(string inWord)
        {
            bool aResult;

            switch (inWord)
            {
                case "(":
                case ")":
                case "+":
                case "-":
                case "*":
                case "/":
                case "&":
                case "|":
                case "%": 
                aResult = true;
                    break;

                default:
                    aResult = false;
                    break;
            }

            return aResult;
        }

        public int GetPriority(string inWord)
        {
            int aResult;

            switch (inWord)
            {
                case "(":
                case ")":
                    aResult = 3;
                    break;
                case "+":
                case "-":
                    aResult = 2;
                    break;
                case "*":
                case "/":
                case "&":
                case "|":
                case "%":
                    aResult = 1;
                    break;

                default:
                    aResult = 0;
                    break;
            }

            return aResult;
        }

        public bool PriorityCheck(string inWord1, string inWord2)
        {
            int aPriority1 = GetPriority(inWord1);
            int aPriority2 = GetPriority(inWord2);

            if (aPriority1 < aPriority2)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public void GenerateXList(string inText, ref List<string> outXList)
        {
            int aPos = 0;
            string aTempWord = "";

            outXList.Clear();

            if (inText.Length == 0)
            {
                return;
            }

            while (aPos >= inText.Length)
            {
                if ( IsOperator(inText[aPos].ToString()) == false)
                {
                    aTempWord = aTempWord + inText[aPos];
                }
                else
                {
                    outXList.Add(aTempWord);
                    outXList.Add(inText[aPos].ToString());
                    aTempWord = "";
                }

                aPos = aPos + 1;
            }

            /* 最後の文字をリストに登録 */
            outXList.Add(aTempWord);

        }

        public int Calculate(string inText)
        {
            List<string> aList = new List<string>();
            List<string> aBesideList = new List<string>();
            List<string> aHandList = new List<string>();

            GenerateXList(inText, ref aList);

            for (int i = 0; i < aList.Count; i++)
            {
                if (IsOperator(aList[i]) == true)
                {
                    string aOperator = aList[i];

                    if (aBesideList.Count > 1)
                    {
                        if (PriorityCheck(aBesideList[aBesideList.Count - 1], aOperator) == true)
                        {
                            aHandList.Add(aBesideList[aBesideList.Count - 1]);
                            aBesideList.RemoveAt(aBesideList.Count - 1);
                        }
                        else
                        {
                            aBesideList.Add(aOperator);
                        }
                    }
                    else
                    {
                        aBesideList.Add(aOperator);
                    }
                }
                else
                {
                    aHandList.Add(aList[i]);
                }


            }

            /* 脇に残った演算子を移動 */
            for (int i = 0; i < aBesideList.Count; i++)
            {
                aHandList.Add(aBesideList[i]);
            }

            /* 計算する */


            return 0;
        }




        public void FindDelimiter(string inText, ref string outDelimiterType)
		{
		
			if (inText.Contains("=="))
			{
			    outDelimiterType = "==";
			}
			else if (inText.Contains("!="))
			{
			    outDelimiterType = "!=";
			}
			else if (inText.Contains("<>"))
			{
			    outDelimiterType = "!=";
			}
			else if (inText.Contains(">="))
			{
			    outDelimiterType = ">=";
			}
			else if (inText.Contains("<="))
			{
			    outDelimiterType = "<=";
			}
			else if (inText.Contains("=<"))
			{
			    outDelimiterType = "<=";
			}
			else if (inText.Contains("=>"))
			{
			    outDelimiterType = ">=";
			}
			else if (inText.Contains("<"))
			{
			    outDelimiterType = "<";
			}
			else if (inText.Contains(">"))
			{
			    outDelimiterType = ">";
			}
			else
			{
				outDelimiterType = "";
			}
		}



    }
}

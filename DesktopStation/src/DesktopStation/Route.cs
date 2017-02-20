using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.IO.Ports;
using System.Windows.Forms;


namespace DesktopStation
{
    public delegate bool checkRoute_delegate(int inRouteNo, bool inAutoUpdate);
    public delegate bool setRoute_delegate(int inRouteNo, bool inDirCheck);

    public class RouteItem
    {
        public int Type;     /* 種類 */
        public int Addr;     /* ポイントアドレス */
        public int Direction;   /* ポイント状態（方向） */
        public int No;          /* 順番 */
        public bool Available;  /* 有効無効選択 */
        public int Logical;     /* 演算論理 */

    }

    public class RouteLogicTree
    {
        public int Logical;     /* 演算論理 */
        public int Value;     /* 値 */
    }

    public class RouteItems
    {
        public List<RouteItem> Items;
        public String NameText;
        public int SignalAddr; /* 対応信号機アドレス */
        public int SignalDir; /* 対応信号機状態(方向) */
        public bool IgnoreOpenStatus;
        public int ID;

        public RouteItems()
        {

            Items = new List<RouteItem>();

        }

        public void CopyItemTo(int inIndex, ref RouteItem outItem)
        {
            outItem.Addr = Items[inIndex].Addr;
            outItem.Available = Items[inIndex].Available;
            outItem.Direction = Items[inIndex].Direction;
            outItem.Logical = Items[inIndex].Logical;
            outItem.No = Items[inIndex].No;
            outItem.Type = Items[inIndex].Type; 
        }
    }

    public class RouteList_temp
    {
        public int IDCount;
        public List<RouteItems> ListItems;

        public RouteList_temp()
        {
            ListItems = new List<RouteItems>();
            IDCount = 1;

        }
    }

    public class RouteList
    {
        public List<RouteItems> ListItems;
        public int IDCount;

        //上位から取得するもの
        private RailuinoSerial serialCmd;
        private List<AccessoryData> accList;
        private getS88Value_delegate getS88Value;
        private getScriptValue_delegate getSciptValue;
        private UpdateAccList_delegate updateAccList;

        public RouteList(RailuinoSerial inSerialCmd, getS88Value_delegate inGetS88Value, getScriptValue_delegate inGetScriptValue, List<AccessoryData> inAccList, UpdateAccList_delegate inUpdateAccList)
        {

            serialCmd = inSerialCmd;
            accList = inAccList;
            getS88Value = inGetS88Value;
            getSciptValue = inGetScriptValue;
            getS88Value = inGetS88Value;
            updateAccList = inUpdateAccList;

            ListItems = new List<RouteItems>();

            IDCount = 1;

        }

        public int GenerateID()
        {
            IDCount++;

            return IDCount;
        }

        public int GetRouteNo(int inID)
        {
            int aFoundNo = -1;

            for (int i = 0; i < ListItems.Count; i++)
            {
                if (ListItems[i].ID == inID)
                {
                    aFoundNo = i;
                }
            }

            return aFoundNo;
        }

        public bool SetRoute(int inRouteNo, bool inDirCheck)
        {
            int aRet = 0;

            if (inRouteNo < 0)
            {
                return false;
            }

            if (inRouteNo >= ListItems.Count)
            {
                return false;
            }

            for (int i = 0; i < ListItems[inRouteNo].Items.Count; i++)
            {
                int aAddr = ListItems[inRouteNo].Items[i].Addr;
                int aAddrIndex = aAddr - 1;
                int aDir = ListItems[inRouteNo].Items[i].Direction;

                if (aAddr > 0)
                {
                    switch (ListItems[inRouteNo].Items[i].Type)
                    {
                        case 0:
                            if ((accList[aAddrIndex].mDirection != aDir) || (inDirCheck == false))
                            {
                                serialCmd.SetTurnout(aAddr, aDir);
                                /* 管理リストに反映 */
                                updateAccList(aAddr, aDir);
                            }
                            break;

                        case 1:
                            //S88は何も出来ない
                            if (getS88Value(aAddr) != aDir)
                            {
                                //S88のセンサの不一致数をカウント
                                aRet++;
                            }
                            break;

                        case 2:
                            int aValue = getSciptValue(aAddr);

                            if ((aDir == 0) && (aValue == 0))
                            {
                                //何もしない
                            }
                            else if (( aDir == 1) && (aValue > 0))
                            {
                                //何もしない
                            }
                            else
                            {
                                //不一致条件
                                aRet++;
                            }
                            break;
                    }
                }
            }

            //開通時に自動的に信号を切り替えるか確認する(S88センサも一致しているとき)
            UpdateSignalStatus(inRouteNo, aRet);



            return (aRet == 0) ? true : false;

        }

        public int CheckRouteAll(bool inAutoUpdate)
        {
            int aRet = 0;

            for (int i = 0; i < ListItems.Count; i++)
            {
                if (CheckRoute(i, inAutoUpdate) == true)
                {
                    aRet++;
                }

            }

            return aRet;
        }


        public bool CheckRoute(int inRouteNo, bool inAutoUpdate)
        {
            int aRet = 0;
            int aLogicResult;
            List<RouteLogicTree> aLogicTree = new List<RouteLogicTree>();

            if (inRouteNo >= ListItems.Count)
            {
                return false;
            }

            if (inRouteNo < 0)
            {
                return false;
            }

            for (int i = 0; i < ListItems[inRouteNo].Items.Count; i++)
            {
                int aAddr = ListItems[inRouteNo].Items[i].Addr;
                int aAddrIndex = aAddr - 1;
                int aDir = ListItems[inRouteNo].Items[i].Direction;

                
                if (aAddrIndex >= 0)
                {
                    aLogicResult = 1;

                    switch (ListItems[inRouteNo].Items[i].Type)
                    {
                        case 0:
                            if (accList[aAddrIndex].mDirection != ListItems[inRouteNo].Items[i].Direction)
                            {
                                aLogicResult  = 0;
                            }
                            else
                            {
                                aLogicResult = 1;
                            }
                            break;

                        case 1:
                            if (getS88Value(aAddrIndex) != ListItems[inRouteNo].Items[i].Direction)
                            {
                                aLogicResult = 0;
                            }
                            else
                            {
                                aLogicResult = 1;
                            }
                            break;

                        case 2:
                            int aValue = getSciptValue(aAddr);

                            if ((aDir == 0) && (aValue == 0))
                            {
                                //何もしない
                                aLogicResult = 1;
                            }
                            else if (( aDir == 1) && (aValue > 0))
                            {
                                //何もしない
                                aLogicResult = 1;
                            }
                            else
                            {
                                //不一致条件
                                aLogicResult = 0;
                            }
                            break;
                    }

                    //論理木に登録
                    RouteLogicTree aItem = new RouteLogicTree();
                    aItem.Logical = ListItems[inRouteNo].Items[i].Logical;
                    aItem.Value = aLogicResult;

                    aLogicTree.Add(aItem);

                }
            }

            //論理チェック
            int aPos = 0;

            while (aPos < aLogicTree.Count)
            {
                //位置が一番最後のときはロジックは無視する。
                if ((aPos + 1) >= aLogicTree.Count)
                {
                    break;
                }

                //ロジックを計算する。
                if (aLogicTree[aPos].Logical <= 1)
                {
                    //and

                    if ((aLogicTree[aPos].Value & aLogicTree[aPos + 1].Value) > 0)
                    {
                        aLogicTree[aPos + 1].Value = 1;
                    }
                    else
                    {
                        aLogicTree[aPos + 1].Value = 0;
                    }

                    //削除
                    aLogicTree.RemoveAt(aPos);


                }
                else
                {
                    //or
                    aPos++;
                }

                if (aLogicTree.Count <= 1)
                {
                    break;
                }
            }

            //orの演算(全部andの場合は、1つしか残らないはず)
            int aOrCheck = 0;

            for (int i = 0; i < aLogicTree.Count; i++)
            {
                aOrCheck |= aLogicTree[i].Value;

            }

            //Orの結果をaRetに反映
            if (aOrCheck == 0)
            {
                aRet = 1;

            }
            else
            {
                aRet = 0;
            }

            //開通時に自動的に信号を切り替えるか確認する(S88センサも一致しているとき)
            UpdateSignalStatus(inRouteNo, aRet);

            return (aRet == 0) ? true : false;
        }


        private void UpdateSignalStatus(int inRouteNo, int inXCounter)
        {
            //開通・未開通時に信号機を切り替える処理（設定による）

            int aAddr_sig = ListItems[inRouteNo].SignalAddr;
            int aDir_sig = inXCounter == 0 ? ListItems[inRouteNo].SignalDir : Math.Abs(ListItems[inRouteNo].SignalDir - 1);


            if ((ListItems[inRouteNo].IgnoreOpenStatus == true) && (inXCounter > 0))
            {
                //未開通で自動セットにしていないときは切り替えない
                aAddr_sig = 0;
            }

            //信号切替え処理
            if (aAddr_sig > 0)
            {

                if (accList[aAddr_sig - 1].mDirection != aDir_sig)
                {
                    //指定の信号機を切り替える
                    serialCmd.SetTurnout(aAddr_sig, aDir_sig);
                    /* 管理リストに反映 */
                    updateAccList(aAddr_sig, aDir_sig);
                }
            }

        }



    }

    public class RouteManager
    {
        public RouteList RoutesList;

        //上位から取得するもの
        private RailuinoSerial serialCmd;
        private List<AccessoryData> accList;
        private UpdateAccList_delegate updateAccList;

        public RouteManager(RailuinoSerial inSerialCmd, getS88Value_delegate inGetS88Value, getScriptValue_delegate inGetScriptValue, List<AccessoryData> inAccList, UpdateAccList_delegate inUpdateAccList)
        {

            serialCmd = inSerialCmd;
            accList = inAccList;
            updateAccList = inUpdateAccList;

            RoutesList = new RouteList(inSerialCmd, inGetS88Value, inGetScriptValue, inAccList, inUpdateAccList); 


        }

        public void Clear()
        {
            RoutesList.IDCount = 1;
            RoutesList.ListItems.Clear();

        }


        public void SaveToFile(String inFilename)
        {
            int i, j;

            //いったん保存領域に展開する
            RouteList_temp aTempCfg = new RouteList_temp();

            aTempCfg.IDCount = RoutesList.IDCount;

            for (i = 0; i < RoutesList.ListItems.Count; i++)
            {
                RouteItems aItems = new RouteItems();

                aItems.NameText = RoutesList.ListItems[i].NameText;
                aItems.SignalAddr = RoutesList.ListItems[i].SignalAddr;
                aItems.SignalDir = RoutesList.ListItems[i].SignalDir;
                aItems.IgnoreOpenStatus = RoutesList.ListItems[i].IgnoreOpenStatus;
                aItems.ID = RoutesList.ListItems[i].ID;


                for (j = 0; j < RoutesList.ListItems[i].Items.Count; j++)
                {
                    RouteItem aItem = new RouteItem();
                    aItem.Addr = RoutesList.ListItems[i].Items[j].Addr;
                    aItem.Available = RoutesList.ListItems[i].Items[j].Available;
                    aItem.Direction = RoutesList.ListItems[i].Items[j].Direction;
                    aItem.No = RoutesList.ListItems[i].Items[j].No;
                    aItem.Type = RoutesList.ListItems[i].Items[j].Type;
                    aItem.Logical = RoutesList.ListItems[i].Items[j].Logical;

                    aItems.Items.Add(aItem);

                }

                aTempCfg.ListItems.Add(aItems);

            }

            //XmlSerializerを呼び出す
            XmlSerializer serializer = new XmlSerializer(typeof(RouteList_temp));
            //ファイルを作る
            FileStream fs = new FileStream(inFilename, FileMode.Create);
            //書き込み
            serializer.Serialize(fs, aTempCfg);  
            //ファイルを閉じる
            fs.Close();
        }

        public bool LoadFromFile(String inFilename)
        {
            bool retVal = false;
            int i, j;

            //設定ファイルがあるときのみ読み込み
            if (File.Exists(inFilename))
            {
                //XmlSerializerを呼び出す
                XmlSerializer serializer = new XmlSerializer(typeof(RouteList_temp));
                //ファイルを開く
                FileStream fs = new FileStream(inFilename, FileMode.Open);

                //読み込み
                RouteList_temp aTempCfg = new RouteList_temp();

                try
                {
                    aTempCfg = (RouteList_temp)serializer.Deserialize(fs);
                }
                catch (System.Exception ex)
                {
                    //ファイルを閉じる
                    fs.Close();

                    //メッセージを表示する
                    MessageBox.Show("Unknown file. (" + ex.Message + ")", "Event File Open Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return false;
                }

                //ファイルを閉じる
                fs.Close();

                //初期化
                Clear();

                //反映
                for (i = 0; i < aTempCfg.ListItems.Count; i++)
                {
                    RouteItems aList = new RouteItems();
                    aList.NameText = aTempCfg.ListItems[i].NameText;
                    aList.SignalAddr = aTempCfg.ListItems[i].SignalAddr;
                    aList.SignalDir = aTempCfg.ListItems[i].SignalDir;
                    aList.ID = aTempCfg.ListItems[i].ID;
                    aList.IgnoreOpenStatus = aTempCfg.ListItems[i].IgnoreOpenStatus;

                    RoutesList.ListItems.Add(aList);


                    for (j = 0; j < aTempCfg.ListItems[i].Items.Count; j++)
                    {
                        RouteItem aItem = new RouteItem();
                        aItem.No = aTempCfg.ListItems[i].Items[j].No;
                        aItem.Addr = aTempCfg.ListItems[i].Items[j].Addr;
                        aItem.Type = aTempCfg.ListItems[i].Items[j].Type;
                        aItem.Available = aTempCfg.ListItems[i].Items[j].Available;
                        aItem.Direction = aTempCfg.ListItems[i].Items[j].Direction;
                        aItem.Logical = aTempCfg.ListItems[i].Items[j].Logical;
                        RoutesList.ListItems[i].Items.Add(aItem);

                    }
                }

                RoutesList.IDCount = aTempCfg.IDCount;

                retVal = true;
            }
            else
            {
                /* 初期化は完了済 */
            }


            return retVal;
        }
    }

    
}

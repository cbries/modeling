using System;
using System.Collections.Generic;
using Microsoft.VisualBasic.FileIO;
using System.IO;
using System.Drawing;

namespace DesktopStation
{
    class TrackData
    {
        public int No;
        public short ImageNo;
        public short AccsessoryAddress;
        public short S88SensorAddress;
        public int RouteNo;

        public TrackData()
        {
            No = 0;
            ImageNo = 0;
            AccsessoryAddress = 0;
            S88SensorAddress = 0;
            RouteNo = -1;

        }

    }

    class SpecialData
    {
        public int No;
        public int X;
        public int Y;
        public int FontSize;
        public string ImageFile;
        public string Text;
        public bool Enable;

        public SpecialData()
        {
            No = 0;
            X = 0;
            Y = 0;
            FontSize = 10;
            ImageFile = "";
            Text = "";
            Enable = false;

        }

    }

    class TrackDiagramManager
    {
        public List<TrackData> Items;
        public List<SpecialData> SpecialItems;
        public int Width;
        public int Height;
        public int ZoomLevel;
        public bool UsingS88;
        private Bitmap RailMapImage;

        public TrackDiagramManager()
        {
            Items = new List<TrackData>();
            SpecialItems = new List<SpecialData>();

            Width = Program.TILE32_LAYOUT_WIDTH;
            Height = Program.TILE32_LAYOUT_HEIGHT;
            ZoomLevel = 100;
            UsingS88 = false;

        }

        public void Clear()
        {
            int i;
            
            Items.Clear();
            SpecialItems.Clear();
            UsingS88 = false;

            for (i = 0; i < Width * Height; i++)
            {
                TrackData aItem = new TrackData();
                aItem.No = i;
                Items.Add(aItem);
            }           


        }

        public bool IsSpecialItem(int inNo)
        {
            bool aRet = false;
            int aX = inNo % Width;
            int aY = inNo / Width;

            for (int i = 0; i < SpecialItems.Count; i++)
            {
                if ((aX == SpecialItems[i].X) && (SpecialItems[i].Y == aY))
                {
                    aRet = true;
                    break;
                }
            }

            return aRet;

        }

        public int GetSpecialItemNo(int inNo)
        {
            int aIndex = -1;
            int aX = inNo % Width;
            int aY = inNo / Width;

            for (int i = 0; i < SpecialItems.Count; i++)
            {
                if ((aX == SpecialItems[i].X) && (SpecialItems[i].Y == aY))
                {
                    aIndex = i;
                    break;
                }
            }

            return aIndex;

        }

        public int GetMaxCount()
        {
            return Width * Height;
        }

        public int GetLayoutTile(int inMapIndex)
        {
            if (inMapIndex < Items.Count)
            {
                return (Items[inMapIndex].ImageNo);
            }
            else
            {
                return 0;
            }

        }

        public int GetLayoutAccNo(int inMapIndex)
        {
            if (inMapIndex < Items.Count)
            {
                return (Items[inMapIndex].AccsessoryAddress);
            }
            else
            {
                return 0;
            }

        }

        public int GetLayoutS88No(int inMapIndex)
        {
            if (inMapIndex < Items.Count)
            {
                return (Items[inMapIndex].S88SensorAddress);
            }
            else
            {
                return 0;
            }

        }

        public int GetLayoutRouteNo(int inMapIndex)
        {
            if (inMapIndex < Items.Count)
            {
               return (Items[inMapIndex].RouteNo);

            }
            else
            {
                return 0;
            }

            }

        public void SetLayoutMapRouteNo(int inMapIndex, int inRouteNo)
        {
            if (inMapIndex < Items.Count)
            {
                Items[inMapIndex].RouteNo = inRouteNo;
            }

        }

        public void UpdateLayoutTile(int inMapIndex, int inTileNo)
        {

            /* 値を更新 */
            Items[inMapIndex].ImageNo = (short)inTileNo;

        }

        public void UpdateLayoutAccNo(int inMapIndex, int inAccAddr)
        {

            /* 値を更新 */
            Items[inMapIndex].AccsessoryAddress = (short)inAccAddr;

        }

        public void UpdateLayoutS88Addr(int inMapIndex, int inS88Addr)
        {
            /* フラグ書き換え */
            if (inS88Addr > 0)
            {
                UsingS88 = true;
            }

            /* 値を更新 */
            Items[inMapIndex].S88SensorAddress = (short)inS88Addr;

        }

        public void Resize(int inWidth, int inHeight)
        {
            List<TrackData> aItems = new List<TrackData>();
            int aLastWidth = Width;
            int aLastHeight = Height;
            int aLastMax = aLastWidth * aLastHeight;
            bool aUsingS88 = UsingS88;
            int x, y;

            for (y = 0; y < inHeight; y++)
            {
                for (x = 0; x < inWidth; x++)
                {
                    TrackData aItem = new TrackData();

                    if ((y < aLastHeight) && (x < aLastWidth))
                    {
                        int aNo = (y * aLastWidth) + x;

                        if ((aNo < aLastMax) && (aNo < Items.Count))
                        {
                            aItem.ImageNo = Items[aNo].ImageNo;
                            aItem.AccsessoryAddress = Items[aNo].AccsessoryAddress;
                            aItem.S88SensorAddress = Items[aNo].S88SensorAddress;
                            aItem.RouteNo = Items[aNo].RouteNo;
                        }
                        else
                        {
                            /* 最後のダミーアイテムまたは不正なとき */
                            aItem.No = 0;
                            aItem.RouteNo = 0;
                            aItem.S88SensorAddress = 0;
                            aItem.AccsessoryAddress = 0;
                        }

                    }
                    else
                    {
                        aItem.No = 0;
                        aItem.RouteNo = 0;
                        aItem.S88SensorAddress = 0;
                        aItem.AccsessoryAddress = 0;
                    }

                    //Add
                    aItems.Add(aItem);

                }

            }

            //いったんクリア
            Items.Clear();

            //コピーする
            UsingS88 = aUsingS88;

            for (int i = 0; i < aItems.Count; i++)
            {
                Items.Add(aItems[i]);
            }

            //サイズを反映
            Width = inWidth;
            Height = inHeight;


        }


        public int GetLayoutMapIndex(int inX, int inY, int inScaleRatio)
        {
            int aX;
            int aY;
            int aTileSize = Program.TILE32_SIZE * inScaleRatio / 100;

            /* XYのインデックス番号に変換 */
            aX = inX / aTileSize;
            aY = inY / aTileSize;

            /* 横方向の最大を規定 */
            if (aX >= Width)
            {

                aX = Width - 1;
            }

            /* 縦方向の最大を規定 */
            if (aY >= Height)
            {

                aY = Height - 1;
            }

            /* 1次元のインデックス番号を確定 */
            return (aY * Width + aX);

        }

        public void GetLayoutMapPos(int inX, int inY, int inScaleRatio, out int outX, out int outY)
        {
            int aX;
            int aY;
            int aTileSize = Program.TILE32_SIZE * inScaleRatio / 100;

            /* XYのインデックス番号に変換 */
            aX = inX / aTileSize;
            aY = inY / aTileSize;

            /* 横方向の最大を規定 */
            if (aX >= Width)
            {

                aX = Width - 1;
            }

            /* 縦方向の最大を規定 */
            if (aY >= Height)
            {

                aY = Height - 1;
            }

            /* 1次元のインデックス番号を確定 */
            outX = aX;
            outY = aY;

        }




        public void SaveToFile(string inFileName)
        {
            int i;

            //ファイルを作る
            StreamWriter aStrWriter = new StreamWriter(inFileName, false);

            aStrWriter.WriteLine("TRACKDIAGRAM,{0},{1},{2}", Width, Height, ZoomLevel);

            for (i = 0; i < SpecialItems.Count; i++)
            {
                /* 書き込み */
                aStrWriter.WriteLine("SPITEM,{0},{1},{2},{3},{4},{5},{6}", SpecialItems[i].No, (SpecialItems[i].Enable == true) ? 1 : 0, SpecialItems[i].X, SpecialItems[i].Y, SpecialItems[i].Text, SpecialItems[i].ImageFile, SpecialItems[i].FontSize);
            }


            for (i = 0; i < Items.Count; i++)
            {
                /* 書き込み */
                aStrWriter.WriteLine("{0},{1},{2},{3}", Items[i].ImageNo, Items[i].AccsessoryAddress, Items[i].S88SensorAddress, Items[i].RouteNo);
            }

            aStrWriter.Close();
        }

        public void LoadFromFile(string inFileName)
        {

            string[] aFields;
            int i;
            int aTileNo;
            int aAccAddr;
            int aS88Addr;
            int aRouteNo;
            int aMax;

            //設定ファイルがあるときのみ読み込みします。
            if (File.Exists(inFileName))
            {

                TextFieldParser aParser = new TextFieldParser(inFileName);
                aParser.TextFieldType = FieldType.Delimited;
                aParser.SetDelimiters(",");

                i = 0;
                aMax = Width * Height;

                Items.Clear();

                while (aParser.EndOfData == false)
                {

                    /* 分析処理する */
                    aFields = aParser.ReadFields();

                    if (aFields[0] == "TRACKDIAGRAM")
                    {
                        Width = GetCSVFieldInt(aFields, 1, 0);
                        Height = GetCSVFieldInt(aFields, 2, 0);
                        ZoomLevel = GetCSVFieldInt(aFields, 3, 0);

                        //サイズを更新
                        aMax = Width * Height + 1;

                        if (ZoomLevel == 0)
                        {
                            ZoomLevel = 100;
                        }

                    }
                    else if (aFields[0] == "SPITEM")
                    {
                        //デコレーションデータ
                        SpecialData aSData = new SpecialData();

                        aSData.No = GetCSVFieldInt(aFields, 1, 0);
                        aSData.Enable = GetCSVFieldInt(aFields, 2, 0) == 1 ? true : false;
                        aSData.X = GetCSVFieldInt(aFields, 3, 0);
                        aSData.Y = GetCSVFieldInt(aFields, 4, 0);
                        aSData.Text = aFields[5];
                        aSData.ImageFile = aFields[6];
                        aSData.FontSize = GetCSVFieldInt(aFields, 7, 0);

                        SpecialItems.Add(aSData);
                    }
                    else
                    {
                        //通常データ
                        aTileNo = GetCSVFieldInt(aFields, 0, 0);
                        aAccAddr = GetCSVFieldInt(aFields, 1, 0);
                        aS88Addr = GetCSVFieldInt(aFields, 2, 0);
                        aRouteNo = GetCSVFieldInt(aFields, 3, 0);

                        /* 追加 */
                        if (i < aMax)
                        {
                            TrackData aItem = new TrackData();

                            aItem.ImageNo = (short)aTileNo;
                            aItem.AccsessoryAddress = (short)aAccAddr;
                            aItem.S88SensorAddress = (short)aS88Addr;
                            aItem.RouteNo = aRouteNo;

                            if (aS88Addr > 0)
                            {
                                UsingS88 = true;
                            }

                            Items.Add(aItem);

                        }

                    }

                    /* インデックスを進める */
                    i = i + 1;

                }

                aParser.Close();

                /* サイズが満たない場合は空データを追加 */
                while (i < aMax)
                {
                    TrackData aItem2 = new TrackData();
                    Items.Add(aItem2);

                    i++;
                }


            }
            else
            {
                /* 存在しない場合は、空データだけ生成する */
                Clear();

            }

        }

        private static int GetCSVFieldInt(string[] inFields, int inIndex, int inDefault)
        {
            int aResult;

            if (inFields.Length > inIndex)
            {
                aResult = ParseStrToInt(inFields[inIndex]);
            }
            else
            {
                aResult = inDefault;
            }
            return aResult;
        }

        public static int ParseStrToInt(string aText)
        {
            int aRet;

            int.TryParse(aText, out aRet);

            return aRet;

        }


        public void LoadRailMap(string inFilename)
        {
            RailMapImage = new Bitmap(inFilename);

        }

        public void DrawRail(Graphics inCanvas, int inIndex, int x, int y, int width, int height)
        {
            if (RailMapImage == null)
            {
                return;
            }

            int ax, ay;

            ax = (inIndex % 4) * 32;
            ay = (inIndex / 4) * 32;


            Rectangle srcRect = new Rectangle(ax, ay, 32, 32);

            Rectangle desRect = new Rectangle(x, y, width, height);

            inCanvas.DrawImage(RailMapImage, desRect, srcRect, GraphicsUnit.Pixel);

        }


    }
}

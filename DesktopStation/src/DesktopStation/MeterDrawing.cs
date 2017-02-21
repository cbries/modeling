using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace DesktopStation
{
    class MeterDrawing
    {
        public void DrawClockBox(Graphics inCanvas, DateTime inTime, int inScaleRatio)
        {
            int aX_hour, aY_hour;
            int aX_min, aY_min;
            int aX_sec, aY_sec;
            int aX_sec2, aY_sec2;
            int aPinWidth = 2;
            int aPinWidth2 = 1;
            int aTheta_sec = 0;
            int aTheta_min = 0;
            int aTheta_hour = 0;
            int aCX = 56 * inScaleRatio / 100;
            int aCY = 92 * inScaleRatio / 100;
            int aR_sec = 45 * inScaleRatio / 100;
            int aR_sec2 = 12 * inScaleRatio / 100;
            int aR_min = 42 * inScaleRatio / 100;
            int aR_hour = 28 * inScaleRatio / 100;

            /* 針を描画 */
            aTheta_sec = inTime.Second * 60;

            aX_sec = aCX + Convert.ToInt32(aR_sec * Math.Cos((aTheta_sec - 900) * 2 * Math.PI / 3600));
            aY_sec = aCY + Convert.ToInt32(aR_sec * Math.Sin((aTheta_sec - 900) * 2 * Math.PI / 3600));
            aX_sec2 = aCX + Convert.ToInt32(aR_sec2 * Math.Cos((aTheta_sec - 2700) * 2 * Math.PI / 3600));
            aY_sec2 = aCY + Convert.ToInt32(aR_sec2 * Math.Sin((aTheta_sec - 2700) * 2 * Math.PI / 3600));

            aTheta_min = inTime.Minute * 60 + inTime.Second;

            /* 針の位置を演算 */
            aX_min = aCX + Convert.ToInt32(aR_min * Math.Cos((aTheta_min - 900) * 2 * Math.PI / 3600));
            aY_min = aCY + Convert.ToInt32(aR_min * Math.Sin((aTheta_min - 900) * 2 * Math.PI / 3600));

            aTheta_hour = (inTime.Hour % 12) * 300 + inTime.Minute * 5;

            /* 針の位置を演算 */
            aX_hour = aCX + Convert.ToInt32(aR_hour * Math.Cos((aTheta_hour - 900) * 2 * Math.PI / 3600));
            aY_hour = aCY + Convert.ToInt32(aR_hour * Math.Sin((aTheta_hour - 900) * 2 * Math.PI / 3600));

            Pen aPen = new Pen(Color.Black, aPinWidth);
            Pen aPen2 = new Pen(Color.Red, aPinWidth2);

            inCanvas.DrawLine(aPen, aCX, aCY, aX_min, aY_min);
            inCanvas.DrawLine(aPen, aCX, aCY, aX_hour, aY_hour);
            inCanvas.DrawLine(aPen2, aX_sec2, aY_sec2, aX_sec, aY_sec);
            inCanvas.FillEllipse(Brushes.Red, aCX - 1, aCY - 1, 3, 3);

            aPen.Dispose();
            aPen2.Dispose();

        }

        private void DrawFuchiText(Graphics inCanvas, string inText, int inX, int inY)
        {

            System.Drawing.Drawing2D.GraphicsPath aPath = new System.Drawing.Drawing2D.GraphicsPath();

            aPath.AddString(inText, new FontFamily("Arial"), (int)FontStyle.Regular, 20, new Point(inX, inY), StringFormat.GenericDefault);

            //パスの線分を描画
            Pen aPen = new Pen(Color.DarkGray, 1.0F);

            inCanvas.DrawPath(aPen, aPath);

            //塗る
            Brush fillBrush = new SolidBrush(Color.White);

            inCanvas.FillPath(fillBrush, aPath);

            aPen.Dispose();
            fillBrush.Dispose();
            aPath.Dispose();

        }


        public void DrawMeterBox_Background(Graphics inCanvas, int inCX, int inCY, int inRadius, int inMaxSpeedDisplay, Color inBackColor)
        {
            int aX, aY, bX, bY;
            int aTheta = 0;
            int i;
            int aMeterLength;
            int aAngleInterval_LargePin;
            int aAngleCntMax;
            int aScaleRatio = inCX / 2;

            RectangleF aGaugeRect = new RectangleF(0, 0, ((float)inCX * 2 - 1), ((float)inCY * 2 - 1));

            /* 塗りつぶし */
            SolidBrush aBackgroundBrush = new SolidBrush(inBackColor);

            inCanvas.FillRectangle(aBackgroundBrush, aGaugeRect);

            /* 外 */
            LinearGradientBrush aGBrush = new LinearGradientBrush(aGaugeRect, Color.Gray, Color.Black, LinearGradientMode.ForwardDiagonal);
            inCanvas.FillEllipse(aGBrush, aGaugeRect);


            /* 内側1 */
            aGaugeRect.Inflate(-1, -1);

            LinearGradientBrush aGBrush2 = new LinearGradientBrush(aGaugeRect, Color.LightGray, Color.Gray, LinearGradientMode.ForwardDiagonal);
            inCanvas.FillEllipse(aGBrush2, aGaugeRect);

            /* 内側1 */
            aGaugeRect.Inflate(-3, -3);

            LinearGradientBrush aGBrush3 = new LinearGradientBrush(aGaugeRect, Color.DarkGray, Color.White, LinearGradientMode.ForwardDiagonal);
            inCanvas.FillEllipse(aGBrush3, aGaugeRect);

            /* 内側1 */
            aGaugeRect.Inflate(-2, -2);

            LinearGradientBrush aGBrush4 = new LinearGradientBrush(aGaugeRect, Color.FromArgb(60, 60, 60), Color.FromArgb(20, 20, 20), LinearGradientMode.ForwardDiagonal);
            inCanvas.FillEllipse(aGBrush4, aGaugeRect);

            //ペンオブジェクトの作成
            Pen aPen = new Pen(Color.White);
            aPen.Width = Program.PEN_WIDTH_DBL * aScaleRatio / 100;

            /* 角度間隔を計算する */
            aAngleCntMax = inMaxSpeedDisplay / 5;
            aAngleInterval_LargePin = (inMaxSpeedDisplay / 30);

            i = 0;
            while (i <= aAngleCntMax)
            {
                aTheta = ((i * 240) / aAngleCntMax) + 60;

                if (i % aAngleInterval_LargePin == 0)
                {
                    aPen.Width = 4 * aScaleRatio / 100;
                    aX = inCX + Convert.ToInt32(inRadius * Math.Cos((aTheta - 270) * 2 * Math.PI / 360));
                    aY = inCY + Convert.ToInt32(inRadius * Math.Sin((aTheta - 270) * 2 * Math.PI / 360));
                    bX = inCX + Convert.ToInt32((inRadius - Program.METER_LINEWIDTH2) * Math.Cos((aTheta - 270) * 2 * Math.PI / 360));
                    bY = inCY + Convert.ToInt32((inRadius - Program.METER_LINEWIDTH2) * Math.Sin((aTheta - 270) * 2 * Math.PI / 360));
                }
                else if (i % 2 == 1)
                {
                    aPen.Width = 3 * aScaleRatio / 100;
                    aMeterLength = (Program.METER_LINEWIDTH2 - Program.METER_LINEWIDTH0) / 2;

                    aX = inCX + Convert.ToInt32((inRadius - aMeterLength) * Math.Cos((aTheta - 270) * 2 * Math.PI / 360));
                    aY = inCY + Convert.ToInt32((inRadius - aMeterLength) * Math.Sin((aTheta - 270) * 2 * Math.PI / 360));
                    bX = inCX + Convert.ToInt32((inRadius - Program.METER_LINEWIDTH0 - aMeterLength) * Math.Cos((aTheta - 270) * 2 * Math.PI / 360));
                    bY = inCY + Convert.ToInt32((inRadius - Program.METER_LINEWIDTH0 - aMeterLength) * Math.Sin((aTheta - 270) * 2 * Math.PI / 360));

                }
                else
                {
                    aPen.Width = 3 * aScaleRatio / 100;
                    aMeterLength = (Program.METER_LINEWIDTH2 - Program.METER_LINEWIDTH1) / 2;

                    aX = inCX + Convert.ToInt32((inRadius - aMeterLength) * Math.Cos((aTheta - 270) * 2 * Math.PI / 360));
                    aY = inCY + Convert.ToInt32((inRadius - aMeterLength) * Math.Sin((aTheta - 270) * 2 * Math.PI / 360));
                    bX = inCX + Convert.ToInt32((inRadius - Program.METER_LINEWIDTH1 - aMeterLength) * Math.Cos((aTheta - 270) * 2 * Math.PI / 360));
                    bY = inCY + Convert.ToInt32((inRadius - Program.METER_LINEWIDTH1 - aMeterLength) * Math.Sin((aTheta - 270) * 2 * Math.PI / 360));
                }

                //直線を描画
                inCanvas.DrawLine(aPen, aX, aY, bX, bY);

                /* 次へ */
                i = i + 1;
            }


            /* 速度表示 */
            System.Drawing.Font aDrawFontMeter = new System.Drawing.Font("Arial", 20 * aScaleRatio / 100);
            StringFormat aDrawFormat = new StringFormat();
            aDrawFormat.Alignment = StringAlignment.Center;

            inCanvas.DrawString("0", aDrawFontMeter, Brushes.White, 70 * aScaleRatio / 100, 260 * aScaleRatio / 100);
            inCanvas.DrawString((inMaxSpeedDisplay * 1 / 6).ToString(), aDrawFontMeter, Brushes.White, 50 * aScaleRatio / 100, 155 * aScaleRatio / 100);
            inCanvas.DrawString((inMaxSpeedDisplay * 2 / 6).ToString(), aDrawFontMeter, Brushes.White, 95 * aScaleRatio / 100, 75 * aScaleRatio / 100);
            inCanvas.DrawString((inMaxSpeedDisplay * 3 / 6).ToString(), aDrawFontMeter, Brushes.White, 200 * aScaleRatio / 100, 45 * aScaleRatio / 100, aDrawFormat);
            inCanvas.DrawString((inMaxSpeedDisplay * 4 / 6).ToString(), aDrawFontMeter, Brushes.White, 250 * aScaleRatio / 100, 75 * aScaleRatio / 100);
            inCanvas.DrawString((inMaxSpeedDisplay * 5 / 6).ToString(), aDrawFontMeter, Brushes.White, 305 * aScaleRatio / 100, 155 * aScaleRatio / 100);
            inCanvas.DrawString(inMaxSpeedDisplay.ToString(), aDrawFontMeter, Brushes.White, 280 * aScaleRatio / 100, 260 * aScaleRatio / 100);

            aDrawFontMeter.Dispose();
            aPen.Dispose();
            aGBrush.Dispose();
            aGBrush2.Dispose();
            aGBrush3.Dispose();
            aGBrush4.Dispose();
            aDrawFontMeter.Dispose();
            aDrawFormat.Dispose();
            aBackgroundBrush.Dispose();
        }

        public void DrawMeterBox_Pin(Graphics inCanvas, int inCX, int inCY, int inRadius, int inMaxSpeed, int inCurrentSpeed)
        {
            int bX, bY;
            int aX1, aY1;
            int aX2, aY2;
            int aTheta = 0;
            int aScaleRatio = inCX / 2;
            int aPinWidth = 5 * aScaleRatio / 100;

            RectangleF aGaugeRect = new RectangleF(0, 0, ((float)inCX * 2 - 1), ((float)inCY * 2 - 1));

            /* 針を描画(240deg分、オフセット60deg) */
            aTheta = ((inCurrentSpeed * 2400) / inMaxSpeed) + 600;

            aX1 = inCX + Convert.ToInt32(aPinWidth * Math.Cos((aTheta - 2700 - 900) * 2 * Math.PI / 3600));
            aY1 = inCY + Convert.ToInt32(aPinWidth * Math.Sin((aTheta - 2700 - 900) * 2 * Math.PI / 3600));
            aX2 = inCX + Convert.ToInt32(aPinWidth * Math.Cos((aTheta - 2700 + 900) * 2 * Math.PI / 3600));
            aY2 = inCY + Convert.ToInt32(aPinWidth * Math.Sin((aTheta - 2700 + 900) * 2 * Math.PI / 3600));


            /* 針の位置を演算 */
            bX = inCX + Convert.ToInt32(inRadius * Math.Cos((aTheta - 2700) * 2 * Math.PI / 3600));
            bY = inCY + Convert.ToInt32(inRadius * Math.Sin((aTheta - 2700) * 2 * Math.PI / 3600));

            Point[] aPoints = {new Point(aX1, aY1),
                     new Point(aX2, aY2),
                     new Point(bX, bY)};

            inCanvas.FillPolygon(Brushes.Red, aPoints, FillMode.Alternate);

            /* 針の中心 */
            aGaugeRect.Inflate((float)(-inRadius + 1), (float)(-inRadius + 1));
            LinearGradientBrush aGBrush = new LinearGradientBrush(aGaugeRect, Color.FromArgb(80, 0, 0), Color.FromArgb(160, 0, 0), LinearGradientMode.ForwardDiagonal);
            inCanvas.FillEllipse(aGBrush, aGaugeRect);

            aGaugeRect.Inflate(-1, -1);
            LinearGradientBrush aGBrush2 = new LinearGradientBrush(aGaugeRect, Color.FromArgb(180, 20, 20), Color.FromArgb(100, 0, 0), LinearGradientMode.ForwardDiagonal);
            inCanvas.FillEllipse(aGBrush2, aGaugeRect);


            aGBrush.Dispose();
            aGBrush2.Dispose();
           

           
        }


        public void DrawS88SensorDisplay(Graphics inCanvas, int[] inS88Flags, int inScaleRatio)
        {
            int i, j;
            int aDrawID;
            int aX, aY;
            int aSize = 14 * inScaleRatio / 100;

            /* 文字系 */
            System.Drawing.Font aDrawFont = new System.Drawing.Font("Arial", 10 * inScaleRatio / 100, FontStyle.Bold);
            StringFormat aDrawFormat = new StringFormat();

            /* フラグ取得 */

            for (i = 0; i < 16; i++)
            {
                aX = i * 45;

                for (j = 0; j < 16; j++)
                {
                    if (((inS88Flags[i] >> j) & 0x01) == 1)
                    {
                        aDrawID = 1;

                    }
                    else
                    {
                        aDrawID = 0;

                    }

                    aY = j * 26 + 10;

                    switch (aDrawID)
                    {
                        case 0:
                            inCanvas.FillEllipse(Brushes.Red, aX * inScaleRatio / 100, aY * inScaleRatio / 100, aSize, aSize);
                            break;
                        case 1:
                            inCanvas.FillEllipse(Brushes.Green, aX * inScaleRatio / 100, aY * inScaleRatio / 100, aSize, aSize);
                            break;
                    }

                    inCanvas.DrawString((i * 16 + j + 1).ToString(), aDrawFont, Brushes.Black, (aX + 16) * inScaleRatio / 100, aY * inScaleRatio / 100);

                }
            }

            /* 開放 */
            aDrawFormat.Dispose();
            aDrawFont.Dispose();

        }

        public void DrawBottomoBarS88Sensors(Graphics inCanvas, int[] inS88Flags, int inGroupNo, int inScaleRatio)
        {
            int j;
            int aDrawID;
            int aX, aY;
            int aSize = 14 * inScaleRatio / 100;

            /* 文字系 */
            System.Drawing.Font aDrawFont = new System.Drawing.Font("Arial", 10, FontStyle.Bold);
            StringFormat aDrawFormat = new StringFormat();

            /* フラグ取得 */


            for (j = 0; j < 16; j++)
            {
                if (((inS88Flags[inGroupNo] >> j) & 0x01) == 1)
                {
                    aDrawID = 1;

                }
                else
                {
                    aDrawID = 0;

                }

                aX = (j % 8) * 40 + 5;
                aY = 5 + (j / 8) * 30;

                switch (aDrawID)
                {
                    case 0:
                        inCanvas.FillEllipse(Brushes.Red, aX * inScaleRatio / 100, aY * inScaleRatio / 100, aSize, aSize);
                        break;
                    case 1:
                        inCanvas.FillEllipse(Brushes.Green, aX * inScaleRatio / 100, aY * inScaleRatio / 100, aSize, aSize);
                        break;
                }

                inCanvas.DrawString((inGroupNo * 16 + j + 1).ToString(), aDrawFont, Brushes.Black, (aX + 14)  * inScaleRatio / 100, aY * inScaleRatio / 100);

            }

            /* 開放 */
            aDrawFormat.Dispose();
            aDrawFont.Dispose();
        }


        public void DrawSignal(Graphics inCanvas, int inX, int inY, int inIndex, int inPower)
        {
            int aX;
            int aY;

            if ((inIndex == 5) || (inIndex == 7))
            {
                /* 右上 */
                aX = 24;
                aY = 0;
            }
            else
            {
                /* 左上 */
                aX = 0;
                aY = 0;
            }

            if (inIndex != 8)
            {

                inCanvas.FillEllipse(Brushes.Gray, inX + aX, inY, 8, 16);

                if (inPower == 0)
                {
                    inCanvas.FillEllipse(Brushes.Red, inX + aX + 1, inY + aY + 9, 6, 6);
                }
                else
                {
                    inCanvas.FillEllipse(Brushes.Green, inX + aX + 1, inY + aY + 1, 6, 6);
                }
            }
            else
            {
                inCanvas.FillEllipse(Brushes.Gray, inX + aX, inY, 16, 8);

                if (inPower == 0)
                {
                    inCanvas.FillEllipse(Brushes.Red, inX + aX + 9, inY + aY + 1, 6, 6);
                }
                else
                {
                    inCanvas.FillEllipse(Brushes.Green, inX + aX + 1, inY + aY + 1, 6, 6);
                }
            }
        }

        public void DrawS88Sensor(Graphics inCanvas, int inX, int inY, bool inFlag, int inPicIndex, int inScaleRatio)
        {
            int aX = 12 * inScaleRatio / 100;
            int aY = 12 * inScaleRatio / 100;

            switch(inPicIndex)
            {
                case 4:
                    aX = 20 * inScaleRatio / 100;
                    aY = 20 * inScaleRatio / 100;
                    break;
                case 5:
                    aX = 4 * inScaleRatio / 100;
                    aY = 20 * inScaleRatio / 100;
                    break;
                case 6:
                    aX = 20 * inScaleRatio / 100;
                    aY = 4 * inScaleRatio / 100;
                    break;
                case 7:
                    aX = 4 * inScaleRatio / 100;
                    aY = 4 * inScaleRatio / 100;
                    break;
                default:
                    break;
            }


            if (inFlag == true)
            {
                inCanvas.FillEllipse(Brushes.Red, inX + aX, inY + aY, 6 * inScaleRatio / 100, 6 * inScaleRatio / 100);
            }
            else
            {
                inCanvas.FillEllipse(Brushes.Gray, inX + aX, inY + aY, 6 * inScaleRatio / 100, 6 * inScaleRatio / 100);
            }

        }


        public void DrawLeverBox_Speed(Graphics inCanvas, int inLocCurrentSpeed, int inLocMaxSpeed)
        {
            int aX, aY, bX, bY;
            int aLeverCntMax;
            int i;

            //ペンオブジェクトの作成
            Pen aPen = new Pen(Color.LightGray);
            aPen.Width = Program.PEN_WIDTH;

            /* ハンドル稼動部を書く */
            aLeverCntMax = Program.LEVER_HEIGHT / 10;

            for (i = 0; i < aLeverCntMax; i++)
            {

                aX = Program.LEVER_STEP_WIDTH - (Program.LEVER_STEP_WIDTH * (Program.LEVER_HEIGHT - i * 10) / Program.LEVER_HEIGHT);
                aY = Program.LEVER_BAR_TOP + i * 10;
                bX = Program.LEVER_STEP_WIDTH;
                bY = aY;

                aPen.Width = 5;
                aPen.Color = Color.FromArgb(255 - i * 2550 / Program.LEVER_HEIGHT, (aLeverCntMax - i) <= (inLocCurrentSpeed * aLeverCntMax / inLocMaxSpeed) ? 255 : 100, 100, 100);
                inCanvas.DrawLine(aPen, aX, aY, bX, bY);
            }

            /* ハンドル稼動部を書く */

            aX = Program.LEVER_PIN_LEFT + Program.LEVER_PIN_WIDTH / 2 - 1;
            aY = Program.LEVER_BAR_TOP;
            bX = 10;
            bY = Program.LEVER_HEIGHT;

            aPen.Width = 1;
            aPen.Color = Color.DarkGray;
            inCanvas.FillRectangle(Brushes.LightGray, aX, aY, bX, bY);
            inCanvas.DrawRectangle(aPen, aX, aY, bX, bY);


            /* レバーを書く */
            aX = Program.LEVER_PIN_LEFT + 14;
            aY = 1 + (Program.LEVER_HEIGHT * (inLocMaxSpeed - inLocCurrentSpeed) / inLocMaxSpeed);
            bX = 28;//Program.LEVER_PIN_WIDTH;
            bY = 28;

            aPen.Width = Program.LEVER_PIN_PENWIDTH;
            RectangleF aGaugeRect = new RectangleF((float)aX, (float)aY, (float)bX, (float)bY);

            LinearGradientBrush aGBrush3 = new LinearGradientBrush(aGaugeRect, Color.DarkGray, Color.Gray, LinearGradientMode.ForwardDiagonal);
            inCanvas.FillEllipse(aGBrush3, aGaugeRect);

            aGaugeRect.Inflate(-1, -1);
            LinearGradientBrush aGBrush = new LinearGradientBrush(aGaugeRect, Color.LightGray, Color.DimGray, LinearGradientMode.ForwardDiagonal);
            inCanvas.FillEllipse(aGBrush, aGaugeRect);

            aGBrush.Dispose();
            aGBrush3.Dispose();
            aPen.Dispose();


        }

        private void DrawSpeedPowerBar(Graphics inCanvas, int inX, int inY, int inValue, int inMax, bool inAcc, int inScale)
        {
            int i;
            int aX, aY, bX, bY;
            int aBarHeight = 130;

            Pen aPen = new Pen(Color.LightGray);
            aPen.Width = (4 * inScale) / 100;

            i = 0;
            while ( i < aBarHeight)
            {
                if (inAcc == true)
                {

                    aX = ((30 - ((aBarHeight - i) * 30) / aBarHeight) * inScale) / 100;
                    aY = inY + (i * inScale) / 100;
                    bX = (30 * inScale) / 100;
                    bY = aY;

                    if (inValue > 0)
                    {
                        aPen.Color = Color.FromArgb(128 + 64 - i * 64 / aBarHeight, (aBarHeight - i) <= (inValue * aBarHeight) / inMax ? 255 : 100, 100, 100);
                    }
                    else
                    {
                        aPen.Color = Color.FromArgb(128 + 64 - i * 64 / aBarHeight, 100, 100, 100);
                    }

                    inCanvas.DrawLine(aPen, aX, aY, bX, bY);

                }
                else
                {

                    aX = ((30 - (i * 30) / aBarHeight) * inScale) / 100;
                    aY = inY + (i * inScale) / 100; 
                    bX = (30 * inScale) / 100;
                    bY = aY;

                    if (inValue < 0)
                    {
                        aPen.Color = Color.FromArgb(128 + i * 64 / aBarHeight, 100, 100, i <= (-inValue * aBarHeight) / inMax ? 255 : 100);
                    }
                    else
                    {
                        aPen.Color = Color.FromArgb(128 + i * 64 / aBarHeight, 100, 100, 100);
                    }
                        
                    inCanvas.DrawLine(aPen, aX, aY, bX, bY);

                }

                i = i + 8;

            }
        }


        public void DrawLeverBoxPower(Graphics inCanvas, int inGears, int inLeverValue, int inEnable, int inScale)
        {
            int aX, aY, bX, bY;
            int aMax;
            int aMin;
            int aCount;
            int aPosition;

            /* 目盛り数を取得 */
            aMin = -1 * inGears;
            aMax = inGears;

            aCount = aMax - aMin;

            //ペンオブジェクトの作成
            Pen aPen = new Pen(Color.DarkGray);

            /* 真ん中 */
            aPen.Width = 1;
            aPen.Color = inLeverValue == 0 ? Color.Red : Color.DarkGray;
            inCanvas.DrawEllipse(aPen, (25 * inScale) / 100, ((136 + Program.LEVER_BAR_TOP) * inScale) / 100, (8 * inScale) / 100, (8 * inScale) / 100);

            /* スピードバーを書く */
            DrawSpeedPowerBar(inCanvas, 0, ((Program.LEVER_BAR_TOP + 2) * inScale) / 100, inLeverValue, inGears, true, inScale);
            DrawSpeedPowerBar(inCanvas, 0, ((Program.LEVER_BAR_TOP + 150) * inScale) / 100, inLeverValue, inGears, false, inScale);

            /* 文字系 */
            System.Drawing.Font aDrawFont = new System.Drawing.Font("Arial", 8, FontStyle.Bold);
            StringFormat aDrawFormat = new StringFormat();
            aDrawFormat.Alignment = StringAlignment.Near;

            inCanvas.DrawString("P", aDrawFont, Brushes.DimGray, 0, (120 * inScale) / 100);
            inCanvas.DrawString("B", aDrawFont, Brushes.DimGray, 0, (180 * inScale) / 100);



            /* ハンドル稼動部を書く */

            aX = ((Program.LEVER_PIN_LEFT + 14) * inScale) / 100;
            aY = (Program.LEVER_BAR_TOP * inScale) / 100;
            bX = (10 * inScale) / 100;
            bY = (Program.LEVER_HEIGHT * inScale) / 100;

            aPen.Width = 1;
            aPen.Color = Color.DarkGray;
            inCanvas.FillRectangle(Brushes.LightGray, aX, aY, bX, bY);
            inCanvas.DrawRectangle(aPen, aX, aY, bX, bY);


            /* レバーを書く */

            aPosition = Math.Abs(inLeverValue - aMax);

            aX = ((Program.LEVER_PIN_LEFT + 5) * inScale) / 100;
            aY = ((Program.LEVER_BAR_TOP + (Program.LEVER_HEIGHT * aPosition / aCount) - 14) * inScale) / 100;
            bX = (28 * inScale) / 100;//Program.LEVER_PIN_WIDTH;
            bY = (28 * inScale) / 100;

            aPen.Width = Program.LEVER_PIN_PENWIDTH;
            aPen.Color = Color.DarkGray;
            RectangleF aGaugeRect = new RectangleF((float)aX, (float)aY, (float)bX, (float)bY);

            LinearGradientBrush aGBrush3 = new LinearGradientBrush(aGaugeRect, Color.DarkGray, Color.Gray, LinearGradientMode.ForwardDiagonal);
            inCanvas.FillEllipse(aGBrush3, aGaugeRect);

            aGaugeRect.Inflate(-1, -1);
            LinearGradientBrush aGBrush = new LinearGradientBrush(aGaugeRect, Color.LightGray, Color.DimGray, LinearGradientMode.ForwardDiagonal);
            inCanvas.FillEllipse(aGBrush, aGaugeRect);

            /* 開放 */
            aGBrush.Dispose();
            aGBrush3.Dispose();
            aDrawFont.Dispose();
            aDrawFormat.Dispose();
            aPen.Dispose();

        }

        public void DrawLocLabel(Graphics inCanvas, int inX, int inY, int Width, int inHeight, int inIndex, Brush inBrush, LocomotiveDB inLocDB, string inNotSelectLabel, int inScaleRatio)
        {
            string aLocName;
            int aLeft = 0;

            //(アンチエイリアス処理されたレタリング)を指定する
            inCanvas.SmoothingMode = SmoothingMode.AntiAlias;
            inCanvas.TextRenderingHint = TextRenderingHint.AntiAlias;

            if ((inIndex >= 0) && (inIndex < inLocDB.Items.Count))
            {


                string aName = inLocDB.GetProtcolName(inLocDB.GetAddressLocProtcol(inLocDB.Items[inIndex].mLocAddr));
                drawLocComboBoxItem(inCanvas, inX, inY, inHeight, inLocDB.GetAddress(inLocDB.Items[inIndex].mLocAddr), aName, inScaleRatio);

                /* 文字の表示位置調整 */
                aLeft = 26 * inScaleRatio / 100;

                if (inLocDB.GetAddress(inLocDB.Items[inIndex].mLocAddr_dbl) > 0)
                {
                    aName = inLocDB.GetProtcolName(inLocDB.GetAddressLocProtcol(inLocDB.Items[inIndex].mLocAddr_dbl));
                    drawLocComboBoxItem(inCanvas, inX + aLeft, inY, inHeight, inLocDB.GetAddress(inLocDB.Items[inIndex].mLocAddr_dbl), aName, inScaleRatio);

                    /* 文字の表示位置調整 */
                    aLeft = aLeft * 2;
                }


                /* 機関車名 */
                aLocName = inLocDB.Items[inIndex].mLocName;

            }
            else
            {
                aLocName = inNotSelectLabel;
                aLeft = 0;
            }

            /* 機関車名描画 */
            System.Drawing.Font aDrawFont2 = new System.Drawing.Font("Arial", 15 * inScaleRatio / 100, FontStyle.Bold);
            float aYPos = (inHeight - inCanvas.MeasureString(aLocName, aDrawFont2).Height) / 2;
            inCanvas.DrawString(aLocName, aDrawFont2, inBrush, inX + (aLeft * inScaleRatio / 100), inY + aYPos);

            /* 開放 */
            aDrawFont2.Dispose();

        }

        private void drawLocComboBoxItem(Graphics inCanvas, int inX, int inY, int inHeight, int inLocAddr, string inProtcolName, int inScaleRatio)
        {
            int aAddrSize;

            //ペンオブジェクトの作成
            Pen aPen = new Pen(Color.DarkGray);

            /* 文字系 */
            System.Drawing.Font aDrawFont = new System.Drawing.Font("Arial", 7 * inScaleRatio / 100, FontStyle.Bold);

            /* プロトコルアイコン */
            float aHeight = inCanvas.MeasureString(inProtcolName, aDrawFont).Height;
            float aWidth = inCanvas.MeasureString(inProtcolName, aDrawFont).Width;
            float aYPos = (inHeight - aHeight) - 1;

            inCanvas.FillRectangle(Brushes.Black, inX, inY + aYPos, aWidth, aHeight - 1);
            inCanvas.DrawString(inProtcolName, aDrawFont, Brushes.White, inX, inY + aYPos);

            /* アドレス */
            string aLocAddress = inLocAddr.ToString();
            aAddrSize = inLocAddr >= 1000 ? 7 : 10;
            System.Drawing.Font aDrawFont2 = new System.Drawing.Font("Arial", aAddrSize * inScaleRatio / 100, FontStyle.Bold);

            aYPos = (inHeight - inCanvas.MeasureString(aLocAddress, aDrawFont2).Height) / 2;
            float aXPos = (aWidth - inCanvas.MeasureString(aLocAddress, aDrawFont2).Width) / 2;
            inCanvas.DrawString(aLocAddress, aDrawFont2, Brushes.Black, inX + aXPos, inY + 0);

            aDrawFont.Dispose();
            aDrawFont2.Dispose();
            aPen.Dispose();

        }



    }
}

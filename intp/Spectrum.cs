using aud;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using static aud.AudioUt;

namespace intp
{
    //may be disposed in the middle of processing...
    //other pixel modes more efficient?
    //may need to raise event to say needs redrawing - resized gets away with it.
    //rounding issues with maxF
    //definately don't let frequency bounds be equal or cross.
    //working on - see improvements
    internal class Spectrum : IDisposable
    {
        //generally assumes not disposed.

        Object DrawingLock = new object();
        // do I need 2 different locks? Will have to clafiry on method names.
        //Lock must be used when changing these or accessing graphic objects at all
        double iMinFreq;
        double iMaxFreq;
        Size iSize;
        Bitmap bmp;
        Graphics gr;

        public Spectrum(double MinFreq, double MaxFreq, Size Size)
        {
            lock (DrawingLock)
            {
                iMinFreq = MinFreq;
                iMaxFreq = MaxFreq;
                iSize = Size;
                bmp = new Bitmap(Size.Width, Size.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                gr = Graphics.FromImage(bmp);
            }
        }

        (Bitmap, Graphics) LockedAdapt(double newMinFreq, double newMaxFreq, Size newSize)
        {
            var tr = new Bitmap(newSize.Width, newSize.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            var grNew = Graphics.FromImage(tr);
            var oldUpper = (float)((Math.Log(newMaxFreq) - Math.Log(iMaxFreq)) / (Math.Log(newMaxFreq) - Math.Log(newMinFreq)));
            var oldHeight = (float)((Math.Log(iMaxFreq) - Math.Log(iMinFreq)) / (Math.Log(newMaxFreq) - Math.Log(newMinFreq)));
            grNew.DrawImage(bmp, 0F, newSize.Height * oldUpper, newSize.Width, newSize.Height * oldHeight);
            return (tr, grNew);
        }

        public double MinFreq
        {
            get => iMinFreq;
            set
            {
                lock (DrawingLock)
                {
                    var result = LockedAdapt(value, iMaxFreq, iSize);
                    gr.Dispose();
                    gr = result.Item2;
                    bmp.Dispose();
                    bmp = result.Item1;
                    iMinFreq = value;
                }
            }
        }

        //may not need to throw away if size not changed.
        //check if values are different first...
        public double MaxFreq
        {
            get => iMaxFreq;
            set
            {
                lock (DrawingLock)
                {
                    var result = LockedAdapt(iMinFreq, value, iSize);
                    gr.Dispose();
                    gr = result.Item2;
                    bmp.Dispose();
                    bmp = result.Item1;
                    iMaxFreq = value;
                }
            }
        }

        public Size Size
        {
            get => iSize;
            set
            {
                lock (DrawingLock)
                {
                    var result = LockedAdapt(iMinFreq, iMaxFreq, value);
                    gr.Dispose();
                    gr = result.Item2;
                    bmp.Dispose();
                    bmp = result.Item1;
                    iSize = value;
                }
            }
        }

        public int Height
        {
            get => iSize.Height;
            set => Size = new Size(iSize.Width, value);
        }

        public int Width
        {
            get => iSize.Width;
            set => Size = new Size(value, iSize.Height);
        }

        public void Draw(Graphics g, Size Size)
        {
            lock (DrawingLock)
            {
                g.DrawImage(bmp, new Rectangle(default, Size));
            }
        }

        public void Draw(Graphics g, Size Size, float X, float ABSdX)
        {
            lock (DrawingLock)
            {
                g.DrawImage(bmp, new Rectangle(default, Size));
                MarkPosition(X, ABSdX, g, Size.Width, Size.Height);
            }
        }

        void LockedPostDrawSpectFRsAbs(IEnumerable<(Brush, float, float, float, float)> input)
        {
            foreach (var item in input)
            {
                gr.FillRectangle(item.Item1, item.Item2, item.Item3, item.Item4, item.Item5);
            }
        }
        
        //should change soon and maybe above also.
        void LockedPostDrawSpectFrRel(Brush Brush, float X, float Y, float w, float h, int SnapWidth, int SnapHeight)
        {
            gr.FillRectangle(Brush, X * SnapWidth, Y * SnapHeight, w * SnapWidth, h * SnapHeight);
        }

        //would color slow down??, off by...
        //probably goes wrong when small number of frequencies testing
        //would array with nulls be faster
        static List<(Brush, float, float, float, float)> PreDrawSpect(int Width, int Height, double[] result2DB, float X, float dX, double TFloorDB, double TCeilDB,
            double rMax, double gMax, double bMax)
        {
            var tr = new List<(Brush, float, float, float, float)>();
            var scY = (float)Height / result2DB.Length;
            for (int i = 0; i < result2DB.Length; i++)
            {
                var Tpp = (result2DB[i] - TFloorDB) / (TCeilDB - TFloorDB);
                if (double.IsNaN(Tpp) || Tpp <= 0) continue;
                if (Tpp > 1) Tpp = 1;
                //should maybe validate after multiplitacion...
                //dispose of brush...
                var b = new SolidBrush(Color.FromArgb((int)(Tpp * rMax), (int)(Tpp * gMax), (int)(Tpp * bMax)));
                tr.Add((b, Width * X, (Height - scY) - (Height - scY) * (i / (float)(result2DB.Length - 1)), Width * dX, scY));
            }
            return tr;
        }

        //(Pen, float, float, float, float)[] DISCARDEDPreDrawSpect((double, double)[] result, Size sz, float FloorDB, float CeilDB)
        //{
        //    var tr = new (Pen, float, float, float, float)[result.Length];
        //    var oldy = 0F;
        //    var oldx = 0F;
        //    for (int i = 0; i < result.Length; i++)
        //    {
        //        var x = i / (float)(result.Length);
        //        var y = (ToDB((float)result[i].Item2) - FloorDB) / (CeilDB - FloorDB);
        //        if (float.IsNaN(y) || y < 0) y = 0;
        //        tr[i] = (Pens.White, oldx * sz.Width, sz.Height - sz.Height * oldy, x * sz.Width, sz.Height - sz.Height * y);
        //        oldy = y;
        //        oldx = x;
        //    }
        //    return tr;
        //}

        //does color slow down?
        //check instance creation of analysisrange.
        //ARBITRARY CHOICES??
        static Complex[] ProcessData(Wave subject, int Avr, Double AliasThr, Fourier.AnalysisRange Range, int local, int SampleCount)
        {
            (Fourier.AnalysisRange, Complex[]) r = (null, null);
            var SafeHalfLength = (int)(subject.SampleRate * local / Range.Frequencies[0] / 2);
            Fourier.iGetGraph(Fourier.WindowType.Hanning, SafeHalfLength, local, SampleCount, 0, AliasThr, ref r, Range,
                subject.extract(Avr - SafeHalfLength, SafeHalfLength * 2), (int)subject.SampleRate);

            return r.Item2;
        }

        //watch out for clippign off top.
        //HERE and references
        static Double[] ProcessFundamentalsDB(double[] inputDB, int or, double additionDB, int harmonics) 
        {

            var hs = new int[harmonics + 1];
            for (int i = 0; i <= harmonics; i++)
            {
                hs[i] = (int)(Math.Log(i + 1, 2) * or);
            }
            var hl = hs.Last();

            var trDB = new double[inputDB.Length];

            for (int i = 0; i < inputDB.Length - hl; i++)
            {
                trDB[i] = hs.Aggregate(0D, (x, y) => x + inputDB[y + i]);
                trDB[i] /= (harmonics + 1);
                trDB[i] += additionDB;
            }
            for (int i = inputDB.Length - hl; i < inputDB.Length; i++)
            {
                trDB[i] = double.NegativeInfinity;
            }

            return trDB;
        }

        //private float totalFloorDB => (FloorDB * hsScale);
        //private float totalCeilDB => (CeilDB * hsScale);
        //private float totalFloorLevel => FromDB(totalFloorDB);
        //static (int, float)[] hs = new (int, float)[] { (br * 12, 1), (br * 19, 1), (br * 24, 0.95F), (br * 28, 0.9F), (br * 31, 0.8F) };
        //static float hsScale = hs.Aggregate(0F, (x, y) => x + y.Item2) + 1;
        //static int hsl = hs.Length == 0 ? 0 : hs.Last().Item1;

        //are these effective??
        //NO VALIDATION DONE
        public int GraphicsQueue => gQueue;
        public int ProcessQueue => pQueue;

        public int GraphicsQueueMax
        {
            get => gQueueMax;
            set => gQueueMax = value;
        }

        public int ProcessQueueMax
        {
            get => pQueueMax;
            set => pQueueMax = value;
        }

        //working on performance reporting.
        //improvenemts: use better locking, range property, stop generating new range object, uses main floor for aliases
        int gQueue = 0;
        int gQueueMax = 3;
        int pQueue = 0;
        int pQueueMax = 8;
        public async Task ProcessPosition(Wave Data, int positionAvr, float X, float dX, double FloorDB, double CeilDB, int or, int Local, int SampleCount, bool FollowAls,
            int fundHarms, double fundAdditionDB, bool HideHarmonics)
        {

            var SnapWidth = iSize.Width;
            var SnapHeight = iSize.Height;
            var SnapRange = new Fourier.AnalysisRange(iMinFreq, iMaxFreq, or);

            await Task.Run(() =>
            {
                if (pQueue < pQueueMax)
                {
                    pQueue++;
                    var tResultDB = Array.ConvertAll(ProcessData(Data, positionAvr, FollowAls ? FromDB(FloorDB) : 0, SnapRange, Local, SampleCount), (x) => ToDB(x.Magnitude));
                    var dataHarm =             PreDrawSpect(SnapWidth, SnapHeight,                       tResultDB,                           X, dX, FloorDB, CeilDB, 255, 255, 255);
                    var dataFund = fundHarms > 0 ? PreDrawSpect(SnapWidth, SnapHeight, ProcessFundamentalsDB(tResultDB, or, fundAdditionDB, fundHarms), X, dX, FloorDB, CeilDB,   0, 255,   0) : null;
                    pQueue--;
                    if (gQueue < gQueueMax)
                    {
                        gQueue++;
                        lock (DrawingLock) { LockedPostDrawSpectFrRel(Brushes.Black, X, 0, dX, 1, SnapWidth, SnapHeight);
                                             if (fundHarms == 0 || !HideHarmonics) LockedPostDrawSpectFRsAbs(dataHarm);
                                             if (fundHarms > 0) LockedPostDrawSpectFRsAbs(dataFund); }
                        gQueue--;
                    }
                    else
                    {
                        lock (DrawingLock) { LockedPostDrawSpectFrRel(Brushes.Red, X, 0, dX, 1, SnapWidth, SnapHeight); }
                    }
                }
                else
                {
                    lock (DrawingLock) { LockedPostDrawSpectFrRel(Brushes.Red, X, 0, dX, 1, SnapWidth, SnapHeight); }
                }
            }
            );
        }

        static void MarkPosition(float X, float ABSdX, Graphics g, int NewWidth, int NewHeight)
        {
            g.FillRectangle(Brushes.Blue, X * NewWidth, 0, ABSdX, NewHeight);
        }

        //better disposal internally and externally
        //could use lock to indicate disposal
        public void Dispose()
        {
            if (gr is null) return;
            lock (DrawingLock)
            {
                gr.Dispose();
                bmp.Dispose();
                gr = null;
                bmp = null;
            }
        }
    }
}

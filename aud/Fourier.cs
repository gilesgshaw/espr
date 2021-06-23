using System;
using System.Collections.Generic;
using System.Numerics;

namespace aud
{
    public static class Fourier
    {

        //IS HEIGHT QUITE RIGHT?
        public class AnalysisRange
        {

            public double LowFrequency { get; }
            public double HighFrequency { get; }
            public int OctaveResolution { get; }
            public int Height { get; }
            public double[] Frequencies { get; }

            public AnalysisRange(double LowFrequency, double HighFrequency, int OctaveResolution)
            {
                this.LowFrequency = LowFrequency;
                this.HighFrequency = HighFrequency;
                this.OctaveResolution = OctaveResolution;
                Height = System.Convert.ToInt32(Math.Floor(Math.Log(this.HighFrequency / this.LowFrequency, 2) * this.OctaveResolution)) + 1;
                Frequencies = new double[Height];
                for (var i = 0; i <= Frequencies.Length - 1; i++)
                    Frequencies[i] = Math.Pow(2, i / (double)this.OctaveResolution) * this.LowFrequency;
            }
        }

        //public enum PointType2 : byte
        //{
        //    Normal,
        //    Peak,
        //    Fundamental,
        //    Overtone
        //}

        //No PROCESS
        //if (Input.Threshold > 0)
        //{
        //    for (var y = 0; y < AllData.Length; y++)
        //    {
        //        if (AllData[y].Magnitude < Input.Threshold)
        //            AllData[y] = new Complex(double.NaN, double.NaN);
        //    }
        //}

        //NO DISTRUBUTE (no process, threshold) OPTION REMOVED
        //THIS WAS JUST TO DELETE BELOW A THRESHOLD
        //Cancellation token removed, and set text thing, and prog thing

        const double maxF = 10000 * 2 * Math.PI;
        //to avoid aliases we need SR at least f + highest frequency present
        /// <summary>for input threshold to be set must be reusing, but aliasthr is fine. Item1 null tested.</summary>
        ///     ''' <exception cref="ArgumentException">Result not Compatable</exception>
        public static void iGetGraph(WindowType Window, int Position, int LocalFrequency, int SampleCount, double InputThreshold,
            double AliasThreshold, ref (AnalysisRange, Complex[]) Result, AnalysisRange Range, float[] Subject, int SampleRate)
        {
            double ModmaxF = maxF / SampleRate;
            var AllData = new Complex[Range.Height];
            if (Result.Item1 != null)
            {
                if (Result.Item1.LowFrequency != Range.LowFrequency)
                    throw new ArgumentException("Result not Compatable");
                if (Result.Item1.HighFrequency != Range.HighFrequency)
                    throw new ArgumentException("Result not Compatable");
                var YFactor = Range.OctaveResolution / Result.Item1.OctaveResolution;
                if (YFactor * Result.Item1.OctaveResolution != Range.OctaveResolution)
                    throw new ArgumentException("Result not Compatable");
                AllData = Distribute(Result.Item2, YFactor, AllData.Length);
            }
            for (var f = 0; f <= Range.Height - 1; f++)
            {
                if (InputThreshold > 0 && (double.IsNaN(AllData[f].Real) || AllData[f].Magnitude < InputThreshold))
                    AllData[f] = new Complex(double.NaN, double.NaN);
                else
                {
                    var NewSampleCount = SampleCount;
                    var NewUSR = GetUnderSamplingRate(Range.Frequencies[f], ref NewSampleCount, LocalFrequency, SampleRate);
                    var RealPhaseFactor = Range.Frequencies[f] / SampleRate * Math.PI * 2;
                    //FinalUSR set even if not used.
                    var FinalUSR = Math.Max(1, (int)Math.Floor(2 * Math.PI / (RealPhaseFactor + ModmaxF)));
                    AllData[f] = iTestPoint(Subject, Window, NewSampleCount, Position, NewUSR, RealPhaseFactor, AliasThreshold, FinalUSR);
                }
            }
            Result.Item1 = Range;
            Result.Item2 = AllData;
        }

        public static Complex[] Distribute(Complex[] initial, int YFactor, int Height)
        {
            var AllData = new Complex[Height];
            for (var y = 0; y < Height; y++)
            {
                var baseY = y / YFactor;
                if (baseY * YFactor < y && baseY + 1 < initial.Length)
                    AllData[y] = initial[baseY].Magnitude > initial[baseY + 1].Magnitude ? initial[baseY] : initial[baseY + 1];
                else
                    AllData[y] = initial[baseY];
            }
            return AllData;
        }

        //FLOOR HERE HOPEFULLY CORRECT EFFECT
        public static int GetUnderSamplingRate(double Frequency, ref int SampleCount, int LocalFrequency, int SampleRate)
        {
            var tr = Math.Max(1, System.Convert.ToInt32(LocalFrequency / Frequency * SampleRate / SampleCount));
            SampleCount = (int)(Math.Floor(LocalFrequency / Frequency * SampleRate / tr));
            return tr;
        }

        private static Complex iiTestPoint(float[] Obj, WindowType Type, int SampleCount, int Position, int UnderSamplingRate, double RealPhaseFactor)
        {
            var Result = new Complex();
            var CStartPosition = Position - (UnderSamplingRate * SampleCount / 2);
            for (var CurrentOffset = 0; CurrentOffset < SampleCount * UnderSamplingRate; CurrentOffset += UnderSamplingRate)
            {
                var tr = Complex.FromPolarCoordinates(
                    GetWindow((double)CurrentOffset / SampleCount / UnderSamplingRate, Type) * Obj[(CStartPosition + CurrentOffset)] * 2 / (double)SampleCount,
                    (CStartPosition + CurrentOffset) * RealPhaseFactor
                                                                          );
                Result += tr;
            }
            return Result;
        }

        //Final USR only used if AliasThreshold set.
        private static Complex iTestPoint(float[] Obj, WindowType Type, int SampleCount, int Position, int UnderSamplingRate, double RealPhaseFactor, double AliasThreshold, int FinalUSR)
        {
            if (AliasThreshold > 0)
            {
                var Result = iiTestPoint(Obj, Type, SampleCount, Position, UnderSamplingRate, RealPhaseFactor);
                while (UnderSamplingRate > FinalUSR && Result.Magnitude >= AliasThreshold)
                {
                    var Factor = SmallestFactor(UnderSamplingRate);
                    UnderSamplingRate /= Factor;
                    SampleCount *= Factor;
                    Result = iiTestPoint(Obj, Type, SampleCount, Position, UnderSamplingRate, RealPhaseFactor);
                }
                return (Result.Magnitude >= AliasThreshold) ? Result : new Complex(double.NaN, double.NaN);
            }
            else
            {
                return iiTestPoint(Obj, Type, SampleCount, Position, UnderSamplingRate, RealPhaseFactor);
            }
        }

        private static Dictionary<int, int> iSmallestFactor = new Dictionary<int, int>();
        private static int SmallestFactor(int Input)
        {
            if (!iSmallestFactor.ContainsKey(Input))
            {
                var value = 2;
                while (!(Input % value == 0))
                    value += 1;
                iSmallestFactor.Add(Input, value);
            }
            return iSmallestFactor[Input];
        }

        public enum WindowType
        {
            Hanning,
            None
        }

        private static double GetWindow(double Position, WindowType Type)
        {
            switch (Type)
            {
                case WindowType.Hanning:
                    {
                        return 1 - Math.Cos(Position * 2 * Math.PI);
                    }
                default:
                    {
                        return 1;
                    }
            }
        }
    }
}

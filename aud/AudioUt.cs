using System;

namespace aud
{
    public static class AudioUt
    {

        //here
        public static string FreqToString(double Value, int SignificantFigures = 3)
        {
            if (Value <= 0)
                throw new ArgumentOutOfRangeException("Value");
            var DP = SignificantFigures - System.Convert.ToInt32(Math.Floor(Math.Log10(Value))) - 1;
            for (var i = 1; i <= DP; i++) Value *= 10;
            for (var i = DP; i <= -1; i++) Value /= 10D;
            Value = Math.Round(Value);
            for (var i = 1; i <= DP; i++) Value /= 10D;
            for (var i = DP; i <= -1; i++) Value *= 10;
            if (Value < 1000)
                return Value.ToString() + " hz";
            else
                return (Value / 1000D).ToString() + " Khz";
        }

        //here
        public static string LevelToString(double val)
        {
            if (val == 0)
                return "0 db";
            else if (val <= -10)
                return System.Convert.ToString(Math.Floor(val)) + " db";
            else
                return System.Convert.ToString(Math.Floor(val * 10) / 10) + " db";
        }

        public static double ToDB(double level)
        {
            return 10 * Math.Log(level, 2);
        }

        public static double FromDB(double db)
        {
            return Math.Pow(2, db / 10);
        }

        public static float ToDB(float level)
        {
            return 10 * (float)Math.Log(level, 2);
        }

        public static float FromDB(float db)
        {
            return (float)Math.Pow(2, db / 10);
        }

    }
}

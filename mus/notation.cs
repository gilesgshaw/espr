using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace mus
{

    public static partial class notation
    {
        
        public static string test()
        {
            int g = 0;
            string jsonString = JsonSerializer.Serialize(g);
            return jsonString;
        }

    }

    public static partial class notation
    {

        public static int mod(int b, int a)
        {
            int result = a % b;
            if (result >= 0)
            {
                return result;
            }
            else if (b >= 0)
            {
                return result + b;
            }
            else
            {
                return result - b;
            }
        }

        //placeholder
        public static string QualityName()
        {
            //remember new convention
            throw new NotImplementedException();

            //public enum QualityPf : int
            //{
            //    _5Diminished = -5,
            //    _4Diminished = -4,
            //    _3Diminished = -3,
            //    _2Diminished = -2,
            //    _Diminished = -1,
            //    _Perfect = 0,
            //    _Augmented = 1,
            //    _2Augmented = 2,
            //    _3Augmented = 3,
            //    _4Augmented = 4,
            //    _5Augmented = 5
            //}

            //public enum QualityMi : int
            //{
            //    _4Diminished = -4,
            //    _3Diminished = -3,
            //    _2Diminished = -2,
            //    _Diminished = -1,
            //    _Minor = 0,
            //    _Major = 1,
            //    _Augmented = 2,
            //    _2Augmented = 3,
            //    _3Augmented = 4,
            //    _4Augmented = 5
            //}

        }

        //should this throw the exception?
        public static string AccidentalSymbol(int alt)
        {
            switch (alt)
            {
                case -2:
                    {
                        return "𝄫";
                    }

                case -1:
                    {
                        return "♭";
                    }

                case 0:
                    {
                        return string.Empty;
                    }

                case 1:
                    {
                        return "♯";
                    }

                case 2:
                    {
                        return "𝄪";
                    }

                default:
                    {
                        throw new NotImplementedException();
                    }
            }
        }

    }
}

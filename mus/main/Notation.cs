using System;

namespace mus
{
    public static class Notation
    {

        //should this throw the exception?
        public static string AccidentalSymbol(int alt, bool giveNatural)
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
                        return giveNatural ? "♮" : "";
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

        public static string QualityName(int value, bool cons)
        {
            if (cons)
            {
                if (value < -1)
                {
                    return (-value) + "-Diminished";
                }
                else if (value == -1)
                {
                    return "Diminished";
                }
                else if (value == 0)
                {
                    return "Perfect";
                }
                else if (value == 1)
                {
                    return "Augmented";
                }
                else
                {
                    return value + "-Augmented";
                }
            }
            else
            {
                if (value < -2)
                {
                    return (-1 - value) + "-Diminished";
                }
                else if (value == -2)
                {
                    return "Diminished";
                }
                else if (value == -1)
                {
                    return "Minor";
                }
                else if (value == 0)
                {
                    return "Major";
                }
                else if (value == 1)
                {
                    return "Augmented";
                }
                else
                {
                    return value + "-Augmented";
                }
            }
        }

        //offset is from 0 to 6
        public static class Degree
        {
            public static int Semis(int offset)
            {
                switch (offset)
                {
                    case 0:
                        {
                            return 0;
                        }
                    case 1:
                        {
                            return 2;
                        }
                    case 2:
                        {
                            return 4;
                        }
                    case 3:
                        {
                            return 5;
                        }
                    case 4:
                        {
                            return 7;
                        }
                    case 5:
                        {
                            return 9;
                        }
                    case 6:
                        {
                            return 11;
                        }
                    default:
                        {
                            throw new ArgumentException();
                        }
                }
            }

            public static bool IsConsonant(int offset)
            {
                return offset == 0 || offset == 3 || offset == 4;
            }

            public static string English(int offset)
            {
                switch (offset)
                {
                    case 0:
                        {
                            return "Tonic";
                        }
                    case 1:
                        {
                            return "Supertonic";
                        }
                    case 2:
                        {
                            return "Mediant";
                        }
                    case 3:
                        {
                            return "Subdominant";
                        }
                    case 4:
                        {
                            return "Dominant";
                        }
                    case 5:
                        {
                            return "Submediant";
                        }
                    case 6:
                        {
                            return "Subtonic";
                        }
                    default:
                        {
                            throw new ArgumentException();
                        }
                }
            }

            public static string Roman(int offset)
            {
                switch (offset)
                {
                    case 0:
                        {
                            return "I";
                        }
                    case 1:
                        {
                            return "II";
                        }
                    case 2:
                        {
                            return "III";
                        }
                    case 3:
                        {
                            return "IV";
                        }
                    case 4:
                        {
                            return "V";
                        }
                    case 5:
                        {
                            return "VI";
                        }
                    case 6:
                        {
                            return "VII";
                        }
                    default:
                        {
                            throw new ArgumentException();
                        }
                }
            }

            public static string Tone(int offset)
            {
                switch (offset)
                {
                    case 0:
                        {
                            return "Root";
                        }
                    case 1:
                        {
                            return "Second";
                        }
                    case 2:
                        {
                            return "Third";
                        }
                    case 3:
                        {
                            return "Fourth";
                        }
                    case 4:
                        {
                            return "Fifth";
                        }
                    case 5:
                        {
                            return "Sixth";
                        }
                    case 6:
                        {
                            return "Seventh";
                        }
                    default:
                        {
                            throw new ArgumentException();
                        }
                }
            }

            public static string Interval(int offset)
            {
                switch (offset)
                {
                    case 0:
                        {
                            return "Unison";
                        }
                    case 1:
                        {
                            return "Second";
                        }
                    case 2:
                        {
                            return "Third";
                        }
                    case 3:
                        {
                            return "Fourth";
                        }
                    case 4:
                        {
                            return "Fifth";
                        }
                    case 5:
                        {
                            return "Sixth";
                        }
                    case 6:
                        {
                            return "Seventh";
                        }
                    default:
                        {
                            throw new ArgumentException();
                        }
                }
            }
        }

    }
}

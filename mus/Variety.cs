using System;
using static mus.Ut;

namespace mus
{
    public static partial class notation
    {

        //this uses the wrong convention really
        //TODO: make immutable (as promised to rest of code)
        //should probably seperate this from class 'Mode'
        public class Variety
        {
            //THESE ARE NAMED CONFUSINGLY!
            public virtual int? PQ1 { get; }
            public virtual int? PQ2 { get; }
            public virtual int? PQ3 { get; }
            public virtual int? PQ4 { get; }
            public virtual int? PQ5 { get; }
            public virtual int? PQ6 { get; }
            public virtual int? PQ7 { get; }

            //true for lower case. false for upper case.
            public (bool, string) Symbol { get; }

            public Variety(int? pQ1, int? pQ2, int? pQ3, int? pQ4, int? pQ5, int? pQ6, int? pQ7, (bool, string) symbol)
            {
                PQ1 = pQ1;
                PQ2 = pQ2;
                PQ3 = pQ3;
                PQ4 = pQ4;
                PQ5 = pQ5;
                PQ6 = pQ6;
                PQ7 = pQ7;
                Symbol = symbol;
            }

            protected Variety((bool, string) symbol)
            {
                Symbol = symbol;
            }

            //will accept any 'number' and calculate residue.
            public IntervalS? PIntervalS(int number)
            {
                if (PQualByOffset(number).HasValue) return IntervalS.GetNew(number, PQualByOffset(number).Value);
                return null;
            }

            public IntervalC? PIntervalC(int number)
            {
                if (PQualByOffset(number).HasValue) return new IntervalC(number, PQualByOffset(number).Value, 0);
                return null;
            }

            //will accept any 'number' and calculate residue.
            public int? PQualByOffset(int number)
            {
                switch (mod(7, number))
                {
                    case 0:
                        {
                            return PQ1;
                        }

                    case 1:
                        {
                            return PQ2;
                        }

                    case 2:
                        {
                            return PQ3;
                        }

                    case 3:
                        {
                            return PQ4;
                        }

                    case 4:
                        {
                            return PQ5;
                        }

                    case 5:
                        {
                            return PQ6;
                        }

                    case 6:
                        {
                            return PQ7;
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

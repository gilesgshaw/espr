using System;

namespace mus
{
    public static partial class notation
    {

        //this uses the wrong convention really
        public class Character
        {
            public virtual int? PQ1 { get; }
            public virtual int? PQ2 { get; }
            public virtual int? PQ3 { get; }
            public virtual int? PQ4 { get; }
            public virtual int? PQ5 { get; }
            public virtual int? PQ6 { get; }
            public virtual int? PQ7 { get; }

            public IntervalS? Interval(int number)
            {
                if (PQualByOffset(number).HasValue) return IntervalS.GetNew(number, PQualByOffset(number).Value);
                return null;
            }

            public int? PQualByOffset(int number)
            {
                switch (number)
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

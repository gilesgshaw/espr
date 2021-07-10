using System;

namespace mus
{
    public static partial class notation
    {
        
        //this uses the wrong convention really
        public abstract class Character
        {
            public abstract int? PQ1 { get; }
            public abstract int? PQ2 { get; }
            public abstract int? PQ3 { get; }
            public abstract int? PQ4 { get; }
            public abstract int? PQ5 { get; }
            public abstract int? PQ6 { get; }
            public abstract int? PQ7 { get; }

            public int? PQualByOffset(int offest)
            {
                switch (offest)
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

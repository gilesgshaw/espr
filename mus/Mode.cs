using System;

namespace mus
{

    public static partial class notation
    {
        //this uses the wrong convention really
        public class Mode : ICharactrer
        {

            public int Q1 { get; }
            public int Q2 { get; }
            public int Q3 { get; }
            public int Q4 { get; }
            public int Q5 { get; }
            public int Q6 { get; }
            public int Q7 { get; }

            int? ICharactrer.Q1 => Q1;
            int? ICharactrer.Q2 => Q2;
            int? ICharactrer.Q3 => Q3;
            int? ICharactrer.Q4 => Q4;
            int? ICharactrer.Q5 => Q5;
            int? ICharactrer.Q6 => Q6;
            int? ICharactrer.Q7 => Q7;

            public Mode(int q1, int q2, int q3, int q4, int q5, int q6, int q7)
            {
                Q1 = q1;
                Q2 = q2;
                Q3 = q3;
                Q4 = q4;
                Q5 = q5;
                Q6 = q6;
                Q7 = q7;
            }

            public int QualByOffset(int offest)
            {
                switch (offest)
                {
                    case 0:
                        {
                            return Q1;
                        }

                    case 1:
                        {
                            return Q2;
                        }

                    case 2:
                        {
                            return Q3;
                        }

                    case 3:
                        {
                            return Q4;
                        }

                    case 4:
                        {
                            return Q5;
                        }

                    case 5:
                        {
                            return Q6;
                        }

                    case 6:
                        {
                            return Q7;
                        }

                    default:
                        {
                            throw new ArgumentException();
                        }
                }
            }

            public static readonly Mode Zero = new Mode();

            private Mode()
            {
            }

            public override bool Equals(object obj)
            {
                return obj is Mode mode &&
                       Q1 == mode.Q1 &&
                       Q2 == mode.Q2 &&
                       Q3 == mode.Q3 &&
                       Q4 == mode.Q4 &&
                       Q5 == mode.Q5 &&
                       Q6 == mode.Q6 &&
                       Q7 == mode.Q7;
            }

            public override int GetHashCode()
            {
                int hashCode = -2001665520;
                hashCode = hashCode * -1521134295 + Q1.GetHashCode();
                hashCode = hashCode * -1521134295 + Q2.GetHashCode();
                hashCode = hashCode * -1521134295 + Q3.GetHashCode();
                hashCode = hashCode * -1521134295 + Q4.GetHashCode();
                hashCode = hashCode * -1521134295 + Q5.GetHashCode();
                hashCode = hashCode * -1521134295 + Q6.GetHashCode();
                hashCode = hashCode * -1521134295 + Q7.GetHashCode();
                return hashCode;
            }
        }

    }
}

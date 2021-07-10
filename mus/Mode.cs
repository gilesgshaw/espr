using System;

namespace mus
{
    public static partial class notation
    {

        public class Mode : Character
        {

            public int Q1 { get; }
            public int Q2 { get; }
            public int Q3 { get; }
            public int Q4 { get; }
            public int Q5 { get; }
            public int Q6 { get; }
            public int Q7 { get; }

            public override int? PQ1 => this.Q1;
            public override int? PQ2 => this.Q2;
            public override int? PQ3 => this.Q3;
            public override int? PQ4 => this.Q4;
            public override int? PQ5 => this.Q5;
            public override int? PQ6 => this.Q6;
            public override int? PQ7 => this.Q7;

            public Mode(int q1, int q2, int q3, int q4, int q5, int q6, int q7) : base()
            {
                Q1 = q1;
                Q2 = q2;
                Q3 = q3;
                Q4 = q4;
                Q5 = q5;
                Q6 = q6;
                Q7 = q7;
            }

            public static readonly Mode Zero = new Mode();

            private Mode() : base()
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

            public int QualByOffset(int offset)
            {
                return (int)PQualByOffset(offset);
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

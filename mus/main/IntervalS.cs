﻿using System;
using static mus.Notation;

namespace mus
{

    //now considered subset of above, with numbers 0 to 6
    public struct IntervalS : IEquatable<IntervalS>
    {
        public int ResidueNumber; //from 0 to 6.
        public int ResidueSemis;

        #region AutoGenerated

        public bool Equals(IntervalS other) => ResidueSemis == other.ResidueSemis && ResidueNumber == other.ResidueNumber;

        public override int GetHashCode()
        {
            int hashCode = 2051449748;
            hashCode = hashCode * -1521134295 + ResidueNumber.GetHashCode();
            hashCode = hashCode * -1521134295 + ResidueSemis.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(IntervalS left, IntervalS right) => left.Equals(right);

        public static bool operator !=(IntervalS left, IntervalS right) => !(left == right);

        public override bool Equals(object obj) => obj is IntervalS interval && Equals(interval);

        #endregion

        public IntervalS(IntervalC interval)
        {
            ResidueNumber = interval.ResidueNumber;
            ResidueSemis = interval.ResidueSemis;
        }

        public IntervalS(int number, int semis) : this(new IntervalC(number, semis)) { } //will automatically 'normalise'

        public override string ToString()
        {
            return QualityName(Quality, Degree.IsConsonant(ResidueNumber)) + " " + Degree.Interval(ResidueNumber);
        }

        //will accept any 'number' and calculate residue.
        public static IntervalS GetNew(int number, int quality) => new IntervalS(new IntervalC(number, quality, 0));

        public static explicit operator IntervalS(IntervalC obj) => new IntervalS(obj);

        public static implicit operator IntervalC(IntervalS obj) => new IntervalC(obj.ResidueNumber, obj.ResidueSemis);

        public int Quality => ResidueSemis - Degree.Semis(ResidueNumber);

        public static IntervalS operator +(IntervalS a, IntervalS b) => new IntervalS(a.ResidueNumber + b.ResidueNumber, a.ResidueSemis + b.ResidueSemis);

        public static IntervalS operator -(IntervalS a) => new IntervalS(-a.ResidueNumber, -a.ResidueSemis);

        public static IntervalS operator -(IntervalS a, IntervalS b) => new IntervalS(a.ResidueNumber - b.ResidueNumber, a.ResidueSemis - b.ResidueSemis);
    }

}

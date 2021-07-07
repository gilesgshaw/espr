//In process of importing this file.

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
    public static class notation
    {

        public static string test()
        {
            
            string jsonString = JsonSerializer.Serialize("ff");
            return jsonString;
        }

        #region Done

        // unison to 7th
        public enum SimpleDistance : int
        {
            Unison_0 = 0,
            _Second_1 = 1,
            _Third_3 = 2,
            Fourth_5 = 3,
            Fifth_7 = 4,
            _Sixth_8 = 5,
            _Seventh_10 = 6
        }

        public enum Inversion : int
        {
            Root_Position_,
            First_Inversion_b,
            Second_Inversion_c,
            Third_Inversion_d
        }

        public enum QualityP : int
        {
            _5Diminished = -5,
            _4Diminished = -4,
            _3Diminished = -3,
            _2Diminished = -2,
            _Diminished = -1,
            _Perfect = 0,
            _Augmented = 1,
            _2Augmented = 2,
            _3Augmented = 3,
            _4Augmented = 4,
            _5Augmented = 5
        }

        public enum QualityMi : int
        {
            _4Diminished = -4,
            _3Diminished = -3,
            _2Diminished = -2,
            _Diminished = -1,
            _Minor = 0,
            _Major = 1,
            _Augmented = 2,
            _2Augmented = 3,
            _3Augmented = 4,
            _4Augmented = 5
        }

        private static string GetDegreeName(SimpleDistance Degree)
        {
            switch (Degree)
            {
                case SimpleDistance.Fifth_7:
                    {
                        return "Dominant";
                    }

                case SimpleDistance.Fourth_5:
                    {
                        return "Subdominant";
                    }

                case SimpleDistance.Unison_0:
                    {
                        return "Tonic";
                    }

                case SimpleDistance._Second_1:
                    {
                        return "Supertonic";
                    }

                case SimpleDistance._Seventh_10:
                    {
                        return "LeadingNone";
                    }

                case SimpleDistance._Sixth_8:
                    {
                        return "Submediant";
                    }

                case SimpleDistance._Third_3:
                    {
                        return "Mediant";
                    }

                default:
                    {
                        throw new ArgumentException();
                        break;
                    }
            }
        }

        private static string GetDegreeSymbol(SimpleDistance Degree)
        {
            switch (Degree)
            {
                case SimpleDistance.Fifth_7:
                    {
                        return "V";
                    }

                case SimpleDistance.Fourth_5:
                    {
                        return "IV";
                    }

                case SimpleDistance.Unison_0:
                    {
                        return "I";
                    }

                case SimpleDistance._Second_1:
                    {
                        return "II";
                    }

                case SimpleDistance._Seventh_10:
                    {
                        return "VII";
                    }

                case SimpleDistance._Sixth_8:
                    {
                        return "VI";
                    }

                case SimpleDistance._Third_3:
                    {
                        return "III";
                    }

                default:
                    {
                        throw new ArgumentException();
                        break;
                    }
            }
        }

        private static string GetDistanceName(SimpleDistance Distance)
        {
            string str = Distance.ToString();
            if (str.StartsWith("_"))
                str = str.Substring(1);
            return str.Substring(0, str.LastIndexOf('_'));
        }

        public static string GetToneName(SimpleDistance Distance)
        {
            return GetDistanceName(Distance).Replace("Unison", "Root");
        }

        private static string GetOrdinalSuffix(int n)
        {
            if (n < 0)
                throw new ArgumentException();
            n = n % 100;
            if (n == 11 || n == 12 || n == 13)
                return "th";
            if (n % 10 == 1)
                return "st";
            if (n % 10 == 2)
                return "nd";
            if (n % 10 == 3)
                return "rd";
            return "th";
        }

        private static string GetAccidental(int alt)
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
                        break;
                    }
            }
        }

        //// unison to 7th
        //// added by possible octave removal
        //// negated by unison or octave
        //// subtracted in order a+-b
        //// compared by number then quality
        //// no error scrutiny, complete, test 1 method - 
        //[StructLayout(LayoutKind.Explicit, Size = 8)]
        //public struct SimpleInterval : IEquatable<SimpleInterval>, IComparable<SimpleInterval>
        //{
        //    [FieldOffset(0)]
        //    public SimpleDistance Number;
        //    [Ignored]
        //    [FieldOffset(4)]
        //    public QualityP QualityP;
        //    [Ignored]
        //    [FieldOffset(4)]
        //    public QualityMi QualityMi;
        //    [FieldOffset(4)]
        //    public int Quality;

        //    public SimpleInterval(SimpleDistance Number, int pSemiTones)
        //    {
        //        this.Number = Number;
        //        string sNumber = this.Number.ToString();
        //        Quality = pSemiTones - Conversions.ToInteger(sNumber.Substring(sNumber.LastIndexOf('_') + 1));
        //    }

        //    public SimpleInterval(int Quality, SimpleDistance Number)
        //    {
        //        this.Number = Number;
        //        this.Quality = Quality;
        //    }

        //    public SimpleInterval(SimpleDistance pNumber, QualityP pQualityP)
        //    {
        //        Number = pNumber;
        //        QualityP = pQualityP;
        //    }

        //    public SimpleInterval(SimpleDistance pNumber, QualityMi pQualityM)
        //    {
        //        Number = pNumber;
        //        QualityMi = pQualityM;
        //    }

        //    public int SemiTones
        //    {
        //        get
        //        {
        //            string sNumber = Number.ToString();
        //            return Quality + Conversions.ToInteger(sNumber.Substring(sNumber.LastIndexOf('_') + 1));
        //        }
        //    }

        //    public override string ToString()
        //    {
        //        return QualityName + " " + GetDistanceName(Number);
        //    }

        //    public string QualityName
        //    {
        //        get
        //        {
        //            if (TypedPerfect)
        //            {
        //                return QualityP.ToString().Substring(1);
        //            }
        //            else
        //            {
        //                return QualityMi.ToString().Substring(1);
        //            }
        //        }
        //    }

        //    public bool TypedPerfect
        //    {
        //        get
        //        {
        //            return !(Number.ToString()[0] == '_');
        //        }
        //    }

        //    public override int GetHashCode()
        //    {
        //        return ((int)Number >> 16 | (int)Number << 16) ^ Quality;
        //    }

        //    public static bool operator ==(SimpleInterval a, SimpleInterval b)
        //    {
        //        return a.Number == b.Number && a.Quality == b.Quality;
        //    }

        //    public static bool operator !=(SimpleInterval a, SimpleInterval b)
        //    {
        //        return !(a == b);
        //    }

        //    public static SimpleInterval operator -(SimpleInterval a)
        //    {
        //        if (a.Number == SimpleDistance.Unison_0)
        //            return new SimpleInterval(-a.Quality, SimpleDistance.Unison_0);
        //        return new SimpleInterval((SimpleDistance)Conversions.ToInteger(7 - (int)a.Number), 12 - a.SemiTones);
        //    }

        //    public static SimpleInterval operator +(SimpleInterval a, SimpleInterval b)
        //    {
        //        if ((int)a.Number + (int)b.Number > 6)
        //        {
        //            return new SimpleInterval((SimpleDistance)Conversions.ToInteger((int)a.Number + (int)b.Number - 7), a.SemiTones + b.SemiTones - 12);
        //        }
        //        else
        //        {
        //            return new SimpleInterval((SimpleDistance)Conversions.ToInteger((int)a.Number + (int)b.Number), a.SemiTones + b.SemiTones);
        //        }
        //    }

        //    public static SimpleInterval operator -(SimpleInterval a, SimpleInterval b)
        //    {
        //        return a + -b;
        //    }

        //    public bool Equals(SimpleInterval other)
        //    {
        //        return (this) == other;
        //    }

        //    public override bool Equals(object obj)
        //    {
        //        return obj is SimpleInterval && (SimpleInterval)obj == (this);
        //    }

        //    public int CompareTo(SimpleInterval other)
        //    {
        //        int CompareToRet = default;
        //        CompareToRet = ((int)Number).CompareTo((int)other.Number);
        //        if (CompareToRet == 0)
        //            CompareToRet = Quality.CompareTo(other.Quality);
        //        return CompareToRet;
        //    }

        //    public static bool operator <(SimpleInterval left, SimpleInterval right)
        //    {
        //        return left.CompareTo(right) < 0;
        //    }

        //    public static bool operator >(SimpleInterval left, SimpleInterval right)
        //    {
        //        return left.CompareTo(right) > 0;
        //    }

        //    public static bool operator <=(SimpleInterval left, SimpleInterval right)
        //    {
        //        return left.CompareTo(right) <= 0;
        //    }

        //    public static bool operator >=(SimpleInterval left, SimpleInterval right)
        //    {
        //        return left.CompareTo(right) >= 0;
        //    }
        //}

        //// unison upwards
        //// added exactly
        //// compared by number then quality
        //public struct CompoundInterval : IEquatable<CompoundInterval>, IComparable<CompoundInterval>
        //{
        //    public int Octaves0;
        //    public SimpleInterval Interval;

        //    public CompoundInterval(SimpleInterval pInterval)
        //    {
        //        Interval = pInterval;
        //    }

        //    public CompoundInterval(SimpleInterval Interval, int Octaves0)
        //    {
        //        this.Interval = Interval;
        //        this.Octaves0 = Octaves0;
        //    }

        //    public CompoundInterval(int pDistance, int pSemiTones)
        //    {
        //        if (pDistance < 0)
        //            throw new ArgumentOutOfRangeException();
        //        while (pDistance >= 7)
        //        {
        //            pDistance -= 7;
        //            Octaves0 += 1;
        //            pSemiTones -= 12;
        //        }

        //        Interval = new SimpleInterval((SimpleDistance)Conversions.ToInteger(pDistance), pSemiTones);
        //    }

        //    public CompoundInterval(int pDistance, QualityMi pQuality)
        //    {
        //        if (pDistance < 0)
        //            throw new ArgumentOutOfRangeException();
        //        while (pDistance >= 7)
        //        {
        //            pDistance -= 7;
        //            Octaves0 += 1;
        //        }

        //        Interval = new SimpleInterval((SimpleDistance)Conversions.ToInteger(pDistance), pQuality);
        //    }

        //    public CompoundInterval(int pDistance, QualityP pQuality)
        //    {
        //        if (pDistance < 0)
        //            throw new ArgumentOutOfRangeException();
        //        while (pDistance >= 7)
        //        {
        //            pDistance -= 7;
        //            Octaves0 += 1;
        //        }

        //        Interval = new SimpleInterval((SimpleDistance)pDistance, pQuality);
        //    }

        //    public static CompoundInterval GetNew(int pDistance, int pQuality)
        //    {
        //        CompoundInterval GetNewRet = default;
        //        if (pDistance < 0)
        //            throw new ArgumentOutOfRangeException();
        //        while (pDistance >= 7)
        //        {
        //            pDistance -= 7;
        //            GetNewRet.Octaves0 += 1;
        //        }

        //        GetNewRet.Interval = new SimpleInterval(pQuality, (SimpleDistance)Conversions.ToInteger(pDistance));
        //        return GetNewRet;
        //    }

        //    public int SemiTones
        //    {
        //        get
        //        {
        //            return Octaves0 * 12 + Interval.SemiTones;
        //        }
        //    }

        //    public override int GetHashCode()
        //    {
        //        return (Distance >> 16 | Distance << 16) ^ Interval.Quality;
        //    }

        //    public static implicit operator CompoundInterval(SimpleInterval input)
        //    {
        //        return new CompoundInterval(input);
        //    }

        //    public static explicit operator SimpleInterval(CompoundInterval input)
        //    {
        //        if (input.Octaves0 != 0)
        //            throw new ArgumentException();
        //        return input.Interval;
        //    }

        //    public bool IsSimple
        //    {
        //        get
        //        {
        //            return Octaves0 == 0;
        //        }
        //    }

        //    public override string ToString()
        //    {
        //        if (IsSimple)
        //            return Interval.ToString();
        //        if (Distance == 7)
        //            return Interval.QualityName + " Octave";
        //        return Interval.QualityName + " " + (Distance + 1).ToString() + GetOrdinalSuffix(Distance + 1);
        //    }

        //    public int Distance
        //    {
        //        get
        //        {
        //            return Octaves0 * 7 + (int)Interval.Number;
        //        }
        //    }

        //    public static bool operator ==(CompoundInterval a, CompoundInterval b)
        //    {
        //        return a.Octaves0 == b.Octaves0 && a.Interval == b.Interval;
        //    }

        //    public static bool operator !=(CompoundInterval a, CompoundInterval b)
        //    {
        //        return !(a == b);
        //    }

        //    public static CompoundInterval operator +(CompoundInterval a, CompoundInterval b)
        //    {
        //        return new CompoundInterval(a.Distance + b.Distance, a.SemiTones + b.SemiTones);
        //    }

        //    /// <summary>true for up, interval returned is commutative</summary>
        //    public static (int, CompoundInterval) operator -(CompoundInterval a, CompoundInterval b)
        //    {
        //        if (a.Distance > b.Distance)
        //        {
        //            return (-1, new CompoundInterval(a.Distance - b.Distance, a.SemiTones - b.SemiTones));
        //        }
        //        else if (a.Distance == b.Distance)
        //        {
        //            if (a.SemiTones > b.SemiTones)
        //            {
        //                return (-1, new CompoundInterval(0, Math.Abs(a.SemiTones - b.SemiTones)));
        //            }
        //            else if (a.SemiTones == b.SemiTones)
        //            {
        //                return (0, new CompoundInterval(0, Math.Abs(a.SemiTones - b.SemiTones)));
        //            }
        //            else
        //            {
        //                return (1, new CompoundInterval(0, Math.Abs(a.SemiTones - b.SemiTones)));
        //            }
        //        }
        //        else
        //        {
        //            return (1, new CompoundInterval(b.Distance - a.Distance, b.SemiTones - a.SemiTones));
        //        }
        //    }

        //    public bool Equals(CompoundInterval other)
        //    {
        //        return (this) == other;
        //    }

        //    public override bool Equals(object obj)
        //    {
        //        return obj is CompoundInterval && (CompoundInterval)obj == (this);
        //    }

        //    public int CompareTo(CompoundInterval other)
        //    {
        //        int CompareToRet = default;
        //        CompareToRet = Octaves0.CompareTo(other.Octaves0);
        //        if (CompareToRet == 0)
        //            CompareToRet = Interval.CompareTo(other.Interval);
        //        return CompareToRet;
        //    }

        //    public static bool operator <(CompoundInterval left, CompoundInterval right)
        //    {
        //        return left.CompareTo(right) < 0;
        //    }

        //    public static bool operator >(CompoundInterval left, CompoundInterval right)
        //    {
        //        return left.CompareTo(right) > 0;
        //    }

        //    public static bool operator <=(CompoundInterval left, CompoundInterval right)
        //    {
        //        return left.CompareTo(right) <= 0;
        //    }

        //    public static bool operator >=(CompoundInterval left, CompoundInterval right)
        //    {
        //        return left.CompareTo(right) >= 0;
        //    }
        //}

        //// compared by degree then quality
        //// no error scrutiny, complete
        //public struct Pitch : IEquatable<Pitch>, IComparable<Pitch>
        //{
        //    public CompoundInterval IntervalFromC0;

        //    public int OffsetFromC4
        //    {
        //        get
        //        {
        //            return IntervalFromC0.Distance - 28;
        //        }
        //    }

        //    public Pitch(CompoundInterval IntervalFromC0)
        //    {
        //        this.IntervalFromC0 = IntervalFromC0;
        //    }

        //    public Pitch(SimpleInterval pIntervalFromC, int pOctavesFromC0)
        //    {
        //        IntervalFromC0.Interval = pIntervalFromC;
        //        IntervalFromC0.Octaves0 = pOctavesFromC0;
        //    }

        //    public int MIDIPitch
        //    {
        //        get
        //        {
        //            return 12 + IntervalFromC0.SemiTones;
        //        }
        //    }

        //    public override int GetHashCode()
        //    {
        //        return IntervalFromC0.GetHashCode();
        //    }

        //    public int CompareTo(Pitch other)
        //    {
        //        return IntervalFromC0.CompareTo(other.IntervalFromC0);
        //    }

        //    public static bool operator <(Pitch left, Pitch right)
        //    {
        //        return left.CompareTo(right) < 0;
        //    }

        //    public static bool operator >(Pitch left, Pitch right)
        //    {
        //        return left.CompareTo(right) > 0;
        //    }

        //    public static bool operator <=(Pitch left, Pitch right)
        //    {
        //        return left.CompareTo(right) <= 0;
        //    }

        //    public static bool operator >=(Pitch left, Pitch right)
        //    {
        //        return left.CompareTo(right) >= 0;
        //    }

        //    public static bool operator ==(Pitch a, Pitch b)
        //    {
        //        return a.IntervalFromC0 == b.IntervalFromC0;
        //    }

        //    public static bool operator !=(Pitch a, Pitch b)
        //    {
        //        return !(a == b);
        //    }

        //    public bool Equals(Pitch other)
        //    {
        //        return (this) == other;
        //    }

        //    public override bool Equals(object obj)
        //    {
        //        return obj is Pitch && (Pitch)obj == (this);
        //    }

        //    /// <summary>true for up, interval returned is commutative</summary>
        //    public static (int, CompoundInterval) operator -(Pitch a, Pitch b)
        //    {
        //        return a.IntervalFromC0 - b.IntervalFromC0;
        //    }

        //    public override string ToString()
        //    {
        //        string acc = GetAccidental(IntervalFromC0.Interval.SemiTones - Mode.Instance("Ionian").get_Interval(IntervalFromC0.Interval.Number).SemiTones);
        //        switch (IntervalFromC0.Interval.Number)
        //        {
        //            case SimpleDistance.Fifth_7:
        //                {
        //                    return "G" + acc + IntervalFromC0.Octaves0;
        //                }

        //            case SimpleDistance.Fourth_5:
        //                {
        //                    return "F" + acc + IntervalFromC0.Octaves0;
        //                }

        //            case SimpleDistance.Unison_0:
        //                {
        //                    return "C" + acc + IntervalFromC0.Octaves0;
        //                }

        //            case SimpleDistance._Second_1:
        //                {
        //                    return "D" + acc + IntervalFromC0.Octaves0;
        //                }

        //            case SimpleDistance._Seventh_10:
        //                {
        //                    return "B" + acc + IntervalFromC0.Octaves0;
        //                }

        //            case SimpleDistance._Sixth_8:
        //                {
        //                    return "A" + acc + IntervalFromC0.Octaves0;
        //                }

        //            case SimpleDistance._Third_3:
        //                {
        //                    return "E" + acc + IntervalFromC0.Octaves0;
        //                }
        //        }

        //        return default;
        //    }
        //}

        //[ImmutableObject(true)]
        //public sealed class CharacterType : UserInstances
        //{
        //    private QualityMi iThird;
        //    private QualityP iFifth;
        //    private QualityMi? iSeventh;
        //    private char iCharacter;
        //    private bool iUpper;

        //    public SimpleInterval Third
        //    {
        //        get
        //        {
        //            return new SimpleInterval(SimpleDistance._Third_3, iThird);
        //        }
        //    }

        //    public SimpleInterval Fifth
        //    {
        //        get
        //        {
        //            return new SimpleInterval(SimpleDistance.Fifth_7, iFifth);
        //        }
        //    }

        //    public SimpleInterval? Seventh
        //    {
        //        get
        //        {
        //            if (iSeventh.HasValue)
        //            {
        //                return new SimpleInterval(SimpleDistance._Seventh_10, iSeventh.Value);
        //            }
        //            else
        //            {
        //                return default;
        //            }
        //        }
        //    }

        //    public SimpleInterval get_Tone(SimpleDistance Degree)
        //    {
        //        switch (Degree)
        //        {
        //            case SimpleDistance._Third_3:
        //                {
        //                    return Third;
        //                }

        //            case SimpleDistance.Fifth_7:
        //                {
        //                    return Fifth;
        //                }

        //            case SimpleDistance._Seventh_10:
        //                {
        //                    return Seventh.Value;
        //                }
        //        }

        //        return default;
        //    }

        //    /// <summary>Safe</summary>
        //    public char Character
        //    {
        //        get
        //        {
        //            return iCharacter;
        //        }
        //    }

        //    /// <summary>Safe</summary>
        //    public bool Upper
        //    {
        //        get
        //        {
        //            return iUpper;
        //        }
        //    }

        //    /// <exception cref="ArgumentNullException">Name</exception>
        //    /// <exception cref="ArgumentException">Name Empty, contains colon or duplicate</exception>
        //    public CharacterType(string Name, QualityMi pThird, QualityP pFifth, QualityMi? pSeventh, char pCharacter, bool pUpper) : base()
        //    {
        //        if (Name is null)
        //            throw new ArgumentNullException("Name");
        //        if ((Name ?? "") == (string.Empty ?? ""))
        //            throw new ArgumentException("Empty", "Name");
        //        if (Names().Contains(Name))
        //            throw new ArgumentException("Duplicate", "Name");
        //        if (Name.Contains(Conversions.ToString(':')))
        //            throw new ArgumentException("Contains colon", "Name");
        //        iThird = pThird;
        //        iFifth = pFifth;
        //        iSeventh = pSeventh;
        //        iCharacter = pCharacter;
        //        iUpper = pUpper;
        //        Add(typeof(CharacterType), Name);
        //    }

        //    protected override bool Deserialize(string Contents)
        //    {
        //        var Parts = Contents.Split(';');
        //        if (Parts.Length != 6)
        //            return false;
        //        var NewParts = new int[3];
        //        for (int i = 0; i <= 2; i++)
        //        {
        //            if (!int.TryParse(Parts[i], out NewParts[i]))
        //                return false;
        //        }

        //        iThird = (QualityMi)Conversions.ToInteger(NewParts[0]);
        //        iFifth = (QualityP)Conversions.ToInteger(NewParts[1]);
        //        iSeventh = (QualityMi)Conversions.ToInteger(NewParts[2]);
        //        if ((Parts[3] ?? "") == (string.Empty ?? ""))
        //            iSeventh = default;
        //        iUpper = (Parts[4] ?? "") != (string.Empty ?? "");
        //        int CharacterInt;
        //        if (!int.TryParse(Parts[5], out CharacterInt) || CharacterInt < -32768 || CharacterInt > 65535)
        //            return false;
        //        iCharacter = (char)CharacterInt;
        //        return true;
        //    }

        //    protected override string Serialize()
        //    {
        //        if (iSeventh.HasValue)
        //        {
        //            return ((int)iThird).ToString() + ';' + (int)iFifth + ';' + Conversions.ToInteger(iSeventh) + (Upper ? ";T;T;" : ";T;;") + Strings.AscW(iCharacter);
        //        }
        //        else
        //        {
        //            return ((int)iThird).ToString() + ';' + (int)iFifth + (Upper ? ";0;;T;" : ";0;;;") + Strings.AscW(iCharacter);
        //        }
        //    }

        //    private CharacterType() : base()
        //    {
        //    }

        //    /// <summary>Safe, *:*</summary>
        //    public static IEnumerable<string> Names()
        //    {
        //        return (IEnumerable<string>)Names().ElementAtOrDefault(typeof(CharacterType));
        //    }

        //    /// <summary>*</summary>
        //    /// <exception cref="ArgumentNullException">Name</exception>
        //    /// <exception cref="ArgumentException">Not listed or invalid</exception>
        //    public static CharacterType Instance(string Name)
        //    {
        //        if (Name is null)
        //            throw new ArgumentNullException("Name");
        //        return Instance(typeof(CharacterType), Name, new CharacterType());
        //    }

        //    public override string ToString()
        //    {
        //        string ToStringRet = default;
        //        ToStringRet = Third.ToString();
        //        ToStringRet += ", " + Fifth.ToString();
        //        if (iSeventh.HasValue)
        //            ToStringRet += ", " + Seventh.ToString();
        //        if (iCharacter != default(char))
        //            ToStringRet += ", " + iCharacter;
        //        if (iUpper)
        //            return ToStringRet.ToUpper();
        //        return ToStringRet;
        //    }
        //}

        //[ImmutableObject(true)]
        //public sealed class Mode : UserInstances
        //{
        //    private QualityMi _2;
        //    private QualityMi _3;
        //    private QualityP _4;
        //    private QualityP _5;
        //    private QualityMi _6;
        //    private QualityMi _7;

        //    private int get_Quality(SimpleDistance Number)
        //    {
        //        switch (Number)
        //        {
        //            case SimpleDistance.Fifth_7:
        //                {
        //                    return (int)_5;
        //                }

        //            case SimpleDistance.Fourth_5:
        //                {
        //                    return (int)_4;
        //                }

        //            case SimpleDistance.Unison_0:
        //                {
        //                    return (int)QualityP._Perfect;
        //                }

        //            case SimpleDistance._Second_1:
        //                {
        //                    return (int)_2;
        //                }

        //            case SimpleDistance._Seventh_10:
        //                {
        //                    return (int)_7;
        //                }

        //            case SimpleDistance._Sixth_8:
        //                {
        //                    return (int)_6;
        //                }

        //            case SimpleDistance._Third_3:
        //                {
        //                    return (int)_3;
        //                }

        //            default:
        //                {
        //                    throw new ArgumentException();
        //                    break;
        //                }
        //        }
        //    }

        //    public SimpleInterval get_Interval(SimpleDistance Number)
        //    {
        //        return new SimpleInterval(get_Quality(Number), Number);
        //    }

        //    /// <exception cref="ArgumentNullException">Name</exception>
        //    /// <exception cref="ArgumentException">Name Empty, contains colon or duplicate</exception>
        //    public Mode(string Name, QualityMi p2, QualityMi p3, QualityP p4, QualityP p5, QualityMi p6, QualityMi p7) : base()
        //    {
        //        if (Name is null)
        //            throw new ArgumentNullException("Name");
        //        if ((Name ?? "") == (string.Empty ?? ""))
        //            throw new ArgumentException("Empty", "Name");
        //        if (Names().Contains(Name))
        //            throw new ArgumentException("Duplicate", "Name");
        //        if (Name.Contains(Conversions.ToString(':')))
        //            throw new ArgumentException("Contains colon", "Name");
        //        _2 = p2;
        //        _3 = p3;
        //        _4 = p4;
        //        _5 = p5;
        //        _6 = p6;
        //        _7 = p7;
        //        Add(typeof(Mode), Name);
        //    }

        //    protected override bool Deserialize(string Contents)
        //    {
        //        var Parts = Contents.Split(';');
        //        var NewParts = new int[6];
        //        if (Parts.Length != 6)
        //            return false;
        //        for (int i = 0; i <= 5; i++)
        //        {
        //            if (!int.TryParse(Parts[i], out NewParts[i]))
        //                return false;
        //        }

        //        _2 = (QualityMi)NewParts[0];
        //        _3 = (QualityMi)NewParts[1];
        //        _4 = (QualityP) NewParts[2];
        //        _5 = (QualityP) NewParts[3];
        //        _6 = (QualityMi)NewParts[4];
        //        _7 = (QualityMi)NewParts[5];
        //        return true;
        //    }

        //    protected override string Serialize()
        //    {
        //        return ((int)_2).ToString() + ';' + (int)_3 + ';' + (int)_4 + ';' + (int)_5 + ';' + (int)_6 + ';' + (int)_7;
        //    }

        //    public override string ToString()
        //    {
        //        string ToStringRet = default;
        //        ToStringRet = string.Empty;
        //        for (int i = 0; i <= 6; i++)
        //            ToStringRet += ", " + get_Interval((SimpleDistance)Conversions.ToInteger(i)).ToString();
        //        ToStringRet = ToStringRet.Substring(2);
        //        return ToStringRet;
        //    }

        //    private Mode() : base()
        //    {
        //    }

        //    /// <summary>Safe, *:*</summary>
        //    public static IEnumerable<string> Names()
        //    {
        //        return (IEnumerable<string>)Names().ElementAtOrDefault(typeof(Mode));
        //    }

        //    /// <summary>*</summary>
        //    /// <exception cref="ArgumentNullException">Name</exception>
        //    /// <exception cref="ArgumentException">Not listed or invalid</exception>
        //    public static Mode Instance(string Name)
        //    {
        //        if (Name is null)
        //            throw new ArgumentNullException("Name");
        //        return Instance(typeof(Mode), Name, new Mode());
        //    }
        //}

        //// not quite finished. serializxation...
        //public struct Key
        //{
        //    public Key(SimpleInterval pTonic, Mode pScale)
        //    {
        //        Tonic = pTonic;
        //        Scale = pScale;
        //    }

        //    public SimpleInterval Tonic;
        //    public Mode Scale;
        //}

        //// no error scrutiny, complete
        //// no validation d with 7
        //[Serial]
        //public sealed class Chord : IEquatable<Chord>
        //{
        //    public static bool operator ==(Chord left, Chord right)
        //    {
        //        if (ReferenceEquals(left, right))
        //            return true;
        //        if (left is null || right is null)
        //            return false;
        //        return left.iRoot == right.iRoot && left.iCharacter == right.iCharacter;
        //    }

        //    public static bool operator !=(Chord left, Chord right)
        //    {
        //        return !(left == right);
        //    }

        //    public override bool Equals(object obj)
        //    {
        //        return (this) == obj as Chord;
        //    }

        //    public override int GetHashCode()
        //    {
        //        return iCharacter.GetHashCode << 20 ^ iRoot.GetHashCode();
        //    }

        //    public bool Equals(Chord other)
        //    {
        //        return (this) == other;
        //    }

        //    // Private Sub GetObjectData(info As SerializationInfo, context As StreamingContext) Implements ISerializable.GetObjectData
        //    // info.AddValue("iRoot.Number", iRoot.Number)
        //    // info.AddValue("iRoot.Quality", iRoot.Quality)
        //    // info.AddValue("iCharacter", Character.Names.Where(Function(x) Character.Instance(x) = iCharacter)(0))
        //    // End Sub

        //    // Public Sub New(info As SerializationInfo, context As StreamingContext)
        //    // iRoot.Number = CType(info.GetInt32("iRoot.Number"), SimpleDistance)
        //    // iRoot.Quality = info.GetInt32("iRoot.Quality")
        //    // iCharacter = Character.Instance(info.GetString("iCharacter"))
        //    // End Sub

        //    private SimpleInterval iRoot;
        //    private CharacterType iCharacter;

        //    public Chord(SimpleInterval Root, CharacterType Character)
        //    {
        //        iRoot = Root;
        //        iCharacter = Character;
        //    }

        //    /// <summary>Safe</summary>
        //    public CharacterType Character
        //    {
        //        get
        //        {
        //            return iCharacter;
        //        }
        //    }

        //    /// <summary>Safe</summary>
        //    public SimpleInterval Root
        //    {
        //        get
        //        {
        //            return iRoot;
        //        }
        //    }

        //    public SimpleInterval GetThird
        //    {
        //        get
        //        {
        //            return iRoot + iCharacter.Third;
        //        }
        //    }

        //    public SimpleInterval GetFifth
        //    {
        //        get
        //        {
        //            return iRoot + iCharacter.Fifth;
        //        }
        //    }

        //    public SimpleInterval? GetSeventh
        //    {
        //        get
        //        {
        //            var obj = iCharacter.Seventh;
        //            if (obj.HasValue)
        //            {
        //                return iRoot + obj.Value;
        //            }
        //            else
        //            {
        //                return default;
        //            }
        //        }
        //    }

        //    // Public ReadOnly Property GetBass() As SimpleInterval
        //    // Get
        //    // Select Case iInversion
        //    // Case Inversion.Root_Position_
        //    // Return iRoot
        //    // Case Inversion.First_Inversion_b
        //    // Return GetThird()
        //    // Case Inversion.Second_Inversion_c
        //    // Return GetFifth()
        //    // Case Inversion.Third_Inversion_d
        //    // Return GetSeventh().Value
        //    // End Select
        //    // End Get
        //    // End Property

        //    // Public ReadOnly Property GetBassFromRoot() As SimpleInterval
        //    // Get
        //    // Select Case iInversion
        //    // Case Inversion.Root_Position_
        //    // Return Nothing
        //    // Case Inversion.First_Inversion_b
        //    // Return iCharacter.Third
        //    // Case Inversion.Second_Inversion_c
        //    // Return iCharacter.Fifth
        //    // Case Inversion.Third_Inversion_d
        //    // Return iCharacter.Seventh.Value
        //    // End Select
        //    // End Get
        //    // End Property

        //    public SimpleInterval[] GetTones
        //    {
        //        get
        //        {
        //            var obj = GetSeventh;
        //            if (obj.HasValue)
        //            {
        //                return new[] { iRoot, GetThird, GetFifth, obj.Value };
        //            }
        //            else
        //            {
        //                return new[] { iRoot, GetThird, GetFifth };
        //            }
        //        }
        //    }

        //    public SimpleInterval get_GetTone(SimpleDistance Degree)
        //    {
        //        switch (Degree)
        //        {
        //            case SimpleDistance.Unison_0:
        //                {
        //                    return iRoot;
        //                }

        //            case SimpleDistance._Third_3:
        //                {
        //                    return GetThird;
        //                }

        //            case SimpleDistance.Fifth_7:
        //                {
        //                    return GetFifth;
        //                }

        //            case SimpleDistance._Seventh_10:
        //                {
        //                    return GetSeventh.Value;
        //                }
        //        }

        //        return default;
        //    }
        //}

        #endregion

    }
}

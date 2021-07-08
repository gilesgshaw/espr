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
            int g = 0;
            string jsonString = JsonSerializer.Serialize(g);
            return jsonString;
        }

        #region Okay
        
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
                            return 1;
                        }
                    case 2:
                        {
                            return 3;
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
                            return 8;
                        }
                    case 6:
                        {
                            return 10;
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

        public static string QualityName()
        {
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

        public static char AccidentalSymbol()
        {
            throw new NotImplementedException();
        //private static string GetAccidental(int alt)
        //{
        //    switch (alt)
        //    {
        //        case -2:
        //            {
        //                return "𝄫";
        //            }

        //        case -1:
        //            {
        //                return "♭";
        //            }

        //        case 0:
        //            {
        //                return string.Empty;
        //            }

        //        case 1:
        //            {
        //                return "♯";
        //            }

        //        case 2:
        //            {
        //                return "𝄪";
        //            }

        //        default:
        //            {
        //                throw new NotImplementedException();
        //            }
        //    }
        //}

        }
        
        // compared by...
        public struct Interval : IEquatable<Interval>, IComparable<Interval>
        {
            
        //private static string GetOrdinalSuffix(int n)
        //{
        //    if (n < 0)
        //        throw new ArgumentException();
        //    n = n % 100;
        //    if (n == 11 || n == 12 || n == 13)
        //        return "th";
        //    if (n % 10 == 1)
        //        return "st";
        //    if (n % 10 == 2)
        //        return "nd";
        //    if (n % 10 == 3)
        //        return "rd";
        //    return "th";
        //}

            public int Semis;
            public int Number;

            public Interval(int number, int semis)
            {
                Number = number;
                Semis = semis;
            }
            
            public Interval(int number, int quality, int octaves)
            {
                Number = number + octaves * 7;
                Semis = 0;
                Semis = quality - Quality;
            }

            public int NumberRem
            {
                get
                {
                    return mod(7, Number);
                }
            }
            
            public int SemisRem
            {
                get
                {
                    return Semis - 12*Octaves;
                }
            }

            public int Octaves
            {
                get
                {
                    return (Number - NumberRem)/7;
                }
            }

            public int Quality
            {
                get
                {
                    return SemisRem - Degree.Semis(NumberRem);
                }
            }

            public override bool Equals(object obj)
            {
                return obj is Interval interval && Equals(interval);
            }

            public bool Equals(Interval other)
            {
                return Semis == other.Semis &&
                       Number == other.Number;
            }

            public override int GetHashCode()
            {
                int hashCode = 1960790096;
                hashCode = hashCode * -1521134295 + Semis.GetHashCode();
                hashCode = hashCode * -1521134295 + Number.GetHashCode();
                return hashCode;
            }

            public int CompareTo(Interval other)
            {
                int result = Number.CompareTo(other.Number);
                if (result != 0) return result;
                return Semis.CompareTo(other.Semis);
            }

            public static bool operator ==(Interval left, Interval right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(Interval left, Interval right)
            {
                return !(left == right);
            }

            public static Interval operator +(Interval a, Interval b)
            {
                return new Interval(a.Number + b.Number, a.Semis + b.Semis);
            }
            
            public static Interval operator -(Interval a)
            {
                return new Interval(-a.Number, -a.Semis);
            }
            
            public static Interval operator -(Interval a, Interval b)
            {
                return new Interval(a.Number - b.Number, a.Semis - b.Semis);
            }

            public static bool operator <(Interval left, Interval right)
            {
                return left.CompareTo(right) < 0;
            }

            public static bool operator <=(Interval left, Interval right)
            {
                return left.CompareTo(right) <= 0;
            }

            public static bool operator >(Interval left, Interval right)
            {
                return left.CompareTo(right) > 0;
            }

            public static bool operator >=(Interval left, Interval right)
            {
                return left.CompareTo(right) >= 0;
            }

            public Interval Residue
            {
                get
                {
                    return this - new Interval(0, 0, Octaves);
                }
            }
        }
        
        // compared by...
        public struct Pitch : IEquatable<Pitch>, IComparable<Pitch>
        {
            public Interval IntervalFromC0;

            public Pitch(Interval IntervalFromC0)
            {
                this.IntervalFromC0 = IntervalFromC0;
            }

            public int MIDIPitch
            {
                get
                {
                    return 12 + IntervalFromC0.Semis;
                }
            }

            public override int GetHashCode()
            {
                return IntervalFromC0.GetHashCode();
            }

            public int CompareTo(Pitch other)
            {
                return IntervalFromC0.CompareTo(other.IntervalFromC0);
            }

            public static bool operator <(Pitch left, Pitch right)
            {
                return left.CompareTo(right) < 0;
            }

            public static bool operator >(Pitch left, Pitch right)
            {
                return left.CompareTo(right) > 0;
            }

            public static bool operator <=(Pitch left, Pitch right)
            {
                return left.CompareTo(right) <= 0;
            }

            public static bool operator >=(Pitch left, Pitch right)
            {
                return left.CompareTo(right) >= 0;
            }

            public static bool operator ==(Pitch a, Pitch b)
            {
                return a.IntervalFromC0 == b.IntervalFromC0;
            }

            public static bool operator !=(Pitch a, Pitch b)
            {
                return !(a == b);
            }

            public bool Equals(Pitch other)
            {
                return (this) == other;
            }

            public override bool Equals(object obj)
            {
                return obj is Pitch && (Pitch)obj == (this);
            }

            public static Interval operator -(Pitch a, Pitch b)
            {
                return a.IntervalFromC0 - b.IntervalFromC0;
            }

            public override string ToString()
            {
                //string acc = AccidentalSymbol(IntervalFromC0.Interval.Semiints - Mode.Instance("Ionian").get_Interval(IntervalFromC0.Interval.Number).Semiints);
                //switch (IntervalFromC0.Residue.Number)
                //{
                //    case 0:
                //        {
                //            return "C" + acc + IntervalFromC0.Octaves;
                //        }
                        
                //    case 1:
                //        {
                //            return "D" + acc + IntervalFromC0.Octaves;
                //        }
                        
                //    case 2:
                //        {
                //            return "E" + acc + IntervalFromC0.Octaves0;
                //        }

                //    case 3:
                //        {
                //            return "F" + acc + IntervalFromC0.Octaves;
                //        }
                        
                //    case 4:
                //        {
                //            return "G" + acc + IntervalFromC0.Octaves;
                //        }

                //    case 5:
                //        {
                //            return "A" + acc + IntervalFromC0.Octaves0;
                //        }
                        
                //    case 6:
                //        {
                //            return "B" + acc + IntervalFromC0.Octaves;
                //        }

                //}
                return base.ToString();
            }
        }

        #endregion

    }
}

        //public enum Inversion : int
        //{
        //    Root_Position_,
        //    First_Inversion_b,
        //    Second_Inversion_c,
        //    Third_Inversion_d
        //}

        //[ImmutableObject(true)]
        //public sealed class CharacterType : UserInstances
        //{
        //    private QualityMi iThird;
        //    private QualityPf iFifth;
        //    private QualityMi? iSeventh;
        //    private char iCharacter;
        //    private bool iUpper;

        //    public SimpleInterval Third
        //    {
        //        get
        //        {
        //            return new SimpleInterval(SimpleDist.2, iThird);
        //        }
        //    }

        //    public SimpleInterval Fifth
        //    {
        //        get
        //        {
        //            return new SimpleInterval(SimpleDist.4, iFifth);
        //        }
        //    }

        //    public SimpleInterval? Seventh
        //    {
        //        get
        //        {
        //            if (iSeventh.HasValue)
        //            {
        //                return new SimpleInterval(SimpleDist.6, iSeventh.Value);
        //            }
        //            else
        //            {
        //                return default;
        //            }
        //        }
        //    }

        //    public SimpleInterval get_int(SimpleDist Degree)
        //    {
        //        switch (Degree)
        //        {
        //            case SimpleDist.2:
        //                {
        //                    return Third;
        //                }

        //            case SimpleDist.4:
        //                {
        //                    return Fifth;
        //                }

        //            case SimpleDist.6:
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
        //    public CharacterType(string Name, QualityMi pThird, QualityPf pFifth, QualityMi? pSeventh, char pCharacter, bool pUpper) : base()
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
        //        iFifth = (QualityPf)Conversions.ToInteger(NewParts[1]);
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
        //    private QualityPf _4;
        //    private QualityPf _5;
        //    private QualityMi _6;
        //    private QualityMi _7;

        //    private int get_Quality(SimpleDist Number)
        //    {
        //        switch (Number)
        //        {
        //            case SimpleDist.4:
        //                {
        //                    return (int)_5;
        //                }

        //            case SimpleDist.3:
        //                {
        //                    return (int)_4;
        //                }

        //            case SimpleDist.0:
        //                {
        //                    return (int)QualityPf._Perfect;
        //                }

        //            case SimpleDist.1:
        //                {
        //                    return (int)_2;
        //                }

        //            case SimpleDist.6:
        //                {
        //                    return (int)_7;
        //                }

        //            case SimpleDist.5:
        //                {
        //                    return (int)_6;
        //                }

        //            case SimpleDist.2:
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

        //    public SimpleInterval get_Interval(SimpleDist Number)
        //    {
        //        return new SimpleInterval(get_Quality(Number), Number);
        //    }

        //    /// <exception cref="ArgumentNullException">Name</exception>
        //    /// <exception cref="ArgumentException">Name Empty, contains colon or duplicate</exception>
        //    public Mode(string Name, QualityMi p2, QualityMi p3, QualityPf p4, QualityPf p5, QualityMi p6, QualityMi p7) : base()
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
        //        _4 = (QualityPf) NewParts[2];
        //        _5 = (QualityPf) NewParts[3];
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
        //            ToStringRet += ", " + get_Interval((SimpleDist)Conversions.ToInteger(i)).ToString();
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
        //    // iRoot.Number = CType(info.GetInt32("iRoot.Number"), SimpleDist)
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

        //    public SimpleInterval[] Getints
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

        //    public SimpleInterval get_Getint(SimpleDist Degree)
        //    {
        //        switch (Degree)
        //        {
        //            case SimpleDist.0:
        //                {
        //                    return iRoot;
        //                }

        //            case SimpleDist.2:
        //                {
        //                    return GetThird;
        //                }

        //            case SimpleDist.4:
        //                {
        //                    return GetFifth;
        //                }

        //            case SimpleDist.6:
        //                {
        //                    return GetSeventh.Value;
        //                }
        //        }

        //        return default;
        //    }
        //}

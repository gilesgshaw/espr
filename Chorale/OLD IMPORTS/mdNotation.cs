using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Utility;
using winUtility;

namespace Chorale
{
    public static class mdNotation
    {

        public class Display
        {

            // what supported, no key signatures, no ledger lines, all as dots with no accidentals, or squares
            // update for degree
            // accidentals and key signature agreement

            [ImmutableObject(true)]
            public sealed class Clef : UserInstances
            {
                private int iMCRankFromTopLine;

                /// <summary>Safe</summary>
                public int MCRankFromTopLine
                {
                    get
                    {
                        return iMCRankFromTopLine;
                    }
                }

                public void Draw(Graphics g, int topLineY, int rankHeightY, int startX, int widthX)
                {
                    if (iMCRankFromTopLine == 10)
                    {
                        g.DrawString("g", new Font("calibri", 14f), Brushes.Black, new Point(startX, topLineY + rankHeightY * 6 - 15));
                    }
                    else
                    {
                        g.DrawString("f", new Font("calibri", 14f), Brushes.Black, new Point(startX, topLineY + rankHeightY * 2 - 10));
                    }
                }

                /// <exception cref="ArgumentNullException">Name</exception>
            /// <exception cref="ArgumentException">Name Empty, contains colon or duplicate</exception>
                public Clef(string Name, int MCRankFromTopLine) : base()
                {
                    iMCRankFromTopLine = MCRankFromTopLine;
                    if (Name is null)
                        throw new ArgumentNullException("Name");
                    if ((Name ?? "") == (string.Empty ?? ""))
                        throw new ArgumentException("Empty", "Name");
                    if (Names().Contains(Name))
                        throw new ArgumentException("Duplicate", "Name");
                    if (Name.Contains(Conversions.ToString(':')))
                        throw new ArgumentException("Contains colon", "Name");
                    Add(typeof(Clef), Name);
                }

                protected override bool Deserialize(string Contents)
                {
                    return int.TryParse(Contents, out iMCRankFromTopLine);
                }

                protected override string Serialize()
                {
                    return iMCRankFromTopLine.ToString();
                }

                public override string ToString()
                {
                    return Names().Where(x => ReferenceEquals(Instance(x), this)).ElementAtOrDefault(0);
                }

                private Clef() : base()
                {
                }

                /// <summary>Safe, *:*</summary>
                public static IEnumerable<string> Names()
                {
                    return (IEnumerable<string>)Names().ElementAtOrDefault(typeof(Clef));
                }

                /// <summary>*</summary>
            /// <exception cref="ArgumentNullException">Name</exception>
            /// <exception cref="ArgumentException">Not listed or invalid</exception>
                public static Clef Instance(string Name)
                {
                    if (Name is null)
                        throw new ArgumentNullException("Name");
                    return Instance(typeof(Clef), Name, new Clef());
                }
            }

            public class TimeSignature
            {
                public static TimeSignature Common { get; private set; } = new TimeSignature();

                public double BarLengthW
                {
                    get
                    {
                        return 1d;
                    }
                }

                private TimeSignature()
                {
                }

                public void Draw(Graphics g, int topLineY, int rankHeightY, int startX, int widthX)
                {
                    g.DrawString("4", new Font("calibri", 14f), Brushes.Black, new Point(widthX + startX - 10, topLineY));
                    g.DrawString("4", new Font("calibri", 14f), Brushes.Black, new Point(widthX + startX - 10, topLineY + rankHeightY * 4));
                }
            }

            public class Part
            {
                public Clef Clef;
                public Event[][][] Bars;
            }

            public struct Event
            {
                public Pitch? Pitch;
                public int WholeDivisionPower;
                public int Dot;
            }

            public TimeSignature TS;
            public Key Key;
            public Part[] Parts;

            public Bitmap Draw()
            {
                return Draw(Parts, TS, Key.Tonic, Key.Scale);
            }

            private const int MainMarginX = 30;
            private const int MainMarginY = 30;
            private const int StaveMargin = 13;
            private const int SignatureWidth = 80;
            private const int PostSignatureMargin = 13;
            private const int BarLineMargin = 8;
            private const int StaffSpacing = 57;
            private const int RankSpacing = 5;
            private const int BarWidth = 85;
            public const int stemlength = 17;

            private static Bitmap Draw(Part[] parts, TimeSignature ts, SimpleInterval key, Mode Scale)
            {
                Bitmap DrawRet = default;
                DrawRet = new Bitmap(2 * MainMarginX + StaveMargin + SignatureWidth + PostSignatureMargin + parts.Aggregate(0, (x, y) => Math.Max(x, y.Bars.Count())) * BarWidth - BarLineMargin / 2, MainMarginY * 2 + parts.Length * (8 * RankSpacing + StaffSpacing) - StaffSpacing);
                using (var g = Graphics.FromImage(DrawRet))
                {
                    for (int index = 0, loopTo = parts.Length - 1; index <= loopTo; index++)
                        DrawStave(g, parts[index].Clef, parts[index].Bars, MainMarginY + index * (8 * RankSpacing + StaffSpacing), RankSpacing, MainMarginX, BarWidth, ts, key, Scale);
                }

                return DrawRet;
            }

            private static void DrawKeySignature(Graphics g, int topLineY, int rankHeightY, int startX, int widthX, SimpleInterval Key, Mode Scale, Clef clef)
            {
                int position = 0;
                for (int ScaleDegree = 0; ScaleDegree <= 6; ScaleDegree++)
                {
                    var AbsoluteNote = Key + Scale.get_Interval((SimpleDistance)Conversions.ToInteger(ScaleDegree));
                    int offset = AbsoluteNote.SemiTones - Mode.Instance("Ionian").get_Interval(AbsoluteNote.Number).SemiTones;
                    switch (offset)
                    {
                        case 0:
                            {
                                break;
                            }

                        case 1:
                            {
                                int currentrank = clef.MCRankFromTopLine - (int)AbsoluteNote.Number;
                                while (currentrank < 0)
                                    currentrank += 7;
                                while (currentrank >= 7)
                                    currentrank -= 7;
                                g.DrawString("#", new Font("calibri", 12f), Brushes.Black, new Point(startX + 27 + position * 8, topLineY + currentrank * rankHeightY - 10));
                                position += 1;
                                break;
                            }

                        case -1:
                            {
                                int currentrank = clef.MCRankFromTopLine - (int)AbsoluteNote.Number;
                                while (currentrank < 0)
                                    currentrank += 7;
                                while (currentrank >= 7)
                                    currentrank -= 7;
                                g.DrawString("b", new Font("calibri", 12f), Brushes.Black, new Point(startX + 27 + position * 8, topLineY + currentrank * rankHeightY - 10));
                                position += 1;
                                break;
                            }

                        default:
                            {
                                break;
                            }
                            // CRITICAL
                            // Throw New NotImplementedException()
                    }
                }
            }

            private static void DrawStave(Graphics g, Clef clef, Event[][][] bars, int topLineY, int rankHeightY, int startX, int barWidth, TimeSignature TimeSignature, SimpleInterval Key, Mode Scale)
            {
                g.DrawLine(Pens.Black, startX, topLineY, startX, topLineY + rankHeightY * 8);
                TimeSignature.Draw(g, topLineY, rankHeightY, startX + StaveMargin, SignatureWidth);
                clef.Draw(g, topLineY, rankHeightY, startX + StaveMargin, SignatureWidth);
                DrawKeySignature(g, topLineY, rankHeightY, startX + StaveMargin, SignatureWidth, Key, Scale, clef);
                int StartPosition = startX + StaveMargin + SignatureWidth + PostSignatureMargin;
                for (int line = 0; line <= 4; line++)
                    g.DrawLine(Pens.Black, startX, topLineY + line * rankHeightY * 2, StartPosition + bars.Count() * barWidth - BarLineMargin / 2, topLineY + line * rankHeightY * 2);
                for (int index = 0, loopTo = bars.Count() - 1; index <= loopTo; index++)
                {
                    g.DrawLine(Pens.Black, StartPosition + (index + 1) * barWidth - BarLineMargin / 2, topLineY, StartPosition + (index + 1) * barWidth - BarLineMargin / 2, topLineY + rankHeightY * 8);
                    DrawBar(g, clef, bars[index], topLineY, rankHeightY, StartPosition + index * barWidth + BarLineMargin, barWidth - BarLineMargin, TimeSignature.BarLengthW);
                }
            }

            // must have at least one voice
            private static void DrawBar(Graphics g, Clef clef, Event[][] voices, int topLineY, int rankHeightY, int startX, int barWidth, double barWidthW)
            {
                switch (voices.Length)
                {
                    case 1:
                        {
                            DrawVoice(g, clef, voices[0], 0, 4, topLineY, rankHeightY, startX, barWidth, barWidthW);
                            break;
                        }

                    case 2:
                        {
                            DrawVoice(g, clef, voices[0], -1, 1, topLineY, rankHeightY, startX, barWidth, barWidthW);
                            DrawVoice(g, clef, voices[1], 1, 7, topLineY, rankHeightY, startX, barWidth, barWidthW);
                            break;
                        }

                    default:
                        {
                            throw new NotImplementedException();
                            break;
                        }
                }
            }

            // use stem direction
            private static void DrawVoice(Graphics g, Clef clef, Event[] voice, int stems, int restRankFromTopLine, int topLineY, int rankHeightY, int startX, int barWidth, double barWidthW)
            {
                double timeW = 0d;
                for (int index = 0, loopTo = voice.Length - 1; index <= loopTo; index++)
                {
                    int PositionY;
                    if (voice[index].Pitch.HasValue)
                    {
                        int ranknumber = clef.MCRankFromTopLine - voice[index].Pitch.Value.OffsetFromC4;
                        int X1 = startX + (int)Math.Round(timeW / barWidthW * barWidth) - rankHeightY;
                        int X2 = startX + (int)Math.Round(timeW / barWidthW * barWidth) + rankHeightY;
                        for (int ledgerRank = -2, loopTo1 = ranknumber; ledgerRank >= loopTo1; ledgerRank -= 2)
                            g.DrawLine(Pens.Black, X1 - 2, topLineY + rankHeightY * ledgerRank, X2 + 2, topLineY + rankHeightY * ledgerRank);
                        for (int ledgerRank = 10, loopTo2 = ranknumber; ledgerRank <= loopTo2; ledgerRank += 2)
                            g.DrawLine(Pens.Black, X1 - 2, topLineY + rankHeightY * ledgerRank, X2 + 2, topLineY + rankHeightY * ledgerRank);
                        PositionY = topLineY + rankHeightY * ranknumber;
                        g.FillEllipse(Brushes.Black, new Rectangle(X1, PositionY - rankHeightY, rankHeightY * 2, rankHeightY * 2));
                        if (stems == -1 || stems == 0 && ranknumber <= 4)
                        {
                            g.DrawLine(Pens.Black, X2, PositionY, X2, PositionY - stemlength);
                        }
                        else
                        {
                            g.DrawLine(Pens.Black, X1, PositionY, X1, PositionY + stemlength);
                        }
                    }
                    else
                    {
                        PositionY = topLineY + rankHeightY * restRankFromTopLine;
                        g.FillEllipse(Brushes.Blue, new Rectangle(startX + (int)Math.Round(timeW / barWidthW * barWidth) - rankHeightY, PositionY - rankHeightY, rankHeightY * 2, rankHeightY * 2));
                    }

                    switch (voice[index].Dot)
                    {
                        case 0:
                            {
                                timeW += Math.Pow(2d, -voice[index].WholeDivisionPower);
                                break;
                            }

                        case 1:
                            {
                                timeW += Math.Pow(2d, -voice[index].WholeDivisionPower) * 1.5d;
                                break;
                            }

                        default:
                            {
                                throw new NotImplementedException();
                                break;
                            }
                    }
                }
            }
        }

        public class GeneralDisposable<T> : IDisposable
        {
            private T iItem;

            /// <summary>Safe</summary>
            public T Item
            {
                get
                {
                    return iItem;
                }
            }

            public GeneralDisposable(T pItem, Action pDisposal)
            {
                iItem = pItem;
                iDisposal = pDisposal;
            }

            private Action iDisposal;

            public void Dispose()
            {
                iDisposal.Invoke();
            }
        }


        // Public Shared ReadOnly DiminishedUnison As New SimpleInterval(SimpleDistance.Unison_0, QualityP._Diminished)
        // Public Shared ReadOnly Unison As New SimpleInterval(SimpleDistance.Unison_0, QualityP._Perfect)
        // Public Shared ReadOnly AugmentedUnison As New SimpleInterval(SimpleDistance.Unison_0, QualityP._Augmented)
        // Public Shared ReadOnly MinorSecond As New SimpleInterval(SimpleDistance._Second_1, QualityMi._Minor)
        // Public Shared ReadOnly MajorSecond As New SimpleInterval(SimpleDistance._Second_1, QualityMi._Major)
        // Public Shared ReadOnly MinorThird As New SimpleInterval(SimpleDistance._Third_3, QualityMi._Minor)
        // Public Shared ReadOnly MajorThird As New SimpleInterval(SimpleDistance._Third_3, QualityMi._Major)
        // Public Shared ReadOnly Fourth As New SimpleInterval(SimpleDistance.Fourth_5, QualityP._Perfect)
        // Public Shared ReadOnly AugmentedFourth As New SimpleInterval(SimpleDistance.Fourth_5, QualityP._Augmented)
        // Public Shared ReadOnly DiminishedFifth As New SimpleInterval(SimpleDistance.Fifth_7, QualityP._Diminished)
        // Public Shared ReadOnly Fifth As New SimpleInterval(SimpleDistance.Fifth_7, QualityP._Perfect)
        // Public Shared ReadOnly MinorSixth As New SimpleInterval(SimpleDistance._Sixth_8, QualityMi._Minor)
        // Public Shared ReadOnly MajorSixth As New SimpleInterval(SimpleDistance._Sixth_8, QualityMi._Major)
        // Public Shared ReadOnly MinorSeventh As New SimpleInterval(SimpleDistance._Seventh_10, QualityMi._Minor)
        // Public Shared ReadOnly MajorSeventh As New SimpleInterval(SimpleDistance._Seventh_10, QualityMi._Major)
        // no error scrutiny, complete



        // CCFRITICAL

        // <Serializable> Public Enum Voice
        // Bass
        // Tenor
        // Alto
        // Soprano
        // End Enum

        // Public Overloads Function ToString(mode As Mode, chord As Chord) As String
        // Dim Built = If(chord.Character.iUpper, GetDegreeSymbol(chord.Root.Number), GetDegreeSymbol(chord.Root.Number).ToLower())
        // Return GetAccidental(chord.Root.SemiTones - mode.Interval(chord.Root.Number).SemiTones) &
        // Built.Substring(Built.IndexOf("_"c) + 1) & If(iCharacter = Nothing, String.Empty, iCharacter) & If(Seventh.HasValue, "7", String.Empty) & chord.Inversion.ToString().Substring(chord.Inversion.ToString().LastIndexOf("_"c) + 1)
        // End Function

        // 'Public Overloads Function ToString(chord As Chord) As String
        // Dim Built = If(chord.Character.iUpper, GetDegreeSymbol(chord.Root.Number), GetDegreeSymbol(chord.Root.Number).ToLower())
        // Return GetAccidental(chord.Root.SemiTones - Mode.NaturalFromC.Interval(chord.Root.Number).SemiTones) &
        // Built.Substring(Built.IndexOf("_"c) + 1) & If(iCharacter = Nothing, String.Empty, iCharacter) & If(Seventh.HasValue, "7", String.Empty) & chord.Inversion.ToString().Substring(chord.Inversion.ToString().LastIndexOf("_"c) + 1)
        // End Function

        // Public Overrides Function ToString() As String
        // Return Instances.Where(Function(x) x.Value.Item2 Is Me)(0).Value.Item1
        // End Function

        // Public Overloads Function ToString(mode As Mode) As String
        // Return iCharacter.ToString(mode, Me)
        // End Function

        // Public Overrides Function ToString() As String
        // Return iCharacter.ToString(Me)
        // End Function

    }

    public static class mdChorale
    {
        [Serial]
        public abstract class Condition
        {
            public abstract bool Evaluate(object Param);
        }

        [Serial]
        public enum Comparison
        {
            Greater,
            Less,
            GreaterEq,
            LessEq,
            IsNot,
            Is
        }

        [Serial]
        public class ValueCondition : Condition
        {
            public override string ToString()
            {
                switch (Comparison)
                {
                    case Comparison.Greater:
                        {
                            return Left + " > " + Right;
                        }

                    case Comparison.GreaterEq:
                        {
                            return Left + " >= " + Right;
                        }

                    case Comparison.Is:
                        {
                            return Left + " = " + Right;
                        }

                    case Comparison.IsNot:
                        {
                            return Left + " <> " + Right;
                        }

                    case Comparison.Less:
                        {
                            return Left + " < " + Right;
                        }

                    case Comparison.LessEq:
                        {
                            return Left + " <= " + Right;
                        }
                }

                return default;
            }

            public Type Type;
            public string Left;
            public Comparison Comparison;
            public string Right;

            private static object GetMember(object parent, string id)
            {
                if (id.Contains(Conversions.ToString('(')))
                {
                    string name = id.Substring(0, id.IndexOf('('));
                    id = id.Substring(id.IndexOf('(') + 1);
                    return ((Array)Interaction.CallByName(parent, name, CallType.Get, Array.Empty<object>())).GetValue(Conversions.ToLong(id.Substring(0, id.Length - 1)));
                }
                else
                {
                    return Interaction.CallByName(parent, id, CallType.Get, Array.Empty<object>());
                }
            }

            private static object GetMemberFull(object parent, string id)
            {
                while (id.Contains(Conversions.ToString('.')))
                {
                    parent = GetMember(parent, id.Substring(0, id.IndexOf('.')));
                    id = id.Substring(id.IndexOf('.') + 1);
                }

                return GetMember(parent, id);
            }

            private static object GetMemberEither(object parent, string id, Type type = null)
            {
                if (id.StartsWith("."))
                {
                    return GetMemberFull(parent, id.Substring(1));
                }
                else
                {
                    return Create(type ?? parent.GetType(), id);
                }
            }

            public override bool Evaluate(object Param)
            {
                var current = GetMemberEither(Param, Left);
                var other = GetMemberEither(Param, Right, current.GetType());
                switch (Comparison)
                {
                    case Comparison.Greater:
                        {
                            return Comparer.Default.Compare(current, other) > 0;
                        }

                    case Comparison.GreaterEq:
                        {
                            return Comparer.Default.Compare(current, other) >= 0;
                        }

                    case Comparison.Is:
                        {
                            return current.Equals(other);
                        }

                    case Comparison.IsNot:
                        {
                            return !current.Equals(other);
                        }

                    case Comparison.Less:
                        {
                            return Comparer.Default.Compare(current, other) < 0;
                        }

                    case Comparison.LessEq:
                        {
                            return Comparer.Default.Compare(current, other) <= 0;
                        }
                }

                return default;
            }
        }

        public enum BinaryOperation
        {
            And,
            Or,
            Xor,
            Equals
        }

        [Serial]
        public class BinaryCondition : Condition
        {
            public override string ToString()
            {
                switch (Operation)
                {
                    case BinaryOperation.And:
                        {
                            return "(" + Condition1.ToString() + " And " + Condition2.ToString() + ")";
                        }

                    case BinaryOperation.Equals:
                        {
                            return "(" + Condition1.ToString() + " = " + Condition2.ToString() + ")";
                        }

                    case BinaryOperation.Or:
                        {
                            return "(" + Condition1.ToString() + " Or " + Condition2.ToString() + ")";
                        }

                    case BinaryOperation.Xor:
                        {
                            return "(" + Condition1.ToString() + " XOr " + Condition2.ToString() + ")";
                        }
                }

                return default;
            }

            public Condition Condition1;
            public BinaryOperation Operation;
            public Condition Condition2;

            public override bool Evaluate(object Param)
            {
                switch (Operation)
                {
                    case BinaryOperation.And:
                        {
                            return Condition1.Evaluate(Param) & Condition2.Evaluate(Param);
                        }

                    case BinaryOperation.Equals:
                        {
                            return Condition1.Evaluate(Param) == Condition2.Evaluate(Param);
                        }

                    case BinaryOperation.Or:
                        {
                            return Condition1.Evaluate(Param) | Condition2.Evaluate(Param);
                        }

                    case BinaryOperation.Xor:
                        {
                            return Condition1.Evaluate(Param) ^ Condition2.Evaluate(Param);
                        }
                }

                return default;
            }
        }

        [Serial]
        public class NotCondition : Condition
        {
            public override string ToString()
            {
                return "(Not " + Condition.ToString() + ")";
            }

            public Condition Condition;

            public override bool Evaluate(object Param)
            {
                return !Condition.Evaluate(Param);
            }
        }

        public static object Create(Type Type, string Data)
        {
            if (Type.IsEnum)
            {
                if (Enum.GetNames(Type).Contains(Data))
                {
                    return Enum.Parse(Type, Data);
                }
                else if (Enum.GetNames(Type).Where(x => x.Contains(Data)).Count() == 1)
                {
                    return Enum.Parse(Type, Enum.GetNames(Type).Where(x => x.Contains(Data)).First());
                }
                else if (Enum.GetNames(Type).Where(x => x.Contains(Data) && x.Replace(Data, string.Empty).ToCharArray().Distinct().Count() == 1 && x.Replace(Data, string.Empty).ToCharArray().Distinct().ElementAtOrDefault(0) == '_').Count() == 1)
                {
                    return Enum.Parse(Type, Enum.GetNames(Type).Where(x => x.Contains(Data) && x.Replace(Data, string.Empty).ToCharArray().Distinct().Count() == 1 && x.Replace(Data, string.Empty).ToCharArray().Distinct().ElementAtOrDefault(0) == '_').First());
                }
            }
            else
            {
                var Parsers = Type.GetMethods().Where(x => x.GetParameters().Length == 1 && x.IsStatic == true && ReferenceEquals(x.ReturnType, Type) && ReferenceEquals(x.GetParameters()[0].ParameterType, typeof(string)));
                if (Parsers.Count() != 1)
                {
                    throw new Exception("");
                }
                else
                {
                    return Parsers.ElementAtOrDefault(0).Invoke(null, new[] { Data });
                }
            }

            return default;
        }

        [Serial]
        public class Rule
        {
            public override string ToString()
            {
                if (Active & Affirmative)
                {
                    return Name + ": Required " + Condition.ToString();
                }
                else if (Active & !Affirmative)
                {
                    return Name + ": Banned " + Condition.ToString();
                }
                else if (Affirmative)
                {
                    return Name + " (inactive): Required " + Condition.ToString();
                }
                else
                {
                    return Name + " (inactive): Banned " + Condition.ToString();
                }
            }

            public Condition Condition;
            public bool Affirmative;
            public bool Active;
            public string Name;
        }

        [Serial]
        public class RuleList
        {
            public Draw Conditions = new Draw<string, Rule>();
        }

        [ImmutableObject(true)]
        public sealed class Voice : UserIndexes
        {
            private string iName;
            private int iMinimum;
            private int iMaximum;

            /// <summary>Safe</summary>
            public int Maximum
            {
                get
                {
                    return iMaximum;
                }
            }

            /// <summary>Safe</summary>
            public int Minimum
            {
                get
                {
                    return iMinimum;
                }
            }

            /// <summary>Safe</summary>
            public string Name
            {
                get
                {
                    return iName;
                }
            }

            /// <exception cref="ArgumentException">pName null or empty</exception>
            public Voice(string Name, int Minimum, int Maximum) : base()
            {
                if (string.IsNullOrEmpty(Name))
                    throw new ArgumentException("null or empty", "pName");
                iName = Name;
                iMinimum = Minimum;
                iMaximum = Maximum;
                Add(typeof(Voice));
            }

            protected override bool Deserialize(string Contents)
            {
                var Result = Contents.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                if (Result.Length != 3)
                    return false;
                iName = Result[0];
                if (!int.TryParse(Result[1], out iMinimum))
                    return false;
                if (!int.TryParse(Result[2], out iMaximum))
                    return false;
                return true;
            }

            protected override string Serialize()
            {
                return iName + ';' + iMinimum.ToString() + ';' + iMaximum.ToString();
            }

            public override string ToString()
            {
                return iName + " (" + iMinimum + "-" + iMaximum + ")";
            }

            private Voice(bool SD) : base()
            {
            }

            /// <summary>Safe</summary>
            public static int Count()
            {
                return Count(typeof(Voice));
            }

            /// <summary>Safe, *:*</summary>
            public static IEnumerable<string> Names()
            {
                return (IEnumerable<string>)Names().ElementAtOrDefault(typeof(Voice));
            }

            /// <summary>*</summary>
        /// <exception cref="ArgumentException">Not listed or invalid</exception>
            public static Voice Instance(int Index)
            {
                return Instance(typeof(Voice), Index, new Voice(false));
            }

            /// <summary>*</summary>
        /// <exception cref="ArgumentNullException">Name</exception>
        /// <exception cref="ArgumentException">Not listed or invalid</exception>
            public static Voice Instance(string Name)
            {
                if (Name is null)
                    throw new ArgumentNullException("Name");
                return Instance(typeof(Voice), Name, new Voice(false));
            }
        }



        // principles:
        // no duplicate data
        // maximum screening
        // enumerate all

        // need pitch, tone
        // Voice, Integer, CompoundInterval, 2-Degree, 2-Pitch
        public class Movement
        {
            public Movement(Voice pVoice, int pDirection, mdNotation.CompoundInterval pInterval, (mdNotation.SimpleInterval, mdNotation.SimpleInterval) pDegree, (mdNotation.Pitch, mdNotation.Pitch) pPitch)
            {
                iPitch = pPitch;
                iVoice = pVoice;
                iDegree = pDegree;
                iDirection = pDirection;
                iInterval = pInterval;
            }

            private (mdNotation.Pitch, mdNotation.Pitch) iPitch;
            private (mdNotation.SimpleInterval, mdNotation.SimpleInterval) iDegree;
            private Voice iVoice;
            private int iDirection;
            private mdNotation.CompoundInterval iInterval;

            /// <summary>Safe</summary>
            public (mdNotation.Pitch, mdNotation.Pitch) Pitch
            {
                get
                {
                    return iPitch;
                }
            }

            /// <summary>Safe</summary>
            public (mdNotation.SimpleInterval, mdNotation.SimpleInterval) Degree
            {
                get
                {
                    return iDegree;
                }
            }

            /// <summary>Safe</summary>
            public mdNotation.CompoundInterval Interval
            {
                get
                {
                    return iInterval;
                }
            }

            /// <summary>Safe</summary>
            public int Direction
            {
                get
                {
                    return iDirection;
                }
            }

            /// <summary>Safe</summary>
            public Voice Voice
            {
                get
                {
                    return iVoice;
                }
            }
        }

        // ( Movement, Movement)
        // Bottom, Top, First, Second
        public class MovementPair
        {
            private (Movement, Movement) obj;

            public MovementPair((Movement, Movement) pObj)
            {
                obj = pObj;
            }

            public Movement Bottom
            {
                get
                {
                    return obj.Item1;
                }
            }

            public Movement Top
            {
                get
                {
                    return obj.Item2;
                }
            }

            public mdNotation.CompoundInterval First
            {
                get
                {
                    return (obj.Item2.Pitch.Item1 - obj.Item1.Pitch.Item1).Item2;
                }
            }

            public mdNotation.CompoundInterval Second
            {
                get
                {
                    return (obj.Item2.Pitch.Item2 - obj.Item1.Pitch.Item2).Item2;
                }
            }
        }

        // 3 Int
        // THIRDS etc
        [Serial]
        public struct Doubling : IEquatable<Doubling>
        {
            public bool IsAllowed(IEnumerable<Rule> rules)
            {
                foreach (var x in rules)
                {
                    if (x.Active && x.Affirmative != x.Condition.Evaluate(this))
                        return false;
                }

                return true;
            }

            public override string ToString()
            {
                if (Sevenths == 0)
                {
                    return Roots + " Roots, " + Thirds + " Thirds, " + Fifths + " Fifths";
                }
                else
                {
                    return Roots + " Roots, " + Thirds + " Thirds, " + Fifths + " Fifths, " + Sevenths + " Sevenths";
                }
            }

            public bool Equals(Doubling other)
            {
                return Data.Equals(other.Data);
            }

            public override bool Equals(object obj)
            {
                return obj is object && obj is Doubling && Equals((Doubling)obj);
            }

            public (int, int, int) Data;

            /// <summary>Safe</summary>
            public int Roots
            {
                get
                {
                    return Data.Item1;
                }

                set
                {
                    Data.Item1 = value;
                }
            }

            /// <summary>Safe</summary>
            public int Thirds
            {
                get
                {
                    return Data.Item2;
                }

                set
                {
                    Data.Item2 = value;
                }
            }

            /// <summary>Safe</summary>
            public int Fifths
            {
                get
                {
                    return Data.Item3;
                }

                set
                {
                    Data.Item3 = value;
                }
            }

            /// <summary>Safe</summary>
            public int Sevenths
            {
                get
                {
                    return Voice.Count() - Roots - Thirds - Fifths;
                }
            }

            public Doubling(int pRoots, int pThirds, int pFifths)
            {
                Data = (pRoots, pThirds, pFifths);
            }

            public Doubling((int, int, int) pData)
            {
                Data = pData;
            }
        }

        // Doubling, SimpleDistance()
        // THIRDS etc, DEGREES, INVERSION
        public class Distribution
        {
            private mdNotation.SimpleDistance[] iParts;
            private Doubling iDoubling;

            /// <summary>Safe</summary>
            public int Roots
            {
                get
                {
                    return iDoubling.Roots;
                }
            }

            /// <summary>Safe</summary>
            public int Thirds
            {
                get
                {
                    return iDoubling.Thirds;
                }
            }

            /// <summary>Safe</summary>
            public int Fifths
            {
                get
                {
                    return iDoubling.Fifths;
                }
            }

            /// <summary>Safe</summary>
            public int Sevenths
            {
                get
                {
                    return iDoubling.Sevenths;
                }
            }

            /// <summary>Safe</summary>
            public mdNotation.Inversion Inversion
            {
                get
                {
                    switch (iParts[0])
                    {
                        case mdNotation.SimpleDistance.Unison_0:
                            {
                                return mdNotation.Inversion.Root_Position_;
                            }

                        case mdNotation.SimpleDistance._Third_3:
                            {
                                return mdNotation.Inversion.First_Inversion_b;
                            }

                        case mdNotation.SimpleDistance.Fifth_7:
                            {
                                return mdNotation.Inversion.Second_Inversion_c;
                            }

                        case mdNotation.SimpleDistance._Seventh_10:
                            {
                                return mdNotation.Inversion.Third_Inversion_d;
                            }
                    }

                    return default;
                }
            }

            /// <summary>Safe</summary>
            public mdNotation.SimpleDistance[] Degrees
            {
                get
                {
                    return iParts;
                }
            }

            public Distribution(Doubling DB, mdNotation.SimpleDistance[] pParts)
            {
                iDoubling = DB;
                iParts = pParts;
            }

            public override string ToString()
            {
                return string.Join(", ", iParts);
            }

            public static IEnumerable<Distribution> GetNew(IEnumerable<Rule> Rules)
            {
                var tr = new List<Distribution>();
                GetNew(new List<mdNotation.SimpleDistance>(), tr);
                return tr.Where(dist => !Rules.Any(cRule => cRule.Active && cRule.Affirmative != cRule.Condition.Evaluate(dist)));
            }

            private static void GetNew(List<mdNotation.SimpleDistance> Intervals, List<Distribution> Result)
            {
                if (Intervals.Count == Voice.Count())
                {
                    var Db = Intervals.Aggregate(new Doubling(), (x, y) =>
                    {
                        switch (y)
                        {
                            case mdNotation.SimpleDistance.Unison_0:
                                {
                                    x.Roots += 1;
                                    break;
                                }

                            case mdNotation.SimpleDistance._Third_3:
                                {
                                    x.Thirds += 1;
                                    break;
                                }

                            case mdNotation.SimpleDistance.Fifth_7:
                                {
                                    x.Fifths += 1;
                                    break;
                                }
                        }

                        return x;
                    });
                    if (Db.IsAllowed(Chorale.My.MySettingsProperty.Settings.Rules.Conditions("Doubling")))
                        Result.Add(new Distribution(Db, Intervals.ToArray()));
                }
                else
                {
                    Intervals.Add(mdNotation.SimpleDistance.Unison_0);
                    GetNew(Intervals, Result);
                    Intervals.RemoveAt(Intervals.Count - 1);
                    Intervals.Add(mdNotation.SimpleDistance._Third_3);
                    GetNew(Intervals, Result);
                    Intervals.RemoveAt(Intervals.Count - 1);
                    Intervals.Add(mdNotation.SimpleDistance.Fifth_7);
                    GetNew(Intervals, Result);
                    Intervals.RemoveAt(Intervals.Count - 1);
                    Intervals.Add(mdNotation.SimpleDistance._Seventh_10);
                    GetNew(Intervals, Result);
                    Intervals.RemoveAt(Intervals.Count - 1);
                }
            }
        }

        // Key, Chord, Pitch(), IntervalVoicing
        // KEY, PITCHES, CHORD, THIRDS etc, DEGREES, INVERSION, SPACES, Tones
        public class Voicing
        {
            private mdNotation.Key iKey;
            private mdNotation.Chord iChord;
            private mdNotation.Pitch[] iNotes;
            private Distribution iVoicing;

            private Voicing(mdNotation.Pitch[] pNotes, mdNotation.Chord pChord, mdNotation.Key pKey, Distribution pVoicing)
            {
                iChord = pChord;
                iNotes = pNotes;
                iKey = pKey;
                iVoicing = pVoicing;
            }

            /// <summary>Safe</summary>
            public mdNotation.Inversion Inversion
            {
                get
                {
                    return iVoicing.Inversion;
                }
            }

            /// <summary>Safe</summary>
            public mdNotation.SimpleDistance[] Degrees
            {
                get
                {
                    return iVoicing.Degrees;
                }
            }

            /// <summary>Safe</summary>
            public mdNotation.SimpleInterval[] Tones
            {
                get
                {
                    var Result = new[] { iNotes[0].IntervalFromC0.Interval - iKey.Tonic, iNotes[1].IntervalFromC0.Interval - iKey.Tonic, iNotes[2].IntervalFromC0.Interval - iKey.Tonic, iNotes[3].IntervalFromC0.Interval - iKey.Tonic };
                    return Result;
                }
            }

            /// <summary>Safe</summary>
            public int Roots
            {
                get
                {
                    return iVoicing.Roots;
                }
            }

            /// <summary>Safe</summary>
            public int Thirds
            {
                get
                {
                    return iVoicing.Thirds;
                }
            }

            /// <summary>Safe</summary>
            public int Fifths
            {
                get
                {
                    return iVoicing.Fifths;
                }
            }

            /// <summary>Safe</summary>
            public int Sevenths
            {
                get
                {
                    return iVoicing.Sevenths;
                }
            }

            /// <summary>Safe</summary>
            public mdNotation.Key Key
            {
                get
                {
                    return iKey;
                }
            }

            /// <summary>Safe</summary>
            public mdNotation.Pitch[] Notes
            {
                get
                {
                    return iNotes;
                }
            }

            /// <summary>Safe</summary>
            public mdNotation.CompoundInterval[] Spaces
            {
                get
                {
                    return new[] { (Notes[1] - Notes[0]).Item2, (Notes[2] - Notes[1]).Item2, (Notes[3] - Notes[2]).Item2 };
                }
            }

            /// <summary>Safe</summary>
            public mdNotation.Chord Chord
            {
                get
                {
                    return iChord;
                }
            }

            public override string ToString()
            {
                return string.Join(", ", iNotes);
            }

            private static void GetNewOV(Distribution Iv, bool Accending, List<(int, mdNotation.SimpleDistance)> Progress, List<(Distribution, (int, mdNotation.SimpleDistance)[])> Result)
            {
                if (Iv.Degrees.Count() == Progress.Count)
                {
                    Result.Add((Iv, Progress.ToArray()));
                    return;
                }

                var Interval = Iv.Degrees[Progress.Count];
                int Octaves = 0;
                do
                {
                    Octaves += 1;
                    if (new mdNotation.CompoundInterval(new mdNotation.SimpleInterval(-3, Interval), Octaves).SemiTones > Voice.Instance(Progress.Count).Maximum - 11)
                        break;
                    if (new mdNotation.CompoundInterval(new mdNotation.SimpleInterval(3, Interval), Octaves).SemiTones < Voice.Instance(Progress.Count).Minimum - 24 || Accending && Progress.Any() && Octaves * 7 + (int)Interval < Progress.Last().Item1 * 7 + (int)Progress.Last().Item2)
                        continue;
                    Progress.Add((Octaves, Interval));
                    GetNewOV(Iv, Accending, Progress, Result);
                    Progress.RemoveAt(Progress.Count - 1);
                }
                while (true);
            }

            public static IEnumerable<(Distribution, (int, mdNotation.SimpleDistance)[])> GetNewOV(bool Accending, IEnumerable<Rule> DistRules)
            {
                var tr = new List<(Distribution, (int, mdNotation.SimpleDistance)[])>();
                foreach (var item in Distribution.GetNew(DistRules))
                    GetNewOV(item, Accending, new List<(int, mdNotation.SimpleDistance)>(), tr);
                return tr.AsEnumerable();
            }

            public static IEnumerable<Voicing> GetNew(IEnumerable<mdNotation.Chord> pChords, mdNotation.Key pKey, bool Accending, mdNotation.Pitch Melody, IEnumerable<Rule> DistRules, IEnumerable<Rule> Rules)
            {
                var tr = new List<Voicing>();
                foreach (var pChord in pChords)
                {
                    foreach (var Item in GetNewOV(Accending, DistRules).Where(X => pChord.Character.Seventh.HasValue || X.Item1.Sevenths == 0))
                    {
                        var Result = Array.ConvertAll(Item.Item2, x => new mdNotation.Pitch(pChord.Root + pKey.Tonic + new mdNotation.CompoundInterval(pChord.Character.get_Tone(x.Item2), x.Item1)));
                        if (Result.Last() == Melody)
                        {
                            bool Success = true;
                            for (int VoiceIndex = 0, loopTo = Voice.Count() - 1; VoiceIndex <= loopTo; VoiceIndex++)
                            {
                                if (Result[VoiceIndex].MIDIPitch < Voice.Instance(VoiceIndex).Minimum)
                                    Success = false;
                                if (Result[VoiceIndex].MIDIPitch > Voice.Instance(VoiceIndex).Maximum)
                                    Success = false;
                            }

                            if (Success)
                                tr.Add(new Voicing(Result, pChord, pKey, Item.Item1));
                        }
                    }
                }

                return tr.Where(vc => !Rules.Any(cRule => cRule.Active && cRule.Affirmative != cRule.Condition.Evaluate(vc)));
            }
        }

        // Voicing, c-Chord, Position
        // KEY, PITCHES, 2 CHORD, THIRDS etc, DEGREES, INVERSION, SPACES, POSITION, Tones
        public class Instant
        {
            public static implicit operator Voicing(Instant input)
            {
                return input.iVoicing;
            }

            private int iPosition;
            private mdNotation.Chord iCadence;
            private Voicing iVoicing;

            /// <summary>Safe</summary>
            public mdNotation.SimpleInterval[] Tones
            {
                get
                {
                    return iVoicing.Tones;
                }
            }

            /// <summary>Safe</summary>
            public mdNotation.CompoundInterval[] Spaces
            {
                get
                {
                    return iVoicing.Spaces;
                }
            }

            /// <summary>Safe</summary>
            public int Position
            {
                get
                {
                    return iPosition;
                }
            }

            /// <summary>Safe</summary>
            public mdNotation.Chord Cadence
            {
                get
                {
                    return iCadence;
                }
            }

            public Instant(Voicing pVoicing, mdNotation.Chord pCadence, int pPosition)
            {
                iVoicing = pVoicing;
                iCadence = pCadence;
                iPosition = pPosition;
            }

            /// <summary>Safe</summary>
            public mdNotation.Inversion Inversion
            {
                get
                {
                    return iVoicing.Inversion;
                }
            }

            /// <summary>Safe</summary>
            public mdNotation.SimpleDistance[] Degrees
            {
                get
                {
                    return iVoicing.Degrees;
                }
            }

            /// <summary>Safe</summary>
            public int Roots
            {
                get
                {
                    return iVoicing.Roots;
                }
            }

            /// <summary>Safe</summary>
            public int Thirds
            {
                get
                {
                    return iVoicing.Thirds;
                }
            }

            /// <summary>Safe</summary>
            public int Fifths
            {
                get
                {
                    return iVoicing.Fifths;
                }
            }

            /// <summary>Safe</summary>
            public int Sevenths
            {
                get
                {
                    return iVoicing.Sevenths;
                }
            }

            /// <summary>Safe</summary>
            public mdNotation.Key Key
            {
                get
                {
                    return iVoicing.Key;
                }
            }

            /// <summary>Safe</summary>
            public mdNotation.Pitch[] Notes
            {
                get
                {
                    return iVoicing.Notes;
                }
            }

            /// <summary>Safe</summary>
            public mdNotation.Chord Chord
            {
                get
                {
                    return iVoicing.Chord;
                }
            }
        }

        // Instant x 2
        // Key, Cadence, Position, First, Second, Movements, MovementPairs
        public class Transition
        {
            private Instant iFirst;
            private Instant iSecond;

            /// <summary>Safe</summary>
            public Movement[] Movements
            {
                get
                {
                    return new[] { new Movement(Voice.Instance(0), (iFirst.Notes[0] - iSecond.Notes[0]).Item1, (iFirst.Notes[0] - iSecond.Notes[0]).Item2, (iFirst.Tones[0], iSecond.Tones[0]), (iFirst.Notes[0], iSecond.Notes[0])), new Movement(Voice.Instance(1), (iFirst.Notes[1] - iSecond.Notes[1]).Item1, (iFirst.Notes[1] - iSecond.Notes[1]).Item2, (iFirst.Tones[1], iSecond.Tones[1]), (iFirst.Notes[1], iSecond.Notes[1])), new Movement(Voice.Instance(2), (iFirst.Notes[2] - iSecond.Notes[2]).Item1, (iFirst.Notes[2] - iSecond.Notes[2]).Item2, (iFirst.Tones[2], iSecond.Tones[2]), (iFirst.Notes[2], iSecond.Notes[2])), new Movement(Voice.Instance(3), (iFirst.Notes[3] - iSecond.Notes[3]).Item1, (iFirst.Notes[3] - iSecond.Notes[3]).Item2, (iFirst.Tones[3], iSecond.Tones[3]), (iFirst.Notes[3], iSecond.Notes[3])) };
                }
            }

            /// <summary>Safe</summary>
            public MovementPair[] MovementPairs
            {
                get
                {
                    var Result = Movements;
                    return new[] { new MovementPair((Result[0], Result[1])), new MovementPair((Result[0], Result[2])), new MovementPair((Result[0], Result[3])), new MovementPair((Result[1], Result[2])), new MovementPair((Result[1], Result[3])), new MovementPair((Result[2], Result[3])) };
                }
            }

            /// <summary>Safe</summary>
            public Instant First
            {
                get
                {
                    return iFirst;
                }
            }

            /// <summary>Safe</summary>
            public Instant Second
            {
                get
                {
                    return iSecond;
                }
            }

            /// <summary>Safe</summary>
            public int Position
            {
                get
                {
                    return iSecond.Position;
                }
            }

            /// <summary>Safe</summary>
            public mdNotation.Chord Cadence
            {
                get
                {
                    return iSecond.Cadence;
                }
            }

            public Transition(Instant pFirst, Instant pSecond)
            {
                iFirst = pFirst;
                iSecond = pSecond;
            }

            /// <summary>Safe</summary>
            public mdNotation.Key Key
            {
                get
                {
                    return iSecond.Key;
                }
            }
        }
    }
}
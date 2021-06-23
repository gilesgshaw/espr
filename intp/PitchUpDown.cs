using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;

namespace intp
{

    //ToString could be improved. Parse too.
    //constructor may be dodgy
    //No fancy symbols
    public struct NamedPitch : IEquatable<NamedPitch>
    {
        public int Octave;
        public Letter Letter;
        public int Acc;

        public static bool TryParse(string input, out NamedPitch result)
        {
            result = default;
            if (string.IsNullOrEmpty(input) || !Enum.TryParse(input.Substring(0, 1).ToUpper(), out result.Letter)) return false;
            input = input.Substring(1).ToLower();
            while (input.Length > 0 && (input[0] == 'b' || input[0] == '#'))
            {
                result.Acc += input[0] == 'b' ? -1 : 1;
                input = input.Substring(1);
            }
            return int.TryParse(input, out result.Octave);
        }

        public override string ToString()
        {
            var tr = Letter.ToString();
            if (Acc > 0)
            {
                for (int i = 0; i < Acc; i++)
                {
                    tr += '#';
                }
            }
            else
            {
                for (int i = 0; i < -Acc; i++)
                {
                    tr += 'b';
                }
            }
            return tr + Octave;
        }

        public NamedPitch(Letter letter, int acc, int octave)
        {
            Octave = octave;
            Letter = letter;
            Acc = acc;
        }

        public NamedPitch(Letter letter, int octave)
        {
            Octave = octave;
            Letter = letter;
            Acc = 0;
        }

        public NamedPitch(Pitch Pitch)
        {
            var deg = Pitch.MIDINumber % 12;
            if (deg < 0) deg += 12;
            Octave = ((Pitch.MIDINumber - deg) / 12) - 1;
            switch (deg)
            {
                case 0:
                    Acc = 0;
                    Letter = Letter.C;
                    break;
                case 1:
                    Acc = 1;
                    Letter = Letter.C;
                    break;
                case 2:
                    Acc = 0;
                    Letter = Letter.D;
                    break;
                case 3:
                    Acc = -1;
                    Letter = Letter.E;
                    break;
                case 4:
                    Acc = 0;
                    Letter = Letter.E;
                    break;
                case 5:
                    Acc = 0;
                    Letter = Letter.F;
                    break;
                case 6:
                    Acc = 1;
                    Letter = Letter.F;
                    break;
                case 7:
                    Acc = 0;
                    Letter = Letter.G;
                    break;
                case 8:
                    Acc = -1;
                    Letter = Letter.A;
                    break;
                case 9:
                    Acc = 0;
                    Letter = Letter.A;
                    break;
                case 10:
                    Acc = -1;
                    Letter = Letter.B;
                    break;
                default:
                    Acc = 0;
                    Letter = Letter.B;
                    break;
            }
        }

        public static NamedPitch[] Sensible(Pitch Pitch)
        {
            var deg = Pitch.MIDINumber % 12;
            if (deg < 0) deg += 12;
            var Octave = ((Pitch.MIDINumber - deg) / 12) - 1;
            switch (deg)
            {
                case 0:
                    return new NamedPitch[] { new NamedPitch(Letter.C, Octave) };
                case 1:
                    return new NamedPitch[] { new NamedPitch(Letter.C, 1, Octave), new NamedPitch(Letter.D, -1, Octave) };
                case 2:
                    return new NamedPitch[] { new NamedPitch(Letter.D, Octave) };
                case 3:
                    return new NamedPitch[] { new NamedPitch(Letter.D, 1, Octave), new NamedPitch(Letter.E, -1, Octave) };
                case 4:
                    return new NamedPitch[] { new NamedPitch(Letter.E, Octave) };
                case 5:
                    return new NamedPitch[] { new NamedPitch(Letter.F, Octave) };
                case 6:
                    return new NamedPitch[] { new NamedPitch(Letter.F, 1, Octave), new NamedPitch(Letter.G, -1, Octave) };
                case 7:
                    return new NamedPitch[] { new NamedPitch(Letter.G, Octave) };
                case 8:
                    return new NamedPitch[] { new NamedPitch(Letter.G, 1, Octave), new NamedPitch(Letter.A, -1, Octave) };
                case 9:
                    return new NamedPitch[] { new NamedPitch(Letter.A, Octave) };
                case 10:
                    return new NamedPitch[] { new NamedPitch(Letter.A, 1, Octave), new NamedPitch(Letter.B, -1, Octave) };
                default:
                    return new NamedPitch[] { new NamedPitch(Letter.B, Octave) };
            }
        }

        public Pitch Pitch => (Octave + 1) * 12 + (int)Letter + Acc;

        public static explicit operator NamedPitch(Pitch p) => new NamedPitch(p);
        public static explicit operator Pitch(NamedPitch n) => n.Pitch;

        public override bool Equals(object obj) => obj is NamedPitch note && Equals(note);

        public bool Equals(NamedPitch other) => this == other;

        public override int GetHashCode()
        {
            int hashCode = 1831541484;
            hashCode = hashCode * -1521134295 + Octave.GetHashCode();
            hashCode = hashCode * -1521134295 + Letter.GetHashCode();
            hashCode = hashCode * -1521134295 + Acc.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(NamedPitch Left, NamedPitch Right) => Left.Octave == Right.Octave && Left.Letter == Right.Letter && Left.Acc == Right.Acc;

        public static bool operator !=(NamedPitch left, NamedPitch right) => !(left == right);
    }

    public enum Letter
    {
        C = 0, D = 2, E = 4, F = 5, G = 7, A = 9, B = 11
    }

    //working on Closest() but otherwise should be perfect.
    //later implement other tunings
    public struct Pitch : IEquatable<Pitch>, IComparable<Pitch>
    {
        public int MIDINumber;

        public static readonly Pitch A4 = new NamedPitch(Letter.A, 4).Pitch;
        public const double A4f = 440D;

        public static Pitch Closest(double f)
        {
            f = f / A4f;
            var semis = (int)(Math.Round((Math.Log(f, 2) * 12)));
            return A4 + semis;
        }

        public double f => Math.Pow(2, ((((double)(this - A4)) / 12))) * A4f;

        public static bool operator <(Pitch left, Pitch right) => (int)left < (int)right;

        public static int operator -(Pitch left, Pitch right) => (int)left - (int)right;

        public static bool operator >(Pitch left, Pitch right) => (int)left > (int)right;

        public static bool operator <=(Pitch left, Pitch right) => (int)left <= (int)right;

        public static bool operator >=(Pitch left, Pitch right) => (int)left >= (int)right;

        public int CompareTo(Pitch other) => ((int)this).CompareTo(other);

        public override string ToString()
        {
            return ((NamedPitch)this).ToString();
        }

        public static implicit operator int(Pitch p) => p.MIDINumber;
        public static implicit operator Pitch(int i) => new Pitch(i);

        public Pitch(int mIDINumber) => MIDINumber = mIDINumber;

        public override bool Equals(object obj) => obj is Pitch pitch && Equals(pitch);

        public bool Equals(Pitch other) => this == other;

        public override int GetHashCode() => -208393347 + MIDINumber.GetHashCode();

        public static bool operator ==(Pitch left, Pitch right) => left.MIDINumber == right.MIDINumber;

        public static bool operator !=(Pitch left, Pitch right) => left.MIDINumber != right.MIDINumber;
    }

    //not really tested especially min/max
    public class PitchUpDown : UpDownBase
    {
        public PitchUpDown(Pitch Min, Pitch Max) : this()
        {
            iMin = Min;
            iMax = Max;
            for (int i = Min; i <= Max; i++)
            {
                Items.AddRange(NamedPitch.Sensible(i));
            }
        }

        Pitch iMin;
        public Pitch Min
        {
            get => iMin;
            set
            {
                for (int i = iMin; i < value; i++)
                {
                    Items.RemoveRange(NamedPitch.Sensible(i));
                }
                for (int i = value; i < iMin; i++)
                {
                    Items.AddRange(NamedPitch.Sensible(i));
                }
                iMin = value;
            }
        }

        Pitch iMax;
        public Pitch Max
        {
            get => iMax;
            set
            {
                for (int i = iMax; i > value; i--)
                {
                    Items.RemoveRange(NamedPitch.Sensible(i));
                }
                for (int i = value; i > iMax; i--)
                {
                    Items.AddRange(NamedPitch.Sensible(i));
                }
                iMax = value;
            }
        }

        //-//////////////////////////////////////////////////////////

        private PitchUpDown() : base()
        {
            Items = new PitchUpDownItemCollection(this);
            Text = String.Empty;
        }

        private int iSelectedIndex = -1;

        private EventHandler onSelectedPitchChanged = null;

        public event EventHandler SelectedPitchChanged
        {
            add
            {
                onSelectedPitchChanged += value;
            }
            remove
            {
                onSelectedPitchChanged -= value;
            }
        }

        protected void OnSelectedPitchChanged(object source, EventArgs e)
        {

            // Call the event handler
            if (onSelectedPitchChanged != null)
            {
                onSelectedPitchChanged(this, e);
            }
        }

        protected override void OnChanged(object source, EventArgs e)
        {
            OnSelectedPitchChanged(source, e);
        }

        private PitchUpDownItemCollection Items;

        private class PitchUpDownItemCollection
        {
            PitchUpDown owner;
            List<NamedPitch> obj = new List<NamedPitch>();

            internal PitchUpDownItemCollection(PitchUpDown owner) => this.owner = owner;

            public int Count => obj.Count;

            public bool Contains(NamedPitch item) => obj.Contains(item);

            public void AddRange(IEnumerable<NamedPitch> collection) => obj.AddRange(collection);

            //validation...
            public bool RemoveRange(IEnumerable<NamedPitch> collection)
            {
                var result = true;
                foreach (var item in collection)
                {
                    result &= obj.Remove(item);
                }
                return result;
            }

            public int IndexOf(NamedPitch item) => obj.IndexOf(item);

            public NamedPitch this[int index]
            {
                get
                {
                    return obj[index];
                }

                set
                {
                    obj[index] = value;

                    if (((owner.UserEdit) ? -1 : owner.iSelectedIndex) == index)
                    {
                        owner.SelectIndex(index);
                    }

                }
            }

            public void Remove(NamedPitch item)
            {
                int index = obj.IndexOf(item);

                if (index == -1)
                {
                    throw new ArgumentOutOfRangeException("item");
                }
                else
                {
                    RemoveAt(index);
                }
            }

            public void RemoveAt(int item)
            {
                // Overridden to update the internal index if neccessary
                obj.RemoveAt(item);

                if (item < owner.iSelectedIndex)
                {
                    // The item removed was before the currently selected item
                    owner.SelectIndex(owner.iSelectedIndex - 1);
                }
                else if (item == owner.iSelectedIndex)
                {
                    // The currently selected item was removed
                    //
                    owner.SelectIndex(-1);
                }
            }

        }

        public NamedPitch? SelectedPitch
        {
            get
            {
                return (UserEdit || iSelectedIndex < 0) ? null : new NamedPitch?(Items[(iSelectedIndex)]);
            }
            set
            {

                // Treat null as selecting no item
                //
                if (value.HasValue)
                {
                    // Attempt to find the given item in the list of items
                    //
                    for (int i = 0; i < Items.Count; i++)
                    {
                        if (value == Items[i])
                        {
                            if (i != ((UserEdit) ? -1 : iSelectedIndex)) SelectIndex(i);
                            break;
                        }
                    }
                }
                else
                {
                    if (-1 != ((UserEdit) ? -1 : iSelectedIndex)) SelectIndex(-1);
                }
            }
        }

        public override void UpButton()
        {
            // Make sure domain values exist, and there are >0 items
            if (Items.Count <= 0) return;

            // If the user has entered text, attempt to match it to the domain list
            if (UserEdit)
            {
                int matchIndex = MatchIndex(Text, iSelectedIndex);
                if (matchIndex != -1)
                {
                    // Found a match, so set the internal index accordingly
                    // We update the selected index and perform spinner action.
                    iSelectedIndex = matchIndex;
                }
            }

            if (iSelectedIndex < 0) return;
            var sp = Items[(iSelectedIndex)];
            // Otherwise, get the previous string in the domain list            

            var prev = (NamedPitch)(Pitch)(((Pitch)sp) + 1);

            //return if cannot do this
            if (!Items.Contains(prev)) return;

            SelectIndex(Items.IndexOf(prev));
        }

        public override void DownButton()
        {
            // Make sure domain values exist, and there are >0 items
            if (Items.Count <= 0) return;

            // If the user has entered text, attempt to match it to the domain list
            if (UserEdit)
            {
                int matchIndex = MatchIndex(Text, iSelectedIndex);
                if (matchIndex != -1)
                {
                    // Found a match, so set the internal index accordingly
                    // We update the selected index and perform spinner action.
                    iSelectedIndex = matchIndex;
                }
            }

            if (iSelectedIndex < 0) return;
            var sp = Items[(iSelectedIndex)];
            // Otherwise, get the previous string in the domain list            

            var prev = (NamedPitch)(Pitch)(((Pitch)sp) - 1);

            //return if cannot do this
            if (!Items.Contains(prev)) return;

            SelectIndex(Items.IndexOf(prev));
        }

        int MatchIndex(string text, int startPosition)
        {

            // Make sure domain values exist
            if (text.Length < 1 || Items.Count <= 0)
            {
                return -1;
            }

            if (startPosition < 0)
            {
                startPosition = Items.Count - 1;
            }
            if (startPosition >= Items.Count)
            {
                startPosition = 0;
            }

            // Attempt to match the supplied string text with
            // the domain list. Returns the index in the list if successful,
            // otherwise returns -1.
            int index = startPosition;
            int matchIndex = -1;
            bool found = false;

            text = text.ToUpper(CultureInfo.InvariantCulture);

            // Attempt to match the string with Items[index]
            do
            {
                found = Items[index].ToString().ToUpper(CultureInfo.InvariantCulture).StartsWith(text);

                if (found) matchIndex = index;

                // Calculate the next index to attempt to match
                index++;
                if (index >= Items.Count) index = 0;

            } while (!found && index != startPosition);

            return matchIndex;
        }

        protected override void OnTextBoxKeyPress(object source, KeyPressEventArgs e)
        {
            if (ReadOnly)
            {
                char[] character = new char[] { e.KeyChar };
                UnicodeCategory uc = Char.GetUnicodeCategory(character[0]);

                if (uc == UnicodeCategory.LetterNumber
                    || uc == UnicodeCategory.LowercaseLetter
                    || uc == UnicodeCategory.DecimalDigitNumber
                    || uc == UnicodeCategory.MathSymbol
                    || uc == UnicodeCategory.OtherLetter
                    || uc == UnicodeCategory.OtherNumber
                    || uc == UnicodeCategory.UppercaseLetter)
                {

                    // Attempt to match the character to a domain item
                    int matchIndex = MatchIndex(new string(character), iSelectedIndex + 1);
                    if (matchIndex != -1)
                    {

                        // Select the matching domain item
                        SelectIndex(matchIndex);
                    }
                    e.Handled = true;
                }
            }
            base.OnTextBoxKeyPress(source, e);
        }

        void SelectIndex(int index)
        {

            // Sanity check index

            Debug.Assert(index < Items.Count && index >= -1, "SelectValue: index out of range");
            if (index < -1 || index >= Items.Count)
            {
                // Defensive programming
                index = -1;
                return;
            }

            // If the selected index has changed, update the text

            iSelectedIndex = index;
            if (iSelectedIndex >= 0)
            {
                UserEdit = false;
                UpdateEditTextValue = Items[iSelectedIndex].ToString();
                UpdateEditText();
            }
            else
            {
                UserEdit = true;
            }

            Debug.Assert(iSelectedIndex >= 0 || UserEdit == true, "UserEdit should be true when iSelectedIndex < 0 " + UserEdit);
        }

        string UpdateEditTextValue = string.Empty;
        protected override void UpdateEditText()
        {

            Debug.Assert(!UserEdit, "UserEdit should be false");
            // Defensive programming
            UserEdit = false;

            ChangingText = true;
            Text = UpdateEditTextValue;
        }

        public override string ToString()
        {
            return base.ToString() + ", SelectedPitch: " + SelectedPitch.ToString();
        }
    }

    #region COPIED

    //not worried about help designing or attributes or comments or accessibility
    //public class DomainUpDown : UpDownBase
    //{

    //    private readonly static string DefaultValue = "";
    //    private readonly static bool DefaultWrap = false;

    //    // Member variables

    //    private DomainUpDownItemCollection domainItems = null;

    //    private string stringValue = DefaultValue;      // Current string value
    //    private int domainIndex = -1;                    // Index in the domain list
    //    private bool sorted = false;                 // Sort the domain values

    //    private bool wrap = DefaultWrap;             // Wrap around domain items

    //    private EventHandler onSelectedItemChanged = null;

    //    private bool inSort = false;

    //    public DomainUpDown() : base()
    //    {
    //        Text = String.Empty;
    //    }

    //    // Properties

    //    public DomainUpDownItemCollection Items
    //    {

    //        get
    //        {
    //            if (domainItems == null)
    //            {
    //                domainItems = new DomainUpDownItemCollection(this);
    //            }
    //            return domainItems;
    //        }
    //    }

    //    public new Padding Padding
    //    {
    //        get { return base.Padding; }
    //        set { base.Padding = value; }
    //    }

    //    public new event EventHandler PaddingChanged
    //    {
    //        add { base.PaddingChanged += value; }
    //        remove { base.PaddingChanged -= value; }
    //    }

    //    public int SelectedIndex
    //    {

    //        get
    //        {
    //            if (UserEdit)
    //            {
    //                return -1;
    //            }
    //            else
    //            {
    //                return domainIndex;
    //            }
    //        }

    //        set
    //        {
    //            if (value < -1 || value >= Items.Count)
    //            {
    //                throw new ArgumentOutOfRangeException("SelectedIndex");
    //            }

    //            if (value != SelectedIndex)
    //            {
    //                SelectIndex(value);
    //            }
    //        }
    //    }

    //    public object SelectedItem
    //    {
    //        get
    //        {
    //            int index = SelectedIndex;
    //            return (index == -1) ? null : Items[index];
    //        }
    //        set
    //        {

    //            // Treat null as selecting no item
    //            //
    //            if (value == null)
    //            {
    //                SelectedIndex = -1;
    //            }
    //            else
    //            {
    //                // Attempt to find the given item in the list of items
    //                //
    //                for (int i = 0; i < Items.Count; i++)
    //                {
    //                    if (value != null && value.Equals(Items[i]))
    //                    {
    //                        SelectedIndex = i;
    //                        break;
    //                    }
    //                }
    //            }
    //        }
    //    }

    //    public bool Sorted
    //    {

    //        get
    //        {
    //            return sorted;
    //        }

    //        set
    //        {
    //            sorted = value;
    //            if (sorted)
    //            {
    //                SortDomainItems();
    //            }
    //        }
    //    }

    //    public bool Wrap
    //    {

    //        get
    //        {
    //            return wrap;
    //        }

    //        set
    //        {
    //            wrap = value;
    //        }
    //    }

    //    // Methods

    //    public event EventHandler SelectedItemChanged
    //    {
    //        add
    //        {
    //            onSelectedItemChanged += value;
    //        }
    //        remove
    //        {
    //            onSelectedItemChanged -= value;
    //        }
    //    }

    //    public override void DownButton()
    //    {

    //        // Make sure domain values exist, and there are >0 items
    //        //
    //        if (domainItems == null)
    //        {
    //            return;
    //        }
    //        if (domainItems.Count <= 0)
    //        {
    //            return;
    //        }

    //        // If the user has entered text, attempt to match it to the domain list
    //        //            
    //        int matchIndex = -1;
    //        if (UserEdit)
    //        {
    //            matchIndex = MatchIndex(Text, false, domainIndex);
    //        }
    //        if (matchIndex != -1)
    //        {

    //            // Found a match, so select this value
    //            //
    //            domainIndex = matchIndex;
    //        }

    //        // Otherwise, get the next string in the domain list
    //        //

    //        if (domainIndex < domainItems.Count - 1)
    //        {
    //            SelectIndex(domainIndex + 1);
    //        }
    //        else if (Wrap)
    //        {
    //            SelectIndex(0);
    //        }

    //    }

    //    internal int MatchIndex(string text, bool complete)
    //    {
    //        return MatchIndex(text, complete, domainIndex);
    //    }

    //    internal int MatchIndex(string text, bool complete, int startPosition)
    //    {

    //        // Make sure domain values exist
    //        if (domainItems == null)
    //        {
    //            return -1;
    //        }

    //        // Sanity check of parameters
    //        if (text.Length < 1)
    //        {
    //            return -1;
    //        }
    //        if (domainItems.Count <= 0)
    //        {
    //            return -1;
    //        }
    //        if (startPosition < 0)
    //        {
    //            startPosition = domainItems.Count - 1;
    //        }
    //        if (startPosition >= domainItems.Count)
    //        {
    //            startPosition = 0;
    //        }

    //        // Attempt to match the supplied string text with
    //        // the domain list. Returns the index in the list if successful,
    //        // otherwise returns -1.
    //        int index = startPosition;
    //        int matchIndex = -1;
    //        bool found = false;

    //        if (!complete)
    //        {
    //            text = text.ToUpper(CultureInfo.InvariantCulture);
    //        }

    //        // Attempt to match the string with Items[index]
    //        do
    //        {
    //            if (complete)
    //                found = Items[index].ToString().Equals(text);
    //            else
    //                found = Items[index].ToString().ToUpper(CultureInfo.InvariantCulture).StartsWith(text);

    //            if (found)
    //            {
    //                matchIndex = index;
    //            }

    //            // Calculate the next index to attempt to match
    //            index++;
    //            if (index >= domainItems.Count)
    //            {
    //                index = 0;
    //            }

    //        } while (!found && index != startPosition);

    //        return matchIndex;
    //    }

    //    protected override void OnChanged(object source, EventArgs e)
    //    {
    //        OnSelectedItemChanged(source, e);
    //    }

    //    protected override void OnTextBoxKeyPress(object source, KeyPressEventArgs e)
    //    {
    //        if (ReadOnly)
    //        {
    //            char[] character = new char[] { e.KeyChar };
    //            UnicodeCategory uc = Char.GetUnicodeCategory(character[0]);

    //            if (uc == UnicodeCategory.LetterNumber
    //                || uc == UnicodeCategory.LowercaseLetter
    //                || uc == UnicodeCategory.DecimalDigitNumber
    //                || uc == UnicodeCategory.MathSymbol
    //                || uc == UnicodeCategory.OtherLetter
    //                || uc == UnicodeCategory.OtherNumber
    //                || uc == UnicodeCategory.UppercaseLetter)
    //            {

    //                // Attempt to match the character to a domain item
    //                int matchIndex = MatchIndex(new string(character), false, domainIndex + 1);
    //                if (matchIndex != -1)
    //                {

    //                    // Select the matching domain item
    //                    SelectIndex(matchIndex);
    //                }
    //                e.Handled = true;
    //            }
    //        }
    //        base.OnTextBoxKeyPress(source, e);
    //    }

    //    protected void OnSelectedItemChanged(object source, EventArgs e)
    //    {

    //        // Call the event handler
    //        if (onSelectedItemChanged != null)
    //        {
    //            onSelectedItemChanged(this, e);
    //        }
    //    }

    //    private void SelectIndex(int index)
    //    {

    //        // Sanity check index

    //        Debug.Assert(domainItems != null, "Domain values array is null");
    //        Debug.Assert(index < domainItems.Count && index >= -1, "SelectValue: index out of range");
    //        if (domainItems == null || index < -1 || index >= domainItems.Count)
    //        {
    //            // Defensive programming
    //            index = -1;
    //            return;
    //        }

    //        // If the selected index has changed, update the text

    //        domainIndex = index;
    //        if (domainIndex >= 0)
    //        {
    //            stringValue = domainItems[domainIndex].ToString();
    //            UserEdit = false;
    //            UpdateEditText();
    //        }
    //        else
    //        {
    //            UserEdit = true;
    //        }

    //        Debug.Assert(domainIndex >= 0 || UserEdit == true, "UserEdit should be true when domainIndex < 0 " + UserEdit);
    //    }

    //    private void SortDomainItems()
    //    {
    //        if (inSort)
    //            return;

    //        inSort = true;
    //        try
    //        {
    //            // Sanity check
    //            Debug.Assert(sorted == true, "Sorted == false");
    //            if (!sorted)
    //            {
    //                return;
    //            }

    //            if (domainItems != null)
    //            {

    //                // Sort the domain values
    //                ArrayList.Adapter(domainItems).Sort(new DomainUpDownItemCompare());

    //                // Update the domain index
    //                if (!UserEdit)
    //                {
    //                    int newIndex = MatchIndex(stringValue, true);
    //                    if (newIndex != -1)
    //                    {
    //                        SelectIndex(newIndex);
    //                    }
    //                }
    //            }
    //        }
    //        finally
    //        {
    //            inSort = false;
    //        }
    //    }

    //    public override string ToString()
    //    {

    //        string s = base.ToString();

    //        if (Items != null)
    //        {
    //            s += ", Items.Count: " + Items.Count.ToString(CultureInfo.CurrentCulture);
    //            s += ", SelectedIndex: " + SelectedIndex.ToString(CultureInfo.CurrentCulture);
    //        }
    //        return s;
    //    }

    //    public override void UpButton()
    //    {

    //        // Make sure domain values exist, and there are >0 items
    //        if (domainItems == null)
    //        {
    //            return;
    //        }
    //        if (domainItems.Count <= 0)
    //        {
    //            return;
    //        }

    //        // If the user has entered text, attempt to match it to the domain list
    //        int matchIndex = -1;
    //        if (UserEdit)
    //        {
    //            matchIndex = MatchIndex(Text, false, domainIndex);
    //        }
    //        if (matchIndex != -1)
    //        {

    //            // Found a match, so set the domain index accordingly
    //            // We update the selected index and perform spinner action.
    //            domainIndex = matchIndex;
    //        }

    //        // Otherwise, get the previous string in the domain list            

    //        if (domainIndex > 0)
    //        {
    //            SelectIndex(domainIndex - 1);
    //        }
    //        else if (Wrap)
    //        {
    //            SelectIndex(domainItems.Count - 1);
    //        }
    //    }

    //    protected override void UpdateEditText()
    //    {

    //        Debug.Assert(!UserEdit, "UserEdit should be false");
    //        // Defensive programming
    //        UserEdit = false;

    //        ChangingText = true;
    //        Text = stringValue;
    //    }

    //    public class DomainUpDownItemCollection : ArrayList
    //    {
    //        DomainUpDown owner;

    //        internal DomainUpDownItemCollection(DomainUpDown owner)
    //        : base()
    //        {
    //            this.owner = owner;
    //        }

    //        public override object this[int index]
    //        {
    //            get
    //            {
    //                return base[index];
    //            }

    //            set
    //            {
    //                base[index] = value;

    //                if (owner.SelectedIndex == index)
    //                {
    //                    owner.SelectIndex(index);
    //                }

    //                if (owner.Sorted)
    //                {
    //                    owner.SortDomainItems();
    //                }
    //            }
    //        }

    //        public override int Add(object item)
    //        {
    //            // Overridden to perform sorting after adding an item

    //            int ret = base.Add(item);
    //            if (owner.Sorted)
    //            {
    //                owner.SortDomainItems();
    //            }
    //            return ret;
    //        }

    //        public override void Remove(object item)
    //        {
    //            int index = IndexOf(item);

    //            if (index == -1)
    //            {
    //                throw new ArgumentOutOfRangeException("item");
    //            }
    //            else
    //            {
    //                RemoveAt(index);
    //            }
    //        }

    //        public override void RemoveAt(int item)
    //        {
    //            // Overridden to update the domain index if neccessary
    //            base.RemoveAt(item);

    //            if (item < owner.domainIndex)
    //            {
    //                // The item removed was before the currently selected item
    //                owner.SelectIndex(owner.domainIndex - 1);
    //            }
    //            else if (item == owner.domainIndex)
    //            {
    //                // The currently selected item was removed
    //                //
    //                owner.SelectIndex(-1);
    //            }
    //        }

    //        public override void Insert(int index, object item)
    //        {
    //            base.Insert(index, item);
    //            if (owner.Sorted)
    //            {
    //                owner.SortDomainItems();
    //            }
    //        }
    //    }

    //    private sealed class DomainUpDownItemCompare : IComparer
    //    {

    //        public int Compare(object p, object q)
    //        {
    //            if (p == q) return 0;
    //            if (p == null || q == null)
    //            {
    //                return 0;
    //            }

    //            return String.Compare(p.ToString(), q.ToString(), false, CultureInfo.CurrentCulture);
    //        }
    //    }
    //}

    #endregion

}

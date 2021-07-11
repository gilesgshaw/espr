using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using winUtility;

namespace Chorale
{
    public partial class Form1
    {
        public Form1()
        {
            InitializeComponent();
            _CreateToolStripMenuItem.Name = "CreateToolStripMenuItem";
            _TEMPToolStripMenuItem.Name = "TEMPToolStripMenuItem";
        }

        // Public Sub FacilitateInstances(Of T)()
        // Dim parent = New ToolStripMenuItem(GetType(T).Name & "s")
        // tsmiData.DropDownItems.Add(parent)
        // AddHandler parent.DropDownItems.Add("View " & GetType(T).Name & "s").Click, Sub()
        // Dim Result = UserEnumerate(Of T)()
        // If Result.Count = 0 Then
        // MsgBox("No " & GetType(T).Name & "s")
        // Else
        // MsgBox(Result.Aggregate("", Function(x, y) x & vbNewLine & vbNewLine & y.Item1 & ": " & y.Item2.ToString).Substring(4))
        // End If
        // End Sub
        // AddHandler parent.DropDownItems.Add("Add " & GetType(T).Name).Click, Sub()
        // Construct(Of T)("Enter New " & GetType(T).Name, AppDomain.CurrentDomain.GetAssemblies(), False)
        // End Sub
        // End Sub

        // Public Sub FacilitateInstances(Of T)(list As ICollection(Of T), Name As String)
        // Dim parent = New ToolStripMenuItem(Name & "s")
        // tsmiData.DropDownItems.Add(parent)
        // AddHandler parent.DropDownItems.Add("View " & Name & "s").Click, Sub()
        // If list.Count = 0 Then
        // MsgBox("No " & Name & "s")
        // Else
        // MsgBox(list.Aggregate("", Function(x, y) x & vbNewLine & vbNewLine & y.ToString).Substring(4))
        // End If
        // End Sub
        // AddHandler parent.DropDownItems.Add("Add " & Name).Click, Sub()
        // Dim value = Construct(Of T)("Enter New " & Name, AppDomain.CurrentDomain.GetAssemblies())
        // list.Add(value)
        // End Sub
        // AddHandler parent.DropDownItems.Add("Remove " & Name).Click, Sub() list.Remove(XMsgBox("Choose " & Name & " to remove", list, True))
        // End Sub

        private void Form1_Load(object sender, EventArgs e)
        {
            // FacilitateInstances(Of Mode)()
            // FacilitateInstances(Of Character)()
            // FacilitateInstances(Of Voice)()
            // FacilitateInstances(Of Display.Clef)()
            // FacilitateInstances(CType(My.Settings.Rules.Conditions("Doubling"), ICollection(Of Rule)), "Doubling Rule")
            // FacilitateInstances(CType(My.Settings.Rules.Conditions("Distribution"), ICollection(Of Rule)), "Distribution Rule")
            // FacilitateInstances(CType(My.Settings.Rules.Conditions("Voicing"), ICollection(Of Rule)), "Voicing Rule")
            // FacilitateInstances(CType(My.Settings.Rules.Conditions("Instant"), ICollection(Of Rule)), "Instant Rule")
            // FacilitateInstances(CType(My.Settings.Rules.Conditions("Transition"), ICollection(Of Rule)), "Transition Rule")
            // FacilitateInstances(CType(My.Settings.Rules.Conditions("Movement"), ICollection(Of Rule)), "Movement Rule")
            // FacilitateInstances(CType(My.Settings.Rules.Conditions("Movement Pair"), ICollection(Of Rule)), "Movement Pair Rule")

            AllowedChords.Add(new mdNotation.Chord(new mdNotation.SimpleInterval(mdNotation.SimpleDistance.Unison_0, mdNotation.QualityP._Perfect), mdNotation.CharacterType.Instance("Minor")));
            AllowedChords.Add(new mdNotation.Chord(new mdNotation.SimpleInterval(mdNotation.SimpleDistance._Second_1, mdNotation.QualityMi._Major), mdNotation.CharacterType.Instance("Diminished")));
            // AllowedChords.Add(New Chord(New SimpleInterval(SimpleDistance._Third_3, QualityMi._Minor), Character.Instance("Augmented")))
            AllowedChords.Add(new mdNotation.Chord(new mdNotation.SimpleInterval(mdNotation.SimpleDistance.Fourth_5, mdNotation.QualityP._Perfect), mdNotation.CharacterType.Instance("Minor")));
            AllowedChords.Add(new mdNotation.Chord(new mdNotation.SimpleInterval(mdNotation.SimpleDistance.Fifth_7, mdNotation.QualityP._Perfect), mdNotation.CharacterType.Instance("Major")));
            AllowedChords.Add(new mdNotation.Chord(new mdNotation.SimpleInterval(mdNotation.SimpleDistance._Sixth_8, mdNotation.QualityMi._Minor), mdNotation.CharacterType.Instance("Major")));
            // AllowedChords.Add(New Chord(New SimpleInterval(SimpleDistance._Seventh_10, QualityMi._Major), Character.Instance("Diminished")))

        }

        private List<mdNotation.Chord> AllowedChords = new List<mdNotation.Chord>();

        private void ListItem(mdChorale.Instant[] data, mdNotation.Key key)
        {
            var obj = new MIDIData() { Notes = Array.ConvertAll(data, x => x.Notes.Reverse().ToArray()), Key = key };
            var c = new PictureBox() { Image = obj.GetBitmap() };
            c.Size = c.Image.Size;
            FlowLayoutPanel1.Controls.Add(c);
            c.Click += () => PlayMIDI(obj);
        }


        private void CreateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FlowLayoutPanel1.Controls.Clear();
            var Key = new mdNotation.Key(new mdNotation.SimpleInterval(mdNotation.SimpleDistance._Third_3, mdNotation.QualityMi._Minor), mdNotation.Mode.Instance("Aeolian"));
            var Soprano = Array.Empty<mdNotation.Pitch>();
            var Options = new List<mdNotation.Pitch>();
            for (int Distance = 6; Distance <= 100; Distance++)
            {
                int val = Distance - (int)Key.Tonic.Number;
                while (val >= 7)
                    val -= 7;
                var Pitch = new mdNotation.Pitch(mdNotation.CompoundInterval.GetNew(Distance, (Key.Tonic + Key.Scale.get_Interval((mdNotation.SimpleDistance)Conversions.ToInteger(val))).Quality));
                if (Pitch.MIDIPitch <= mdChorale.Voice.Instance(3).Maximum && Pitch.MIDIPitch >= mdChorale.Voice.Instance(3).Minimum)
                {
                    Options.Add(Pitch);
                }
            }

            var ROptions = Options.ToArray();
            do
            {
                var RPictutes = Array.ConvertAll(ROptions, x =>
                {
                    var val = new MIDIData() { Notes = Array.ConvertAll(Soprano.Concat(new[] { x }).ToArray(), y => new[] { y }), Key = Key }.GetBitmap();
                    return ((Image)val, x);
                });

                // CHANGED HERE
                var RRPictures = Array.ConvertAll(RPictutes, x => x.Item1);
                var Result = mdMessage.XMsgBox("Choose Pitch", RRPictures.Cast<object>().AsEnumerable(), true, default, null, default, null, null, null);
                if (Result is null)
                {
                    break;
                }
                else
                {
                    Array.Resize(ref Soprano, Soprano.Length + 1);
                    Soprano[Soprano.Length - 1] = RPictutes.Where(x => ReferenceEquals(x.Item1, Result)).First().Item2;
                }
            }
            while (true);
            var Trials = new List<List<mdChorale.Instant>>(new[] { new List<mdChorale.Instant>() });
            for (int Position = 0, loopTo = Soprano.Length - 1; Position <= loopTo; Position++)
            {
                var results = mdChorale.Voicing.GetNew(AllowedChords, Key, true, Soprano[Soprano.Length - 1 - Position], My.MySettingsProperty.Settings.Rules.Conditions["Distribution"], My.MySettingsProperty.Settings.Rules.Conditions["Voicing"]);
                for (int i = Trials.Count - 1; i >= 0; i -= 1)
                {
                    int iL = i;
                    foreach (var item in results)
                    {
                        var nextValue = new mdChorale.Instant(item, Trials[i].Any() ? Trials[i][0].Chord : item.Chord, Position);
                        if (!My.MySettingsProperty.Settings.Rules.Conditions["Instant"].Any(cRule => cRule.Active && cRule.Affirmative != cRule.Condition.Evaluate(nextValue)) && (Trials[iL].Count == 0 || !My.MySettingsProperty.Settings.Rules.Conditions["Transition"].Any(tRule => tRule.Active && tRule.Affirmative != tRule.Condition.Evaluate(new mdChorale.Transition(nextValue, Trials[iL].Last())))) && (Trials[iL].Count == 0 || !My.MySettingsProperty.Settings.Rules.Conditions["Movement Pair"].Any(pRule => pRule.Active && new mdChorale.Transition(nextValue, Trials[iL].Last()).MovementPairs.Any(mvp => pRule.Affirmative != pRule.Condition.Evaluate(mvp)))) && (Trials[iL].Count == 0 || !My.MySettingsProperty.Settings.Rules.Conditions["Movement"].Any(iRule => iRule.Active && new mdChorale.Transition(nextValue, Trials[iL].Last()).Movements.Any(mv =>
                        {
                            // DELETE
                            int g = Trials.Count;
                            bool value = iRule.Condition.Evaluate(mv);
                            return iRule.Affirmative != value;
                        }))))
                        {
                            Trials[i].Add(nextValue);
                            Trials.Add(new List<mdChorale.Instant>(Trials[i]));
                            Trials[i].RemoveAt(Trials[i].Count - 1);
                        }
                    }

                    Trials.RemoveAt(i);
                }
            }

            foreach (var Trial in Trials)
            {
                Trial.Reverse();
                ListItem(Trial.ToArray(), Key);
            }

            Interaction.MsgBox(FlowLayoutPanel1.Controls.Count);
        }

        private void TEMPToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace intp
{
    public static class Ut
    {

        //do these actually work? where are thead illigal oprtation exceptions thrown?
        /// <summary>Thread safe to get Value. Represents a Windows spin box (also known as an up-down control) that displays numeric values.</summary>
        [DefaultBindingProperty("Value")]
        [DefaultEvent("ValueChanged")]
        [DefaultProperty("Value")]
        public class VSNumericUpDown : NumericUpDown
        {
            public VSNumericUpDown() : base()
            {
                iValue = base.Value;
            }

            /// <summary>Raises the System.Windows.Forms.NumericUpDown.ValueChanged event.</summary>
            /// <param name="e">A System.EventArgs that contains the event data.</param>
            protected override void OnValueChanged(EventArgs e)
            {
                iValue = base.Value;
                base.OnValueChanged(e);
            }

            decimal iValue;
            /// <summary>Thread safe to get. Gets or sets the value assigned to the spin box (also known as an up-down control).</summary>
            /// <exception cref="System.ArgumentOutOfRangeException">The assigned value is less than the System.Windows.Forms.NumericUpDown.Minimum property value or the assigned value is greater than the System.Windows.Forms.NumericUpDown.Maximum property value.</exception>
            /// <value>The numeric value of the System.Windows.Forms.NumericUpDown control.</value>
            [Bindable(true)]
            public new decimal Value
            {
                get => iValue;
                set => base.Value = value;
            }
        }

        #region DEAD_PLACEHOLDER_COPIES

        public class SiSComboBox : ComboBox
        {
            public SiSComboBox() : base()
            {
                iSelectedIndex = base.SelectedIndex;
            }

            protected override void OnSelectedIndexChanged(EventArgs e)
            {
                iSelectedIndex = base.SelectedIndex;
                base.OnSelectedIndexChanged(e);
            }

            int iSelectedIndex;
            [Bindable(true)]
            public new int SelectedIndex
            {
                get => iSelectedIndex;
                set => base.SelectedIndex = value;
            }
        }

        //is this needed??
        public class SpSPitchUpDown : PitchUpDown
        {
            public SpSPitchUpDown(Pitch Min, Pitch Max) : base(Min, Max)
            {
                iSelectedPitch = base.SelectedPitch;
            }

            protected override void OnChanged(object Source ,EventArgs e)
            {
                iSelectedPitch = base.SelectedPitch;
                base.OnChanged(Source, e);
            }

            NamedPitch? iSelectedPitch;
            public new NamedPitch? SelectedPitch
            {
                get => iSelectedPitch;
                set => base.SelectedPitch = value;
            }
        }

        public class CSCheckBox : CheckBox
        {
            public CSCheckBox() : base()
            {
                iChecked = base.Checked;
            }

            protected override void OnCheckedChanged(EventArgs e)
            {
                iChecked = base.Checked;
                base.OnCheckedChanged(e);
            }

            bool iChecked;
            public new bool Checked
            {
                get => iChecked;
                set => base.Checked = value;
            }
        }

        #endregion



        //dont forget to raise event
        public abstract class GuiSetting
        {
            protected GuiSetting(Control Control) => this.Control = Control;

            public Control Control { get; }

            public event EventHandler ValueChanged;

            protected virtual void OnValueChanged(EventArgs e)
            {
                EventHandler handler = ValueChanged;
                handler?.Invoke(this, e);
            }
        }

        public abstract class GuiSetting<T> : GuiSetting
        {
            protected GuiSetting(Control Control, T DefaultValue) : base(Control) => this.DefaultValue = DefaultValue;

            public abstract T Value { get; set; }

            public T DefaultValue { get; }
        }

        public class NudSetting : GuiSetting<decimal>
        {
            public NudSetting(int Width, decimal Min, decimal Max, decimal DefaultValue, decimal Increment = 1M, int DecimalPlaces = 0) : base(new VSNumericUpDown(), DefaultValue)
            {
                NudControl = (VSNumericUpDown)Control;
                NudControl.ThousandsSeparator = true;
                NudControl.Minimum = Min;
                NudControl.Width = Width;
                NudControl.Maximum = Max;
                NudControl.DecimalPlaces = DecimalPlaces;
                NudControl.Increment = Increment;
                NudControl.Value = DefaultValue;
                NudControl.ValueChanged += (sender, e) => OnValueChanged(e);
            }

            public VSNumericUpDown NudControl { get; private set; }

            public override decimal Value
            {
                get => NudControl.Value;
                set => NudControl.Value = value;
            }
        }

        public class PudSetting : GuiSetting<NamedPitch>
        {
            public PudSetting(int Width, Pitch Min, Pitch Max, NamedPitch DefaultValue) : base(new SpSPitchUpDown(Min, Max), DefaultValue)
            {
                PudControl = (SpSPitchUpDown)Control;
                PudControl.Width = Width;
                PudControl.ReadOnly = true;
                PudControl.SelectedPitch = DefaultValue;
                PudControl.SelectedPitchChanged += (sender, e) => OnValueChanged(e);
            }

            public SpSPitchUpDown PudControl { get; private set; }

            public override NamedPitch Value
            {
                get
                {
                    var tr = PudControl.SelectedPitch;
                    Debug.Assert(!(tr is null));
                    return tr.Value;
                }
                set => PudControl.SelectedPitch = value;
            }
        }

        public class CbSetting : GuiSetting<bool>
        {
            public CbSetting(bool DefaultValue) : base(new CSCheckBox(), DefaultValue)
            {
                CBControl = (CSCheckBox)Control;
                CBControl.AutoSize = true;
                CBControl.Checked = DefaultValue;
                CBControl.CheckedChanged += (sender, e) => OnValueChanged(e);
            }

            public CSCheckBox CBControl { get; private set; }

            public override bool Value
            {
                get => CBControl.Checked;
                set => CBControl.Checked = value;
            }
        }

        //public class CoSetting : GuiSetting
        //{
        //    public CoSetting(Size Size, Type Type)
        //    {
        //        var AddCBRet = new SiSComboBox();
        //        foreach (string Item in Enum.GetNames(Type))
        //            AddCBRet.Items.Add(Item.Replace("_", " "));
        //        AddCBRet.Size = Size;
        //        AddCBRet.SelectedIndex = 0;
        //        AddCBRet.DropDownStyle = ComboBoxStyle.DropDownList;
        //        AddCBRet.FormattingEnabled = true;
        //    }
        //}

        public class ConditionalSetting<T> : GuiSetting<T>
        {
            GuiSetting<T> Subject;
            GuiSetting<bool> Enabler;
            bool ResetWhenDisabled;

            public ConditionalSetting(GuiSetting<T> Subject, GuiSetting<bool> Enabler, bool ResetWhenDisabled = false) : base(Subject.Control, Subject.DefaultValue)
            {
                this.Subject = Subject;
                this.Enabler = Enabler;
                this.ResetWhenDisabled = ResetWhenDisabled;
                Enabler.ValueChanged += (sender, e) => Refresh();
                Subject.ValueChanged += (sender, e) => OnValueChanged(e);
                Refresh();
            }

            void Refresh()
            {
                if (Control.Enabled && !Enabler.Value)
                {
                    Control.Enabled = false;
                    if (ResetWhenDisabled) Value = DefaultValue;
                }
                else if (!Control.Enabled && Enabler.Value)
                {
                    Control.Enabled = true;
                }
            }

            public override T Value { get => Subject.Value; set => Subject.Value = value; }
        }

        //controls are aligned to left hopefully
        //layout is a mess but is seems to work
        //give (strig, null) for now
        public class SettingPanel
        {
            private static Label GetLabelMR(string Text)
            {
                var Item = new Label();
                Item.AutoSize = false;
                Item.Anchor = AnchorStyles.None;
                Item.Dock = DockStyle.Fill;
                Item.TextAlign = ContentAlignment.MiddleRight;
                Item.Text = Text;
                return Item;
            }

            List<(string, GuiSetting)> Rows;
            TableLayoutPanel Parent;

            private class IDComparer<T> : IEqualityComparer<T> where T : class
            {
                private IDComparer(){}

                public static readonly IDComparer<T> One = new IDComparer<T>();

                public bool Equals(T x, T y)
                {
                    return object.ReferenceEquals(x, y);
                }

                public int GetHashCode(T obj)
                {
                    return obj.GetHashCode();
                }
            }

            Dictionary<Control, Label> EventRegister = new Dictionary<Control, Label>(IDComparer<Control>.One);

            void WhenEnabledChanged(Object sender, EventArgs e)
            {
                var nSender = (Control)sender;
                if (!EventRegister.ContainsKey(nSender)) return;
                EventRegister[nSender].Enabled = nSender.Enabled;
            }

            void Update()
            {

                foreach (var item in EventRegister)
                {
                    item.Key.EnabledChanged -= WhenEnabledChanged;
                }
                EventRegister.Clear();

                Parent.Controls.Clear();
                Parent.RowStyles.Clear();
                Parent.AutoSize = true;
                Parent.RowCount = Rows.Count + 1;
                for (int i = 0; i < Rows.Count; i++)
                {
                    Parent.RowStyles.Add(new RowStyle(SizeType.Absolute, 27F));
                    var label = GetLabelMR(Rows[i].Item1);
                    Parent.Controls.Add(label, 0, i);
                    var ct = Rows[i].Item2?.Control;
                    if (ct is null) continue;
                    Parent.Controls.Add(ct, 1, i);
                    ct.Anchor = AnchorStyles.Left;
                    ct.Margin = new Padding(3, 5, 3, 3);

                    EventRegister.Add(ct, label);
                    ct.EnabledChanged += WhenEnabledChanged;
                    WhenEnabledChanged(ct, new EventArgs());

                }
                Parent.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            }

            private SettingPanel(int col1, TableLayoutPanel pnl)
            {
                Parent = pnl;
                pnl.ColumnCount = 2;
                pnl.ColumnStyles.Clear();
                pnl.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, col1));
                pnl.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
                //pnl.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            }

            //parent needs to have all the right details - size, border
            public SettingPanel(TableLayoutPanel pnl, int Col1) : this(Col1, pnl)
            {
                Rows = new List<(string, GuiSetting)>();
                Update();
            }

            public SettingPanel(TableLayoutPanel pnl, (string, GuiSetting)[] InitialRows, int Col1) : this(Col1, pnl)
            {
                Rows = new List<(string, GuiSetting)>(InitialRows);
                Update();
            }

            public void Add(string Text, GuiSetting Setting)
            {
                Rows.Add((Text, Setting));
                Update();
            }

            //public event EventHandler ObjUpdate;
        }





        public static Color HsvToRgb(double h, double S, double V)
        {
            double H = h;
            while (H < 0) { H += 360; };
            while (H >= 360) { H -= 360; };
            double R, G, B;
            if (V <= 0)
            { R = G = B = 0; }
            else if (S <= 0)
            {
                R = G = B = V;
            }
            else
            {
                double hf = H / 60.0;
                int i = (int)Math.Floor(hf);
                double f = hf - i;
                double pv = V * (1 - S);
                double qv = V * (1 - S * f);
                double tv = V * (1 - S * (1 - f));
                switch (i)
                {

                    // Red is the dominant color

                    case 0:
                        R = V;
                        G = tv;
                        B = pv;
                        break;

                    // Green is the dominant color

                    case 1:
                        R = qv;
                        G = V;
                        B = pv;
                        break;
                    case 2:
                        R = pv;
                        G = V;
                        B = tv;
                        break;

                    // Blue is the dominant color

                    case 3:
                        R = pv;
                        G = qv;
                        B = V;
                        break;
                    case 4:
                        R = tv;
                        G = pv;
                        B = V;
                        break;

                    // Red is the dominant color

                    case 5:
                        R = V;
                        G = pv;
                        B = qv;
                        break;

                    // Just in case we overshoot on our math by a little, we put these here. Since its a switch it won't slow us down at all to put these here.

                    case 6:
                        R = V;
                        G = tv;
                        B = pv;
                        break;
                    case -1:
                        R = V;
                        G = pv;
                        B = qv;
                        break;

                    // The color is not defined, we should throw an error.

                    default:
                        //LFATAL("i Value error in Pixel conversion, Value is %d", i);
                        R = G = B = V; // Just pretend its black/white
                        break;
                }
            }
            return Color.FromArgb(Clamp((int)(R * 255.0)), Clamp((int)(G * 255.0)), Clamp((int)(B * 255.0)));
        }

        /// <summary>
        /// Clamp a value to 0-255
        /// </summary>
        public static int Clamp(int i)
        {
            if (i < 0) return 0;
            if (i > 255) return 255;
            return i;
        }


    }
}

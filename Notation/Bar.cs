using System;
using System.Collections.Generic;
using mus;

namespace Notation
{
    public partial class Display
    {

        //should I try and make this (mostly) immutable?
        public class Bar
        {
            public Key Sig { get; }
            public Clef Clef { get; }
            public float Top { get; }
            public float Bottom { get => Top + Height; }
            public float Height { get; }

            public float X { get; private set; }

            private Dictionary<int, IntervalC> accs; //counting from middle C

            public Bar(Key sig, Clef clef, float top, float height, float initialX)
            {
                Sig = sig;
                Clef = clef;
                Top = top;
                Height = height;
                X = initialX;
                accs = new Dictionary<int, IntervalC>();
            }

            public void AdvanceTo(float x)
            {
                if (x < X) throw new ArgumentException();
                X = x;
            }

            public void ResetAccidentals()
            {
                accs.Clear();
            }

            public int? QueryAccidental(Pitch note)
            {

                var desired = note.FromMC;
                int number = desired.Number;

                var current = Sig.GetModeFromC().IntervalC(number);
                if (accs.ContainsKey(number)) current = accs[number];

                if (current == desired) return null;

                accs[number] = desired;
                return desired.Quality;

            }
        }

    }
}

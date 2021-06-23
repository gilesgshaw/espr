using System;
using System.Collections.Generic;
using System.Text;

namespace aud
{
    public static class DebugUt
    {

        //Application.OpenForms[0].Invoke(new Action<Form, int>((Form x, int y) => x.Text = y.ToString()), new object[] { Application.OpenForms[0], DebugUt.another() });
        static int others = 0;
        public static int another()
        {
            others++;
            return others;
        }

    }
}

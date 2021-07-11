﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using static mus.notation;

namespace mus
{
    public static partial class notation
    {

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

    }
}

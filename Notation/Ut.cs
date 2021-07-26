using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Notation
{

    public enum NamedColor
    {

        Transparent,
        AliceBlue,
        AntiqueWhite,
        Aqua,
        Aquamarine,
        Azure,
        Beige,
        Bisque,
        Black,
        BlanchedAlmond,
        Blue,
        BlueViolet,
        Brown,
        BurlyWood,
        CadetBlue,
        Chartreuse,
        Chocolate,
        Coral,
        CornflowerBlue,
        Cornsilk,
        Crimson,
        Cyan,
        DarkBlue,
        DarkCyan,
        DarkGoldenrod,
        DarkGray,
        DarkGreen,
        DarkKhaki,
        DarkMagenta,
        DarkOliveGreen,
        DarkOrange,
        DarkOrchid,
        DarkRed,
        DarkSalmon,
        DarkSeaGreen,
        DarkSlateBlue,
        DarkSlateGray,
        DarkTurquoise,
        DarkViolet,
        DeepPink,
        DeepSkyBlue,
        DimGray,
        DodgerBlue,
        Firebrick,
        FloralWhite,
        ForestGreen,
        Fuchsia,
        Gainsboro,
        GhostWhite,
        Gold,
        Goldenrod,
        Gray,
        Green,
        GreenYellow,
        Honeydew,
        HotPink,
        IndianRed,
        Indigo,
        Ivory,
        Khaki,
        Lavender,
        LavenderBlush,
        LawnGreen,
        LemonChiffon,
        LightBlue,
        LightCoral,
        LightCyan,
        LightGoldenrodYellow,
        LightGreen,
        LightGray,
        LightPink,
        LightSalmon,
        LightSeaGreen,
        LightSkyBlue,
        LightSlateGray,
        LightSteelBlue,
        LightYellow,
        Lime,
        LimeGreen,
        Linen,
        Magenta,
        Maroon,
        MediumAquamarine,
        MediumBlue,
        MediumOrchid,
        MediumPurple,
        MediumSeaGreen,
        MediumSlateBlue,
        MediumSpringGreen,
        MediumTurquoise,
        MediumVioletRed,
        MidnightBlue,
        MintCream,
        MistyRose,
        Moccasin,
        NavajoWhite,
        Navy,
        OldLace,
        Olive,
        OliveDrab,
        Orange,
        OrangeRed,
        Orchid,
        PaleGoldenrod,
        PaleGreen,
        PaleTurquoise,
        PaleVioletRed,
        PapayaWhip,
        PeachPuff,
        Peru,
        Pink,
        Plum,
        PowderBlue,
        Purple,
        Red,
        RosyBrown,
        RoyalBlue,
        SaddleBrown,
        Salmon,
        SandyBrown,
        SeaGreen,
        SeaShell,
        Sienna,
        Silver,
        SkyBlue,
        SlateBlue,
        SlateGray,
        Snow,
        SpringGreen,
        SteelBlue,
        Tan,
        Teal,
        Thistle,
        Tomato,
        Turquoise,
        Violet,
        Wheat,
        White,
        WhiteSmoke,
        Yellow,
        YellowGreen,

    }

    public static class Ut
    {

        public static Pen GetPen(NamedColor col)
        {
            switch (col)
            {
                case NamedColor.Transparent:
                    return Pens.Transparent;
                case NamedColor.AliceBlue:
                    return Pens.AliceBlue;
                case NamedColor.AntiqueWhite:
                    return Pens.AntiqueWhite;
                case NamedColor.Aqua:
                    return Pens.Aqua;
                case NamedColor.Aquamarine:
                    return Pens.Aquamarine;
                case NamedColor.Azure:
                    return Pens.Azure;
                case NamedColor.Beige:
                    return Pens.Beige;
                case NamedColor.Bisque:
                    return Pens.Bisque;
                case NamedColor.Black:
                    return Pens.Black;
                case NamedColor.BlanchedAlmond:
                    return Pens.BlanchedAlmond;
                case NamedColor.Blue:
                    return Pens.Blue;
                case NamedColor.BlueViolet:
                    return Pens.BlueViolet;
                case NamedColor.Brown:
                    return Pens.Brown;
                case NamedColor.BurlyWood:
                    return Pens.BurlyWood;
                case NamedColor.CadetBlue:
                    return Pens.CadetBlue;
                case NamedColor.Chartreuse:
                    return Pens.Chartreuse;
                case NamedColor.Chocolate:
                    return Pens.Chocolate;
                case NamedColor.Coral:
                    return Pens.Coral;
                case NamedColor.CornflowerBlue:
                    return Pens.CornflowerBlue;
                case NamedColor.Cornsilk:
                    return Pens.Cornsilk;
                case NamedColor.Crimson:
                    return Pens.Crimson;
                case NamedColor.Cyan:
                    return Pens.Cyan;
                case NamedColor.DarkBlue:
                    return Pens.DarkBlue;
                case NamedColor.DarkCyan:
                    return Pens.DarkCyan;
                case NamedColor.DarkGoldenrod:
                    return Pens.DarkGoldenrod;
                case NamedColor.DarkGray:
                    return Pens.DarkGray;
                case NamedColor.DarkGreen:
                    return Pens.DarkGreen;
                case NamedColor.DarkKhaki:
                    return Pens.DarkKhaki;
                case NamedColor.DarkMagenta:
                    return Pens.DarkMagenta;
                case NamedColor.DarkOliveGreen:
                    return Pens.DarkOliveGreen;
                case NamedColor.DarkOrange:
                    return Pens.DarkOrange;
                case NamedColor.DarkOrchid:
                    return Pens.DarkOrchid;
                case NamedColor.DarkRed:
                    return Pens.DarkRed;
                case NamedColor.DarkSalmon:
                    return Pens.DarkSalmon;
                case NamedColor.DarkSeaGreen:
                    return Pens.DarkSeaGreen;
                case NamedColor.DarkSlateBlue:
                    return Pens.DarkSlateBlue;
                case NamedColor.DarkSlateGray:
                    return Pens.DarkSlateGray;
                case NamedColor.DarkTurquoise:
                    return Pens.DarkTurquoise;
                case NamedColor.DarkViolet:
                    return Pens.DarkViolet;
                case NamedColor.DeepPink:
                    return Pens.DeepPink;
                case NamedColor.DeepSkyBlue:
                    return Pens.DeepSkyBlue;
                case NamedColor.DimGray:
                    return Pens.DimGray;
                case NamedColor.DodgerBlue:
                    return Pens.DodgerBlue;
                case NamedColor.Firebrick:
                    return Pens.Firebrick;
                case NamedColor.FloralWhite:
                    return Pens.FloralWhite;
                case NamedColor.ForestGreen:
                    return Pens.ForestGreen;
                case NamedColor.Fuchsia:
                    return Pens.Fuchsia;
                case NamedColor.Gainsboro:
                    return Pens.Gainsboro;
                case NamedColor.GhostWhite:
                    return Pens.GhostWhite;
                case NamedColor.Gold:
                    return Pens.Gold;
                case NamedColor.Goldenrod:
                    return Pens.Goldenrod;
                case NamedColor.Gray:
                    return Pens.Gray;
                case NamedColor.Green:
                    return Pens.Green;
                case NamedColor.GreenYellow:
                    return Pens.GreenYellow;
                case NamedColor.Honeydew:
                    return Pens.Honeydew;
                case NamedColor.HotPink:
                    return Pens.HotPink;
                case NamedColor.IndianRed:
                    return Pens.IndianRed;
                case NamedColor.Indigo:
                    return Pens.Indigo;
                case NamedColor.Ivory:
                    return Pens.Ivory;
                case NamedColor.Khaki:
                    return Pens.Khaki;
                case NamedColor.Lavender:
                    return Pens.Lavender;
                case NamedColor.LavenderBlush:
                    return Pens.LavenderBlush;
                case NamedColor.LawnGreen:
                    return Pens.LawnGreen;
                case NamedColor.LemonChiffon:
                    return Pens.LemonChiffon;
                case NamedColor.LightBlue:
                    return Pens.LightBlue;
                case NamedColor.LightCoral:
                    return Pens.LightCoral;
                case NamedColor.LightCyan:
                    return Pens.LightCyan;
                case NamedColor.LightGoldenrodYellow:
                    return Pens.LightGoldenrodYellow;
                case NamedColor.LightGreen:
                    return Pens.LightGreen;
                case NamedColor.LightGray:
                    return Pens.LightGray;
                case NamedColor.LightPink:
                    return Pens.LightPink;
                case NamedColor.LightSalmon:
                    return Pens.LightSalmon;
                case NamedColor.LightSeaGreen:
                    return Pens.LightSeaGreen;
                case NamedColor.LightSkyBlue:
                    return Pens.LightSkyBlue;
                case NamedColor.LightSlateGray:
                    return Pens.LightSlateGray;
                case NamedColor.LightSteelBlue:
                    return Pens.LightSteelBlue;
                case NamedColor.LightYellow:
                    return Pens.LightYellow;
                case NamedColor.Lime:
                    return Pens.Lime;
                case NamedColor.LimeGreen:
                    return Pens.LimeGreen;
                case NamedColor.Linen:
                    return Pens.Linen;
                case NamedColor.Magenta:
                    return Pens.Magenta;
                case NamedColor.Maroon:
                    return Pens.Maroon;
                case NamedColor.MediumAquamarine:
                    return Pens.MediumAquamarine;
                case NamedColor.MediumBlue:
                    return Pens.MediumBlue;
                case NamedColor.MediumOrchid:
                    return Pens.MediumOrchid;
                case NamedColor.MediumPurple:
                    return Pens.MediumPurple;
                case NamedColor.MediumSeaGreen:
                    return Pens.MediumSeaGreen;
                case NamedColor.MediumSlateBlue:
                    return Pens.MediumSlateBlue;
                case NamedColor.MediumSpringGreen:
                    return Pens.MediumSpringGreen;
                case NamedColor.MediumTurquoise:
                    return Pens.MediumTurquoise;
                case NamedColor.MediumVioletRed:
                    return Pens.MediumVioletRed;
                case NamedColor.MidnightBlue:
                    return Pens.MidnightBlue;
                case NamedColor.MintCream:
                    return Pens.MintCream;
                case NamedColor.MistyRose:
                    return Pens.MistyRose;
                case NamedColor.Moccasin:
                    return Pens.Moccasin;
                case NamedColor.NavajoWhite:
                    return Pens.NavajoWhite;
                case NamedColor.Navy:
                    return Pens.Navy;
                case NamedColor.OldLace:
                    return Pens.OldLace;
                case NamedColor.Olive:
                    return Pens.Olive;
                case NamedColor.OliveDrab:
                    return Pens.OliveDrab;
                case NamedColor.Orange:
                    return Pens.Orange;
                case NamedColor.OrangeRed:
                    return Pens.OrangeRed;
                case NamedColor.Orchid:
                    return Pens.Orchid;
                case NamedColor.PaleGoldenrod:
                    return Pens.PaleGoldenrod;
                case NamedColor.PaleGreen:
                    return Pens.PaleGreen;
                case NamedColor.PaleTurquoise:
                    return Pens.PaleTurquoise;
                case NamedColor.PaleVioletRed:
                    return Pens.PaleVioletRed;
                case NamedColor.PapayaWhip:
                    return Pens.PapayaWhip;
                case NamedColor.PeachPuff:
                    return Pens.PeachPuff;
                case NamedColor.Peru:
                    return Pens.Peru;
                case NamedColor.Pink:
                    return Pens.Pink;
                case NamedColor.Plum:
                    return Pens.Plum;
                case NamedColor.PowderBlue:
                    return Pens.PowderBlue;
                case NamedColor.Purple:
                    return Pens.Purple;
                case NamedColor.Red:
                    return Pens.Red;
                case NamedColor.RosyBrown:
                    return Pens.RosyBrown;
                case NamedColor.RoyalBlue:
                    return Pens.RoyalBlue;
                case NamedColor.SaddleBrown:
                    return Pens.SaddleBrown;
                case NamedColor.Salmon:
                    return Pens.Salmon;
                case NamedColor.SandyBrown:
                    return Pens.SandyBrown;
                case NamedColor.SeaGreen:
                    return Pens.SeaGreen;
                case NamedColor.SeaShell:
                    return Pens.SeaShell;
                case NamedColor.Sienna:
                    return Pens.Sienna;
                case NamedColor.Silver:
                    return Pens.Silver;
                case NamedColor.SkyBlue:
                    return Pens.SkyBlue;
                case NamedColor.SlateBlue:
                    return Pens.SlateBlue;
                case NamedColor.SlateGray:
                    return Pens.SlateGray;
                case NamedColor.Snow:
                    return Pens.Snow;
                case NamedColor.SpringGreen:
                    return Pens.SpringGreen;
                case NamedColor.SteelBlue:
                    return Pens.SteelBlue;
                case NamedColor.Tan:
                    return Pens.Tan;
                case NamedColor.Teal:
                    return Pens.Teal;
                case NamedColor.Thistle:
                    return Pens.Thistle;
                case NamedColor.Tomato:
                    return Pens.Tomato;
                case NamedColor.Turquoise:
                    return Pens.Turquoise;
                case NamedColor.Violet:
                    return Pens.Violet;
                case NamedColor.Wheat:
                    return Pens.Wheat;
                case NamedColor.White:
                    return Pens.White;
                case NamedColor.WhiteSmoke:
                    return Pens.WhiteSmoke;
                case NamedColor.Yellow:
                    return Pens.Yellow;
                case NamedColor.YellowGreen:
                    return Pens.YellowGreen;
                default:
                    throw new ArgumentException();
            }
        }

        public static Brush GetBrush(NamedColor col)
        {
            switch (col)
            {
                case NamedColor.Transparent:
                    return Brushes.Transparent;
                case NamedColor.AliceBlue:
                    return Brushes.AliceBlue;
                case NamedColor.AntiqueWhite:
                    return Brushes.AntiqueWhite;
                case NamedColor.Aqua:
                    return Brushes.Aqua;
                case NamedColor.Aquamarine:
                    return Brushes.Aquamarine;
                case NamedColor.Azure:
                    return Brushes.Azure;
                case NamedColor.Beige:
                    return Brushes.Beige;
                case NamedColor.Bisque:
                    return Brushes.Bisque;
                case NamedColor.Black:
                    return Brushes.Black;
                case NamedColor.BlanchedAlmond:
                    return Brushes.BlanchedAlmond;
                case NamedColor.Blue:
                    return Brushes.Blue;
                case NamedColor.BlueViolet:
                    return Brushes.BlueViolet;
                case NamedColor.Brown:
                    return Brushes.Brown;
                case NamedColor.BurlyWood:
                    return Brushes.BurlyWood;
                case NamedColor.CadetBlue:
                    return Brushes.CadetBlue;
                case NamedColor.Chartreuse:
                    return Brushes.Chartreuse;
                case NamedColor.Chocolate:
                    return Brushes.Chocolate;
                case NamedColor.Coral:
                    return Brushes.Coral;
                case NamedColor.CornflowerBlue:
                    return Brushes.CornflowerBlue;
                case NamedColor.Cornsilk:
                    return Brushes.Cornsilk;
                case NamedColor.Crimson:
                    return Brushes.Crimson;
                case NamedColor.Cyan:
                    return Brushes.Cyan;
                case NamedColor.DarkBlue:
                    return Brushes.DarkBlue;
                case NamedColor.DarkCyan:
                    return Brushes.DarkCyan;
                case NamedColor.DarkGoldenrod:
                    return Brushes.DarkGoldenrod;
                case NamedColor.DarkGray:
                    return Brushes.DarkGray;
                case NamedColor.DarkGreen:
                    return Brushes.DarkGreen;
                case NamedColor.DarkKhaki:
                    return Brushes.DarkKhaki;
                case NamedColor.DarkMagenta:
                    return Brushes.DarkMagenta;
                case NamedColor.DarkOliveGreen:
                    return Brushes.DarkOliveGreen;
                case NamedColor.DarkOrange:
                    return Brushes.DarkOrange;
                case NamedColor.DarkOrchid:
                    return Brushes.DarkOrchid;
                case NamedColor.DarkRed:
                    return Brushes.DarkRed;
                case NamedColor.DarkSalmon:
                    return Brushes.DarkSalmon;
                case NamedColor.DarkSeaGreen:
                    return Brushes.DarkSeaGreen;
                case NamedColor.DarkSlateBlue:
                    return Brushes.DarkSlateBlue;
                case NamedColor.DarkSlateGray:
                    return Brushes.DarkSlateGray;
                case NamedColor.DarkTurquoise:
                    return Brushes.DarkTurquoise;
                case NamedColor.DarkViolet:
                    return Brushes.DarkViolet;
                case NamedColor.DeepPink:
                    return Brushes.DeepPink;
                case NamedColor.DeepSkyBlue:
                    return Brushes.DeepSkyBlue;
                case NamedColor.DimGray:
                    return Brushes.DimGray;
                case NamedColor.DodgerBlue:
                    return Brushes.DodgerBlue;
                case NamedColor.Firebrick:
                    return Brushes.Firebrick;
                case NamedColor.FloralWhite:
                    return Brushes.FloralWhite;
                case NamedColor.ForestGreen:
                    return Brushes.ForestGreen;
                case NamedColor.Fuchsia:
                    return Brushes.Fuchsia;
                case NamedColor.Gainsboro:
                    return Brushes.Gainsboro;
                case NamedColor.GhostWhite:
                    return Brushes.GhostWhite;
                case NamedColor.Gold:
                    return Brushes.Gold;
                case NamedColor.Goldenrod:
                    return Brushes.Goldenrod;
                case NamedColor.Gray:
                    return Brushes.Gray;
                case NamedColor.Green:
                    return Brushes.Green;
                case NamedColor.GreenYellow:
                    return Brushes.GreenYellow;
                case NamedColor.Honeydew:
                    return Brushes.Honeydew;
                case NamedColor.HotPink:
                    return Brushes.HotPink;
                case NamedColor.IndianRed:
                    return Brushes.IndianRed;
                case NamedColor.Indigo:
                    return Brushes.Indigo;
                case NamedColor.Ivory:
                    return Brushes.Ivory;
                case NamedColor.Khaki:
                    return Brushes.Khaki;
                case NamedColor.Lavender:
                    return Brushes.Lavender;
                case NamedColor.LavenderBlush:
                    return Brushes.LavenderBlush;
                case NamedColor.LawnGreen:
                    return Brushes.LawnGreen;
                case NamedColor.LemonChiffon:
                    return Brushes.LemonChiffon;
                case NamedColor.LightBlue:
                    return Brushes.LightBlue;
                case NamedColor.LightCoral:
                    return Brushes.LightCoral;
                case NamedColor.LightCyan:
                    return Brushes.LightCyan;
                case NamedColor.LightGoldenrodYellow:
                    return Brushes.LightGoldenrodYellow;
                case NamedColor.LightGreen:
                    return Brushes.LightGreen;
                case NamedColor.LightGray:
                    return Brushes.LightGray;
                case NamedColor.LightPink:
                    return Brushes.LightPink;
                case NamedColor.LightSalmon:
                    return Brushes.LightSalmon;
                case NamedColor.LightSeaGreen:
                    return Brushes.LightSeaGreen;
                case NamedColor.LightSkyBlue:
                    return Brushes.LightSkyBlue;
                case NamedColor.LightSlateGray:
                    return Brushes.LightSlateGray;
                case NamedColor.LightSteelBlue:
                    return Brushes.LightSteelBlue;
                case NamedColor.LightYellow:
                    return Brushes.LightYellow;
                case NamedColor.Lime:
                    return Brushes.Lime;
                case NamedColor.LimeGreen:
                    return Brushes.LimeGreen;
                case NamedColor.Linen:
                    return Brushes.Linen;
                case NamedColor.Magenta:
                    return Brushes.Magenta;
                case NamedColor.Maroon:
                    return Brushes.Maroon;
                case NamedColor.MediumAquamarine:
                    return Brushes.MediumAquamarine;
                case NamedColor.MediumBlue:
                    return Brushes.MediumBlue;
                case NamedColor.MediumOrchid:
                    return Brushes.MediumOrchid;
                case NamedColor.MediumPurple:
                    return Brushes.MediumPurple;
                case NamedColor.MediumSeaGreen:
                    return Brushes.MediumSeaGreen;
                case NamedColor.MediumSlateBlue:
                    return Brushes.MediumSlateBlue;
                case NamedColor.MediumSpringGreen:
                    return Brushes.MediumSpringGreen;
                case NamedColor.MediumTurquoise:
                    return Brushes.MediumTurquoise;
                case NamedColor.MediumVioletRed:
                    return Brushes.MediumVioletRed;
                case NamedColor.MidnightBlue:
                    return Brushes.MidnightBlue;
                case NamedColor.MintCream:
                    return Brushes.MintCream;
                case NamedColor.MistyRose:
                    return Brushes.MistyRose;
                case NamedColor.Moccasin:
                    return Brushes.Moccasin;
                case NamedColor.NavajoWhite:
                    return Brushes.NavajoWhite;
                case NamedColor.Navy:
                    return Brushes.Navy;
                case NamedColor.OldLace:
                    return Brushes.OldLace;
                case NamedColor.Olive:
                    return Brushes.Olive;
                case NamedColor.OliveDrab:
                    return Brushes.OliveDrab;
                case NamedColor.Orange:
                    return Brushes.Orange;
                case NamedColor.OrangeRed:
                    return Brushes.OrangeRed;
                case NamedColor.Orchid:
                    return Brushes.Orchid;
                case NamedColor.PaleGoldenrod:
                    return Brushes.PaleGoldenrod;
                case NamedColor.PaleGreen:
                    return Brushes.PaleGreen;
                case NamedColor.PaleTurquoise:
                    return Brushes.PaleTurquoise;
                case NamedColor.PaleVioletRed:
                    return Brushes.PaleVioletRed;
                case NamedColor.PapayaWhip:
                    return Brushes.PapayaWhip;
                case NamedColor.PeachPuff:
                    return Brushes.PeachPuff;
                case NamedColor.Peru:
                    return Brushes.Peru;
                case NamedColor.Pink:
                    return Brushes.Pink;
                case NamedColor.Plum:
                    return Brushes.Plum;
                case NamedColor.PowderBlue:
                    return Brushes.PowderBlue;
                case NamedColor.Purple:
                    return Brushes.Purple;
                case NamedColor.Red:
                    return Brushes.Red;
                case NamedColor.RosyBrown:
                    return Brushes.RosyBrown;
                case NamedColor.RoyalBlue:
                    return Brushes.RoyalBlue;
                case NamedColor.SaddleBrown:
                    return Brushes.SaddleBrown;
                case NamedColor.Salmon:
                    return Brushes.Salmon;
                case NamedColor.SandyBrown:
                    return Brushes.SandyBrown;
                case NamedColor.SeaGreen:
                    return Brushes.SeaGreen;
                case NamedColor.SeaShell:
                    return Brushes.SeaShell;
                case NamedColor.Sienna:
                    return Brushes.Sienna;
                case NamedColor.Silver:
                    return Brushes.Silver;
                case NamedColor.SkyBlue:
                    return Brushes.SkyBlue;
                case NamedColor.SlateBlue:
                    return Brushes.SlateBlue;
                case NamedColor.SlateGray:
                    return Brushes.SlateGray;
                case NamedColor.Snow:
                    return Brushes.Snow;
                case NamedColor.SpringGreen:
                    return Brushes.SpringGreen;
                case NamedColor.SteelBlue:
                    return Brushes.SteelBlue;
                case NamedColor.Tan:
                    return Brushes.Tan;
                case NamedColor.Teal:
                    return Brushes.Teal;
                case NamedColor.Thistle:
                    return Brushes.Thistle;
                case NamedColor.Tomato:
                    return Brushes.Tomato;
                case NamedColor.Turquoise:
                    return Brushes.Turquoise;
                case NamedColor.Violet:
                    return Brushes.Violet;
                case NamedColor.Wheat:
                    return Brushes.Wheat;
                case NamedColor.White:
                    return Brushes.White;
                case NamedColor.WhiteSmoke:
                    return Brushes.WhiteSmoke;
                case NamedColor.Yellow:
                    return Brushes.Yellow;
                case NamedColor.YellowGreen:
                    return Brushes.YellowGreen;
                default:
                    throw new ArgumentException();
            }
        }

        public static void DrawCenteredEllipse(this Graphics g, Pen pen, Brush bsh, bool solid, PointF pos, SizeF size)
        {
            if (solid)
            {
                g.FillEllipse(bsh, new RectangleF(pos.X - size.Width / 2, pos.Y - size.Height / 2, size.Width, size.Height));
            }
            else
            {
                g.DrawEllipse(pen, new RectangleF(pos.X - size.Width / 2, pos.Y - size.Height / 2, size.Width, size.Height));
            }
        }

        public static RectangleF[] PartitionH(this RectangleF Parent, int number)
        {
            var tr = new RectangleF[number];
            for (int i = 0; i < number; i++)
            {
                var prop = (float)i / number;
                tr[i] = new RectangleF(Parent.Left + prop * Parent.Width, Parent.Top, Parent.Width / number, Parent.Height);
            }
            return tr;
        }

        public static RectangleF[] PartitionV(this RectangleF Parent, int number)
        {
            var tr = new RectangleF[number];
            for (int i = 0; i < number; i++)
            {
                var prop = (float)i / number;
                tr[i] = new RectangleF(Parent.Left, Parent.Top + prop * Parent.Height, Parent.Width, Parent.Height / number);
            }
            return tr;
        }

        public static RectangleF Translate(this RectangleF rect, PointF offset)
        {
            return new RectangleF(rect.Location + new SizeF(offset), rect.Size);
        }

    }

}

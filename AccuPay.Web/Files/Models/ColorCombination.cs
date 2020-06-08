using System.Collections.Generic;
using System.Linq;

namespace AccuPay.Web.Files.Models
{
    public class ColorCombination
    {
        private const string BLACK = "#000000";

        private const string WHITE = "#ffffff";

        public string Background { get; private set; }

        public string Foreground { get; private set; }

        public double Opacity { get; private set; }

        public static IReadOnlyList<ColorCombination> ColorCombinations()
        {
            var listColor = new List<ColorCombination>[] {
                Red(),
                Pink(),
                Purple(),
                DeepPurple(),
                Indigo(),
                Blue(),
                LightBlue(),
                Cyan(),
                Teal(),
                Green(),
                LightGreen(),
                Lime(),
                Yellow(),
                Amber(),
                Orange(),
                DeepOrange(),
                Brown(),
                Gray(),
                BlueGray(),
                BlackAndWhite(),
            };

            // Flatten the list
            var list = listColor.SelectMany(t => t).ToList();

            return list;
        }

        private static List<ColorCombination> Red()
        {
            var list = new List<ColorCombination>()
            {
                new ColorCombination { Background="#FFEBEE", Foreground=BLACK, Opacity=43 },
                new ColorCombination { Background="#FFCDD2", Foreground=BLACK, Opacity=44 },
                new ColorCombination { Background="#EF9A9A", Foreground=BLACK, Opacity=47 },
                new ColorCombination { Background="#E57373", Foreground=BLACK, Opacity=51 },
                new ColorCombination { Background="#EF5350", Foreground=WHITE, Opacity=88 },
                new ColorCombination { Background="#F44336", Foreground=WHITE, Opacity=85 },
                new ColorCombination { Background="#E53935", Foreground=WHITE, Opacity=77 },
                new ColorCombination { Background="#D32F2F", Foreground=WHITE, Opacity=68 },
                new ColorCombination { Background="#C62828", Foreground=WHITE, Opacity=63 },
                new ColorCombination { Background="#B71C1C", Foreground=WHITE, Opacity=58 },
                new ColorCombination { Background="#FF8A80", Foreground=BLACK, Opacity=48 },
                new ColorCombination { Background="#FF5252", Foreground=WHITE, Opacity=95 },
                new ColorCombination { Background="#FF1744", Foreground=WHITE, Opacity=84 },
                new ColorCombination { Background="#D50000", Foreground=WHITE, Opacity=69 },
            };

            return list;
        }

        private static List<ColorCombination> Pink()
        {
            var list = new List<ColorCombination>()
            {
                new ColorCombination { Background="#FCE4EC", Foreground=BLACK, Opacity=43 },
                new ColorCombination { Background="#F8BBD0", Foreground=BLACK, Opacity=45 },
                new ColorCombination { Background="#F48FB1", Foreground=BLACK, Opacity=48 },
                new ColorCombination { Background="#F06292", Foreground=WHITE, Opacity=98 },
                new ColorCombination { Background="#EC407A", Foreground=WHITE, Opacity=83 },
                new ColorCombination { Background="#E91E63", Foreground=WHITE, Opacity=77 },
                new ColorCombination { Background="#D81B60", Foreground=WHITE, Opacity=70 },
                new ColorCombination { Background="#C2185B", Foreground=WHITE, Opacity=63 },
                new ColorCombination { Background="#AD1457", Foreground=WHITE, Opacity=56 },
                new ColorCombination { Background="#880E4F", Foreground=WHITE, Opacity=47 },
                new ColorCombination { Background="#FF80AB", Foreground=BLACK, Opacity=48 },
                new ColorCombination { Background="#FF4081", Foreground=WHITE, Opacity=92 },
                new ColorCombination { Background="#F50057", Foreground=WHITE, Opacity=81 },
                new ColorCombination { Background="#C51162", Foreground=WHITE, Opacity=64 },
            };

            return list;
        }

        private static List<ColorCombination> Purple()
        {
            var list = new List<ColorCombination>()
            {
                new ColorCombination { Background="#F3E5F5", Foreground=BLACK, Opacity=43 },
                new ColorCombination { Background="#E1BEE7", Foreground=BLACK, Opacity=45 },
                new ColorCombination { Background="#CE93D8", Foreground=BLACK, Opacity=48 },
                new ColorCombination { Background="#BA68C8", Foreground=WHITE, Opacity=85 },
                new ColorCombination { Background="#AB47BC", Foreground=WHITE, Opacity=67 },
                new ColorCombination { Background="#9C27B0", Foreground=WHITE, Opacity=57 },
                new ColorCombination { Background="#8E24AA", Foreground=WHITE, Opacity=53 },
                new ColorCombination { Background="#7B1FA2", Foreground=WHITE, Opacity=48 },
                new ColorCombination { Background="#6A1B9A", Foreground=WHITE, Opacity=45 },
                new ColorCombination { Background="#4A148C", Foreground=WHITE, Opacity=40 },
                new ColorCombination { Background="#EA80FC", Foreground=BLACK, Opacity=48 },
                new ColorCombination { Background="#E040FB", Foreground=WHITE, Opacity=91 },
                new ColorCombination { Background="#D500F9", Foreground=WHITE, Opacity=83 },
                new ColorCombination { Background="#AA00FF", Foreground=WHITE, Opacity=72 },
            };

            return list;
        }

        private static List<ColorCombination> DeepPurple()
        {
            var list = new List<ColorCombination>()
            {
                new ColorCombination { Background="#EDE7F6", Foreground=BLACK, Opacity=43 },
                new ColorCombination { Background="#D1C4E9", Foreground=BLACK, Opacity=45 },
                new ColorCombination { Background="#B39DDB", Foreground=BLACK, Opacity=48 },
                new ColorCombination { Background="#9575CD", Foreground=WHITE, Opacity=82 },
                new ColorCombination { Background="#7E57C2", Foreground=WHITE, Opacity=62 },
                new ColorCombination { Background="#673AB7", Foreground=WHITE, Opacity=50 },
                new ColorCombination { Background="#5E35B1", Foreground=WHITE, Opacity=47 },
                new ColorCombination { Background="#512DA8", Foreground=WHITE, Opacity=44 },
                new ColorCombination { Background="#4527A0", Foreground=WHITE, Opacity=41 },
                new ColorCombination { Background="#311B92", Foreground=WHITE, Opacity=39 },
                new ColorCombination { Background="#B388FF", Foreground=BLACK, Opacity=49 },
                new ColorCombination { Background="#7C4DFF", Foreground=WHITE, Opacity=67 },
                new ColorCombination { Background="#651FFF", Foreground=WHITE, Opacity=58 },
                new ColorCombination { Background="#6200EA", Foreground=WHITE, Opacity=55 },
            };

            return list;
        }

        private static List<ColorCombination> Indigo()
        {
            var list = new List<ColorCombination>()
            {
                new ColorCombination { Background="#E8EAF6", Foreground=BLACK, Opacity=43 },
                new ColorCombination { Background="#C5CAE9", Foreground=BLACK, Opacity=45 },
                new ColorCombination { Background="#9FA8DA", Foreground=BLACK, Opacity=48 },
                new ColorCombination { Background="#7986CB", Foreground=WHITE, Opacity=87 },
                new ColorCombination { Background="#5C6BC0", Foreground=WHITE, Opacity=64 },
                new ColorCombination { Background="#3F51B5", Foreground=WHITE, Opacity=51 },
                new ColorCombination { Background="#3949AB", Foreground=WHITE, Opacity=48 },
                new ColorCombination { Background="#303F9F", Foreground=WHITE, Opacity=44 },
                new ColorCombination { Background="#283593", Foreground=WHITE, Opacity=41 },
                new ColorCombination { Background="#1A237E", Foreground=WHITE, Opacity=37 },
                new ColorCombination { Background="#8C9EFF", Foreground=BLACK, Opacity=48 },
                new ColorCombination { Background="#536DFE", Foreground=WHITE, Opacity=73 },
                new ColorCombination { Background="#3D5AFE", Foreground=WHITE, Opacity=64 },
                new ColorCombination { Background="#304FFE", Foreground=WHITE, Opacity=60 },
            };

            return list;
        }

        private static List<ColorCombination> Blue()
        {
            var list = new List<ColorCombination>()
            {
                new ColorCombination { Background="#E3F2FD", Foreground=BLACK, Opacity=43 },
                new ColorCombination { Background="#BBDEFB", Foreground=BLACK, Opacity=44 },
                new ColorCombination { Background="#90CAF9", Foreground=BLACK, Opacity=45 },
                new ColorCombination {  Background="#64B5F6", Foreground=BLACK, Opacity=47 },
                new ColorCombination { Background="#42A5F5", Foreground=BLACK, Opacity=49 },
                new ColorCombination { Background="#2196F3", Foreground=WHITE, Opacity=97 },
                new ColorCombination { Background="#1E88E5", Foreground=WHITE, Opacity=83 },
                new ColorCombination { Background="#1976D2", Foreground=WHITE, Opacity=69 },
                new ColorCombination { Background="#1565C0", Foreground=WHITE, Opacity=59 },
                new ColorCombination { Background="#0D47A1", Foreground=WHITE, Opacity=45 },
                new ColorCombination { Background="#82B1FF", Foreground=BLACK, Opacity=47 },
                new ColorCombination { Background="#448AFF", Foreground=WHITE, Opacity=91 },
                new ColorCombination { Background="#2979FF", Foreground=WHITE, Opacity=78 },
                new ColorCombination { Background="#2962FF", Foreground=WHITE, Opacity=66 },
            };

            return list;
        }

        private static List<ColorCombination> LightBlue()
        {
            var list = new List<ColorCombination>()
            {
                new ColorCombination { Background="#E1F5FE", Foreground=BLACK, Opacity=42 },
                new ColorCombination { Background="#B3E5FC", Foreground=BLACK, Opacity=44 },
                new ColorCombination { Background="#81D4FA", Foreground=BLACK, Opacity=45 },
                new ColorCombination { Background="#4FC3F7", Foreground=BLACK, Opacity=46 },
                new ColorCombination { Background="#29B6F6", Foreground=BLACK, Opacity=48 },
                new ColorCombination { Background="#03A9F4", Foreground=BLACK, Opacity=49 },
                new ColorCombination { Background="#039BE5", Foreground=WHITE, Opacity=98 },
                new ColorCombination { Background="#0288D1", Foreground=WHITE, Opacity=80 },
                new ColorCombination { Background="#0277BD", Foreground=WHITE, Opacity=67 },
                new ColorCombination { Background="#01579B", Foreground=WHITE, Opacity=50 },
                new ColorCombination { Background="#80D8FF", Foreground=BLACK, Opacity=45 },
                new ColorCombination { Background="#40C4FF", Foreground=BLACK, Opacity=46 },
                new ColorCombination { Background="#00B0FF", Foreground=BLACK, Opacity=48 },
                new ColorCombination { Background="#0091EA", Foreground=WHITE, Opacity=91 },
            };

            return list;
        }

        private static List<ColorCombination> Cyan()
        {
            var list = new List<ColorCombination>()
            {
                new ColorCombination { Background="#E0F7FA", Foreground=BLACK, Opacity=42 },
                new ColorCombination { Background="#B2EBF2", Foreground=BLACK, Opacity=43 },
                new ColorCombination { Background="#80DEEA", Foreground=BLACK, Opacity=45 },
                new ColorCombination { Background="#4DD0E1", Foreground=BLACK, Opacity=45 },
                new ColorCombination { Background="#26C6DA", Foreground=BLACK, Opacity=46 },
                new ColorCombination { Background="#00BCD4", Foreground=BLACK, Opacity=48 },
                new ColorCombination { Background="#00ACC1", Foreground=BLACK, Opacity=49 },
                new ColorCombination { Background="#0097A7", Foreground=WHITE, Opacity=87 },
                new ColorCombination { Background="#00838F", Foreground=WHITE, Opacity=70 },
                new ColorCombination { Background="#006064", Foreground=WHITE, Opacity=49 },
                new ColorCombination { Background="#84FFFF", Foreground=BLACK, Opacity=43 },
                new ColorCombination { Background="#18FFFF", Foreground=BLACK, Opacity=43 },
                new ColorCombination { Background="#00E5FF", Foreground=BLACK, Opacity=44 },
                new ColorCombination { Background="#00B8D4", Foreground=BLACK, Opacity=48 },
            };

            return list;
        }

        private static List<ColorCombination> Teal()
        {
            var list = new List<ColorCombination>()
            {
                new ColorCombination { Background="#E0F2F1", Foreground=BLACK, Opacity=43 },
                new ColorCombination { Background="#B2DFDB", Foreground=BLACK, Opacity=44 },
                new ColorCombination { Background="#80CBC4", Foreground=BLACK, Opacity=45 },
                new ColorCombination { Background="#4DB6AC", Foreground=BLACK, Opacity=48 },
                new ColorCombination { Background="#26A69A", Foreground=BLACK, Opacity=51 },
                new ColorCombination { Background="#009688", Foreground=WHITE, Opacity=84 },
                new ColorCombination { Background="#00897B", Foreground=WHITE, Opacity=73 },
                new ColorCombination { Background="#00796B", Foreground=WHITE, Opacity=62 },
                new ColorCombination { Background="#00695C", Foreground=WHITE, Opacity=53 },
                new ColorCombination { Background="#004D40", Foreground=WHITE, Opacity=41 },
                new ColorCombination { Background="#A7FFEB", Foreground=BLACK, Opacity=43 },
                new ColorCombination { Background="#64FFDA", Foreground=BLACK, Opacity=43 },
                new ColorCombination { Background="#1DE9B6", Foreground=BLACK, Opacity=45 },
                new ColorCombination { Background="#00BFA5", Foreground=BLACK, Opacity=48 },
            };

            return list;
        }

        private static List<ColorCombination> Green()
        {
            var list = new List<ColorCombination>()
            {
                new ColorCombination { Background="#E8F5E9", Foreground=BLACK, Opacity=42 },
                new ColorCombination { Background="#C8E6C9", Foreground=BLACK, Opacity=44 },
                new ColorCombination { Background="#A5D6A7", Foreground=BLACK, Opacity=45 },
                new ColorCombination { Background="#81C784", Foreground=BLACK, Opacity=46 },
                new ColorCombination { Background="#66BB6A", Foreground=BLACK, Opacity=48 },
                new ColorCombination { Background="#4CAF50", Foreground=BLACK, Opacity=50 },
                new ColorCombination { Background="#43A047", Foreground=WHITE, Opacity=91 },
                new ColorCombination { Background="#388E3C", Foreground=WHITE, Opacity=75 },
                new ColorCombination { Background="#2E7D32", Foreground=WHITE, Opacity=63 },
                new ColorCombination { Background="#1B5E20", Foreground=WHITE, Opacity=47 },
                new ColorCombination { Background="#B9F6CA", Foreground=BLACK, Opacity=43 },
                new ColorCombination { Background="#69F0AE", Foreground=BLACK, Opacity=44 },
                new ColorCombination { Background="#00E676", Foreground=BLACK, Opacity=45 },
                new ColorCombination { Background="#00C853", Foreground=BLACK, Opacity=47 },
            };

            return list;
        }

        private static List<ColorCombination> LightGreen()
        {
            var list = new List<ColorCombination>()
            {
                new ColorCombination { Background="#F1F8E9", Foreground=BLACK, Opacity=42 },
                new ColorCombination { Background="#DCEDC8", Foreground=BLACK, Opacity=43 },
                new ColorCombination { Background="#C5E1A5", Foreground=BLACK, Opacity=44 },
                new ColorCombination { Background="#AED581", Foreground=BLACK, Opacity=45 },
                new ColorCombination { Background="#9CCC65", Foreground=BLACK, Opacity=45 },
                new ColorCombination { Background="#8BC34A", Foreground=BLACK, Opacity=47 },
                new ColorCombination { Background="#7CB342", Foreground=BLACK, Opacity=48 },
                new ColorCombination { Background="#689F38", Foreground=WHITE, Opacity=95 },
                new ColorCombination { Background="#558B2F", Foreground=WHITE, Opacity=74 },
                new ColorCombination { Background="#33691E", Foreground=WHITE, Opacity=52 },
                new ColorCombination { Background="#CCFF90", Foreground=BLACK, Opacity=43 },
                new ColorCombination { Background="#B2FF59", Foreground=BLACK, Opacity=43 },
                new ColorCombination { Background="#76FF03", Foreground=BLACK, Opacity=43 },
                new ColorCombination { Background="#64DD17", Foreground=BLACK, Opacity=45 },
            };

            return list;
        }

        private static List<ColorCombination> Lime()
        {
            var list = new List<ColorCombination>()
            {
                new ColorCombination { Background="#F9FBE7", Foreground=BLACK, Opacity=42 },
                new ColorCombination { Background="#F0F4C3", Foreground=BLACK, Opacity=42 },
                new ColorCombination { Background="#E6EE9C", Foreground=BLACK, Opacity=43 },
                new ColorCombination { Background="#DCE775", Foreground=BLACK, Opacity=44 },
                new ColorCombination { Background="#D4E157", Foreground=BLACK, Opacity=44 },
                new ColorCombination { Background="#CDDC39", Foreground=BLACK, Opacity=44 },
                new ColorCombination { Background="#C0CA33", Foreground=BLACK, Opacity=45 },
                new ColorCombination { Background="#AFB42B", Foreground=BLACK, Opacity=48 },
                new ColorCombination { Background="#9E9D24", Foreground=BLACK, Opacity=51 },
                new ColorCombination { Background="#827717", Foreground=WHITE, Opacity=68 },
                new ColorCombination { Background="#F4FF81", Foreground=BLACK, Opacity=42 },
                new ColorCombination { Background="#EEFF41", Foreground=BLACK, Opacity=42 },
                new ColorCombination { Background="#C6FF00", Foreground=BLACK, Opacity=43 },
                new ColorCombination { Background="#AEEA00", Foreground=BLACK, Opacity=44 },
            };

            return list;
        }

        private static List<ColorCombination> Yellow()
        {
            var list = new List<ColorCombination>()
            {
                new ColorCombination { Background="#FFFDE7", Foreground=BLACK, Opacity=42 },
                new ColorCombination { Background="#FFF9C4", Foreground=BLACK, Opacity=42 },
                new ColorCombination { Background="#FFF59D", Foreground=BLACK, Opacity=42 },
                new ColorCombination { Background="#FFF176", Foreground=BLACK, Opacity=43 },
                new ColorCombination { Background="#FFEE58", Foreground=BLACK, Opacity=43 },
                new ColorCombination { Background="#FFEB3B", Foreground=BLACK, Opacity=43 },
                new ColorCombination { Background="#FDD835", Foreground=BLACK, Opacity=44 },
                new ColorCombination { Background="#FBC02D", Foreground=BLACK, Opacity=45 },
                new ColorCombination { Background="#F9A825", Foreground=BLACK, Opacity=46 },
                new ColorCombination { Background="#F57F17", Foreground=BLACK, Opacity=49 },
                new ColorCombination { Background="#FFFF8D", Foreground=BLACK, Opacity=42 },
                new ColorCombination { Background="#FFFF00", Foreground=BLACK, Opacity=42 },
                new ColorCombination { Background="#FFEA00", Foreground=BLACK, Opacity=43 },
                new ColorCombination { Background="#FFD600", Foreground=BLACK, Opacity=44 },
            };

            return list;
        }

        private static List<ColorCombination> Amber()
        {
            var list = new List<ColorCombination>()
            {
                new ColorCombination { Background="#FFF8E1", Foreground=BLACK, Opacity=42 },
                new ColorCombination { Background="#FFECB3", Foreground=BLACK, Opacity=43 },
                new ColorCombination { Background="#FFE082", Foreground=BLACK, Opacity=43 },
                new ColorCombination { Background="#FFD54F", Foreground=BLACK, Opacity=44 },
                new ColorCombination { Background="#FFCA28", Foreground=BLACK, Opacity=45 },
                new ColorCombination { Background="#FFC107", Foreground=BLACK, Opacity=45 },
                new ColorCombination { Background="#FFB300", Foreground=BLACK, Opacity=45 },
                new ColorCombination { Background="#FFA000", Foreground=BLACK, Opacity=46 },
                new ColorCombination { Background="#FF8F00", Foreground=BLACK, Opacity=48 },
                new ColorCombination { Background="#FF6F00", Foreground=BLACK, Opacity=50 },
                new ColorCombination { Background="#FFE57F", Foreground=BLACK, Opacity=43 },
                new ColorCombination { Background="#FFD740", Foreground=BLACK, Opacity=44 },
                new ColorCombination { Background="#FFC400", Foreground=BLACK, Opacity=45 },
                new ColorCombination { Background="#FFAB00", Foreground=BLACK, Opacity=45 },
            };

            return list;
        }

        private static List<ColorCombination> Orange()
        {
            var list = new List<ColorCombination>()
            {
                new ColorCombination { Background="#FFF3E0", Foreground=BLACK, Opacity=42 },
                new ColorCombination { Background="#FFE0B2", Foreground=BLACK, Opacity=43 },
                new ColorCombination { Background="#FFCC80", Foreground=BLACK, Opacity=44 },
                new ColorCombination { Background="#FFB74D", Foreground=BLACK, Opacity=45 },
                new ColorCombination { Background="#FFA726", Foreground=BLACK, Opacity=46 },
                new ColorCombination { Background="#FF9800", Foreground=BLACK, Opacity=47 },
                new ColorCombination { Background="#FB8C00", Foreground=BLACK, Opacity=48 },
                new ColorCombination { Background="#F57C00", Foreground=BLACK, Opacity=49 },
                new ColorCombination { Background="#EF6C00", Foreground=WHITE, Opacity=98 },
                new ColorCombination { Background="#E65100", Foreground=WHITE, Opacity=82 },
                new ColorCombination { Background="#FFD180", Foreground=BLACK, Opacity=44 },
                new ColorCombination { Background="#FFAB40", Foreground=BLACK, Opacity=45 },
                new ColorCombination { Background="#FF9100", Foreground=BLACK, Opacity=47 },
                new ColorCombination { Background="#FF6D00", Foreground=BLACK, Opacity=50 },
            };

            return list;
        }

        private static List<ColorCombination> DeepOrange()
        {
            var list = new List<ColorCombination>()
            {
                new ColorCombination { Background="#FBE9E7", Foreground=BLACK, Opacity=43 },
                new ColorCombination { Background="#FFCCBC", Foreground=BLACK, Opacity=44 },
                new ColorCombination { Background="#FFAB91", Foreground=BLACK, Opacity=45 },
                new ColorCombination { Background="#FF8A65", Foreground=BLACK, Opacity=48 },
                new ColorCombination { Background="#FF7043", Foreground=BLACK, Opacity=49 },
                new ColorCombination { Background="#FF5722", Foreground=WHITE, Opacity=96 },
                new ColorCombination { Background="#F4511E", Foreground=WHITE, Opacity=88 },
                new ColorCombination { Background="#E64A19", Foreground=WHITE, Opacity=80 },
                new ColorCombination { Background="#D84315", Foreground=WHITE, Opacity=73 },
                new ColorCombination { Background="#BF360C", Foreground=WHITE, Opacity=62 },
                new ColorCombination { Background="#FF9E80", Foreground=BLACK, Opacity=46 },
                new ColorCombination { Background="#FF6E40", Foreground=BLACK, Opacity=50 },
                new ColorCombination { Background="#FF3D00", Foreground=WHITE, Opacity=88 },
                new ColorCombination { Background="#DD2C00", Foreground=WHITE, Opacity=72 },
            };

            return list;
        }

        private static List<ColorCombination> Brown()
        {
            var list = new List<ColorCombination>()
            {
                new ColorCombination { Background="#EFEBE9", Foreground=BLACK, Opacity=43 },
                new ColorCombination { Background="#D7CCC8", Foreground=BLACK, Opacity=45 },
                new ColorCombination { Background="#BCAAA4", Foreground=BLACK, Opacity=48 },
                new ColorCombination { Background="#A1887F", Foreground=WHITE, Opacity=91 },
                new ColorCombination { Background="#8D6E63", Foreground=WHITE, Opacity=66 },
                new ColorCombination { Background="#795548", Foreground=WHITE, Opacity=52 },
                new ColorCombination { Background="#6D4C41", Foreground=WHITE, Opacity=47 },
                new ColorCombination { Background="#5D4037", Foreground=WHITE, Opacity=41 },
                new ColorCombination { Background="#4E342E", Foreground=WHITE, Opacity=38 },
                new ColorCombination { Background="#3E2723", Foreground=WHITE, Opacity=35 },
            };

            return list;
        }

        private static List<ColorCombination> Gray()
        {
            var list = new List<ColorCombination>()
            {
                new ColorCombination { Background="#FAFAFA", Foreground=BLACK, Opacity=42 },
                new ColorCombination { Background="#F5F5F5", Foreground=BLACK, Opacity=42 },
                new ColorCombination { Background="#EEEEEE", Foreground=BLACK, Opacity=43 },
                new ColorCombination { Background="#E0E0E0", Foreground=BLACK, Opacity=43 },
                new ColorCombination { Background="#BDBDBD", Foreground=BLACK, Opacity=46 },
                new ColorCombination { Background="#9E9E9E", Foreground=BLACK, Opacity=50 },
                new ColorCombination { Background="#757575", Foreground=WHITE, Opacity=66 },
                new ColorCombination { Background="#616161", Foreground=WHITE, Opacity=53 },
                new ColorCombination { Background="#424242", Foreground=WHITE, Opacity=40 },
                new ColorCombination { Background="#212121", Foreground=WHITE, Opacity=34 },
            };

            return list;
        }

        private static List<ColorCombination> BlueGray()
        {
            var list = new List<ColorCombination>()
            {
                new ColorCombination { Background="#ECEFF1", Foreground=BLACK, Opacity=43 },
                new ColorCombination { Background="#CFD8DC", Foreground=BLACK, Opacity=44 },
                new ColorCombination { Background="#B0BEC5", Foreground=BLACK, Opacity=46 },
                new ColorCombination { Background="#90A4AE", Foreground=BLACK, Opacity=49 },
                new ColorCombination { Background="#78909C", Foreground=WHITE, Opacity=89 },
                new ColorCombination { Background="#607D8B", Foreground=WHITE, Opacity=70 },
                new ColorCombination { Background="#546E7A", Foreground=WHITE, Opacity=59 },
                new ColorCombination { Background="#455A64", Foreground=WHITE, Opacity=48 },
                new ColorCombination { Background="#37474F", Foreground=WHITE, Opacity=41 },
                new ColorCombination { Background="#263238", Foreground=WHITE, Opacity=35 },
            };

            return list;
        }

        private static List<ColorCombination> BlackAndWhite()
        {
            var list = new List<ColorCombination>()
            {
                new ColorCombination { Background=BLACK, Foreground=WHITE },
                new ColorCombination { Background=WHITE, Foreground=BLACK },
            };

            return list;
        }
    }
}

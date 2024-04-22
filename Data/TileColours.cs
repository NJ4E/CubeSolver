using CubeSolver.Models;

namespace CubeSolver.Data
{
    public static class TileColours
    {
        public static ColourPalette Red = new() { ColourName = "Red", Red = 185, Blue = 0, Green = 0, Color = new System.Windows.Media.Color { R = 185, B = 0, G = 0, A = 255 } };
        public static ColourPalette Green = new() { ColourName = "Green", Red = 0, Blue = 72, Green = 155, Color = new System.Windows.Media.Color { R = 0, B = 72, G = 155, A = 255 } };
        public static ColourPalette Blue = new() { ColourName = "Blue", Red = 0, Blue = 173, Green = 69, Color = new System.Windows.Media.Color { R = 0, B = 173, G = 69, A = 255 } };
        public static ColourPalette Yellow = new() { ColourName = "Yellow", Red = 255, Blue = 0, Green = 213, Color = new System.Windows.Media.Color { R = 255, B = 0, G = 213, A = 255 } };
        public static ColourPalette Orange = new() { ColourName = "Orange", Red = 255, Blue = 0, Green = 89, Color = new System.Windows.Media.Color { R = 255, B = 0, G = 89, A = 255 } };
        public static ColourPalette White = new() { ColourName = "White", Red = 255, Blue = 255, Green = 255, Color = new System.Windows.Media.Color { R = 255, B = 255, G = 255, A = 255 } };
        public static ColourPalette Purple = new() { ColourName = "Purple", Red = 163, Green = 55, Blue = 230, Color = new System.Windows.Media.Color { R = 163, G = 55, B = 230, A = 255 } };

        public static List<ColourPalette> TileColourList =
        [
            TileColours.Blue,
            TileColours.Yellow,
            TileColours.White,
            TileColours.Red,
            TileColours.Orange,
            TileColours.Green,
            TileColours.Purple,
        ];
    }
}
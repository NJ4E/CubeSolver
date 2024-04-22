using CubeSolver.Data;
using CubeSolver.Models;
using System.Windows.Media;

namespace CubeSolver.ColourLogic
{
    public class GetTileColour
    {
        public static Color GetTile(int red, int green, int blue)
        {
            if (red > 150 && green > 150 && blue > 150)
            {
                return TileColours.White.Color;
            }

            if (red > 180 && green > 180)
            {
                return TileColours.Yellow.Color;
            }

            if (red > 180 && green < 120)
            {
                return TileColours.Orange.Color;
            }

            if (red > 140 && green < 120 && blue < 100)
            {
                return TileColours.Red.Color;
            }

            if (blue > green && blue > red)
            {
                return TileColours.Blue.Color;
            }

            if (green > blue && green > 2 * red)
            {
                return TileColours.Green.Color;
            }

            List<ColourMatch> colours = [];
            foreach (ColourPalette colour in TileColours.TileColourList)
            {
                int total = colour.Green - green + colour.Blue - blue + colour.Red - red;

                ColourMatch colourMatch = new()
                {
                    ColourName = colour.ColourName,
                    ColourValue = total
                };
                colours.Add(colourMatch);
            }

            var closest = colours.OrderBy(x => Math.Abs((long)x.ColourValue - 0)).First();

            var foundColour = TileColours.TileColourList.SingleOrDefault(x => x.ColourName == closest.ColourName)?.Color;
            if (foundColour != null)
            {
                return (Color)foundColour;
            }

            return new Color() { R = 163, G = 55, B = 230, A = 255 };
        }

        public static Brush GetBrush(Color color)
        {
            return new SolidColorBrush(color);
        }
    }
}
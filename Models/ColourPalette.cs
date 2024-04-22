using System.Windows.Media;

namespace CubeSolver.Models
{
    public class ColourPalette : IColourPalette
    {
        public string ColourName { get; set; } = string.Empty;
        public int Red { get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }
        public Color Color { get; set; }
    }
}
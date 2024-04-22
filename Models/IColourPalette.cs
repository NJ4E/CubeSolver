using System.Windows.Media;

namespace CubeSolver.Models
{
    public interface IColourPalette
    {
        string ColourName { get; set; }
        int Red { get; set; }
        int Green { get; set; }
        int Blue { get; set; }
        Color Color { get; set; }
    }
}
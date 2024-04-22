namespace CubeSolver.Models
{
    public class CubeFace
    {
        //This is a class for the description of a face with the faceColour being determined by the FaceId which maps to the
        //central colour this also means that the colour type of the central tile is always the same.
        //Central Colour Id's  White = 1, Yellow = 2, Blue = 3, Orange = 4, Green = 5, Red = 6

        public int FaceId { get; set; }

        public string FaceColour
        {
            get
            {
                string faceColour = FaceId switch
                {
                    1 => "White",
                    2 => "Yellow",
                    3 => "Blue",
                    4 => "Orange",
                    5 => "Green",
                    6 => "Red",
                    _ => "White"
                };
                return faceColour;
            }
        }

        public string FaceName { get; set; } = string.Empty;

        public ColourTypes TopLeftColour { get; set; }
        public ColourTypes TopMiddleColour { get; set; }
        public ColourTypes TopRightColour { get; set; }
        public ColourTypes MiddleLeftColour { get; set; }
        public ColourTypes MiddleMiddleColour
        { get { return (ColourTypes)Enum.Parse(typeof(ColourTypes), FaceColour); } }
        public ColourTypes MiddleRightColour { get; set; }
        public ColourTypes BottomLeftColour { get; set; }
        public ColourTypes BottomMiddleColour { get; set; }
        public ColourTypes BottomRightColour { get; set; }
    }
}
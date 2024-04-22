namespace CubeSolver.Models
{
    public static class CheckColours
    {
        private static int Red;
        private static int Green;
        private static int Blue;
        private static int Yellow;
        private static int White;
        private static int Orange;

        public static bool CheckFaces(List<CubeFace> faces)
        {
            Red = 0;
            Green = 0;
            Blue = 0;
            Yellow = 0;
            White = 0;
            Orange = 0;

            foreach (CubeFace face in faces)
            {
                ColourMatch(face.TopLeftColour);
                ColourMatch(face.TopMiddleColour);
                ColourMatch(face.TopRightColour);
                ColourMatch(face.MiddleLeftColour);
                ColourMatch(face.MiddleRightColour);
                ColourMatch(face.BottomLeftColour);
                ColourMatch(face.BottomMiddleColour);
                ColourMatch(face.BottomRightColour);
            }
            if (White == 8 && Yellow == 8 && Blue == 8 && Red == 8 && Green == 8 && Orange == 8)
            {
                return true;
            }
            return false;
        }

        public static void ColourMatch(ColourTypes colours)
        {
            if (colours == ColourTypes.White)
            {
                White++;
            }
            if (colours == ColourTypes.Green)
            {
                Green++;
            }
            if (colours == ColourTypes.Blue)
            {
                Blue++;
            }
            if (colours == ColourTypes.Red)
            {
                Red++;
            }
            if (colours == ColourTypes.Yellow)
            {
                Yellow++;
            }
            if (colours == ColourTypes.Orange)
            {
                Orange++;
            }
        }
    }
}
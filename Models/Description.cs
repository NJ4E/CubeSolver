namespace CubeSolver.Models
{
    public static class Description
    {
        //Using the new style of switch this static class will generate the instruction based
        //on the colour of the face, the direction (clockwise or anti-clockwise) and move type.
        //In one simple sentence to make the following of instructions as easy as possible.

        public static string MoveDescription(string face, string top, bool clockwise, string move , string twice)
        {
            string direction;
            if (clockwise == true)
            {
                direction = move.Split(" ")[0] switch
                {
                    "top" => "part of the cube to the left",
                    "bottom" => "part of the cube to the left",
                    "right" => "side of the cube away from you",
                    "left" => "side of the cube towards you",
                    "face" => "clockwise",
                    "back" => "clockwise",
                    _ => "clockwise",
                };
            }
            else
            {
                direction = move.Split(" ")[0] switch
                {
                    "top" => "part of the cube to the right",
                    "bottom" => "part of the cube to the right",
                    "right" => "side of the cube towards you",
                    "left" => "side of the cube away from you",
                    "face" => "anti-clockwise",
                    "back" => "anti-clockwise",
                    _ => "anti-clockwise",
                };
            }

            string describe = $"With {face} middle tile facing you and {top} middle tile on top, turn the {move} {direction}{twice}.";
            return describe;
        }
    }
}
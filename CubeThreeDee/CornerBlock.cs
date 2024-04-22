namespace CubeSolver.CubeThreeDee
{
    public class CornerBlock
    {
        public int FaceNoOne;
        public int FaceNoTwo;
        public int FaceNoThree;
        public int FaceColorOne;
        public int FaceColorTwo;
        public int FaceColorThree;

        public CornerBlock(int FaceNoOne, int FaceNoTwo, int FaceNoThree)
        {
            this.FaceNoOne = FaceNoOne;
            this.FaceNoTwo = FaceNoTwo;
            this.FaceNoThree = FaceNoThree;
            FaceColorOne = FaceNoOne / Cube.FaceNoToColor;
            FaceColorTwo = FaceNoTwo / Cube.FaceNoToColor;
            FaceColorThree = FaceNoThree / Cube.FaceNoToColor;
            return;
        }
    }
}
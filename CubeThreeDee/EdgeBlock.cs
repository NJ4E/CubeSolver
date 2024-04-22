namespace CubeSolver.CubeThreeDee
{
    public class EdgeBlock
    {
        public int FaceNoOne;

        public int FaceNoTwo;

        public int FaceColorOne;

        public int FaceColorTwo;

        public EdgeBlock
                (
                int FaceNoOne,
                int FaceNoTwo
                )
        {
            this.FaceNoOne = FaceNoOne;
            this.FaceNoTwo = FaceNoTwo;
            FaceColorOne = FaceNoOne / Cube.FaceNoToColor;
            FaceColorTwo = FaceNoTwo / Cube.FaceNoToColor;
            return;
        }
    }
}
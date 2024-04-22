namespace CubeSolver.CubeThreeDee
{
    public class StepCtrl
    {
        public int UpFaceColor;
        public int[][] StepsArray;

        public int[] Steps(int Index) // 0=Blue, 1=Red, 2=Green, 3=Orange)
        {
            return StepsArray[Index];
        }

        public StepCtrl(int UpFaceColor, params int[] RelativeSteps)
        {
            this.UpFaceColor = UpFaceColor;
            int UpDown = UpFaceColor == Cube.WhiteFace ? 0 : 4;
            StepsArray = new int[4][];
            for (int Index = 0; Index < 4; Index++)
            {
                int[] Xlate = Cube.RelativeToColor[UpDown + Index];
                StepsArray[Index] = new int[RelativeSteps.Length];
                for (int Ptr = 0; Ptr < RelativeSteps.Length; Ptr++)
                {
                    int Step = RelativeSteps[Ptr];
                    StepsArray[Index][Ptr] = 3 * Xlate[Step / 3] + (Step % 3);
                }
            }
            return;
        }
    }
}
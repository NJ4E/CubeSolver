namespace CubeSolver.CubeThreeDee
{
    public class MidLayer
    {
        public bool MoveToYellow;
        public int Rotation;
        public int YellowRotation;

        private readonly int FaceNo;
        private readonly int FacePos;
        private int FrontFace;
        private StepCtrl StepCtrl;
        private int[] Steps;

        public static MidLayer Create(int[] FaceArray, int FaceNo)
        {
            return FaceArray[FaceNo] == FaceNo ? null : new MidLayer(FaceArray, FaceNo);
        }

        private MidLayer(int[] FaceArray, int FaceNo)
        {
            this.FaceNo = FaceNo;
            FacePos = Cube.FindEdge(FaceArray, FaceNo);

            if (Cube.FaceNoToBlockNo[FacePos] < 18)
            {
                MoveToYellow = true;
                return;
            }

            int FaceNoColor = FaceNo / Cube.FaceNoToColor;
            if (FacePos / Cube.FaceNoToColor == Cube.YellowFace)
            {
                FrontFace = (FaceNoColor % 4) + 1;
                StepCtrl = Cube.MidLayerLeft;
                Steps = StepCtrl.Steps(FrontFace - 1);
                Rotation = (33 - 4 * (FacePos - 41)) / 8 - FrontFace + 4;
            }
            else
            {
                FrontFace = FaceNoColor;
                StepCtrl = Cube.MidLayerRight;
                Steps = StepCtrl.Steps(FaceNoColor - 1);
                Rotation = FacePos / 8 - FaceNoColor + 4;
            }

            YellowRotation = (Steps[0] - Cube.YellowCW + 1 + Rotation) % 4;
            return;
        }

        public SolutionStep CreateSolutionStepOne(string Message)
        {
            int Len = this.Steps.Length - 1;
            int[] TempSteps = new int[Len];
            Array.Copy(Steps, 1, TempSteps, 0, Len);
            return new SolutionStep(StepCode.MidLayer, Message, FaceNo, Cube.YellowFace, FrontFace, TempSteps);
        }

        public SolutionStep CreateSolutionStepTwo(string Message)
        {
            return new SolutionStep(StepCode.MidLayer, Message, FaceNo, Cube.YellowFace, FrontFace, Steps);
        }

        public SolutionStep CreateSolutionStepThree(string Message)
        {
            int Len = Steps.Length;
            int[] TempSteps = new int[Len];
            Array.Copy(Steps, 0, TempSteps, 0, Len);
            TempSteps[0] = Cube.YellowCW + YellowRotation - 1;
            return new SolutionStep(StepCode.MidLayer, Message, FaceNo, Cube.YellowFace, FrontFace, TempSteps);
        }

        public SolutionStep CreateSolutionStepFour(string Message)
        {
            FrontFace = FacePos switch
            {
                11 or 23 => 1,
                19 or 31 => 2,
                27 or 39 => 3,
                35 or 15 => 4,
                _ => throw new Exception("Mid layer FacePos"),
            };

            StepCtrl = Cube.MidLayerRight;
            Steps = StepCtrl.Steps(FrontFace - 1);
            int Len = Steps.Length - 1;
            int[] TempSteps = new int[Len];
            Array.Copy(Steps, 1, TempSteps, 0, Len);
            return new SolutionStep(StepCode.MidLayer, Message, FacePos, Cube.YellowFace, FrontFace, TempSteps);
        }
    }
}
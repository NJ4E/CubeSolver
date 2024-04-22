namespace CubeSolver.CubeThreeDee
{
    public class WhiteCorner
    {
        public bool MoveToYellow;
        public int YellowRotation;

        private readonly int FaceNo;
        private int FacePos;

        public static WhiteCorner Create(int[] FaceArray, int FaceNo)
        {
            return FaceArray[FaceNo] == FaceNo ? null : new WhiteCorner(FaceArray, FaceNo);
        }

        private WhiteCorner(int[] FaceArray, int FaceNo)
        {
            this.FaceNo = FaceNo;
            FacePos = Cube.FindCorner(FaceArray, FaceNo);
            int BlockNo = Cube.FaceNoToBlockNo[FacePos];
            if (BlockNo < 9)
            {
                MoveToYellow = true;
                return;
            }
            YellowRotation = (46 - Cube.BlockFace[BlockNo, Cube.YellowFace] - FaceNo) / 2;
            if (YellowRotation < 0) YellowRotation += 4;
            return;
        }

        public SolutionStep CreateSolutionStep(string Message)
        {
            if (MoveToYellow)
            {
                FacePos = 0;
                switch (FaceNo)
                {
                    case 0:
                        FacePos = 16;
                        break;

                    case 2:
                        FacePos = 24;
                        break;

                    case 4:
                        FacePos = 32;
                        break;

                    case 6:
                        FacePos = 8;
                        break;
                }
            }
            else if (YellowRotation != 0) FacePos = Cube.RotMatrix[Cube.YellowCW + YellowRotation - 1][FacePos];

            int CtrlIndex = Cube.WhiteCornerIndex[FacePos / 2];
            int Case = CtrlIndex / 4;
            int StepsIndex = CtrlIndex % 4;
            int FrontFace = StepsIndex + 1;
            StepCtrl StepCtrl = Cube.WhiteCornerCases[Case];
            int[] Steps = StepCtrl.Steps(StepsIndex);

            if (!MoveToYellow)
            {
                if (YellowRotation == 0)
                    return new SolutionStep(StepCode.WhiteCorners, Message, FaceNo, Cube.WhiteFace, FrontFace, Steps);

                int Len = Steps.Length;
                int[] TempSteps = new int[Len + 1];
                Array.Copy(Steps, 0, TempSteps, 1, Len);
                TempSteps[0] = Cube.YellowCW + YellowRotation - 1;

                return new SolutionStep(StepCode.WhiteCorners, Message, FaceNo, Cube.WhiteFace, FrontFace, TempSteps);
            }
            return new SolutionStep(StepCode.WhiteCorners, Message, FaceNo, Cube.WhiteFace, FrontFace, Steps);
        }
    }
}
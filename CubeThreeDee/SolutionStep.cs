namespace CubeSolver.CubeThreeDee
{
    public class SolutionStep
    {
        public StepCode StepCode;
        public string Message;
        public int FaceNo;
        public int UpFaceColor;
        public int FrontFaceColor;
        public int[] Steps;

        public SolutionStep(StepCode StepCode, string Message, int FaceNo, int UpFaceColor, int FrontFaceColor, int[] Steps)
        {
            this.StepCode = StepCode;
            this.Message = Message;
            this.FaceNo = FaceNo;
            this.UpFaceColor = UpFaceColor;
            this.FrontFaceColor = FrontFaceColor;
            this.Steps = Steps;
            return;
        }

        public SolutionStep()
        {
            StepCode = StepCode.CubeIsSolved;
            return;
        }
    }
}
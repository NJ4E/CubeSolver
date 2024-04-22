using System.Text;

namespace CubeSolver.CubeThreeDee
{
    public enum StepCode
    {
        WhiteEdges,
        WhiteCorners,
        MidLayer,
        YellowCross,
        YellowCornersPos,
        YellowCorners,
        YellowEdges,
        CubeIsSolved,
    }

    public class Cube
    {
        public const int BlocksPerCube = 27;
        public const int BlocksPerFace = 9;
        public const int FaceNoToColor = 8;
        public const int MovableFaces = 48;

        public const int WhiteFace = 0;
        public const int BlueFace = 1;
        public const int RedFace = 2;
        public const int GreenFace = 3;
        public const int OrangeFace = 4;
        public const int YellowFace = 5;
        public const int FaceColors = 6;

        public const int UpCW = 0;
        public const int UpCW2 = 1;
        public const int UpCCW = 2;
        public const int FrontCW = 3;
        public const int FrontCW2 = 4;
        public const int FrontCCW = 5;
        public const int RightCW = 6;
        public const int RightCW2 = 7;
        public const int RightCCW = 8;
        public const int BackCW = 9;
        public const int BackCW2 = 10;
        public const int BackCCW = 11;
        public const int LeftCW = 12;
        public const int LeftCW2 = 13;
        public const int LeftCCW = 14;
        public const int DownCW = 15;
        public const int DownCW2 = 16;
        public const int DownCCW = 17;

        public const int WhiteCW = 0;
        public const int WhiteCW2 = 1;
        public const int WhiteCCW = 2;
        public const int BlueCW = 3;
        public const int BlueCW2 = 4;
        public const int BlueCCW = 5;
        public const int RedCW = 6;
        public const int RedCW2 = 7;
        public const int RedCCW = 8;
        public const int GreenCW = 9;
        public const int GreenCW2 = 10;
        public const int GreenCCW = 11;
        public const int OrangeCW = 12;
        public const int OrangeCW2 = 13;
        public const int OrangeCCW = 14;
        public const int YellowCW = 15;
        public const int YellowCW2 = 16;
        public const int YellowCCW = 17;
        public const int RotationCodes = 18;
        public const int RotMovesPerColor = 3;

        public static readonly string[] FaceColorName =
            [
            "White",
            "Blue",
            "Red",
            "Green",
            "Orange",
            "Yellow",
            "Black"
            ];

        public static readonly string[] SaveSolutionHeader =
            [
            "White  Red",
            "Blue   Yellow",
            "Red    Yellow",
            "Green  Yellow",
            "Orange Yellow",
            "Yellow Orange",
            ];

        public static readonly string[] RelativeRotationName =
            [
            "U",
            "U2",
            "U'",

            "F",
            "F2",
            "F'",

            "R",
            "R2",
            "R'",

            "B",
            "B2",
            "B'",

            "L",
            "L2",
            "L'",

            "D",
            "D2",
            "D'",
            ];

        public static readonly string[] ColorRotationName =
            [
            "W",
            "W2",
            "W'",

            "B",
            "B2",
            "B'",

            "R",
            "R2",
            "R'",

            "G",
            "G2",
            "G'",

            "O",
            "O2",
            "O'",

            "Y",
            "Y2",
            "Y'",
            ];

        public static readonly int[,] BlockFace = new int[,]
            {
			// W   B   R   G   O   Y            Z   Y   X
			{  0, 12, 22, -1, -1, -1}, // 0		0	0	0	W0	B4	R6	--	--	--
			{  1, -1, 21, -1, -1, -1}, // 1		0	0	1	W1	--	R5	--	--	--
			{  2, -1, 20, 30, -1, -1}, // 2		0	0	2	W2	--	R4	G6	--	--
			{  7, 13, -1, -1, -1, -1}, // 3		0	1	0	W7	B5	--	--	--	--
			{ 48, -1, -1, -1, -1, -1}, // 4		0	1	1	White
			{  3, -1, -1, 29, -1, -1}, // 5		0	1	2	W3	--	--	G5	--	--
			{  6, 14, -1, -1, 36, -1}, // 6		0	2	0	W6	B6	--	--	O4	--
			{  5, -1, -1, -1, 37, -1}, // 7		0	2	1	W5	--	--	--	O5	--
			{  4, -1, -1, 28, 38, -1}, // 8		0	2	2	W4	--	--	G4	O6	--
			{ -1, 11, 23, -1, -1, -1}, // 9		1	0	0	--	B3	R7	--	--	--
			{ -1, -1, 50, -1, -1, -1}, // 10	1	0	1	--	--	Red	--	--	--
			{ -1, -1, 19, 31, -1, -1}, // 11	1	0	2	--	--	R3	G7	--	--
			{ -1, 49, -1, -1, -1, -1}, // 12	1	1	0	--	Blue--	--	--	--
			{ -1, -1, -1, -1, -1, -1}, // 13	1	1	1
			{ -1, -1, -1, 51, -1, -1}, // 14	1	1	2	--	--	--	Green-	--
			{ -1, 15, -1, -1, 35, -1}, // 15	1	2	0	--	B7	--	--	O3	--
			{ -1, -1, -1, -1, 52, -1}, // 16	1	2	1	--	--	--	--	Orange
			{ -1, -1, -1, 27, 39, -1}, // 17	1	2	2	--	--	--	G3	O7	--
			{ -1, 10, 16, -1, -1, 46}, // 18	2	0	0	--	B2	R0	--	--	Y7
			{ -1, -1, 17, -1, -1, 45}, // 19	2	0	1	--	--	R1	--	--	Y6
			{ -1, -1, 18, 24, -1, 44}, // 29	2	0	2	--	--	R2	G0	--	Y4
			{ -1,  9, -1, -1, -1, 47}, // 21	2	1	0	--	B1	--	--	--	Y8
			{ -1, -1, -1, -1, -1, 53}, // 22	2	1	1	--	--	--	--	--	Yellow
			{ -1, -1, -1, 25, -1, 43}, // 23	2	1	2	--	--	--	G1	--	Y3
			{ -1,  8, -1, -1, 34, 40}, // 24	2	2	0	--	B0	--	--	O2	Y0
			{ -1, -1, -1, -1, 33, 41}, // 25	2	2	1	--	--	--	--	O1	Y1
			{ -1, -1, -1, 26, 32, 42}, // 26	2	2	2				G2	O0	Y2
			};

        public static readonly int[][] RotMatrix =
            [
            [2,3,4,5,6,7,0,1,8,9,10,11,20,21,22,15,16,17,18,19,28,29,30,23,24,25,26,27,36,37,38,31,32,33,34,35,12,13,14,39,40,41,42,43,44,45,46,47],
            null,
            null,
            [36,1,2,3,4,5,34,35,10,11,12,13,14,15,8,9,0,17,18,19,20,21,6,7,24,25,26,27,28,29,30,31,32,33,46,47,40,37,38,39,16,41,42,43,44,45,22,23],
            null,
            null,
            [10,11,12,3,4,5,6,7,8,9,44,45,46,13,14,15,18,19,20,21,22,23,16,17,2,25,26,27,28,29,0,1,32,33,34,35,36,37,38,39,40,41,42,43,30,31,24,47],
            null,
            null,
            [0,1,18,19,20,5,6,7,8,9,10,11,12,13,14,15,16,17,42,43,44,21,22,23,26,27,28,29,30,31,24,25,4,33,34,35,36,37,2,3,40,41,38,39,32,45,46,47],
            null,
            null,
            [0,1,2,3,26,27,28,7,6,9,10,11,12,13,4,5,16,17,18,19,20,21,22,23,24,25,40,41,42,29,30,31,34,35,36,37,38,39,32,33,14,15,8,43,44,45,46,47],
            null,
            null,
            [0,1,2,3,4,5,6,7,32,33,34,11,12,13,14,15,8,9,10,19,20,21,22,23,16,17,18,27,28,29,30,31,24,25,26,35,36,37,38,39,42,43,44,45,46,47,40,41],
            null,
            null,
            ];

        public static readonly int[][] RelativeToColor =
            [
			//				up		  front	     right		  back		  left	    down
			[WhiteFace,   BlueFace, OrangeFace,  GreenFace,    RedFace, YellowFace],
            [WhiteFace,    RedFace,   BlueFace, OrangeFace,  GreenFace, YellowFace],
            [WhiteFace,  GreenFace,    RedFace,   BlueFace, OrangeFace, YellowFace],
            [WhiteFace, OrangeFace,  GreenFace,    RedFace,   BlueFace, YellowFace],
            [YellowFace,  BlueFace,    RedFace,  GreenFace, OrangeFace, WhiteFace],
            [YellowFace,   RedFace,  GreenFace, OrangeFace,   BlueFace, WhiteFace],
            [YellowFace, GreenFace, OrangeFace,   BlueFace,    RedFace, WhiteFace],
            [YellowFace, OrangeFace,  BlueFace,    RedFace,  GreenFace, WhiteFace],
            ];

        public static readonly int[][] ColorToRelative =
            [
			//		  White		Blue	    Red			Green		Orange		Yellow
			[UpCW,    FrontCW,    LeftCW,     BackCW,     RightCW,    DownCW],
            [UpCW,    RightCW,    FrontCW,    LeftCW,     BackCW,     DownCW],
            [UpCW,    BackCW,     RightCW,    FrontCW,    LeftCW,     DownCW],
            [UpCW,    LeftCW,     BackCW,     RightCW,    FrontCW,    DownCW],
            [DownCW,  FrontCW,    RightCW,    BackCW,     LeftCW,     UpCW],
            [DownCW,  LeftCW,     FrontCW,    RightCW,    RightCW,    UpCW],
            [DownCW,  BackCW,     LeftCW,     FrontCW,    RightCW,    UpCW],
            [DownCW,  RightCW,    BackCW,     LeftCW,     FrontCW,    UpCW],
            ];

        public static readonly string[] StepCodeName =
            [
            "White Edges",
            "White Corners",
            "Mid Layer Edges",
            "Yellow Cross",
            "Yellow Corners Position",
            "Yellow Corners",
            "Yellow Edges",
            "Cube is Solved"
            ];

        public static readonly int[] WhiteCornerIndex =
            [
            -1, -1, -1, -1,  4,  1, -1, -1,  5,  2,
            -1, -1,  6,  3, -1, -1,  7,  0, -1, -1,
             8, 11, 10,  9,
            ];

        public static readonly StepCtrl[] WhiteCornerCases =
            [
            new StepCtrl(WhiteFace, RightCCW, DownCCW, RightCW),
            new StepCtrl(WhiteFace, FrontCW, DownCW, FrontCCW),
            new StepCtrl(WhiteFace, RightCCW, DownCW2, RightCW, DownCW, RightCCW, DownCCW, RightCW),
            ];

        public static readonly StepCtrl MidLayerLeft =
            new(YellowFace, UpCCW, LeftCCW, UpCW, LeftCW, UpCW, FrontCW, UpCCW, FrontCCW);

        public static readonly StepCtrl MidLayerRight =
            new(YellowFace, UpCW, RightCW, UpCCW, RightCCW, UpCCW, FrontCCW, UpCW, FrontCW);

        public static readonly StepCtrl YellowCrossLineCase =
            new(YellowFace, FrontCW, RightCW, UpCW, RightCCW, UpCCW, FrontCCW);

        public static readonly StepCtrl YellowCrossLShapeToCrossCase =
            new(YellowFace, FrontCW, UpCW, RightCW, UpCCW, RightCCW, FrontCCW);

        public static readonly StepCtrl YellowCornerPosLeft =
            new(YellowFace, LeftCW, RightCCW, UpCCW, RightCW, UpCW, LeftCCW, UpCCW, RightCCW, UpCW, RightCW);

        public static readonly StepCtrl YellowCornerPosRight =
            new(YellowFace, RightCCW, UpCCW, RightCW, UpCW, LeftCW, UpCCW, RightCCW, UpCW, RightCW, LeftCCW);

        public static readonly StepCtrl YellowCornerOrientLeft =
            new(YellowFace, LeftCW, UpCW, LeftCCW, UpCW, LeftCW, UpCW2, LeftCCW, UpCW2);

        public static readonly StepCtrl YellowCornerOrientRight =
            new(YellowFace, RightCCW, UpCCW, RightCW, UpCCW, RightCCW, UpCW2, RightCW, UpCW2);

        public static readonly StepCtrl YellowEdgeLeft =
            new(YellowFace, RightCW, UpCCW, RightCW, UpCW, RightCW, UpCW, RightCW, UpCCW, RightCCW, UpCCW, RightCW2);

        public static readonly StepCtrl YellowEdgeRight =
            new(YellowFace, RightCW2, UpCW, RightCW, UpCW, RightCCW, UpCCW, RightCCW, UpCCW, RightCCW, UpCW, RightCCW);

        public static readonly string[] BlockName;
        public static readonly int[] FaceNoToBlockNo;
        public static readonly EdgeBlock[] EdgeBlockArray;
        public static readonly CornerBlock[] CornerBlockArray;
        public static readonly int[] EdgePairArray;
        private int[] FaceArray;

        static Cube()
        {
            for (int RotateIndex = 0; RotateIndex < RotationCodes; RotateIndex += 3)
            {
                int[] M1 = RotMatrix[RotateIndex];
                int[] M2 = new int[MovableFaces];
                int[] M3 = new int[MovableFaces];
                for (int Index = 0; Index < MovableFaces; Index++)
                {
                    int R1 = M1[Index];
                    int R2 = M1[R1];
                    int R3 = M1[R2];
                    M2[Index] = R2;
                    M3[Index] = R3;
                }

                RotMatrix[RotateIndex + 1] = M2;
                RotMatrix[RotateIndex + 2] = M3;
            }

            FaceNoToBlockNo = new int[MovableFaces];
            for (int Block = 0; Block < BlocksPerCube; Block++)
                for (int Face = 0; Face < FaceColors; Face++)
                {
                    int FaceNo = BlockFace[Block, Face];
                    if (FaceNo >= 0 && FaceNo < MovableFaces) FaceNoToBlockNo[FaceNo] = Block;
                }

            BlockName = new string[BlocksPerCube];
            for (int BlockNo = 0; BlockNo < BlocksPerCube; BlockNo++)
            {
                StringBuilder Name = new();
                if (BlockFace[BlockNo, WhiteFace] >= 0 && BlockFace[BlockNo, WhiteFace] < MovableFaces)
                    Name.Append(FaceColorName[WhiteFace] + " ");
                if (BlockFace[BlockNo, YellowFace] >= 0 && BlockFace[BlockNo, YellowFace] < MovableFaces)
                    Name.Append(FaceColorName[YellowFace] + " ");
                for (int FaceColor = BlueFace; FaceColor < YellowFace; FaceColor++)
                {
                    if (BlockFace[BlockNo, FaceColor] >= 0 && BlockFace[BlockNo, FaceColor] < MovableFaces)
                        Name.Append(FaceColorName[FaceColor] + " ");
                }
                if (Name.Length > 0)
                {
                    Name.Append((BlockNo & 1) == 0 ? "Corner" : "Edge");
                    BlockName[BlockNo] = Name.ToString();
                }
            }

            EdgeBlockArray = new EdgeBlock[12];
            EdgePairArray = new int[24];
            int Ptr = 0;
            for (int BlockNo = 1; BlockNo < BlocksPerCube; BlockNo += 2)
            {
                int FaceNo1 = -1;
                int FaceNo2 = -1;
                for (int FaceColor = WhiteFace; FaceColor <= YellowFace; FaceColor++)
                {
                    int FaceNo = BlockFace[BlockNo, FaceColor];
                    if (FaceNo >= 0 && FaceNo < MovableFaces)
                    {
                        if (FaceNo1 < 0) FaceNo1 = FaceNo;
                        else FaceNo2 = FaceNo;
                    }
                }

                if (FaceNo1 >= 0 && FaceNo2 >= 0)
                {
                    EdgeBlockArray[Ptr++] = new EdgeBlock(FaceNo1, FaceNo2);
                    EdgePairArray[FaceNo1 / 2] = FaceNo2;
                    EdgePairArray[FaceNo2 / 2] = FaceNo1;
                }
            }

            CornerBlockArray = new CornerBlock[8];
            Ptr = 0;
            for (int BlockNo = 0; BlockNo < BlocksPerCube; BlockNo += 2)
            {
                int FaceNo1 = -1;
                int FaceNo2 = -1;
                int FaceNo3 = -1;
                for (int FaceColor = WhiteFace; FaceColor <= YellowFace; FaceColor++)
                {
                    int FaceNo = BlockFace[BlockNo, FaceColor];
                    if (FaceNo >= 0 && FaceNo < MovableFaces)
                    {
                        if (FaceNo1 < 0) FaceNo1 = FaceNo;
                        else if (FaceNo2 < 0) FaceNo2 = FaceNo;
                        else FaceNo3 = FaceNo;
                    }
                }
                if (FaceNo1 >= 0 && FaceNo2 >= 0 && FaceNo3 >= 0) CornerBlockArray[Ptr++] = new CornerBlock(FaceNo1, FaceNo2, FaceNo3);
            }

            return;
        }

        public Cube()
        {
            Reset();
            return;
        }

        public Cube(Cube BaseCube)
        {
            FaceArray = (int[])BaseCube.FaceArray.Clone();
            return;
        }

        public void Reset()
        {
            FaceArray = new int[MovableFaces];
            for (int Index = 0; Index < MovableFaces; Index++) FaceArray[Index] = Index;
            return;
        }

        public int FaceColor(int FaceNo)
        {
            return FaceArray[FaceNo] / FaceNoToColor;
        }

        public int[] ColorArray
        {
            get
            {
                int[] ColorArray = new int[MovableFaces];
                for (int FaceNo = 0; FaceNo < MovableFaces; FaceNo++) ColorArray[FaceNo] = FaceColor(FaceNo);
                return ColorArray;
            }
            set
            {
                FaceArray = TestUserColorArray(value);
                return;
            }
        }

        public bool AllBlocksInPlace
        {
            get
            {
                for (int Index = 0; Index < MovableFaces; Index++) if (FaceArray[Index] != Index) return false;
                return true;
            }
        }

        public bool AllWhiteEdgesInPlace
        {
            get
            {
                return FaceArray[1] == 1 && FaceArray[3] == 3 && FaceArray[5] == 5 && FaceArray[7] == 7;
            }
        }

        public bool AllWhiteCornersInPlace
        {
            get
            {
                return FaceArray[0] == 0 && FaceArray[2] == 2 && FaceArray[4] == 4 && FaceArray[6] == 6;
            }
        }

        public bool AllMidLayerEdgesInPlace
        {
            get
            {
                return FaceArray[11] == 11 && FaceArray[19] == 19 && FaceArray[27] == 27 && FaceArray[35] == 35;
            }
        }

        public bool YellowEdgesInCrossShape
        {
            get
            {
                return FaceColor(41) == YellowFace && FaceColor(43) == YellowFace && FaceColor(45) == YellowFace && FaceColor(47) == YellowFace;
            }
        }

        public bool AllYellowEdgesInPlace
        {
            get
            {
                return FaceArray[41] == 41 && FaceArray[43] == 43 && FaceArray[45] == 45 && FaceArray[47] == 47;
            }
        }

        public bool AllYellowCornersInPosition
        {
            get
            {
                return FaceNoToBlockNo[40] == FaceNoToBlockNo[FaceArray[40]] &&
                    FaceNoToBlockNo[42] == FaceNoToBlockNo[FaceArray[42]] &&
                    FaceNoToBlockNo[44] == FaceNoToBlockNo[FaceArray[44]] &&
                    FaceNoToBlockNo[46] == FaceNoToBlockNo[FaceArray[46]];
            }
        }

        public bool AllYellowCornersInPlace
        {
            get
            {
                return FaceArray[40] == 40 && FaceArray[42] == 42 && FaceArray[44] == 44 && FaceArray[46] == 46;
            }
        }

        public void RotateArray(int RotationCode)
        {
            FaceArray = RotateArray(FaceArray, RotationCode);
            return;
        }

        public void RotateArray(int[] RotationSteps)
        {
            foreach (int RotateCode in RotationSteps) RotateArray(RotateCode);
            return;
        }

        public SolutionStep NextSolutionStep()
        {
            try
            {
                if (!AllWhiteEdgesInPlace) return SolveWhiteEdges();
                else if (!AllWhiteCornersInPlace) return SolveWhiteCorners();
                else if (!AllMidLayerEdgesInPlace) return SolveMidLayerEdges();
                else if (!YellowEdgesInCrossShape) return SolveYellowEdgesCrossShape();
                else if (!AllYellowCornersInPosition) return SolveYellowCornersPosition();
                else if (!AllYellowCornersInPlace) return SolveYellowCornersOrientation();
                else if (!AllBlocksInPlace) return SolveYellowEdgesOrientation();
                return new SolutionStep();
            }
            catch
            {
                return null;
            }
        }

        private SolutionStep SolveWhiteEdges()
        {
            int[] TempFaceArray = FaceArray;
            int BestCount = 0;
            int BestRot = 0;
            int BestFaceNo = 0;
            for (int Rotation = 0; Rotation < 4; Rotation++)
            {
                if (Rotation > 0) TempFaceArray = RotateArray(TempFaceArray, WhiteCW);

                int SaveFaceNo = 0;
                int Count = 0;
                for (int FaceNo = 1; FaceNo < 9; FaceNo += 2) if (TempFaceArray[FaceNo] == FaceNo)
                    {
                        Count++;
                        SaveFaceNo = FaceNo;
                    }

                if (Count > BestCount)
                {
                    BestCount = Count;
                    BestRot = Rotation;
                    BestFaceNo = SaveFaceNo;
                }
            }

            if (BestCount > 0 && BestRot > 0)
            {
                return new SolutionStep(StepCode.WhiteEdges, "Rotate to position", BestFaceNo, WhiteFace,
                    OtherEdgeColor(BestFaceNo), [BestRot - 1]);
            }

            bool[] WhiteEdges =
            [
                FaceArray[1] == 1,
                FaceArray[3] == 3,
                FaceArray[5] == 5,
                FaceArray[7] == 7,
            ];

            for (int R1 = BlueCW; R1 < YellowCW; R1++)
            {
                SolutionStep Step = TestWhiteEdges(WhiteEdges, [R1]);
                if (Step != null) return Step;
            }

            for (int R1 = 0; R1 < RotationCodes; R1++)
            {
                for (int R2 = 0; R2 < RotationCodes; R2++)
                {
                    if (R1 / 3 == R2 / 3) continue;
                    SolutionStep Step = TestWhiteEdges(WhiteEdges, [R1, R2]);
                    if (Step != null) return Step;
                }
            }

            for (int R1 = 0; R1 < RotationCodes; R1++)
            {
                for (int R2 = 0; R2 < RotationCodes; R2++)
                {
                    if (R1 / 3 == R2 / 3) continue;
                    for (int R3 = 0; R3 < RotationCodes; R3++)
                    {
                        if (R2 / 3 == R3 / 3) continue;
                        SolutionStep Step = TestWhiteEdges(WhiteEdges, [R1, R2, R3]);
                        if (Step != null) return Step;
                    }
                }
            }

            for (int R1 = 0; R1 < RotationCodes; R1++)
            {
                for (int R2 = 0; R2 < RotationCodes; R2++)
                {
                    if (R1 / 3 == R2 / 3) continue;
                    for (int R3 = 0; R3 < RotationCodes; R3++)
                    {
                        if (R2 / 3 == R3 / 3) continue;
                        for (int R4 = 0; R4 < RotationCodes; R4++)
                        {
                            if (R3 / 3 == R4 / 3) continue;
                            SolutionStep Step = TestWhiteEdges(WhiteEdges, [R1, R2, R3, R4]);
                            if (Step != null) return Step;
                        }
                    }
                }
            }

            throw new Exception("Solve white edges. Four rotations is not enough.");
        }

        private SolutionStep TestWhiteEdges(bool[] WhiteEdges, int[] Steps)
        {
            int[] TestArray = RotateArray(FaceArray, Steps);

            if (WhiteEdges[0] && TestArray[1] != 1 || WhiteEdges[1] && TestArray[3] != 3 ||
                WhiteEdges[2] && TestArray[5] != 5 || WhiteEdges[3] && TestArray[7] != 7) return null;
            for (int FaceNo = 1; FaceNo < 9; FaceNo += 2)
            {
                if (!WhiteEdges[FaceNo / 2] && TestArray[FaceNo] == FaceNo)
                {
                    int FrontFace = RedFace + FaceNo / 2;
                    if (FrontFace > OrangeFace) FrontFace = BlueFace;
                    return new SolutionStep(StepCode.WhiteEdges, "Move to position", FaceNo, WhiteFace, FrontFace, Steps);
                }
            }
            return null;
        }

        private SolutionStep SolveWhiteCorners()
        {
            WhiteCorner[] CornerArray = new WhiteCorner[4];

            for (int Index = 0; Index < 4; Index++)
            {
                WhiteCorner Corner = WhiteCorner.Create(FaceArray, 2 * Index);
                if (Corner == null) continue;
                if (!Corner.MoveToYellow && Corner.YellowRotation == 0) return Corner.CreateSolutionStep("Move to position");
                CornerArray[Index] = Corner;
            }
            foreach (WhiteCorner Corner in CornerArray)
            {
                if (Corner != null && !Corner.MoveToYellow) return Corner.CreateSolutionStep("Rotate and move to position");
            }

            foreach (WhiteCorner Corner in CornerArray)
            {
                if (Corner != null) return Corner.CreateSolutionStep("Move to yellow face");
            }

            throw new Exception("Solve white corners. Invalid cube.");
        }

        private SolutionStep SolveMidLayerEdges()
        {
            MidLayer[] MidLayerArray = new MidLayer[4];

            for (int Index = 0; Index < 4; Index++)
            {
                MidLayer Edge = MidLayer.Create(FaceArray, 11 + 8 * Index);
                if (Edge != null && !Edge.MoveToYellow && Edge.YellowRotation == 0) return Edge.CreateSolutionStepOne("Move to position (remove first step)");
                MidLayerArray[Index] = Edge;
            }

            foreach (MidLayer Edge in MidLayerArray)
            {
                if (Edge != null && !Edge.MoveToYellow)
                {
                    if (Edge.Rotation == 0) return Edge.CreateSolutionStepTwo("Move to position");
                    return Edge.CreateSolutionStepThree("Move to position (adjust first step)");
                }
            }

            foreach (MidLayer Edge in MidLayerArray)
            {
                if (Edge != null) return Edge.CreateSolutionStepFour("Move edge to yellow face");
            }
            throw new Exception("Solve mid layer edges. Invalid cube.");
        }

        private SolutionStep SolveYellowEdgesCrossShape()
        {
            int YelEdgeOne;
            for (YelEdgeOne = 41; YelEdgeOne < 49 && FaceColor(YelEdgeOne) != YellowFace; YelEdgeOne += 2) ;

            if (YelEdgeOne == 49) return new SolutionStep(StepCode.YellowCross, "Move first two yellow faces", 45,
                 YellowFace, GreenFace, YellowCrossLineCase.Steps(2));

            int YelEdgeTwo;
            for (YelEdgeTwo = YelEdgeOne + 2; YelEdgeTwo < 49 && FaceColor(YelEdgeTwo) != YellowFace; YelEdgeTwo += 2) ;

            if (YelEdgeOne == 49) throw new Exception("Solve yellow cross. Invalid cube. Two yellow faces.");

            if (YelEdgeOne == 41 && YelEdgeTwo == 45)
                return new SolutionStep(StepCode.YellowCross, "Move from line to cross", 43,
                    YellowFace, GreenFace, YellowCrossLineCase.Steps(2));

            if (YelEdgeOne == 43 && YelEdgeTwo == 47)
                return new SolutionStep(StepCode.YellowCross, "Move from line to cross", 45,
                    YellowFace, RedFace, YellowCrossLineCase.Steps(1));

            int Index;
            if (YelEdgeOne == 41)
            {
                if (YelEdgeTwo == 43) Index = 0; // 41-43 Blue
                else Index = 1; // 41-47 Red
            }
            else if (YelEdgeOne == 43) Index = 3; // 43-45 Orange
            else Index = 2; // 45-47 Green

            return new SolutionStep(StepCode.YellowCross, "Move from L shape to cross", 47 - 2 * Index,
                YellowFace, Index + 1, YellowCrossLShapeToCrossCase.Steps(Index));
        }

        private SolutionStep SolveYellowCornersPosition()
        {
            int[] TempFaceArray = FaceArray;
            int Count;
            int Rotate = 0;
            int Match = 0;
            int Match2 = 0;
            for (; ; )
            {
                Count = 0;
                for (int Index = 40; Index < MovableFaces; Index += 2)
                {
                    if (FaceNoToBlockNo[Index] == FaceNoToBlockNo[TempFaceArray[Index]])
                    {
                        Count++;
                        Match = Index;
                        if (Count == 2 && Match2 == 0)
                        {
                            Match2 = Match;
                        }
                    }
                }
                if (Count == 1 || Count == 4 || Rotate == 3) break;
                TempFaceArray = RotateArray(TempFaceArray, YellowCW);
                Rotate++;
            }

            if (Count == 4) return new SolutionStep(StepCode.YellowCornersPos, "Rotate yellow face to position", 44,
                 YellowFace, RedFace, [YellowCW + Rotate - 1]);

            if (Count == 1)
            {
                int MatchR = Match + 2;
                if (MatchR > 46) MatchR = 40;

                int MatchL = Match - 2;
                if (MatchL < 40) MatchL = 46;

                StepCtrl Step = FaceNoToBlockNo[MatchL] == FaceNoToBlockNo[TempFaceArray[MatchR]] ? YellowCornerPosLeft : YellowCornerPosRight;

                return SolveYellowCornersPosition(Step, Match, Rotate, MatchR, "Rotate 3 corners into position");
            }

            if (Match2 == 0) throw new Exception("Solve yellow corners position. Invalid cube.");

            return SolveYellowCornersPosition(YellowCornerPosLeft, Match2, Rotate, Match2 > 44 ? 40 : Match2 + 2, "Rotate 3 corners to get one corner match");
        }

        private static SolutionStep SolveYellowCornersPosition(StepCtrl Step, int Match, int Rotate, int FaceNo, string Message)
        {
            int Index = 0;
            switch (Match)
            {
                // green
                case 40:
                    Index = 2;
                    break;

                // red
                case 42:
                    Index = 1;
                    break;

                // blue
                case 44:
                    Index = 0;
                    break;

                // orange
                case 46:
                    Index = 3;
                    break;
            }

            int[] Steps = Step.Steps(Index);
            if (Rotate != 0)
            {
                int[] TempSteps = Steps;
                int Len = TempSteps.Length;
                Steps = new int[Len + 1];
                Steps[0] = YellowCW + Rotate - 1;
                Array.Copy(TempSteps, 0, Steps, 1, Len);
            }
            return new SolutionStep(StepCode.YellowCornersPos, Message, FaceNo, YellowFace, BlueFace + Index, Steps);
        }

        private SolutionStep SolveYellowCornersOrientation()
        {
            SolutionStep Step = LookForOneYellowCornerMatch(FaceArray);
            if (Step != null) return Step;
            for (int Index = 0; Index < 8; Index++)
            {
                StepCtrl StepCtrl = Index < 4 ? YellowCornerOrientLeft : YellowCornerOrientRight;
                int StepsIndex = Index % 4;
                int[] Steps = StepCtrl.Steps(StepsIndex);
                int[] TempFaceArray = RotateArray(FaceArray, Steps);

                SolutionStep Step2 = LookForOneYellowCornerMatch(TempFaceArray);

                if (Step2 != null) return new SolutionStep(StepCode.YellowCorners, "Shuffle three yellow corners", Step2.FaceNo,
                     YellowFace, BlueFace + StepsIndex, Steps);
            }

            throw new Exception("Solve yellow corners orientation. Invalid cube.");
        }

        private static SolutionStep LookForOneYellowCornerMatch(int[] FaceArrayArg)
        {
            int Count = 0;
            int Match = 0;
            for (int FaceNo = 40; FaceNo < MovableFaces; FaceNo += 2) if (FaceArrayArg[FaceNo] == FaceNo)
                {
                    Count++;
                    Match = FaceNo;
                }

            if (Count != 1) return null;

            int Index = 0;
            switch (Match)
            {
                // green
                case 40:
                    Index = 2;
                    break;

                // red
                case 42:
                    Index = 1;
                    break;

                // blue
                case 44:
                    Index = 0;
                    break;

                // orange
                case 46:
                    Index = 3;
                    break;
            }
            bool LeftAlgo = FaceArrayArg[10 + 8 * Index] / FaceNoToColor == YellowFace;

            if (!LeftAlgo)
            {
                Index--;
                if (Index < 0) Index = 3;
            }

            StepCtrl StepCtrl = LeftAlgo ? YellowCornerOrientLeft : YellowCornerOrientRight;
            return new SolutionStep(StepCode.YellowCorners, "Rotate 3 yellow corners into their place",
                Match, YellowFace, BlueFace + Index, StepCtrl.Steps(Index));
        }

        private SolutionStep SolveYellowEdgesOrientation()
        {
            SolutionStep Step = LookForOneYellowEdgeMatch(FaceArray);
            if (Step != null) return Step;

            for (int Index = 0; Index < 8; Index++)
            {
                StepCtrl StepCtrl = Index < 4 ? YellowEdgeLeft : YellowEdgeRight;
                int StepsIndex = Index % 4;
                int[] Steps = StepCtrl.Steps(StepsIndex);

                int[] TempFaceArray = RotateArray(FaceArray, Steps);

                SolutionStep Step2 = LookForOneYellowEdgeMatch(TempFaceArray);
                if (Step2 != null) return new SolutionStep(StepCode.YellowEdges, "Shuffle yellow edges to get one match",
                     Step2.FaceNo, YellowFace, BlueFace + StepsIndex, Steps);
            }

            throw new Exception("Solve yellow edges orientation. Invalid cube.");
        }

        private static SolutionStep LookForOneYellowEdgeMatch(int[] FaceArrayArg)
        {
            int Count = 0;
            int Match = 0;
            for (int FaceNo = 41; FaceNo < MovableFaces; FaceNo += 2) if (FaceArrayArg[FaceNo] == FaceNo)
                {
                    Count++;
                    Match = FaceNo;
                }

            if (Count != 1) return null;
            int MatchR = Match + 2;
            if (MatchR > MovableFaces) MatchR = 41;

            int MatchL = Match - 2;
            if (MatchL < 41) MatchL = 47;
            int Index = 0;
            switch (Match)
            {
                // red
                case 41:
                    Index = 1;
                    break;

                // blue
                case 43:
                    Index = 0;
                    break;

                // orange
                case 45:
                    Index = 3;
                    break;

                // green
                case 47:
                    Index = 2;
                    break;
            }
            StepCtrl StepCtrl = FaceArrayArg[MatchR] == MatchL ? YellowEdgeLeft : YellowEdgeRight;
            return new SolutionStep(StepCode.YellowEdges, "Rotate 3 edges to position",
                MatchR, YellowFace, BlueFace + Index, StepCtrl.Steps(Index));
        }

        public static int[] RotateArray(int[] FaceArrayArg, int RotationCode)
        {
            int[] RotateVector = RotMatrix[RotationCode];
            int[] TempFaceArray = new int[MovableFaces];
            for (int Index = 0; Index < MovableFaces; Index++) TempFaceArray[RotateVector[Index]] = FaceArrayArg[Index];
            return TempFaceArray;
        }

        public static int[] RotateArray(int[] FaceArrayArg, int[] RotationSteps)
        {
            int[] TempFaceArray = FaceArrayArg;
            foreach (int RotateCode in RotationSteps) TempFaceArray = RotateArray(TempFaceArray, RotateCode);
            return TempFaceArray;
        }

        public static string GetBlockName(int FaceNo)
        {
            return FaceNo >= 0 ? BlockName[FaceNoToBlockNo[FaceNo]] : string.Empty;
        }

        public static int FindCorner(int[] FaceArrayArg, int FaceNo)
        {
            for (int FacePos = 0; FacePos < MovableFaces; FacePos += 2) if (FaceNo == FaceArrayArg[FacePos]) return FacePos;
            throw new Exception("Find corner failed");
        }

        public static int FindEdge(int[] FaceArrayArg, int FaceNo)
        {
            for (int FacePos = 1; FacePos < MovableFaces; FacePos += 2) if (FaceNo == FaceArrayArg[FacePos]) return FacePos;
            throw new Exception("Find edge failed");
        }

        public static int OtherEdgeColor(int FaceNo)
        {
            return EdgePairArray[FaceNo / 2] / FaceNoToColor;
        }

        public static int[] TestUserColorArray(int[] UserColorArray)
        {
            try
            {
                int[] Count = new int[FaceColors];
                for (int Index = 0; Index < MovableFaces; Index += 2)
                {
                    int ColorCode = UserColorArray[Index];
                    if (ColorCode < WhiteFace || ColorCode > YellowFace) throw new Exception("Color array corner item is not valid color");
                    Count[ColorCode]++;
                }
                for (int Index = 0; Index < FaceColors; Index++) if (Count[Index] != 4)
                    {
                        throw new Exception(string.Format("Set color error. There are too {0} {1} corner faces.",
                            Count[Index] > 4 ? "many" : "little", FaceColorName[Index]));
                    }
                Count = new int[FaceColors];

                for (int Index = 1; Index < MovableFaces; Index += 2)
                {
                    int ColorCode = UserColorArray[Index];
                    if (ColorCode < WhiteFace || ColorCode > YellowFace) throw new Exception("Color array edge item is not valid color");
                    Count[ColorCode]++;
                }

                for (int Index = 0; Index < FaceColors; Index++) if (Count[Index] != 4)
                    {
                        throw new Exception(string.Format("Set color error. There are too {0} {1} edge faces.",
                            Count[Index] > 4 ? "many" : "little", FaceColorName[Index]));
                    }

                int[] UserFaceArray = new int[MovableFaces];

                for (int Index = 0; Index < 8; Index++) TestUserCorner(CornerBlockArray[Index], UserColorArray, UserFaceArray);

                for (int Index = 0; Index < 12; Index++) TestUserEdge(EdgeBlockArray[Index], UserColorArray, UserFaceArray);

                Cube TestCube = new()
                {
                    FaceArray = (int[])UserFaceArray.Clone()
                };

                StepCode StepNo = StepCode.WhiteEdges;
                int StepCounter = 0;

                for (; ; )
                {
                    SolutionStep SolveStep = TestCube.NextSolutionStep();

                    if (SolveStep.StepCode == StepCode.CubeIsSolved) break;
                    if (SolveStep.StepCode > StepNo)
                    {
                        StepNo = SolveStep.StepCode;
                        StepCounter = 0;
                    }
                    else if (SolveStep.StepCode < StepNo)
                    {
                        throw new Exception("Invalid cube. Solution regression.");
                    }
                    else if (StepCounter > 20)
                    {
                        throw new Exception("Invalid cube. Solution is in a loop.");
                    }
                    TestCube.RotateArray(SolveStep.Steps);
                    StepCounter++;
                }
                return UserFaceArray;
            }
            catch
            {
                return [];
            }
        }

        private static void TestUserCorner(CornerBlock CornerBlock, int[] UserColorArray, int[] UserFaceArray)
        {
            int StandardFaceNoOne = CornerBlock.FaceNoOne;
            int StandardFaceNoTwo = CornerBlock.FaceNoTwo;
            int StandardFaceNoThree = CornerBlock.FaceNoThree;
            int UserFaceColorOne = UserColorArray[StandardFaceNoOne];
            int UserFaceColorTwo = UserColorArray[StandardFaceNoTwo];
            int UserFaceColorThree = UserColorArray[StandardFaceNoThree];

            if (UserFaceColorOne == UserFaceColorTwo || UserFaceColorOne == UserFaceColorThree || UserFaceColorTwo == UserFaceColorThree)
                throw new Exception(string.Format("Corner faces colors {0}, {1} and {2} must be all different.",
                    FaceColorName[UserFaceColorOne], FaceColorName[UserFaceColorTwo], FaceColorName[UserFaceColorThree]));
            foreach (CornerBlock Corner in CornerBlockArray)
            {
                int FaceNoOne;
                if (UserFaceColorOne == Corner.FaceColorOne) FaceNoOne = Corner.FaceNoOne;
                else if (UserFaceColorOne == Corner.FaceColorTwo) FaceNoOne = Corner.FaceNoTwo;
                else if (UserFaceColorOne == Corner.FaceColorThree) FaceNoOne = Corner.FaceNoThree;
                else continue;

                int FaceNoTwo;
                if (UserFaceColorTwo == Corner.FaceColorOne) FaceNoTwo = Corner.FaceNoOne;
                else if (UserFaceColorTwo == Corner.FaceColorTwo) FaceNoTwo = Corner.FaceNoTwo;
                else if (UserFaceColorTwo == Corner.FaceColorThree) FaceNoTwo = Corner.FaceNoThree;
                else continue;

                int FaceNoThree;
                if (UserFaceColorThree == Corner.FaceColorOne) FaceNoThree = Corner.FaceNoOne;
                else if (UserFaceColorThree == Corner.FaceColorTwo) FaceNoThree = Corner.FaceNoTwo;
                else if (UserFaceColorThree == Corner.FaceColorThree) FaceNoThree = Corner.FaceNoThree;
                else continue;

                UserFaceArray[StandardFaceNoOne] = FaceNoOne;
                UserFaceArray[StandardFaceNoTwo] = FaceNoTwo;
                UserFaceArray[StandardFaceNoThree] = FaceNoThree;
                return;
            }

            throw new Exception(string.Format("Corner faces colors {0}, {1} and {2} are in error",
                    FaceColorName[UserFaceColorOne], FaceColorName[UserFaceColorTwo], FaceColorName[UserFaceColorThree]));
        }

        private static void TestUserEdge(EdgeBlock EdgeBlock, int[] UserColorArray, int[] UserFaceArray)
        {
            int StandardFaceNoOne = EdgeBlock.FaceNoOne;
            int StandardFaceNoTwo = EdgeBlock.FaceNoTwo;
            int FaceColorOne = UserColorArray[StandardFaceNoOne];
            int FaceColorTwo = UserColorArray[StandardFaceNoTwo];

            if (FaceColorOne == FaceColorTwo)
                throw new Exception(string.Format("Edge faces colors {0} and {1} must be different.",
                    FaceColorName[FaceColorOne], FaceColorName[FaceColorTwo]));

            foreach (EdgeBlock Edge in EdgeBlockArray)
            {
                int FaceNoOne;
                if (FaceColorOne == Edge.FaceColorOne) FaceNoOne = Edge.FaceNoOne;
                else if (FaceColorOne == Edge.FaceColorTwo) FaceNoOne = Edge.FaceNoTwo;
                else continue;

                int FaceNoTwo;
                if (FaceColorTwo == Edge.FaceColorOne) FaceNoTwo = Edge.FaceNoOne;
                else if (FaceColorTwo == Edge.FaceColorTwo) FaceNoTwo = Edge.FaceNoTwo;
                else continue;

                UserFaceArray[StandardFaceNoOne] = FaceNoOne;
                UserFaceArray[StandardFaceNoTwo] = FaceNoTwo;
                return;
            }

            throw new Exception(string.Format("Edge faces colors {0} and {1} are in error",
                    FaceColorName[FaceColorOne], FaceColorName[FaceColorTwo]));
        }

        public static string ColorCodesToText(int[] Steps)
        {
            StringBuilder Text = new();
            for (int Index = 0; Index < Steps.Length; Index++)
            {
                string Separator;
                if (Index == 0) Separator = string.Empty;
                else if ((Index % 3) == 0) Separator = " - ";
                else Separator = " ";
                Text.AppendFormat("{0}{1}", Separator, ColorRotationName[Steps[Index]]);
            }
            return Text.ToString();
        }

        public static string RelativeCodesToText(int UpFaceColor, int FrontFaceColor, int[] Steps)
        {
            StringBuilder Text = new();

            int[] Xlate = ColorToRelative[(FrontFaceColor - 1) + (UpFaceColor == WhiteFace ? 0 : 4)];
            for (int Index = 0; Index < Steps.Length; Index++)
            {
                int Step = Steps[Index];
                string Separator;
                if (Index == 0) Separator = string.Empty;
                else if ((Index % 3) == 0) Separator = " - ";
                else Separator = " ";
                Text.AppendFormat("{0}{1}", Separator, RelativeRotationName[Xlate[Step / 3] + (Step % 3)]);
            }
            return Text.ToString();
        }
    }
}
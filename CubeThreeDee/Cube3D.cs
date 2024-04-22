using CubeSolver.ColourLogic;
using CubeSolver.Data;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace CubeSolver.CubeThreeDee
{
    public class Cube3D : ModelVisual3D
    {
        public const double BlockWidth = 1.0;
        public const double BlockSpacing = 0.05;
        public const double CubeWidth = 3 * BlockWidth + 2 * BlockSpacing;
        public const double HalfCubeWidth = 0.5 * CubeWidth;
        public const double CameraDistance = 4.0 * CubeWidth;
        public const double CameraUpAngle = 25.0 * Math.PI / 180.0;
        public const double CameraRightAngle = 25.0 * Math.PI / 180.0;
        public const double CameraViewAngle = 40;
        internal static Thickness ThinBorder = new(1);
        internal static Thickness ThickBorder = new(5);

        internal static Brush[] FaceColor =
            [

            GetTileColour.GetBrush(TileColours.White.Color),
            GetTileColour.GetBrush(TileColours.Blue.Color),
            GetTileColour.GetBrush(TileColours.Red.Color),
            GetTileColour.GetBrush(TileColours.Green.Color),
            GetTileColour.GetBrush(TileColours.Orange.Color),
            GetTileColour.GetBrush(TileColours.Yellow.Color),
            Brushes.Black,
            ];

        internal static DiffuseMaterial[] Material =
            [
            new DiffuseMaterial(GetTileColour.GetBrush(TileColours.White.Color)),
            new DiffuseMaterial(GetTileColour.GetBrush(TileColours.Blue.Color)),
            new DiffuseMaterial(GetTileColour.GetBrush(TileColours.Red.Color)),
            new DiffuseMaterial(GetTileColour.GetBrush(TileColours.Green.Color)),
            new DiffuseMaterial(GetTileColour.GetBrush(TileColours.Orange.Color)),
            new DiffuseMaterial(GetTileColour.GetBrush(TileColours.Yellow.Color)),
            new DiffuseMaterial(Brushes.Black)
            ];

        internal static Vector3D[] RotationAxis =
            [
            new Vector3D(0, 0, 1),
            new Vector3D(1, 0, 0),
            new Vector3D(0, 1, 0),
            new Vector3D(-1, 0, 0),
            new Vector3D(0, -1, 0),
            new Vector3D(0, 0, -1),
            ];

        internal static string[] RotMoveName =
            [
            "CW",
            "CW2",
            "CCW",
            ];

        internal static int[][] FullMoveAngle =
            [
            [-90, 0, 0],		// white
			[0, 0, 90],		// blue
			[0, 0, 0],		// red
			[0, 0, -90],		// green
			[0, 0, 180],		// orange
			[90, 0, 0],       // yellow
			];

        internal static int[] RotMoveAngle =
            [
            0,
            90,
            180,
            -90,
            ];

        internal static int[][] FullMoveTopColor =
            [
            [2, 3, 4, 1],		// white
			[5, 2, 0, 4],		// blue
			[5, 3, 0, 1],		// red
			[5, 4, 0, 2],		// green
			[5, 1, 0, 3],		// orange
			[4, 3, 2, 1],     // yellow
			];

        internal static int[,] BlockNoOfOneFace = new int[,]
            {
            {  0,  1,  2,  3,  4,  5,  6,  7,  8},		// white
			{  0,  3,  6,  9, 12, 15, 18, 21, 24},		// blue
			{  0,  1,  2,  9, 10, 11, 18, 19, 20},		// red
			{  2,  5,  8, 11, 14, 17, 20, 23, 26},		// green
			{  6,  7,  8, 15, 16, 17, 24, 25, 26},		// orange
			{ 18, 19, 20, 21, 22, 23, 24, 25, 26},      // yellow
			};

        public Cube FullCube;
        public BlockThreeDee[][] CubeFaceBlockArray;
        public BlockFaceThreeDee[] MovableFaceArray;

        public Cube3D()
        {
            FullCube = new Cube();

            MovableFaceArray = new BlockFaceThreeDee[Cube.MovableFaces];
            for (int BlockNo = 0; BlockNo < Cube.BlocksPerCube; BlockNo++)
            {
                BlockThreeDee Block = new(BlockNo);
                Children.Add(Block);
                if (Block.BlockFaceArray == null) continue;
                foreach (BlockFaceThreeDee Face in Block.BlockFaceArray)
                {
                    if (Face.FaceNo >= 0 && Face.FaceNo < Cube.MovableFaces) MovableFaceArray[Face.FaceNo] = Face;
                }
            }
            CubeFaceBlockArray = new BlockThreeDee[Cube.FaceColors][];

            for (int ColorIndex = 0; ColorIndex < Cube.FaceColors; ColorIndex++)
            {
                CubeFaceBlockArray[ColorIndex] = new BlockThreeDee[Cube.BlocksPerFace];
                for (int BlockIndex = 0; BlockIndex < Cube.BlocksPerFace; BlockIndex++)
                {
                    CubeFaceBlockArray[ColorIndex][BlockIndex] = (BlockThreeDee)Children[BlockNoOfOneFace[ColorIndex, BlockIndex]];
                }
            }
            return;
        }

        public void SetColorOfAllFaces()
        {
            for (int FaceNo = 0; FaceNo < Cube.MovableFaces; FaceNo++)
            {
                int FaceColor = FullCube.FaceColor(FaceNo);
                if (MovableFaceArray[FaceNo].CurrentColor != FaceColor) MovableFaceArray[FaceNo].ChangeColor(FaceColor);
            }
            return;
        }
    }
}
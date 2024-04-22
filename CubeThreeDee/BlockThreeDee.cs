using System.Windows.Media.Media3D;

namespace CubeSolver.CubeThreeDee
{
    public class BlockThreeDee : ModelVisual3D
    {
        public BlockFaceThreeDee[] BlockFaceArray;
        public double OrigX;
        public double OrigY;
        public double OrigZ;

        public BlockThreeDee(int BlockNo)
        {
            if (BlockNo == 13) return;
            OrigX = -Cube3D.HalfCubeWidth + (BlockNo % 3) * (Cube3D.BlockWidth + Cube3D.BlockSpacing);
            OrigY = -Cube3D.HalfCubeWidth + ((BlockNo / 3) % 3) * (Cube3D.BlockWidth + Cube3D.BlockSpacing);
            OrigZ = -Cube3D.HalfCubeWidth + (BlockNo / 9) * (Cube3D.BlockWidth + Cube3D.BlockSpacing);
            BlockFaceArray = new BlockFaceThreeDee[6];

            for (int FaceColor = 0; FaceColor < Cube.FaceColors; FaceColor++)
            {
                BlockFaceArray[FaceColor] = new BlockFaceThreeDee(this, Cube.BlockFace[BlockNo, FaceColor], FaceColor);
            }
            return;
        }
    }
}
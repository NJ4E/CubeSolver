using System.Windows.Media.Media3D;

namespace CubeSolver.CubeThreeDee
{
    public class ModelVisual3DCube : ModelVisual3D
    {
        public BlockFaceThreeDee BlockFace;

        public ModelVisual3DCube
                (
                BlockFaceThreeDee BlockFace,
                GeometryModel3D GeometryModel
                )
        {
            this.BlockFace = BlockFace;
            Content = GeometryModel;
            return;
        }
    }
}
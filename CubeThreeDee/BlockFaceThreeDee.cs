using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace CubeSolver.CubeThreeDee
{
    public class BlockFaceThreeDee
    {
        public int FaceNo;
        public int CurrentColor;
        private GeometryModel3D TrigGeometryOne;
        private GeometryModel3D TrigGeometryTwo;

        public BlockFaceThreeDee
                (
                BlockThreeDee Block,
                int FaceNo,
                int FaceColor
                )
        {
            // save face number
            this.FaceNo = FaceNo;

            // set current color
            CurrentColor = FaceColor;

            // initialize some geometric variables
            Point3D PointZreo = new();
            Point3D PoinOne = new();
            Point3D PointTwo = new();
            Point3D PointThree = new();
            Vector3D Normal = new();

            switch (FaceColor)
            {
                case Cube.WhiteFace:
                    PointZreo = new Point3D(Block.OrigX, Block.OrigY, Block.OrigZ);
                    PoinOne = new Point3D(Block.OrigX, Block.OrigY + Cube3D.BlockWidth, Block.OrigZ);
                    PointTwo = new Point3D(Block.OrigX + Cube3D.BlockWidth, Block.OrigY + Cube3D.BlockWidth, Block.OrigZ);
                    PointThree = new Point3D(Block.OrigX + Cube3D.BlockWidth, Block.OrigY, Block.OrigZ);
                    Normal = new Vector3D(0, 0, -1);
                    break;

                case Cube.BlueFace:
                    PointZreo = new Point3D(Block.OrigX, Block.OrigY + Cube3D.BlockWidth, Block.OrigZ + Cube3D.BlockWidth);
                    PoinOne = new Point3D(Block.OrigX, Block.OrigY + Cube3D.BlockWidth, Block.OrigZ);
                    PointTwo = new Point3D(Block.OrigX, Block.OrigY, Block.OrigZ);
                    PointThree = new Point3D(Block.OrigX, Block.OrigY, Block.OrigZ + Cube3D.BlockWidth);
                    Normal = new Vector3D(-1, 0, 0);
                    break;

                case Cube.RedFace:
                    PointZreo = new Point3D(Block.OrigX, Block.OrigY, Block.OrigZ + Cube3D.BlockWidth);
                    PoinOne = new Point3D(Block.OrigX, Block.OrigY, Block.OrigZ);
                    PointTwo = new Point3D(Block.OrigX + Cube3D.BlockWidth, Block.OrigY, Block.OrigZ);
                    PointThree = new Point3D(Block.OrigX + Cube3D.BlockWidth, Block.OrigY, Block.OrigZ + Cube3D.BlockWidth);
                    Normal = new Vector3D(0, -1, 0);
                    break;

                case Cube.GreenFace:
                    PointZreo = new Point3D(Block.OrigX + Cube3D.BlockWidth, Block.OrigY, Block.OrigZ + Cube3D.BlockWidth);
                    PoinOne = new Point3D(Block.OrigX + Cube3D.BlockWidth, Block.OrigY, Block.OrigZ);
                    PointTwo = new Point3D(Block.OrigX + Cube3D.BlockWidth, Block.OrigY + Cube3D.BlockWidth, Block.OrigZ);
                    PointThree = new Point3D(Block.OrigX + Cube3D.BlockWidth, Block.OrigY + Cube3D.BlockWidth, Block.OrigZ + Cube3D.BlockWidth);
                    Normal = new Vector3D(1, 0, 0);
                    break;

                case Cube.OrangeFace:
                    PointZreo = new Point3D(Block.OrigX + Cube3D.BlockWidth, Block.OrigY + Cube3D.BlockWidth, Block.OrigZ + Cube3D.BlockWidth);
                    PoinOne = new Point3D(Block.OrigX + Cube3D.BlockWidth, Block.OrigY + Cube3D.BlockWidth, Block.OrigZ);
                    PointTwo = new Point3D(Block.OrigX, Block.OrigY + Cube3D.BlockWidth, Block.OrigZ);
                    PointThree = new Point3D(Block.OrigX, Block.OrigY + Cube3D.BlockWidth, Block.OrigZ + Cube3D.BlockWidth);
                    Normal = new Vector3D(0, 1, 0);
                    break;

                case Cube.YellowFace:
                    PointZreo = new Point3D(Block.OrigX, Block.OrigY + Cube3D.BlockWidth, Block.OrigZ + Cube3D.BlockWidth);
                    PoinOne = new Point3D(Block.OrigX, Block.OrigY, Block.OrigZ + Cube3D.BlockWidth);
                    PointTwo = new Point3D(Block.OrigX + Cube3D.BlockWidth, Block.OrigY, Block.OrigZ + Cube3D.BlockWidth);
                    PointThree = new Point3D(Block.OrigX + Cube3D.BlockWidth, Block.OrigY + Cube3D.BlockWidth, Block.OrigZ + Cube3D.BlockWidth);
                    Normal = new Vector3D(0, 0, 1);
                    break;
            }

            // hidden black faces
            if (FaceNo < 0)
            {
                DiffuseMaterial BlackMaterial = new(Brushes.Black);
                Block.Children.Add(CreateTriangle(PointZreo, PoinOne, PointTwo, Normal, BlackMaterial));
                Block.Children.Add(CreateTriangle(PointZreo, PointTwo, PointThree, Normal, BlackMaterial));
                return;
            }

            // calculate points to separate block edge and block face
            Vector3D DiagZeroTwo = Point3D.Subtract(PointTwo, PointZreo);
            Point3D PointFour = Point3D.Add(PointZreo, Vector3D.Multiply(0.04, DiagZeroTwo));
            Point3D PointSix = Point3D.Add(PointZreo, Vector3D.Multiply(0.96, DiagZeroTwo));

            Vector3D DiagOneThree = Point3D.Subtract(PointThree, PoinOne);
            Point3D PointFive = Point3D.Add(PoinOne, Vector3D.Multiply(0.04, DiagOneThree));
            Point3D PointSeven = Point3D.Add(PoinOne, Vector3D.Multiply(0.96, DiagOneThree));

            // gray edge
            DiffuseMaterial GrayMaterial = new(Brushes.DarkGray);
            Block.Children.Add(CreateTriangle(PointZreo, PoinOne, PointFive, Normal, GrayMaterial));
            Block.Children.Add(CreateTriangle(PointZreo, PointFive, PointFour, Normal, GrayMaterial));

            Block.Children.Add(CreateTriangle(PoinOne, PointTwo, PointSix, Normal, GrayMaterial));
            Block.Children.Add(CreateTriangle(PoinOne, PointSix, PointFive, Normal, GrayMaterial));

            Block.Children.Add(CreateTriangle(PointTwo, PointThree, PointSeven, Normal, GrayMaterial));
            Block.Children.Add(CreateTriangle(PointTwo, PointSeven, PointSix, Normal, GrayMaterial));

            Block.Children.Add(CreateTriangle(PointThree, PointZreo, PointFour, Normal, GrayMaterial));
            Block.Children.Add(CreateTriangle(PointThree, PointFour, PointSeven, Normal, GrayMaterial));

            // block face color
            DiffuseMaterial ColorMaterial = Cube3D.Material[FaceColor];
            Block.Children.Add(CreateTriangle(PointFour, PointFive, PointSix, Normal, ColorMaterial, 1));
            Block.Children.Add(CreateTriangle(PointFour, PointSix, PointSeven, Normal, ColorMaterial, 2));
            return;
        }

        private ModelVisual3DCube CreateTriangle
                (
                Point3D PointZero,
                Point3D PointOne,
                Point3D PointTwo,
                Vector3D Normal,
                DiffuseMaterial Material,
                int ColorTriangle = 0
                )
        {
            MeshGeometry3D TriangleMesh = new();
            TriangleMesh.Positions.Add(PointZero);
            TriangleMesh.Positions.Add(PointOne);
            TriangleMesh.Positions.Add(PointTwo);

            TriangleMesh.TriangleIndices.Add(0);
            TriangleMesh.TriangleIndices.Add(1);
            TriangleMesh.TriangleIndices.Add(2);

            TriangleMesh.Normals.Add(Normal);
            TriangleMesh.Normals.Add(Normal);
            TriangleMesh.Normals.Add(Normal);

            GeometryModel3D GeometryModel = new(TriangleMesh, Material);
            if (ColorTriangle == 1) TrigGeometryOne = GeometryModel;
            else if (ColorTriangle == 2) TrigGeometryTwo = GeometryModel;
            return new ModelVisual3DCube(this, GeometryModel);
        }

        public void ChangeColor
                (
                int NewColor
                )
        {
            CurrentColor = NewColor;
            DiffuseMaterial Material = Cube3D.Material[NewColor];
            TrigGeometryOne.Material = Material;
            TrigGeometryTwo.Material = Material;
            return;
        }
    }
}
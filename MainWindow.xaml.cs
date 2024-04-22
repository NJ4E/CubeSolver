using CubeSolver.ColourLogic;
using CubeSolver.CubeThreeDee;
using CubeSolver.Data;
using CubeSolver.Models;
using Emgu.CV;
using Emgu.CV.Face;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace CubeSolver
{
    //Main layout screen Code behind this has all the Static and Dynamic content
    //as well as methods that work on the content.

    public enum RotationActive
    {
        Idle,
        Cube,   // whole cube rotation
        Face,   // one face rotation
    }

    public partial class MainWindow : Window
    {
        private Viewport3D CubeViewPort3D;
        private Cube3D RubiksCube3D;
        private AxisAngleRotation3D FullCubeXRotation;
        private AxisAngleRotation3D FullCubeYRotation;
        private AxisAngleRotation3D FullCubeZRotation;
        private Transform3DGroup RotateTransformGroup;

        private Button[] FrontFaceButtons;
        private Button[] UpFaceButtons;
        private int frontFaceColor;
        private int topFaceColor;
        public int[] userColorArray;
        private bool rotationLock;
        private int rotateCode;
        private List<int> pastMoves;
        private List<int> nextMoves;
        private bool stopTimer;
        private bool autoSolve;
        private bool okCube;
        public List<CubeFace> initialCube = [];
        public int[] startPos = new int[48];
        private decimal cubeSpeed = 0.1M;

        private static readonly DoubleAnimation[] AnimationArray = [
           new(90.0, new Duration(new TimeSpan(2500000))),
            new DoubleAnimation(180.0, new Duration(new TimeSpan(5000000))),
            new DoubleAnimation(-90.0, new Duration(new TimeSpan(2500000))),
        ];

        public MainWindow()
        {
            InitializeComponent();
            Application.Current.MainWindow = this;
        }

        private void OnMainGridLoaded(object sender, RoutedEventArgs e)
        {
            UpFaceButtons = new Button[Cube.FaceColors];
            UpFaceButtons[0] = UpFaceButton0;
            UpFaceButtons[1] = UpFaceButton1;

            // front face color buttons
            FrontFaceButtons = new Button[Cube.FaceColors];
            FrontFaceButtons[0] = FrontFaceButton1;
            FrontFaceButtons[1] = FrontFaceButton2;
            FrontFaceButtons[2] = FrontFaceButton3;
            FrontFaceButtons[3] = FrontFaceButton4;
            AnimationArray[0].SpeedRatio = (double)cubeSpeed;
            AnimationArray[1].SpeedRatio = (double)cubeSpeed;
            AnimationArray[2].SpeedRatio = (double)cubeSpeed;
            AnimationArray[0].Completed += AnimationCompleted;
            AnimationArray[1].Completed += AnimationCompleted;
            AnimationArray[2].Completed += AnimationCompleted;
            Speed.Text = cubeSpeed.ToString();
            return;
        }

        private void StartImageCapture_Click(object sender, RoutedEventArgs e)
        {
            Button captureImage = (Button)sender;
            captureImage.IsEnabled = false;

            if (Main.FindName("Stop") is Button btn)
            {
                btn.IsEnabled = true;
            }
            AddImage();
            stopTimer = false;
            var _timer = new System.Timers.Timer
            {
                Enabled = true,
                Interval = 2000
            };
            _timer.Elapsed += (sender, e) => CheckTimer(_timer);
        }

        private void StopCapture_Click(object sender, RoutedEventArgs e)
        {
            stopTimer = true;
            Button captureStop = (Button)sender;
            captureStop.IsEnabled = false;
            GetImageColours();
            if (Main.FindName("ResetProg") is Button btnRst)
            {
                btnRst.IsEnabled = true;
            }
            if (Main.FindName("Save") is Button btnSve)
            {
                btnSve.IsEnabled = true;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Button captureReset = (Button)sender;
            captureReset.IsEnabled = false;
            if (Main.FindName("Start") is Button btnStr)
            {
                btnStr.IsEnabled = true;
                stopTimer = false;
            }
            if (Main.FindName("Stop") is Button btnStp)
            {
                btnStp.IsEnabled = false;
            }
            if (Main.FindName("ResetProg") is Button btnRst)
            {
                btnRst.IsEnabled = false;
            }
            CreateInitialCube();
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            Button captureReset = (Button)sender;
            captureReset.IsEnabled = false;
            if (Main.FindName("Start") is Button btnStr)
            {
                btnStr.IsEnabled = true;
                stopTimer = false;
                CubeCanvas.Children.Clear();
            }
            if (Main.FindName("Stop") is Button btnStp)
            {
                btnStp.IsEnabled = false;
            }
            if (Main.FindName("Save") is Button btnSve)
            {
                btnSve.IsEnabled = false;
            }
            RemoveRectangle(true, "");
        }

        private void Solve_Click(object sender, RoutedEventArgs e)
        {
            SolvingText.Children.Clear();
            AddTextBlock("Cube Solution", true, true);
            SolveCube();
        }

        private void SolveAuto_Click(object sender, RoutedEventArgs e)
        {
            autoSolve = true;
            SolveStage.IsEnabled = false;
            SolveAuto.IsEnabled = false;
            SolveCube();
        }

        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            SolveAuto.IsEnabled = true;
            SolveStage.IsEnabled = true;
            SetUpSolving();
        }

        private void UseDemo_Click(object sender, RoutedEventArgs e)
        {
            Retry.Visibility = Visibility.Hidden;
            initialCube = DemoCube();
            SetUpSolving();
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            InstBlock.Text = "";
            SolveStage.IsEnabled = false;
            SolveAuto.IsEnabled = false;
            SolveTab.IsEnabled = false;
            AddTab.IsEnabled = true;
            AddTab.IsSelected = true;
            SolveTab.IsEnabled = false;
            Restart.IsEnabled = false;
            Start.IsEnabled = true;
            Plus.IsEnabled = false;
            Minus.IsEnabled = false;
            initialCube = [];
            Result.Children.Clear();
            Retry.Visibility = Visibility.Hidden;
            if (Main.FindName("FaceColoursBtns") is StackPanel btnfaces)
            {
                btnfaces.Visibility = Visibility.Hidden;
            }
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            HelpWindow helpWindow = new();
            helpWindow.ShowDialog();
        }

        private void Retry_Click(object sender, RoutedEventArgs e)
        {
            okCube = true;
            Retry.Visibility = Visibility.Hidden;
            UpdateAlteredCube();
            SetUpSolving();
        }

        private void Plus_Click(object sender, RoutedEventArgs e)
        {
            if (cubeSpeed < 2)
            {

                cubeSpeed += 0.1M;
                Speed.Text = cubeSpeed.ToString();
                AnimationArray[0].SpeedRatio = (double)cubeSpeed;
                AnimationArray[1].SpeedRatio = (double)cubeSpeed;
                AnimationArray[2].SpeedRatio = (double)cubeSpeed;
                Minus.IsEnabled = true;
            }
            else
            {
                Plus.IsEnabled = false;
            }
        }

        private void Minus_Click(object sender, RoutedEventArgs e)
        {
            if (cubeSpeed > 0.1M)
            {
                cubeSpeed -= 0.1M;
                Speed.Text = cubeSpeed.ToString();
                AnimationArray[0].SpeedRatio = (double)cubeSpeed;
                AnimationArray[1].SpeedRatio = (double)cubeSpeed;
                AnimationArray[2].SpeedRatio = (double)cubeSpeed;
                Plus.IsEnabled = true;
            }
            else
            {
                Minus.IsEnabled = false;
            }
        }

        private void Rect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is System.Windows.Shapes.Rectangle rect)
            {
                if (!rect.Name.StartsWith("MM"))
                {
                    ModalWindow modalWindow = new();
                    modalWindow.Selected += value => rect.Fill = new SolidColorBrush(value);
                    modalWindow.ShowDialog();
                }
            }
        }

        private void FrontFaceButtonClick(object sender, RoutedEventArgs e)
        {
            if (!rotationLock) SetUpAndFrontFace(false, TagTranslator(sender), topFaceColor);
            return;
        }

        private void UpFaceButtonClick(object sender, RoutedEventArgs e)
        {
            if (!rotationLock) SetUpAndFrontFace(true, frontFaceColor, TagTranslator(sender));
            return;
        }

        private void SetUpAndFrontFace(bool TopFaceClick, int frontFaceColor, int topFaceColor)
        {
            // no change
            if (frontFaceColor == this.frontFaceColor && topFaceColor == this.topFaceColor) return;

            // front face
            if (!TopFaceClick)
            {
                int TopIndex;
                for (TopIndex = 0; TopIndex < 4; TopIndex++)
                {
                    if (Cube3D.FullMoveTopColor[frontFaceColor][TopIndex] == topFaceColor) break;
                }
                if (TopIndex == 4) topFaceColor = Cube3D.FullMoveTopColor[frontFaceColor][0];
            }

            // top face
            else
            {
                int FrontIndex;
                for (FrontIndex = 0; FrontIndex < 4; FrontIndex++)
                {
                    if (Cube3D.FullMoveTopColor[topFaceColor][FrontIndex] == frontFaceColor) break;
                }
                if (FrontIndex == 4) frontFaceColor = Cube3D.FullMoveTopColor[topFaceColor][0];
            }

            // save
            this.frontFaceColor = frontFaceColor;
            this.topFaceColor = topFaceColor;

            // y axis rotation angle
            int YIndex;
            for (YIndex = 0; YIndex < 4; YIndex++)
            {
                if (Cube3D.FullMoveTopColor[frontFaceColor][YIndex] == topFaceColor) break;
            }

            // rotate the full cube
            FullCubeXRotation.Angle = Cube3D.FullMoveAngle[frontFaceColor][0];
            FullCubeYRotation.Angle = -Cube3D.RotMoveAngle[YIndex];
            FullCubeZRotation.Angle = Cube3D.FullMoveAngle[frontFaceColor][2];
            return;
        }

        private void SetUpSolving()
        {
            InstBlock.Text = "";
            if (initialCube.Count == 6)
            {
                foreach (CubeFace face in initialCube)
                {
                    ShowResultCube(face);
                }
                Set3DArray(initialCube);
                ResetThreeDee();

                if (okCube)
                {
                    SolveStage.IsEnabled = true;
                    SolveAuto.IsEnabled = true;
                    SolveTab.IsEnabled = true;
                    AddTab.IsEnabled = false;
                    Restart.IsEnabled = true;
                    Start.IsEnabled = false;
                    SolveTab.IsSelected = true;
                    Plus.IsEnabled = true;
                    Minus.IsEnabled = true;
                    if (Main.FindName("FaceColoursBtns") is StackPanel btnfaces)
                    {
                        btnfaces.Visibility = Visibility.Visible;
                    }
                }
            }
        }

        private void ResetThreeDee()
        {
            Retry.Visibility = Visibility.Hidden;
            try
            {
                nextMoves = [];
                pastMoves = [];
                CubeViewPort3D = new Viewport3D()
                {
                    ClipToBounds = true
                };
                CubeGrid.Children.Clear();
                CubeGrid.Children.Add(CubeViewPort3D);
                ModelVisual3D ModelVisual = new();
                CubeViewPort3D.Children.Add(ModelVisual);
                Model3DGroup ModelGroup = new();
                ModelGroup.Children.Add(new AmbientLight(Colors.White));
                ModelVisual.Content = ModelGroup;
                RubiksCube3D = new Cube3D();
                CubeViewPort3D.Children.Add(RubiksCube3D);

                double PosZ = Cube3D.CameraDistance * Math.Sin(Cube3D.CameraUpAngle);
                double Temp = Cube3D.CameraDistance * Math.Cos(Cube3D.CameraUpAngle);
                double PosX = Temp * Math.Sin(Cube3D.CameraRightAngle);
                double PosY = Temp * Math.Cos(Cube3D.CameraRightAngle);

                CubeViewPort3D.Camera = new PerspectiveCamera(new Point3D(PosX, -PosY, PosZ),
                    new Vector3D(-PosX, PosY, -PosZ), new Vector3D(0, 0, 1), Cube3D.CameraViewAngle);

                Transform3DGroup FullCubeMotion = new();
                RubiksCube3D.Transform = FullCubeMotion;
                FullCubeZRotation = new AxisAngleRotation3D(new Vector3D(0, 0, 1), 0);
                FullCubeMotion.Children.Add(new RotateTransform3D(FullCubeZRotation));
                FullCubeXRotation = new AxisAngleRotation3D(new Vector3D(1, 0, 0), 0);
                FullCubeMotion.Children.Add(new RotateTransform3D(FullCubeXRotation));
                FullCubeYRotation = new AxisAngleRotation3D(new Vector3D(0, 1, 0), 0);
                FullCubeMotion.Children.Add(new RotateTransform3D(FullCubeYRotation));

                rotationLock = false;
                topFaceColor = -1;
                frontFaceColor = -1;
                RubiksCube3D.FullCube.ColorArray = (int[])((MainWindow)Application.Current.MainWindow).startPos.Clone();
                RubiksCube3D.SetColorOfAllFaces();
                SolvingText.Children.Clear();
                AddTextBlock("Cube Solution", true, true);
                okCube = true;
                return;
            }
            catch
            {
                okCube = false;
                InstBlock.Text = "Error: Cube is incorrect, please check and try again.";
                Retry.Visibility = Visibility.Visible;
            }
        }

        private static int TagTranslator(object sender)
        {
            return int.Parse((string)((Button)sender).Tag);
        }

        private void AnimationCompleted(object sender, EventArgs e)
        {
            RotateTransformGroup.Children.Clear();
            RubiksCube3D.FullCube.RotateArray(rotateCode);
            RubiksCube3D.SetColorOfAllFaces();

            for (int Index = 0; Index < Cube.BlocksPerFace; Index++)
                RubiksCube3D.CubeFaceBlockArray[rotateCode / 3][Index].Transform = null;

            if (RubiksCube3D.FullCube.AllBlocksInPlace)
            {
                nextMoves.Clear();
                autoSolve = false;
            }
            else if (nextMoves.Count != 0)
            {
                int NextMove = nextMoves[0];
                nextMoves.RemoveAt(0);
                RotateSide(NextMove);
            }
            else if (autoSolve)
            {
                SolveCube();
            }

            rotationLock = false;
            return;
        }

        private void SolveCube()
        {
            SolutionStep SolveStep = RubiksCube3D.FullCube.NextSolutionStep();
            if (SolveStep.StepCode != StepCode.CubeIsSolved)
            {
                string solveTest = "";
                string solveTestTwo = "";
                solveTest += Cube.StepCodeName[(int)SolveStep.StepCode] + " ";
                AddTextBlock(solveTest, true);

                solveTestTwo += Cube.GetBlockName(SolveStep.FaceNo) + " ";
                AddTextBlock(solveTestTwo, false);

                GetDescription(SolveStep.UpFaceColor, SolveStep.FrontFaceColor, SolveStep.Steps);
                nextMoves.AddRange(SolveStep.Steps);
                SetUpAndFrontFace(true, SolveStep.FrontFaceColor, SolveStep.UpFaceColor);
                int NextMove = nextMoves[0];

                nextMoves.RemoveAt(0);
                RotateSide(NextMove);
            }
            else
            {
                SolveAuto.IsEnabled = false;
                SolveStage.IsEnabled = false;
            }
        }

        private void RotateSide(int RotateCode)
        {
            if (RotateCode >= 0)
            {
                pastMoves.Add(RotateCode);
            }
            else
            {
                RotateCode = pastMoves[^1];
                pastMoves.RemoveAt(pastMoves.Count - 1);
                switch (RotateCode % 3)
                {
                    case 0:
                        RotateCode += 2;
                        break;

                    case 2:
                        RotateCode -= 2;
                        break;
                }
            }

            this.rotateCode = RotateCode;
            int RotateFace = RotateCode / 3;
            RotateTransformGroup = new Transform3DGroup();

            for (int Index = 0; Index < Cube.BlocksPerFace; Index++)
            {
                RubiksCube3D.CubeFaceBlockArray[RotateFace][Index].Transform = RotateTransformGroup;
            }
            AxisAngleRotation3D AxisRot = new(Cube3D.RotationAxis[RotateFace], 0);
            RotateTransformGroup.Children.Add(new RotateTransform3D(AxisRot));
            AxisRot.BeginAnimation(AxisAngleRotation3D.AngleProperty, AnimationArray[RotateCode % 3]);

            rotationLock = true;
            return;
        }

        private void CheckTimer(System.Timers.Timer timer)
        {
            if (!stopTimer)
            {
                AddImage();
            }
            else
            {
                timer.Enabled = false;
            }
        }

        private void AddImage()
        {
            this.Dispatcher.Invoke(() =>
            {
                using var capture = new VideoCapture(0, VideoCapture.API.DShow);
                if (capture != null)
                {
                    var image = capture.QueryFrame();
                    if (image != null)
                    {
                        webCamImage.Source = image.ToBitmapSource();
                    }
                }
            });
        }

        private void GetImageColours()
        {
            BitmapSource selectedImage = (BitmapSource)webCamImage.Source;
            TilePosition tp = new();
            var pixels = CheckColour.FindColours(selectedImage);

            AddRec(160, 160, CheckColour.CheckColourAt(pixels, tp.TopLeft[0], tp.TopLeft[1]), "TopLeft", false);
            AddRec(80, 160, CheckColour.CheckColourAt(pixels, tp.TopMiddle[0], tp.TopMiddle[1]), "TopMiddle", false);
            AddRec(0, 160, CheckColour.CheckColourAt(pixels, tp.TopRight[0], tp.TopRight[1]), "TopRight", false);
            AddRec(160, 80, CheckColour.CheckColourAt(pixels, tp.MiddleLeft[0], tp.MiddleLeft[1]), "MiddleLeft", false);
            AddRec(80, 80, CheckColour.CheckColourAt(pixels, tp.MiddleMiddle[0], tp.MiddleMiddle[1]), "MiddleMiddle", false);
            AddRec(0, 80, CheckColour.CheckColourAt(pixels, tp.MiddleRight[0], tp.MiddleRight[1]), "MiddleRight", false);
            AddRec(160, 0, CheckColour.CheckColourAt(pixels, tp.BottomLeft[0], tp.BottomLeft[1]), "BottomLeft", false);
            AddRec(80, 0, CheckColour.CheckColourAt(pixels, tp.BottomMiddle[0], tp.BottomMiddle[1]), "BottomMiddle", false);
            AddRec(0, 0, CheckColour.CheckColourAt(pixels, tp.BottomRight[0], tp.BottomRight[1]), "BottomRight", false);
        }

        private void RemoveRectangle(bool main, string faceName)
        {
            string[] rectangles;
            if (main)
            {
                rectangles = ["TopLeft", "TopMiddle", "TopRight", "MiddleLeft", "MiddleMiddle", "MiddleRight", "BottomLeft", "BottomMiddle", "BottomRight"];
            }
            else
            {
                List<string> list =
                [
                    "TL" + faceName,
                    "TM" + faceName,
                    "TR" + faceName,
                    "ML" + faceName,
                    "MM" + faceName,
                    "MR" + faceName,
                    "BL" + faceName,
                    "BM" + faceName,
                    "BR" + faceName,
                ];

                rectangles = [.. list];
            }

            foreach (string name in rectangles)
            {
                if (CubeCanvas.FindName(name) is System.Windows.Shapes.Rectangle rect)
                {
                    if (main)
                    {
                        CubeCanvas.Children.Remove(rect);
                        NameScope.GetNameScope(this).UnregisterName(name);
                    }
                    else
                    {
                        Result.Children.Remove(rect);
                        NameScope.GetNameScope(this).UnregisterName(name);
                    }
                }
            }
        }

        private void AddRec(int right, int bottom, Color colour, string name, bool small)
        {
            int width = 80;
            int height = 80;
            if (small == true)
            {
                width = 40;
                height = 40;
            }
            System.Windows.Shapes.Rectangle rect;
            rect = new()
            {
                Stroke = new SolidColorBrush(new Color() { R = 0, B = 0, G = 0, A = 255 }),
                Fill = new SolidColorBrush(colour),
                Width = width,
                Height = height,
                Name = name,
            };
            RegisterName(name, rect);
            rect.MouseLeftButtonDown += Rect_MouseLeftButtonDown;
            Canvas.SetRight(rect, right);
            Canvas.SetBottom(rect, bottom);
            if (small)
            {
                Result.Children.Add(rect);
            }
            else
            {
                CubeCanvas.Children.Add(rect);
            }
        }

        private void AddTextBlock(string text, bool isBold, bool isHeading = false)
        {
            TextBlock textBlock = new()
            {
                Text = text,
                FontSize = 12,
                TextWrapping = TextWrapping.Wrap,
                MaxWidth = 400,
            };
            if (isBold == true)
            {
                textBlock.FontWeight = FontWeights.Bold;
            }
            if (isHeading == true)
            {
                textBlock.HorizontalAlignment = HorizontalAlignment.Center;
                textBlock.FontSize = 16;
            }
            SolvingText.Children.Add(textBlock);
        }

        private void GetDescription(int upColour, int frontColour, int[] steps)
        {
            string top = upColour switch
            {
                0 => "White",
                1 => "Blue",
                2 => "Red",
                3 => "Green",
                4 => "Orange",
                5 => "Yellow",
                _ => "White"
            };
            string face = frontColour switch
            {
                0 => "White",
                1 => "Blue",
                2 => "Red",
                3 => "Green",
                4 => "Orange",
                5 => "Yellow",
                _ => "White"
            };

            foreach (var step in steps)
            {
                bool clockwise = true;
                string move = "";
                string twice = "";

                if (Cube.RelativeRotationName[step][..].Contains("'"))
                {
                    clockwise = false;
                }
                if (Cube.RelativeRotationName[step].Contains('F'))
                {
                    move = "front";
                }

                if (Cube.RelativeRotationName[step].Contains('R'))
                {
                    move = "right";
                }

                if (Cube.RelativeRotationName[step].Contains('L'))
                {
                    move = "left";
                }

                if (Cube.RelativeRotationName[step].Contains('U'))
                {
                    move = "top";
                }

                if (Cube.RelativeRotationName[step].Contains('B'))
                {
                    move = "back";
                }

                if (Cube.RelativeRotationName[step].Contains('D'))
                {
                    move = "bottom";
                }


                TranslatedMoves translatedMoves = AlteredMove(face, top, clockwise, move);
                move = translatedMoves.NewMove;
                clockwise = translatedMoves.NewDirection;

                if (Cube.RelativeRotationName[step].Contains('2'))
                {
                    twice = " twice";
                }

                AddTextBlock(Description.MoveDescription(face, top, clockwise, move, twice), false);
            }
        }

        private static TranslatedMoves AlteredMove(string front, string top, bool direction, string move)
        {
            TranslatedMoves newMoves = new();
            if (top == "White")
            {
                newMoves.NewDirection = move switch
                {
                    "bottom" => !direction,
                    "top" => !direction,
                    _ => direction
                };

                if (front == "Blue")
                {
                    newMoves.NewMove = move switch
                    {
                        "left" => "right",
                        "right" => "left",
                        _ => move
                    };
                }
                if (front == "Orange")
                {
                    newMoves.NewMove = move switch
                    {
                        "front" => "left",
                        "back" => "right",
                        "right" => "back",
                        "left" => "front",
                        _ => move
                    };
                }

                if (front == "Green")
                {
                    newMoves.NewMove = move;
                    newMoves.NewDirection = direction;
                }

                if (front == "Red")
                {
                    newMoves.NewMove = move switch
                    {
                        "front" => "right",
                        "back" => "left",
                        "right" => "front",
                        "left" => "back",
                        _ => move
                    };
                }
            }
            if (top == "Yellow")
            {
                if (front == "Blue")
                {
                    newMoves.NewMove = move switch
                    {
                        "bottom" => "top",
                        "top" => "bottom",
                        _ => move
                    };
                    newMoves.NewDirection = direction;
                }

                if (front == "Orange")
                {
                    newMoves.NewMove = move switch
                    {
                        "bottom" => "top",
                        "top" => "bottom",
                        "back" => "left",
                        "right" => "back",
                        "left" => "front",
                        "front" => "right",
                        _ => move
                    };
                    newMoves.NewDirection = direction;
                }

                if (front == "Green")
                {
                    newMoves.NewMove = move switch
                    {
                        "left" => "right",
                        "right" => "left",
                        "front" => "back",
                        "back" => "front",
                        "bottom" => "top",
                        "top" => "bottom",
                        _ => move
                    };
                    newMoves.NewDirection = direction;
                }

                if (front == "Red")
                {
                    newMoves.NewMove = move switch
                    {
                        "bottom" => "top",
                        "top" => "bottom",
                        "back" => "right",
                        "right" => "front",
                        "left" => "back",
                        "front" => "left",
                        _ => move
                    };
                    newMoves.NewDirection = direction;
                }
            }
            return newMoves;
        }

        private ColourTypes GetColourType(string rectName, bool resultCanvas = false)
        {
            System.Windows.Shapes.Rectangle? rectFound;
            if (!resultCanvas)
            {
                rectFound = Result.FindName(rectName) as System.Windows.Shapes.Rectangle;
            }
            else
            {
                rectFound = CubeCanvas.FindName(rectName) as System.Windows.Shapes.Rectangle;
            }

            if (rectFound != null)
            {
                Brush brush = rectFound.Fill;
                Color colour = ((SolidColorBrush)brush).Color;
                int red = colour.R;
                int green = colour.G;
                int blue = colour.B;

                if (red == 185 && green == 0 && blue == 0)
                {
                    return ColourTypes.Red;
                }
                if (red == 0 && green == 155 && blue == 72)
                {
                    return ColourTypes.Green;
                }
                if (red == 0 && green == 69 && blue == 173)
                {
                    return ColourTypes.Blue;
                }
                if (red == 255 && green == 89 && blue == 0)
                {
                    return ColourTypes.Orange;
                }
                if (red == 255 && green == 213 && blue == 0)
                {
                    return ColourTypes.Yellow;
                }
            }
            return ColourTypes.White;
        }

        private void UpdateAlteredCube()
        {
            string[] faces = ["Top", "Bottom", "Left", "Right", "Front", "Back"];

            foreach (string faceName in faces)
            {
                CubeFace newFace = new()
                {
                    FaceId = (int)GetColourType("MM" + faceName),
                    TopLeftColour = GetColourType("TL" + faceName),
                    TopMiddleColour = GetColourType("TM" + faceName),
                    TopRightColour = GetColourType("TR" + faceName),
                    MiddleLeftColour = GetColourType("ML" + faceName),
                    MiddleRightColour = GetColourType("MR" + faceName),
                    BottomLeftColour = GetColourType("BL" + faceName),
                    BottomMiddleColour = GetColourType("BM" + faceName),
                    BottomRightColour = GetColourType("BR" + faceName),
                    FaceName = faceName
                };
                var oldFace = initialCube.Where(i => i.FaceId == newFace.FaceId).FirstOrDefault();
                if (oldFace != null)
                {
                    initialCube.Remove(oldFace);
                }
                initialCube.Add(newFace);
            }
        }

        private void CreateInitialCube()
        {
            CubeFace face = new()
            {
                FaceId = (int)GetColourType("MiddleMiddle"),
                TopLeftColour = GetColourType("TopLeft"),
                TopMiddleColour = GetColourType("TopMiddle"),
                TopRightColour = GetColourType("TopRight"),
                MiddleLeftColour = GetColourType("MiddleLeft"),
                MiddleRightColour = GetColourType("MiddleRight"),
                BottomLeftColour = GetColourType("BottomLeft"),
                BottomMiddleColour = GetColourType("BottomMiddle"),
                BottomRightColour = GetColourType("BottomRight"),
            };

            if (!initialCube.Where(i => i.FaceId == face.FaceId).Any())
            {
                string faceName = face.FaceId switch
                {
                    1 => "Top",
                    2 => "Bottom",
                    3 => "Back",
                    4 => "Left",
                    5 => "Front",
                    6 => "Right",
                    _ => "Top"
                };
                face.FaceName = faceName;
                initialCube.Add(face);
                if (initialCube.Count == 6)
                {
                    SetUpSolving();
                }
            }
            RemoveRectangle(true, "");
            ShowResultCube(face);
        }

        private static int TranslateTo3D(ColourTypes colour)
        {
            return colour switch
            {
                ColourTypes.White => 0,
                ColourTypes.Blue => 1,
                ColourTypes.Red => 2,
                ColourTypes.Green => 3,
                ColourTypes.Orange => 4,
                ColourTypes.Yellow => 5,
                _ => 0
            };
        }

        private void Set3DArray(List<CubeFace> nowFace)
        {
            int frontFaceId = 5;
            int topFaceId = 1;
            var front = nowFace.Where(x => x.FaceName == "Front").Single();
            var top = nowFace.Where(x => x.FaceName == "Top").Single();
            if (front != null && top != null)
            {
                frontFaceId = front.FaceId;
                topFaceId = top.FaceId;
            }

            int[] posSideMove = topFaceId switch
            {
                1 => [8, 7, 6, 5, 4, 3, 2, 1],
                2 => [4, 3, 2, 1, 8, 7, 6, 5],
                _ => [8, 7, 6, 5, 4, 3, 2, 1]
            };

            int[] posTopMove = frontFaceId switch
            {
                3 => [4, 3, 2, 1, 8, 7, 6, 5],
                4 => [6, 5, 4, 3, 2, 1, 8, 7],
                5 => [8, 7, 6, 5, 4, 3, 2, 1],
                6 => [2, 1, 8, 7, 6, 5, 4, 3],
                _ => [8, 7, 6, 5, 4, 3, 2, 1]
            };

            int[] posBottomMove = frontFaceId switch
            {
                3 => [4, 3, 2, 1, 8, 7, 6, 5],
                4 => [2, 1, 8, 7, 6, 5, 4, 3],
                5 => [8, 7, 6, 5, 4, 3, 2, 1],
                6 => [6, 5, 4, 3, 2, 1, 8, 7],
                _ => [8, 7, 6, 5, 4, 3, 2, 1]
            };

            foreach (CubeFace face in nowFace.OrderBy(x => x.FaceId))
            {
                int newFaceId = face.FaceId switch
                {
                    1 => 1, //White
                    2 => 6, //Yellow
                    3 => 2, //Blue
                    4 => 5, //Orange
                    5 => 4, //Green
                    6 => 3, //Red
                    _ => 1
                };

                if (face.FaceName == "Top")
                {
                    startPos.SetValue(TranslateTo3D(face.TopRightColour), (newFaceId * 8) - posTopMove[0]); // green 0
                    startPos.SetValue(TranslateTo3D(face.MiddleRightColour), (newFaceId * 8) - posTopMove[1]);
                    startPos.SetValue(TranslateTo3D(face.BottomRightColour), (newFaceId * 8) - posTopMove[2]); // red 0
                    startPos.SetValue(TranslateTo3D(face.BottomMiddleColour), (newFaceId * 8) - posTopMove[3]);
                    startPos.SetValue(TranslateTo3D(face.BottomLeftColour), (newFaceId * 8) - posTopMove[4]); //blue 0
                    startPos.SetValue(TranslateTo3D(face.MiddleLeftColour), (newFaceId * 8) - posTopMove[5]);
                    startPos.SetValue(TranslateTo3D(face.TopLeftColour), (newFaceId * 8) - posTopMove[6]); // orange 0
                    startPos.SetValue(TranslateTo3D(face.TopMiddleColour), (newFaceId * 8) - posTopMove[7]);
                }

                if (face.FaceName != "Top" && face.FaceName != "Bottom")
                {
                    startPos.SetValue(TranslateTo3D(face.BottomRightColour), (newFaceId * 8) - posSideMove[0]);
                    startPos.SetValue(TranslateTo3D(face.BottomMiddleColour), (newFaceId * 8) - posSideMove[1]);
                    startPos.SetValue(TranslateTo3D(face.BottomLeftColour), (newFaceId * 8) - posSideMove[2]);
                    startPos.SetValue(TranslateTo3D(face.MiddleLeftColour), (newFaceId * 8) - posSideMove[3]);
                    startPos.SetValue(TranslateTo3D(face.TopLeftColour), (newFaceId * 8) - posSideMove[4]);
                    startPos.SetValue(TranslateTo3D(face.TopMiddleColour), (newFaceId * 8) - posSideMove[5]);
                    startPos.SetValue(TranslateTo3D(face.TopRightColour), (newFaceId * 8) - posSideMove[6]);
                    startPos.SetValue(TranslateTo3D(face.MiddleRightColour), (newFaceId * 8) - posSideMove[7]);
                }

                if (face.FaceName == "Bottom")
                {
                    startPos.SetValue(TranslateTo3D(face.BottomLeftColour), (newFaceId * 8) - posBottomMove[0]); // green 0
                    startPos.SetValue(TranslateTo3D(face.MiddleLeftColour), (newFaceId * 8) - posBottomMove[1]);
                    startPos.SetValue(TranslateTo3D(face.TopLeftColour), (newFaceId * 8) - posBottomMove[2]); // oranage
                    startPos.SetValue(TranslateTo3D(face.TopMiddleColour), (newFaceId * 8) - posBottomMove[3]);
                    startPos.SetValue(TranslateTo3D(face.TopRightColour), (newFaceId * 8) - posBottomMove[4]); // blue 0
                    startPos.SetValue(TranslateTo3D(face.MiddleRightColour), (newFaceId * 8) - posBottomMove[5]);
                    startPos.SetValue(TranslateTo3D(face.BottomRightColour), (newFaceId * 8) - posBottomMove[6]); // red 0
                    startPos.SetValue(TranslateTo3D(face.BottomMiddleColour), (newFaceId * 8) - posBottomMove[7]);
                }
            }
        }

        private void ShowResultCube(CubeFace face)
        {
            int startBottom = face.FaceId switch
            {
                1 => 320,
                2 => 80,
                3 => 200,
                4 => 200,
                5 => 200,
                6 => 200,
                _ => 200
            };
            int startRight = face.FaceId switch
            {
                1 => 400, //white
                2 => 400, //Yellow
                3 => 160, //blue
                4 => 520, //oranage
                5 => 400, //green
                6 => 280, //red
                _ => 400
            };

            RemoveRectangle(false, face.FaceName);

            AddRec(startRight + 80, startBottom + 80, FindColour(face.TopLeftColour), "TL" + face.FaceName, true);
            AddRec(startRight + 40, startBottom + 80, FindColour(face.TopMiddleColour), "TM" + face.FaceName, true);
            AddRec(startRight + 0, startBottom + 80, FindColour(face.TopRightColour), "TR" + face.FaceName, true);
            AddRec(startRight + 80, startBottom + 40, FindColour(face.MiddleLeftColour), "ML" + face.FaceName, true);
            AddRec(startRight + 40, startBottom + 40, FindColour(face.MiddleMiddleColour), "MM" + face.FaceName, true);
            AddRec(startRight + 0, startBottom + 40, FindColour(face.MiddleRightColour), "MR" + face.FaceName, true);
            AddRec(startRight + 80, startBottom + 0, FindColour(face.BottomLeftColour), "BL" + face.FaceName, true);
            AddRec(startRight + 40, startBottom + 0, FindColour(face.BottomMiddleColour), "BM" + face.FaceName, true);
            AddRec(startRight + 0, startBottom + 0, FindColour(face.BottomRightColour), "BR" + face.FaceName, true);
        }

        private static Color FindColour(ColourTypes colourType)
        {
            Color colour = colourType switch
            {
                ColourTypes.White => new Color { R = 255, B = 255, G = 255, A = 255 },
                ColourTypes.Yellow => new Color { R = 255, B = 0, G = 213, A = 255 },
                ColourTypes.Blue => new Color { R = 0, B = 173, G = 69, A = 255 },
                ColourTypes.Orange => new Color { R = 255, B = 0, G = 89, A = 255 },
                ColourTypes.Green => new Color { R = 0, B = 72, G = 155, A = 255 },
                ColourTypes.Red => new Color { R = 185, B = 0, G = 0, A = 255 },
                _ => new Color { R = 255, B = 255, G = 255, A = 255 }
            };
            return colour;
        }

        private static List<CubeFace> DemoCube()
        {
            List<CubeFace> testCubes = [];
            var faceOne = new CubeFace
            {
                FaceId = 1,
                TopLeftColour = ColourTypes.White,
                TopMiddleColour = ColourTypes.White,
                TopRightColour = ColourTypes.White,
                MiddleLeftColour = ColourTypes.White,
                MiddleRightColour = ColourTypes.White,
                BottomLeftColour = ColourTypes.Green,
                BottomMiddleColour = ColourTypes.White,
                BottomRightColour = ColourTypes.Yellow,
                FaceName = "Top"
            };
            testCubes.Add(faceOne);

            var faceTwo = new CubeFace
            {
                FaceId = 2,
                TopLeftColour = ColourTypes.Red,
                TopMiddleColour = ColourTypes.Red,
                TopRightColour = ColourTypes.Green,
                MiddleLeftColour = ColourTypes.Red,
                MiddleRightColour = ColourTypes.Yellow,
                BottomLeftColour = ColourTypes.Blue,
                BottomMiddleColour = ColourTypes.Orange,
                BottomRightColour = ColourTypes.Green,
                FaceName = "Bottom"
            };
            testCubes.Add(faceTwo);

            var faceThree = new CubeFace
            {
                FaceId = 3,
                TopLeftColour = ColourTypes.Blue,
                TopMiddleColour = ColourTypes.Red,
                TopRightColour = ColourTypes.Blue,
                MiddleLeftColour = ColourTypes.Green,
                MiddleRightColour = ColourTypes.Orange,
                BottomLeftColour = ColourTypes.Yellow,
                BottomMiddleColour = ColourTypes.Yellow,
                BottomRightColour = ColourTypes.Orange,
                FaceName = "Back"
            };
            testCubes.Add(faceThree);

            var faceFour = new CubeFace
            {
                FaceId = 4,
                TopLeftColour = ColourTypes.Orange,
                TopMiddleColour = ColourTypes.Orange,
                TopRightColour = ColourTypes.Red,
                MiddleLeftColour = ColourTypes.Blue,
                MiddleRightColour = ColourTypes.Blue,
                BottomLeftColour = ColourTypes.Yellow,
                BottomMiddleColour = ColourTypes.Yellow,
                BottomRightColour = ColourTypes.Blue,
                FaceName = "Left"
            };
            testCubes.Add(faceFour);

            var faceFive = new CubeFace
            {
                FaceId = 5,
                TopLeftColour = ColourTypes.White,
                TopMiddleColour = ColourTypes.Blue,
                TopRightColour = ColourTypes.Green,
                MiddleLeftColour = ColourTypes.Yellow,
                MiddleRightColour = ColourTypes.Blue,
                BottomLeftColour = ColourTypes.Yellow,
                BottomMiddleColour = ColourTypes.Green,
                BottomRightColour = ColourTypes.Orange,
                FaceName = "Front"
            };
            testCubes.Add(faceFive);

            var faceSix = new CubeFace
            {
                FaceId = 6,
                TopLeftColour = ColourTypes.Orange,
                TopMiddleColour = ColourTypes.Green,
                TopRightColour = ColourTypes.Red,
                MiddleLeftColour = ColourTypes.Red,
                MiddleRightColour = ColourTypes.Orange,
                BottomLeftColour = ColourTypes.White,
                BottomMiddleColour = ColourTypes.Green,
                BottomRightColour = ColourTypes.Red,
                FaceName = "Right"
            };
            testCubes.Add(faceSix);

            return testCubes;
        }
    }
}
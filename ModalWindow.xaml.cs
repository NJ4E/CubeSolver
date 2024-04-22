using System.Windows;
using System.Windows.Media;

namespace CubeSolver
{
    //This is a Modal window used for altering colours to fix the image.

    public partial class ModalWindow : Window
    {
        public ModalWindow()
        {
            InitializeComponent();
            AddColours();
        }

        public event Action<Color> Selected;

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private new void MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is System.Windows.Shapes.Rectangle rect)
            {
                Brush brush = rect.Fill;
                Color colour = ((SolidColorBrush)brush).Color;
                Selected?.Invoke(colour);
                this.Close();
            }
        }

        private void AddColours()
        {
            Red.Fill = new SolidColorBrush(new Color { R = 185, B = 0, G = 0, A = 255 });
            Green.Fill = new SolidColorBrush(new Color { R = 0, B = 72, G = 155, A = 255 });
            Blue.Fill = new SolidColorBrush(new Color { R = 0, B = 173, G = 69, A = 255 });
            Yellow.Fill = new SolidColorBrush(new Color { R = 255, B = 0, G = 213, A = 255 });
            Orange.Fill = new SolidColorBrush(new Color { R = 255, B = 0, G = 89, A = 255 });
            White.Fill = new SolidColorBrush(new Color { R = 255, B = 255, G = 255, A = 255 });
        }
    }
}
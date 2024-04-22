using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;

namespace CubeSolver.ColourLogic
{
    public class CheckColour
    {
        public static Bitmap FindColours(BitmapSource image)
        {
            if (image != null)
            {
                var pixels = GetColour.GetPixels(image);
                var width = 640;
                var height = 480;
                Bitmap bmp = new(width, height, width * 4, System.Drawing.Imaging.PixelFormat.Format32bppArgb,
                            GCHandle.Alloc(pixels, GCHandleType.Pinned).AddrOfPinnedObject());
                return bmp;
            }
            return null;
        }

        public static System.Windows.Media.Color CheckColourAt(Bitmap bitmap, int coordX, int coordY)
        {
            if (bitmap != null)
            {
                int x = coordX;
                int y = coordY;

                Color colour = bitmap.GetPixel(x, y);

                int red = colour.R;
                int blue = colour.B;
                int green = colour.G;

                return GetTileColour.GetTile(red, green, blue);
            }
            return new System.Windows.Media.Color() { R = 163, G = 55, B = 230, A = 255 };
        }
    }
}
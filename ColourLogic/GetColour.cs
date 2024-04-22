using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CubeSolver.ColourLogic
{
    public static class GetColour
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct PixelColor
        {
            public byte Blue;
            public byte Green;
            public byte Red;
            public byte Alpha;
        }

        public static PixelColor[,] GetPixels(BitmapSource source)
        {
            if (source != null)
            {
                if (source.Format != PixelFormats.Bgra32)
                    source = new FormatConvertedBitmap(source, PixelFormats.Bgra32, null, 0);
                PixelColor[,] pixels = new PixelColor[source.PixelWidth, source.PixelHeight];
                int stride = source.PixelWidth * ((source.Format.BitsPerPixel + 7) / 8);
                GCHandle pinnedPixels = GCHandle.Alloc(pixels, GCHandleType.Pinned);
                source.CopyPixels(
                  new Int32Rect(0, 0, source.PixelWidth, source.PixelHeight),
                  pinnedPixels.AddrOfPinnedObject(),
                  pixels.GetLength(0) * pixels.GetLength(1) * 4,
                      stride);
                pinnedPixels.Free();
                return pixels;
            }
            return new PixelColor[0, 0];
        }
    }
}
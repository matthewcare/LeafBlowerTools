using System.Drawing;

namespace Lbr.Tools.Services.BitmapService;

public class BitmapService : IBitmapService
{
    public bool ColorExistsInBitmap(Bitmap bitmap, Color targetColor)
    {
        for (var i = 0; i < bitmap.Height; i++)
        {
            for (var j = 0; j < bitmap.Width; j++)
            {
                var pixelColor = bitmap.GetPixel(j, i);
                if (pixelColor == targetColor)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
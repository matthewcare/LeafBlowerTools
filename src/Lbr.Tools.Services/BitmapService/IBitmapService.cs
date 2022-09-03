using System.Drawing;

namespace Lbr.Tools.Services.BitmapService;

public interface IBitmapService
{
    bool ColorExistsInBitmap(Bitmap bitmap, Color targetColor);
}
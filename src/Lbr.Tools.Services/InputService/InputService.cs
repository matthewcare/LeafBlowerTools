using Dapplo.Windows.Common.Structs;
using Dapplo.Windows.Input.Enums;
using Dapplo.Windows.Input.Mouse;

namespace Lbr.Tools.Services.InputService;

public class InputService : IInputService
{
    public void LeftClickAtPoint(int x, int y, int screen = 0)
    {
        x *= (screen + 1);

        // Upstream issue means you can't use MouseInputGenerator.MouseClick because it
        // does a MouseDown twice instead of MouseDown and then MouseUp
        MouseInputGenerator.MouseDown(MouseButtons.Left, new NativePoint(x, y));
        // Mousedown registers at between 40 and 100 ms.
        // Using upper bounds for safety
        Thread.Sleep(100);
        MouseInputGenerator.MouseUp(MouseButtons.Left, new NativePoint(x, y));
        Thread.Sleep(100);
    }
}
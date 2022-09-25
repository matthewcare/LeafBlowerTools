using Dapplo.Windows.Common.Structs;
using Dapplo.Windows.Input.Enums;
using Dapplo.Windows.Input.Keyboard;
using Dapplo.Windows.Input.Mouse;

namespace Lbr.Tools.Services.InputService;

public class InputService : IInputService
{
    public void LeftClickAtPoint(int x, int y, int screen = 0)
    {
        x *= (screen + 1);
        MouseInputGenerator.MouseDown(MouseButtons.Left, new NativePoint(x, y));
        // Game registers at between 40 and 100 ms.
        // Using upper bounds for safety
        Thread.Sleep(100);
        MouseInputGenerator.MouseUp(MouseButtons.Left, new NativePoint(x, y));
        Thread.Sleep(100);
    }

    public void ScrollDown(int triggers)
    {
        for (var i = 0; i < triggers; i++)
        {
            MouseInputGenerator.MoveMouseWheel(-1);
            Thread.Sleep(10);
        }
    }
    public void ScrollUp(int triggers)
    {
        for (var i = 0; i < triggers; i++)
        {
            MouseInputGenerator.MoveMouseWheel(1);
            Thread.Sleep(10);
        }
    }

    public void ScrollToBottom() => ScrollDown(20);

    public void ScrollToTop() => ScrollUp(20);

    public void TriggerKeyPress(VirtualKeyCode key)
    {
        KeyboardInputGenerator.KeyDown(key);
        // Game registers at between 40 and 100 ms.
        // Using upper bounds for safety
        Thread.Sleep(100);
        KeyboardInputGenerator.KeyUp(key);
    }


    public void TriggerKeyPress(ConsoleKey key)
    {
        TriggerKeyPress((VirtualKeyCode)key);    
    }

}
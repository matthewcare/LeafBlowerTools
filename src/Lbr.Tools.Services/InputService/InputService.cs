﻿using Dapplo.Windows.Common.Structs;
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

    public void ScrollToBottom()
    {
        for (var i = 0; i < 20; i++)
        {
            MouseInputGenerator.MoveMouseWheel(-1);
            Thread.Sleep(100);
        }
    }

    public void TriggerKeyPress(VirtualKeyCode key)
    {
        KeyboardInputGenerator.KeyDown(key);
        // Game registers at between 40 and 100 ms.
        // Using upper bounds for safety
        Thread.Sleep(100);
        KeyboardInputGenerator.KeyUp(key);
        Thread.Sleep(100);
    }
}
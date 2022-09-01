using Dapplo.Windows.Input.Enums;

namespace Lbr.Tools.Services.InputService;

public interface IInputService
{
    void LeftClickAtPoint(int x, int y, int screen = 0);
    void ScrollToBottom();
    void TriggerKeyPress(VirtualKeyCode key);
}
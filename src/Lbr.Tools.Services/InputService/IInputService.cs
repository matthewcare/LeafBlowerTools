using Dapplo.Windows.Input.Enums;

namespace Lbr.Tools.Services.InputService;

public interface IInputService
{
    void LeftClickAtPoint(int x, int y, int screen = 0);
    void ScrollDown(int triggers);
    void ScrollToBottom();
    void TriggerKeyPress(VirtualKeyCode key);
}
using System.Diagnostics;
using Lbr.Tools.Core.Constants;
using Lbr.Tools.Services.InputService;

namespace Lbr.Tools.Services.CardService;

public class CardService : ICardService
{
    private readonly IInputService _inputService;

    public CardService(IInputService inputService)
    {
        _inputService = inputService;
    }

    public TimeSpan Cycle(TimeSpan duration)
    {
        var sw = Stopwatch.StartNew();

        // Close menus so you can see the cards flying
        _inputService.TriggerKeyPress(ConsoleKey.Escape);

        var stopTime = DateTime.Now.Add(duration);
        while (DateTime.Now < stopTime)
        {
            _inputService.TriggerKeyPress(Constants.HotKeys.BlazingSkull);
            _inputService.TriggerKeyPress(Constants.HotKeys.Wind);
        }

        _inputService.TriggerKeyPress(Constants.HotKeys.CardsMenu);
        // Ensure Transcend tab
        _inputService.LeftClickAtPoint(520, 895);
        // Transcend regular cards button
        _inputService.LeftClickAtPoint(395, 300);
        // Confirm button
        _inputService.LeftClickAtPoint(960, 405);

        sw.Stop();
        return sw.Elapsed;
    }
}
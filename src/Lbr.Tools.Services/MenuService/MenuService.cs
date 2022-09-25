using System.Runtime.InteropServices;
using Lbr.Tools.Core.Constants;
using Lbr.Tools.Core.Entities;
using Lbr.Tools.Core.Helpers;
using Lbr.Tools.Services.BossFarmService;
using Lbr.Tools.Services.CardService;
using Lbr.Tools.Services.InputService;
using Lbr.Tools.Services.TradeService;

namespace Lbr.Tools.Services.MenuService;

public class MenuService : IMenuService
{
    private readonly ITradeService _tradeService;
    private readonly IBossFarmService _bossFarmService;
    private readonly ICardService _cardService;
    private readonly IInputService _inputService;

    private readonly List<string> _menuOptions = new() { "Start", "Exit" };


    public MenuService(ITradeService tradeService, IBossFarmService bossFarmService, ICardService cardService, IInputService inputService)
    {
        _tradeService = tradeService;
        _bossFarmService = bossFarmService;
        _cardService = cardService;
        _inputService = inputService;
    }

    public void ShowMainMenu()
    {
        var menuOption = GetMenuOption(_menuOptions, "Select an option");

        if (menuOption == "Exit")
        {
            Environment.Exit(0);
        }
        else
        {
            Console.Clear();
            StartActions();
        }
    }

    private void StartActions()
    {
        var cycles = 0;

        while (true)
        {
            // Ensure we are focused on the game / close any open overlays
            _inputService.LeftClickAtPoint(0, Constants.Screen.Height / 2);

            var bossDuration = _bossFarmService.Cycle();
            _inputService.TriggerKeyPress(Constants.HotKeys.Vortex);
            var tradeDuration = _tradeService.Cycle(new Gem());

            var allowedCardDuration = BossFarmService.BossFarmService.BossRespawnTime;
            allowedCardDuration = allowedCardDuration.Add(-bossDuration);
            allowedCardDuration = allowedCardDuration.Add(-tradeDuration);

            _cardService.Cycle(allowedCardDuration);
            
            cycles++;

            var stop = ConsoleHelper.ShowCountdown($"Cycle {cycles} finished. Continuing in {{0:mm\\:ss}}", TimeSpan.FromSeconds(10));
            if (stop)
            {
                break;
            }
        }

        ShowMainMenu();
    }
    
    private static string GetMenuOption(IReadOnlyList<string> menuItems, string message)
    {
        string? menuOption = null;
        while (menuOption is null)
        {
            Console.Clear();
            for (var i = 0; i < menuItems.Count; i++)
            {
                Console.WriteLine($"{i + 1}) {menuItems[i]}");
            }
            Console.Write($"\r\n{message}: ");

            if (int.TryParse(Console.ReadLine(), out var option) && option > 0 && option - 1 < menuItems.Count)
            {
                menuOption = menuItems[option - 1];
            }
        }

        return menuOption;
    }
}
using Lbr.Tools.Core.Constants;
using Lbr.Tools.Core.Entities;
using Lbr.Tools.Services.BossFarmService;
using Lbr.Tools.Services.TradeService;

namespace Lbr.Tools.Services.MenuService;

public class MenuService : IMenuService
{
    private readonly ITradeService _tradeService;
    private readonly IBossFarmService _bossFarmService;

    public MenuService(ITradeService tradeService, IBossFarmService bossFarmService)
    {
        _tradeService = tradeService;
        _bossFarmService = bossFarmService;
    }

    public void ShowMainMenu()
    {
        var menuOption = GetMenuOption(Constants.Menus.MainMenu);

        switch (menuOption)
        {
            case "Trader":
                ShowTradeMenu();
                break;
            case "Boss Farmer":
                ShowBossFarmerMenu();
                break;
            case "Exit":
                Environment.Exit(0);
                break;
            default:
                ShowMainMenu();
                break;
        }
    }

    public void ShowTradeMenu()
    {
        var menuOption = GetMenuOption(Constants.Menus.TradeMenu);
        
        switch (menuOption)
        {
            case "Gems":
                _tradeService.StartTrading(new Gem());
                ShowMainMenu();
                break;
            case "Main Menu":
                ShowMainMenu();
                break;
            default:
                ShowTradeMenu();
                break;
        }
    }

    public void ShowBossFarmerMenu()
    {
        var menuOption = GetMenuOption(Constants.Menus.BossFarmMenu);

        switch (menuOption)
        {
            case "Cycle":
                _bossFarmService.StartFarming(false);
                ShowMainMenu();
                break;
            case "Cycle + Vortex":
                _bossFarmService.StartFarming(true);
                ShowMainMenu();
                break;
            case "Cycle + Vortex + Gem Trade":
                _bossFarmService.StartFarming(true, new Gem());
                ShowMainMenu();
                break;
            case "Main Menu":
                ShowMainMenu();
                break;
            default:
                ShowTradeMenu();
                break;
        }
    }

    private static string GetMenuOption(IReadOnlyList<string> menuItems)
    {
        string? menuOption = null;
        while (menuOption is null)
        {
            Console.Clear();
            for (var i = 0; i < menuItems.Count; i++)
            {
                Console.WriteLine($"{i + 1}) {menuItems[i]}");
            }
            Console.Write("\r\nSelect an option: ");

            if (int.TryParse(Console.ReadLine(), out var option) && option > 0 && option - 1 < menuItems.Count)
            {
                menuOption = menuItems[option - 1];
            }
        }

        return menuOption;
    }
}
using System.Windows;
using Lbr.Tools.Core.Constants;
using Lbr.Tools.Core.Entities;
using Lbr.Tools.Services.TradeService;

namespace Lbr.Tools.Services.MenuService;

public class MenuService : IMenuService
{
    private readonly ITradeService _tradeService;

    public MenuService(ITradeService tradeService)
    {
        _tradeService = tradeService;
    }

    public void ShowMainMenu()
    {
        string? menuOption = null;
        while (menuOption is null)
        {
            menuOption = GetMenuOption(Constants.Menus.MainMenu);
        }

        switch (menuOption)
        {
            case "Trader":
                ShowTradeMenu();
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
        string? menuOption = null;
        while (menuOption is null)
        {
            menuOption = GetMenuOption(Constants.Menus.TradeMenu);
        }

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

    private static string? GetMenuOption(string[] menuItems)
    {
        Console.Clear();
        for (var i = 0; i < menuItems.Length; i++)
        {
            Console.WriteLine($"{i + 1}) {menuItems[i]}");
        }
        Console.Write("\r\nSelect an option: ");

        if (!int.TryParse(Console.ReadLine(), out var option) || menuItems.Length <= option - 1)
        {
            return null;
        }

        return menuItems[option - 1];
    }
}
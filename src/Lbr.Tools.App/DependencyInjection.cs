using Lbr.Tools.Services.BitmapService;
using Lbr.Tools.Services.BossFarmService;
using Lbr.Tools.Services.CardService;
using Lbr.Tools.Services.InputService;
using Lbr.Tools.Services.MenuService;
using Lbr.Tools.Services.TradeService;
using Microsoft.Extensions.DependencyInjection;

namespace Lbr.Tools.App;

public class DependencyInjection
{
    public static ServiceProvider ServiceProvider = null!;

    public static void RegisterServices()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IInputService, InputService>();
        services.AddSingleton<ITradeService, TradeService>();
        services.AddSingleton<IMenuService, MenuService>();
        services.AddSingleton<IBossFarmService, BossFarmService>();
        services.AddSingleton<ICardService, CardService>();
        services.AddSingleton<IBitmapService, BitmapService>();
        ServiceProvider = services.BuildServiceProvider(true);
    }

    public static void DisposeServices()
    {
        if (ServiceProvider is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
}
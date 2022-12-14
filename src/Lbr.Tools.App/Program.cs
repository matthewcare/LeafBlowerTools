using Lbr.Tools.App;
using Lbr.Tools.Services.MenuService;
using Microsoft.Extensions.DependencyInjection;

DependencyInjection.RegisterServices();
var menuService = DependencyInjection.ServiceProvider.GetService<IMenuService>() ?? throw new Exception("Menu service was null");
menuService.ShowMainMenu();
DependencyInjection.DisposeServices();
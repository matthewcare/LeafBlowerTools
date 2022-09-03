using Lbr.Tools.Core.Entities;

namespace Lbr.Tools.Services.TradeService;

public interface ITradeService
{
    void StartTrading<TTradable>(TTradable entity) where TTradable : ITradable;
    void Cycle<TTradable>(TTradable entity) where TTradable : ITradable;
}
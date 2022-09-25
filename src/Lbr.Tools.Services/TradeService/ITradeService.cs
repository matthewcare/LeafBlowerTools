using Lbr.Tools.Core.Entities;

namespace Lbr.Tools.Services.TradeService;

public interface ITradeService
{
    TimeSpan Cycle<TTradable>(TTradable entity) where TTradable : ITradable;
}
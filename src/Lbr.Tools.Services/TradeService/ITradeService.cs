using Lbr.Tools.Core.Entities;

namespace Lbr.Tools.Services.TradeService;

public interface ITradeService
{
    void StartTrading<TEntity>(TEntity entity) where TEntity : IEntity;
}
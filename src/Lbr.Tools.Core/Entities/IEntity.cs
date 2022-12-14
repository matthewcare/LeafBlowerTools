using System.Drawing;

namespace Lbr.Tools.Core.Entities;

public interface IEntity
{
    int Width { get; }
    int Height { get; }
    Color DistinguishingColor { get; }
    int TradeSpacing { get; }
    (int, int) FirstTradeCoordinate { get; }
}
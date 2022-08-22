using System.Drawing;

namespace Lbr.Tools.Core.Entities;

public record Gem : IEntity
{
    public int Width => 23;
    public int Height => 26;
    public Color DistinguishingColor => Color.FromArgb(156, 21, 57);
    public int TradeSpacing => 58;
    public (int, int) FirstTradeCoordinate => (829, 331);
}
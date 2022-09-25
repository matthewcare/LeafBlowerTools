using System.Drawing;

namespace Lbr.Tools.Core.Entities;

public record Gem : ITradable
{
    public int Width => 23;
    public int Height => 26;
    public Color DistinguishingColor => Color.FromArgb(156, 21, 57);
    public int TradeSpacing => 30;
    public (int, int) FirstTradeCoordinate => (927, 300);
}
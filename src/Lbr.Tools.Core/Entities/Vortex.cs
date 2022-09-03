namespace Lbr.Tools.Core.Entities;

public record Vortex : IArtifact
{
    public bool RequiresScroll => true;
    public (int, int) UseButtonCoordinates => (1350, 583);
}
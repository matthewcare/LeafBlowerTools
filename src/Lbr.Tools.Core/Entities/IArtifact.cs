namespace Lbr.Tools.Core.Entities;

public interface IArtifact
{
    /// <summary>
    /// If the artifact coordinates are
    /// relative to the position of the
    /// window's scroll position
    /// </summary>
    bool RequiresScroll { get; }
    (int, int) UseButtonCoordinates { get; }
}
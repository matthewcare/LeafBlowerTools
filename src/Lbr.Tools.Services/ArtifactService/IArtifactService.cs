using Lbr.Tools.Core.Entities;

namespace Lbr.Tools.Services.ArtifactService;

public interface IArtifactService
{
    void UseArtifact<TArtifact>(TArtifact artifact) where TArtifact : IArtifact;
}
using Lbr.Tools.Core.Entities;

namespace Lbr.Tools.Services.BossFarmService;

public interface IBossFarmService
{
    void StartFarming(bool vortex, ITradable? tradeEntity = null);
    void Cycle();
}
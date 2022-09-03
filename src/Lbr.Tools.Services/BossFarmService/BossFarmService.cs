using Dapplo.Windows.Input.Enums;
using Lbr.Tools.Core.Constants;
using Lbr.Tools.Core.Entities;
using Lbr.Tools.Core.Helpers;
using Lbr.Tools.Services.ArtifactService;
using Lbr.Tools.Services.InputService;
using Lbr.Tools.Services.TradeService;

namespace Lbr.Tools.Services.BossFarmService;

public class BossFarmService : IBossFarmService
{
    private readonly IInputService _inputService;
    private readonly IArtifactService _artifactService;
    private readonly ITradeService _tradeService;
    private const VirtualKeyCode AreasHotKey = VirtualKeyCode.KeyV;
   
    private static readonly TimeSpan BossRespawnTime = TimeSpan.FromSeconds(170);

    private static readonly (int, int) FirstTeleportButtonCoordinates = (1350, 270);
    private static readonly (int, int) FavouritesTabButtonCoordinates = (400, 893);
    private const int TeleportButtonSpacing = 106;
    private const int TeleportButtonCount = 5;
    private const int WaitInterval = 2500;

    public BossFarmService(IInputService inputService, IArtifactService artifactService, ITradeService tradeService)
    {
        _inputService = inputService;
        _artifactService = artifactService;
        _tradeService = tradeService;
    }

    /// <summary>
    /// Cycles through the areas
    /// in the "favourites" tab.
    /// </summary>
    /// <remarks>
    /// Assumes hotkeys of "v" for areas
    /// </remarks>
    public void StartFarming(bool vortex, ITradable? tradeEntity = null)
    {
        Console.Clear();
        Console.WriteLine("Farming started. Press escape during wait timer to stop");
        Thread.Sleep(2000);

        var cycles = 1;

        while (true)
        {
            Cycle();
            if (vortex)
            {
                _artifactService.UseArtifact(new Vortex());
            }

            var tradeStarted = DateTime.Now;

            if (tradeEntity is not null)
            {
                _tradeService.Cycle(tradeEntity);
            }

            var tradeEnded = DateTime.Now;

            var tradeDuration = (int)(tradeEnded - tradeStarted).TotalSeconds;

            var remainingBossRespawn = BossRespawnTime.Add(TimeSpan.FromSeconds(-tradeDuration));

            var frameDropAdjustment = Math.Round((double)cycles / 10);
            remainingBossRespawn = remainingBossRespawn.Add(TimeSpan.FromSeconds(frameDropAdjustment));
            
            if (remainingBossRespawn.TotalSeconds > 0)
            {
                var stop = ConsoleHelper.ShowCountdown($"Cycle {cycles} complete, continuing in {{0:mm\\:ss}}",
                    remainingBossRespawn);

                if (stop)
                {
                    break;
                }
            }

            cycles++;
        }

    }

    public void Cycle()
    {
        // Ensure we are focused on the game / close any open overlays
        _inputService.LeftClickAtPoint(0, Constants.Screen.Height / 2);

        // Open areas menu
        _inputService.TriggerKeyPress(AreasHotKey);

        // Ensure favourites tab is selected
        _inputService.LeftClickAtPoint(FavouritesTabButtonCoordinates.Item1, FavouritesTabButtonCoordinates.Item2);

        for (var i = 0; i < TeleportButtonCount; i++)
        {
            var yCoordinate = FirstTeleportButtonCoordinates.Item2 + (TeleportButtonSpacing * i);
            _inputService.LeftClickAtPoint(FirstTeleportButtonCoordinates.Item1, yCoordinate);
            Thread.Sleep(WaitInterval);
        }
    }
}
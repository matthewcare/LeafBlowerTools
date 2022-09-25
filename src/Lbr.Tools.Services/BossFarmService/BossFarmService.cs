using Lbr.Tools.Core.Constants;
using Lbr.Tools.Services.InputService;
using System.Diagnostics;

namespace Lbr.Tools.Services.BossFarmService;

public class BossFarmService : IBossFarmService
{
    private readonly IInputService _inputService;

    public static readonly TimeSpan BossRespawnTime = TimeSpan.FromMinutes(3);
    private const int BossesToFarm = 9;
    private const int WaitInterval = 2500;

    private static readonly (int, int) FirstTeleportButtonCoordinates = (1350, 270);
    private static readonly (int, int) FirstScrolledTeleportButtonCoordinates = (1350, 365);
    private static readonly (int, int) FavouritesTabButtonCoordinates = (400, 893);
    private const int TeleportButtonSpacing = 106;
    
    public BossFarmService(IInputService inputService)
    {
        _inputService = inputService;
    }

    public TimeSpan Cycle()
    {
        var sw = Stopwatch.StartNew();
        
        _inputService.TriggerKeyPress(Constants.HotKeys.AreasMenu);

        // Ensure favourites tab is selected
        _inputService.LeftClickAtPoint(FavouritesTabButtonCoordinates.Item1, FavouritesTabButtonCoordinates.Item2);

        var hasScrolledToBottom = false;
        var count = 0;

        for (var i = 0; i < BossesToFarm; i++)
        {
            var coords = hasScrolledToBottom ? FirstScrolledTeleportButtonCoordinates : FirstTeleportButtonCoordinates;
            var yCoordinate = coords.Item2 + (TeleportButtonSpacing * count);
            _inputService.LeftClickAtPoint(coords.Item1, yCoordinate);

            // Wait for slap
            Thread.Sleep(WaitInterval);

            count++;

            if (i == 4 && !hasScrolledToBottom)
            {
                _inputService.ScrollToBottom();
                hasScrolledToBottom = true;
                count = 0;
            }

        }

        if (hasScrolledToBottom)
        {
            _inputService.ScrollToTop();
        }

        sw.Stop();
        return sw.Elapsed;
    }
}
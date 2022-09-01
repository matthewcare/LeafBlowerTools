using Dapplo.Windows.Input.Enums;
using Lbr.Tools.Core.Constants;
using Lbr.Tools.Core.Helpers;
using Lbr.Tools.Services.InputService;

namespace Lbr.Tools.Services.BossFarmService;

public class BossFarmService : IBossFarmService
{
    private readonly IInputService _inputService;
    private const VirtualKeyCode AreasHotKey = VirtualKeyCode.KeyV;
    private const VirtualKeyCode ArtifactsHotKey = VirtualKeyCode.F1;
    private static readonly TimeSpan BossRespawnTime = TimeSpan.FromSeconds(170);

    private static readonly (int, int) FirstTeleportButtonCoordinates = (1350, 270);
    private static readonly (int, int) FavouritesTabButtonCoordinates = (400, 893);
    private static readonly (int, int) VortexButtonCoordinates = (1350, 583);
    private const int TeleportButtonSpacing = 106;
    private const int TeleportButtonCount = 5;
    private const int WaitInterval = 2500;

    public BossFarmService(IInputService inputService)
    {
        _inputService = inputService;
    }

    /// <summary>
    /// Cycles through the areas
    /// in the "favourites" tab.
    /// </summary>
    /// <remarks>
    /// Assumes hotkeys of "v" for areas, and "F1" for artifacts
    /// </remarks>
    public void StartFarming(bool vortex)
    {
        Console.Clear();

        Console.WriteLine("Farming started. Press escape during wait timer to stop");

        Thread.Sleep(2000);

        var cycles = 1;

        while (true)
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

            if (vortex)
            {
                // Open artifacts menu
                _inputService.TriggerKeyPress(ArtifactsHotKey);
                _inputService.ScrollToBottom();
                _inputService.LeftClickAtPoint(VortexButtonCoordinates.Item1, VortexButtonCoordinates.Item2);
            }

            var stop = ConsoleHelper.ShowCountdown($"Cycle {cycles} complete, continuing in {{0:mm\\:ss}}", BossRespawnTime);
            if (stop)
            {
                break;
            }

            cycles++;
        }

    }
}
using Dapplo.Windows.Input.Enums;
using Lbr.Tools.Core.Constants;
using Lbr.Tools.Core.Entities;
using Lbr.Tools.Services.InputService;

namespace Lbr.Tools.Services.ArtifactService;

public class ArtifactService : IArtifactService
{
    private readonly IInputService _inputService;
    private const VirtualKeyCode ArtifactsHotKey = VirtualKeyCode.F1;
    private static readonly (int, int) VortexButtonCoordinates = (1350, 583);

    public ArtifactService(IInputService inputService)
    {
        _inputService = inputService;
    }


    public void UseArtifact<TArtifact>(TArtifact artifact) where TArtifact : IArtifact
    {

        // Ensure we are focused on the game / close any open overlays
        _inputService.LeftClickAtPoint(0, Constants.Screen.Height / 2);

        // Open artifact menu
        _inputService.TriggerKeyPress(ArtifactsHotKey);

        if (artifact.RequiresScroll)
        {
            _inputService.ScrollToBottom();
        }

        _inputService.LeftClickAtPoint(VortexButtonCoordinates.Item1, VortexButtonCoordinates.Item2);
    }
}
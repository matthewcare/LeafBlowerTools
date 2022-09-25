using System.Diagnostics;
using Dapplo.Windows.Input.Enums;
using Lbr.Tools.Core.Constants;
using Lbr.Tools.Core.Entities;
using Lbr.Tools.Services.BitmapService;
using Lbr.Tools.Services.InputService;
using System.Drawing;
using System.Drawing.Imaging;

namespace Lbr.Tools.Services.TradeService;

public class TradeService : ITradeService
{
    private readonly IInputService _inputService;
    private readonly IBitmapService _bitmapService;
    private static readonly (int, int) RefreshButtonCoordinates = (470, 835);
    private static readonly (int, int) CollectButtonCoordinates = (1475, 835);
    private static readonly (int, int) FirstStartButtonCoordinates = (1474, 300);
    private const int StartButtonWidth = 94;
    private const int StartButtonHeight = 38;
    private const int StartButtonSpacing = 19;
    private const int TradeSlots = 9;

    private const VirtualKeyCode TradeHotKey = VirtualKeyCode.KeyY;

    private readonly Bitmap _captureBmp;
    private readonly Graphics _captureGraphic;
    private ITradable _tradable = null!;


    public TradeService(IInputService inputService, IBitmapService bitmapService)
    {
        _inputService = inputService;
        _bitmapService = bitmapService;
        _captureBmp = new Bitmap(Constants.Screen.Width, Constants.Screen.Height, PixelFormat.Format32bppArgb);
        _captureGraphic = Graphics.FromImage(_captureBmp);
    }

    ~TradeService()
    {
        _captureGraphic.Dispose();
    }

    public TimeSpan Cycle<TTradable>(TTradable entity) where TTradable : ITradable
    {
        var sw = Stopwatch.StartNew();
        _tradable = entity;

        // Ensure we are focused on the game / close any open overlays
        _inputService.LeftClickAtPoint(0, Constants.Screen.Height / 2);

        // Open trade menu
        _inputService.TriggerKeyPress(TradeHotKey);

        // Collect any trades which might be finished
        _inputService.LeftClickAtPoint(CollectButtonCoordinates.Item1, CollectButtonCoordinates.Item2);

        RefreshTradeWindow();

        while (TradesAvailable())
        {
            FindEntityInTradeSlots();
            RefreshTradeWindow();
        }

        sw.Stop();
        return sw.Elapsed;
    }

    private void RefreshTradeWindow()
    {
        // Refresh all trades
        _inputService.LeftClickAtPoint(RefreshButtonCoordinates.Item1, RefreshButtonCoordinates.Item2);

        // Take a screen shot
        CaptureScreen(Constants.Screen.Width, Constants.Screen.WindowLocation);
    }

    private void CaptureScreen(int screenWidth, int screenIndex)
    {
        int xCoordinate;
        if (screenIndex <= 0)
        {
            screenIndex = (screenIndex + 1) * -1;
            xCoordinate = screenIndex * screenWidth;
            _captureGraphic.CopyFromScreen(xCoordinate, 0, 0, 0, _captureBmp.Size);
        }

        screenIndex -= 1;
        xCoordinate = screenIndex * screenWidth;
        _captureGraphic.CopyFromScreen(xCoordinate, 0, 0, 0, _captureBmp.Size);
    }

    private bool TradesAvailable()
    {
        var cloneRect = new Rectangle(FirstStartButtonCoordinates.Item1, FirstStartButtonCoordinates.Item2,
            StartButtonWidth, StartButtonHeight);
        var cloneBitmap = _captureBmp.Clone(cloneRect, _captureBmp.PixelFormat);

        return _bitmapService.ColorExistsInBitmap(cloneBitmap, Constants.Colors.ClickableButton);
    }

    private void FindEntityInTradeSlots()
    {
        for (var i = 0; i < TradeSlots; i++)
        {
            var offset = _tradable.FirstTradeCoordinate.Item2 + (i * (_tradable.TradeSpacing + _tradable.Height));
            var cloneRect = new Rectangle(_tradable.FirstTradeCoordinate.Item1, offset, _tradable.Width, _tradable.Height);

            var cloneBitmap = _captureBmp.Clone(cloneRect, _captureBmp.PixelFormat);

            if (_bitmapService.ColorExistsInBitmap(cloneBitmap, _tradable.DistinguishingColor))
            {
                ProcessEntity(i);
            }
        }
    }

    private void ProcessEntity(int tradeIndex)
    {
        var offset = FirstStartButtonCoordinates.Item2 + (tradeIndex * (StartButtonSpacing + StartButtonHeight));
        var cloneRect = new Rectangle(FirstStartButtonCoordinates.Item1, offset, StartButtonWidth, StartButtonHeight);
        var cloneBitmap = _captureBmp.Clone(cloneRect, _captureBmp.PixelFormat);

        if (!_bitmapService.ColorExistsInBitmap(cloneBitmap, Constants.Colors.ClickableButton))
        {
            // Trade already started for entity
            return;
        }

        var topLeftY = FirstStartButtonCoordinates.Item2 + (tradeIndex * (StartButtonSpacing + StartButtonHeight));

        var xCoord = FirstStartButtonCoordinates.Item1 + StartButtonWidth;
        var yCoord = topLeftY + (StartButtonHeight / 2);

        _inputService.LeftClickAtPoint(xCoord, yCoord);
    }
}
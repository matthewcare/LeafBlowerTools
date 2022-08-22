using System.Drawing;
using System.Drawing.Imaging;
using Lbr.Tools.Core.Constants;
using Lbr.Tools.Core.Entities;
using Lbr.Tools.Core.Helpers;
using Lbr.Tools.Services.InputService;

namespace Lbr.Tools.Services.TradeService;

public class TradeService : ITradeService
{
    private readonly IInputService _inputService;
    private static readonly (int, int) RefreshButtonCoordinates = (580, 835);
    private static readonly (int, int) CollectButtonCoordinates = (1420, 835);
    private static readonly (int, int) FirstStartButtonCoordinates = (1362, 314);
    private const int StartButtonWidth = 94;
    private const int StartButtonHeight = 38;
    private const int StartButtonSpacing = 47;
    private const int TradeSlots = 6;

    private Bitmap _captureBmp = null!;
    private Graphics _captureGraphic = null!;
    private IEntity _entity = null!;


    public TradeService(IInputService inputService)
    {
        _inputService = inputService;
    }

    public void StartTrading<TEntity>(TEntity entity) where TEntity : IEntity
    {
        _entity = entity;
        Console.Clear();
        Console.WriteLine("Trading started. Press escape during no trades available timer to stop");

        Thread.Sleep(2000);
        CollectTrades();
        RefreshTrades();

        _captureBmp = new Bitmap(Constants.Screen.Width, Constants.Screen.Height, PixelFormat.Format32bppArgb);
        _captureGraphic = Graphics.FromImage(_captureBmp);

        while (true)
        {
            CaptureScreen(Constants.Screen.Width, Constants.Screen.WindowLocation);

            if (TradesAvailable())
            {
                FindEntity();
            }
            else
            {
                var stop = ConsoleHelper.ShowCountdown("No trades available, refreshing in {0:00}", 10);
                if (stop)
                {
                    break;
                }
                CollectTrades();
            }

            RefreshTrades();
        }

        _captureGraphic.Dispose();
    }

    void CollectTrades()
    {
        _inputService.LeftClickAtPoint(CollectButtonCoordinates.Item1, CollectButtonCoordinates.Item2);
    }

    void RefreshTrades()
    {
        _inputService.LeftClickAtPoint(RefreshButtonCoordinates.Item1, RefreshButtonCoordinates.Item2);
    }

    void StartTrade(int tradeIndex)
    {
        var topLeftY = FirstStartButtonCoordinates.Item2 + (tradeIndex * (StartButtonSpacing + StartButtonHeight));

        var xCoord = FirstStartButtonCoordinates.Item1 + StartButtonWidth;
        var yCoord = topLeftY + (StartButtonHeight / 2);

        _inputService.LeftClickAtPoint(xCoord, yCoord);
    }

    void CaptureScreen(int screenWidth, int screenIndex)
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

    bool TradesAvailable()
    {
        var cloneRect = new Rectangle(FirstStartButtonCoordinates.Item1, FirstStartButtonCoordinates.Item2,
            StartButtonWidth, StartButtonHeight);
        var cloneBitmap = _captureBmp.Clone(cloneRect, _captureBmp.PixelFormat);

        for (var i = 0; i < cloneBitmap.Height; i++)
        {
            for (var j = 0; j < cloneBitmap.Width; j++)
            {
                var color = cloneBitmap.GetPixel(j, i);
                if (color == Constants.Colors.ClickableButton)
                {
                    return true;
                }
            }
        }

        return false;
    }

    void FindEntity()
    {
        for (var i = 0; i < TradeSlots; i++)
        {
            var offset = _entity.FirstTradeCoordinate.Item2 + (i * (_entity.TradeSpacing + _entity.Height));
            var cloneRect = new Rectangle(_entity.FirstTradeCoordinate.Item1, offset, _entity.Width, _entity.Height);

            var cloneBitmap = _captureBmp.Clone(cloneRect, _captureBmp.PixelFormat);
            var colorFound = false;

            for (var j = 0; j < cloneBitmap.Height; j++)
            {
                for (var k = 0; k < cloneBitmap.Width; k++)
                {
                    var color = cloneBitmap.GetPixel(k, j);
                    if (color == _entity.DistinguishingColor)
                    {
                        ProcessEntity(i);
                        colorFound = true;
                        break;
                    }
                }

                if (colorFound)
                {
                    break;
                }
            }
        }
    }

    void ProcessEntity(int tradeIndex)
    {
        var offset = FirstStartButtonCoordinates.Item2 + (tradeIndex * (StartButtonSpacing + StartButtonHeight));
        var cloneRect = new Rectangle(FirstStartButtonCoordinates.Item1, offset, StartButtonWidth, StartButtonHeight);
        var cloneBitmap = _captureBmp.Clone(cloneRect, _captureBmp.PixelFormat);

        var tradeAvailable = false;

        for (var j = 0; j < cloneBitmap.Height; j++)
        {
            for (var k = 0; k < cloneBitmap.Width; k++)
            {
                var color = cloneBitmap.GetPixel(k, j);
                if (color == Constants.Colors.ClickableButton)
                {
                    tradeAvailable = true;
                    break;
                }
            }

            if (tradeAvailable)
            {
                break;
            }
        }

        if (!tradeAvailable)
        {
            // Trade exists, but is already started
            return;
        }

        StartTrade(tradeIndex);
    }
}
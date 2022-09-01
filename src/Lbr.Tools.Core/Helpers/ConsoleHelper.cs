namespace Lbr.Tools.Core.Helpers;

public class ConsoleHelper
{
    public static bool ShowCountdown(string message, TimeSpan timeSpan)
    {
        while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape) && timeSpan.TotalSeconds != 0)
        {
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(message, timeSpan);
            Thread.Sleep(1000);
            timeSpan = timeSpan.Add(TimeSpan.FromSeconds(-1));
        }

        var currentLineCursor = Console.CursorTop;
        Console.SetCursorPosition(0, Console.CursorTop);
        Console.Write(new string(' ', Console.WindowWidth));
        Console.SetCursorPosition(0, currentLineCursor);

        // If i isn't 0 then we must have broken out
        return timeSpan.TotalSeconds != 0;
    }
}
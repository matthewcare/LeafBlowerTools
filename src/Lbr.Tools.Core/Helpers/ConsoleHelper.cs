namespace Lbr.Tools.Core.Helpers;

public class ConsoleHelper
{
    public static bool ShowCountdown(string message, int seconds)
    {
        var i = seconds;
        while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape) && i != 0)
        {
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(message, i);
            Thread.Sleep(1000);
            i--;
        }

        var currentLineCursor = Console.CursorTop;
        Console.SetCursorPosition(0, Console.CursorTop);
        Console.Write(new string(' ', Console.WindowWidth));
        Console.SetCursorPosition(0, currentLineCursor);

        // If i isn't 0 then we must have broken out
        return i != 0;
    }
}
namespace Lbr.Tools.Core.Constants;

public partial class Constants
{
    public class Screen
    {
        /// <summary>
        /// The screen which Leaf Blower is running on
        /// </summary>
        /// <remarks>
        /// 0 indexed
        /// </remarks>
        public static int WindowLocation { get; set; } = 0;
        public const int Width = 1920;
        public const int Height = 1024;
    }
}
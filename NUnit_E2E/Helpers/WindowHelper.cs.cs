namespace NUnit_E2E.Helpers
{
    internal static class WindowHelper
    {
        internal const string WindowSizeCommandFormat = "--window-size={0},{1}";
        internal const string WindowPositionCommandFormat = "--window-position={0},{1}";
        internal const string ManagementObjectSearcherQueryString = "SELECT * FROM Win32_VideoController";
        internal static int CurrentHorizontalResolution = 1280;
        internal static int CurrentVerticalResolution = 720;
        internal static int CurrentScreenX = 0;
        internal static int CurrentScreenY = 0;
        internal static string WindowSizeCommand
        {
            get => string.Format(WindowSizeCommandFormat, CurrentHorizontalResolution, CurrentVerticalResolution);
        }
        internal static string WindowPositionCommand
        {
            get => string.Format(WindowPositionCommandFormat, CurrentScreenX, CurrentScreenY);
        }

        public static (int Width, int Height, int X, int Y) GetScreenDimensionsAndPosition()
        {
            // If on windows platform check
            if(System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
            {
                
            }
            return (CurrentHorizontalResolution, CurrentVerticalResolution, CurrentScreenX, CurrentScreenY);
        }
    }
}

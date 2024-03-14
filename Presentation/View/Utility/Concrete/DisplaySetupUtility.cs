using Presentation.View.Utility.Abstract;
using System.Runtime.InteropServices;

namespace Presentation.View.Utility.Concrete;
internal class DisplaySetupUtility : IDisplaySetupUtilityService
{
    // Documentation: https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-deletemenu
    private const int MF_BYCOMMAND = 0x00000000;

    // Documentation: https://docs.microsoft.com/en-us/windows/win32/menurc/wm-syscommand
    public const int SC_SIZE = 0xF000;
    public const int SC_MINIMIZE = 0xF020;
    public const int SC_MAXIMIZE = 0xF030;

    // Documentation: https://learn.microsoft.com/en-us/windows/win32/winmsg/window-styles
    private const int GWL_STYLE = -16;
    private const int WS_MINIMIZEBOX = 0x00020000;
    private const int WS_MAXIMIZEBOX = 0x00010000;
    private const int WS_THICKFRAME = 0x00040000;

    // Documentation: https://docs.microsoft.com/en-us/dotnet/api/system.runtime.interopservices.dllimportattribute?view=netcore-3.1
    [DllImport("user32.dll")]
    // Documentation: https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-deletemenu
    public static extern int DeleteMenu(nint hMenu, int nPosition, int wFlags);

    [DllImport("user32.dll")]
    // Documentation: https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getsystemmenu
    private static extern nint GetSystemMenu(nint hWnd, bool bRevert);

    [DllImport("kernel32.dll", ExactSpelling = true)]
    // Documentation: https://docs.microsoft.com/en-us/windows/console/getconsolewindow
    private static extern nint GetConsoleWindow();

    [DllImport("user32.dll")]
    // Documentation: https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getwindowlonga
    private static extern int GetWindowLong(nint hWnd, int nIndex);

    [DllImport("user32.dll")]
    // Documentation: https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setwindowlonga
    private static extern int SetWindowLong(nint hWnd, int nIndex, int dwNewLong);
    public void DisableConsoleFrameSettings()
    {
        nint consoleWindow = GetConsoleWindow();
        int currentStyle = GetWindowLong(consoleWindow, GWL_STYLE);
        int newStyle = currentStyle & ~WS_MINIMIZEBOX & ~WS_MAXIMIZEBOX & ~WS_THICKFRAME;

        SetWindowLong(consoleWindow, GWL_STYLE, newStyle);                              // Disable minimize,maximize button, disabe resize cursor
        DeleteMenu(GetSystemMenu(consoleWindow, false), SC_SIZE, MF_BYCOMMAND);         // Disable resizing
        DeleteMenu(GetSystemMenu(consoleWindow, false), SC_MINIMIZE, MF_BYCOMMAND);     // Disable minimizing
        DeleteMenu(GetSystemMenu(consoleWindow, false), SC_MAXIMIZE, MF_BYCOMMAND);     // Disable maximizing  

        Console.BufferHeight = Console.WindowHeight;                                    // Disable scrollbar

        Console.CursorVisible = false;                                                  // Disable blinking cursor
    }
}

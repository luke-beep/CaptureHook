using System.Runtime.InteropServices;

namespace CaptureHook.Helpers;

internal partial class WindowHelper
{
    [LibraryImport("dwmapi.dll")]
    public static partial int DwmSetWindowAttribute(nint hwnd, int attr, ref int attrValue, int attrSize);

    private const int DwmwaWindowCornerPreference = 33;
    private const int DwmwcpRound = 2;

    internal static void SetWindowRounded(nint handle)
    {
        var value = DwmwcpRound;
        DwmSetWindowAttribute(handle, DwmwaWindowCornerPreference, ref value, sizeof(int));
    }
}
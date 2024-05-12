using System.Runtime.InteropServices;

namespace CaptureHook.Enums;

[StructLayout(LayoutKind.Sequential)]
internal struct WindowCompositionAttributeData
{
    public CompositionAttribute Attribute;
    public IntPtr Data;
    public int SizeOfData;
}
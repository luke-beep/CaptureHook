using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace CaptureHook;

public partial class KeyHookingService
{
    [LibraryImport("user32.dll", EntryPoint = "SetWindowsHookExW", SetLastError = true)]
    private static partial IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

    [LibraryImport("user32.dll", EntryPoint = "UnhookWindowsHookEx", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool UnhookWindowsHookEx(IntPtr hhk);

    [LibraryImport("user32.dll", EntryPoint = "CallNextHookEx", SetLastError = true)]
    private static partial IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [LibraryImport("kernel32.dll", EntryPoint = "GetModuleHandleW", SetLastError = true,
        StringMarshalling = StringMarshalling.Utf16)]
    private static partial IntPtr GetModuleHandle(string lpModuleName);

    [LibraryImport("user32.dll")]
    private static partial short GetAsyncKeyState(Keys vKey);

    private const int WhKeyboardLl = 13;
    private const int WmKeydown = 0x0100;
    private const int WmKeyup = 0x0101;
    private static IntPtr _hookId = IntPtr.Zero;

    public KeyHookingService() => _hookId = SetHook(HookCallback);

    private static IntPtr SetHook(LowLevelKeyboardProc proc)
    {
        using var curProcess = Process.GetCurrentProcess();
        using var curModule = curProcess.MainModule;
        if (curModule != null) return SetWindowsHookEx(WhKeyboardLl, proc, GetModuleHandle(curModule.ModuleName), 0);
        throw new Win32Exception(Marshal.GetLastWin32Error());
    }

    private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

    private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode < 0) return CallNextHookEx(_hookId, nCode, wParam, lParam);
        var key = (Keys)Marshal.ReadInt32(lParam);

        switch (wParam)
        {
            case WmKeydown:
                Form1.UpdateLabel(BuildKeyCombination(key));
                break;
        }

        return CallNextHookEx(_hookId, nCode, wParam, lParam);
    }

    private static string BuildKeyCombination(Keys key) =>
        !IsModifierKey(key)
            ? GetModifierKeysString() + GetKeyName(key)
            : GetModifierKeysString().TrimEnd(' ', '+');

    private static bool IsModifierKey(Keys key) =>
        key is Keys.LWin or Keys.RWin or Keys.ControlKey or Keys.ShiftKey or Keys.Menu or Keys.LControlKey
            or Keys.RControlKey or Keys.LShiftKey or Keys.RShiftKey or Keys.LMenu or Keys.RMenu or Keys.Shift
            or Keys.Control or Keys.Alt or Keys.Modifiers or Keys.None or Keys.KeyCode;

    private static string GetModifierKeysString()
    {
        var sb = new StringBuilder();
        if (GetAsyncKeyState(Keys.LWin) != 0 || GetAsyncKeyState(Keys.RWin) != 0) sb.Append("Win + ");
        if (GetAsyncKeyState(Keys.ControlKey) != 0 || GetAsyncKeyState(Keys.LControlKey) != 0 ||
            GetAsyncKeyState(Keys.RControlKey) != 0) sb.Append("Ctrl + ");
        if (GetAsyncKeyState(Keys.ShiftKey) != 0 || GetAsyncKeyState(Keys.LShiftKey) != 0 ||
            GetAsyncKeyState(Keys.RShiftKey) != 0) sb.Append("Shift + ");
        if (GetAsyncKeyState(Keys.Menu) != 0 || GetAsyncKeyState(Keys.LMenu) != 0 || GetAsyncKeyState(Keys.RMenu) != 0)
            sb.Append("Alt + ");
        return sb.ToString();
    }

    private static string GetKeyName(Keys key) =>
        key switch
        {
            Keys.Oemcomma => ",",
            Keys.OemPeriod => ".",
            Keys.OemQuestion => "?",
            Keys.KeyCode => "KEY",
            Keys.Modifiers => "MODIFIERS",
            Keys.None => "NONE",
            Keys.LButton => "Left Button",
            Keys.RButton => "Right Button",
            Keys.Cancel => "Cancel",
            Keys.MButton => "Middle Button",
            Keys.XButton1 => "XButton1",
            Keys.XButton2 => "XButton2",
            Keys.Back => "Backspace",
            Keys.Tab => "Tab",
            Keys.LineFeed => "Line Feed",
            Keys.Clear => "Clear",
            Keys.Return => "Enter",
            Keys.ShiftKey => "Shift",
            Keys.ControlKey => "Ctrl",
            Keys.Menu => "Alt",
            Keys.Pause => "Pause",
            Keys.Capital => "Caps Lock",
            Keys.KanaMode => "Kana Mode",
            Keys.JunjaMode => "Junja Mode",
            Keys.FinalMode => "Final Mode",
            Keys.HanjaMode => "Hanja Mode",
            Keys.Escape => "Esc",
            Keys.IMEConvert => "IME Convert",
            Keys.IMENonconvert => "IME Nonconvert",
            Keys.IMEAccept => "IME Accept",
            Keys.IMEModeChange => "IME Mode Change",
            Keys.Space => "Space",
            Keys.Prior => "Page Up",
            Keys.Next => "Page Down",
            Keys.End => "End",
            Keys.Home => "Home",
            Keys.Left => "Left Arrow",
            Keys.Up => "Up Arrow",
            Keys.Right => "Right Arrow",
            Keys.Down => "Down Arrow",
            Keys.Select => "Select",
            Keys.Print => "Print",
            Keys.Execute => "Execute",
            Keys.Snapshot => "Print Screen",
            Keys.Insert => "Insert",
            Keys.Delete => "Delete",
            Keys.Help => "Help",
            Keys.D0 => "0",
            Keys.D1 => "1",
            Keys.D2 => "2",
            Keys.D3 => "3",
            Keys.D4 => "4",
            Keys.D5 => "5",
            Keys.D6 => "6",
            Keys.D7 => "7",
            Keys.D8 => "8",
            Keys.D9 => "9",
            Keys.A => "A",
            Keys.B => "B",
            Keys.C => "C",
            Keys.D => "D",
            Keys.E => "E",
            Keys.F => "F",
            Keys.G => "G",
            Keys.H => "H",
            Keys.I => "I",
            Keys.J => "J",
            Keys.K => "K",
            Keys.L => "L",
            Keys.M => "M",
            Keys.N => "N",
            Keys.O => "O",
            Keys.P => "P",
            Keys.Q => "Q",
            Keys.R => "R",
            Keys.S => "S",
            Keys.T => "T",
            Keys.U => "U",
            Keys.V => "V",
            Keys.W => "W",
            Keys.X => "X",
            Keys.Y => "Y",
            Keys.Z => "Z",
            Keys.LWin => "Win",
            Keys.RWin => "Win",
            Keys.Apps => "Applications",
            Keys.Sleep => "Sleep",
            Keys.NumPad0 => "0",
            Keys.NumPad1 => "1",
            Keys.NumPad2 => "2",
            Keys.NumPad3 => "3",
            Keys.NumPad4 => "4",
            Keys.NumPad5 => "5",
            Keys.NumPad6 => "6",
            Keys.NumPad7 => "7",
            Keys.NumPad8 => "8",
            Keys.NumPad9 => "9",
            Keys.Multiply => "Multiply",
            Keys.Add => "Add",
            Keys.Separator => "Separator",
            Keys.Subtract => "Subtract",
            Keys.Decimal => "Decimal",
            Keys.Divide => "Divide",
            Keys.F1 => "F1",
            Keys.F2 => "F2",
            Keys.F3 => "F3",
            Keys.F4 => "F4",
            Keys.F5 => "F5",
            Keys.F6 => "F6",
            Keys.F7 => "F7",
            Keys.F8 => "F8",
            Keys.F9 => "F9",
            Keys.F10 => "F10",
            Keys.F11 => "F11",
            Keys.F12 => "F12",
            Keys.F13 => "F13",
            Keys.F14 => "F14",
            Keys.F15 => "F15",
            Keys.F16 => "F16",
            Keys.F17 => "F17",
            Keys.F18 => "F18",
            Keys.F19 => "F19",
            Keys.F20 => "F20",
            Keys.F21 => "F21",
            Keys.F22 => "F22",
            Keys.F23 => "F23",
            Keys.F24 => "F24",
            Keys.NumLock => "Num Lock",
            Keys.Scroll => "Scroll Lock",
            Keys.LShiftKey => "Left Shift",
            Keys.RShiftKey => "Right Shift",
            Keys.LControlKey => "Left Ctrl",
            Keys.RControlKey => "Right Ctrl",
            Keys.LMenu => "Left Alt",
            Keys.RMenu => "Right Alt",
            Keys.BrowserBack => "Browser Back",
            Keys.BrowserForward => "Browser Forward",
            Keys.BrowserRefresh => "Browser Refresh",
            Keys.BrowserStop => "Browser Stop",
            Keys.BrowserSearch => "Browser Search",
            Keys.BrowserFavorites => "Browser Favorites",
            Keys.BrowserHome => "Browser Home",
            Keys.VolumeMute => "Volume Mute",
            Keys.VolumeDown => "Volume Down",
            Keys.VolumeUp => "Volume Up",
            Keys.MediaNextTrack => "Next Track",
            Keys.MediaPreviousTrack => "Previous Track",
            Keys.MediaStop => "Stop",
            Keys.MediaPlayPause => "Play/Pause",
            Keys.LaunchMail => "Launch Mail",
            Keys.SelectMedia => "Select Media",
            Keys.LaunchApplication1 => "Launch Application 1",
            Keys.LaunchApplication2 => "Launch Application 2",
            Keys.OemSemicolon => ";",
            Keys.Oemplus => "+",
            Keys.OemMinus => "-",
            Keys.Oemtilde => "~",
            Keys.OemOpenBrackets => "[",
            Keys.OemPipe => "|",
            Keys.OemCloseBrackets => "]",
            Keys.OemQuotes => "\"",
            Keys.Oem8 => "Oem 8",
            Keys.OemBackslash => "\\",
            Keys.ProcessKey => "Process Key",
            Keys.Packet => "Packet",
            Keys.Attn => "Attn",
            Keys.Crsel => "CrSel",
            Keys.Exsel => "ExSel",
            Keys.EraseEof => "Erase EOF",
            Keys.Play => "Play",
            Keys.Zoom => "Zoom",
            Keys.NoName => "No Name",
            Keys.Pa1 => "PA1",
            Keys.OemClear => "Clear",
            Keys.Shift => "Shift",
            Keys.Control => "Control",
            Keys.Alt => "Alt",
            _ => key.ToString()
        };

    internal static void Dispose() => UnhookWindowsHookEx(_hookId);
}
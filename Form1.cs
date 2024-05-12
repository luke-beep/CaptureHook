using CaptureHook.Helpers;

namespace CaptureHook;

public partial class Form1 : Form
{
    private readonly KeyHookingService _keyHooking = new();

    public Form1()
    {
        InitializeComponent();
        Load += (_, _) =>
        {
            BlurHelper.EnableBlur(Handle.ToInt32());
            WindowHelper.SetWindowRounded(Handle);
            Location = new Point(10, 10);
        };

        ShowInTaskbar = false;
        NotifyIcon notifyIcon = new()
        {
            Icon = Icon,
            Visible = true,
            Text = @"CaptureHook",
            ContextMenuStrip = new ContextMenuStrip()
        };
        notifyIcon.ContextMenuStrip.Items.Add("Exit", null, (_, _) => Application.Exit());
    }

    private void ChangePosition(object sender, EventArgs e)
    {
        if (ModifierKeys != Keys.Shift) return;
        if (Screen.PrimaryScreen != null)
            Location = Location switch
            {
                { X: 10, Y: 10 } => new Point(10, Screen.PrimaryScreen.WorkingArea.Height - Height - 10),
                { X: 10 } => new Point(Screen.PrimaryScreen.WorkingArea.Width - Width - 10,
                    Screen.PrimaryScreen.WorkingArea.Height - Height - 10),
                { Y: 10 } => new Point(10, 10),
                { } => new Point(Screen.PrimaryScreen.WorkingArea.Width - Width - 10, 10)
            };
    }

    internal static void UpdateLabel(string text)
    {
        if (key.InvokeRequired)
            key.Invoke(new MethodInvoker(() => key.Text = text));
        else
            key.Text = text;
    }
}
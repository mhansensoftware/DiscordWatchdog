using DiscordWatchdog;
using DiscordWatchdog.Checks;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using Vanara.PInvoke;

internal class Program
{
    private static Task backGroundTask;
    private static CancellationTokenSource cancellationTokenSource;

    [STAThread]
    private static void Main(string[] args)
    {
        try
        {
            cancellationTokenSource = new CancellationTokenSource();

            DiscordPathGetter discordPathGetter = new DiscordPathGetter();
            Watchdog watchdog = new Watchdog(discordPathGetter);
            AppTray appTray = new AppTray();
            appTray.Tray_OnEnabledChanged += value => watchdog.Enabled = value;
            appTray.Tray_OnExit += AppTray_Exit;

            backGroundTask = new Task(() => {
                while (true)
                {
                    DiscordPresenseChecker.Check(discordPathGetter);
                    watchdog.Tick();

                    Task.Delay(1000).Wait();
                }
            }, cancellationTokenSource.Token);

            backGroundTask.Start();

            Application.Run();
        }
        catch (ExitException)
        {
            cancellationTokenSource.Cancel();   
            Application.Exit();
            return;
        }
        catch (Exception ex)
        {
            User32.MB_RESULT result = User32.MessageBox(HWND.NULL, ex.Message + "\nTry again?", "Error", User32.MB_FLAGS.MB_YESNO | User32.MB_FLAGS.MB_ICONERROR);
            switch (result)
            {
                case User32.MB_RESULT.IDYES:
                    Main(args);
                    break;

                default:
                    return;
            }
        }
    }

    private static void AppTray_Exit()
    {
        throw new ExitException();
    }
}
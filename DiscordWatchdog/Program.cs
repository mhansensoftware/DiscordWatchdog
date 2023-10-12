using DiscordWatchdog;
using DiscordWatchdog.Checks;
using Vanara.PInvoke;

internal class Program
{
    private static Task? backGroundTask;
    private static CancellationTokenSource? cancellationTokenSource;

    [STAThread]
    private static void Main(string[] args)
    {
        try
        {
            cancellationTokenSource = new CancellationTokenSource();

            DiscordPathGetter discordPathGetter = new();
            Watchdog watchdog = new(discordPathGetter);
            AppTray appTray = new();
            appTray.Tray_OnEnabledChanged += value => watchdog.Enabled = value;
            appTray.Tray_OnExit += AppTray_Exit;

            backGroundTask = new Task(() =>
            {
                while (true)
                {
                    DiscordPresenceChecker.Check(discordPathGetter);
                    watchdog.Tick();

                    Task.Delay(1000).Wait();
                }
            }, cancellationTokenSource.Token);

            backGroundTask.Start();

            Application.Run();
        }
        catch (ExitException)
        {
            cancellationTokenSource?.Cancel();
            Application.Exit();
            return;
        }
        catch (Exception ex)
        {
            User32.MB_RESULT result = User32.MessageBox(HWND.NULL, $"{ex.Message}\nTry again?", "Error", User32.MB_FLAGS.MB_YESNO | User32.MB_FLAGS.MB_ICONERROR);
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
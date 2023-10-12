using System;
using System.Diagnostics;
using System.Linq;

namespace DiscordWatchdog
{
    internal class Watchdog
    {

        private bool wasDiscordOpen;
        private readonly DiscordPathGetter getter;

        public Watchdog(DiscordPathGetter getter)
        {
            this.getter = getter;
        }

        public bool Enabled { get; set; } = true;

        public void Tick()
        {
            if (!Enabled) return;

            // Get the target process
            Process[] processes = GetDiscordProcesses();
            if (processes.Length > 0)
            {
                wasDiscordOpen = true;
                return;
            }

            if (!wasDiscordOpen)
            {
                return;
            }

            ProcessStartInfo startInfo = new()
            {
                FileName = getter.GetDiscordExecutablePath(),
                Arguments = "--processStart Discord.exe",
                WorkingDirectory = getter.GetDiscordWorkingDirectory(),
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            Process? process = Process.Start(startInfo);

            if (process == null) return;

            process.WaitForExit();
            Console.WriteLine(process.StandardOutput.ReadToEnd());
            Console.WriteLine(process.StandardError.ReadToEnd());

        }

        private static Process[] GetDiscordProcesses()
        {
            return Process.GetProcessesByName("Discord");
        }
    }
}

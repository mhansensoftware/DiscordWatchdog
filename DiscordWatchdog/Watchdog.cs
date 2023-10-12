using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiscordWatchdog
{
    internal class Watchdog
    {

        bool wasDiscordOpen = false;
        DiscordPathGetter getter;

        public Watchdog(DiscordPathGetter getter)
        {
            this.getter = getter;
        }

        public bool Enabled { get; set; } = true;

        public void Tick()
        {
            if (!Enabled) return;

            // Get the target process
            Process[] processes = GetDiscordProcessses();
            if (processes.Length > 0)
            {
                wasDiscordOpen = true;
                return;
            }

            if (!wasDiscordOpen)
            {
                return;
            }

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = getter.GetDiscordExecutablePath();
            startInfo.Arguments = "--processStart Discord.exe";
            startInfo.WorkingDirectory = getter.GetDiscordWorkingDirectory();
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            Process process = Process.Start(startInfo);

            process.WaitForExit();
            Console.WriteLine(process.StandardOutput.ReadToEnd());
            Console.WriteLine(process.StandardError.ReadToEnd());
            
        }

        private Process[] GetDiscordProcessses()
        {
            return Process.GetProcessesByName("Discord");
        }
    }
}

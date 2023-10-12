using System;
using System.Linq;

namespace DiscordWatchdog
{
    internal class DiscordPathGetter
    {
        readonly string discordDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Discord");

        public string GetDiscordDirectory()
        {
            return discordDir;
        }

        public string GetDiscordExecutablePath()
        {
            return Path.Combine(GetDiscordDirectory(), "Update.exe");
        }
        
        public string GetDiscordWorkingDirectory()
        {
            string dir = Directory.GetDirectories(discordDir, "app*").First();
            return dir;
        }
    }
}

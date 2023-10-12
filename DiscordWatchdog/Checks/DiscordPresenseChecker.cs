using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordWatchdog.Checks
{
    internal static class DiscordPresenseChecker
    {
        public static void Check(DiscordPathGetter getter)
        {
            if (getter == null) throw new ArgumentNullException(nameof(getter));
            string dir = getter.GetDiscordDirectory();

            if (!Directory.Exists(dir))
                throw new DirectoryNotFoundException(dir);

            string exe = getter.GetDiscordExecutablePath();

            if (!File.Exists(exe))
                throw new FileNotFoundException(exe);
        }
    }
}

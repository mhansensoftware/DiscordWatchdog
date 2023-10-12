using System;
using System.Linq;

namespace DiscordWatchdog
{
    internal class AppTray
    {
        readonly NotifyIcon _notifyIcon;
        private bool _enabled = true;

        public Action<bool>? Tray_OnEnabledChanged { get; set; }
        public Action? Tray_OnExit { get; set; }

        public AppTray()
        {
            ContextMenuStrip contextMenu = new()
            {
                Visible = true,
                Enabled = true
            };
            contextMenu.Items.Add("Turn off", null, Tray_ToggleOnOffOnClick);
            contextMenu.Items.Add("Exit", null, Tray_Exit);

            NotifyIcon notifyIcon = new()
            {
                Icon = Images.DiscordWatchDogTrayIcon,
                Text = "Discord Watchdog",
                Visible = true,
                ContextMenuStrip = contextMenu,
            };
            _notifyIcon = notifyIcon;
        }

        private void Tray_ToggleOnOffOnClick(object? sender, EventArgs e)
        {
            if (_enabled)
            {
                _notifyIcon.ContextMenuStrip.Items[0].Text = "Turn on";
                _notifyIcon.Icon = Images.DiscordWatchDogTrayIconOff;
                _enabled = false;
                Tray_OnEnabledChanged?.Invoke(_enabled);
                return;
            }

            _notifyIcon.ContextMenuStrip.Items[0].Text = "Turn off";
            _notifyIcon.Icon = Images.DiscordWatchDogTrayIcon;
            _enabled = true;
            Tray_OnEnabledChanged?.Invoke(_enabled);
        }

        private void Tray_Exit(object? sender, EventArgs e)
        {
            Tray_OnExit?.Invoke();
        }
    }
}

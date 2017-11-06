using Lykke.blue.Api.Core.Settings.ServiceSettings;
using Lykke.blue.Api.Core.Settings.SlackNotifications;

namespace Lykke.blue.Api.Core.Settings
{
    public class AppSettings
    {
        public ApiSettings BlueApi { get; set; }
        public SlackNotificationsSettings SlackNotifications { get; set; }
    }
}

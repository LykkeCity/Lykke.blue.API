using Lykke.Service.Api.Core.Settings.ServiceSettings;
using Lykke.Service.Api.Core.Settings.SlackNotifications;

namespace Lykke.Service.Api.Core.Settings
{
    public class AppSettings
    {
        public ApiSettings BlueApi { get; set; }
        public SlackNotificationsSettings SlackNotifications { get; set; }
    }
}

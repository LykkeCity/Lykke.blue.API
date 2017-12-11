using System.Threading.Tasks;

namespace Lykke.blue.Api.Core.Settings.LykkeSettings
{
    public interface ILykkeGlobalSettingsRepositry
    {
        Task<LykkeGlobalSettings> GetAsync();
    }
}

using AutoMapper;
using AzureStorage;
using Lykke.blue.Api.Core.Settings.LykkeSettings;
using System.Threading.Tasks;

namespace Lykke.blue.Api.AzureRepositories.LykkeSettings
{
    public class LykkeGlobalSettingsRepository : ILykkeGlobalSettingsRepositry
    {
        private readonly INoSQLTableStorage<LykkeGlobalSettingsEntity> _tableStorage;

        public LykkeGlobalSettingsRepository(INoSQLTableStorage<LykkeGlobalSettingsEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        async Task<LykkeGlobalSettings> ILykkeGlobalSettingsRepositry.GetAsync()
        {
            var partitionKey = LykkeGlobalSettingsEntity.GeneratePartitionKey();
            var rowKey = LykkeGlobalSettingsEntity.GenerateRowKey();
            var settings = await _tableStorage.GetDataAsync(partitionKey, rowKey);
            return Mapper.Map<LykkeGlobalSettings>(settings);
        }
    }
}

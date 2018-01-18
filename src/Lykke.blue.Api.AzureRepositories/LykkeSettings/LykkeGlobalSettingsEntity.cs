using Microsoft.WindowsAzure.Storage.Table;
namespace Lykke.blue.Api.AzureRepositories.LykkeSettings
{
    public class LykkeGlobalSettingsEntity : TableEntity
    {
        // ReSharper disable once UnusedMember.Global
        // used by AutoMapper
        public bool IsOnMaintenance { get; set; }

        public static string GeneratePartitionKey()
        {
            return "Setup";
        }

        public static string GenerateRowKey()
        {
            return "AppSettings";
        }
    }
}

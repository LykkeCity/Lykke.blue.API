namespace Lykke.blue.Api.Core.Settings.ServiceSettings
{
    public class ApiSettings
    {
        public DbSettings Db { get; set; }
        public ServiceSettings Services { get; set; }
        public BlueApiSettings BlueApiSettings { get; set; }
    }
}

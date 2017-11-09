namespace Lykke.blue.Api.Core.Settings.ServiceSettings
{
    public class ServiceSettings
    {
        public string ClientAccountServiceUrl { get; set; }
        public string RegistrationServiceUrl { get; set; }
        public string SessionServiceUrl { get; set; }
        public string PledgesServiceUrl { get; set; }
        public InspireStreamService InspireStreamService { get; set; }
    }

    public class InspireStreamService
    {
        public string ServiceUrl { get; set; }
        public int RequestTimeout { get; set; }
    }
}

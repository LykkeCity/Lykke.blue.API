namespace Lykke.blue.Api.Responses
{
    public class CreatePledgeResponse
    {
        public string Id { get; set; }
        public string ClientId { get; set; }
        public int CO2Footprint { get; set; }
        public int ClimatePositiveValue { get; set; }
    }
}

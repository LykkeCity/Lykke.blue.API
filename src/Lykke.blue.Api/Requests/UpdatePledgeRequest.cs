using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Lykke.blue.Api.Requests
{
    public class UpdatePledgeRequest
    {
        [IgnoreDataMember]
        public string Id { get; set; }
        [IgnoreDataMember]
        public string ClientId { get; set; }
        [Required]
        public int CO2Footprint { get; set; }
        [Required]
        public int ClimatePositiveValue { get; set; }
    }
}

// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Service.Api.AutorestClient.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class UpdatePledgeRequest
    {
        /// <summary>
        /// Initializes a new instance of the UpdatePledgeRequest class.
        /// </summary>
        public UpdatePledgeRequest()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the UpdatePledgeRequest class.
        /// </summary>
        public UpdatePledgeRequest(int cO2Footprint, int climatePositiveValue)
        {
            CO2Footprint = cO2Footprint;
            ClimatePositiveValue = climatePositiveValue;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "CO2Footprint")]
        public int CO2Footprint { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "ClimatePositiveValue")]
        public int ClimatePositiveValue { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            //Nothing to validate
        }
    }
}
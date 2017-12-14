// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Service.Api.AutorestClient.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class ClaimRefLinkModel
    {
        /// <summary>
        /// Initializes a new instance of the ClaimRefLinkModel class.
        /// </summary>
        public ClaimRefLinkModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the ClaimRefLinkModel class.
        /// </summary>
        public ClaimRefLinkModel(bool isNewClient, string referalLinkId = default(string), string referalLinkUrl = default(string))
        {
            ReferalLinkId = referalLinkId;
            ReferalLinkUrl = referalLinkUrl;
            IsNewClient = isNewClient;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "ReferalLinkId")]
        public string ReferalLinkId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "ReferalLinkUrl")]
        public string ReferalLinkUrl { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "IsNewClient")]
        public bool IsNewClient { get; set; }

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
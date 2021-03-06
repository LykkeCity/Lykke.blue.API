// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Service.Api.AutorestClient.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class GetReferralLinkResponse
    {
        /// <summary>
        /// Initializes a new instance of the GetReferralLinkResponse class.
        /// </summary>
        public GetReferralLinkResponse()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the GetReferralLinkResponse class.
        /// </summary>
        public GetReferralLinkResponse(double amount, string id = default(string), string url = default(string), System.DateTime? expirationDate = default(System.DateTime?), string senderClientId = default(string), string asset = default(string), string state = default(string), string type = default(string), string senderOffchainTransferId = default(string))
        {
            Id = id;
            Url = url;
            ExpirationDate = expirationDate;
            SenderClientId = senderClientId;
            Asset = asset;
            State = state;
            Amount = amount;
            Type = type;
            SenderOffchainTransferId = senderOffchainTransferId;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Id")]
        public string Id { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Url")]
        public string Url { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "ExpirationDate")]
        public System.DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "SenderClientId")]
        public string SenderClientId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Asset")]
        public string Asset { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "State")]
        public string State { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Amount")]
        public double Amount { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Type")]
        public string Type { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "SenderOffchainTransferId")]
        public string SenderOffchainTransferId { get; set; }

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

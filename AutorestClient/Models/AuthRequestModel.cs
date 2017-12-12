// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Service.Api.AutorestClient.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class AuthRequestModel
    {
        /// <summary>
        /// Initializes a new instance of the AuthRequestModel class.
        /// </summary>
        public AuthRequestModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the AuthRequestModel class.
        /// </summary>
        public AuthRequestModel(string email = default(string), string password = default(string), string clientInfo = default(string), string partnerId = default(string))
        {
            Email = email;
            Password = password;
            ClientInfo = clientInfo;
            PartnerId = partnerId;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Email")]
        public string Email { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Password")]
        public string Password { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "ClientInfo")]
        public string ClientInfo { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "PartnerId")]
        public string PartnerId { get; set; }

    }
}

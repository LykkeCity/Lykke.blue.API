// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Service.Api.AutorestClient.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class TweetsRequestModel
    {
        /// <summary>
        /// Initializes a new instance of the TweetsRequestModel class.
        /// </summary>
        public TweetsRequestModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the TweetsRequestModel class.
        /// </summary>
        public TweetsRequestModel(bool isExtendedSearch, int maxResult, System.DateTime untilDate, int pageSize, int pageNumber, string accountEmail = default(string), string searchQuery = default(string))
        {
            IsExtendedSearch = isExtendedSearch;
            AccountEmail = accountEmail;
            SearchQuery = searchQuery;
            MaxResult = maxResult;
            UntilDate = untilDate;
            PageSize = pageSize;
            PageNumber = pageNumber;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "IsExtendedSearch")]
        public bool IsExtendedSearch { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "AccountEmail")]
        public string AccountEmail { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "SearchQuery")]
        public string SearchQuery { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "MaxResult")]
        public int MaxResult { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "UntilDate")]
        public System.DateTime UntilDate { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "PageSize")]
        public int PageSize { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "PageNumber")]
        public int PageNumber { get; set; }

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

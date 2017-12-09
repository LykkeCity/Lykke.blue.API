using Lykke.blue.Service.ReferralLinks.Client.AutorestClient.Models;
using System.Runtime.Serialization;

namespace Lykke.blue.Api.Models.RefLinksModels
{
    public class ClaimRefLinkModel : RefLinksBaseRequestModel<ClaimReferralLinkRequest>
    {
        public string ReferalLinkUrl { get; set; }
        public bool IsNewClient { get; set; }

        [IgnoreDataMember]
        public override string LogMessage { get => "Invitation link claimed"; }

        public override ClaimReferralLinkRequest ConvertToServiceModel()
        {
            return new ClaimReferralLinkRequest
            {
                IsNewClient = this.IsNewClient,
                ReferalLinkUrl = this.ReferalLinkUrl
            };
        }
    }
}

using Lykke.blue.Service.ReferralLinks.Client.AutorestClient.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Lykke.blue.Api.Models.RefLinksModels
{
    public class ClaimRefLinkModel : RefLinksBaseRequestModel<ClaimReferralLinkRequest>
    {
        public string RecipientClientId { get; set; }
        public string ReferalLinkId { get; set; }
        public string ReferalLinkUrl { get; set; }
        public bool IsNewClient { get; set; }

        [IgnoreDataMember]
        public override string LogMessage { get => "Invitation link claimed"; }

        public override ClaimReferralLinkRequest ConvertToServiceModel()
        {
            return new ClaimReferralLinkRequest
            {
                IsNewClient = this.IsNewClient,
                RecipientClientId = this.RecipientClientId,
                ReferalLinkId = this.ReferalLinkId,
                ReferalLinkUrl = this.ReferalLinkUrl
            };
        }
    }
}

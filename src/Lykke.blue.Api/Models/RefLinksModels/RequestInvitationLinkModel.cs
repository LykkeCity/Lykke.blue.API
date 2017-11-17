using Lykke.blue.Service.ReferralLinks.Client.AutorestClient.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Lykke.blue.Api.Models.RefLinksModels
{
    public class RequestInvitationLinkModel : RefLinksBaseRequestModel<InvitationReferralLinkRequest>
    {
        public string SenderClientId { get; set; }

        [IgnoreDataMember]
        public override string LogMessage { get => "Invitation link requested"; }

        public override InvitationReferralLinkRequest ConvertToServiceModel()
        {
            return new InvitationReferralLinkRequest { SenderClientId = this.SenderClientId };
        }
    }
}

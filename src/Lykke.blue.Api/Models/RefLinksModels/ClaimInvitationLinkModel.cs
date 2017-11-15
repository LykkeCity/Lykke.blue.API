using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.blue.Api.Models.RefLinksModels
{
    public class ClaimInvitationLinkModel
    {
        public string RecipientClientId { get; set; }
        public string ReferalLinkId { get; set; }
        public string ReferalLinkUrl { get; set; }
        public bool IsNewClient { get; set; }
    }
}

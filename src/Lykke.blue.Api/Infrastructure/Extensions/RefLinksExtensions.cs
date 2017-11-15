using Lykke.blue.Api.Models.RefLinksModels;
using Lykke.blue.Service.ReferralLinks.Client.AutorestClient.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.blue.Api.Infrastructure.Extensions
{
    public static class RefLinksExtensions
    {
        public static InvitationReferralLinkRequest ConvertToServiceModel(this RequestInvitationLinkModel src)
        {
            return new InvitationReferralLinkRequest { SenderClientId = src.SenderClientId };
        }
        public static ClaimReferralLinkRequest ConvertToServiceModel(this ClaimInvitationLinkModel src)
        {
            return new ClaimReferralLinkRequest {
                IsNewClient = src.IsNewClient,
                RecipientClientId = src.RecipientClientId,
                ReferalLinkId = src.ReferalLinkId,
                ReferalLinkUrl = src.ReferalLinkUrl
            };
        }
        public static GiftCoinsReferralLinkRequest ConvertToServiceModel(this RequestGiftCoinsLinkModel src)
        {
            return new GiftCoinsReferralLinkRequest
            {
                SenderClientId = src.SenderClientId,
                Asset = src.Asset,
                Amount = src.Amount,
            };
        }
    }
}

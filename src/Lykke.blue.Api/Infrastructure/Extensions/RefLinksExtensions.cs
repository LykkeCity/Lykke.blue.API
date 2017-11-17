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
        public static ClaimReferralLinkRequest ConvertToServiceModel(this ClaimRefLinkModel src)
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
        public static TransferToLykkeWallet ConvertToServiceModel(this TransferToLykkeWalletModel src)
        {
            return new TransferToLykkeWallet
            {
                ClientId = src.ClientId,
                ReferralLinkId = src.ReferralLinkId,
                PrevTempPrivateKey = src.PrevTempPrivateKey
            };
        }
        public static OffchainChannelProcessModel ConvertToServiceModel(this ProcessChannelModel src)
        {
            return new OffchainChannelProcessModel
            {
                TransferId = src.TransferId,
                ClientId = src.ClientId,
                SignedChannelTransaction = src.SignedChannelTransaction
            };
        }
        public static OffchainFinalizeModel ConvertToServiceModel(this FinalizeTransferModel src)
        {
            return new OffchainFinalizeModel
            {
                TransferId = src.TransferId,
                ClientRevokePubKey = src.ClientRevokePubKey,
                ClientRevokeEncryptedPrivateKey = src.ClientRevokeEncryptedPrivateKey,
                SignedTransferTransaction = src.SignedTransferTransaction,
                ClientId = src.ClientId,
                RefLinkId = src.RefLinkId
            };
        }



        



    }
}

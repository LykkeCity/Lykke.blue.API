using Lykke.blue.Service.ReferralLinks.Client.AutorestClient.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Lykke.blue.Api.Models.RefLinksModels
{
    public class RequestGiftCoinsLinkModel : RefLinksBaseRequestModel<GiftCoinsReferralLinkRequest>
    {
        public string Asset { get; set; }
        public double Amount { get; set; }

        [IgnoreDataMember]
        public override string LogMessage { get => "GiftCoins link requested"; }

        public override GiftCoinsReferralLinkRequest ConvertToServiceModel()
        {
            return new GiftCoinsReferralLinkRequest
            {
                Amount = this.Amount,
                Asset = this.Asset,
            };
        }
    }
}

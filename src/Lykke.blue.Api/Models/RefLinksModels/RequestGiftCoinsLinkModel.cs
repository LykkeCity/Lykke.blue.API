using Lykke.blue.Service.ReferralLinks.Client.AutorestClient.Models;
using System.Runtime.Serialization;

namespace Lykke.blue.Api.Models.RefLinksModels
{
    // ReSharper disable once UnusedMember.Global
    //reserver for v2
    public class RequestGiftCoinsLinkModel : RefLinksBaseRequestModel<GiftCoinsReferralLinkRequest>
    {
        public string Asset { get; set; }
        public double Amount { get; set; }

        [IgnoreDataMember]
        public override string LogMessage => "GiftCoins link requested";

        public override GiftCoinsReferralLinkRequest ConvertToServiceModel()
        {
            return new GiftCoinsReferralLinkRequest
            {
                Amount = Amount,
                Asset = Asset,
            };
        }
    }
}

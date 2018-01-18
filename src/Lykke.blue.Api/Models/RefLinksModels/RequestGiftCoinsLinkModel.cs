using Lykke.blue.Service.ReferralLinks.Client.AutorestClient.Models;
using System.Runtime.Serialization;

namespace Lykke.blue.Api.Models.RefLinksModels
{
    // ReSharper disable once UnusedMember.Global
    //reserver for v2
    public class RequestGiftCoinsLinkModel : RefLinksBaseRequestModel<GiftCoinRequest>
    {
        public string Asset { get; set; }
        public double Amount { get; set; }

        [IgnoreDataMember]
        public override string LogMessage => "GiftCoins link requested";

        public override GiftCoinRequest ConvertToServiceModel()
        {
            return new GiftCoinRequest
            {
                Amount = Amount,
                Asset = Asset,
            };
        }
    }
}

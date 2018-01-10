using Lykke.blue.Service.ReferralLinks.Client.AutorestClient.Models;
using System.Collections.Generic;
using System.Runtime.Serialization;
// ReSharper disable MemberCanBePrivate.Global

namespace Lykke.blue.Api.Models.RefLinksModels
{
    public class GiftCoinRequestGroupModel : RefLinksBaseRequestModel<GiftCoinRequestGroup>
    {
        public double?[] AmountForEachLink { get; set; }
        public string Asset { get; set; }

        [IgnoreDataMember]
        public override string LogMessage => "Mass generate GiftCoin links requested";

        public override GiftCoinRequestGroup ConvertToServiceModel()
        {
            return new GiftCoinRequestGroup
            {
                Asset = Asset,
                AmountForEachLink = new List<double?>(AmountForEachLink)
            };
        }
    }
}




using Lykke.blue.Service.ReferralLinks.Client.AutorestClient.Models;
using System.Runtime.Serialization;

namespace Lykke.blue.Api.Models.RefLinksModels
{
    // ReSharper disable once UnusedMember.Global
    //reserver for v2
    public class OffchainGetChannelKeyModel : RefLinksBaseRequestModel<OffchainGetChannelKeyRequest>
    {
        public string Asset { get; set; }

        [IgnoreDataMember]
        public override string LogMessage => "Offchain channel key requested";

        public override OffchainGetChannelKeyRequest ConvertToServiceModel()
        {
            return new OffchainGetChannelKeyRequest { Asset = Asset};
        }
    }
}

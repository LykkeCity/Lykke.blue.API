using Lykke.blue.Service.ReferralLinks.Client.AutorestClient.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Lykke.blue.Api.Models.RefLinksModels
{
    public class OffchainGetChannelKeyModel : RefLinksBaseRequestModel<OffchainGetChannelKeyRequest>
    {
        public string Asset { get; set; }
        public string ClientId { get; set; }

        [IgnoreDataMember]
        public override string LogMessage { get => "Offchain channel key requested"; }

        public override OffchainGetChannelKeyRequest ConvertToServiceModel()
        {
            return new OffchainGetChannelKeyRequest { Asset = this.Asset, ClientId = this.ClientId };
        }
    }
}

using Lykke.blue.Service.ReferralLinks.Client.AutorestClient.Models;
using System.Runtime.Serialization;

namespace Lykke.blue.Api.Models.RefLinksModels
{
    // ReSharper disable once UnusedMember.Global
    //reserver for v2
    public class ProcessChannelModel : RefLinksBaseRequestModel<OffchainChannelProcessModel>
    {
        public string TransferId { get; set; }
        public string SignedChannelTransaction { get; set; }

        [IgnoreDataMember]
        public override string LogMessage => "Offchain process channel";

        public override OffchainChannelProcessModel ConvertToServiceModel()
        {
            return new OffchainChannelProcessModel { SignedChannelTransaction = SignedChannelTransaction, TransferId = TransferId };
        }
    }
}

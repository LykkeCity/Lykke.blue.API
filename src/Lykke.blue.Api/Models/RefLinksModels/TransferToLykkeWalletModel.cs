using Lykke.blue.Service.ReferralLinks.Client.AutorestClient.Models;
using System.Runtime.Serialization;

namespace Lykke.blue.Api.Models.RefLinksModels
{
    // ReSharper disable once UnusedMember.Global
    // reserver for v2
    public class TransferToLykkeWalletModel : RefLinksBaseRequestModel<TransferToLykkeWallet>
    {
        public string ReferralLinkId { get; set; }
        public string PrevTempPrivateKey { get; set; }

        [IgnoreDataMember]
        public override string LogMessage => "Offchain transfer to Lykke wallet";

        public override TransferToLykkeWallet ConvertToServiceModel()
        {
            return new TransferToLykkeWallet { PrevTempPrivateKey = PrevTempPrivateKey, ReferralLinkId = ReferralLinkId };

        }
    }
}

using Lykke.blue.Service.ReferralLinks.Client.AutorestClient.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Lykke.blue.Api.Models.RefLinksModels
{
    public class TransferToLykkeWalletModel : RefLinksBaseRequestModel<TransferToLykkeWallet>
    {
        public string ReferralLinkId { get; set; }
        public string PrevTempPrivateKey { get; set; }

        [IgnoreDataMember]
        public override string LogMessage { get => "Offchain transfer to Lykke wallet"; }
        
        public override TransferToLykkeWallet ConvertToServiceModel()
        {
            return new TransferToLykkeWallet { PrevTempPrivateKey = this.PrevTempPrivateKey, ReferralLinkId = this.ReferralLinkId };

        }
    }
}

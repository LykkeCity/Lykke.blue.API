using Lykke.blue.Service.ReferralLinks.Client.AutorestClient.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Lykke.blue.Api.Models.RefLinksModels
{
    public class ProcessChannelModel : RefLinksBaseRequestModel<OffchainChannelProcessModel>
    {
        public string TransferId { get; set; }
        public string SignedChannelTransaction { get; set; }

        [IgnoreDataMember]
        public override string LogMessage { get => "Offchain process channel"; }

        public override OffchainChannelProcessModel ConvertToServiceModel()
        {
            return new OffchainChannelProcessModel { SignedChannelTransaction = this.SignedChannelTransaction, TransferId = this.TransferId };
        }
    }
}

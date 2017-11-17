using Lykke.blue.Service.ReferralLinks.Client.AutorestClient.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Lykke.blue.Api.Models.RefLinksModels
{
    public class RefLinkStatisticsModel : RefLinksBaseRequestModel<RefLinkStatisticsRequest>
    {
        public string SenderClientId { get; set; }

        [IgnoreDataMember]
        public override string LogMessage { get => "Statistics requested"; }

        public override RefLinkStatisticsRequest ConvertToServiceModel()
        {
            return new RefLinkStatisticsRequest { SenderClientId = this.SenderClientId };
        }
    }
}

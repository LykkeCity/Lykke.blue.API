using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.blue.Api.Models.RefLinksModels
{
    public class RequestGiftCoinsLinkModel
    {
        public string SenderClientId { get; set; }
        public string Asset { get; set; }
        public double Amount { get; set; }
    }
}

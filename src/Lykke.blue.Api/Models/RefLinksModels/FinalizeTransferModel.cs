﻿using Lykke.blue.Service.ReferralLinks.Client.AutorestClient.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.blue.Api.Models.RefLinksModels
{
    public class FinalizeTransferModel : RefLinksBaseRequestModel<OffchainFinalizeModel>
    {
        public string TransferId { get; set; }
        public string ClientRevokePubKey { get; set; }
        public string ClientRevokeEncryptedPrivateKey { get; set; }
        public string SignedTransferTransaction { get; set; }
        public string ClientId { get; set; }
        public string RefLinkId { get; set; }

        public override string LogMessage { get => "Finalize offchain transfer"; }

        public override OffchainFinalizeModel ConvertToServiceModel()
        {
            return new OffchainFinalizeModel
            {
                TransferId = this.TransferId,
                ClientRevokePubKey = this.ClientRevokePubKey,
                ClientRevokeEncryptedPrivateKey = this.ClientRevokeEncryptedPrivateKey,
                SignedTransferTransaction = this.SignedTransferTransaction,
                ClientId = this.ClientId,
                RefLinkId = this.RefLinkId
            };
        }
    }
}

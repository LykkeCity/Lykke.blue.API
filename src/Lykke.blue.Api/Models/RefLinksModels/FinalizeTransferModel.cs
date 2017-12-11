using Lykke.blue.Service.ReferralLinks.Client.AutorestClient.Models;
using System.Runtime.Serialization;

namespace Lykke.blue.Api.Models.RefLinksModels
{
    // ReSharper disable once UnusedMember.Global
    // reserver for v2
    public class FinalizeTransferModel : RefLinksBaseRequestModel<OffchainFinalizeModel>
    {
        public string TransferId { get; set; }
        public string ClientRevokePubKey { get; set; }
        public string ClientRevokeEncryptedPrivateKey { get; set; }
        public string SignedTransferTransaction { get; set; }
        public string RefLinkId { get; set; }

        [IgnoreDataMember]
        public override string LogMessage => "Finalize offchain transfer";

        public override OffchainFinalizeModel ConvertToServiceModel()
        {
            return new OffchainFinalizeModel
            {
                TransferId = TransferId,
                ClientRevokePubKey = ClientRevokePubKey,
                ClientRevokeEncryptedPrivateKey = ClientRevokeEncryptedPrivateKey,
                SignedTransferTransaction = SignedTransferTransaction,
                RefLinkId = RefLinkId
            };
        }
    }
}

// ReSharper disable UnusedMember.Global
// DTO object
namespace Lykke.blue.Api.Responses.ReferralLinks.Offchain
{
    public class RefLinksTransferOffchainResponse
    {
        public string TransferId { get; set; }
        public string TransactionHex { get; set; }
        public EnOffchainOperationResult OperationResult { get; set; }
    }
}

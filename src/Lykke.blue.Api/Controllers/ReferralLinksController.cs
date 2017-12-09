using Common;
using Common.Log;
using Lykke.blue.Api.Infrastructure;
using Lykke.blue.Api.Infrastructure.Extensions;
using Lykke.blue.Api.Models.RefLinksModels;
using Lykke.blue.Service.ReferralLinks.Client.AutorestClient;
using Lykke.blue.Service.ReferralLinks.Client.AutorestClient.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Rest;
using Swashbuckle.SwaggerGen.Annotations;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Lykke.blue.Api.Controllers
{
    [Route("api/referralLinks")]
    [Authorize]
    public class RefLinksController : BluApiBaseController
    {
        private readonly ILykkeReferralLinksService _referralLinksService;
        private readonly IRequestContext _requestContext;

        public RefLinksController(ILog log, 
            ILykkeReferralLinksService refSrv,
            IRequestContext requestContext) : base (log)
        {
            _referralLinksService = refSrv;
            _requestContext = requestContext;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        [SwaggerOperation("GetReferralLinkById")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetReferralLinkById(string id)
        {
            var result = await ExecuteRefLinksMethod((p) => _referralLinksService.GetReferralLinkByIdWithHttpMessagesAsync(p), id, "Get Referral Link By Id");
            return result;
        }

        [HttpGet("url/{url}")]
        [AllowAnonymous]
        [SwaggerOperation("GetReferralLinkByUrl")]
        [ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(GetReferralLinkResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetReferralLinkByUrl(string url)
        {
            var decoded = WebUtility.UrlDecode(url);

            var result = await ExecuteRefLinksMethod((p) => _referralLinksService.GetReferralLinkByUrlWithHttpMessagesAsync(p), decoded, "Get Referral Link By Url");
            return result;
        }
        
        [HttpPost("invitation")]
        [SwaggerOperation("RequestInvitationReferralLink")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> RequestInvitationReferralLink()
        {
            var result = await ExecuteRefLinksMethod((p) => _referralLinksService.RequestInvitationReferralLinkWithHttpMessagesAsync(p), new InvitationReferralLinkRequest { SenderClientId = _requestContext.ClientId }, "Invitation Link requested");
            return result;
        }

        [HttpPut("invitation/{refLinkId}/claim")]
        [SwaggerOperation("ClaimInvitationLink")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> ClaimInvitationLink(string refLinkId, [FromBody] ClaimRefLinkModel request)
        {
            var serviceRequest = request.ConvertToServiceModel();
            serviceRequest.RecipientClientId = _requestContext.ClientId;

            var result = await ExecuteRefLinksMethod((p) => _referralLinksService.ClaimInvitationLinkWithHttpMessagesAsync(request.ReferalLinkId, serviceRequest), serviceRequest, request.LogMessage);
            return result;
        }

        /// <summary>
        /// Request giftcoins referral link.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        //[HttpPost("giftCoinLinks")] - for v2
        //[SwaggerOperation("RequestGiftCoinsReferralLink")]
        //[ProducesResponseType((int)HttpStatusCode.BadRequest)]
        //[ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        private async Task<IActionResult> RequestGiftCoinsReferralLink([FromBody] RequestGiftCoinsLinkModel request)
        {
            return NotFound("Reserved for version 2");

            //var serviceRequest = request.ConvertToServiceModel();
            //serviceRequest.SenderClientId = _requestContext.ClientId;

            //var result = await ExecuteRefLinksMethod((p) => _referralLinksService.RequestGiftCoinsReferralLinkWithHttpMessagesAsync(p), serviceRequest, request.LogMessage);
            //return result;
        }

        /// <summary>
        /// Claim giftcoins referral link.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        //[HttpPost("claim/giftCoins")] - for v2
        //[SwaggerOperation("ClaimGiftCoins")]
        //[ProducesResponseType((int)HttpStatusCode.BadRequest)]
        //[ProducesResponseType((int)HttpStatusCode.NotFound)]
        //[ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        private async Task<IActionResult> ClaimGiftCoins([FromBody] ClaimRefLinkModel request)
        {
            return NotFound("Reserved for version 2");

            //var serviceRequest = request.ConvertToServiceModel();
            //serviceRequest.RecipientClientId = _requestContext.ClientId;

            //var result = await ExecuteRefLinksMethod((p) => _referralLinksService.ClaimGiftCoinsWithHttpMessagesAsync(p), serviceRequest, request.LogMessage);
            //return result;            
        }

        [HttpGet("statistics")]
        [SwaggerOperation("GetReferralLinksStatisticsBySenderId")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(GetReferralLinksStatisticsBySenderIdResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetReferralLinksStatisticsBySenderId()
        {
            var result = await ExecuteRefLinksMethod((p) => _referralLinksService.GetReferralLinksStatisticsBySenderIdWithHttpMessagesAsync(p), new RefLinkStatisticsRequest { SenderClientId = _requestContext.ClientId }, "Reflink statistics requested");
            return result;
        }

        /// <summary>
        /// Get offchain ChannelKey for transfer.  
        /// </summary>
        /// <returns></returns>
        //[HttpPost("channelKey")] --reserved for version 2
        //[SwaggerOperation("GetChannelKey")]
        //[ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        private async Task<IActionResult> GetChannelKey([FromQuery] string asset)
        {
            return NotFound("Reserved for version 2");

            //var serviceRequest = new OffchainGetChannelKeyRequest { Asset = asset, ClientId = _requestContext.ClientId };

            //var result = await ExecuteRefLinksMethod((p) => _referralLinksService.GetChannelKeyWithHttpMessagesAsync(p), serviceRequest, "Offchain channel key requested");
            //return result;         
        }

        /// <summary>
        /// Create offchain transfer to Lykke wallet
        /// </summary>
        /// <returns></returns>
        //[HttpPost("transferToLykkeWallet")] --reserved for version 2
        //[SwaggerOperation("TransferToLykkeWallet")]
        //[ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        //[ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        //[ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
        private async Task<IActionResult> TransferToLykkeWallet([FromBody] TransferToLykkeWalletModel request)
        {
            return NotFound("Reserved for version 2");

            //var serviceRequest = request.ConvertToServiceModel();
            //serviceRequest.ClientId = _requestContext.ClientId;

            //var result = await ExecuteRefLinksMethod((p) => _referralLinksService.TransferToLykkeWalletMethodWithHttpMessagesAsync(p), serviceRequest, request.LogMessage);
            //return result;           
        }

        /// <summary>
        /// Process offchain channel
        /// </summary>
        /// <returns></returns>
        //[HttpPost("processChannel")] --reserved for version 2
        //[SwaggerOperation("ProcessChannel")]
        //[ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        //[ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        //[ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
        private async Task<IActionResult> ProcessChannel([FromBody] ProcessChannelModel request)
        {
            return NotFound("Reserved for version 2");

            //var serviceRequest = request.ConvertToServiceModel();
            //serviceRequest.ClientId = _requestContext.ClientId;

            //var result = await ExecuteRefLinksMethod((p) => _referralLinksService.ProcessChannelWithHttpMessagesAsync(p), serviceRequest, request.LogMessage);
            //return result;           
        }

        /// <summary>
        /// Process offchain channel
        /// </summary>
        /// <returns></returns>
        //[HttpPost("finalizeRefLinkTransfer")] --reserved for version 2
        //[SwaggerOperation("FinalizeRefLinkTransfer")]
        //[ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        //[ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        //[ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
        private async Task<IActionResult> Finalize([FromBody] FinalizeTransferModel request)
        {
            return NotFound("Reserved for version 2");

            //var serviceRequest = request.ConvertToServiceModel();
            //serviceRequest.ClientId = _requestContext.ClientId;

            //var result = await ExecuteRefLinksMethod((p) => _referralLinksService.FinalizeRefLinkTransferWithHttpMessagesAsync(p), serviceRequest, request.LogMessage);
            //return result;         
        }

        private async Task<IActionResult> ExecuteRefLinksMethod<U, T>(Func<U, Task<HttpOperationResponse<T>>> method, U request, string logMessage)
        {
            var response = await method(request);

            var error = await response.CheckClientResponseForErrors();
            if (error != null)
            {
                return error;
            }

            var result = response.Body;

            await LogInfo(request, ControllerContext, $"{logMessage}: {new { response.Response.StatusCode, result }.ToJson()}");

            return StatusCode((int)response.Response.StatusCode, result);

        }
    }
}

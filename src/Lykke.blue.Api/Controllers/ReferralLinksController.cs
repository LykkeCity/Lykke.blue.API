using AutoMapper;
using Common;
using Common.Log;
using Lykke.blue.Api.Core.Filters;
using Lykke.blue.Api.Infrastructure;
using Lykke.blue.Api.Infrastructure.Extensions;
using Lykke.blue.Api.Models.RefLinksModels;
using Lykke.blue.Api.Responses.ReferralLinks.Offchain;
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
    [ServiceFilter(typeof(DisableOnMaintenanceFilter))]
    public class ReferralLinksController : BluApiBaseController
    {
        private readonly ILykkeReferralLinksService _referralLinksService;
        private readonly IRequestContext _requestContext;

        public ReferralLinksController(ILog log, 
            ILykkeReferralLinksService refSrv,
            IRequestContext requestContext) : base (log)
        {
            _referralLinksService = refSrv;
            _requestContext = requestContext;
        }

        /// <summary>
        /// Get referral link by id.
        /// </summary>
        /// <param name="id">Id of the referral link we want to get.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Get referral link by url.
        /// </summary>
        /// <param name="url">Url of the referral link we want to get.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Request invitation referral link.
        /// </summary>
        /// <returns></returns>
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

            var result = await ExecuteRefLinksMethod((p) => _referralLinksService.ClaimInvitationLinkWithHttpMessagesAsync(refLinkId, serviceRequest), serviceRequest, request.LogMessage);
            return result;
        }

        /// <summary>
        /// Get referral links statistics by sender client id.
        /// </summary>
        /// <returns></returns>
        [HttpGet("statistics")]
        [SwaggerOperation("GetReferralLinksStatisticsBySenderId")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(GetReferralLinksStatisticsBySenderIdResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetReferralLinksStatisticsBySenderId()
        {
            var result = await ExecuteRefLinksMethod((p) => _referralLinksService.GetReferralLinksStatisticsBySenderIdWithHttpMessagesAsync(p), _requestContext.ClientId, "Reflink statistics requested");
            return result;
        }


        /// <summary>
        /// Generate Gift Coins referral link
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("giftCoins")]
        [SwaggerOperation("RequestGiftCoinsReferralLink")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> RequestGiftCoinsReferralLink([FromBody] RequestGiftCoinsLinkModel request)
        {
            var serviceRequest = request.ConvertToServiceModel();
            serviceRequest.SenderClientId = _requestContext.ClientId;

            var result = await ExecuteRefLinksMethod((p) => _referralLinksService.RequestGiftCoinsReferralLinkWithHttpMessagesAsync(p), serviceRequest, request.LogMessage);
            return result;
        }

        /// <summary>
        /// Claim gift coins referral link
        /// </summary>
        /// <param name="refLinkId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("giftCoins/{refLinkId}/claim")]
        [SwaggerOperation("ClaimGiftCoins")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ClaimGiftCoins(string refLinkId, [FromBody] ClaimRefLinkModel request)
        {
            var serviceRequest = request.ConvertToServiceModel();
            serviceRequest.RecipientClientId = _requestContext.ClientId;

            var result = await ExecuteRefLinksMethod((p) => _referralLinksService.ClaimGiftCoinsWithHttpMessagesAsync(refLinkId, serviceRequest), serviceRequest, request.LogMessage);
            return result;
        }

        /// <summary>
        /// Get offchain ChannelKey for transfer.  
        /// </summary>
        /// <returns></returns>
        [HttpGet("channel/key/{asset}")]
        [SwaggerOperation("GetChannelKey")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetChannelKey(string asset)
        {
            var res = await _referralLinksService.GetChannelKeyAsync(asset, _requestContext.ClientId);
            return Ok(res.Key);
        }

        /// <summary>
        /// Create offchain transfer to Lykke wallet
        /// </summary>
        /// <returns></returns>
        [HttpPost("transfer/hotWallet")]
        [SwaggerOperation("TransferToLykkeHotWallet")]
        [ProducesResponseType(typeof(RefLinksTransferOffchainResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> TransferToLykkeWallet([FromBody] TransferToLykkeWalletModel request)
        {
            var serviceRequest = request.ConvertToServiceModel();
            serviceRequest.ClientId = _requestContext.ClientId;

            var response = await _referralLinksService.TransferToLykkeHotWalletWithHttpMessagesAsync(serviceRequest);
            return await ProcessAndReturnOffchainResult(response, request);
        }

        /// <summary>
        /// Process offchain channel
        /// </summary>
        /// <returns></returns>
        [HttpPost("channel/process")]
        [SwaggerOperation("ProcessChannel")]
        [ProducesResponseType(typeof(RefLinksTransferOffchainResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> ProcessChannel([FromBody] ProcessChannelModel request)
        {
            var serviceRequest = request.ConvertToServiceModel();
            serviceRequest.ClientId = _requestContext.ClientId;

            var response = await _referralLinksService.ProcessChannelWithHttpMessagesAsync(serviceRequest);
            return await ProcessAndReturnOffchainResult(response, request);
        }

        /// <summary>
        /// Process offchain channel
        /// </summary>
        /// <returns></returns>
        [HttpPost("transfer/finalize")]
        [SwaggerOperation("FinalizeRefLinkTransfer")]
        [ProducesResponseType(typeof(RefLinksTransferOffchainResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Finalize([FromBody] FinalizeTransferModel request)
        {
            var serviceRequest = request.ConvertToServiceModel();
            serviceRequest.ClientId = _requestContext.ClientId;

            var response = await _referralLinksService.FinalizeRefLinkTransferWithHttpMessagesAsync(serviceRequest);
            return await ProcessAndReturnOffchainResult(response, request);
        }

        private async Task<IActionResult> ProcessAndReturnOffchainResult<T, TU>(HttpOperationResponse<T> offchainResult, TU request)
        {

            var error = await offchainResult.CheckClientResponseForErrors();
            if (error != null)
            {
                return error;
            }

            var result = offchainResult.Body as OffchainTradeRespModel;

            if (result!=null)
            {
                return Ok(Mapper.Map<OffchainTradeRespModel>(result));
            }

            await LogError(request, ControllerContext, $"Unexpected response type from service client: {offchainResult.Body.GetType()}. Client response: {offchainResult.ToJson()} ");
            return StatusCode((int)HttpStatusCode.InternalServerError);

        }

        private async Task<IActionResult> ExecuteRefLinksMethod<TU, T>(Func<TU, Task<HttpOperationResponse<T>>> method, TU request, string logMessage)
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

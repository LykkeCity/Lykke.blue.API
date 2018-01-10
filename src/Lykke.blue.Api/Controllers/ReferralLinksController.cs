using Common;
using Common.Log;
using Lykke.blue.Api.Core.Filters;
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
    [ServiceFilter(typeof(DisableOnMaintenanceFilter))]
    public class ReferralLinksController : BluApiBaseController
    {
        private readonly ILykkeBlueServiceReferralLinks _referralLinksService;
        private readonly IRequestContext _requestContext;

        public ReferralLinksController(ILog log, 
            ILykkeBlueServiceReferralLinks refSrv,
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
            var result = await ExecuteRefLinksMethod(p => _referralLinksService.GetReferralLinkByIdWithHttpMessagesAsync(p), id, "Get Referral Link By Id");
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

            var result = await ExecuteRefLinksMethod(p => _referralLinksService.GetReferralLinkByUrlWithHttpMessagesAsync(p), decoded, "Get Referral Link By Url");
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
            var result = await ExecuteRefLinksMethod(p => _referralLinksService.RequestInvitationReferralLinkWithHttpMessagesAsync(p), new InvitationReferralLinkRequest { SenderClientId = _requestContext.ClientId }, "Invitation Link requested");
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

            var result = await ExecuteRefLinksMethod(p => _referralLinksService.ClaimInvitationLinkWithHttpMessagesAsync(refLinkId, serviceRequest), serviceRequest, request.LogMessage);
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
            var result = await ExecuteRefLinksMethod(p => _referralLinksService.GetReferralLinksStatisticsBySenderIdWithHttpMessagesAsync(p), _requestContext.ClientId, "Reflink statistics requested");
            return result;
        }

        /// <summary>
        /// Get all gift coin links per sender.
        /// </summary>
        /// <returns></returns>
        [HttpGet("giftCoins/sender/{senderId}")]
        [SwaggerOperation("GetGiftCoinReferralLinkBySenderId")]
        [ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(GetReferralLinkResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetGiftCoinReferralLinkBySenderId(string senderId)
        {
            var result = await ExecuteRefLinksMethod(p => _referralLinksService.GetGroupReferralLinkBySenderIdWithHttpMessagesAsync(p), senderId, "Reflink statistics requested");
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

            var result = await ExecuteRefLinksMethod(p => _referralLinksService.RequestGiftCoinsReferralLinkWithHttpMessagesAsync(p), serviceRequest, request.LogMessage);
            return result;
        }

        /// <summary>
        /// Mass Generate Gift Coin referral links
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("giftCoins/group")]
        [SwaggerOperation("GroupGenerateGiftCoinLinks")]
        [ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(RequestRefLinkResponse), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> GroupGenerateGiftCoinLinks([FromBody] GiftCoinRequestGroupModel request)
        {
            var serviceRequest = request.ConvertToServiceModel();
            serviceRequest.SenderClientId = _requestContext.ClientId;

            var result = await ExecuteRefLinksMethod(p => _referralLinksService.GroupGenerateGiftCoinLinksWithHttpMessagesAsync(p), serviceRequest, request.LogMessage);
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

            var result = await ExecuteRefLinksMethod(p => _referralLinksService.ClaimGiftCoinsWithHttpMessagesAsync(refLinkId, serviceRequest), serviceRequest, request.LogMessage);
            return result;
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

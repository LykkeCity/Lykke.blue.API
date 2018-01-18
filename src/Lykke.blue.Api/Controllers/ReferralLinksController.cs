using Common.Log;
using Lykke.blue.Api.Core.Filters;
using Lykke.blue.Api.Infrastructure;
using Lykke.blue.Api.Models.RefLinksModels;
using Lykke.blue.Service.ReferralLinks.Client;
using Lykke.blue.Service.ReferralLinks.Client.AutorestClient.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Lykke.blue.Api.Controllers
{
    [Route("api/referralLinks")]
    [Authorize]
    [ServiceFilter(typeof(DisableOnMaintenanceFilter))]
    public class ReferralLinksController : BluApiBaseController
    {
        private readonly IReferralLinksClient _referralLinksService;
        private readonly IRequestContext _requestContext;

        public ReferralLinksController(ILog log,
            IReferralLinksClient refSrv,
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
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetReferralLinkById(string id)
        {
            try
            {
                var refLink = await _referralLinksService.GetReferralLinkAsync(id);
                if (refLink == null) return NotFound();
                return Ok(refLink);
            }
            catch (Exception e)
            {
                await LogError(id, ControllerContext, e.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Get referral link by url.
        /// </summary>
        /// <param name="url">Url of the referral link we want to get.</param>
        /// <returns></returns>
        [HttpGet("url/{url}")]
        [AllowAnonymous]
        [SwaggerOperation("GetReferralLinkByUrl")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetReferralLinkByUrl(string url)
        {
            try
            {
                var decoded = WebUtility.UrlDecode(url);

                var refLink = await _referralLinksService.GetReferralLinkByUrlAsync(decoded);
                if (refLink == null) return NotFound();
                return Ok(refLink);
            }
            catch (Exception e)
            {
                await LogError(url, ControllerContext, e.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Request invitation referral link.
        /// </summary>
        /// <returns></returns>
        [HttpPost("invitation")]
        [SwaggerOperation("RequestInvitationReferralLink")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> RequestInvitationReferralLink()
        {
            try
            {
                var refLink = await _referralLinksService.RequestInvitationReferralLinkAsync(new InvitationReferralLinkRequest { SenderClientId = _requestContext.ClientId });
                return refLink;
            }
            catch (Exception e)
            {
                await LogError(_requestContext.ClientId, ControllerContext, e.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut("invitation/{refLinkId}/claim")]
        [SwaggerOperation("ClaimInvitationLink")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ClaimInvitationLink(string refLinkId, [FromBody] ClaimRefLinkModel request)
        {
            try
            {
                var serviceRequest = request.ConvertToServiceModel();
                serviceRequest.RecipientClientId = _requestContext.ClientId;

                var refLink = await _referralLinksService.ClaimInvitationLinkAsync(refLinkId, serviceRequest);
                return refLink;
            }
            catch (Exception e)
            {
                await LogError(_requestContext.ClientId, ControllerContext, e.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Get referral links statistics by sender client id.
        /// </summary>
        /// <returns></returns>
        [HttpGet("statistics")]
        [SwaggerOperation("GetReferralLinksStatisticsBySenderId")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)] 
        public async Task<IActionResult> GetReferralLinksStatisticsBySenderId()
        {
            try
            {
                var statistics = await _referralLinksService.GetReferralLinksStatisticsBySenderIdAsync(_requestContext.ClientId);
                if (statistics == null) return NotFound();
                return Ok(statistics); 

            }
            catch (Exception e)
            {
                await LogError(_requestContext.ClientId, ControllerContext, e.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }


        /// <summary>
        /// Get all gift coin links per sender.
        /// </summary>
        /// <returns></returns>
        [HttpGet("giftCoins/sender/{senderId}")]
        [SwaggerOperation("GetGiftCoinReferralLinkBySenderId")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<string>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetGiftCoinReferralLinkBySenderId(string senderId)
        {
            try
            {
                var refLinks = await _referralLinksService.GetGiftCoinReferralLinksAsync(senderId);
                if (refLinks == null) return NotFound();
                return Ok(refLinks);
            }
            catch (Exception e)
            {
                await LogError(senderId, ControllerContext, e.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }


        /// <summary>
        /// Generate Gift Coins referral link
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("giftCoins")]
        [SwaggerOperation("RequestGiftCoinsReferralLink")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> RequestGiftCoinsReferralLink([FromBody] RequestGiftCoinsLinkModel request)
        {
            try
            {
                var serviceRequest = request.ConvertToServiceModel();
                serviceRequest.SenderClientId = _requestContext.ClientId;

                var refLink = await _referralLinksService.RequestGiftCoinsReferralLinkAsync(serviceRequest);
                return refLink;
            }
            catch (Exception e)
            {
                await LogError(request, ControllerContext, e.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Mass Generate Gift Coin referral links
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("giftCoins/group")]
        [SwaggerOperation("GroupGenerateGiftCoinLinks")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> GroupGenerateGiftCoinLinks([FromBody] GiftCoinRequestGroupModel request)
        {
            try
            {
                var serviceRequest = request.ConvertToServiceModel();
                serviceRequest.SenderClientId = _requestContext.ClientId;

                var refLinks = await _referralLinksService.GroupGenerateGiftCoinLinksAsync(serviceRequest);
                return refLinks;
            }
            catch (Exception e)
            {
                await LogError(request, ControllerContext, e.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
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
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ClaimGiftCoins(string refLinkId, [FromBody] ClaimRefLinkModel request)
        {
            try
            {
                var serviceRequest = request.ConvertToServiceModel();
                serviceRequest.RecipientClientId = _requestContext.ClientId;

                var result = await _referralLinksService.ClaimGiftCoinsAsync(refLinkId, serviceRequest);
                return result;
            }
            catch (Exception e)
            {
                await LogError(request, ControllerContext, e.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}

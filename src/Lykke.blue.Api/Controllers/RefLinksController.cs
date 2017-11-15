using Common.Log;
using Microsoft.AspNetCore.Mvc;
using System;
using Swashbuckle.SwaggerGen.Annotations;
using System.Net;
using System.Threading.Tasks;
using Lykke.blue.Api.Models.RefLinksModels;
using Lykke.blue.Service.ReferralLinks.Client.AutorestClient;
using Lykke.blue.Api.Infrastructure.Extensions;

namespace Lykke.blue.Api.Controllers
{
    [Route("api/refLinks")]
    public class RefLinksController : BluApiBaseController
    {
        private readonly ILog _log;
        private readonly ILykkeReferralLinksService _referralLinksService;

        public RefLinksController(ILog log, ILykkeReferralLinksService refSrv) : base (log)
        {
            _log = log;
            _referralLinksService = refSrv;
        }

        /// <summary>
        /// Request invitation referral link.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("request/invitationLink")]
        [SwaggerOperation("RequestInvitationReferralLink")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> RequestInvitationReferralLink([FromBody] RequestInvitationLinkModel request)
        {
            var response = await _referralLinksService.RequestInvitationReferralLinkWithHttpMessagesAsync(request.ConvertToServiceModel());

            var error = await response.CheckClientResponseForErrors();
            if(error != null)
            {
                return error;
            }

            var result = response.Body;

            await LogInfo(request, ControllerContext, $"Invitation link requested: {result}. {response.GetStatusCodeAndMessage()}");

            return Created(uri: $"api/referralLinks/{result}", value: result);
        }


        [HttpPost("claim/invitationLink")]
        [SwaggerOperation("ClaimInvitationLink")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> ClaimInvitationLink([FromBody] ClaimInvitationLinkModel request)
        {
            var response = await _referralLinksService.ClaimInvitationLinkWithHttpMessagesAsync (request.ConvertToServiceModel());

            var error = await response.CheckClientResponseForErrors();
            if (error != null)
            {
                return error;
            }

            var result = response.Body;

            await LogInfo(request, ControllerContext, $"Invitation link claimed: {result}. {response.GetStatusCodeAndMessage()}");

            return Ok(result);
        }

        /// <summary>
        /// Request giftcoins referral link.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("request/giftCoinslLink")]
        [SwaggerOperation("RequestGiftCoinsReferralLink")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> RequestGiftCoinsReferralLink([FromBody] RequestGiftCoinsLinkModel request)
        {
            var response = await _referralLinksService.RequestGiftCoinsReferralLinkWithHttpMessagesAsync(request.ConvertToServiceModel());

            var error = await response.CheckClientResponseForErrors();
            if (error != null)
            {
                return error;
            }
            //create separate methods for getting code and msg from httpResponse, return StatusCode(httpCode, msg) from controller
            var result = response.Body;

            await LogInfo(request, ControllerContext, $"Invitation link requested: {result}. {response.GetStatusCodeAndMessage()}");

            return StatusCode(1,"");   //create separate methods for getting code and msg from httpResponse, return StatusCode(httpCode, msg) from controller
        }
    }
}

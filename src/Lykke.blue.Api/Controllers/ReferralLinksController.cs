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
        private readonly ILykkeReferralLinksService _referralLinksService;
        private readonly IRequestContext _requestContext;

        public ReferralLinksController(ILog log, 
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

            var result = await ExecuteRefLinksMethod((p) => _referralLinksService.ClaimInvitationLinkWithHttpMessagesAsync(refLinkId, serviceRequest), serviceRequest, request.LogMessage);
            return result;
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

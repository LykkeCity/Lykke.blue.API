﻿using Common.Log;
using Microsoft.AspNetCore.Mvc;
using System;
using Swashbuckle.SwaggerGen.Annotations;
using System.Net;
using System.Threading.Tasks;
using Lykke.blue.Api.Models.RefLinksModels;
using Lykke.blue.Service.ReferralLinks.Client.AutorestClient;
using Lykke.blue.Api.Infrastructure.Extensions;
using Common;
using Microsoft.Rest;
using Lykke.blue.Service.ReferralLinks.Client.AutorestClient.Models;
using Lykke.blue.Service.ReferralLinks.Client;
using Microsoft.AspNetCore.Authorization;
using Lykke.blue.Api.Infrastructure;

namespace Lykke.blue.Api.Controllers
{
    [Route("api/refLinks")]
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

        [HttpGet("id/{id}")]
        [AllowAnonymous]
        [SwaggerOperation("GetReferralLinkById")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetReferralLinkById(string id)
        {
            var result = await ExecuteRefLinksMethod((p) => _referralLinksService.GetReferralLinkByIdWithHttpMessagesAsync(p), id, "Get Referral Link By Id");
            return result;
        }


        [HttpGet("request/invitationLink")]
        [SwaggerOperation("RequestInvitationReferralLink")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> RequestInvitationReferralLink()
        {
            var result = await ExecuteRefLinksMethod((p) => _referralLinksService.RequestInvitationReferralLinkWithHttpMessagesAsync(p), new InvitationReferralLinkRequest { SenderClientId = _requestContext.ClientId }, "Invitation Link requested");
            return result;
        }
        
        [HttpPost("claim/invitationLink")]
        [SwaggerOperation("ClaimInvitationLink")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> ClaimInvitationLink([FromBody] ClaimRefLinkModel request)
        {
            var serviceRequest = request.ConvertToServiceModel();
            serviceRequest.RecipientClientId = _requestContext.ClientId;

            var result = await ExecuteRefLinksMethod((p) => _referralLinksService.ClaimInvitationLinkWithHttpMessagesAsync(p), serviceRequest, request.LogMessage);
            return result;          
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
            var serviceRequest = request.ConvertToServiceModel();
            serviceRequest.SenderClientId = _requestContext.ClientId;

            var result = await ExecuteRefLinksMethod((p) => _referralLinksService.RequestGiftCoinsReferralLinkWithHttpMessagesAsync(p), serviceRequest, request.LogMessage);
            return result;
        }
        
        /// <summary>
        /// Claim giftcoins referral link.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("claim/giftCoins")]
        [SwaggerOperation("ClaimGiftCoins")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ClaimGiftCoins([FromBody] ClaimRefLinkModel request)
        {
            var serviceRequest = request.ConvertToServiceModel();
            serviceRequest.RecipientClientId = _requestContext.ClientId;

            var result = await ExecuteRefLinksMethod((p) => _referralLinksService.ClaimGiftCoinsWithHttpMessagesAsync(p), serviceRequest, request.LogMessage);
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

        /// <summary>
        /// Get offchain ChannelKey for transfer.  
        /// </summary>
        /// <returns></returns>
        [HttpGet("offchain/channelKey")]
        [SwaggerOperation("GetChannelKey")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetChannelKey([FromQuery] string asset)
        {
            var serviceRequest = new OffchainGetChannelKeyRequest { Asset = asset, ClientId = _requestContext.ClientId };

            var result = await ExecuteRefLinksMethod((p) => _referralLinksService.GetChannelKeyWithHttpMessagesAsync(p), serviceRequest, "Offchain channel key requested");
            return result;         
        }        

        /// <summary>
        /// Create offchain transfer to Lykke wallet
        /// </summary>
        /// <returns></returns>
        [HttpPost("offchain/transferToLykkeWallet")]
        [SwaggerOperation("TransferToLykkeWallet")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> TransferToLykkeWallet([FromBody] TransferToLykkeWalletModel request)
        {
            var serviceRequest = request.ConvertToServiceModel();
            serviceRequest.ClientId = _requestContext.ClientId;

            var result = await ExecuteRefLinksMethod((p) => _referralLinksService.TransferToLykkeWalletMethodWithHttpMessagesAsync(p), serviceRequest, request.LogMessage);
            return result;           
        }       

        /// <summary>
        /// Process offchain channel
        /// </summary>
        /// <returns></returns>
        [HttpPost("offchain/processChannel")]
        [SwaggerOperation("ProcessChannel")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> ProcessChannel([FromBody] ProcessChannelModel request)
        {
            var serviceRequest = request.ConvertToServiceModel();
            serviceRequest.ClientId = _requestContext.ClientId;

            var result = await ExecuteRefLinksMethod((p) => _referralLinksService.ProcessChannelWithHttpMessagesAsync(p), serviceRequest, request.LogMessage);
            return result;           
        }        

        /// <summary>
        /// Process offchain channel
        /// </summary>
        /// <returns></returns>
        [HttpPost("offchain/finalize")]
        [SwaggerOperation("FinalizeRefLinkTransfer")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Finalize([FromBody] FinalizeTransferModel request)
        {
            var serviceRequest = request.ConvertToServiceModel();
            serviceRequest.ClientId = _requestContext.ClientId;

            var result = await ExecuteRefLinksMethod((p) => _referralLinksService.FinalizeRefLinkTransferWithHttpMessagesAsync(p), serviceRequest, request.LogMessage);
            return result;         
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

///// <summary>
///// Request invitation referral link.
///// </summary>
///// <param name="request"></param>
///// <returns></returns>
//[HttpPost("request/invitationLink")]
//[SwaggerOperation("RequestInvitationReferralLink")]
//[ProducesResponseType((int)HttpStatusCode.BadRequest)]
//[ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
//public async Task<IActionResult> RequestInvitationReferralLink([FromBody] RequestInvitationLinkModel request)
//{
//    var response = await _referralLinksService.RequestInvitationReferralLinkWithHttpMessagesAsync(request.ConvertToServiceModel());

//    var error = await response.CheckClientResponseForErrors();
//    if(error != null)
//    {
//        return error;
//    }

//    var result = response.Body;

//    await LogInfo(request, ControllerContext, $"Invitation link requested: {new { response.Response.StatusCode, result }.ToJson()}");

//    return Created(uri: $"api/referralLinks/{result}", value: result);
//}



//[HttpPost("claim/invitationLink")]
//[SwaggerOperation("ClaimInvitationLink")]
//[ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
//[ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
//[ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
//[ProducesResponseType(typeof(string), (int)HttpStatusCode.NoContent)]
//public async Task<IActionResult> ClaimInvitationLink([FromBody] ClaimRefLinkModel request)
//{
//    var response = await _referralLinksService.ClaimInvitationLinkWithHttpMessagesAsync (request.ConvertToServiceModel());

//    var error = await response.CheckClientResponseForErrors();
//    if (error != null)
//    {
//        return error;
//    }

//    var result = response.Body;

//    await LogInfo(request, ControllerContext, $"Invitation link claimed: {new { response.Response.StatusCode, result }.ToJson()}");

//    return StatusCode((int)response.Response.StatusCode, result);
//}


///// <summary>
///// Request giftcoins referral link.
///// </summary>
///// <param name="request"></param>
///// <returns></returns>
//[HttpPost("request/giftCoinslLink")]
//[SwaggerOperation("RequestGiftCoinsReferralLink")]
//[ProducesResponseType((int)HttpStatusCode.BadRequest)]
//[ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
//public async Task<IActionResult> RequestGiftCoinsReferralLink([FromBody] RequestGiftCoinsLinkModel request)
//{
//    var response = await _referralLinksService.RequestGiftCoinsReferralLinkWithHttpMessagesAsync(request.ConvertToServiceModel());

//    var error = await response.CheckClientResponseForErrors();
//    if (error != null)
//    {
//        return error;
//    }

//    var result = response.Body;

//    await LogInfo(request, ControllerContext, $"GiftCoins link requested: {new { response.Response.StatusCode, result }.ToJson()}");

//    return StatusCode((int)response.Response.StatusCode, result);
//}

///// <summary>
///// Claim giftcoins referral link.
///// </summary>
///// <param name="request"></param>
///// <returns></returns>
//[HttpPost("claim/giftCoins")]
//[SwaggerOperation("ClaimGiftCoins")]
//[ProducesResponseType((int)HttpStatusCode.BadRequest)]
//[ProducesResponseType((int)HttpStatusCode.NotFound)]
//[ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
//public async Task<IActionResult> ClaimGiftCoins([FromBody] ClaimRefLinkModel request)
//{
//    var response = await _referralLinksService.ClaimGiftCoinsWithHttpMessagesAsync(request.ConvertToServiceModel());

//    var error = await response.CheckClientResponseForErrors();
//    if (error != null)
//    {
//        return error;
//    }

//    var result = response.Body;

//    await LogInfo(request, ControllerContext, $"Gift coins link claimed: {new { response.Response.StatusCode, result }.ToJson()}");

//    return StatusCode((int)response.Response.StatusCode, result);
//}


///// <summary>
///// Get offchain ChannelKey for transfer.  
///// </summary>
///// <returns></returns>
//[HttpGet("offchain/channelKey")]
//[SwaggerOperation("GetChannelKey")]
//[ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
//public async Task<IActionResult> GetChannelKey([FromQuery] string asset, [FromQuery] string clientId)
//{
//    var response = await _referralLinksService.GetChannelKeyWithHttpMessagesAsync(asset, clientId);

//    var error = await response.CheckClientResponseForErrors();
//    if (error != null)
//    {
//        return error;
//    }

//    var result = response.Body;

//    await LogInfo(new { Asset=asset, ClientId = clientId }, ControllerContext, $"Offchain channel key requested: {new { response.Response.StatusCode, result }.ToJson()}");

//    return StatusCode((int)response.Response.StatusCode, result);
//}

///// <summary>
///// Create offchain transfer to Lykke wallet
///// </summary>
///// <returns></returns>
//[HttpPost("offchain/transferToLykkeWallet")]
//[SwaggerOperation("TransferToLykkeWallet")]
//[ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
//[ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
//[ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
//public async Task<IActionResult> TransferToLykkeWallet([FromBody] TransferToLykkeWalletModel request)
//{
//    var response = await _referralLinksService.TransferToLykkeWalletMethodWithHttpMessagesAsync(request.ConvertToServiceModel());

//    var error = await response.CheckClientResponseForErrors();
//    if (error != null)
//    {
//        return error;
//    }

//    var result = response.Body;

//    await LogInfo(request, ControllerContext, $"Offchain transfer to Lykke wallet: {new { response.Response.StatusCode, result }.ToJson()}");

//    return StatusCode((int)response.Response.StatusCode, result);
//}


///// <summary>
///// Process offchain channel
///// </summary>
///// <returns></returns>
//[HttpPost("offchain/processChannel")]
//[SwaggerOperation("ProcessChannel")]
//[ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
//[ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
//[ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
//public async Task<IActionResult> ProcessChannel([FromBody] ProcessChannelModel request)
//{
//    var response = await _referralLinksService.ProcessChannelWithHttpMessagesAsync(request.ConvertToServiceModel());

//    var error = await response.CheckClientResponseForErrors();
//    if (error != null)
//    {
//        return error;
//    }

//    var result = response.Body;

//    await LogInfo(request, ControllerContext, $"Offchain process channel: {new { response.Response.StatusCode, result }.ToJson()}");

//    return StatusCode((int)response.Response.StatusCode, result);
//}


///// <summary>
///// Process offchain channel
///// </summary>
///// <returns></returns>
//[HttpPost("offchain/finalize")]
//[SwaggerOperation("FinalizeRefLinkTransfer")]
//[ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
//[ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
//[ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
//public async Task<IActionResult> Finalize([FromBody] FinalizeTransferModel request)
//{
//    var response = await _referralLinksService.FinalizeRefLinkTransferWithHttpMessagesAsync(request.ConvertToServiceModel());

//    var error = await response.CheckClientResponseForErrors();
//    if (error != null)
//    {
//        return error;
//    }

//    var result = response.Body;

//    await LogInfo(request, ControllerContext, $"Finalize offchain transfer: {new { response.Response.StatusCode, result }.ToJson()}");

//    return StatusCode((int)response.Response.StatusCode, result);
//}

using Common.Log;
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

namespace Lykke.blue.Api.Controllers
{
    [Route("api/refLinks")]
    public class RefLinksController : BluApiBaseController
    {
        private readonly ILykkeReferralLinksService _referralLinksService;

        public RefLinksController(ILog log, ILykkeReferralLinksService refSrv) : base (log)
        {
            _referralLinksService = refSrv;
        }     

        /// <summary>
        /// Request invitation referral link.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("request/invitationLink")]
        [SwaggerOperation("RequestInvitationReferralLink")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> RequestInvitationReferralLink([FromBody] RequestInvitationLinkModel request)
        {
            var result = await ExecuteRefLinksMethod((p) => _referralLinksService.RequestInvitationReferralLinkWithHttpMessagesAsync(p), request.ConvertToServiceModel(), request.LogMessage);
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
            var result = await ExecuteRefLinksMethod((p) => _referralLinksService.ClaimInvitationLinkWithHttpMessagesAsync(p), request.ConvertToServiceModel(), request.LogMessage);
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
            var result = await ExecuteRefLinksMethod((p) => _referralLinksService.RequestGiftCoinsReferralLinkWithHttpMessagesAsync(p), request.ConvertToServiceModel(), request.LogMessage);
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
            var result = await ExecuteRefLinksMethod((p) => _referralLinksService.ClaimGiftCoinsWithHttpMessagesAsync(p), request.ConvertToServiceModel(), request.LogMessage);
            return result;            
        }

        /// <summary>
        /// Get referral links statistics by sender client id.
        /// </summary>
        /// <param name="request">Sender client id by which we wanna get statistics.</param>
        /// <returns></returns>
        [HttpPost("statistics")]
        [SwaggerOperation("GetReferralLinksStatisticsBySenderId")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(GetReferralLinksStatisticsBySenderIdResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetReferralLinksStatisticsBySenderId([FromBody]RefLinkStatisticsModel request)
        {
            var result = await ExecuteRefLinksMethod((p) => _referralLinksService.GetReferralLinksStatisticsBySenderIdWithHttpMessagesAsync(p), request.ConvertToServiceModel(), request.LogMessage);
            return result;
        }

        /// <summary>
        /// Get offchain ChannelKey for transfer.  
        /// </summary>
        /// <returns></returns>
        [HttpPost("offchain/channelKey")]
        [SwaggerOperation("GetChannelKey")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetChannelKey([FromBody]OffchainGetChannelKeyModel request)
        {
            var result = await ExecuteRefLinksMethod((p) => _referralLinksService.GetChannelKeyWithHttpMessagesAsync(p), request.ConvertToServiceModel(), request.LogMessage);
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
            var result = await ExecuteRefLinksMethod((p) => _referralLinksService.TransferToLykkeWalletMethodWithHttpMessagesAsync(p), request.ConvertToServiceModel(), request.LogMessage);
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
            var result = await ExecuteRefLinksMethod((p) => _referralLinksService.ProcessChannelWithHttpMessagesAsync(p), request.ConvertToServiceModel(), request.LogMessage);
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
            var result = await ExecuteRefLinksMethod((p) => _referralLinksService.FinalizeRefLinkTransferWithHttpMessagesAsync(p), request.ConvertToServiceModel(), request.LogMessage);
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

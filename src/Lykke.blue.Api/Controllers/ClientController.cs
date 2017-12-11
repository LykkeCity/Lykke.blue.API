using AutoMapper;
using Common.Log;
using Lykke.blue.Api.Core.Settings.ServiceSettings;
using Lykke.blue.Api.Infrastructure;
using Lykke.blue.Api.Models.ClientsModels;
using Lykke.blue.Api.Requests;
using Lykke.blue.Api.Responses;
using Lykke.blue.Api.Strings;
using Lykke.Service.ClientAccount.Client;
using Lykke.Service.Registration;
using Lykke.Service.Registration.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Lykke.blue.Api.Controllers
{
    [LowerVersion(Devices = "IPhone,IPad", LowerVersion = 181)]
    [LowerVersion(Devices = "android", LowerVersion = 659)]
    [Route("api/client")]
    public class ClientController : Controller
    {
        private readonly ILykkeRegistrationClient _lykkeRegistrationClient;
        private readonly IPartnersClient _partnersClient;
        private readonly IRequestContext _requestContext;
        private readonly BlueApiSettings _blueApiSettings;

        public ClientController(
            ILog log,
            ILykkeRegistrationClient lykkeRegistrationClient,
            IPartnersClient partnersClient,
            IRequestContext requestContext,
            BlueApiSettings blueApiSettings)
        {
            _lykkeRegistrationClient = lykkeRegistrationClient ?? throw new ArgumentNullException(nameof(lykkeRegistrationClient));
            _partnersClient = partnersClient;
            _requestContext = requestContext ?? throw new ArgumentNullException(nameof(requestContext));
            _blueApiSettings = blueApiSettings;
        }

        /// <summary>
        /// Authenticate user.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("auth")]
        [SwaggerOperation("Auth")]
        [ProducesResponseType(typeof(AuthResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Auth([FromBody]AuthRequestModel request)
        {
            var authModel = Mapper.Map<AuthModel>(request);
            authModel.Ip = _requestContext.GetIp();
            authModel.UserAgent = _requestContext.UserAgent;

            var authResult = await _lykkeRegistrationClient.AuthorizeAsync(authModel);

            if (authResult?.Status == AuthenticationStatus.Error)
                return BadRequest(new { message = authResult.ErrorMessage });

            return Ok(Mapper.Map<AuthResponseModel>(authResult));
        }

        /// <summary>
        /// Return the amount of registered Lykke.blue users by partner.
        /// </summary>   
        [HttpGet("getUsersCountByPartner")]
        [SwaggerOperation("GeUsersCountRegistered")]
        [ProducesResponseType(typeof(UsersCountResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetRegisteredUsersCount()
        {
            string partnerId = _blueApiSettings.DefaultBlueLifePartnerId;
            try
            {

                int? count = await _partnersClient.GetUsersCountByPartnerIdAsync(partnerId);

                if (!count.HasValue)
                    return NotFound(Phrases.UsersByPartnerIdNotFound);

                return Ok(UsersCountResponseModel.Create(count.Value));
            }
            catch (Exception ex)
            {
                //fake community count
                await _log.WriteInfoAsync(nameof(ClientController), nameof(GetRegisteredUsersCount), partnerId , ex.ToString(), DateTime.Now);
                return Ok(UsersCountResponseModel.Create(135));
            }
        }
    }
}

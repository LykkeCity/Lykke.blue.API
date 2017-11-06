using System;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Common.Log;
using Lykke.blue.Api.Credentials;
using Lykke.blue.Api.Infrastructure;
using Lykke.blue.Api.Requests;
using Lykke.blue.Api.Responses;
using Lykke.Service.Registration;
using Lykke.Service.Registration.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;

namespace Lykke.blue.Api.Controllers
{
    [LowerVersion(Devices = "IPhone,IPad", LowerVersion = 181)]
    [LowerVersion(Devices = "android", LowerVersion = 659)]
    [Route("api/client")]
    public class ClientController : Controller
    {
        private readonly ILog _log;
        private readonly ILykkeRegistrationClient _lykkeRegistrationClient;
        private readonly ClientAccountLogic _clientAccountLogic;
        private readonly IRequestContext _requestContext;

        public ClientController(
            ILog log,
            ILykkeRegistrationClient lykkeRegistrationClient,
            ClientAccountLogic clientAccountLogic,
            IRequestContext requestContext)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));
            _lykkeRegistrationClient = lykkeRegistrationClient ?? throw new ArgumentNullException(nameof(lykkeRegistrationClient));
            _clientAccountLogic = clientAccountLogic;
            _requestContext = requestContext ?? throw new ArgumentNullException(nameof(requestContext));
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
    }
}

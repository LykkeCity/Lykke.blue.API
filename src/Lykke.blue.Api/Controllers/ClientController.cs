using System;
using System.Net;
using System.Threading.Tasks;
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
    //[LowerVersion(Devices = "IPhone,IPad", LowerVersion = 181)]
    //[LowerVersion(Devices = "android", LowerVersion = 659)]
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
        /// Authenticate.
        /// </summary>
        [HttpPost("auth")]
        [SwaggerOperation("Auth")]
        [ProducesResponseType(typeof(AuthResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Auth([FromBody]AuthRequestModel model)
        {
            var authResult = await _lykkeRegistrationClient.AuthorizeAsync(new AuthModel
            {
                ClientInfo = model.ClientInfo,
                Email = model.Email,
                Password = model.Password,
                Ip = _requestContext.GetIp(),
                UserAgent = _requestContext.UserAgent,
                PartnerId = model.PartnerId
            });

            if (authResult?.Status == AuthenticationStatus.Error)
                return BadRequest(new { message = authResult.ErrorMessage });

            return Ok(new AuthResponseModel
            {
                AccessToken = authResult?.Token,
            });
        }
    }
}

using System;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Common.Log;
using Lykke.blue.Api.Infrastructure;
using ApiRequests = Lykke.blue.Api.Requests;
using ApiResponses = Lykke.blue.Api.Responses;
using ClientModel = Lykke.Service.Pledges.Client.AutorestClient.Models;
using Lykke.Service.Pledges.Client;
using Lykke.Service.Pledges.Client.AutorestClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;

namespace Lykke.blue.Api.Controllers
{
    [Authorize]
    [Route("api/pledges")]
    public class PledgesController : Controller
    {
        private readonly IRequestContext _requestContext;
        private readonly IPledgesClient _pledgesClient;
        private readonly IPledgesAPI _pledgesApi;
        private readonly ILog _log;

        public PledgesController(ILog log, 
            IPledgesClient pledgesClient, 
            IRequestContext requestContext,
            IPledgesAPI pledgesApi)
        {
            _log = log ?? throw new ArgumentException(nameof(log));
            _pledgesClient = pledgesClient ?? throw new ArgumentException(nameof(pledgesClient));
            _requestContext = requestContext ?? throw new ArgumentNullException(nameof(requestContext));
            _pledgesApi = pledgesApi ?? throw new ArgumentNullException(nameof(pledgesApi));
        }

        /// <summary>
        /// Create a new pledge.
        /// </summary>
        /// <param name="request">Pledge value.</param>
        /// <returns>Created pledge.</returns>
        [HttpPost]
        [SwaggerOperation("CreatePledge")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Create([FromBody] ApiRequests.CreatePledgeRequest request)
        {
            if (request == null)
                return BadRequest();

            var clientRequest = Mapper.Map<ClientModel.CreatePledgeRequest>(request);
            clientRequest.ClientId = _requestContext.ClientId;

            var pledgeApiResponse = await _pledgesApi.CreatePledgeWithHttpMessagesAsync(clientRequest);

            if (pledgeApiResponse.Response.StatusCode == HttpStatusCode.BadRequest)
            {
                var message = await pledgeApiResponse.Response.Content.ReadAsStringAsync().ConfigureAwait(false);

                return BadRequest(message);
            }
            else if (pledgeApiResponse.Response.StatusCode == HttpStatusCode.NotFound)
            {
                var message = await pledgeApiResponse.Response.Content.ReadAsStringAsync().ConfigureAwait(false);

                return NotFound(message);
            }

            return Created(uri: $"api/pledges/{clientRequest.ClientId}", value: string.Empty);
        }

        /// <summary>
        /// Get pledge.
        /// </summary>
        /// <param name="clientId">Id of the pledge we wanna find.</param>
        /// <returns>Found pledge.</returns>
        [HttpGet]
        [SwaggerOperation("GetPledge")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiResponses.GetPledgeResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var pledgeApiResponse = await _pledgesApi.GetPledgeWithHttpMessagesAsync(_requestContext.ClientId);

            if (pledgeApiResponse.Response.StatusCode == HttpStatusCode.BadRequest)
            {
                var message = await pledgeApiResponse.Response.Content.ReadAsStringAsync().ConfigureAwait(false);

                return BadRequest(message);
            }
            else if (pledgeApiResponse.Response.StatusCode == HttpStatusCode.NotFound)
            {
                var message = await pledgeApiResponse.Response.Content.ReadAsStringAsync().ConfigureAwait(false);

                return NotFound(message);
            }

            var pledge = pledgeApiResponse.Body;

            var response = Mapper.Map<ApiResponses.GetPledgeResponse>(pledge);

            return Ok(response);
        }

        /// <summary>
        /// Update pledge details.
        /// </summary>
        /// <param name="id">Id of the pledge we wanna update.</param>
        /// <param name="request">Pledge values we wanna change.</param>
        /// <returns>Updated pledge.</returns>
        [HttpPut]
        [SwaggerOperation("UpdatePledge")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Update([FromBody] ApiRequests.UpdatePledgeRequest request)
        {
            if (request == null)
                return BadRequest();

            var clientRequest = Mapper.Map<ClientModel.UpdatePledgeRequest>(request);
            clientRequest.ClientId = _requestContext.ClientId;

            var pledgeApiResponse = await _pledgesApi.UpdatePledgeWithHttpMessagesAsync(clientRequest);

            if (pledgeApiResponse.Response.StatusCode == HttpStatusCode.BadRequest)
            {
                var message = await pledgeApiResponse.Response.Content.ReadAsStringAsync().ConfigureAwait(false);

                return BadRequest(message);
            }
            else if (pledgeApiResponse.Response.StatusCode == HttpStatusCode.NotFound)
            {
                var message = await pledgeApiResponse.Response.Content.ReadAsStringAsync().ConfigureAwait(false);

                return NotFound(message);
            }

            return NoContent();
        }

        /// <summary>
        /// Delete pledge.
        /// </summary>
        /// <param name="clientId">Id of the pledge we wanna delete.</param>
        [HttpDelete]
        [SwaggerOperation("DeletePledge")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Delete()
        {
            var pledgeApiResponse = await _pledgesApi.DeletePledgeWithHttpMessagesAsync(_requestContext.ClientId);

            if (pledgeApiResponse.Response.StatusCode == HttpStatusCode.BadRequest)
            {
                var message = await pledgeApiResponse.Response.Content.ReadAsStringAsync().ConfigureAwait(false);

                return BadRequest(message);
            }
            else if (pledgeApiResponse.Response.StatusCode == HttpStatusCode.NotFound)
            {
                var message = await pledgeApiResponse.Response.Content.ReadAsStringAsync().ConfigureAwait(false);

                return NotFound(message);
            }

            return NoContent();
        }
    }
}

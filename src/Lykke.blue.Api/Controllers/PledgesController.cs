using AutoMapper;
using Common.Log;
using Lykke.blue.Api.Infrastructure;
using Lykke.blue.Api.Infrastructure.Extensions;
using Lykke.Service.Pledges.Client;
using Lykke.Service.Pledges.Client.AutorestClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;
using System;
using System.Net;
using System.Threading.Tasks;
using ApiRequests = Lykke.blue.Api.Requests;
using ApiResponses = Lykke.blue.Api.Responses;
using ClientModel = Lykke.Service.Pledges.Client.AutorestClient.Models;

namespace Lykke.blue.Api.Controllers
{
    [Authorize]
    [Route("api/pledges")]
    public class PledgesController : Controller
    {
        private readonly IRequestContext _requestContext;
        private readonly IPledgesAPI _pledgesApi;

        public PledgesController(ILog log, 
            IPledgesClient pledgesClient, 
            IRequestContext requestContext,
            IPledgesAPI pledgesApi)
        {
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

            var badResponse = await pledgeApiResponse.CheckClientResponseForErrors();

            if (badResponse != null)
                return badResponse;

            return Created(uri: $"api/pledges/{clientRequest.ClientId}", value: string.Empty);
        }

        /// <summary>
        /// Get pledge.
        /// </summary>
        /// <returns>Found pledge.</returns>
        [HttpGet]
        [SwaggerOperation("GetPledge")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiResponses.GetPledgeResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var pledgeApiResponse = await _pledgesApi.GetPledgeWithHttpMessagesAsync(_requestContext.ClientId);

            var badResponse = await pledgeApiResponse.CheckClientResponseForErrors();

            if (badResponse != null)
                return badResponse;

            var pledge = pledgeApiResponse.Body;

            var response = Mapper.Map<ApiResponses.GetPledgeResponse>(pledge);

            return Ok(response);
        }

        /// <summary>
        /// Update pledge details.
        /// </summary>
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

            var badResponse = await pledgeApiResponse.CheckClientResponseForErrors();

            if (badResponse != null)
                return badResponse;

            return NoContent();
        }

        /// <summary>
        /// Delete pledge.
        /// </summary>
        [HttpDelete]
        [SwaggerOperation("DeletePledge")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Delete()
        {
            var pledgeApiResponse = await _pledgesApi.DeletePledgeWithHttpMessagesAsync(_requestContext.ClientId);

            var badResponse = await pledgeApiResponse.CheckClientResponseForErrors();

            if (badResponse != null)
                return badResponse;

            return NoContent();
        }
    }
}

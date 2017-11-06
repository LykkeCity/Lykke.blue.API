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
        private readonly ILog _log;

        public PledgesController(ILog log, IPledgesClient pledgesClient, IRequestContext requestContext)
        {
            _log = log ?? throw new ArgumentException(nameof(log));
            _pledgesClient = pledgesClient ?? throw new ArgumentException(nameof(pledgesClient));
            _requestContext = requestContext ?? throw new ArgumentNullException(nameof(requestContext));
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
        [ProducesResponseType(typeof(ApiResponses.CreatePledgeResponse), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Create([FromBody] ApiRequests.CreatePledgeRequest request)
        {
            if (request == null)
                return BadRequest();

            var clientRequest = Mapper.Map<ClientModel.CreatePledgeRequest>(request);
            clientRequest.ClientId = _requestContext.ClientId;

            var pledge = await _pledgesClient.Create(clientRequest);
            var response = Mapper.Map<ApiResponses.CreatePledgeResponse>(pledge);

            return Created(uri: $"api/pledges/{pledge.Id}", value: response);
        }

        /// <summary>
        /// Get pledge.
        /// </summary>
        /// <param name="id">Id of the pledge we wanna find.</param>
        /// <returns>Found pledge.</returns>
        [HttpGet("{id}")]
        [SwaggerOperation("GetPledge")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiResponses.GetPledgeResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get(string id)
        {
            if (String.IsNullOrEmpty(id))
                return BadRequest();

            var pledge = await _pledgesClient.Get(id);

            if (pledge == null)
                return NotFound();

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
        [ProducesResponseType(typeof(ApiResponses.UpdatePledgeResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update([FromBody] ApiRequests.UpdatePledgeRequest request)
        {
            if (request == null)
                return BadRequest();

            var clientRequest = Mapper.Map<ClientModel.UpdatePledgeRequest>(request);
            clientRequest.ClientId = _requestContext.ClientId;

            var pledge = await _pledgesClient.Update(clientRequest);
            var response = Mapper.Map<ApiResponses.UpdatePledgeResponse>(pledge);

            return Ok(response);
        }

        /// <summary>
        /// Delete pledge.
        /// </summary>
        /// <param name="id">Id of the pledge we wanna delete.</param>
        [HttpDelete("{id}")]
        [SwaggerOperation("DeletePledge")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Delete(string id)
        {
            if (String.IsNullOrEmpty(id))
                return BadRequest();

            await _pledgesClient.Delete(id);

            return NoContent();
        }

        /// <summary>
        /// Get client pledge. 
        /// </summary>
        [HttpGet]
        [SwaggerOperation("GetPledge")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiResponses.GetPledgeResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPledge()
        {
            var pledges = await _pledgesClient.GetPledgeByClientId(_requestContext.ClientId);
            var response = Mapper.Map<ApiResponses.GetPledgeResponse>(pledges);

            return Ok(response);
        }
    }
}

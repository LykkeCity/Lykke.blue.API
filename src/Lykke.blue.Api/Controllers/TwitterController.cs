using Common.Log;
using Lykke.blue.Api.Models.TwitterModels;
using Lykke.blue.Api.Strings;
using Lykke.blue.Service.InspireStream.Client;
using Lykke.blue.Service.InspireStream.Client.AutorestClient.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Swashbuckle.SwaggerGen.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Lykke.blue.Api.Controllers
{
    [Route("api/twitter")]
    public class TwitterController : Controller
    {
        private readonly IInspireStreamClient _inspireStreamClient;

        public TwitterController(
           ILog log,
           IInspireStreamClient inspireStreamClient
            )
        {
            _inspireStreamClient = inspireStreamClient ?? throw new ArgumentNullException(nameof(inspireStreamClient));
        }

        /// <summary>
        /// Get tweets cash data
        /// </summary>
        /// <param name="model">Tweets request model by which we search for tweets</param>
        /// <returns>
        /// Return tweets json format according twitter api
        /// </returns>
        [HttpPost("getTweetsJSON")]
        [SwaggerOperation("GetTweetsJSON")]
        [ProducesResponseType(typeof(IEnumerable<JObject>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetTweetsJson([FromBody]TweetsRequestModel model)
        {
            var resEnum = await _inspireStreamClient.GetAsync(model.CreateReques(model));
            var result = resEnum as TweetsResponseModel[] ?? resEnum?.ToArray();

            if (result == null || !result.Any())
                return NotFound(Phrases.TweetsNotFound);

            return Ok(result.Select(t => JObject.Parse(t.TweetJSON)));
        }
    }
}

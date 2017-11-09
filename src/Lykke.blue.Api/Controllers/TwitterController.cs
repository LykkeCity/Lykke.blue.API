using Common.Log;
using Lykke.blue.Api.Infrastructure;
using Lykke.blue.Api.Models.TwitterModels;
using Lykke.blue.Api.Strings;
using Lykke.blue.Service.InspireStream.Client;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Lykke.blue.Api.Controllers
{
    [LowerVersion(Devices = "IPhone,IPad", LowerVersion = 181)]
    [LowerVersion(Devices = "android", LowerVersion = 659)]
    [Route("api/twitter")]
    public class TwitterController : Controller
    {
        private readonly ILog _log;
        private readonly IInspireStreamClient _inspireStreamClient;

        public TwitterController(
           ILog log,
           IInspireStreamClient inspireStreamClient
            )
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));
            _inspireStreamClient = inspireStreamClient ?? throw new ArgumentNullException(nameof(inspireStreamClient));
        }

        /// <summary>
        /// Get tweets
        /// </summary>
        [HttpGet]
        [SwaggerOperation("GetTweets")]
        [ProducesResponseType(typeof(IEnumerable<TweetsResponseModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Get([FromBody]TweetsRequestModel model)
        {
            var result = await _inspireStreamClient.GetAsync(model.CreateReques(model));

            if (result.Count() < 0)
                return NotFound(Phrases.TweetsNotFound);


            return Ok(result);
        }
    }
}

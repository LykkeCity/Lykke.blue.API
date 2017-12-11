using Common;
using Common.Log;
using Lykke.blue.Api.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Lykke.blue.Api.Controllers
{
    public class BluApiBaseController : Controller
    {
        private readonly ILog _log;

        public BluApiBaseController(ILog log)
        {
            _log = log;
        }

        protected async Task LogInfo<T>(T callParams, ControllerContext controllerCtx, string info)
        {
            await _log.WriteInfoAsync(controllerCtx.GetExecutongControllerAndAction(), (new { callParams }).ToJson(), info);
        }
    }
}

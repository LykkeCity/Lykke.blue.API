using Common.Cache;
using Lykke.blue.Api.Core.Settings.LykkeSettings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Lykke.blue.Api.Core.Filters
{
    public class DisableOnMaintenanceFilter : ActionFilterAttribute
    {
        private const string IsOnMaintenanceCacheKey = "globalsetting-is-on-maintenance";

        private readonly ICacheManager _cacheManager;
        private readonly ILykkeGlobalSettingsRepositry _appGlobalSettings;

        public DisableOnMaintenanceFilter(ICacheManager cacheManager, ILykkeGlobalSettingsRepositry appGlobalSettings)
        {
            _cacheManager = cacheManager;
            _appGlobalSettings = appGlobalSettings;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (_cacheManager.Get(IsOnMaintenanceCacheKey, 1, async () => (await _appGlobalSettings.GetAsync()).IsOnMaintenance).Result)
            {
                ReturnOnMaintenance(context);
            }
        }

        private void ReturnOnMaintenance(ActionExecutingContext actionContext)
        {
            actionContext.Result = new ObjectResult("Sorry, application is on maintenance. Please try again later.")
                {
                    StatusCode = (int)HttpStatusCode.ServiceUnavailable
                }; 
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.blue.Api.Infrastructure.Extensions
{
    public static class ControllerExtensions
    {
        public static string GetExecutongControllerAndAction(this ControllerContext contContext)
        {
            return $"api/{contContext.RouteData.Values["controller"].ToString()}/{contContext.RouteData.Values["action"].ToString()}";
        }
    }
}

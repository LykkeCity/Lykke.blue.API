using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Rest;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Lykke.blue.Api.Infrastructure.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetUserAgent(this HttpRequest request)
        {
            return request.Headers["User-Agent"].ToString();
        }

        public static T GetHeaderValueAs<T>(this HttpContext httpContext, string headerName)
        {
            if (httpContext?.Request?.Headers?.TryGetValue(headerName, out var values) ?? false)
            {
                string rawValues = values.ToString();   // writes out as Csv when there are multiple.

                if (!string.IsNullOrEmpty(rawValues))
                    return (T)Convert.ChangeType(values.ToString(), typeof(T));
            }
            return default(T);
        }

        public static async Task<ObjectResult> CheckClientResponseForErrors(this HttpOperationResponse httpResponse)
        {
            var internalServerError = new ObjectResult("Service call returned null HttpOperationResponse.")
            {
                StatusCode = (int) HttpStatusCode.InternalServerError
            };

            if (httpResponse == null)
            {
                return internalServerError;
            }

            var message = await httpResponse.Response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (httpResponse.Response.IsSuccessStatusCode)
            {
                return null;
            }

            if (httpResponse.Response.StatusCode == HttpStatusCode.BadRequest)
            {
                return new BadRequestObjectResult(message);
            }
            if (httpResponse.Response.StatusCode == HttpStatusCode.NotFound)
            {
                return new NotFoundObjectResult(message);
            }
           
            internalServerError.Value = message;
            return internalServerError;
            
        }

    }
}

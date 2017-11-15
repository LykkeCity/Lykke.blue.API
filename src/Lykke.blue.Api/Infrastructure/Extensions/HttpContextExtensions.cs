using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.Rest;

namespace Lykke.blue.Api.Infrastructure.Extensions
{
    public static class HttpContextExtensions
    {
        public static Uri GetUri(this HttpRequest request)
        {
            var hostComponents = request.Host.ToUriComponent().Split(':');

            var builder = new UriBuilder
            {
                Scheme = request.Scheme,
                Host = hostComponents[0],
                Path = request.Path,
                Query = request.QueryString.ToUriComponent()
            };

            if (hostComponents.Length == 2)
            {
                builder.Port = Convert.ToInt32(hostComponents[1]);
            }

            return builder.Uri;
        }

        public static string GetUserAgent(this HttpRequest request)
        {
            return request.Headers["User-Agent"].ToString();
        }

        public static T GetHeaderValueAs<T>(this HttpContext httpContext, string headerName)
        {
            StringValues values;

            if (httpContext?.Request?.Headers?.TryGetValue(headerName, out values) ?? false)
            {
                string rawValues = values.ToString();   // writes out as Csv when there are multiple.

                if (!string.IsNullOrEmpty(rawValues))
                    return (T)Convert.ChangeType(values.ToString(), typeof(T));
            }
            return default(T);
        }

        public static async Task<ObjectResult> CheckClientResponseForErrors(this HttpOperationResponse httpResponse)
        {
            var message = await httpResponse.Response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (httpResponse.Response.IsSuccessStatusCode)
            {
                return null;
            }

            if (httpResponse.Response.StatusCode == HttpStatusCode.BadRequest)
            {
                return new BadRequestObjectResult(message);
            }
            else if (httpResponse.Response.StatusCode == HttpStatusCode.NotFound)
            {
                return new NotFoundObjectResult(message);
            }
            else
            {
                var result = new ObjectResult(message);
                result.StatusCode = (int) HttpStatusCode.InternalServerError;
                return result;
            }
        }

        //create separate methods for getting code and msg from httpResponse, return StatusCode(httpCode, msg) from controller

        public static async Task<string> GetStatusCodeAndMessage(this HttpOperationResponse httpResponse)
        {
            var message = await httpResponse.Response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return $"HttpCode:{httpResponse.Response.StatusCode}, Message:{message}";
        }
    }
}

using System;
using Common.Log;

namespace Lykke.blue.Api.Client
{
    public class ApiClient : IApiClient, IDisposable
    {
        private readonly ILog _log;

        public ApiClient(string serviceUrl, ILog log)
        {
            _log = log;
        }

        public void Dispose()
        {
            //if (_service == null)
            //    return;
            //_service.Dispose();
            //_service = null;
        }
    }
}

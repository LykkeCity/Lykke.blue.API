using System;
using System.Security.Principal;

namespace Lykke.blue.Api.Services.Identity
{
    public class LykkeIdentity : IIdentity
    {
        public string Name { get; private set; }
        public string AuthenticationType { get; private set; }
        public bool IsAuthenticated { get; private set; }
        private DateTime Created { get; set; }

        public static LykkeIdentity Create(string clientId)
        {
            return new LykkeIdentity
            {
                Name = clientId,
                AuthenticationType = "Token",
                Created = DateTime.UtcNow,
                IsAuthenticated = true
            };
        }
    }
}

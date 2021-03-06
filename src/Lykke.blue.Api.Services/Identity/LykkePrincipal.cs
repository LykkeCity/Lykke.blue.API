﻿using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Lykke.blue.Api.Core.Constants;
using Lykke.blue.Api.Core.Identity;
using Lykke.Service.Session;
using Microsoft.AspNetCore.Http;

namespace Lykke.blue.Api.Services.Identity
{
    public class LykkePrincipal : ILykkePrincipal
    {
        private readonly ClaimsCache _claimsCache = new ClaimsCache();
        private readonly IClientSessionsClient _clientSessionsClient;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public LykkePrincipal(IHttpContextAccessor httpContextAccessor, IClientSessionsClient clientSessionsClient)
        {
            _httpContextAccessor = httpContextAccessor;
            _clientSessionsClient = clientSessionsClient;
        }

        public string GetToken()
        {
            var context = _httpContextAccessor.HttpContext;

            var header = context.Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(header))
                return null;

            var values = header.Split(' ');

            if (values.Length != 2)
                return null;

            if (values[0] != "Bearer")
                return null;

            return values[1];
        }

        public void InvalidateCache(string token)
        {
            _claimsCache.Invalidate(token);
        }

        public async Task<ClaimsPrincipal> GetCurrent()
        {
            var token = GetToken();

            if (string.IsNullOrWhiteSpace(token))
                return null;

            var result = _claimsCache.Get(token);
            if (result != null)
                return result;

            var session = await _clientSessionsClient.GetAsync(token);
            if (session == null)
                return null;

            if (DateTime.UtcNow - session.LastAction > LykkeConstants.SessionLifetime)
            {
                await _clientSessionsClient.DeleteSessionIfExistsAsync(token);
                return null;
            }

            if (DateTime.UtcNow - session.LastAction > LykkeConstants.SessionRefreshPeriod)
            {
                await _clientSessionsClient.RefreshSessionAsync(token);
            }

            result = new ClaimsPrincipal(LykkeIdentity.Create(session.ClientId));
            if (session.PartnerId != null)
            {
                (result.Identity as ClaimsIdentity)?.AddClaim(new Claim("PartnerId", session.PartnerId));
            }

            if (session.Pinned)
            {
                (result.Identity as ClaimsIdentity)?.AddClaim(new Claim("TokenType", "Pinned"));
            }

            _claimsCache.Set(token, result);
            return result;
        }
    }
}

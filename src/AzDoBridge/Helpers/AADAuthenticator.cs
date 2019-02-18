using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Threading;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace AzDoBridge.Helpers
{
    public static class AADAuthenticator
    {
        private static readonly string ISSUER = Environment.GetEnvironmentVariable("AzureSecurity:Issuer", EnvironmentVariableTarget.Process);
        private static readonly string AUDIENCE = Environment.GetEnvironmentVariable("AzureSecurity:Audience", EnvironmentVariableTarget.Process);
        private static readonly string AudienceID = Environment.GetEnvironmentVariable("AzureSecurity:AudienceID", EnvironmentVariableTarget.Process);
        private static readonly IConfigurationManager<OpenIdConnectConfiguration> _configurationManager;

        static AADAuthenticator()
        {
            HttpDocumentRetriever documentRetriever = new HttpDocumentRetriever();
            documentRetriever.RequireHttps = ISSUER.StartsWith("https://");

            _configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
                $"{ISSUER}/.well-known/openid-configuration",
                new OpenIdConnectConfigurationRetriever(),
                documentRetriever
            );
        }

        public static async Task<ClaimsPrincipal> ValidateTokenAsync(string value, ILogger log)
        {
            log.LogTrace($"Validating User ... ");           
            OpenIdConnectConfiguration config = await GetOpenIDConfig(_configurationManager, log);
            string issuer = ISSUER;
            string audience = AUDIENCE;

            TokenValidationParameters validationParameter = new TokenValidationParameters()
            {
                RequireSignedTokens = true,
                ValidAudiences = new[] { audience, AudienceID },
                ValidateAudience = true,
                ValidIssuers = new[] { issuer, $"{issuer}v2.0" },
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                IssuerSigningKeys = config.SigningKeys
            };

            ClaimsPrincipal result = null;
            int tries = 0;

            while (result == null && tries <= 1)
            {
                try
                {
                    JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                    result = handler.ValidateToken(value, validationParameter, out var token);
                }
                catch (SecurityTokenSignatureKeyNotFoundException)
                {
                    _configurationManager.RequestRefresh();
                    tries++;
                }
                catch (SecurityTokenException ex)
                {
                    log.LogTrace($"token exception:{ex.Message} ");
                    return null;
                }
            }

            return result;
        }
        static async Task<OpenIdConnectConfiguration> GetOpenIDConfig(IConfigurationManager<OpenIdConnectConfiguration> _configurationManager, ILogger log)
        {
            OpenIdConnectConfiguration config = null;
            try
            {
                config = await _configurationManager.GetConfigurationAsync(CancellationToken.None);
            }
            catch (Exception e) { log.LogError($"OpenID Validaiton Exception: {e.Message}"); }
            return config;
        }
    }
}

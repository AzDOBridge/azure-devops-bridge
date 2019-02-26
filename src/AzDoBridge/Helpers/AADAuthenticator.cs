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
    public class AADAuthenticator
    {
        public IConfigurationManager<OpenIdConnectConfiguration> ConfigurationManager { get; set; }

        readonly ILogger Log;

        public AADAuthenticator(string issuer,ILogger log)
        {
            Log = log;
            HttpDocumentRetriever documentRetriever = new HttpDocumentRetriever()
            { RequireHttps = issuer.StartsWith("https://") };           

            ConfigurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
                $"{issuer}/.well-known/openid-configuration",
                new OpenIdConnectConfigurationRetriever(),
                documentRetriever
            );
        }              

        public async Task<ClaimsPrincipal> ValidateTokenAsync(string value, string issuer, string audience, string audienceid)
        {
            Log.LogTrace("Validating User ... ");           
            OpenIdConnectConfiguration config = await GetOpenIDConfig(ConfigurationManager);
         
            TokenValidationParameters validationParameter = new TokenValidationParameters()
            {
                RequireSignedTokens = true,
                ValidAudiences = new[] { audience, audienceid },
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
                    ConfigurationManager.RequestRefresh();
                    tries++;
                }
                catch (SecurityTokenException ex)
                {
                    Log.LogTrace($"token exception:{ex.Message} ");
                    return null;
                }
            }

            return result;
        }
        async Task<OpenIdConnectConfiguration> GetOpenIDConfig(IConfigurationManager<OpenIdConnectConfiguration> configurationManager)
        {
            OpenIdConnectConfiguration config = null;
            CancellationTokenSource cs = new CancellationTokenSource(3000);
            try
            {
                config = await configurationManager.GetConfigurationAsync(cs.Token);
            }
            catch (OperationCanceledException oce)
            {
                Log.LogError($"OpenID Validaiton Cancelled due to delay timeout : {oce.Message}");
            }
            catch (Exception e)
            {
                Log.LogError($"OpenID Validaiton Exception: {e.Message}", e);
            }
            return config;
        }
    }
}

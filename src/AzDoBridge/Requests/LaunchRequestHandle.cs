

using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using AzDoBridge.Helpers;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace AzDoBridge.Requests
{
    public class LaunchRequestHandle : AzDoBridgeRequest
    {
        public LaunchRequestHandle(ILogger log) : base(log)
        {
            ILogger Log = log;
        }

        public override async Task <SkillResponse> Handle(WorkItemStore workItemStore, SkillRequest skillRequest)
        {
            try
            {
                //Authenticate and maintain User through session 
                SkillResponse skillResponse = null;
                if (skillRequest?.Session?.User?.AccessToken != null)
                {
                    string AADAuthenticatorIssuer = Environment.GetEnvironmentVariable("AzureSecurity:Issuer", EnvironmentVariableTarget.Process);
                    string AADAuthenticatorAudience = Environment.GetEnvironmentVariable("AzureSecurity:Audience", EnvironmentVariableTarget.Process);
                    string AADAuthenticatorAudienceId = Environment.GetEnvironmentVariable("AzureSecurity:AudienceID", EnvironmentVariableTarget.Process);

                    AADAuthenticator authenticator = new AADAuthenticator(AADAuthenticatorIssuer, Log);

                    ClaimsPrincipal claimsPrincipal = await authenticator.ValidateTokenAsync(skillRequest.Session.User.AccessToken, AADAuthenticatorIssuer, AADAuthenticatorAudience, AADAuthenticatorAudienceId);

                    if (claimsPrincipal != null)
                    {
                        string LoggedUsername = claimsPrincipal.FindFirst("name").Value;
                        skillResponse = ResponseBuilder.Ask($"hello, {LoggedUsername}. How can I help you?", null);
                        skillResponse.SessionAttributes = new Dictionary<string, object>
                             {
                                 { "LoggedInUser",LoggedUsername }
                             };
                        skillResponse.Response.ShouldEndSession = false;
                    }
                }
                else { skillResponse = ResponseBuilder.Tell($"Unautherized"); }
                return skillResponse;
            }
            catch (Exception e)
            {
                Log.LogError($"Failed to handle Launch Request: {e.Message}");
                return ResponseBuilder.Tell($"Exception {e.Message}");
            }
        }
    }
}

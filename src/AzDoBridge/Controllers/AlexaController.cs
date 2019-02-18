using System;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using AzDoBridge.Actions;
using AzDoBridge.Clients;
using AzDoBridge.Models;
using AzDoBridge.Helpers;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace AzDoBridge.Controllers
{
    public static class AlexaController
    {
        [FunctionName(nameof(AlexaController))]
        public static async Task<SkillResponse> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestMessage httpRequestMessage, ILogger log)
        {
            log.LogTrace($"Controller Started");



            WorkItemStore workItemStore;
            try
            {
                // Loading Environment Information
                string azureDevopsHostname = Environment.GetEnvironmentVariable("AzureDevOps:Hostname", EnvironmentVariableTarget.Process);
                string azureDevopsUsername = string.Empty;
                string azureDevopsAccessToken = Environment.GetEnvironmentVariable("AzureDevOps:PAT", EnvironmentVariableTarget.Process);

                // Load Clients
                AzureDevOpsClient azureDevOpsClient = new AzureDevOpsClient(new Uri(azureDevopsHostname), azureDevopsUsername, azureDevopsAccessToken);


                workItemStore = await azureDevOpsClient.FetchWorkItemStoreAsync()
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                log.LogError($"Failed to fetch work item store: {ex.Message}", ex);
                throw;
            }
            
            // try to read request
            SkillRequest skillRequest;
            try
            {
                skillRequest = await httpRequestMessage.Content.ReadAsAsync<SkillRequest>()
                   .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                log.LogError($"Failed to read skill from request message: {ex.Message}", ex);
                throw;
            }
            
            switch (skillRequest.Request.Type)
            {
                case RequestType.LaunchRequest:
                {
                     //Authenticate and maintain User through session 
                     SkillResponse skillResponse = null;
                     if (skillRequest?.Session?.User?.AccessToken != null)
                     {
                         ClaimsPrincipal claimsPrincipal = await AADAuthenticator.ValidateTokenAsync(skillRequest.Session.User.AccessToken, log);
                         if (claimsPrincipal != null)
                         {
                             string LoggedUsername = claimsPrincipal.FindFirst("name").Value;
                             log.LogTrace(LoggedUsername);
                             skillResponse = ResponseBuilder.Ask($"hello, {LoggedUsername}. How can I help you?", null);
                             skillResponse.SessionAttributes = new Dictionary<string, object>();
                             skillResponse.SessionAttributes["LoggedInUser"] = LoggedUsername;                             
                             skillResponse.Response.ShouldEndSession = false;                             
                         }
                     }
                     else { skillResponse = ResponseBuilder.Tell($"Unautherized"); }
                     return skillResponse;
                }

                case RequestType.IntentRequest:
                {
                    if (!(skillRequest.Request is IntentRequest intentRequest))
                    {
                        throw new InvalidOperationException(
                            $"Expected type {typeof(IntentRequest)} but got {skillRequest.Request.GetType()}");
                    }

                    // First step: check for cancel

                    string intentName = intentRequest.Intent.Name;
                    if (intentRequest.Intent.Name.Equals("AMAZON.CancelIntent", StringComparison.OrdinalIgnoreCase))
                    {
                        log.LogTrace($"Cancel Intent");
                        return ResponseBuilder.Tell("Goodbye then!");
                    }

                    // Try to read the affected item
                    //    and run AzDoBridge actions

                    if (AzDoBridgeActionFactory.TryGetAction(intentName, log, out IAzDoBridgeAction action))
                    {
                        return action.Run(workItemStore, skillRequest);
                    }


                    log.LogTrace($"intent not recognized");
                    return ResponseBuilder.Ask("IntentRequest Not Recognized, you can say, set item ID to Item State", null);
                }

                default:
                {
                    log.LogTrace($"No Speech Detected");
                    return ResponseBuilder.Tell("No Speech Detected");
                }
            }

        }
    }
}



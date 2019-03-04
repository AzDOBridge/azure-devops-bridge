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
using AzDoBridge.Requests;

namespace AzDoBridge.Controllers
{
    public static class AlexaController
    {
        [FunctionName(nameof(AlexaController))]
        public static async Task<SkillResponse> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestMessage httpRequestMessage, ILogger log)
        {
            log.LogTrace($"Controller Started");


            //WorkItemStore workItemStore;
            AzureDevOpsClient azureDevOpsClient;
            try
            {
                // Loading Environment Information
                string azureDevopsHostname = Environment.GetEnvironmentVariable("AzureDevOps:Hostname", EnvironmentVariableTarget.Process);
                string azureDevopsUsername = string.Empty;
                string azureDevopsAccessToken = Environment.GetEnvironmentVariable("AzureDevOps:PAT", EnvironmentVariableTarget.Process);

                // Load Clients
                azureDevOpsClient = new AzureDevOpsClient(new Uri(azureDevopsHostname), azureDevopsUsername, azureDevopsAccessToken);              

            }
            catch (Exception ex)
            {
                log.LogError($"Failed to construct azureDevOpsClient : {ex.Message}", ex);
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

            if (AzDoBridgeRequestFactory.TryGetRequest(skillRequest.Request.Type, log, out IAzDoBridgeRequest request))
            {
                return await request.Handle(azureDevOpsClient, skillRequest).ConfigureAwait(false);
            }

            log.LogTrace($"No Speech Detected");
            return ResponseBuilder.Tell("No Speech Detected");

            }

    }
}




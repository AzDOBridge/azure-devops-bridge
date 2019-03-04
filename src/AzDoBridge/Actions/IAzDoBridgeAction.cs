using Alexa.NET.Request;
using Alexa.NET.Response;
using AzDoBridge.Clients;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace AzDoBridge.Actions
{
    public interface IAzDoBridgeAction
    {
        SkillResponse Run(AzureDevOpsClient azureDevOpsClient, SkillRequest skillRequest);
    }
}
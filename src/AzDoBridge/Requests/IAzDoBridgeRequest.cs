using Alexa.NET.Request;
using Alexa.NET.Response;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using System.Threading.Tasks;
using AzDoBridge.Clients;

namespace AzDoBridge.Requests
{
    public interface IAzDoBridgeRequest
    {       
        Task <SkillResponse> Handle(AzureDevOpsClient azureDevOpsClient, SkillRequest skillRequest);
    }
}

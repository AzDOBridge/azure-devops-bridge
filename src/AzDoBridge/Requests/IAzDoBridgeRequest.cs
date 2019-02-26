using Alexa.NET.Request;
using Alexa.NET.Response;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using System.Threading.Tasks;

namespace AzDoBridge.Requests
{
    public interface IAzDoBridgeRequest
    {       
        Task <SkillResponse> Handle(WorkItemStore workItemStore, SkillRequest skillRequest);
    }
}

using Alexa.NET.Request;
using Alexa.NET.Response;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace AzDoBridge.Actions
{
    public interface IAzDoBridgeAction
    {
        SkillResponse Run(WorkItemStore workItemStore, SkillRequest skillRequest);
    }
}
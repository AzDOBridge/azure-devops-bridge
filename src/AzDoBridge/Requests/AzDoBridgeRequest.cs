using Alexa.NET.Request;
using Alexa.NET.Response;
using Microsoft.Extensions.Logging;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using System.Threading.Tasks;

namespace AzDoBridge.Requests
{
    public abstract class AzDoBridgeRequest: IAzDoBridgeRequest
    {
        protected readonly ILogger Log;

        protected AzDoBridgeRequest(ILogger log)
        {
            Log = log;
        }

        public abstract  Task <SkillResponse> Handle(WorkItemStore workItemStore,  SkillRequest skillRequest);
        
    }

}

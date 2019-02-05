using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Microsoft.Extensions.Logging;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace AzDoBridge.Actions
{
    public abstract class AzDoBridgeAction : IAzDoBridgeAction
    {
        protected readonly ILogger Log;

        protected AzDoBridgeAction(ILogger log)
        {
            Log = log;
        }

        public abstract SkillResponse Run(WorkItemStore workItemStore, SkillRequest skillRequest);
    }
}

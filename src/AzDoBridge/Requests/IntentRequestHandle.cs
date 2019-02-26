using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using AzDoBridge.Actions;
using Microsoft.Extensions.Logging;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzDoBridge.Requests
{
    public class IntentRequestHandle : AzDoBridgeRequest
    {
        public IntentRequestHandle(ILogger log) : base(log)
        {

        }
        public override async Task <SkillResponse> Handle(WorkItemStore workItemStore, SkillRequest skillRequest)
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
                Log.LogTrace($"Cancel Intent");
                return await Task.Run(() =>
                {
                   return ResponseBuilder.Tell("Goodbye then!");
                }).ConfigureAwait(false);

            }

            // Try to read the affected item
            //    and run AzDoBridge actions

            if (AzDoBridgeActionFactory.TryGetAction(intentName, Log, out IAzDoBridgeAction action))
            {
                return await Task.Run(() =>
                {              
                    return action.Run(workItemStore, skillRequest);
                }).ConfigureAwait(false);

            }

            Log.LogTrace($"intent not recognized");
            return await Task.Run(() =>
            {
                return ResponseBuilder.Ask("IntentRequest Not Recognized, you can say, set item ID to Item State", null);
            }).ConfigureAwait(false);

        }
    }
}

using System;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using AzDoBridge.Clients;
using AzDoBridge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace AzDoBridge.Actions
{
    public class SetWorkItemPriorityAction : AzDoBridgeAction
    {
        public SetWorkItemPriorityAction(ILogger log) : base(log)
        {
        }

        public override SkillResponse Run(AzureDevOpsClient azureDevOpsClient, SkillRequest skillRequest)
        {

            try
            {
                // Verify request type
                if (!(skillRequest.Request is IntentRequest intentRequest))
                {
                    throw new InvalidOperationException(
                        $"Expected type {typeof(IntentRequest)} but got {skillRequest.Request.GetType()}");
                }

                // Read affected work item
                string requestItemId = intentRequest.GetSlotValue("itemid");
                if (!int.TryParse(requestItemId, out int workItemId))
                {
                    return ResponseBuilder.Tell("Received invalid work item Id.");
                }

                // load affected work item
              
                WorkItemStore workItemStore = azureDevOpsClient.FetchWorkItemStore();
                WorkItem workItem = workItemStore.GetWorkItem(workItemId);
                if (workItem is null)
                {
                    return ResponseBuilder.Tell($"No work item with id {workItemId} found.");
                }

                // read new priority
                string requestSetPriority = intentRequest.GetSlotValue("itempriority");

                // save fields
                workItem.SetFieldValue(WorkItemFieldNames.Priority, requestSetPriority);
                workItem.Save();

                return ResponseBuilder.Ask(
                    $"Priority changed to {requestSetPriority}. Listening", null);
            }

            catch (Exception e)
            {
                Log.LogError($"Failed to handle work item priority change action: {e.Message}");
                return ResponseBuilder.Tell($"Exception {e.Message}");
            }
        }
    }
}
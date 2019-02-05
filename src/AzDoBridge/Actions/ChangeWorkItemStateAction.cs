using System;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using AzDoBridge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace AzDoBridge.Actions
{
    public class ChangeWorkItemStateAction : AzDoBridgeAction
    {
        public ChangeWorkItemStateAction(ILogger log) : base(log) { }

        public override SkillResponse Run(WorkItemStore workItemStore, SkillRequest skillRequest)
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
                WorkItem workItem = workItemStore.GetWorkItem(workItemId);
                if (workItem is null)
                {
                    return ResponseBuilder.Tell($"No work item with id {workItemId} found.");
                }


                // read request to be set
                string requestedState = intentRequest.GetSlotValue("itemstate");
                if (!Enum.TryParse(requestedState, true, out WorkItemStatus workItemStatus))
                {
                    return ResponseBuilder.Tell($"The received itemstate {requestedState} is invalid and cannot be set.", null);
                }

                // check if state is already set
                string currentWorkItemState = workItem.ReadFieldValue(WorkItemFieldNames.State);
                if (String.Equals(currentWorkItemState, requestedState, StringComparison.OrdinalIgnoreCase))
                {
                    return ResponseBuilder.Ask($"Item already on {requestedState} status, repeat request with different Status", null);
                }

                // set new state
                workItem.SetFieldValue(WorkItemFieldNames.State, requestedState);
                workItem.Save();

                return ResponseBuilder.Ask($"Item moved to new Status {requestedState} . and Listening", null);
            }
            catch (Exception e)
            {
                Log.LogError($"Failed to handle work item state: {e.Message}");
                return ResponseBuilder.Tell($"Exception {e.Message}");
            }
        }
    }
}
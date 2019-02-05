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
    public class AssignToWorkItemAction : AzDoBridgeAction
    {
        public AssignToWorkItemAction(ILogger log) : base(log)
        {
        }

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

                // read the assignee
                string requestAssignToName = intentRequest.GetSlotValue("Fullname");

                // set fields
                workItem.SetFieldValue(WorkItemFieldNames.AssignedTo, requestAssignToName);
                workItem.Save();

                return ResponseBuilder.Ask($"Assigned To {requestAssignToName}. Listening ", null);
            }

            catch (Exception e)
            {
                Log.LogError($"Failed to handle work item assign to action: {e.Message}");
                return ResponseBuilder.Tell($"Exception {e.Message}");
            }
        }
    }
}
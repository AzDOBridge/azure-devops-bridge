using AzDoBridge.Models;
using Microsoft.Extensions.Logging;

namespace AzDoBridge.Actions
{
    public static class AzDoBridgeActionFactory
    {
        public static bool TryGetAction(string intentName, ILogger log, out IAzDoBridgeAction action)
        {
            if (AzDoBridgeIntent.ChangeWiStatus.Is(intentName))
            {
                action = new ChangeWorkItemStateAction(log);
                return true;
            }

            // Work item assign
            if (AzDoBridgeIntent.AssignTo.Is(intentName))
            {
                action = new AssignToWorkItemAction(log);
                return true;
            }

            // Work Item Priority Action
            if (AzDoBridgeIntent.SetPriority.Is(intentName))
            {
                action = action = new SetWorkItemPriorityAction(log);
                return true;
            }

            action = null;
            return false;
        }
    }
}
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace AzDoBridge.Models
{
    public static class WorkItemFieldExtensions
    {
        public static string ReadFieldValue(this WorkItem workItem, string fieldName)
        {
            Field field = workItem.Fields[fieldName];
            return field?.Value?.ToString();
        }

        public static void SetFieldValue(this WorkItem workItem, string fieldName, string value)
        {
            Field field = workItem.Fields[fieldName];
            field.Value = value;
        }
    }
}
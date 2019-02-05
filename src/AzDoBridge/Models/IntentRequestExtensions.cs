using Alexa.NET.Request;
using Alexa.NET.Request.Type;

namespace AzDoBridge.Models
{
    public static class IntentRequestExtensions
    {
        public static string GetSlotValue(this IntentRequest intentRequest, string slotName)
        {
            return !intentRequest.Intent.Slots.TryGetValue(slotName, out Slot slot) ? null : slot.Value;
        }
    }
}
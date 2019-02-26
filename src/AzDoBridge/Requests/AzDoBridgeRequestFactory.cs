using AzDoBridge.Models;
using Microsoft.Extensions.Logging;

namespace AzDoBridge.Requests
{
    public static class AzDoBridgeRequestFactory
    {
        public static bool TryGetRequest(string requestType, ILogger log, out IAzDoBridgeRequest request)
        {
            if (AzDoBridgeRequests.LaunchRequest.Is(requestType))
            {
                //new launch request
                request = new LaunchRequestHandle(log);
                return true;
            }
            if (AzDoBridgeRequests.IntentRequest.Is(requestType))
            {
                //new intent request
                request = new IntentRequestHandle(log);
                return true;
            }
            request = null;
            return false;
        }
    }
}

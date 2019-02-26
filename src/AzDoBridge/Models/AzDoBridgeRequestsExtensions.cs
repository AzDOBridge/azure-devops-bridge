
using Alexa.NET.Request.Type;
using System;

namespace AzDoBridge.Models
{
    public static class AzDoBridgeRequestsExtensions
    {
        public static bool Is(this AzDoBridgeRequests request, string name)
        {
            return request.ToString().Equals(name, StringComparison.OrdinalIgnoreCase);
        }
    }
}

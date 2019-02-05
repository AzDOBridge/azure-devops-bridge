using System;

namespace AzDoBridge.Models
{
    public static class AzDoBridgeIntentsExtensions
    {
        public static bool Is(this AzDoBridgeIntent intent, string name)
        {
            return intent.ToString().Equals(name, StringComparison.OrdinalIgnoreCase);
        }
    }
}
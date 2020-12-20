using System;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace HonestyBar.Configuration.Constants
{
    public static class CorsPolicyName
    {
        public const string AllowConfiguredHosts = nameof(AllowConfiguredHosts);
        public const string AllowAny = nameof(AllowAny);
    }
}
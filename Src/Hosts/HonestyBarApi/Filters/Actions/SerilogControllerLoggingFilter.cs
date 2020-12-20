using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace HonestyBar.Filters.Actions
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class SerilogControllerLoggingFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context != null)
            {
                var diagnosticContext = context.HttpContext.RequestServices.GetService(typeof(IDiagnosticContext)) as IDiagnosticContext;
                diagnosticContext.Set("ActionName", context.ActionDescriptor.DisplayName);
                diagnosticContext.Set("ActionId", context.ActionDescriptor.Id);
            }
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SmartBankStatementAPI.Models;

namespace SmartBankStatementAPI.Filters;

/// <summary>
/// Service availability filter — checks service hours (§2.33)
/// </summary>
public class AvailableFilter : IActionFilter
{
    private readonly IConfiguration _configuration;

    public AvailableFilter(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var controllerName = context.RouteData.Values["controller"]?.ToString() ?? "";
        var actionName = context.RouteData.Values["action"]?.ToString() ?? "";
        var route = $"{controllerName}/{actionName}";

        // ── Skip filter for ignored routes ──
        var ignoreRoutes = _configuration.GetSection("ServiceTime:Ignore").Get<string[]>();
        if (ignoreRoutes is not null && ignoreRoutes.Contains(route, StringComparer.OrdinalIgnoreCase))
        {
            return;
        }

        if (!CheckAvailable())
        {
            var outOfServiceMessage = _configuration["ServiceTime:OutOfServiceMessage"]
                ?? "Service is currently unavailable.";

            context.Result = new ObjectResult(new ResultModel
            {
                Status = 503,
                IsSuccess = false,
                Message = outOfServiceMessage
            })
            {
                StatusCode = 503
            };
        }
    }

    public void OnActionExecuted(ActionExecutedContext context) { }

    private bool CheckAvailable()
    {
        var startConfig = _configuration["ServiceTime:Start"];
        var endConfig = _configuration["ServiceTime:End"];

        if (string.IsNullOrEmpty(startConfig) || string.IsNullOrEmpty(endConfig))
        {
            return true; // no config = always available
        }

        if (!TimeOnly.TryParse(startConfig, out var startTime) ||
            !TimeOnly.TryParse(endConfig, out var endTime))
        {
            return true;
        }

        var now = TimeOnly.FromDateTime(DateTime.Now);
        return now >= startTime && now <= endTime;
    }
}

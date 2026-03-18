using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SmartBankStatementAPI.Filters;

/// <summary>
/// Action filter for automatic request/response logging (§8.7, §2.33)
/// </summary>
public class LogFilter : IActionFilter
{
    private readonly ILogger<LogFilter> _logger;

    public LogFilter(ILogger<LogFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var controllerName = context.RouteData.Values["controller"]?.ToString();
        var actionName = context.RouteData.Values["action"]?.ToString();

        _logger.LogInformation(
            "Request: {Method} {Controller}/{Action} | Params: {Params}",
            context.HttpContext.Request.Method,
            controllerName,
            actionName,
            context.ActionArguments);

        context.HttpContext.Items["Stopwatch"] = Stopwatch.StartNew();
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        var controllerName = context.RouteData.Values["controller"]?.ToString();
        var actionName = context.RouteData.Values["action"]?.ToString();
        var statusCode = context.HttpContext.Response.StatusCode;

        var elapsed = "N/A";
        if (context.HttpContext.Items["Stopwatch"] is Stopwatch sw)
        {
            sw.Stop();
            elapsed = $"{sw.ElapsedMilliseconds}ms";
        }

        _logger.LogInformation(
            "Response: {Controller}/{Action} | Status: {StatusCode} | Elapsed: {Elapsed}",
            controllerName,
            actionName,
            statusCode,
            elapsed);
    }
}

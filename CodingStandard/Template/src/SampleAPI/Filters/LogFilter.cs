// §8.7 — LogFilter: Action Filter auto log ทุก Request/Response
// §1.6 — อยู่ใน Filters/ แยกจาก Controller

using Microsoft.AspNetCore.Mvc.Filters;

namespace SampleAPI.Filters;

/// <summary>
/// Auto Log ทุก HTTP Request/Response เข้า-ออก Controller
/// §16.1 — Structured Logging (ห้าม string concat)
/// </summary>
public class LogFilter : IAsyncActionFilter
{
    private readonly ILogger<LogFilter> _logger;

    public LogFilter(ILogger<LogFilter> logger)
    {
        _logger = logger;
    }

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var controllerName = context.RouteData.Values["controller"]?.ToString();
        var actionName = context.RouteData.Values["action"]?.ToString();
        var method = context.HttpContext.Request.Method;
        var path = context.HttpContext.Request.Path.Value;

        // §16.1 — Structured Logging with properties
        _logger.LogInformation(
            "Request {Method} {Path} → {Controller}.{Action}",
            method, path, controllerName, actionName);

        var executedContext = await next();

        if (executedContext.Exception is not null)
        {
            _logger.LogError(executedContext.Exception,
                "Response {Method} {Path} → Exception in {Controller}.{Action}",
                method, path, controllerName, actionName);
        }
        else
        {
            var statusCode = context.HttpContext.Response.StatusCode;
            _logger.LogInformation(
                "Response {Method} {Path} → {StatusCode}",
                method, path, statusCode);
        }
    }
}

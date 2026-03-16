// §16.2 — CorrelationId Middleware: ทุก Request ต้องมี CorrelationId
// §1.10 — อยู่ใน Middleware/

namespace SampleAPI.Middleware;

/// <summary>
/// Generate/Forward X-Correlation-Id สำหรับ Distributed Tracing
/// §16.2 — generate UUID ถ้าไม่มีใน Header
/// §18.8 — ส่งต่อไปกับ Outbound Request ด้วย
/// </summary>
public class CorrelationIdMiddleware
{
    private const string CorrelationIdHeader = "X-Correlation-Id";
    private readonly RequestDelegate _next;
    private readonly ILogger<CorrelationIdMiddleware> _logger;

    public CorrelationIdMiddleware(
        RequestDelegate next,
        ILogger<CorrelationIdMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // ดึง CorrelationId จาก Header หรือ generate ใหม่
        var correlationId = context.Request.Headers[CorrelationIdHeader].FirstOrDefault()
            ?? Guid.NewGuid().ToString("N");

        // เก็บใน HttpContext.Items สำหรับ Service layer
        context.Items["CorrelationId"] = correlationId;

        // ส่งกลับใน Response Header
        context.Response.Headers.Append(CorrelationIdHeader, correlationId);

        // §16.1 — Structured Log Scope ให้ทุก log ใน request มี CorrelationId
        using (_logger.BeginScope(new Dictionary<string, object>
            { ["CorrelationId"] = correlationId }))
        {
            await _next(context);
        }
    }
}

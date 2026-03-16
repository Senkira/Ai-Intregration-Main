// §1.6 — AvailableFilter: Action Filter ตรวจสอบ Service Availability
// §25 — FeatureFlags ควบคุมผ่าน Configuration

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SampleAPI.Filters;

/// <summary>
/// Filter ตรวจสอบ Service Availability — return 503 เมื่อ Service ปิดซ่อมบำรุง
/// </summary>
public class AvailableFilter : IAsyncActionFilter
{
    private readonly IConfiguration _configuration;

    public AvailableFilter(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // §4.8 — ค่า config จาก appsettings ห้าม hard-code
        bool isAvailable = _configuration.GetValue("ServiceAvailable", true);

        if (!isAvailable)
        {
            context.Result = new ObjectResult(new
            {
                Status = 503,
                IsSuccess = false,
                Message = _configuration["Messages:ServiceUnavailable"]
            })
            {
                StatusCode = 503
            };
            return;
        }

        await next();
    }
}

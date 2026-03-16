// §5.6 — HealthCheckController: อย่างน้อย 1 endpoint ไม่ต้อง Auth
// K8s liveness/readiness probe

using Microsoft.AspNetCore.Mvc;

namespace SampleAPI.Controllers;

/// <summary>
/// Health Check API — K8s / Docker Health Probe
/// §16.6 — HealthCheck ต้องตรวจ Self (liveness)
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class HealthCheckController : ControllerBase
{
    /// <summary>
    /// Liveness Probe — ตรวจว่า Application ยังทำงานอยู่
    /// </summary>
    /// <returns>200 OK</returns>
    [HttpGet]
    public IActionResult Get() =>
        Ok(new { status = "Healthy", timestamp = DateTime.UtcNow });
}

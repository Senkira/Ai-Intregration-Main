using System.Net;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using SmartBankStatementAPI.Models;

namespace SmartBankStatementAPI.Controllers;

/// <summary>
/// Health check endpoint — no authentication required (§5.6)
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class HealthCheckController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public HealthCheckController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Health check — returns environment, build date, hostname
    /// </summary>
    [HttpGet]
    public HealthCheckModel Index()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var file = new FileInfo(assembly.Location);
        var dateModified = file.LastWriteTime.ToString("dd/MM/yyyy : HH:mm:ss");
        var hostName = Dns.GetHostName();

        return new HealthCheckModel
        {
            Environment = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
            DateModified = dateModified,
            IpHost = hostName
        };
    }
}

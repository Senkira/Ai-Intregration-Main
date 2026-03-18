namespace __PROJECT_NAME__API.Models;

/// <summary>
/// Health check response model
/// </summary>
public class HealthCheckModel
{
    public string? Environment { get; set; }
    public string? DateModified { get; set; }
    public string? IpHost { get; set; }
}

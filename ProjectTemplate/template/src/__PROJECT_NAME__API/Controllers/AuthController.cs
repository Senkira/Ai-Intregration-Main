using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using __PROJECT_NAME__API.Models.Requests;
using __PROJECT_NAME__API.Services.Interfaces;

namespace __PROJECT_NAME__API.Controllers;

/// <summary>
/// Authentication API endpoints (§5, §7)
/// </summary>
[Produces("application/json")]
[Route("api/[controller]/[action]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Login to get JWT token
    /// </summary>
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> LoginAsync(
        LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.LoginAsync(request, cancellationToken);
        return StatusCode(result.Status, result);
    }
}

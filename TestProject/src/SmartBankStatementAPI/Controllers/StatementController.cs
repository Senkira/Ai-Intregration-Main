using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartBankStatementAPI.Models.Requests;
using SmartBankStatementAPI.Services.Interfaces;

namespace SmartBankStatementAPI.Controllers;

/// <summary>
/// Statement API endpoints (§5)
/// </summary>
[Produces("application/json")]
[Route("api/[controller]/[action]")]
[ApiController]
public class StatementController : ControllerBase
{
    private readonly IStatementService _statementService;

    public StatementController(IStatementService statementService)
    {
        _statementService = statementService;
    }

    /// <summary>
    /// Get statements by contract number
    /// </summary>
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> GetStatementAsync(
        GetStatementRequest request, CancellationToken cancellationToken)
    {
        var result = await _statementService.GetStatementAsync(
            request, cancellationToken);
        return StatusCode(result.Status, result);
    }

    /// <summary>
    /// Get statement by ID
    /// </summary>
    [Authorize]
    [HttpGet("{statementId}")]
    public async Task<IActionResult> GetStatementByIdAsync(
        int statementId, CancellationToken cancellationToken)
    {
        var result = await _statementService.GetStatementByIdAsync(
            statementId, cancellationToken);
        return StatusCode(result.Status, result);
    }
}

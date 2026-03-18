using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartBankStatementAPI.Models.Requests;
using SmartBankStatementAPI.Services.Interfaces;

namespace SmartBankStatementAPI.Controllers;

/// <summary>
/// Customer API endpoints (§5)
/// </summary>
[Produces("application/json")]
[Route("api/[controller]/[action]")]
[ApiController]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    /// <summary>
    /// Get customer by ID
    /// </summary>
    [Authorize]
    [HttpGet("{customerId}")]
    public async Task<IActionResult> GetCustomerAsync(
        string customerId, CancellationToken cancellationToken)
    {
        var result = await _customerService.GetCustomerAsync(
            customerId, cancellationToken);
        return StatusCode(result.Status, result);
    }

    /// <summary>
    /// Get all active customers
    /// </summary>
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetActiveCustomersAsync(
        CancellationToken cancellationToken)
    {
        var result = await _customerService.GetActiveCustomersAsync(cancellationToken);
        return StatusCode(result.Status, result);
    }

    /// <summary>
    /// Create a new customer
    /// </summary>
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateCustomerAsync(
        CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        var result = await _customerService.CreateCustomerAsync(
            request, cancellationToken);
        return StatusCode(result.Status, result);
    }
}

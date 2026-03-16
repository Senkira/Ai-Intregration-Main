// §5 — Controller Layer
// §5.1 — สืบทอด ControllerBase (ไม่ใช่ Controller)
// §5.2 — [ApiController], [Route], [Produces]
// §5.3 — รับ IService ผ่าน Constructor ห้ามมี Business Logic
// §5.7 — Controller บาง: รับ → เรียก → ส่ง (1-3 บรรทัดต่อ action)

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SampleAPI.Models;
using SampleAPI.Models.Requests;
using SampleAPI.Services.Interfaces;

namespace SampleAPI.Controllers;

/// <summary>
/// Customer API — CRUD operations สำหรับลูกค้า
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]  // §13.1 — /api/v{version}/{resource}
[Produces("application/json")]
public class CustomerController : ControllerBase
{
    // §5.3 — inject IService ผ่าน Constructor
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    /// <summary>
    /// ดึงข้อมูลลูกค้าจาก ID
    /// </summary>
    /// <param name="customerId">รหัสลูกค้า</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Customer data หรือ Error</returns>
    [Authorize]  // §5.4 — ทุก Endpoint ที่ต้อง Auth
    [HttpGet("{customerId}")]  // §5.5 — HTTP Verb attribute
    public async Task<IActionResult> GetCustomerAsync(
        string customerId, CancellationToken cancellationToken)  // §5.8 — CT
    {
        // §5.7 — Controller บาง: เรียก Service → return StatusCode
        var result = await _customerService.GetCustomerAsync(
            customerId, cancellationToken);
        return StatusCode(result.Status, result);
    }

    /// <summary>
    /// ดึงรายการลูกค้าแบบ Pagination
    /// </summary>
    /// <param name="pageNumber">หน้าที่ต้องการ (default: 1)</param>
    /// <param name="pageSize">จำนวนต่อหน้า (default: 20, max: 100)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetCustomersAsync(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _customerService.GetCustomersAsync(
            pageNumber, pageSize, cancellationToken);
        return StatusCode(result.Status, result);
    }

    /// <summary>
    /// สร้างลูกค้าใหม่
    /// </summary>
    /// <param name="request">ข้อมูลลูกค้า</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [Authorize]
    [HttpPost]  // §13.2 — POST สำหรับสร้าง
    public async Task<IActionResult> CreateCustomerAsync(
        CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        var result = await _customerService.CreateCustomerAsync(
            request, cancellationToken);
        return StatusCode(result.Status, result);
    }
}

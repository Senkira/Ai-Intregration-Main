// §4.1 — ทุก Service ต้องมี Interface (SOLID — Dependency Inversion)
// §1.3 — อยู่ใน Services/Interfaces/

using SampleAPI.Models;
using SampleAPI.Models.Requests;

namespace SampleAPI.Services.Interfaces;

/// <summary>
/// Customer Service Interface — Business Logic สำหรับ Customer Feature
/// §11.4 — ทุก Public Method ต้องมี XML Comment
/// </summary>
public interface ICustomerService
{
    /// <summary>
    /// ดึงข้อมูลลูกค้าจาก ID
    /// </summary>
    /// <param name="customerId">รหัสลูกค้า</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>ResultModel พร้อม CustomerResponse หรือ Error</returns>
    Task<ResultModel<CustomerResponse>> GetCustomerAsync(
        string customerId, CancellationToken cancellationToken = default);

    /// <summary>
    /// ดึงรายการลูกค้าแบบ Pagination
    /// </summary>
    /// <param name="pageNumber">หน้าที่ต้องการ (§15.8 — ≥ 1)</param>
    /// <param name="pageSize">จำนวนต่อหน้า (§15.8 — 1-100)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<ResultModel<PagedResponse<CustomerResponse>>> GetCustomersAsync(
        int pageNumber, int pageSize, CancellationToken cancellationToken = default);

    /// <summary>
    /// สร้างลูกค้าใหม่
    /// </summary>
    /// <param name="request">ข้อมูลลูกค้าที่ต้องการสร้าง</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<ResultModel> CreateCustomerAsync(
        CreateCustomerRequest request, CancellationToken cancellationToken = default);
}

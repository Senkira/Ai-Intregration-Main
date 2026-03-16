// §3.2 — Repository Pattern: Interface สำหรับ Customer Feature
// §1.5 — อยู่ใน Repositories/Interfaces/

using SampleAPI.Models.Entities;

namespace SampleAPI.DataAccess.Repositories.Interfaces;

/// <summary>
/// Customer Repository Interface — Mock ได้ใน Unit Test (§12.10)
/// </summary>
public interface ICustomerRepository
{
    /// <summary>
    /// ดึงข้อมูลลูกค้าจาก ID
    /// </summary>
    Task<CustomerEntity?> GetByIdAsync(
        string customerId, CancellationToken cancellationToken = default);

    /// <summary>
    /// ดึงรายการลูกค้าแบบ Pagination (§13.4)
    /// </summary>
    Task<IEnumerable<CustomerEntity>> GetPagedAsync(
        int pageNumber, int pageSize, CancellationToken cancellationToken = default);

    /// <summary>
    /// นับจำนวนลูกค้าทั้งหมด (สำหรับ Pagination metadata)
    /// </summary>
    Task<int> CountAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// สร้างลูกค้าใหม่ (§3.27 — INSERT ต้องมี Audit Log)
    /// </summary>
    Task<int> CreateAsync(
        CustomerEntity entity, CancellationToken cancellationToken = default);
}

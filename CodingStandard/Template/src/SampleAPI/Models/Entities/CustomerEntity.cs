// §1.2 — Entity: DB Mapping Model (อยู่ใน Models/Entities/)
// §2 — Naming: {Name}Entity, PascalCase properties

namespace SampleAPI.Models.Entities;

/// <summary>
/// Customer Entity — Map ตรงกับ Customer Table ใน Database
/// ห้ามใช้เป็น API Response ตรง → ต้อง Map เป็น DTO ก่อน (§4.7)
/// </summary>
public class CustomerEntity
{
    public string CustomerId { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public bool IsActive { get; set; }  // §2 — Boolean = Is/Has/Can + Adj
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}

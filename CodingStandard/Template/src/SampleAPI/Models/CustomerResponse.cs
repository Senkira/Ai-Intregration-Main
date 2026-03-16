// §4.7 — Response DTO: ห้าม return Entity ตรง
// §2 — Naming: {Feature}Response

namespace SampleAPI.Models;

/// <summary>
/// Customer API Response DTO — Map จาก CustomerEntity
/// Client เห็นเฉพาะ field ที่จำเป็น (§3.11 Least Privilege concept)
/// </summary>
public class CustomerResponse
{
    public string CustomerId { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public bool IsActive { get; set; }
}

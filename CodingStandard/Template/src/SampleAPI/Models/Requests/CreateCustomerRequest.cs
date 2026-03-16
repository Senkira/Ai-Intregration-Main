// §1.2 — Request DTO: Input Model (อยู่ใน Models/Requests/)
// §2 — Naming: {Action}{Feature}Request
// §13.11 — Strongly-typed DTO ห้ามรับ string/int เดี่ยว

namespace SampleAPI.Models.Requests;

/// <summary>
/// Request DTO สำหรับสร้างลูกค้าใหม่
/// Validation Rules อยู่ใน CreateCustomerRequestValidator (§15.4)
/// </summary>
public class CreateCustomerRequest
{
    public string CustomerName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
}

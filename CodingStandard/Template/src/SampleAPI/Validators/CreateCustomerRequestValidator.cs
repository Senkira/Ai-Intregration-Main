// §15.4 — Validator Class แยก (ห้ามเขียน if-else ยาวใน Service)
// §15.1 — FluentValidation เป็น Primary Validation

using FluentValidation;
using SampleAPI.Models.Requests;

namespace SampleAPI.Validators;

/// <summary>
/// Validation Rules สำหรับ CreateCustomerRequest
/// §15.2 — Validate ที่ Controller Level ก่อนเข้า Service
/// </summary>
public class CreateCustomerRequestValidator : AbstractValidator<CreateCustomerRequest>
{
    public CreateCustomerRequestValidator()
    {
        // §15.3 — Required + String Length
        RuleFor(x => x.CustomerName)
            .NotEmpty().WithMessage("CustomerName is required.")
            .MaximumLength(200).WithMessage("CustomerName must not exceed 200 characters.");

        // §15.3 — Required + Email Format
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email format is invalid.")
            .MaximumLength(256).WithMessage("Email must not exceed 256 characters.");

        // §15.3 — Regex Pattern (optional field)
        RuleFor(x => x.PhoneNumber)
            .Matches(@"^\d{9,10}$").WithMessage("PhoneNumber must be 9-10 digits.")
            .When(x => !string.IsNullOrEmpty(x.PhoneNumber));
    }
}

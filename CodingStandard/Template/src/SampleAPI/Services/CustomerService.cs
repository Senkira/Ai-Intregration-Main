// §4 — Service Layer: Business Logic Implementation
// §4.2 — Constructor Injection เท่านั้น ห้าม new
// §4.3 — ใช้ IRepository เข้าถึง DB
// §4.4 — async + CancellationToken ทุก I/O method

using SampleAPI.DataAccess.Repositories.Interfaces;
using SampleAPI.Models;
using SampleAPI.Models.Entities;
using SampleAPI.Models.Requests;
using SampleAPI.Services.Interfaces;

namespace SampleAPI.Services;

/// <summary>
/// Customer Service — Business Logic
/// Registered เป็น Scoped (§6.4)
/// </summary>
public class CustomerService : ICustomerService
{
    // §2 — Private field: _camelCase
    private readonly ICustomerRepository _customerRepository;
    private readonly ILogger<CustomerService> _logger;
    private readonly IConfiguration _configuration;

    // §4.2 — Constructor Injection
    public CustomerService(
        ICustomerRepository customerRepository,
        ILogger<CustomerService> logger,
        IConfiguration configuration)
    {
        _customerRepository = customerRepository;
        _logger = logger;
        _configuration = configuration;
    }

    /// <inheritdoc />
    public async Task<ResultModel<CustomerResponse>> GetCustomerAsync(
        string customerId, CancellationToken cancellationToken = default)
    {
        // §4.5 — Guard Clause (null/empty check) ก่อน Business Logic
        if (string.IsNullOrWhiteSpace(customerId))
        {
            return new ResultModel<CustomerResponse>
            {
                Status = 400,
                IsSuccess = false,
                // §9.6 — Message จาก Configuration ห้าม Hard-code
                Message = _configuration["Messages:CustomerIdRequired"]
            };
        }

        try
        {
            // §15.6 — Trim() whitespace ก่อนใช้งาน
            customerId = customerId.Trim();

            // §4.3 — เข้าถึง DB ผ่าน Repository เท่านั้น
            var entity = await _customerRepository.GetByIdAsync(
                customerId, cancellationToken);

            if (entity is null)
            {
                return new ResultModel<CustomerResponse>
                {
                    Status = 404,
                    IsSuccess = false,
                    Message = _configuration["Messages:CustomerNotFound"]
                };
            }

            // §4.7 — Map Entity → Response DTO (ห้าม return Entity ตรง)
            var response = MapToResponse(entity);

            return new ResultModel<CustomerResponse>
            {
                Status = 200,
                IsSuccess = true,
                Data = response
            };
        }
        catch (Exception ex)
        {
            // §4.6 — try-catch + Structured Logging
            // §8.2 — Log Error + Context (Method Name, Parameters)
            // §16.1 — Structured Logging (ห้าม string concat)
            _logger.LogError(ex,
                "Error in {Method} for CustomerId={CustomerId}",
                nameof(GetCustomerAsync), customerId);

            // §8.6 — ห้าม return Exception/StackTrace ให้ Client
            return new ResultModel<CustomerResponse>
            {
                Status = 500,
                IsSuccess = false,
                Message = _configuration["Messages:InternalServerError"]
            };
        }
    }

    /// <inheritdoc />
    public async Task<ResultModel<PagedResponse<CustomerResponse>>> GetCustomersAsync(
        int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        try
        {
            var entities = await _customerRepository.GetPagedAsync(
                pageNumber, pageSize, cancellationToken);

            int totalCount = await _customerRepository.CountAsync(cancellationToken);

            // §13.5 — Pagination Response
            var pagedResponse = new PagedResponse<CustomerResponse>
            {
                Data = entities.Select(MapToResponse).ToList(),
                Pagination = new PaginationMeta
                {
                    Page = pageNumber,
                    PageSize = pageSize,
                    TotalCount = totalCount
                }
            };

            return new ResultModel<PagedResponse<CustomerResponse>>
            {
                Status = 200,
                IsSuccess = true,
                Data = pagedResponse
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error in {Method} for Page={PageNumber}, Size={PageSize}",
                nameof(GetCustomersAsync), pageNumber, pageSize);

            return new ResultModel<PagedResponse<CustomerResponse>>
            {
                Status = 500,
                IsSuccess = false,
                Message = _configuration["Messages:InternalServerError"]
            };
        }
    }

    /// <inheritdoc />
    public async Task<ResultModel> CreateCustomerAsync(
        CreateCustomerRequest request, CancellationToken cancellationToken = default)
    {
        // §4.5 — Guard Clause
        if (request is null)
        {
            return new ResultModel
            {
                Status = 400,
                IsSuccess = false,
                Message = "Request body is required."
            };
        }

        try
        {
            // §4.7 — Map Request DTO → Entity
            var entity = new CustomerEntity
            {
                CustomerId = Guid.NewGuid().ToString("N"),
                CustomerName = request.CustomerName.Trim(),  // §15.6 Trim
                Email = request.Email.Trim(),
                PhoneNumber = request.PhoneNumber?.Trim(),
                IsActive = true,
                CreatedDate = DateTime.UtcNow  // §13.9 — UTC
            };

            await _customerRepository.CreateAsync(entity, cancellationToken);

            _logger.LogInformation(
                "Customer created: CustomerId={CustomerId}, Name={CustomerName}",
                entity.CustomerId, entity.CustomerName);

            return new ResultModel
            {
                Status = 201,
                IsSuccess = true,
                Message = "Customer created successfully."
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error in {Method} for Email={Email}",
                nameof(CreateCustomerAsync), request.Email);

            return new ResultModel
            {
                Status = 500,
                IsSuccess = false,
                Message = _configuration["Messages:InternalServerError"]
            };
        }
    }

    // ── Private Helper — Map Entity → DTO ──

    /// <summary>
    /// §4.7 — Map CustomerEntity → CustomerResponse
    /// </summary>
    private static CustomerResponse MapToResponse(CustomerEntity entity) =>
        new()
        {
            CustomerId = entity.CustomerId,
            CustomerName = entity.CustomerName,
            Email = entity.Email,
            PhoneNumber = entity.PhoneNumber,
            IsActive = entity.IsActive
        };
}

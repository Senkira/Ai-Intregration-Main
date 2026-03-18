using SmartBankStatementAPI.DataAccess.Repositories.Interfaces;
using SmartBankStatementAPI.Models;
using SmartBankStatementAPI.Models.Entities;
using SmartBankStatementAPI.Models.Requests;
using SmartBankStatementAPI.Services.Interfaces;

namespace SmartBankStatementAPI.Services;

/// <summary>
/// Customer service — Business Logic (§4)
/// </summary>
public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ILogger<CustomerService> _logger;

    public CustomerService(
        ICustomerRepository customerRepository,
        ILogger<CustomerService> logger)
    {
        _customerRepository = customerRepository;
        _logger = logger;
    }

    public async Task<ResultModel<CustomerResponse>> GetCustomerAsync(
        string customerId, CancellationToken cancellationToken = default)
    {
        var result = new ResultModel<CustomerResponse>();
        try
        {
            // ── Guard Clause (§4.5) ──
            if (string.IsNullOrWhiteSpace(customerId))
            {
                result.Status = 400;
                result.IsSuccess = false;
                result.Message = "CustomerId is required.";
                return result;
            }

            var entity = await _customerRepository.GetByIdAsync(
                customerId, cancellationToken);

            if (entity is null)
            {
                result.Status = 404;
                result.IsSuccess = false;
                result.Message = "Customer not found.";
                return result;
            }

            result.Status = 200;
            result.IsSuccess = true;
            result.Data = MapToResponse(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetCustomerAsync for {CustomerId}", customerId);
            result.Status = 500;
            result.IsSuccess = false;
            result.Message = "Internal server error.";
        }
        return result;
    }

    public async Task<ResultModel<IEnumerable<CustomerResponse>>> GetActiveCustomersAsync(
        CancellationToken cancellationToken = default)
    {
        var result = new ResultModel<IEnumerable<CustomerResponse>>();
        try
        {
            var entities = await _customerRepository.GetActiveListAsync(cancellationToken);
            result.Status = 200;
            result.IsSuccess = true;
            result.Data = entities.Select(MapToResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetActiveCustomersAsync");
            result.Status = 500;
            result.IsSuccess = false;
            result.Message = "Internal server error.";
        }
        return result;
    }

    public async Task<ResultModel> CreateCustomerAsync(
        CreateCustomerRequest request, CancellationToken cancellationToken = default)
    {
        var result = new ResultModel();
        try
        {
            // ── Guard Clause (§4.5) ──
            if (string.IsNullOrWhiteSpace(request.CustomerId))
            {
                result.Status = 400;
                result.IsSuccess = false;
                result.Message = "CustomerId is required.";
                return result;
            }

            // ── Check duplicate ──
            var existing = await _customerRepository.GetByIdAsync(
                request.CustomerId, cancellationToken);

            if (existing is not null)
            {
                result.Status = 409;
                result.IsSuccess = false;
                result.Message = "Customer already exists.";
                return result;
            }

            var entity = new CustomerEntity
            {
                CustomerId = request.CustomerId,
                CustomerName = request.CustomerName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Status = "ACTIVE",
                CreatedDate = DateTime.UtcNow
            };

            await _customerRepository.InsertAsync(entity, cancellationToken);

            _logger.LogInformation("Created customer {CustomerId}: {CustomerName}",
                entity.CustomerId, entity.CustomerName);

            result.Status = 201;
            result.IsSuccess = true;
            result.Message = "Customer created successfully.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in CreateCustomerAsync for {CustomerId}",
                request.CustomerId);
            result.Status = 500;
            result.IsSuccess = false;
            result.Message = "Internal server error.";
        }
        return result;
    }

    // ── Entity → Response Mapping (§4.7) ──
    private static CustomerResponse MapToResponse(CustomerEntity entity) => new()
    {
        CustomerId = entity.CustomerId,
        CustomerName = entity.CustomerName,
        Email = entity.Email,
        PhoneNumber = entity.PhoneNumber,
        Status = entity.Status
    };
}

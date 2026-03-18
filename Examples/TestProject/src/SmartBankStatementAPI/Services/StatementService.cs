using SmartBankStatementAPI.DataAccess.Repositories.Interfaces;
using SmartBankStatementAPI.Models;
using SmartBankStatementAPI.Models.Entities;
using SmartBankStatementAPI.Models.Requests;
using SmartBankStatementAPI.Services.Interfaces;

namespace SmartBankStatementAPI.Services;

/// <summary>
/// Statement service — Business Logic (§4)
/// </summary>
public class StatementService : IStatementService
{
    private readonly IStatementRepository _statementRepository;
    private readonly ILogger<StatementService> _logger;

    public StatementService(
        IStatementRepository statementRepository,
        ILogger<StatementService> logger)
    {
        _statementRepository = statementRepository;
        _logger = logger;
    }

    public async Task<ResultModel<IEnumerable<StatementResponse>>> GetStatementAsync(
        GetStatementRequest request, CancellationToken cancellationToken = default)
    {
        var result = new ResultModel<IEnumerable<StatementResponse>>();
        try
        {
            // ── Guard Clause (§4.5) ──
            if (string.IsNullOrWhiteSpace(request.ContractNo))
            {
                result.Status = 400;
                result.IsSuccess = false;
                result.Message = "ContractNo is required.";
                return result;
            }

            // ── Business Logic ──
            var entities = await _statementRepository.GetByContractNoAsync(
                request.ContractNo, cancellationToken);

            var data = entities.Select(MapToResponse);

            result.Status = 200;
            result.IsSuccess = true;
            result.Data = data;
        }
        catch (Exception ex)
        {
            // ── Structured Logging (§4.6, §8.2) ──
            _logger.LogError(ex, "Error in GetStatementAsync for ContractNo: {ContractNo}",
                request.ContractNo);
            result.Status = 500;
            result.IsSuccess = false;
            result.Message = "Internal server error.";
        }
        return result;
    }

    public async Task<ResultModel<StatementResponse>> GetStatementByIdAsync(
        int statementId, CancellationToken cancellationToken = default)
    {
        var result = new ResultModel<StatementResponse>();
        try
        {
            var entity = await _statementRepository.GetByIdAsync(
                statementId, cancellationToken);

            if (entity is null)
            {
                result.Status = 404;
                result.IsSuccess = false;
                result.Message = "Statement not found.";
                return result;
            }

            result.Status = 200;
            result.IsSuccess = true;
            result.Data = MapToResponse(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetStatementByIdAsync for StatementId: {StatementId}",
                statementId);
            result.Status = 500;
            result.IsSuccess = false;
            result.Message = "Internal server error.";
        }
        return result;
    }

    // ── Entity → Response Mapping (§4.7) ──
    private static StatementResponse MapToResponse(StatementEntity entity) => new()
    {
        StatementId = entity.StatementId,
        ContractNo = entity.ContractNo,
        CutOffDate = entity.CutOffDate,
        AsOfDate = entity.AsOfDate,
        FilePath = entity.FilePath,
        Status = entity.Status
    };
}

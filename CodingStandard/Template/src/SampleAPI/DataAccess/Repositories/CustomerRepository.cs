// §3.2 — CustomerRepository: inherit BaseRepository + implement ICustomerRepository
// §3.6 — ใช้ @parameter เท่านั้น ห้าม String Interpolation SQL
// §3.11 — ห้าม SELECT * ระบุ Column เท่านั้น

using SampleAPI.DataAccess.Connections;
using SampleAPI.DataAccess.Repositories.Interfaces;
using SampleAPI.Models.Entities;

namespace SampleAPI.DataAccess.Repositories;

/// <summary>
/// Customer Repository — Data Access สำหรับ Customer Feature
/// Registered เป็น Scoped (§6.4)
/// </summary>
public class CustomerRepository : BaseRepository, ICustomerRepository
{
    public CustomerRepository(IDbConnectionFactory connectionFactory)
        : base(connectionFactory)
    {
    }

    /// <inheritdoc />
    public async Task<CustomerEntity?> GetByIdAsync(
        string customerId, CancellationToken cancellationToken = default)
    {
        // §3.6 — ใช้ @parameter ห้าม string interpolation
        // §3.11 — ระบุ Column ห้าม SELECT *
        // §3.8 — Column Alias = Property Name สำหรับ Dapper auto-map
        const string sql = """
            SELECT
                CustomerId,
                CustomerName,
                Email,
                PhoneNumber,
                IsActive,
                CreatedDate,
                UpdatedDate
            FROM Customer
            WHERE CustomerId = @CustomerId
            """;

        return await QueryFirstOrDefaultAsync<CustomerEntity>(
            sql,
            new { CustomerId = customerId },
            cancellationToken: cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<CustomerEntity>> GetPagedAsync(
        int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        // §13.4 — Pagination ด้วย OFFSET FETCH
        const string sql = """
            SELECT
                CustomerId,
                CustomerName,
                Email,
                PhoneNumber,
                IsActive,
                CreatedDate,
                UpdatedDate
            FROM Customer
            ORDER BY CreatedDate DESC
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY
            """;

        int offset = (pageNumber - 1) * pageSize;

        return await QueryAsync<CustomerEntity>(
            sql,
            new { Offset = offset, PageSize = pageSize },
            cancellationToken: cancellationToken);
    }

    /// <inheritdoc />
    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        const string sql = "SELECT COUNT(1) FROM Customer";

        return await QueryFirstOrDefaultAsync<int>(
            sql,
            cancellationToken: cancellationToken);
    }

    /// <inheritdoc />
    public async Task<int> CreateAsync(
        CustomerEntity entity, CancellationToken cancellationToken = default)
    {
        // §3.27 — INSERT ต้อง log Who/What/When (ผ่าน Service layer)
        const string sql = """
            INSERT INTO Customer (
                CustomerId, CustomerName, Email,
                PhoneNumber, IsActive, CreatedDate
            )
            VALUES (
                @CustomerId, @CustomerName, @Email,
                @PhoneNumber, @IsActive, @CreatedDate
            )
            """;

        return await ExecuteAsync(
            sql,
            entity,
            cancellationToken: cancellationToken);
    }
}

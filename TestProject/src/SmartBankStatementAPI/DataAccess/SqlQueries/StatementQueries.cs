namespace SmartBankStatementAPI.DataAccess.SqlQueries;

/// <summary>
/// SQL queries for Statement feature (§3.7, §3.11: no SELECT *)
/// </summary>
public static class StatementQueries
{
    public const string GetByContractNo = @"
        SELECT statement_id  AS StatementId,
               contract_no   AS ContractNo,
               cut_off_date  AS CutOffDate,
               as_of_date    AS AsOfDate,
               file_path     AS FilePath,
               status        AS Status,
               created_date  AS CreatedDate,
               updated_date  AS UpdatedDate
        FROM   statement
        WHERE  contract_no = @ContractNo
        ORDER BY cut_off_date DESC";

    public const string GetById = @"
        SELECT statement_id  AS StatementId,
               contract_no   AS ContractNo,
               cut_off_date  AS CutOffDate,
               as_of_date    AS AsOfDate,
               file_path     AS FilePath,
               status        AS Status,
               created_date  AS CreatedDate,
               updated_date  AS UpdatedDate
        FROM   statement
        WHERE  statement_id = @StatementId";

    public const string Insert = @"
        INSERT INTO statement (contract_no, cut_off_date, as_of_date, file_path, status, created_date)
        VALUES (@ContractNo, @CutOffDate, @AsOfDate, @FilePath, @Status, @CreatedDate)";
}

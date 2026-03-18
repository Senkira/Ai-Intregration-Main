namespace SmartBankStatementAPI.DataAccess.SqlQueries;

/// <summary>
/// SQL queries for Customer feature (§3.7, §3.11: no SELECT *)
/// </summary>
public static class CustomerQueries
{
    public const string GetById = @"
        SELECT customer_id    AS CustomerId,
               customer_name  AS CustomerName,
               email          AS Email,
               phone_number   AS PhoneNumber,
               status         AS Status,
               created_date   AS CreatedDate,
               updated_date   AS UpdatedDate
        FROM   customer
        WHERE  customer_id = @CustomerId";

    public const string GetActiveList = @"
        SELECT customer_id    AS CustomerId,
               customer_name  AS CustomerName,
               email          AS Email,
               phone_number   AS PhoneNumber,
               status         AS Status
        FROM   customer
        WHERE  status = 'ACTIVE'
        ORDER BY customer_name";

    public const string Insert = @"
        INSERT INTO customer (customer_id, customer_name, email, phone_number, status, created_date)
        VALUES (@CustomerId, @CustomerName, @Email, @PhoneNumber, @Status, @CreatedDate)";

    public const string Update = @"
        UPDATE customer
        SET    customer_name = @CustomerName,
               email         = @Email,
               phone_number  = @PhoneNumber,
               updated_date  = @UpdatedDate
        WHERE  customer_id   = @CustomerId";
}

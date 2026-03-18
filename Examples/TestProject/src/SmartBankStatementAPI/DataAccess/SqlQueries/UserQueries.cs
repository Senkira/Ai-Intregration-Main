namespace SmartBankStatementAPI.DataAccess.SqlQueries;

/// <summary>
/// SQL queries for User/Auth feature (§3.7, §3.11: no SELECT *)
/// </summary>
public static class UserQueries
{
    public const string GetByUserName = @"
        SELECT user_id        AS UserId,
               user_name      AS UserName,
               password_hash  AS PasswordHash,
               role           AS Role,
               is_active      AS IsActive,
               created_date   AS CreatedDate,
               last_login_date AS LastLoginDate
        FROM   app_user
        WHERE  user_name = @UserName";

    public const string Insert = @"
        INSERT INTO app_user (user_name, password_hash, role, is_active, created_date)
        VALUES (@UserName, @PasswordHash, @Role, @IsActive, @CreatedDate)";

    public const string UpdateLastLogin = @"
        UPDATE app_user
        SET    last_login_date = @LastLoginDate
        WHERE  user_id = @UserId";
}

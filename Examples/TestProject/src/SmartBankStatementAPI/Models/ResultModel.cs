namespace SmartBankStatementAPI.Models;

/// <summary>
/// Standard API response model (§8.5)
/// </summary>
public class ResultModel
{
    public int Status { get; set; }
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
}

/// <summary>
/// Generic API response model with data payload
/// </summary>
public class ResultModel<T> : ResultModel
{
    public T? Data { get; set; }
}

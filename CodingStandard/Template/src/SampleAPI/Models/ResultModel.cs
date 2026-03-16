// §8.5 — Error Response โครงสร้างเดียวกัน: Status, IsSuccess, Message

namespace SampleAPI.Models;

/// <summary>
/// Unified API Response — ทุก endpoint return โครงสร้างเดียวกัน
/// StatusCode() ใน Controller ใช้ Status field (§5.7)
/// </summary>
public class ResultModel
{
    public int Status { get; set; }
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
}

/// <summary>
/// Unified API Response with Data payload
/// </summary>
public class ResultModel<T> : ResultModel
{
    public T? Data { get; set; }
}

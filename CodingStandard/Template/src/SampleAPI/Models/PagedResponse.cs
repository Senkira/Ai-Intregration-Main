// §13.5 — Pagination Response โครงสร้างมาตรฐาน

namespace SampleAPI.Models;

/// <summary>
/// Paged Response — Collection endpoint ทุกตัวต้องใช้ (§13.4)
/// </summary>
public class PagedResponse<T>
{
    public IReadOnlyList<T> Data { get; set; } = [];
    public PaginationMeta Pagination { get; set; } = new();
}

/// <summary>
/// Pagination metadata — page, pageSize, totalCount, totalPages
/// </summary>
public class PaginationMeta
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}

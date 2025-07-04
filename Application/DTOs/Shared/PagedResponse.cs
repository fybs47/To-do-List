﻿namespace Application.DTOs.Shared;

public class PagedResponse<T>
{
    public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages
    {
        get { return (int)Math.Ceiling(TotalCount / (double)PageSize); }
    }
}
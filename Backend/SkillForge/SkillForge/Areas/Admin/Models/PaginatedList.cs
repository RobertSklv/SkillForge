﻿using Microsoft.EntityFrameworkCore;

namespace SkillForge.Areas.Admin.Models;

public class PaginatedList<T> : List<T>
{
    public int PageIndex { get; private set; }
    public int TotalItems { get; private set; }
    public int TotalPages { get; private set; }

    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPages;

    public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        TotalItems = count;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);

        AddRange(items);
    }

    public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
    {
        int count = await source.CountAsync();
        List<T> items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PaginatedList<T>(items, count, pageIndex, pageSize);
    }
}

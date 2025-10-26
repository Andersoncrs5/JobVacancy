using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Utils.Page;

public class PaginatedList<T>
{
    private PaginatedList(IEnumerable<T> data, int pageIndex, int pageSize, long totalCount)
    {
        Data = data;
        PageIndex = pageIndex;
        PageSize = pageSize;
        TotalCount = totalCount;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
    }

    public IEnumerable<T> Data { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public long TotalCount { get; set; }
    public int TotalPages { get; set; }
    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPages;
    
    public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
    {
        if (pageIndex < 1) pageIndex = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        long count = await source.CountAsync();
        List<T>? data = await source
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        return new PaginatedList<T>(data, pageIndex, pageSize, count);

    }
    
    public static PaginatedList<TDestination> MapTo<TSource, TDestination>(
        PaginatedList<TSource> source, 
        IMapper mapper)
    {
        IEnumerable<TDestination> dtos = mapper.Map<IEnumerable<TDestination>>(source.Data);

        return new PaginatedList<TDestination>(
            data: dtos,
            pageIndex: source.PageIndex,
            pageSize: source.PageSize,
            totalCount: source.TotalCount
        );
    }
    
}
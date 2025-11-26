using JobVacancy.API.models.entities;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Utils.Filters.Resume;

public class ResumeFilterQuery
{
    public static IQueryable<ResumeEntity> ApplyFilter(IQueryable<ResumeEntity> query,
        ResumeFilterParams filter)
    {
        if (
            filter.LoadUser.HasValue ||
            !string.IsNullOrWhiteSpace(filter.UserName) ||
            !string.IsNullOrWhiteSpace(filter.Email)
        )
            query = query.Include(x => x.User);
        
        if (!string.IsNullOrWhiteSpace(filter.Name))
            query = query.Where(c => EF.Functions.Like(c.Name, $"%{filter.Name}%"));
        
        query = query
            .Where(x => !filter.VersionMin.HasValue || x.Version >= filter.VersionMin.Value)
            .Where(x => !filter.VersionMax.HasValue || x.Version <= filter.VersionMax.Value);
        
        if (!string.IsNullOrWhiteSpace(filter.BucketName))
            query = query.Where(c => EF.Functions.Like(c.BucketName, $"%{filter.BucketName}%"));

        if (!string.IsNullOrWhiteSpace(filter.ObjectKey))
            query = query.Where(c => EF.Functions.Like(c.ObjectKey, $"%{filter.ObjectKey}%"));

        if (!string.IsNullOrWhiteSpace(filter.UserId))
            query = query.Where(c => EF.Functions.Like(c.userId, $"%{filter.UserId}%"));
        
        if (!string.IsNullOrWhiteSpace(filter.UserName))
            query = query.Where(c => EF.Functions.Like(c.User!.UserName, $"%{filter.UserName}%"));
        
        if (!string.IsNullOrWhiteSpace(filter.Email))
            query = query.Where(c => EF.Functions.Like(c.User!.Email, $"%{filter.Email}%"));
        
        return FilterBaseQuery.ApplyBaseFilters(query, filter);
    }
}
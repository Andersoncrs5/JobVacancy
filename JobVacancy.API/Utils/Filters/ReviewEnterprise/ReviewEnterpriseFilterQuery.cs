using JobVacancy.API.models.entities;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Utils.Filters.ReviewEnterprise;

public class ReviewEnterpriseFilterQuery
{
    public static IQueryable<ReviewEnterpriseEntity> ApplyFilter(IQueryable<ReviewEnterpriseEntity> query, ReviewEnterpriseFilterParams filter)
    {
        if (
            !string.IsNullOrWhiteSpace(filter.PositionId) ||
            !string.IsNullOrWhiteSpace(filter.NamePosition)
            )
        {
            query = query.Include(x => x.Position);
        }
        
        if (
            !string.IsNullOrWhiteSpace(filter.EnterpriseId) || 
            !string.IsNullOrWhiteSpace(filter.EnterpriseName) || 
            filter.TypeEnterprise.HasValue
            )
        {
            query = query.Include(x => x.Enterprise);
        }
        
        // REVIEW
        if (!string.IsNullOrWhiteSpace(filter.Title))
        {
            query = query.Where(c => EF.Functions.Like(c.Title, $"%{filter.Title}%"));
        }
        
        if (!string.IsNullOrWhiteSpace(filter.Content))
        {
            query = query.Where(c => EF.Functions.Like(c.Content, $"%{filter.Content}%"));
        }

        if (filter.RatingOverallMin.HasValue)
        {
            query = query.Where(x => x.RatingOverall >= filter.RatingOverallMin.Value);
        }
        if (filter.RatingOverallMax.HasValue)
        {
            query = query.Where(x => x.RatingOverall <= filter.RatingOverallMax.Value);
        }

        if (filter.RatingCultureMin.HasValue)
        {
            query = query.Where(x => x.RatingCulture >= filter.RatingCultureMin.Value);
        }
        if (filter.RatingCultureMax.HasValue)
        {
            query = query.Where(x => x.RatingCulture <= filter.RatingCultureMax.Value);
        }

        if (filter.RatingCompensationMin.HasValue)
        {
            query = query.Where(x => x.RatingCompensation >= filter.RatingCompensationMin.Value);
        }
        if (filter.RatingCompensationMax.HasValue)
        {
            query = query.Where(x => x.RatingCompensation <= filter.RatingCompensationMax.Value);
        }

        if (filter.RatingWorkLifeBalanceMin.HasValue)
        {
            query = query.Where(x => x.RatingWorkLifeBalance >= filter.RatingWorkLifeBalanceMin.Value);
        }
        if (filter.RatingWorkLifeBalanceMax.HasValue)
        {
            query = query.Where(x => x.RatingWorkLifeBalance <= filter.RatingWorkLifeBalanceMax.Value);
        }

        if (filter.RatingManagementMin.HasValue)
        {
            query = query.Where(x => x.RatingManagement >= filter.RatingManagementMin.Value);
        }
        if (filter.RatingManagementMax.HasValue)
        {
            query = query.Where(x => x.RatingManagement <= filter.RatingManagementMax.Value);
        }

        if (filter.IsAnonymous.HasValue) 
        {
            query = query.Where(x => x.IsAnonymous == filter.IsAnonymous.Value);
        }

        // POSITION
        if (!string.IsNullOrWhiteSpace(filter.PositionId))
        {
            query = query.Where(x => x.PositionId == filter.PositionId);
        }
        
        if (!string.IsNullOrWhiteSpace(filter.NamePosition))
        {
            query = query.Where(x => x.Position!.Name == filter.NamePosition);
        }
        
        // ENTERPRISE
        if (!string.IsNullOrWhiteSpace(filter.EnterpriseId))
        {
            query = query.Where(x => x.EnterpriseId == filter.EnterpriseId);
        }

        if (!string.IsNullOrWhiteSpace(filter.EnterpriseName))
        {
            query = query.Where(x => x.Enterprise!.Name == filter.EnterpriseName);
        }

        if (filter.TypeEnterprise.HasValue)
        {
            query = query.Where(x => x.Enterprise!.Type == filter.TypeEnterprise.Value);
        }
        
        return FilterBaseQuery.ApplyBaseFilters(query, filter);
    }
}
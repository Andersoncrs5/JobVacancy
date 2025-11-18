using JobVacancy.API.models.entities;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Utils.Filters.ReviewUser;

public class ReviewUserFilterQuery
{
    public static IQueryable<ReviewUserEntity> ApplyFilter(IQueryable<ReviewUserEntity> query,
        ReviewUserFilterParams filter)
    {
        if (
            filter.LoadTarget.HasValue ||
            !string.IsNullOrWhiteSpace(filter.TargetName) ||
            !string.IsNullOrWhiteSpace(filter.TargetEmail)
        )
            query = query.Include(x => x.TargetUser);
        
        if (
            filter.LoadActor.HasValue ||
            !string.IsNullOrWhiteSpace(filter.ActorName) ||
            !string.IsNullOrWhiteSpace(filter.ActorEmail)
        )
            query = query.Include(x => x.Actor);
        
        // ACTOR USER
        if (!string.IsNullOrWhiteSpace(filter.ActorId)) 
            query = query.Where(x => x.ActorId == filter.ActorId );
        
        if (!string.IsNullOrWhiteSpace(filter.ActorName))
            query = query.Where(x => x.Actor!.UserName == filter.ActorName );
            
        if (!string.IsNullOrWhiteSpace(filter.ActorEmail))
            query = query.Where(x => x.Actor!.Email == filter.ActorEmail );
            
        // TARGET USER
        if (!string.IsNullOrWhiteSpace(filter.TargetId)) 
            query = query.Where(x => x.TargetUserId == filter.TargetId );
        
        if (!string.IsNullOrWhiteSpace(filter.TargetName))
            query = query.Where(x => x.TargetUser!.UserName == filter.TargetName );
            
        if (!string.IsNullOrWhiteSpace(filter.TargetEmail))
            query = query.Where(x => x.TargetUser!.Email == filter.TargetEmail );
        
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
        if (filter.Recommendation.HasValue) 
        {
            query = query.Where(x => x.Recommendation == filter.Recommendation.Value);
        }
        
        return FilterBaseQuery.ApplyBaseFilters(query, filter);
    }
}
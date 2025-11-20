using JobVacancy.API.models.entities;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Utils.Filters.UserEvaluation;

public class UserEvaluationFilterQuery
{
    public static IQueryable<UserEvaluationEntity> ApplyFilter(IQueryable<UserEvaluationEntity> query, UserEvaluationFilterParams filter)
    {
        if (
            filter.LoadEnterprise.HasValue ||
            !string.IsNullOrWhiteSpace(filter.EnterpriseName) ||
            filter.EnterpriseType.HasValue
            )
            query = query.Include(x => x.Enterprise);

        if (
            filter.LoadTargetUser.HasValue ||
            !string.IsNullOrWhiteSpace(filter.TargetUserFullName) ||
            !string.IsNullOrWhiteSpace(filter.TargetUserName) ||
            !string.IsNullOrWhiteSpace(filter.TargetUserEmail)
        )
            query = query.Include(x => x.TargetUser);
        
        if (
            filter.LoadReviewerUser.HasValue ||
            !string.IsNullOrWhiteSpace(filter.ReviewerUserFullName) ||
            !string.IsNullOrWhiteSpace(filter.ReviewerUserName) ||
            !string.IsNullOrWhiteSpace(filter.ReviewerUserEmail)
        )
            query = query.Include(x => x.ReviewerUser);
        
        if (
            filter.LoadPosition.HasValue ||
            !string.IsNullOrWhiteSpace(filter.PositionName)
        )
            query = query.Include(x => x.Position);
        
        if (!string.IsNullOrWhiteSpace(filter.Title))
            query = query.Where(c => EF.Functions.Like(c.Title, $"%{filter.Title}%"));
        
        if (!string.IsNullOrWhiteSpace(filter.Content))
            query = query.Where(c => EF.Functions.Like(c.Content, $"%{filter.Content}%"));
        
        query = query
            .Where(x => !filter.RatingOverallMin.HasValue || x.RatingOverall >= filter.RatingOverallMin.Value)
            .Where(x => !filter.RatingOverallMax.HasValue || x.RatingOverall <= filter.RatingOverallMax.Value);

        query = query
            .Where(x => !filter.RatingCultureMin.HasValue || x.RatingCulture >= filter.RatingCultureMin.Value)
            .Where(x => !filter.RatingCultureMax.HasValue || x.RatingCulture <= filter.RatingCultureMax.Value);

        query = query
            .Where(x => !filter.RatingCompensationMin.HasValue || x.RatingCompensation >= filter.RatingCompensationMin.Value)
            .Where(x => !filter.RatingCompensationMax.HasValue || x.RatingCompensation <= filter.RatingCompensationMax.Value);

        query = query
            .Where(x => !filter.RatingManagementMin.HasValue || x.RatingManagement >= filter.RatingManagementMin.Value)
            .Where(x => !filter.RatingManagementMax.HasValue || x.RatingManagement <= filter.RatingManagementMax.Value);

        query = query
            .Where(x => !filter.RatingWorkLifeBalanceMin.HasValue || x.RatingWorkLifeBalance >= filter.RatingWorkLifeBalanceMin.Value)
            .Where(x => !filter.RatingWorkLifeBalanceMax.HasValue || x.RatingWorkLifeBalance <= filter.RatingWorkLifeBalanceMax.Value);

        query = query
            .Where(x => !filter.RatingProfessionalismMin.HasValue || x.RatingProfessionalism >= filter.RatingProfessionalismMin.Value)
            .Where(x => !filter.RatingProfessionalismMax.HasValue || x.RatingProfessionalism <= filter.RatingProfessionalismMax.Value);

        query = query
            .Where(x => !filter.RatingSkillMatchMin.HasValue || x.RatingSkillMatch >= filter.RatingSkillMatchMin.Value)
            .Where(x => !filter.RatingSkillMatchMax.HasValue || x.RatingSkillMatch <= filter.RatingSkillMatchMax.Value);

        query = query
            .Where(x => !filter.RatingTeamworkMin.HasValue || x.RatingTeamwork >= filter.RatingTeamworkMin.Value)
            .Where(x => !filter.RatingTeamworkMax.HasValue || x.RatingTeamwork <= filter.RatingTeamworkMax.Value);

        query = query
            .Where(x => !filter.RecommendationToneMin.HasValue || x.RecommendationTone >= filter.RecommendationToneMin.Value)
            .Where(x => !filter.RecommendationToneMax.HasValue || x.RecommendationTone <= filter.RecommendationToneMax.Value);

        if (filter.IsAnonymous.HasValue)
            query = query.Where(x => x.IsAnonymous == filter.IsAnonymous.Value);

        if (filter.EmploymentStatus.HasValue)
            query = query.Where(x => x.EmploymentStatus == filter.EmploymentStatus.Value);

        // ENTERPRISE
        if (!string.IsNullOrWhiteSpace(filter.EnterpriseId))
            query = query.Where(x => x.EnterpriseId == filter.EnterpriseId);

        if (!string.IsNullOrWhiteSpace(filter.EnterpriseName))
            query = query.Where(x => x.Enterprise!.Name == filter.EnterpriseName);

        if (filter.EnterpriseType.HasValue)
            query = query.Where(x => x.Enterprise!.Type == filter.EnterpriseType.Value);
        
        //  TARGET USER
        if (!string.IsNullOrWhiteSpace(filter.TargetUserId))
            query = query.Where(x => x.TargetUserId == filter.TargetUserId);
        
        if (!string.IsNullOrWhiteSpace(filter.TargetUserFullName))
            query = query.Where(x => x.TargetUser!.FullName == filter.TargetUserFullName);

        if (!string.IsNullOrWhiteSpace(filter.TargetUserName))
            query = query.Where(x => x.TargetUser!.UserName == filter.TargetUserName);

        if (!string.IsNullOrWhiteSpace(filter.TargetUserEmail))
            query = query.Where(x => x.TargetUser!.Email == filter.TargetUserEmail);

        //  REVIEWER USER
        if (!string.IsNullOrWhiteSpace(filter.ReviewerUserId))
            query = query.Where(x => x.ReviewerUserId == filter.ReviewerUserId);
        
        if (!string.IsNullOrWhiteSpace(filter.ReviewerUserFullName))
            query = query.Where(x => x.ReviewerUser!.FullName == filter.ReviewerUserFullName);

        if (!string.IsNullOrWhiteSpace(filter.ReviewerUserName))
            query = query.Where(x => x.ReviewerUser!.UserName == filter.ReviewerUserName);

        if (!string.IsNullOrWhiteSpace(filter.TargetUserEmail))
            query = query.Where(x => x.TargetUser!.Email == filter.TargetUserEmail);

        // POSITION 
        if (!string.IsNullOrWhiteSpace(filter.PositionId))
            query = query.Where(x => x.PositionId == filter.PositionId);
        
        if (!string.IsNullOrWhiteSpace(filter.PositionName))
            query = query.Where(x => x.Position!.Name == filter.PositionName);
        
        return FilterBaseQuery.ApplyBaseFilters(query, filter);
    }
}
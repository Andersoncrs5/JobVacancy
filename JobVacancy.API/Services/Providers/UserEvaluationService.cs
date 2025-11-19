using JobVacancy.API.models.dtos.UserEvaluation;
using JobVacancy.API.models.entities;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Uow.Interfaces;

namespace JobVacancy.API.Services.Providers;

public class UserEvaluationService(IUnitOfWork uow): IUserEvaluationService
{
    public async Task<bool> ExistsById(string id)
        => await uow.UserEvaluationRepository.ExistsById(id);

    public async Task<UserEvaluationEntity?> GetById(string id)
        => await uow.UserEvaluationRepository.GetByIdAsync(id);
    
    public async Task<bool> ExistsByEnterpriseIdAndTargetUserId(string enterpriseId, string targetUserId)
        => await uow.UserEvaluationRepository.ExistsByEnterpriseIdAndTargetUserId(enterpriseId, targetUserId);

    public async Task<UserEvaluationEntity?>  GetByEnterpriseIdAndTargetUserId(string enterpriseId, string targetUserId)
        => await uow.UserEvaluationRepository.GetByEnterpriseIdAndTargetUserId(enterpriseId, targetUserId);

    public IQueryable<UserEvaluationEntity> Query()
        => uow.UserEvaluationRepository.Query();
    
    public async Task Delete(UserEvaluationEntity entity)
    {
        await uow.UserEvaluationRepository.DeleteAsync(entity);
        await uow.Commit();
    }

    public async Task<UserEvaluationEntity> Create(CreateUserEvaluationDto dto, string enterpriseId, string? reviewUserId)
    {
        UserEvaluationEntity map = uow.Mapper.Map<UserEvaluationEntity>(dto);
        map.EnterpriseId = enterpriseId;
        map.ReviewerUserId = reviewUserId;

        UserEvaluationEntity async = await uow.UserEvaluationRepository.AddAsync(map);
        await uow.Commit();
        return async;
    }

    public async Task<UserEvaluationEntity> Update(UpdateUserEvaluationDto dto, UserEvaluationEntity evaluation)
    {
        if (!string.IsNullOrWhiteSpace(dto.Title))
            evaluation.Title = dto.Title;
        
        if (!string.IsNullOrWhiteSpace(dto.Content))
            evaluation.Content = dto.Content;
        
        if (!string.IsNullOrWhiteSpace(dto.PositionId))
            evaluation.PositionId = dto.PositionId;
        
        if (dto.RatingOverall.HasValue)
            evaluation.RatingOverall = dto.RatingOverall.Value;
        
        if (dto.RatingCulture.HasValue)
            evaluation.RatingCulture = dto.RatingCulture.Value;
        
        if (dto.RatingCompensation.HasValue)
            evaluation.RatingCompensation = dto.RatingCompensation.Value;
        
        if (dto.RatingManagement.HasValue)
            evaluation.RatingManagement = dto.RatingManagement.Value;
        
        if (dto.RatingWorkLifeBalance.HasValue)
            evaluation.RatingWorkLifeBalance = dto.RatingWorkLifeBalance.Value;
        
        if (dto.IsAnonymous.HasValue)
            evaluation.IsAnonymous = dto.IsAnonymous.Value;
        
        if (dto.RatingProfessionalism.HasValue)
            evaluation.RatingProfessionalism = dto.RatingProfessionalism.Value;
        
        if (dto.RatingSkillMatch.HasValue)
            evaluation.RatingSkillMatch = dto.RatingSkillMatch.Value;
        
        if (dto.RatingTeamwork.HasValue)
            evaluation.RatingTeamwork = dto.RatingTeamwork.Value;
        
        if (dto.RecommendationTone.HasValue)
            evaluation.RecommendationTone = dto.RecommendationTone.Value;
        
        if (dto.EmploymentStatus.HasValue)
            evaluation.EmploymentStatus = dto.EmploymentStatus.Value;
        
        UserEvaluationEntity update = await uow.UserEvaluationRepository.Update(evaluation);
        await uow.Commit();
        return update;
    }
    
}
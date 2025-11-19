using JobVacancy.API.models.dtos.UserEvaluation;
using JobVacancy.API.models.entities;

namespace JobVacancy.API.Services.Interfaces;

public interface IUserEvaluationService
{
    Task<bool> ExistsById(string id);
    Task<UserEvaluationEntity?> GetById(string id);
    Task<bool> ExistsByEnterpriseIdAndTargetUserId(string enterpriseId, string targetUserId);
    Task<UserEvaluationEntity?> GetByEnterpriseIdAndTargetUserId(string enterpriseId, string targetUserId);
    IQueryable<UserEvaluationEntity> Query();
    Task Delete(UserEvaluationEntity entity);
    Task<UserEvaluationEntity> Create(CreateUserEvaluationDto dto, string enterpriseId, string? reviewUserId);
    Task<UserEvaluationEntity> Update(UpdateUserEvaluationDto dto, UserEvaluationEntity evaluation);
}
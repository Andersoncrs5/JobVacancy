using JobVacancy.API.models.entities;

namespace JobVacancy.API.Repositories.Interfaces;

public interface IUserEvaluationRepository: IGenericRepository<UserEvaluationEntity>
{
    Task<bool> ExistsByEnterpriseIdAndTargetUserId(string enterpriseId, string targetUserId);
    Task<UserEvaluationEntity?> GetByEnterpriseIdAndTargetUserId(string enterpriseId, string targetUserId);
    
}
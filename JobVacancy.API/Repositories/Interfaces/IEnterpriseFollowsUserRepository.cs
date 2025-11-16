using JobVacancy.API.models.entities;

namespace JobVacancy.API.Repositories.Interfaces;

public interface IEnterpriseFollowsUserRepository: IGenericRepository<EnterpriseFollowsUserEntity>
{
    Task<bool> ExistsByEnterpriseIdAndUserId(string enterpriseId, string userId);
    Task<EnterpriseFollowsUserEntity?> GetByEnterpriseIdAndUserId(string enterpriseId, string userId);
}
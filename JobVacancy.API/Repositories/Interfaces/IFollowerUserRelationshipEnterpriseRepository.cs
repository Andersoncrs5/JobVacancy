using JobVacancy.API.models.entities;

namespace JobVacancy.API.Repositories.Interfaces;

public interface IFollowerUserRelationshipEnterpriseRepository: IGenericRepository<FollowerUserRelationshipEnterpriseEntity>
{
    Task<bool> ExistsByEnterpriseIdAndUserId(string enterpriseId, string userId);
    Task<FollowerUserRelationshipEnterpriseEntity?> GetByEnterpriseIdAndUserId(string enterpriseId, string userId);
}
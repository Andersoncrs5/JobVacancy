using JobVacancy.API.models.dtos.FollowerUserRelationshipEnterprise;
using JobVacancy.API.models.entities;

namespace JobVacancy.API.Services.Interfaces;

public interface IFollowerUserRelationshipEnterpriseService
{
    Task<bool> ExistsByEnterpriseIdAndUserId(string enterpriseId, string userId);
    Task<FollowerUserRelationshipEnterpriseEntity?> GetByEnterpriseIdAndUserId(string enterpriseId, string userId);
    Task<bool> ExistsById(string id);
    IQueryable<FollowerUserRelationshipEnterpriseEntity> Query();
    Task<FollowerUserRelationshipEnterpriseEntity?> GetById(string id);
    Task Delete(FollowerUserRelationshipEnterpriseEntity entity);
    Task<FollowerUserRelationshipEnterpriseEntity> Create(string enterpriseId, string userId);
    Task<FollowerUserRelationshipEnterpriseEntity> Update(UpdateFollowerUserRelationshipEnterpriseDto dto,
        FollowerUserRelationshipEnterpriseEntity entity);
    
}
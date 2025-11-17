using JobVacancy.API.models.dtos.EnterpriseFollowsUser;
using JobVacancy.API.models.entities;

namespace JobVacancy.API.Services.Interfaces;

public interface IEnterpriseFollowsUserService
{
    Task<bool> ExistsByEnterpriseIdAndUserId(string enterpriseId, string userId);
    Task<EnterpriseFollowsUserEntity?> GetByEnterpriseIdAndUserId(string enterpriseId, string userId);
    Task<bool> ExistsById(string id);
    Task<EnterpriseFollowsUserEntity?> GetById(string id);
    IQueryable<EnterpriseFollowsUserEntity> Query();
    Task Delete(EnterpriseFollowsUserEntity entity);
    Task<EnterpriseFollowsUserEntity> Create(string enterpriseId, string userId);

    Task<EnterpriseFollowsUserEntity> Update(
        UpdateEnterpriseFollowsUserDto dto, EnterpriseFollowsUserEntity entity
    );
}
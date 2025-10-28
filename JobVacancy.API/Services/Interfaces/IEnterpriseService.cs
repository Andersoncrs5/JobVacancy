using JobVacancy.API.models.dtos.Enterprise;
using JobVacancy.API.models.entities;

namespace JobVacancy.API.Services.Interfaces;

public interface IEnterpriseService
{
    Task<EnterpriseEntity?> GetById(string id);
    Task<EnterpriseEntity?> GetByUserId(string userId);
    void Delete(EnterpriseEntity entity);
    Task<bool> ExistsById(string id);
    Task<bool> ExistsByUserId(string userId);
    Task<bool> ExistsByName(string name);
    IQueryable<EnterpriseEntity> Query();
    Task<EnterpriseEntity> CreateAsync(CreateEnterpriseDto dto, string userId);
    Task<EnterpriseEntity> UpdateAsync(EnterpriseEntity entity, UpdateEnterpriseDto dto);
}
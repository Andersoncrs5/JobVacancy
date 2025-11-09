using JobVacancy.API.models.dtos.EmployeeEnterprise;
using JobVacancy.API.models.entities;

namespace JobVacancy.API.Services.Interfaces;

public interface IEmployeeEnterpriseService
{
    IQueryable<EmployeeEnterpriseEntity> Query();
    Task<EmployeeEnterpriseEntity> Update(UpdateEmployeeEnterpriseDto dto, EmployeeEnterpriseEntity entity);
    Task<bool> ExistsByUserIdAndEnterpriseId(string userId, string enterpriseId);
    Task<EmployeeEnterpriseEntity> Create(CreateEmployeeEnterpriseDto dto, string enterpriseId, string invitationUserId);
    Task Delete(EmployeeEnterpriseEntity entity);
    Task<bool> ExistsById(string id);
    Task<EmployeeEnterpriseEntity?> GetById(string id);
}
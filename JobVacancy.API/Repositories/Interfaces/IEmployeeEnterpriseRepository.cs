using JobVacancy.API.models.entities;

namespace JobVacancy.API.Repositories.Interfaces;

public interface IEmployeeEnterpriseRepository: IGenericRepository<EmployeeEnterpriseEntity>
{
    Task<bool> ExistsByUserIdAndEnterpriseId(string userId, string enterpriseId);
    Task<EmployeeEnterpriseEntity?> GetByUserIdAndEnterpriseId(string userId, string enterpriseId);
}
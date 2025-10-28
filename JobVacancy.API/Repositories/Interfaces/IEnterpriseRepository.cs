using JobVacancy.API.models.entities;

namespace JobVacancy.API.Repositories.Interfaces;

public interface IEnterpriseRepository: IGenericRepository<EnterpriseEntity>
{
    Task<bool> ExistsByName(string name);
    Task<bool> ExistsByUserId(string userId);
    Task<EnterpriseEntity?> GetByUserId(string userId);
}
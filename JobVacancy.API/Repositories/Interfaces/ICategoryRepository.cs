using JobVacancy.API.models.entities;

namespace JobVacancy.API.Repositories.Interfaces;

public interface ICategoryRepository: IGenericRepository<CategoryEntity>
{
    Task<bool> ExistsByName(string name);
}
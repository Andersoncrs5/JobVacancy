using JobVacancy.API.models.entities;

namespace JobVacancy.API.Repositories.Interfaces;

public interface IAreaRepository: IGenericRepository<AreaEntity>
{
    Task<bool> ExistsByName(string name);
}
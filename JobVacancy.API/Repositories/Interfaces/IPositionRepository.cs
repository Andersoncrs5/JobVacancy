using JobVacancy.API.models.entities;

namespace JobVacancy.API.Repositories.Interfaces;

public interface IPositionRepository: IGenericRepository<PositionEntity>
{
    Task<bool> ExistsByName(string name);
}
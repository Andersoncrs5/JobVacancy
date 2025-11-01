using JobVacancy.API.models.entities;

namespace JobVacancy.API.Repositories.Interfaces;

public interface ISkillRepository: IGenericRepository<SkillEntity>
{
    Task<bool> ExistsByName(string name);
}
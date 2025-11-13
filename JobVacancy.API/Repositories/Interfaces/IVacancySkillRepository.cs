using JobVacancy.API.models.entities;

namespace JobVacancy.API.Repositories.Interfaces;

public interface IVacancySkillRepository: IGenericRepository<VacancySkillEntity>
{
    Task<bool> ExistsByVacancyIdAndSkillId(string vacancyId, string skillId);
    Task<VacancySkillEntity?> GetByVacancyIdAndSkillId(string vacancyId, string skillId);
}
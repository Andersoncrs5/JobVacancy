using JobVacancy.API.models.dtos.VacancySkill;
using JobVacancy.API.models.entities;

namespace JobVacancy.API.Services.Interfaces;

public interface IVacancySkillService
{
    Task<bool> ExistsByVacancyIdAndSkillId(string vacancyId, string skillId);
    Task<VacancySkillEntity?> GetByVacancyIdAndSkillId(string vacancyId, string skillId);
    IQueryable<VacancySkillEntity> Query();
    Task<bool> ExistsById(string id);
    Task<VacancySkillEntity?> GetById(string id);
    Task Delete(VacancySkillEntity entity);
    Task<VacancySkillEntity> CreateAsync(CreateVacancySkillDto dto);
    Task<VacancySkillEntity> UpdateAsync(UpdateVacancyDto dto, VacancySkillEntity entity);
}
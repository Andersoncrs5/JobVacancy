using JobVacancy.API.models.dtos.Skill;
using JobVacancy.API.models.entities;

namespace JobVacancy.API.Services.Interfaces;

public interface ISkillService
{
    Task<SkillEntity?> GetById(string id);
    Task Delete(SkillEntity skill);
    Task<bool> ExistsById(string id);
    Task<bool> ExistsByName(string name);
    Task<SkillEntity> CreateAsync(CreateSkillDto dto);
    Task<SkillEntity> UpdateAsync(UpdateSkillDto dto, SkillEntity skill);
    IQueryable<SkillEntity> Query();

}
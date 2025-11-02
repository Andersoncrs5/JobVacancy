using JobVacancy.API.models.dtos.UserSkill;
using JobVacancy.API.models.entities;

namespace JobVacancy.API.Services.Interfaces;

public interface IUserSkillService
{
    Task<bool> ExistsByUserIdAndSkillId(string userId, string skillId);
    Task<UserSkillEntity?> GetByUserIdAndSkillId(string userId, string skillId);
    Task Delete(UserSkillEntity userSkill);
    Task<UserSkillEntity> CreateAsync(string userId, string skillId);
    Task<UserSkillEntity> UpdateAsync(UserSkillEntity userSkill, UpdateUserSkillDto dto);
    IQueryable<UserSkillEntity> Query();
}
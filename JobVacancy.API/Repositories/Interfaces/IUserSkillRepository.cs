using JobVacancy.API.models.entities;

namespace JobVacancy.API.Repositories.Interfaces;

public interface IUserSkillRepository: IGenericRepository<UserSkillEntity>
{
    Task<bool> ExistsByUserIdAndSkillId(string userId, string skillId);
    Task<UserSkillEntity?> GetByUserIdAndSkillId(string userId, string skillId);
}
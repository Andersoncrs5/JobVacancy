using JobVacancy.API.Context;
using JobVacancy.API.models.entities;
using JobVacancy.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Repositories.Provider;

public class UserSkillRepository(AppDbContext context) 
    : GenericRepository<UserSkillEntity>(context),  IUserSkillRepository
{
    public async Task<bool> ExistsByUserIdAndSkillId(string userId, string skillId)
    {
        return await context.UserSkill.AnyAsync(e => e.UserId == userId && e.SkillId == skillId);
    }
    
    public async Task<UserSkillEntity?> GetByUserIdAndSkillId(string userId, string skillId)
    {
        return await context.UserSkill.FirstOrDefaultAsync(e => e.UserId == userId && e.SkillId == skillId);
    }
    
    
}
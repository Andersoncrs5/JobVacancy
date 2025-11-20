using JobVacancy.API.Context;
using JobVacancy.API.models.entities;
using JobVacancy.API.Repositories.Interfaces;
using JobVacancy.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Repositories.Provider;

public class UserSkillRepository(AppDbContext context, IRedisService redisService) 
    : GenericRepository<UserSkillEntity>(context, redisService),  IUserSkillRepository
{
    public async Task<bool> ExistsByUserIdAndSkillId(string userId, string skillId)
    {
        return await context.UserSkill.AnyAsync(e => e.UserId == userId && e.SkillId == skillId);
    }
    
    public async Task<UserSkillEntity?> GetByUserIdAndSkillId(string userId, string skillId)
    {
        return await context.UserSkill.FirstOrDefaultAsync(e => e.UserId == userId && e.SkillId == skillId);
    }
    
    public async Task<UserSkillEntity?> GetByUserIdAndSkillIdWithEntity(string userId, string skillId)
    {
        IQueryable<UserSkillEntity> query = context.UserSkill.AsQueryable();
        query = query.Where(e => e.UserId == userId && e.SkillId == skillId);
        
        query = query.Include(e => e.Skill);
        query = query.Include(e => e.User);
        
        return await query.FirstOrDefaultAsync();
    }
    
}
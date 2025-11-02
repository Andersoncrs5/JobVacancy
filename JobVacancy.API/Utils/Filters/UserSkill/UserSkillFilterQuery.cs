using JobVacancy.API.models.entities;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Utils.Filters.UserSkill;

public class UserSkillFilterQuery
{
    public static IQueryable<UserSkillEntity> ApplyFilter(IQueryable<UserSkillEntity> query, UserSkillFilterParam filter)
    {
        query = query.Include(e => e.User);
        query = query.Include(e => e.Skill);
        
        if (!string.IsNullOrWhiteSpace(filter.SkillId))
        {
            query = query.Where(c => EF.Functions.Like(c.SkillId, $"%{filter.SkillId}%"));
        }
        
        if (!string.IsNullOrWhiteSpace(filter.NameSkill))
        {
            query = query.Where(c => c.Skill.Name.Contains(filter.NameSkill));
        }
        
        if (filter.YearsOfExperience.HasValue)
        {
            query = query.Where(c => c.YearsOfExperience ==  filter.YearsOfExperience);
        }
        
        if (!string.IsNullOrWhiteSpace(filter.ExternalCertificateUrl))
        {
            query = query.Where(c => c.ExternalCertificateUrl.Contains(filter.ExternalCertificateUrl));
        }
        
        if (!string.IsNullOrWhiteSpace(filter.FullNameUser))
        {
            query = query.Where(c => c.User.FullName.Contains(filter.FullNameUser));
        }

        if (!string.IsNullOrWhiteSpace(filter.Username))
        {
            query = query.Where(c => c.User!.UserName!.Contains(filter.Username));
        }
        
        if (!string.IsNullOrWhiteSpace(filter.Email))
        {
            query = query.Where(c => c.User!.Email!.Contains(filter.Email));
        }
        
        if (!string.IsNullOrWhiteSpace(filter.UserId))
        {
            query = query.Where(c => c.UserId == filter.UserId);
        }
        
        return FilterBaseQuery.ApplyBaseFilters(query, filter);
    }
}
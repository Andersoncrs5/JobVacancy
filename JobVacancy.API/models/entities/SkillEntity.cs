namespace JobVacancy.API.models.entities;

public class SkillEntity: BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public string? IconUrl { get; set; }
    public virtual ICollection<UserSkillEntity>? UserSkill { get; set; } 
    
}
namespace JobVacancy.API.models.dtos.Skill;

public class SkillDto: BaseDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public string? IconUrl { get; set; }
}
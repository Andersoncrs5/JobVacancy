namespace JobVacancy.API.models.dtos.Skill;

public class UpdateSkillDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool? IsActive { get; set; }
    public string? IconUrl { get; set; }
}
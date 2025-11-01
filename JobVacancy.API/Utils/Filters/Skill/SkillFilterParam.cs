namespace JobVacancy.API.Utils.Filters.Skill;

public class SkillFilterParam: FilterBaseParams
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool? IsActive { get; set; }
    public string? IconUrl { get; set; }
}
namespace JobVacancy.API.models.dtos.Area;

public class AreaDto: BaseDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}
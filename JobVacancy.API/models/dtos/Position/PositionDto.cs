namespace JobVacancy.API.models.dtos.Position;

public class PositionDto: BaseDto
{
    public string Name { get; set; } = string.Empty;
    public string? Describe { get; set; }
    public bool IsActive { get; set; }
}
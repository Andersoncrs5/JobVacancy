namespace JobVacancy.API.models.dtos;

public class BaseDto
{
    public string Id { get; set; } 
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
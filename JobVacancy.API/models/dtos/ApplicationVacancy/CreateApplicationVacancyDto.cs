namespace JobVacancy.API.models.dtos.ApplicationVacancy;

public class CreateApplicationVacancyDto
{
    public required string VacancyId { get; set; }
    public string? CoverLetter { get; set; }
}
using System.ComponentModel.DataAnnotations;
using JobVacancy.API.Utils.Annotations.Validations.Vacancy;

namespace JobVacancy.API.models.dtos.ApplicationVacancy;

public class CreateApplicationVacancyDto
{
    [Required]
    [ExistsVacancyById]
    public required string VacancyId { get; set; }
    public string? CoverLetter { get; set; }
}
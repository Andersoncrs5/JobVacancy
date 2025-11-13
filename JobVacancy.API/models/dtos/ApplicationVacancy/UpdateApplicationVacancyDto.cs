using JobVacancy.API.models.entities.Enums;

namespace JobVacancy.API.models.dtos.ApplicationVacancy;

public class UpdateApplicationVacancyDto
{
    public ApplicationStatusEnum? Status { get; set; }
    public bool? IsViewedByRecruiter { get; set; }
}
using JobVacancy.API.models.entities.Enums;

namespace JobVacancy.API.models.dtos.IndicationUser;

public class UpdateIndicationUserEndorsedDto
{
    public IndicationStatusEnum? Status { get; set; }
}
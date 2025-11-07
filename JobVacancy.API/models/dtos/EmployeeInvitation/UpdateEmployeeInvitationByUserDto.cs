using JobVacancy.API.models.entities.Enums;

namespace JobVacancy.API.models.dtos.EmployeeInvitation;

public class UpdateEmployeeInvitationByUserDto
{
    public StatusEnum? Status { get; set; }
    public string? RejectReason {get; set;} 
}
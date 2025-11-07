using JobVacancy.API.models.dtos.EmployeeInvitation;
using JobVacancy.API.models.entities;

namespace JobVacancy.API.Services.Interfaces;

public interface IEmployeeInvitationService
{
    Task<EmployeeInvitationEntity?> GetById(string id);
    Task<bool> ExistsById(string id);
    Task Delete(EmployeeInvitationEntity entity);
    Task<EmployeeInvitationEntity> Create(CreateEmployeeInvitationDto dto, string enterpriseId,
        string? inviteSenderId);
    Task<EmployeeInvitationEntity> Update(UpdateEmployeeInvitationDto dto,
        EmployeeInvitationEntity invitation);

    Task<EmployeeInvitationEntity> UpdateByUser(UpdateEmployeeInvitationByUserDto dto,
        EmployeeInvitationEntity invitation);
    IQueryable<EmployeeInvitationEntity> Query();
}
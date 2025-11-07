using JobVacancy.API.models.dtos.EmployeeInvitation;
using JobVacancy.API.models.entities;
using JobVacancy.API.models.entities.Enums;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Uow.Interfaces;

namespace JobVacancy.API.Services.Providers;

public class EmployeeInvitationService(IUnitOfWork uow): IEmployeeInvitationService
{
    public async Task<EmployeeInvitationEntity?> GetById(string id)
    {
        return await uow.EmployeeInvitationRepository.GetByIdAsync(id);
    }

    public async Task<bool> ExistsById(string id)
    {
        return await uow.EmployeeInvitationRepository.ExistsById(id);
    }

    public async Task Delete(EmployeeInvitationEntity entity)
    {
        uow.EmployeeInvitationRepository.Delete(entity);
        await uow.Commit();
    }

    public async Task<EmployeeInvitationEntity> Create(CreateEmployeeInvitationDto dto, string enterpriseId,
        string? inviteSenderId)
    {
        EmployeeInvitationEntity map = uow.Mapper.Map<EmployeeInvitationEntity>(dto);
        map.InviteSenderId = inviteSenderId;
        map.EnterpriseId = enterpriseId;
        map.Status = StatusEnum.Pending;
    
        map.ExpiresAt = DateTime.UtcNow.AddDays(7); 
        
        if (map.ProposedEndDate.HasValue && map.ProposedEndDate.Value < map.ProposedStartDate.AddDays(10))
        {
            map.ExpiresAt = DateTime.UtcNow.AddDays(3);
        }
        else
        {
            map.ExpiresAt = DateTime.UtcNow.AddDays(30); 
        }
    
        map.ResponseDate = null; 

        EmployeeInvitationEntity created = await uow.EmployeeInvitationRepository.AddAsync(map);
        await uow.Commit();
        return created;
    }

    public async Task<EmployeeInvitationEntity> Update(UpdateEmployeeInvitationDto dto,
        EmployeeInvitationEntity invitation)
    {
        uow.Mapper.Map(dto, invitation);

        EmployeeInvitationEntity update = await uow.EmployeeInvitationRepository.Update(invitation);
        await uow.Commit();
        return invitation;
    }

    public IQueryable<EmployeeInvitationEntity> Query()
    {
        return uow.EmployeeInvitationRepository.ReturnIQueryable();
    }
    
}
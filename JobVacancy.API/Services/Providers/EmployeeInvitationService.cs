using AutoMapper;
using JobVacancy.API.models.dtos.EmployeeInvitation;
using JobVacancy.API.models.entities;
using JobVacancy.API.models.entities.Enums;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Uow.Interfaces;

namespace JobVacancy.API.Services.Providers;

public class EmployeeInvitationService(IUnitOfWork uow, IMapper mapper): IEmployeeInvitationService
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
        
        if (map.ProposedEndDate.HasValue)
        {
            map.ExpiresAt = DateTime.UtcNow.AddDays(map.ProposedEndDate.Value.Day + 15);
        }
        
        map.ResponseDate = null; 

        EmployeeInvitationEntity created = await uow.EmployeeInvitationRepository.AddAsync(map);
        await uow.Commit();
        return created;
    }

    public async Task<EmployeeInvitationEntity> Update(UpdateEmployeeInvitationDto dto,
        EmployeeInvitationEntity invitation)
    {
        if (!string.IsNullOrEmpty(dto.Message))
        {
            invitation.Message = dto.Message;
        }
        
        if (!string.IsNullOrEmpty(dto.PositionId))
        {
            invitation.PositionId = dto.PositionId;
        }
        
        if (!string.IsNullOrEmpty(dto.SalaryRange))
        {
            invitation.SalaryRange = dto.SalaryRange;
        }
        
        if (dto.EmploymentType.HasValue)
        {
            invitation.EmploymentType = dto.EmploymentType.Value;
        }
        
        if (dto.ProposedStartDate.HasValue && dto.ProposedStartDate != invitation.ProposedStartDate)
        {
            invitation.ProposedStartDate = dto.ProposedStartDate.Value;
        }
        
        if (dto.ProposedEndDate.HasValue && dto.ProposedEndDate != invitation.ProposedEndDate)
        {
            invitation.ProposedEndDate = dto.ProposedEndDate.Value;
        }
        
        if (dto.Currency.HasValue)
        {
            invitation.Currency = dto.Currency.Value;
        }
        
        if (dto.Currency.HasValue)
        {
            invitation.Currency = dto.Currency.Value;
        }
        
        EmployeeInvitationEntity update = await uow.EmployeeInvitationRepository.Update(invitation);
        await uow.Commit();
        return invitation;
    }

    public async Task<EmployeeInvitationEntity> UpdateByUser(UpdateEmployeeInvitationByUserDto dto,
        EmployeeInvitationEntity invitation)
    {
        if (!string.IsNullOrEmpty(dto.RejectReason))
        {
            invitation.RejectReason = dto.RejectReason;
        }
        
        if (dto.Status.HasValue)
        {
            invitation.Status = dto.Status.Value;
        }

        invitation.ResponseDate = DateTime.UtcNow;
        
        EmployeeInvitationEntity update = await uow.EmployeeInvitationRepository.Update(invitation);
        await uow.Commit();
        return invitation;
    }

    public IQueryable<EmployeeInvitationEntity> Query()
    {
        return uow.EmployeeInvitationRepository.ReturnIQueryable();
    }
    
}
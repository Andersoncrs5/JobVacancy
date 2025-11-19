
using JobVacancy.API.models.dtos.IndicationUser;
using JobVacancy.API.models.entities;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Uow.Interfaces;

namespace JobVacancy.API.Services.Providers;

public class IndicationUserService(IUnitOfWork uow): IIndicationUserService
{
    public async Task<bool> ExistsByEndorserIdAndEndorsedId(string endorserId, string endorsedId)
    {
        return await uow.IndicationUserRepository.ExistsByEndorserIdAndEndorsedId(endorserId, endorsedId);
    }
    
    public async Task<IndicationUserEntity?> GetByEndorserIdAndEndorsedId(string endorserId, string endorsedId)
    {
        return await uow.IndicationUserRepository.GetByEndorserIdAndEndorsedId(endorserId, endorsedId);
    }

    public async Task Delete(IndicationUserEntity entity)
    {
        uow.IndicationUserRepository.Delete(entity);
        await uow.Commit();
    }

    public async Task<IndicationUserEntity?> GetById(string id)
    {
        return await uow.IndicationUserRepository.GetByIdAsync(id);
    }
    
    public async Task<IndicationUserEntity> Create(CreateIndicationUserDto dto, string endorserId, string endorsedId)
    {
        IndicationUserEntity map = uow.Mapper.Map<IndicationUserEntity>(dto);
        map.EndorsedId = endorsedId;
        map.EndorserId = endorserId;

        IndicationUserEntity indicationNew = await uow.IndicationUserRepository.AddAsync(map);
        await uow.Commit();
        return indicationNew;
    }

    public async Task<IndicationUserEntity> Update(UpdateIndicationUserEndorserDto endorserDto, IndicationUserEntity entity)
    {
        if (!string.IsNullOrWhiteSpace(endorserDto.Content))
            entity.Content = endorserDto.Content;
        
        if (endorserDto.SkillRating.HasValue)
            entity.SkillRating = endorserDto.SkillRating.Value;
        
        IndicationUserEntity update = await uow.IndicationUserRepository.Update(entity);
        await uow.Commit();
        return update;
    }

    public async Task<IndicationUserEntity> UpdateByEndorsed(UpdateIndicationUserEndorsedDto endorserDto, IndicationUserEntity entity)
    {
        if (endorserDto.Status.HasValue)
            entity.Status = endorserDto.Status.Value;
        
        IndicationUserEntity update = await uow.IndicationUserRepository.Update(entity);
        await uow.Commit();
        return update;
    }

    public IQueryable<IndicationUserEntity> Query() 
    {
        return uow.IndicationUserRepository.Query();
    }
    
}
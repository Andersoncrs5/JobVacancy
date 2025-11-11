
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
    
    public async Task<IndicationUserEntity> Create(CreateIndicationUserDto dto, string endorserId, string endorsedId)
    {
        IndicationUserEntity map = uow.Mapper.Map<IndicationUserEntity>(dto);
        map.EndorsedId = endorsedId;
        map.EndorserId = endorserId;

        IndicationUserEntity indicationNew = await uow.IndicationUserRepository.AddAsync(map);
        await uow.Commit();
        return indicationNew;
    }

    public async Task<IndicationUserEntity> Update(UpdateIndicationUserDto dto, IndicationUserEntity entity)
    {
        if (!string.IsNullOrEmpty(entity.Content))
            entity.Content = entity.Content;
        
        if (dto.Status.HasValue)
            entity.Status = dto.Status.Value;
        
        if (dto.SkillRating.HasValue)
            entity.SkillRating = dto.SkillRating.Value;
        
        IndicationUserEntity update = await uow.IndicationUserRepository.Update(entity);
        await uow.Commit();
        return update;
    }

    public IQueryable<IndicationUserEntity> Query() 
    {
        return uow.IndicationUserRepository.ReturnIQueryable();
    }
    
}
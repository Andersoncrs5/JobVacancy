using JobVacancy.API.models.dtos.IndicationUser;
using JobVacancy.API.models.entities;

namespace JobVacancy.API.Services.Interfaces;

public interface IIndicationUserService
{
    Task<bool> ExistsByEndorserIdAndEndorsedId(string endorserId, string endorsedId);
    Task<IndicationUserEntity?> GetByEndorserIdAndEndorsedId(string endorserId, string endorsedId);
    Task Delete(IndicationUserEntity entity);
    Task<IndicationUserEntity> Create(CreateIndicationUserDto dto, string endorserId, string endorsedId);
    Task<IndicationUserEntity> Update(UpdateIndicationUserDto dto, IndicationUserEntity entity);
    IQueryable<IndicationUserEntity> Query();
    Task<IndicationUserEntity?> GetById(string id);
}
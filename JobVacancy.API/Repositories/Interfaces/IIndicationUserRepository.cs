using JobVacancy.API.models.entities;

namespace JobVacancy.API.Repositories.Interfaces;

public interface IIndicationUserRepository: IGenericRepository<IndicationUserEntity>
{
    Task<bool> ExistsByEndorserIdAndEndorsedId(string endorserId, string endorsedId);
    Task<IndicationUserEntity?> GetByEndorserIdAndEndorsedId(string endorserId, string endorsedId);
}
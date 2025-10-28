using JobVacancy.API.models.entities;

namespace JobVacancy.API.Repositories.Interfaces;

public interface IIndustryRepository: IGenericRepository<IndustryEntity>
{
    Task<bool> ExistsByName(string name);
}
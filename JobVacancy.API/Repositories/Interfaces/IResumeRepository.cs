using JobVacancy.API.models.entities;

namespace JobVacancy.API.Repositories.Interfaces;

public interface IResumeRepository: IGenericRepository<ResumeEntity>
{
    Task<bool> ExistsByName(string name);
    Task<ResumeEntity?> GetByName(string name);
    Task<bool> ExistsByObjectKey(string key);
    Task<ResumeEntity?> GetByObjectKey(string key);
}
using JobVacancy.API.models.entities;

namespace JobVacancy.API.Repositories.Interfaces;

public interface IApplicationVacancyRepository: IGenericRepository<ApplicationVacancyEntity>
{
    Task<bool> ExistsByVacancyIdAndUserId(string vacancyId, string userId);
    Task<ApplicationVacancyEntity?> GetByVacancyIdAndUserId(string vacancyId, string userId);
}
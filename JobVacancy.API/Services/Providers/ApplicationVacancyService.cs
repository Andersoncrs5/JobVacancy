using JobVacancy.API.models.dtos.ApplicationVacancy;
using JobVacancy.API.models.entities;
using JobVacancy.API.models.entities.Enums;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Uow.Interfaces;

namespace JobVacancy.API.Services.Providers;

public class ApplicationVacancyService(IUnitOfWork uow): IApplicationVacancyService
{
    public async Task<bool> ExistsById(string id)
        => await uow.ApplicationVacancyRepository.ExistsById(id);
    
    public async Task<ApplicationVacancyEntity?> GetById(string id)
        => await uow.ApplicationVacancyRepository.GetByIdAsync(id);
    
    public async Task<bool> ExistsByVacancyIdAndUserId(string vacancyId, string userId)
        => await uow.ApplicationVacancyRepository.ExistsByVacancyIdAndUserId(vacancyId, userId);
    
    public async Task<ApplicationVacancyEntity?> GetByVacancyIdAndUserId(string vacancyId, string userId)
        => await uow.ApplicationVacancyRepository.GetByVacancyIdAndUserId(vacancyId, userId);
    
    public IQueryable<ApplicationVacancyEntity> Query()
        => uow.ApplicationVacancyRepository.ReturnIQueryable();

    public async Task Delete(ApplicationVacancyEntity app)
    {
        uow.ApplicationVacancyRepository.Delete(app);
        await uow.Commit();
    }

    public async Task<ApplicationVacancyEntity> Create(CreateApplicationVacancyDto dto, string userId)
    {
        ApplicationVacancyEntity app = uow.Mapper.Map<ApplicationVacancyEntity>(dto);
        app.UserId = userId;
        app.Status = ApplicationStatusEnum.Submitted;

        ApplicationVacancyEntity applied = await uow.ApplicationVacancyRepository.AddAsync(app);
        await uow.Commit();
        return app;
    }

    public async Task<ApplicationVacancyEntity> Update(UpdateApplicationVacancyDto dto, ApplicationVacancyEntity app)
    {
        if (dto.Status.HasValue) 
        {
            app.Status = dto.Status.Value;
            app.LastStatusUpdateDate = DateTime.UtcNow;
        }
        
        if (dto.IsViewedByRecruiter.HasValue)
            app.IsViewedByRecruiter = dto.IsViewedByRecruiter.Value;

        ApplicationVacancyEntity update = await uow.ApplicationVacancyRepository.Update(app);
        
        await uow.Commit();
        return update;
    }
    
}
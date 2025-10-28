using AutoMapper;
using JobVacancy.API.models.entities;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Uow.Interfaces;

namespace JobVacancy.API.Services.Providers;

public class EnterpriseService(IUnitOfWork uow, IMapper mapper): IEnterpriseService
{
    public async Task<EnterpriseEntity?> GetById(string id)
    {
        return await uow.EnterpriseRepository.GetByIdAsync(id);
    }
    
    
    
}
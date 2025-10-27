using JobVacancy.API.models.dtos;
using JobVacancy.API.models.dtos.Users;
using JobVacancy.API.models.entities;
using AutoMapper;
using JobVacancy.API.models.dtos.Category;
using JobVacancy.API.Utils.Page;

namespace JobVacancy.API.Utils.Mappers;

public class ConfigMapper: Profile
{
    public ConfigMapper()
    {
        // User
        CreateMap<CreateUserDto, UserEntity>();
        CreateMap<UserEntity, UserDto>();
        
        CreateMap(typeof(PaginatedList<>), typeof(Page<>));
        
        // Category
        CreateMap<CreateCategoryDto, CategoryEntity>();
        CreateMap<CategoryEntity, CategoryDto>();
        CreateMap<UpdateCategoryDto, CategoryEntity>()
           .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
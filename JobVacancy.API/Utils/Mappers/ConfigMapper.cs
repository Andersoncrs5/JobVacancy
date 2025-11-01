using JobVacancy.API.models.dtos;
using JobVacancy.API.models.dtos.Users;
using JobVacancy.API.models.entities;
using AutoMapper;
using JobVacancy.API.models.dtos.Category;
using JobVacancy.API.models.dtos.Enterprise;
using JobVacancy.API.models.dtos.EnterpriseIndustry;
using JobVacancy.API.models.dtos.Industry;
using JobVacancy.API.models.dtos.PostEnterprise;
using JobVacancy.API.models.dtos.PostUser;
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
        CreateMap<CategoryDto, CategoryEntity>();
        CreateMap<UpdateCategoryDto, CategoryEntity>()
           .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        
        // Industry
        CreateMap<UpdateIndustryDto, IndustryEntity>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<CreateIndustryDto, IndustryEntity>();
        CreateMap<IndustryEntity, IndustryDto>();
        
        // Enterprise
        CreateMap<UpdateEnterpriseDto, EnterpriseEntity>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<CreateEnterpriseDto, EnterpriseEntity>();
        CreateMap<EnterpriseEntity, EnterpriseDto>();

        // Enterprise industry
        CreateMap<EnterpriseIndustryEntity, EnterpriseIndustryDto>();
        CreateMap<EnterpriseIndustryDto, EnterpriseIndustryEntity>();
        
        // PostUser
        CreateMap<CreatePostUserDto, PostUserEntity>();
        CreateMap<UpdatePostUserDto, PostUserEntity>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<PostUserEntity, PostUserDto>();
        
        // PostEnterprise
        CreateMap<CreatePostEnterpriseDto, PostEnterpriseEntity>();
        CreateMap<UpdatePostEnterpriseDto, PostEnterpriseEntity>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<PostEnterpriseEntity, PostEnterpriseDto>();
    }
}
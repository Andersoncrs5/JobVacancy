using JobVacancy.API.models.dtos;
using JobVacancy.API.models.dtos.Users;
using JobVacancy.API.models.entities;
using AutoMapper;
using JobVacancy.API.models.dtos.Area;
using JobVacancy.API.models.dtos.Base;
using JobVacancy.API.models.dtos.Category;
using JobVacancy.API.models.dtos.CommentPostEnterprise;
using JobVacancy.API.models.dtos.CommentPostUser;
using JobVacancy.API.models.dtos.EmployeeEnterprise;
using JobVacancy.API.models.dtos.EmployeeInvitation;
using JobVacancy.API.models.dtos.Enterprise;
using JobVacancy.API.models.dtos.EnterpriseIndustry;
using JobVacancy.API.models.dtos.FavoriteCommentPostEnterprise;
using JobVacancy.API.models.dtos.FavoriteCommentPostUser;
using JobVacancy.API.models.dtos.FavoritePost;
using JobVacancy.API.models.dtos.FavoritePostEnterprise;
using JobVacancy.API.models.dtos.IndicationUser;
using JobVacancy.API.models.dtos.Industry;
using JobVacancy.API.models.dtos.Position;
using JobVacancy.API.models.dtos.PostEnterprise;
using JobVacancy.API.models.dtos.PostUser;
using JobVacancy.API.models.dtos.ReviewEnterprise;
using JobVacancy.API.models.dtos.Skill;
using JobVacancy.API.models.dtos.UserSkill;
using JobVacancy.API.models.entities.Base;
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
        
        CreateMap<CreateSkillDto, SkillEntity>();
        CreateMap<UpdateSkillDto, SkillEntity>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<SkillEntity, SkillDto>();
        
        CreateMap<UserSkillDto, UserSkillEntity>();
        CreateMap<UserSkillEntity, UserSkillDto>();
        CreateMap<UpdateUserSkillDto, UserSkillEntity>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        
        
        CreateMap<FavoritePostUserEntity, FavoritePostUserDto>();
        CreateMap<FavoritePostUserDto, FavoritePostUserEntity>();
        CreateMap<UpdateFavoritePostUserDto, FavoritePostUserEntity>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        
        CreateMap<FavoritePostEnterpriseEntity, FavoritePostEnterpriseDto>();
        CreateMap<FavoritePostEnterpriseDto, FavoritePostEnterpriseEntity>();
        
        CreateMap<CommentPostUserEntity, CommentPostUserDto>();
        CreateMap<CommentPostUserDto, CommentPostUserEntity>();
        CreateMap<UpdateCommentPostUserDto, CommentPostUserEntity>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<CreateCommentPostUserDto, CommentPostUserEntity>();
        
        CreateMap<CommentPostEnterpriseEntity, CommentPostEnterpriseDto>();
        CreateMap<CommentPostEnterpriseDto, CommentPostEnterpriseEntity>();
        CreateMap<UpdateCommentPostEnterpriseDto, CommentPostEnterpriseEntity>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<CreateCommentPostEnterpriseDto, CommentPostEnterpriseEntity>();
        
        CreateMap<FavoriteCommentEntity, FavoriteCommentPostUserDto>();
        CreateMap<FavoriteCommentEntity, FavoriteCommentPostEnterpriseDto>();

        CreateMap<CommentBaseEntity, CommentBase>()
            .Include<CommentPostUserEntity, CommentPostUserDto>()
            .Include<CommentPostEnterpriseEntity, CommentPostEnterpriseDto>();
        
        CreateMap<CreateEmployeeInvitationDto, EmployeeInvitationEntity>();
        CreateMap<UpdateEmployeeInvitationDto, EmployeeInvitationEntity>()

            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.EnterpriseId, opt => opt.Ignore())
            .ForMember(dest => dest.InviteSenderId, opt => opt.Ignore())
            .ForMember(dest => dest.Token, opt => opt.Ignore())
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null)) ;
            
        CreateMap<EmployeeInvitationEntity, EmployeeInvitationDto>();
        
        CreateMap<PositionEntity, PositionDto>();
        CreateMap<CreatePositionDto, PositionEntity>();
        CreateMap<UpdatePositionDto, PositionEntity>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null)) ;

        
        CreateMap<EmployeeEnterpriseEntity, EmployeeEnterpriseDto>();
        CreateMap<CreateEmployeeEnterpriseDto, EmployeeEnterpriseEntity>();
        CreateMap<UpdateEmployeeEnterpriseDto, EmployeeEnterpriseEntity>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null)) ;

        CreateMap<ReviewEnterpriseEntity, ReviewEnterpriseDto>();
        CreateMap<CreateReviewEnterpriseDto, ReviewEnterpriseEntity>();
        CreateMap<UpdateReviewEnterpriseDto, ReviewEnterpriseEntity>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null)) ;
        
        CreateMap<IndicationUserEntity, IndicationUserDto>();
        CreateMap<CreateIndicationUserDto, IndicationUserEntity>();
        
        CreateMap<AreaEntity, AreaDto>();
        CreateMap<AreaDto, AreaEntity>();
        CreateMap<CreateAreaDto, AreaEntity>();
    }
}
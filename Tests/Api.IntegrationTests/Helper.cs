using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using FluentAssertions.Extensions;
using JobVacancy.API.IntegrationTests.Utils;
using JobVacancy.API.models.dtos.ApplicationVacancy;
using JobVacancy.API.models.dtos.Area;
using JobVacancy.API.models.dtos.Category;
using JobVacancy.API.models.dtos.CommentPostEnterprise;
using JobVacancy.API.models.dtos.CommentPostUser;
using JobVacancy.API.models.dtos.EmployeeEnterprise;
using JobVacancy.API.models.dtos.EmployeeInvitation;
using JobVacancy.API.models.dtos.Enterprise;
using JobVacancy.API.models.dtos.EnterpriseFollowsUser;
using JobVacancy.API.models.dtos.EnterpriseIndustry;
using JobVacancy.API.models.dtos.FavoritePost;
using JobVacancy.API.models.dtos.FavoritePostEnterprise;
using JobVacancy.API.models.dtos.FollowerRelationshipUser;
using JobVacancy.API.models.dtos.FollowerUserRelationshipEnterprise;
using JobVacancy.API.models.dtos.IndicationUser;
using JobVacancy.API.models.dtos.Industry;
using JobVacancy.API.models.dtos.Position;
using JobVacancy.API.models.dtos.PostEnterprise;
using JobVacancy.API.models.dtos.PostUser;
using JobVacancy.API.models.dtos.ReviewEnterprise;
using JobVacancy.API.models.dtos.ReviewUser;
using JobVacancy.API.models.dtos.Skill;
using JobVacancy.API.models.dtos.UserEvaluation;
using JobVacancy.API.models.dtos.Users;
using JobVacancy.API.models.dtos.UserSkill;
using JobVacancy.API.models.dtos.Vacancy;
using JobVacancy.API.models.dtos.VacancySkill;
using JobVacancy.API.models.entities.Enums;
using JobVacancy.API.Utils.Res;
using Microsoft.Extensions.Configuration;

namespace JobVacancy.API.IntegrationTests;

public class Helper(
    HttpClient client
    )
{

    public async Task<UserEvaluationDto> CreateUserEvaluation(UserResultTest userGuest, PositionDto positionDto)
    {
        CreateUserEvaluationDto dto = new CreateUserEvaluationDto()
        {
            TargetUserId = userGuest.User!.Id,
            Content = string.Concat(Enumerable.Repeat("Any", 30)),
            EmploymentStatus = EmploymentTypeEnum.Freelance,
            PositionId = positionDto.Id,
            IsAnonymous = false,
            RatingCompensation = 5,
            RatingCulture = 5,
            RatingManagement = 5,
            RatingOverall = 5,
            RatingProfessionalism = 5,
            RatingSkillMatch = 5,
            RatingTeamwork = 5,
            RatingWorkLifeBalance = 5,
            RecommendationTone = 5,
            Title = string.Concat(Enumerable.Repeat("Any", 30)),
        };

        HttpResponseMessage message = await client.PostAsJsonAsync("/api/v1/UserEvaluation", dto);
        
        message.StatusCode.Should().Be(HttpStatusCode.Created);

        ResponseHttp<UserEvaluationDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<UserEvaluationDto>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int)HttpStatusCode.Created);
        http.Message.Should().NotBeNullOrWhiteSpace();
        
        http.Data.Should().NotBeNull();
        http.Data.Id.Should().NotBeNullOrWhiteSpace();
        http.Data.TargetUserId.Should().Be(dto.TargetUserId);
        http.Data.Content.Should().Be(dto.Content);
        http.Data.EmploymentStatus.Should().Be(dto.EmploymentStatus);
        http.Data.PositionId.Should().Be(dto.PositionId);
        http.Data.IsAnonymous.Should().Be(dto.IsAnonymous);
        http.Data.RatingCompensation.Should().Be(dto.RatingCompensation);
        http.Data.RatingCulture.Should().Be(dto.RatingCulture);
        http.Data.RatingManagement.Should().Be(dto.RatingManagement);
        http.Data.RatingOverall.Should().Be(dto.RatingOverall);
        http.Data.RatingProfessionalism.Should().Be(dto.RatingProfessionalism);
        http.Data.RatingSkillMatch.Should().Be(dto.RatingSkillMatch);
        http.Data.RatingWorkLifeBalance.Should().Be(dto.RatingWorkLifeBalance);
        http.Data.RecommendationTone.Should().Be(dto.RecommendationTone);
        http.Data.Title.Should().Be(dto.Title);
        http.Data.TargetUserId.Should().Be(dto.TargetUserId);
        
        return http.Data;
    }
    
    public async Task<ReviewUserDto> CreateReviewToUser(UserResultTest target, UserResultTest actor)
    {
        
        CreateReviewUserDto dto = new CreateReviewUserDto()
        {
            TargetUserId = target.User!.Id,
            Content = string.Concat(Enumerable.Repeat("Content", 20)),
            IsAnonymous = false,
            RatingCompensation = 4,
            RatingCulture = 4,
            RatingManagement = 4,
            RatingOverall = 4,
            RatingWorkLifeBalance = 4,
            Recommendation = true,
            Title = string.Concat(Enumerable.Repeat("Title", 10)),
        };

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", actor.Tokens!.Token!);

        HttpResponseMessage message = await client.PostAsJsonAsync($"/api/v1/ReviewUser", dto);
        message.StatusCode.Should().Be(HttpStatusCode.Created);

        ResponseHttp<ReviewUserDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<ReviewUserDto>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int)HttpStatusCode.Created);
        http.Status.Should().BeTrue();
        http.Message.Should().NotBeNullOrWhiteSpace();
        
        http.Data.Should().NotBeNull();
        http.Data.Id.Should().NotBeNullOrWhiteSpace();
        http.Data.Title.Should().Be(dto.Title);
        http.Data.IsAnonymous.Should().Be(dto.IsAnonymous);
        http.Data.RatingCompensation.Should().Be(dto.RatingCompensation);
        http.Data.RatingCulture.Should().Be(dto.RatingCulture);
        http.Data.RatingManagement.Should().Be(dto.RatingManagement);
        http.Data.RatingOverall.Should().Be(dto.RatingOverall);
        http.Data.RatingWorkLifeBalance.Should().Be(dto.RatingWorkLifeBalance);
        http.Data.Recommendation.Should().Be(dto.Recommendation);
        http.Data.TargetUserId.Should().Be(dto.TargetUserId);
        http.Data.Content.Should().Be(dto.Content);
        
        return http.Data;
    }
    
    public async Task<EnterpriseFollowsUserDto> AddEnterpriseFollowsUser(UserResultTest userToFollow, EnterpriseDto enterprise)
    {
        HttpResponseMessage message = await client.PostAsync($"/api/v1/EnterpriseFollowsUser/{userToFollow.User!.Id}", null);
        message.StatusCode.Should().Be(HttpStatusCode.Created);

        ResponseHttp<EnterpriseFollowsUserDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<EnterpriseFollowsUserDto>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int)HttpStatusCode.Created);
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Status.Should().BeTrue();
        
        http.Data.Should().NotBeNull();
        http.Data.Id.Should().NotBeNullOrWhiteSpace();
        http.Data.EnterpriseId.Should().Be(enterprise.Id);
        http.Data.UserId.Should().Be(userToFollow.User.Id);
        
        return http.Data;
    }
    
    public async Task<FollowerUserRelationshipEnterpriseDto> AddUserFollowEnterprise(EnterpriseDto enterprise, UserResultTest userFollowed)
    {
        HttpResponseMessage message = await client.PostAsync($"/api/v1/FollowerUserRelationshipEnterprise/{enterprise.Id}/Toggle", null);
        message.StatusCode.Should().Be(HttpStatusCode.Created);

        ResponseHttp<FollowerUserRelationshipEnterpriseDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<FollowerUserRelationshipEnterpriseDto>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int) HttpStatusCode.Created);
        http.Status.Should().BeTrue();
        
        http.Data.Should().NotBeNull();
        http.Data.UserId.Should().Be(userFollowed.User!.Id);
        http.Data.EnterpriseId.Should().Be(enterprise.Id);
        http.Data.WishReceiveNotifyByNewComment.Should().BeTrue();
        http.Data.WishReceiveNotifyByNewPost.Should().BeTrue();
        http.Data.WishReceiveNotifyByNewVacancy.Should().BeTrue();
        http.Data.WishReceiveNotifyByNewInteraction.Should().BeFalse();
        
        return http.Data;
    }
    
    public async Task<FollowerRelationshipUserDto> CreateFollowerRelationshipUser(UserResultTest follower, UserResultTest followed)
    {
        HttpResponseMessage message = await client.PostAsync($"/api/v1/FollowerRelationshipUser/{followed.User!.Id}/Toggle", null);
        message.StatusCode.Should().Be(HttpStatusCode.Created);

        ResponseHttp<FollowerRelationshipUserDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<FollowerRelationshipUserDto>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int) HttpStatusCode.Created);
        http.Status.Should().BeTrue();
        
        http.Data.Should().NotBeNull();
        http.Data.Id.Should().NotBeNullOrWhiteSpace();
        http.Data.FollowedId.Should().Be(followed.User.Id);
        http.Data.FollowerId.Should().Be(follower.User!.Id);
        
        return http.Data;
    }
    
    public async Task<ApplicationVacancyDto> CreateApplication(VacancyDto vacancy, UserResultTest candidate)
    {
        
        CreateApplicationVacancyDto dto = new CreateApplicationVacancyDto
        {
            VacancyId = vacancy.Id,
            CoverLetter = string.Concat(Enumerable.Repeat("AnyMessage", 20)),
        };

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", candidate.Tokens!.Token!);
        
        HttpResponseMessage message = await client.PostAsJsonAsync("/api/v1/ApplicationVacancy", dto);
        
        message.StatusCode.Should().Be(HttpStatusCode.Created);

        ResponseHttp<ApplicationVacancyDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<ApplicationVacancyDto>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int)HttpStatusCode.Created);
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Status.Should().BeTrue();
        
        http.Data.Should().NotBeNull();
        http.Data.Id.Should().NotBeNullOrWhiteSpace();
        http.Data.VacancyId.Should().Be(vacancy.Id);
        http.Data.UserId.Should().Be(candidate.User!.Id);
        
        return http.Data;
    }

    public async Task<VacancyDto> ChangeStatusVacancy( string vacancyId, VacancyStatusEnum? status = null, DateTime? deadLine = null)
    {
        UpdateVacancyDto dto = new UpdateVacancyDto
        {
            Status = status,
            ApplicationDeadLine = deadLine
        };
        
        HttpResponseMessage message = await client.PatchAsJsonAsync($"/api/v1/Vacancy/{vacancyId}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<VacancyDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<VacancyDto>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int) HttpStatusCode.OK);
        http.Status.Should().BeTrue();
        http.Message.Should().NotBeNullOrWhiteSpace();
        
        http.Data.Should().NotBeNull();
        
        return http.Data;
    }
    
    public async Task<VacancySkillDto> AddVacancySkillToVacancy(VacancyDto vacancyDto, SkillDto skillDto)
    {
        CreateVacancySkillDto dto = new CreateVacancySkillDto
        {
            VacancyId = vacancyDto.Id,
            SkillId = skillDto.Id,
            IsMandatory = true,
            RequiredLevel = SkillProficiencyLevelEnum.Beginner,
            Weight = 4,
            YearsOfExperienceRequired = 2
        };

        HttpResponseMessage message = await client.PostAsJsonAsync("/api/v1/VacancySkill", dto);
        message.StatusCode.Should().Be(HttpStatusCode.Created);

        ResponseHttp<VacancySkillDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<VacancySkillDto>>();
        http.Should().NotBeNull();
        
        http.Code.Should().Be((int)HttpStatusCode.Created);
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Status.Should().BeTrue();
        
        http.Data.Should().NotBeNull();
        http.Data.Id.Should().NotBeNullOrWhiteSpace();
        http.Data.SkillId.Should().Be(dto.SkillId);
        http.Data.VacancyId.Should().Be(dto.VacancyId);
        http.Data.IsMandatory.Should().Be(dto.IsMandatory);
        http.Data.RequiredLevel.Should().Be(dto.RequiredLevel);
        http.Data.Weight.Should().Be(dto.Weight);
        http.Data.YearsOfExperienceRequired.Should().Be(dto.YearsOfExperienceRequired);
        
        return http.Data;
    }
    
    public async Task<VacancyDto> CreateVacancy(AreaDto areaDto)
    {
        CreateVacancyDto dto = new CreateVacancyDto()
        {
            Title = string.Concat(Enumerable.Repeat(Guid.NewGuid().ToString(), 5)),
            Description = string.Concat(Enumerable.Repeat(Guid.NewGuid().ToString(), 15)),
            AreaId = areaDto.Id,
            Benefits = string.Concat(Enumerable.Repeat(Guid.NewGuid().ToString(), 15)),
            Currency = CurrencyEnum.Usd,
            EducationLevel = EducationLevelEnum.Technical,
            EmploymentType = EmploymentTypeEnum.PartTime,
            ExperienceLevel = ExperienceLevelEnum.Junior,
            Opening = 1,
            Requirements = string.Concat(Enumerable.Repeat(Guid.NewGuid().ToString(), 15)),
            Responsibilities = string.Concat(Enumerable.Repeat(Guid.NewGuid().ToString(), 15)),
            SalaryMin = 2000.8m,
            SalaryMax = 4000.8m,
            Seniority = 3,
            WorkplaceType = WorkplaceTypeEnum.Hybrid,
            ApplicationDeadLine = DateTime.UtcNow.AddMonths(1),
        };

        HttpResponseMessage message = await client.PostAsJsonAsync("/api/v1/Vacancy", dto);
        message.StatusCode.Should().Be(HttpStatusCode.Created);

        ResponseHttp<VacancyDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<VacancyDto>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int) HttpStatusCode.Created);
        http.Status.Should().BeTrue();
        http.Message.Should().NotBeNullOrWhiteSpace();
        
        http.Data.Should().NotBeNull();
        http.Data.Id.Should().NotBeNullOrWhiteSpace();
        http.Data.Title.Should().Be(dto.Title);
        http.Data.Description.Should().Be(dto.Description);
        http.Data.AreaId.Should().Be(areaDto.Id);
        http.Data.Benefits.Should().Be(dto.Benefits);
        http.Data.Currency.Should().Be(dto.Currency);
        http.Data.EducationLevel.Should().Be(dto.EducationLevel);
        http.Data.EmploymentType.Should().Be(dto.EmploymentType);
        http.Data.ExperienceLevel.Should().Be(dto.ExperienceLevel);
        http.Data.Opening.Should().Be(dto.Opening);
        http.Data.Requirements.Should().Be(dto.Requirements);
        http.Data.SalaryMin.Should().Be(dto.SalaryMin);
        http.Data.SalaryMax.Should().Be(dto.SalaryMax);
        http.Data.Seniority.Should().Be(dto.Seniority);
        http.Data.WorkplaceType.Should().Be(dto.WorkplaceType);
        http.Data.Responsibilities.Should().Be(dto.Responsibilities);
        http.Data.ApplicationDeadLine.Should().BeCloseTo(dto.ApplicationDeadLine.Value, TimeSpan.FromSeconds(1));
        
        return http.Data;
    }
    
    public async Task<AreaDto> CreateArea(bool isActive = true)
    {
        CreateAreaDto dto = new CreateAreaDto
        {
            IsActive = isActive,
            Name = $"Test {Guid.NewGuid()}",
            Description = $"Test description {Guid.NewGuid()}",
        };

        HttpResponseMessage message = await client.PostAsJsonAsync("api/v1/Area", dto);
        message.StatusCode.Should().Be(HttpStatusCode.Created);

        ResponseHttp<AreaDto>? response = await message.Content.ReadFromJsonAsync<ResponseHttp<AreaDto>>();
        response.Should().NotBeNull();
        response.Status.Should().BeTrue();
        response.Message.Should().NotBeNullOrWhiteSpace();
        response.Code.Should().Be((int) HttpStatusCode.Created);
        
        Assert.NotNull(response.Data);
        response.Data.Id.Should().NotBeNullOrWhiteSpace();
        response.Data.Name.Should().Be(dto.Name);
        response.Data.Description.Should().Be(dto.Description);
        response.Data.IsActive.Should().Be(dto.IsActive);
        
        return response.Data;
    }
    
    public async Task<IndicationUserDto> AddIndicationUser(UserResultTest endorsedUser)
    {
        CreateIndicationUserDto dto = new CreateIndicationUserDto()
        {
            EndorsedId = endorsedUser.User!.Id,
            Content = string.Concat(Enumerable.Repeat("HeIsAWellEmployee", 10)),
            SkillRating = Random.Shared.Next(0, 10),
        };
        
        HttpResponseMessage message = await client.PostAsJsonAsync("/api/v1/IndicationUser", dto);
        message.StatusCode.Should().Be(HttpStatusCode.Created);

        ResponseHttp<IndicationUserDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<IndicationUserDto>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int)HttpStatusCode.Created);
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Status.Should().BeTrue();
        
        http.Data.Should().NotBeNull();
        http.Data.Id.Should().NotBeNullOrWhiteSpace();
        http.Data.EndorsedId.Should().Be(endorsedUser.User.Id);
        http.Data.Content.Should().Be(dto.Content);
        http.Data.SkillRating.Should().Be(dto.SkillRating);
        
        return http.Data;
    }
    
    public async Task<ReviewEnterpriseDto> CreateReviewEnterprise(EmployeeInvitationDto invitation)
    {
        CreateReviewEnterpriseDto reviewDto = new CreateReviewEnterpriseDto()
        {
            Title = string.Concat(Enumerable.Repeat("HelloWorld", 20)),
            Content = string.Concat(Enumerable.Repeat("HelloWorld", 50)),
            IsAnonymous = true,
            RatingOverall = Random.Shared.Next(0, 5),
            RatingCulture = Random.Shared.Next(0, 5),
            RatingManagement = Random.Shared.Next(0, 5),
            RatingCompensation = Random.Shared.Next(0, 5),
            RatingWorkLifeBalance = Random.Shared.Next(0, 5),
        };

        HttpResponseMessage message = await client.PostAsJsonAsync($"/api/v1/ReviewEnterprise/{invitation.EnterpriseId}", reviewDto);
        message.StatusCode.Should().Be(HttpStatusCode.Created);

        ResponseHttp<ReviewEnterpriseDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<ReviewEnterpriseDto>>();
        http.Should().NotBeNull();
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Code.Should().Be((int)HttpStatusCode.Created);
        http.Status.Should().BeTrue();
        http.Version.Should().Be(1);
        
        http.Data.Should().NotBeNull();
        http.Data.Id.Should().NotBeNullOrWhiteSpace();
        http.Data.Title.Should().Be(reviewDto.Title);
        http.Data.Content.Should().Be(reviewDto.Content);
        http.Data.IsAnonymous.Should().Be(reviewDto.IsAnonymous);
        http.Data.RatingOverall.Should().Be(reviewDto.RatingOverall);
        http.Data.RatingCulture.Should().Be(reviewDto.RatingCulture);
        http.Data.RatingManagement.Should().Be(reviewDto.RatingManagement);
        http.Data.RatingCompensation.Should().Be(reviewDto.RatingCompensation);
        http.Data.RatingWorkLifeBalance.Should().Be(reviewDto.RatingWorkLifeBalance);
        http.Data.PositionId.Should().Be(invitation.PositionId);
        http.Data.EnterpriseId.Should().Be(invitation.EnterpriseId);
        
        return http.Data;
    }
    
    public async Task<EmployeeEnterpriseDto> CreateEmployeeEnterprise(EmployeeInvitationDto invitationUpdated)
    {
        string _url = "/api/v1/EmployeeEnterprise";
        CreateEmployeeEnterpriseDto dto = new CreateEmployeeEnterpriseDto()
        {
            InviteSenderId = invitationUpdated.InviteSenderId,
            UserId = invitationUpdated.UserId,
            ContractLegalType = ContractLegalTypeEnum.Indefinite,
            ContractLink = null,
            Currency = CurrencyEnum.Cad,
            EmploymentStatus = EmploymentStatusEnum.CurrentEmployee,
            EmploymentType = invitationUpdated.EmploymentType,
            PositionId = invitationUpdated.PositionId,
            EndDate = null,
            Notes = string.Concat(Enumerable.Repeat("OKOK", 20)),
            PaymentFrequency = PaymentFrequencyEnum.Monthly,
            SalaryRange = invitationUpdated.SalaryRange,
            SalaryValue = 8659.75m,
            StartDate = invitationUpdated.ProposedStartDate,
        };
        
        HttpResponseMessage message = await client.PostAsJsonAsync($"{_url}/{invitationUpdated.Id}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.Created);

        ResponseHttp<EmployeeEnterpriseDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<EmployeeEnterpriseDto>>();
        http.Should().NotBeNull();
        http.Version.Should().Be(1);
        http.Code.Should().Be((int)HttpStatusCode.Created);
        http.Status.Should().BeTrue();
        
        http.Data.Should().NotBeNull();
        http.Data.Id.Should().NotBeNullOrWhiteSpace();
        http.Data.InviteSenderId.Should().Be(invitationUpdated.InviteSenderId);
        http.Data.UserId.Should().Be(invitationUpdated.UserId);
        http.Data.ContractLegalType.Should().Be(dto.ContractLegalType);
        http.Data.ContractLink.Should().Be(dto.ContractLink);
        http.Data.Currency.Should().Be(dto.Currency);
        http.Data.EmploymentStatus.Should().Be(dto.EmploymentStatus);
        http.Data.EmploymentType.Should().Be(dto.EmploymentType);
        http.Data.PositionId.Should().Be(dto.PositionId);
        http.Data.EndDate.Should().Be(dto.EndDate);
        http.Data.Notes.Should().Be(dto.Notes);
        http.Data.PaymentFrequency.Should().Be(dto.PaymentFrequency);
        http.Data.SalaryRange.Should().Be(dto.SalaryRange);
        http.Data.SalaryValue.Should().Be(dto.SalaryValue);
        http.Data.StartDate.Should().Be(dto.StartDate);
        
        return http.Data;
    }
    
    public async Task<EmployeeInvitationDto> UpdateInvitationByUser(StatusEnum status, EmployeeInvitationDto invitation)
    {
        string url = "/api/v1/EmployeeInvitation";
        UpdateEmployeeInvitationByUserDto dto = new UpdateEmployeeInvitationByUserDto
        {
            Status = status,
            RejectReason = string.Concat(Enumerable.Repeat("SalaryBad", 30))
        };
        
        HttpResponseMessage message = await client.PatchAsJsonAsync($"{url}/{invitation.Id}/By/User", dto);
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<EmployeeInvitationDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<EmployeeInvitationDto>>();
        http.Should().NotBeNull();
        http.Data.Should().NotBeNull();
        http.Status.Should().BeTrue();
        http.Code.Should().Be((int)HttpStatusCode.OK);
        http.Message.Should().NotBeNullOrWhiteSpace();
        
        http.Data.Id.Should().Be(invitation.Id);
        http.Data.EnterpriseId.Should().Be(invitation.EnterpriseId);
        http.Data.InviteSenderId.Should().Be(invitation.InviteSenderId);
        
        http.Data.Status.Should().Be(dto.Status);
        http.Data.RejectReason.Should().Be(dto.RejectReason);
        
        http.Data.ProposedStartDate.Should().BeCloseTo(
            invitation.ProposedStartDate, 
            1.Microseconds() 
        );
        http.Data.ProposedEndDate.Should().NotBeNull();
        http.Data.ProposedEndDate.Value.Should().BeCloseTo(
            invitation.ProposedEndDate!.Value, 
            1.Microseconds() 
        );
        http.Data.Currency.Should().Be(invitation.Currency);
        http.Data.EmploymentType.Should().Be(invitation.EmploymentType);
        http.Data.Message.Should().Be(invitation.Message);
        http.Data.PositionId.Should().Be(invitation.PositionId);
        http.Data.SalaryRange.Should().Be(invitation.SalaryRange);
        
        return http.Data;
    }
    
    public async Task<PositionDto> CreatePositionAsync()
    {
        string _URL = "/api/v1/Position";
        int num = Random.Shared.Next(1, 1000000);
        
        CreatePositionDto dto = new CreatePositionDto()
        {
            Describe = string.Concat(Enumerable.Repeat("AnyDesc", 5)),
            Name = Guid.NewGuid().ToString(),
            IsActive = true
        };

        HttpResponseMessage message = await client.PostAsJsonAsync(_URL, dto);
        message.StatusCode.Should().Be(HttpStatusCode.Created);
        ResponseHttp<PositionDto>? content = await message.Content.ReadFromJsonAsync<ResponseHttp<PositionDto>>();
        
        content.Should().NotBeNull();
        content.Status.Should().BeTrue();
        content.Message.Should().NotBeNullOrWhiteSpace();
        content.Code.Should().Be((int)HttpStatusCode.Created);
        
        content.Data.Should().NotBeNull();
        content.Data.Id.Should().NotBeNullOrWhiteSpace();
        content.Data.Name.Should().Be(dto.Name);
        content.Data.Describe.Should().Be(dto.Describe);
        content.Data.IsActive.Should().Be(dto.IsActive);
        
        return content.Data;
    }
    
    public async Task<EmployeeInvitationDto> CreateEmployeeInvitation(UserResultTest userGuest, PositionDto position)
    {
        string _URL = "/api/v1/EmployeeInvitation";
        CreateEmployeeInvitationDto dto = new CreateEmployeeInvitationDto
        {
            UserId = userGuest.User!.Id,
            Currency = CurrencyEnum.Aud,
            EmploymentType = EmploymentTypeEnum.Temporary,
            Message = "AnyMessage",
            PositionId = position.Id,
            ProposedStartDate = DateTime.UtcNow.AddDays(7),
            ProposedEndDate = DateTime.UtcNow.AddDays(20),
            SalaryRange = "5000-7000",
            
        };

        HttpResponseMessage message = await client.PostAsJsonAsync($"{_URL}", dto);
        
        message.StatusCode.Should().Be(HttpStatusCode.Created);

        ResponseHttp<EmployeeInvitationDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<EmployeeInvitationDto>>();
        http.Should().NotBeNull();
        http.Data.Should().NotBeNull();
        http.Status.Should().BeTrue();
        http.Code.Should().Be((int)HttpStatusCode.Created);
        http.Message.Should().NotBeNullOrWhiteSpace();
        
        http.Data.Id.Should().NotBeNullOrWhiteSpace();
        http.Data.UserId.Should().Be(dto.UserId);
        http.Data.Currency.Should().Be(dto.Currency);
        http.Data.EmploymentType.Should().Be(dto.EmploymentType);
        http.Data.Message.Should().Be(dto.Message);
        http.Data.PositionId.Should().Be(dto.PositionId);
        http.Data.ProposedStartDate.Should().Be(dto.ProposedStartDate);
        http.Data.ProposedEndDate.Should().Be(dto.ProposedEndDate);
        http.Data.SalaryRange.Should().Be(dto.SalaryRange);

        return http.Data;
    }
    
    public async Task AddFavoriteCommentPostEnterprise(string commentId)
    {
        string _url = "/api/v1/FavoriteCommentPostEnterprise";
            
        HttpResponseMessage message = await client.PostAsync($"{_url}/{commentId}/Toggle", null);
        message.StatusCode.Should().Be(HttpStatusCode.Created);

        ResponseHttp<object>? content = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        
        content.Should().NotBeNull();
        content.Data.Should().BeNull();
        content.Code.Should().Be((int)HttpStatusCode.Created);
        content.Status.Should().BeTrue();
    }
    
    public async Task<CommentPostEnterpriseDto> CreateCommentPostEnterpriseDto(string postId, bool isActive, string? parentId = null, string? content = null)
    {
        string _url = "/api/v1/CommentPostEnterprise";
        
        CreateCommentPostEnterpriseDto dto = new CreateCommentPostEnterpriseDto
        {
            Content = string.Concat(Enumerable.Repeat(content != null ? content : "AnyContent", 30)),
            Depth = 5,
            PostId = postId,
            ImageUrl = "https://github.com/Andersoncrs5",
            IsActive = isActive
        };
        
        HttpResponseMessage message = await client.PostAsJsonAsync($"{_url}?parentId={parentId}", dto);
        
        message.StatusCode.Should().Be(HttpStatusCode.Created);

        ResponseHttp<CommentPostEnterpriseDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<CommentPostEnterpriseDto>>();
        http.Code.Should().Be(201);
        http.Data.Should().NotBeNull();
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Status.Should().BeTrue();
        
        http.Data.Id.Should().NotBeNullOrWhiteSpace();
        
        return http.Data;
    }
    
    public async Task AddFavoriteCommentPostUser(string commentId)
    {
        string _url = "/api/v1/FavoriteCommentPostUser";
        HttpResponseMessage message = await client.PostAsync($"{_url}/{commentId}/Toggle", null);
        message.StatusCode.Should().Be(HttpStatusCode.Created);

        ResponseHttp<object>? content = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        
        content.Should().NotBeNull();
        content.Data.Should().BeNull();
        content.Code.Should().Be((int)HttpStatusCode.Created);
        content.Status.Should().BeTrue();
    }
    
    public async Task<FavoritePostEnterpriseDto> AddFavoritePostEnterprise(PostEnterpriseDto post)
    {
        string _url = "/api/v1/FavoritePostEnterprise";
        
        HttpResponseMessage message = await client.PostAsync($"{_url}/{post.Id}/Toggle", null);
        message.StatusCode.Should().Be(HttpStatusCode.Created);

        ResponseHttp<FavoritePostEnterpriseDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<FavoritePostEnterpriseDto>>();
        http.Should().NotBeNull();
        http.Status.Should().BeTrue();
        
        http.Data.Should().NotBeNull();
        http.Data.Id.Should().NotBeNullOrWhiteSpace();
        
        return http.Data;
    }
    
    public async Task<FavoritePostUserDto> AddFavoritePost(PostUserDto postUser)
    {
        string _url = "/api/v1/FavoritePostUser";  
        
        HttpResponseMessage message = await client.PostAsync($"{_url}/{postUser.Id}/Toggle", null);
        
        message.StatusCode.Should().Be(HttpStatusCode.Created);

        ResponseHttp<FavoritePostUserDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<FavoritePostUserDto>>();
        http.Should().NotBeNull();
        http.Data.Should().NotBeNull();
        http.Status.Should().BeTrue();
        
        return http.Data;
    }
    
    public async Task<UserSkillDto> CreateUserSkill(SkillDto skill)
    {
        string _URL = "/api/v1/UserSkill";
        HttpResponseMessage message = await client.PostAsync($"{_URL}/Toggle/{skill.Id}", null);
        message.StatusCode.Should().Be(HttpStatusCode.Created);

        ResponseHttp<UserSkillDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<UserSkillDto>>();
        http.Should().NotBeNull();

        http.Code.Should().Be(201);
        http.Status.Should().BeTrue();
        http.Message.Should().NotBeNullOrWhiteSpace();
        
        http.Data.Should().NotBeNull();
        http.Data.Id.Should().NotBeNullOrWhiteSpace();
        http.Data.SkillId.Should().Be(skill.Id);
        http.Data.ExternalCertificateUrl.Should().BeNull();
        http.Data.YearsOfExperience.Should().BeNull();
        http.Data.ProficiencyLevel.Should().BeNull();
        
        return http.Data;
    }
    
    public async Task<SkillDto> CreateSkill()
    {
        CreateSkillDto dto = new CreateSkillDto
        {
            Name = "TestSkill" + Guid.NewGuid().ToString(),
            Description = "TestSkill" + Guid.NewGuid().ToString(),
            IconUrl = "https://github.com/Andersoncrs5",
            IsActive = true
        };

        HttpResponseMessage message = await client.PostAsJsonAsync("/api/v1/Skill", dto);
        message.StatusCode.Should().Be(HttpStatusCode.Created);

        ResponseHttp<SkillDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<SkillDto>>();
        http.Should().NotBeNull();
        http.Data.Should().NotBeNull();
        http.Data.Name.Should().Be(dto.Name);
        http.Data.Description.Should().Be(dto.Description);
        http.Data.IconUrl.Should().Be(dto.IconUrl);
        http.Data.IsActive.Should().Be(true);
        http.Data.Id.Should().NotBeEmpty();
        
        return http.Data;
    }
    
    public async Task<ResponseTokens> LoginMaster(IConfiguration configuration)
    {
        var datasSystemSection = configuration.GetSection("DataSystem");
        string systemUserEmail = datasSystemSection["systemUserEmail"] ?? throw new InvalidOperationException("System user email configuration is missing.");
        string systemUserPassword = datasSystemSection["SystemUserPassword"] ?? throw new InvalidOperationException("System user password configuration is missing.");
        
        var http = await _Login(systemUserEmail, systemUserPassword);
        return http.Data!;
    }
    public async Task<ResponseTokens> LoginUser(string email, string password)
    {
        var http = await _Login(email, password);
        http.Data.Should().NotBeNull();
        return http.Data;
    }

    private async Task<ResponseHttp<ResponseTokens>> _Login(string email, string password)
    {
        LoginDto dto = new LoginDto
        {
            Email = email,
            Password = password
        };
        
        HttpResponseMessage response = await client.PostAsJsonAsync($"/api/v1/Auth/Login", dto);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Should().NotBeNull();
        
        ResponseHttp<ResponseTokens>? content = await response.Content.ReadFromJsonAsync<ResponseHttp<ResponseTokens>>();
        
        content.Should().NotBeNull();
        content.Data.Should().NotBeNull();
        content.Data.Token.Should().NotBeNull();
        content.Data.RefreshToken.Should().NotBeNull();
        
        return content;
    }

    public async Task<CommentPostUserDto> CreateComment(PostUserDto postUser, string? title = null, string? parentId = null)
    {
        string _url = "/api/v1/CommentPostUser";
        CreateCommentPostUserDto dto = new CreateCommentPostUserDto()
        {
            Content = title == null? string.Concat(Enumerable.Repeat("AnyContent", 30)) : string.Concat(Enumerable.Repeat(title, 30)) ,
            Depth = 5,
            PostId = postUser.Id,
            ImageUrl = "https://github.com/Andersoncrs5",
            IsActive = true
        };
        
        HttpResponseMessage message = await client.PostAsJsonAsync($"{_url}?parentId={parentId}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.Created);

        ResponseHttp<CommentPostUserDto>? content = await message.Content.ReadFromJsonAsync<ResponseHttp<CommentPostUserDto>>();
        content.Should().NotBeNull();
        content.Data.Should().NotBeNull();
        content.Code.Should().Be((int)HttpStatusCode.Created);
        content.Status.Should().BeTrue();
        
        content.Data.PostId.Should().Be(postUser.Id);
        content.Data.Id.Should().NotBeNullOrEmpty();
        
        return content.Data;
    }
    
    public async Task<UserResultTest> CreateAndGetUser()
    {
        int num = Random.Shared.Next(1, 100000000);
        CreateUserDto dto = new CreateUserDto();
        
        dto.Email = $"email{num}@gmail.com";
        dto.Username = $"username{num}";
        dto.PasswordHash = "45356675@3Afecv13$";
        dto.FullName = $"fullname{num}";
        
        HttpResponseMessage response = await client.PostAsJsonAsync("/api/v1/Auth", dto);
        
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        ResponseHttp<ResponseTokens>? content = await response.Content.ReadFromJsonAsync<ResponseHttp<ResponseTokens>>();

        content.Should().NotBeNull();
        content.Data.Should().NotBeNull();
        content.Data.Token.Should().NotBeNullOrWhiteSpace();
        
        string token = content.Data.Token;
        client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        HttpResponseMessage getResponse = await client.GetAsync($"/api/v1/Auth");
        
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        ResponseHttp<UserDto>? user = await getResponse.Content.ReadFromJsonAsync<ResponseHttp<UserDto>>();
        user.Should().NotBeNull();
        user.Data.Should().NotBeNull();
        user.Data.Id.Should().NotBeNullOrWhiteSpace();
        user.Data.Email.Should().Be(dto.Email);
        user.Data.Username.Should().Be(dto.Username);   
        user.Data.FullName.Should().Be(dto.FullName);

        return new UserResultTest
        {
            CreateUser = dto,
            Tokens = content.Data,
            User = user.Data
        };
    }

    public async Task<CategoryDto> CreateCategory(ResponseTokens master, bool isActive = true)
    {
        int num = Random.Shared.Next(1, 10000000);
        
        string token = master.Token!;
        client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        CreateCategoryDto dto = new CreateCategoryDto
        {
            IsActive = isActive,
            Name = $"Test{num}",
            Description = $"Test description {num}",
        };

        HttpResponseMessage message = await client.PostAsJsonAsync("api/v1/Category", dto);
        message.StatusCode.Should().Be(HttpStatusCode.Created);

        ResponseHttp<CategoryDto>? response = await message.Content.ReadFromJsonAsync<ResponseHttp<CategoryDto>>();
        
        Assert.NotNull(response);
        Assert.NotNull(response.Data);
        Assert.NotNull(response.Data.Id);
        
        return response.Data;
    }

    public async Task<EnterpriseDto> CreateEnterprise(UserResultTest user, IndustryDto industryDto)
    {
        string token = user.Tokens!.Token!;
        client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        CreateEnterpriseDto dto = new CreateEnterpriseDto
        {
            Description = "New description",
            Name = "New name" + Guid.NewGuid(),
            Type = EnterpriseTypeEnum.MediumBusiness,
        };

        HttpResponseMessage message = await client.PostAsJsonAsync("/api/v1/Enterprise", dto);
        message.StatusCode.Should().Be(HttpStatusCode.Created);
        
        ResponseHttp<EnterpriseDto>? response = await message.Content.ReadFromJsonAsync<ResponseHttp<EnterpriseDto>>();
        response.Should().NotBeNull();
        response.Code.Should().Be(201);
        response.Status.Should().BeTrue();
        
        response.Data.Should().NotBeNull();
        response.Data.Id.Should().NotBeEmpty();
        response.Data.Description.Should().Be(dto.Description);
        response.Data.Name.Should().Be(dto.Name);
        response.Data.Type.Should().Be(dto.Type);
        response.Data.UserId.Should().Be(user.User!.Id);
        
        return response.Data;
    }
    
    public async Task<IndustryDto> CreateIndustry(ResponseTokens master)
    {
        long num = Random.Shared.NextInt64(1, 10000000000000);
        
        string token = master.Token!;
        client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        CreateIndustryDto dto = new CreateIndustryDto
        {
            IsActive = true,
            Name = $"Test{num}",
            Description = $"Test description {num}",
        };

        HttpResponseMessage message = await client.PostAsJsonAsync("/api/v1/Industry", dto);
        message.StatusCode.Should().Be(HttpStatusCode.Created);

        ResponseHttp<IndustryDto>? response = await message.Content.ReadFromJsonAsync<ResponseHttp<IndustryDto>>();
        
        Assert.NotNull(response);
        Assert.NotNull(response.Data);
        Assert.NotNull(response.Data.Id);
        
        response.Data.Name.Should().Be(dto.Name);
        response.Data.Description.Should().Be(dto.Description);
        response.Data.IsActive.Should().Be(dto.IsActive);
        
        return response.Data;
    }
    
    public async Task<IndustryDto> CreateIndustryWithIsActive(ResponseTokens master, bool isActive)
    {
        long num = Random.Shared.NextInt64(1, 10000000000000);
        
        string token = master.Token!;
        client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        CreateIndustryDto dto = new CreateIndustryDto
        {
            IsActive = isActive,
            Name = $"Test{num}",
            Description = $"Test description {num}",
        };

        HttpResponseMessage message = await client.PostAsJsonAsync("/api/v1/Industry", dto);
        message.StatusCode.Should().Be(HttpStatusCode.Created);

        ResponseHttp<IndustryDto>? response = await message.Content.ReadFromJsonAsync<ResponseHttp<IndustryDto>>();
        
        Assert.NotNull(response);
        Assert.NotNull(response.Data);
        Assert.NotNull(response.Data.Id);
        
        response.Data.Name.Should().Be(dto.Name);
        response.Data.Description.Should().Be(dto.Description);
        response.Data.IsActive.Should().Be(dto.IsActive);
        
        return response.Data;
    }

    public async Task<string> CreateEnterpriseIndustry(string enterpriseId, string industryId, bool isPrimary = true)
    {
        CreateEnterpriseIndustryDto dto = new CreateEnterpriseIndustryDto
        {
            EnterpriseId = enterpriseId,
            IndustryId = industryId,
            IsPrimary = isPrimary
        };

        HttpResponseMessage message = await client.PostAsJsonAsync("/api/v1/EnterpriseIndustry", dto);
        message.StatusCode.Should().Be(HttpStatusCode.Created);

        ResponseHttp<string>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<string>>();
        
        http.Should().NotBeNull();
        http.Status.Should().BeTrue();
        http.Data.Should().NotBeNull();
        
        return http.Data;
    }

    public async Task<PostUserDto> CreatePostUser(CategoryDto categoryDto, string userId, int readingTimeMinutes = 5)
    {
        CreatePostUserDto dto = new CreatePostUserDto
        {
            CategoryId = categoryDto.Id,
            Content = string.Concat(Enumerable.Repeat("AnyContent", 30)),
            IsActive = true,
            ImageUrl = "https://github.com/Andersoncrs5",
            ReadingTimeMinutes = readingTimeMinutes,
            Title = "A Title simple to a post simple"
        };
        
        HttpResponseMessage message = await client.PostAsJsonAsync("/api/v1/PostUser", dto);
        message.StatusCode.Should().Be(HttpStatusCode.Created);
        ResponseHttp<PostUserDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<PostUserDto>>();
        http.Should().NotBeNull();
        http.Data.Should().NotBeNull();
        
        http.Data.Id.Should().NotBeEmpty();
        http.Data.Title.Should().Be(dto.Title);
        http.Data.Content.Should().Be(dto.Content);
        http.Data.CategoryId.Should().Be(dto.CategoryId);
        http.Data.IsActive.Should().Be(dto.IsActive);
        http.Data.ReadingTimeMinutes.Should().Be(dto.ReadingTimeMinutes);
        http.Data.ImageUrl.Should().Be(dto.ImageUrl);
        http.Data.UserId.Should().Be(userId);
        
        return http.Data;
    }

    public async Task<PostEnterpriseDto> CreatePostEnterprise(CategoryDto categoryDto, bool isActive = true)
    {
        CreatePostEnterpriseDto dto = new CreatePostEnterpriseDto
        {
            CategoryId = categoryDto.Id,
            Content = string.Concat(Enumerable.Repeat("AnyContent", 30)),
            IsActive = isActive,
            ImageUrl = "https://github.com/Andersoncrs5",
            ReadingTimeMinutes = 4,
            Title = "A Title simple to a post simple"
        };

        HttpResponseMessage message = await client.PostAsJsonAsync("/api/v1/PostEnterprise", dto);
        
        message.StatusCode.Should().Be(HttpStatusCode.Created);
        ResponseHttp<PostEnterpriseDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<PostEnterpriseDto>>();
        http.Should().NotBeNull();
        http.Data.Should().NotBeNull();
        
        http.Data.Id.Should().NotBeEmpty();
        http.Data.Title.Should().Be(dto.Title);
        http.Data.Content.Should().Be(dto.Content);
        http.Data.CategoryId.Should().Be(dto.CategoryId);
        http.Data.IsActive.Should().Be(dto.IsActive);
        http.Data.ReadingTimeMinutes.Should().Be(dto.ReadingTimeMinutes);
        http.Data.ImageUrl.Should().Be(dto.ImageUrl);
        
        return http.Data;
    }
    
}
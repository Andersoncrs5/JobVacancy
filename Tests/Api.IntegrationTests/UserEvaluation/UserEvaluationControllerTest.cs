using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using JobVacancy.API.IntegrationTests.Utils;
using JobVacancy.API.models.dtos.EmployeeEnterprise;
using JobVacancy.API.models.dtos.EmployeeInvitation;
using JobVacancy.API.models.dtos.Enterprise;
using JobVacancy.API.models.dtos.Industry;
using JobVacancy.API.models.dtos.Position;
using JobVacancy.API.models.dtos.UserEvaluation;
using JobVacancy.API.models.entities.Enums;
using JobVacancy.API.Utils.Res;
using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;

namespace JobVacancy.API.IntegrationTests.UserEvaluation;

public class UserEvaluationControllerTest: IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly Helper _helper;
    private readonly IConfiguration _configuration;
    private readonly string _url = "/api/v1/UserEvaluation";
    private readonly ITestOutputHelper _output;
    
    public UserEvaluationControllerTest(CustomWebApplicationFactory factory, ITestOutputHelper output) 
    {
        _client = factory.CreateClient(); 
        _helper = new Helper(_client); 
        _configuration = factory.Configuration;
        _output = output;
    }

    [Fact]
    public async Task Create() 
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);
        PositionDto positionDto = await _helper.CreatePositionAsync();

        UserResultTest userGuest = await _helper.CreateAndGetUser();
        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterpriseDto = await _helper.CreateEnterprise(user, industryDto);

        ResponseTokens loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token!);

        EmployeeInvitationDto invitation = await _helper.CreateEmployeeInvitation(userGuest, positionDto);
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userGuest.Tokens!.Token);

        EmployeeInvitationDto invitationUpdated = await _helper.UpdateInvitationByUser(StatusEnum.Accepted, invitation);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token);
        
        EmployeeEnterpriseDto employeeDto = await _helper.CreateEmployeeEnterprise(invitationUpdated);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userGuest.Tokens!.Token);
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token);

        CreateUserEvaluationDto dto = new CreateUserEvaluationDto()
        {
            TargetUserId = userGuest.User!.Id,
            Content = string.Concat(Enumerable.Repeat("Any", 30)),
            EmploymentStatus = employeeDto.EmploymentType,
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

        HttpResponseMessage message = await _client.PostAsJsonAsync(_url, dto);
        
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
        http.Data.EnterpriseId.Should().Be(enterpriseDto.Id);
        http.Data.TargetUserId.Should().Be(dto.TargetUserId);
        http.Data.ReviewerUserId.Should().Be(user.User.Id);
    }
    
    [Fact]
    public async Task CreateReturnConflictBecauseAlreadyExistsReview() 
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);
        PositionDto positionDto = await _helper.CreatePositionAsync();

        UserResultTest userGuest = await _helper.CreateAndGetUser();
        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterpriseDto = await _helper.CreateEnterprise(user, industryDto);

        ResponseTokens loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token!);

        EmployeeInvitationDto invitation = await _helper.CreateEmployeeInvitation(userGuest, positionDto);
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userGuest.Tokens!.Token);

        EmployeeInvitationDto invitationUpdated = await _helper.UpdateInvitationByUser(StatusEnum.Accepted, invitation);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token);
        
        EmployeeEnterpriseDto employeeDto = await _helper.CreateEmployeeEnterprise(invitationUpdated);
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token);

        await _helper.CreateUserEvaluation(userGuest, positionDto);
        
        CreateUserEvaluationDto dto = new CreateUserEvaluationDto()
        {
            TargetUserId = userGuest.User!.Id,
            Content = string.Concat(Enumerable.Repeat("Any", 30)),
            EmploymentStatus = employeeDto.EmploymentType,
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

        HttpResponseMessage message = await _client.PostAsJsonAsync(_url, dto);
        
        message.StatusCode.Should().Be(HttpStatusCode.Conflict);

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int)HttpStatusCode.Conflict);
        http.Message.Should().NotBeNullOrWhiteSpace();
        
        http.Data.Should().BeNull();
        
    }
    
    [Fact]
    public async Task CreateReturnForbBecauseNotExistsEmployee() 
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);
        PositionDto positionDto = await _helper.CreatePositionAsync();

        UserResultTest userGuest = await _helper.CreateAndGetUser();
        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterpriseDto = await _helper.CreateEnterprise(user, industryDto);

        ResponseTokens loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token);

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

        HttpResponseMessage message = await _client.PostAsJsonAsync(_url, dto);
        
        message.StatusCode.Should().Be(HttpStatusCode.Forbidden);

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int)HttpStatusCode.Forbidden);
        http.Message.Should().NotBeNullOrWhiteSpace();
        
        http.Data.Should().BeNull();
    }

    [Fact]
    public async Task Get()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);
        PositionDto positionDto = await _helper.CreatePositionAsync();

        UserResultTest userGuest = await _helper.CreateAndGetUser();
        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterpriseDto = await _helper.CreateEnterprise(user, industryDto);

        ResponseTokens loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token!);

        EmployeeInvitationDto invitation = await _helper.CreateEmployeeInvitation(userGuest, positionDto);
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userGuest.Tokens!.Token);

        EmployeeInvitationDto invitationUpdated = await _helper.UpdateInvitationByUser(StatusEnum.Accepted, invitation);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token);
        
        EmployeeEnterpriseDto employeeDto = await _helper.CreateEmployeeEnterprise(invitationUpdated);
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token);

        UserEvaluationDto evaluationDto = await _helper.CreateUserEvaluation(userGuest, positionDto);

        HttpResponseMessage message = await _client.GetAsync($"{_url}/{evaluationDto.Id}");
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<UserEvaluationDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<UserEvaluationDto>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int)HttpStatusCode.OK);
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Data.Should().NotBeNull();
        
        http.Data.Id.Should().Be(evaluationDto.Id);
    }
    
    [Fact]
    public async Task GetNotFound()
    {
        UserResultTest user = await _helper.CreateAndGetUser();
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Tokens!.Token);
        
        HttpResponseMessage message = await _client.GetAsync($"{_url}/{Guid.NewGuid()}");
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int)HttpStatusCode.NotFound);
        http.Message.Should().NotBeNullOrWhiteSpace();

        http.Data.Should().BeNull();
    }
    
    [Fact]
    public async Task Del() 
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);
        PositionDto positionDto = await _helper.CreatePositionAsync();

        UserResultTest userGuest = await _helper.CreateAndGetUser();
        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterpriseDto = await _helper.CreateEnterprise(user, industryDto);

        ResponseTokens loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token!);

        EmployeeInvitationDto invitation = await _helper.CreateEmployeeInvitation(userGuest, positionDto);
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userGuest.Tokens!.Token);

        EmployeeInvitationDto invitationUpdated = await _helper.UpdateInvitationByUser(StatusEnum.Accepted, invitation);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token);
        
        EmployeeEnterpriseDto employeeDto = await _helper.CreateEmployeeEnterprise(invitationUpdated);
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token);

        UserEvaluationDto evaluationDto = await _helper.CreateUserEvaluation(userGuest, positionDto);

        HttpResponseMessage message = await _client.DeleteAsync($"{_url}/{evaluationDto.Id}");
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int)HttpStatusCode.OK);
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Status.Should().BeTrue();
        
        http.Data.Should().BeNull();
    }
    
    [Fact]
    public async Task DelNotFound() 
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        UserResultTest userGuest = await _helper.CreateAndGetUser();
        UserResultTest user = await _helper.CreateAndGetUser();
        await _helper.CreateEnterprise(user, industryDto);

        ResponseTokens loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token);

        HttpResponseMessage message = await _client.DeleteAsync($"{_url}/{Guid.NewGuid()}");
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int)HttpStatusCode.NotFound);
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Status.Should().BeFalse();
        
        http.Data.Should().BeNull();
    }
    
    
    
}
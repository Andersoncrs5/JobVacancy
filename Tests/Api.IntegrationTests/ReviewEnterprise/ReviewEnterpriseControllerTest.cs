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
using JobVacancy.API.models.dtos.ReviewEnterprise;
using JobVacancy.API.models.entities.Enums;
using JobVacancy.API.Utils.Page;
using JobVacancy.API.Utils.Res;
using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;

namespace JobVacancy.API.IntegrationTests.ReviewEnterprise;

public class ReviewEnterpriseControllerTest: IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly Helper _helper;
    private readonly IConfiguration _configuration;
    private readonly string _url = "/api/v1/ReviewEnterprise";
    private readonly ITestOutputHelper _output;
    
    public ReviewEnterpriseControllerTest(CustomWebApplicationFactory factory, ITestOutputHelper output) 
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
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);

        ResponseTokens loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token!);

        EmployeeInvitationDto invitation = await _helper.CreateEmployeeInvitation(userGuest, positionDto);
        
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", userGuest.Tokens!.Token);

        EmployeeInvitationDto invitationUpdated = await _helper.UpdateInvitationByUser(StatusEnum.Accepted, invitation);

        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token);
        
        EmployeeEnterpriseDto employeeDto = await _helper.CreateEmployeeEnterprise(invitationUpdated);

        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", userGuest.Tokens!.Token);

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

        HttpResponseMessage message = await _client.PostAsJsonAsync($"{_url}/{enterprise.Id}", reviewDto);
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
    }
    
    [Fact]
    public async Task CreateReturnAlreadyExists()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);
        PositionDto positionDto = await _helper.CreatePositionAsync();

        UserResultTest userGuest = await _helper.CreateAndGetUser();
        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);

        ResponseTokens loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token!);

        EmployeeInvitationDto invitation = await _helper.CreateEmployeeInvitation(userGuest, positionDto);
        
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", userGuest.Tokens!.Token);

        EmployeeInvitationDto invitationUpdated = await _helper.UpdateInvitationByUser(StatusEnum.Accepted, invitation);

        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token);
        
        EmployeeEnterpriseDto employeeDto = await _helper.CreateEmployeeEnterprise(invitationUpdated);

        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", userGuest.Tokens!.Token);

        ReviewEnterpriseDto reviewCreated = await _helper.CreateReviewEnterprise(invitation);

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

        HttpResponseMessage message = await _client.PostAsJsonAsync($"{_url}/{reviewCreated.EnterpriseId}", reviewDto);
        message.StatusCode.Should().Be(HttpStatusCode.Conflict);

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Code.Should().Be((int)HttpStatusCode.Conflict);
        http.Status.Should().BeFalse();
        http.Version.Should().Be(1);
        
        http.Data.Should().BeNull();
    }
    
    [Fact]
    public async Task CreateNotFoundEnterprise()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);
        PositionDto positionDto = await _helper.CreatePositionAsync();

        UserResultTest userGuest = await _helper.CreateAndGetUser();
        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);

        ResponseTokens loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token!);

        EmployeeInvitationDto invitation = await _helper.CreateEmployeeInvitation(userGuest, positionDto);
        
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", userGuest.Tokens!.Token);

        EmployeeInvitationDto invitationUpdated = await _helper.UpdateInvitationByUser(StatusEnum.Accepted, invitation);

        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token);
        
        EmployeeEnterpriseDto employeeDto = await _helper.CreateEmployeeEnterprise(invitationUpdated);

        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", userGuest.Tokens!.Token);

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

        HttpResponseMessage message = await _client.PostAsJsonAsync($"{_url}/{Guid.NewGuid()}", reviewDto);
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Code.Should().Be((int)HttpStatusCode.NotFound);
        http.Status.Should().BeFalse();
        http.Version.Should().Be(1);
        
        http.Data.Should().BeNull();
    }
    
    [Fact]
    public async Task CreateNotFoundEmployeeEnterprise()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);
        PositionDto positionDto = await _helper.CreatePositionAsync();

        UserResultTest userGuest = await _helper.CreateAndGetUser();
        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);

        ResponseTokens loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token!);

        EmployeeInvitationDto invitation = await _helper.CreateEmployeeInvitation(userGuest, positionDto);
        
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", userGuest.Tokens!.Token);

        EmployeeInvitationDto invitationUpdated = await _helper.UpdateInvitationByUser(StatusEnum.Accepted, invitation);

        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token);
        
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", userGuest.Tokens!.Token);

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

        HttpResponseMessage message = await _client.PostAsJsonAsync($"{_url}/{enterprise.Id}", reviewDto);
        message.StatusCode.Should().Be(HttpStatusCode.Conflict);

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Code.Should().Be((int)HttpStatusCode.Conflict);
        http.Status.Should().BeFalse();
        http.Version.Should().Be(1);
        
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
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);

        ResponseTokens loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token!);

        EmployeeInvitationDto invitation = await _helper.CreateEmployeeInvitation(userGuest, positionDto);
        
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", userGuest.Tokens!.Token);

        EmployeeInvitationDto invitationUpdated = await _helper.UpdateInvitationByUser(StatusEnum.Accepted, invitation);

        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token);
        
        EmployeeEnterpriseDto employeeDto = await _helper.CreateEmployeeEnterprise(invitationUpdated);

        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", userGuest.Tokens!.Token);

        ReviewEnterpriseDto reviewDto = await _helper.CreateReviewEnterprise(invitation);

        HttpResponseMessage message = await _client.GetAsync($"{_url}/{reviewDto.Id}");
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<ReviewEnterpriseDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<ReviewEnterpriseDto>>();
        http.Should().NotBeNull();
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Code.Should().Be((int)HttpStatusCode.OK);
        http.Status.Should().BeTrue();
        http.Version.Should().Be(1);
        
        http.Data.Should().NotBeNull();
        http.Data.Id.Should().Be(reviewDto.Id);
    }
    
    [Fact]
    public async Task GetNotFound()
    {
        UserResultTest userGuest = await _helper.CreateAndGetUser();
        
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", userGuest.Tokens!.Token);
        
        HttpResponseMessage message = await _client.GetAsync($"{_url}/{Guid.NewGuid()}");
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Code.Should().Be((int)HttpStatusCode.NotFound);
        http.Status.Should().BeFalse();
        http.Version.Should().Be(1);
        
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
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);

        ResponseTokens loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token!);

        EmployeeInvitationDto invitation = await _helper.CreateEmployeeInvitation(userGuest, positionDto);
        
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", userGuest.Tokens!.Token);

        EmployeeInvitationDto invitationUpdated = await _helper.UpdateInvitationByUser(StatusEnum.Accepted, invitation);

        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token);
        
        EmployeeEnterpriseDto employeeDto = await _helper.CreateEmployeeEnterprise(invitationUpdated);

        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", userGuest.Tokens!.Token);

        ReviewEnterpriseDto reviewDto = await _helper.CreateReviewEnterprise(invitation);

        HttpResponseMessage message = await _client.DeleteAsync($"{_url}/{reviewDto.Id}");
        message.StatusCode.Should().Be(HttpStatusCode.NoContent);

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Code.Should().Be((int)HttpStatusCode.NoContent);
        http.Status.Should().BeTrue();
        http.Version.Should().Be(1);
        
        http.Data.Should().BeNull();
    }
    
    [Fact]
    public async Task DelNotFound()
    {
        
        UserResultTest userGuest = await _helper.CreateAndGetUser();

        HttpResponseMessage message = await _client.DeleteAsync($"{_url}/{Guid.NewGuid()}");
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Code.Should().Be((int)HttpStatusCode.NotFound);
        http.Status.Should().BeFalse();
        http.Version.Should().Be(1);
        
        http.Data.Should().BeNull();
    }
    
    [Fact]
    public async Task PatchFullFields()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);
        PositionDto positionDto = await _helper.CreatePositionAsync();

        UserResultTest userGuest = await _helper.CreateAndGetUser();
        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);

        ResponseTokens loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token!);

        EmployeeInvitationDto invitation = await _helper.CreateEmployeeInvitation(userGuest, positionDto);
        
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", userGuest.Tokens!.Token);

        EmployeeInvitationDto invitationUpdated = await _helper.UpdateInvitationByUser(StatusEnum.Accepted, invitation);

        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token);
        
        EmployeeEnterpriseDto employeeDto = await _helper.CreateEmployeeEnterprise(invitationUpdated);

        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", userGuest.Tokens!.Token);

        ReviewEnterpriseDto reviewDto = await _helper.CreateReviewEnterprise(invitation);

        UpdateReviewEnterpriseDto dto = new UpdateReviewEnterpriseDto()
        {
            Content = string.Concat(Enumerable.Repeat("Update", 25)),
            IsAnonymous = false,
            RatingCompensation = 3,
            RatingCulture = 3,
            RatingManagement = 3,
            RatingOverall = 3,
            RatingWorkLifeBalance = 3,
            Title = string.Concat(Enumerable.Repeat("TitleUpdate", 15)),
        };

        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_url}/{reviewDto.Id}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<ReviewEnterpriseDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<ReviewEnterpriseDto>>();
        http.Should().NotBeNull();
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Code.Should().Be((int)HttpStatusCode.OK);
        http.Status.Should().BeTrue();
        http.Version.Should().Be(1);
        
        http.Data.Should().NotBeNull();
        http.Data.Id.Should().Be(reviewDto.Id);
        http.Data.Content.Should().Be(dto.Content);
        http.Data.IsAnonymous.Should().Be(dto.IsAnonymous.Value);
        http.Data.RatingCompensation.Should().Be(dto.RatingCompensation);
        http.Data.RatingCulture.Should().Be(dto.RatingCulture);
        http.Data.RatingManagement.Should().Be(dto.RatingManagement);
        http.Data.RatingOverall.Should().Be(dto.RatingOverall);
        http.Data.RatingWorkLifeBalance.Should().Be(dto.RatingWorkLifeBalance);
        http.Data.Title.Should().Be(dto.Title);
    }
    
    [Fact]
    public async Task PatchNotFound()
    {
        UserResultTest userGuest = await _helper.CreateAndGetUser();
        
        UpdateReviewEnterpriseDto dto = new UpdateReviewEnterpriseDto();

        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_url}/{Guid.NewGuid()}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Code.Should().Be((int)HttpStatusCode.NotFound);
        http.Status.Should().BeFalse();
        http.Version.Should().Be(1);
        
        http.Data.Should().BeNull();
    }
    
    [Fact]
    public async Task PatchJustTitle()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);
        PositionDto positionDto = await _helper.CreatePositionAsync();

        UserResultTest userGuest = await _helper.CreateAndGetUser();
        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);

        ResponseTokens loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token!);

        EmployeeInvitationDto invitation = await _helper.CreateEmployeeInvitation(userGuest, positionDto);
        
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", userGuest.Tokens!.Token);

        EmployeeInvitationDto invitationUpdated = await _helper.UpdateInvitationByUser(StatusEnum.Accepted, invitation);

        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token);
        
        EmployeeEnterpriseDto employeeDto = await _helper.CreateEmployeeEnterprise(invitationUpdated);

        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", userGuest.Tokens!.Token);

        ReviewEnterpriseDto reviewDto = await _helper.CreateReviewEnterprise(invitation);

        UpdateReviewEnterpriseDto dto = new UpdateReviewEnterpriseDto()
        {
            Title = string.Concat(Enumerable.Repeat(Guid.NewGuid(), 5)),
        };

        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_url}/{reviewDto.Id}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<ReviewEnterpriseDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<ReviewEnterpriseDto>>();
        http.Should().NotBeNull();
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Code.Should().Be((int)HttpStatusCode.OK);
        http.Status.Should().BeTrue();
        http.Version.Should().Be(1);
        
        http.Data.Should().NotBeNull();
        http.Data.Id.Should().Be(reviewDto.Id);
        http.Data.Content.Should().Be(reviewDto.Content);
        http.Data.IsAnonymous.Should().Be(reviewDto.IsAnonymous);
        http.Data.RatingCompensation.Should().Be(reviewDto.RatingCompensation);
        http.Data.RatingCulture.Should().Be(reviewDto.RatingCulture);
        http.Data.RatingManagement.Should().Be(reviewDto.RatingManagement);
        http.Data.RatingOverall.Should().Be(reviewDto.RatingOverall);
        http.Data.RatingWorkLifeBalance.Should().Be(reviewDto.RatingWorkLifeBalance);
        http.Data.Title.Should().Be(dto.Title);
    }
  
    [Fact]
    public async Task PatchJustRatingWorkLifeBalance()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);
        PositionDto positionDto = await _helper.CreatePositionAsync();

        UserResultTest userGuest = await _helper.CreateAndGetUser();
        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);

        ResponseTokens loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token!);

        EmployeeInvitationDto invitation = await _helper.CreateEmployeeInvitation(userGuest, positionDto);
        
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", userGuest.Tokens!.Token);

        EmployeeInvitationDto invitationUpdated = await _helper.UpdateInvitationByUser(StatusEnum.Accepted, invitation);

        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token);
        
        EmployeeEnterpriseDto employeeDto = await _helper.CreateEmployeeEnterprise(invitationUpdated);

        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", userGuest.Tokens!.Token);

        ReviewEnterpriseDto reviewDto = await _helper.CreateReviewEnterprise(invitation);

        UpdateReviewEnterpriseDto dto = new UpdateReviewEnterpriseDto()
        {
            RatingWorkLifeBalance = 3,
        };

        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_url}/{reviewDto.Id}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<ReviewEnterpriseDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<ReviewEnterpriseDto>>();
        http.Should().NotBeNull();
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Code.Should().Be((int)HttpStatusCode.OK);
        http.Status.Should().BeTrue();
        http.Version.Should().Be(1);
        
        http.Data.Should().NotBeNull();
        http.Data.Id.Should().Be(reviewDto.Id);
        http.Data.Content.Should().Be(reviewDto.Content);
        http.Data.IsAnonymous.Should().Be(reviewDto.IsAnonymous);
        http.Data.RatingCompensation.Should().Be(reviewDto.RatingCompensation);
        http.Data.RatingCulture.Should().Be(reviewDto.RatingCulture);
        http.Data.RatingManagement.Should().Be(reviewDto.RatingManagement);
        http.Data.RatingOverall.Should().Be(reviewDto.RatingOverall);
        http.Data.RatingWorkLifeBalance.Should().Be(dto.RatingWorkLifeBalance);
        http.Data.Title.Should().Be(reviewDto.Title);
    }
  
    [Fact]
    public async Task PatchJustRatingOverall()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);
        PositionDto positionDto = await _helper.CreatePositionAsync();

        UserResultTest userGuest = await _helper.CreateAndGetUser();
        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);

        ResponseTokens loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token!);

        EmployeeInvitationDto invitation = await _helper.CreateEmployeeInvitation(userGuest, positionDto);
        
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", userGuest.Tokens!.Token);

        EmployeeInvitationDto invitationUpdated = await _helper.UpdateInvitationByUser(StatusEnum.Accepted, invitation);

        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token);
        
        EmployeeEnterpriseDto employeeDto = await _helper.CreateEmployeeEnterprise(invitationUpdated);

        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", userGuest.Tokens!.Token);

        ReviewEnterpriseDto reviewDto = await _helper.CreateReviewEnterprise(invitation);

        UpdateReviewEnterpriseDto dto = new UpdateReviewEnterpriseDto()
        {
            RatingOverall = 3,
        };

        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_url}/{reviewDto.Id}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<ReviewEnterpriseDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<ReviewEnterpriseDto>>();
        http.Should().NotBeNull();
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Code.Should().Be((int)HttpStatusCode.OK);
        http.Status.Should().BeTrue();
        http.Version.Should().Be(1);
        
        http.Data.Should().NotBeNull();
        http.Data.Id.Should().Be(reviewDto.Id);
        http.Data.Content.Should().Be(reviewDto.Content);
        http.Data.IsAnonymous.Should().Be(reviewDto.IsAnonymous);
        http.Data.RatingCompensation.Should().Be(reviewDto.RatingCompensation);
        http.Data.RatingCulture.Should().Be(reviewDto.RatingCulture);
        http.Data.RatingManagement.Should().Be(reviewDto.RatingManagement);
        http.Data.RatingOverall.Should().Be(dto.RatingOverall);
        http.Data.RatingWorkLifeBalance.Should().Be(reviewDto.RatingWorkLifeBalance);
        http.Data.Title.Should().Be(reviewDto.Title);
    }
  
    [Fact]
    public async Task PatchJustRatingManagement()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);
        PositionDto positionDto = await _helper.CreatePositionAsync();

        UserResultTest userGuest = await _helper.CreateAndGetUser();
        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);

        ResponseTokens loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token!);

        EmployeeInvitationDto invitation = await _helper.CreateEmployeeInvitation(userGuest, positionDto);
        
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", userGuest.Tokens!.Token);

        EmployeeInvitationDto invitationUpdated = await _helper.UpdateInvitationByUser(StatusEnum.Accepted, invitation);

        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token);
        
        EmployeeEnterpriseDto employeeDto = await _helper.CreateEmployeeEnterprise(invitationUpdated);

        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", userGuest.Tokens!.Token);

        ReviewEnterpriseDto reviewDto = await _helper.CreateReviewEnterprise(invitation);

        UpdateReviewEnterpriseDto dto = new UpdateReviewEnterpriseDto()
        {
            RatingManagement = 3,
        };

        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_url}/{reviewDto.Id}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<ReviewEnterpriseDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<ReviewEnterpriseDto>>();
        http.Should().NotBeNull();
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Code.Should().Be((int)HttpStatusCode.OK);
        http.Status.Should().BeTrue();
        http.Version.Should().Be(1);
        
        http.Data.Should().NotBeNull();
        http.Data.Id.Should().Be(reviewDto.Id);
        http.Data.Content.Should().Be(reviewDto.Content);
        http.Data.IsAnonymous.Should().Be(reviewDto.IsAnonymous);
        http.Data.RatingCompensation.Should().Be(reviewDto.RatingCompensation);
        http.Data.RatingCulture.Should().Be(reviewDto.RatingCulture);
        http.Data.RatingManagement.Should().Be(dto.RatingManagement);
        http.Data.RatingOverall.Should().Be(reviewDto.RatingOverall);
        http.Data.RatingWorkLifeBalance.Should().Be(reviewDto.RatingWorkLifeBalance);
        http.Data.Title.Should().Be(reviewDto.Title);
    }
    
    [Fact]
    public async Task PatchJustRatingCulture()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);
        PositionDto positionDto = await _helper.CreatePositionAsync();

        UserResultTest userGuest = await _helper.CreateAndGetUser();
        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);

        ResponseTokens loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token!);

        EmployeeInvitationDto invitation = await _helper.CreateEmployeeInvitation(userGuest, positionDto);
        
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", userGuest.Tokens!.Token);

        EmployeeInvitationDto invitationUpdated = await _helper.UpdateInvitationByUser(StatusEnum.Accepted, invitation);

        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token);
        
        EmployeeEnterpriseDto employeeDto = await _helper.CreateEmployeeEnterprise(invitationUpdated);

        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", userGuest.Tokens!.Token);

        ReviewEnterpriseDto reviewDto = await _helper.CreateReviewEnterprise(invitation);

        UpdateReviewEnterpriseDto dto = new UpdateReviewEnterpriseDto()
        {
            RatingCulture = 3,
        };

        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_url}/{reviewDto.Id}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<ReviewEnterpriseDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<ReviewEnterpriseDto>>();
        http.Should().NotBeNull();
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Code.Should().Be((int)HttpStatusCode.OK);
        http.Status.Should().BeTrue();
        http.Version.Should().Be(1);
        
        http.Data.Should().NotBeNull();
        http.Data.Id.Should().Be(reviewDto.Id);
        http.Data.Content.Should().Be(reviewDto.Content);
        http.Data.IsAnonymous.Should().Be(reviewDto.IsAnonymous);
        http.Data.RatingCompensation.Should().Be(reviewDto.RatingCompensation);
        http.Data.RatingCulture.Should().Be(dto.RatingCulture);
        http.Data.RatingManagement.Should().Be(reviewDto.RatingManagement);
        http.Data.RatingOverall.Should().Be(reviewDto.RatingOverall);
        http.Data.RatingWorkLifeBalance.Should().Be(reviewDto.RatingWorkLifeBalance);
        http.Data.Title.Should().Be(reviewDto.Title);
    }
    
    [Fact]
    public async Task PatchJustRatingCompensation()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);
        PositionDto positionDto = await _helper.CreatePositionAsync();

        UserResultTest userGuest = await _helper.CreateAndGetUser();
        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);

        ResponseTokens loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token!);

        EmployeeInvitationDto invitation = await _helper.CreateEmployeeInvitation(userGuest, positionDto);
        
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", userGuest.Tokens!.Token);

        EmployeeInvitationDto invitationUpdated = await _helper.UpdateInvitationByUser(StatusEnum.Accepted, invitation);

        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token);
        
        EmployeeEnterpriseDto employeeDto = await _helper.CreateEmployeeEnterprise(invitationUpdated);

        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", userGuest.Tokens!.Token);

        ReviewEnterpriseDto reviewDto = await _helper.CreateReviewEnterprise(invitation);

        UpdateReviewEnterpriseDto dto = new UpdateReviewEnterpriseDto()
        {
            RatingCompensation = 3,
        };

        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_url}/{reviewDto.Id}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<ReviewEnterpriseDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<ReviewEnterpriseDto>>();
        http.Should().NotBeNull();
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Code.Should().Be((int)HttpStatusCode.OK);
        http.Status.Should().BeTrue();
        http.Version.Should().Be(1);
        
        http.Data.Should().NotBeNull();
        http.Data.Id.Should().Be(reviewDto.Id);
        http.Data.Content.Should().Be(reviewDto.Content);
        http.Data.IsAnonymous.Should().Be(reviewDto.IsAnonymous);
        http.Data.RatingCompensation.Should().Be(dto.RatingCompensation);
        http.Data.RatingCulture.Should().Be(reviewDto.RatingCulture);
        http.Data.RatingManagement.Should().Be(reviewDto.RatingManagement);
        http.Data.RatingOverall.Should().Be(reviewDto.RatingOverall);
        http.Data.RatingWorkLifeBalance.Should().Be(reviewDto.RatingWorkLifeBalance);
        http.Data.Title.Should().Be(reviewDto.Title);
    }
    
    [Fact]
    public async Task PatchJustIsAnonymous()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);
        PositionDto positionDto = await _helper.CreatePositionAsync();

        UserResultTest userGuest = await _helper.CreateAndGetUser();
        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);

        ResponseTokens loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token!);

        EmployeeInvitationDto invitation = await _helper.CreateEmployeeInvitation(userGuest, positionDto);
        
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", userGuest.Tokens!.Token);

        EmployeeInvitationDto invitationUpdated = await _helper.UpdateInvitationByUser(StatusEnum.Accepted, invitation);

        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token);
        
        EmployeeEnterpriseDto employeeDto = await _helper.CreateEmployeeEnterprise(invitationUpdated);

        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", userGuest.Tokens!.Token);

        ReviewEnterpriseDto reviewDto = await _helper.CreateReviewEnterprise(invitation);

        UpdateReviewEnterpriseDto dto = new UpdateReviewEnterpriseDto()
        {
            IsAnonymous = false,
        };

        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_url}/{reviewDto.Id}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<ReviewEnterpriseDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<ReviewEnterpriseDto>>();
        http.Should().NotBeNull();
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Code.Should().Be((int)HttpStatusCode.OK);
        http.Status.Should().BeTrue();
        http.Version.Should().Be(1);
        
        http.Data.Should().NotBeNull();
        http.Data.Id.Should().Be(reviewDto.Id);
        http.Data.Content.Should().Be(reviewDto.Content);
        http.Data.IsAnonymous.Should().Be(dto.IsAnonymous.Value);
        http.Data.RatingCompensation.Should().Be(reviewDto.RatingCompensation);
        http.Data.RatingCulture.Should().Be(reviewDto.RatingCulture);
        http.Data.RatingManagement.Should().Be(reviewDto.RatingManagement);
        http.Data.RatingOverall.Should().Be(reviewDto.RatingOverall);
        http.Data.RatingWorkLifeBalance.Should().Be(reviewDto.RatingWorkLifeBalance);
        http.Data.Title.Should().Be(reviewDto.Title);
    }
    
    [Fact]
    public async Task PatchJustContent()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);
        PositionDto positionDto = await _helper.CreatePositionAsync();

        UserResultTest userGuest = await _helper.CreateAndGetUser();
        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);

        ResponseTokens loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token!);

        EmployeeInvitationDto invitation = await _helper.CreateEmployeeInvitation(userGuest, positionDto);
        
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", userGuest.Tokens!.Token);

        EmployeeInvitationDto invitationUpdated = await _helper.UpdateInvitationByUser(StatusEnum.Accepted, invitation);

        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token);
        
        EmployeeEnterpriseDto employeeDto = await _helper.CreateEmployeeEnterprise(invitationUpdated);

        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", userGuest.Tokens!.Token);

        ReviewEnterpriseDto reviewDto = await _helper.CreateReviewEnterprise(invitation);

        UpdateReviewEnterpriseDto dto = new UpdateReviewEnterpriseDto()
        {
            Content = string.Concat(Enumerable.Repeat("Update", 25)),
        };

        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_url}/{reviewDto.Id}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<ReviewEnterpriseDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<ReviewEnterpriseDto>>();
        http.Should().NotBeNull();
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Code.Should().Be((int)HttpStatusCode.OK);
        http.Status.Should().BeTrue();
        http.Version.Should().Be(1);
        
        http.Data.Should().NotBeNull();
        http.Data.Id.Should().Be(reviewDto.Id);
        http.Data.Content.Should().Be(dto.Content);
        http.Data.IsAnonymous.Should().Be(reviewDto.IsAnonymous);
        http.Data.RatingCompensation.Should().Be(reviewDto.RatingCompensation);
        http.Data.RatingCulture.Should().Be(reviewDto.RatingCulture);
        http.Data.RatingManagement.Should().Be(reviewDto.RatingManagement);
        http.Data.RatingOverall.Should().Be(reviewDto.RatingOverall);
        http.Data.RatingWorkLifeBalance.Should().Be(reviewDto.RatingWorkLifeBalance);
        http.Data.Title.Should().Be(reviewDto.Title);
    }

    [Fact]
    public async Task GetAll()
    {
        UserResultTest userGuest = await _helper.CreateAndGetUser();
        
        HttpResponseMessage message = await _client.GetAsync($"{_url}");
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        
        Page<ReviewEnterpriseDto>? http = await message.Content.ReadFromJsonAsync<Page<ReviewEnterpriseDto>>();
        http.Should().NotBeNull();
        http.PageIndex.Should().Be(1);
        http.PageSize.Should().Be(10);
    }
    
}
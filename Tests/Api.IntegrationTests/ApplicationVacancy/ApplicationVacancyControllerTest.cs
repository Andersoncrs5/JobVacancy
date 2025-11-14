using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using JobVacancy.API.IntegrationTests.Utils;
using JobVacancy.API.models.dtos.ApplicationVacancy;
using JobVacancy.API.models.dtos.Area;
using JobVacancy.API.models.dtos.Enterprise;
using JobVacancy.API.models.dtos.Industry;
using JobVacancy.API.models.dtos.Vacancy;
using JobVacancy.API.models.entities.Enums;
using JobVacancy.API.Utils.Page;
using JobVacancy.API.Utils.Res;
using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;

namespace JobVacancy.API.IntegrationTests.ApplicationVacancy;

public class ApplicationVacancyControllerTest: IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly Helper _helper;
    private readonly IConfiguration _configuration;
    private readonly string _URL = "/api/v1/ApplicationVacancy";
    private readonly ITestOutputHelper _output;
    
    public ApplicationVacancyControllerTest(CustomWebApplicationFactory factory, ITestOutputHelper output) 
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
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", master.Token!);
        
        IndustryDto industryDto = await _helper.CreateIndustry(master);
        AreaDto areaDto = await _helper.CreateArea();

        UserResultTest user = await _helper.CreateAndGetUser();
        UserResultTest candidate = await _helper.CreateAndGetUser();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Tokens!.Token!);
        
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        var loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token!);
        VacancyDto vacancyDto = await _helper.CreateVacancy(areaDto);
        VacancyDto vacancy = await _helper.ChangeStatusVacancy(vacancyDto.Id, VacancyStatusEnum.Open);

        CreateApplicationVacancyDto dto = new CreateApplicationVacancyDto
        {
            VacancyId = vacancy.Id,
            CoverLetter = string.Concat(Enumerable.Repeat("AnyMessage", 20)),
        };

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", candidate.Tokens!.Token!);
        
        HttpResponseMessage message = await _client.PostAsJsonAsync($"{_URL}", dto);
        
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
    }
    
    [Fact]
    public async Task Update() 
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", master.Token!);
        
        IndustryDto industryDto = await _helper.CreateIndustry(master);
        AreaDto areaDto = await _helper.CreateArea();

        UserResultTest user = await _helper.CreateAndGetUser();
        UserResultTest candidate = await _helper.CreateAndGetUser();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Tokens!.Token!);
        
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        var loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token!);
        VacancyDto vacancyDto = await _helper.CreateVacancy(areaDto);
        VacancyDto vacancy = await _helper.ChangeStatusVacancy(vacancyDto.Id, VacancyStatusEnum.Open);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", candidate.Tokens!.Token!);
        ApplicationVacancyDto application = await _helper.CreateApplication(vacancy, candidate);

        UpdateApplicationVacancyDto dto = new UpdateApplicationVacancyDto()
        {
            Status = ApplicationStatusEnum.UnderReview,
            IsViewedByRecruiter = true
        };

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token!);
        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_URL}/{application.Id}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<ApplicationVacancyDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<ApplicationVacancyDto>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int)HttpStatusCode.OK);
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Status.Should().BeTrue();
        
        http.Data.Should().NotBeNull();
        http.Data.Id.Should().Be(application.Id);
        http.Data.VacancyId.Should().Be(application.VacancyId);
        http.Data.IsViewedByRecruiter.Should().Be(dto.IsViewedByRecruiter);
        http.Data.Status.Should().Be(dto.Status);
        http.Data.Score.Should().Be(application.Score);
        http.Data.CoverLetter.Should().Be(application.CoverLetter);
    }
    
    [Fact]
    public async Task UpdateNotFound() 
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", master.Token!);
        
        IndustryDto industryDto = await _helper.CreateIndustry(master);
        
        UserResultTest user = await _helper.CreateAndGetUser();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Tokens!.Token!);
        
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        var loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        UpdateApplicationVacancyDto dto = new UpdateApplicationVacancyDto()
        {
            Status = ApplicationStatusEnum.UnderReview,
            IsViewedByRecruiter = true
        };

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token!);
        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_URL}/{Guid.NewGuid()}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int)HttpStatusCode.NotFound);
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Status.Should().BeFalse();
        
        http.Data.Should().BeNull();
    }
    
    [Fact]
    public async Task Get() 
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", master.Token!);
        
        IndustryDto industryDto = await _helper.CreateIndustry(master);
        AreaDto areaDto = await _helper.CreateArea();

        UserResultTest user = await _helper.CreateAndGetUser();
        UserResultTest candidate = await _helper.CreateAndGetUser();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Tokens!.Token!);
        
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        var loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token!);
        VacancyDto vacancyDto = await _helper.CreateVacancy(areaDto);
        VacancyDto vacancy = await _helper.ChangeStatusVacancy(vacancyDto.Id, VacancyStatusEnum.Open);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", candidate.Tokens!.Token!);
        ApplicationVacancyDto application = await _helper.CreateApplication(vacancy, candidate);

        HttpResponseMessage message = await _client.GetAsync($"{_URL}/{application.Id}");
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<ApplicationVacancyDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<ApplicationVacancyDto>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int)HttpStatusCode.OK);
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Status.Should().BeTrue();
        
        http.Data.Should().NotBeNull();
        http.Data.Id.Should().Be(application.Id);
    }
    
    [Fact]
    public async Task GetNotFound() 
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", master.Token!);
        
        HttpResponseMessage message = await _client.GetAsync($"{_URL}/{Guid.NewGuid()}");
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        
        http.Should().NotBeNull();
        http.Code.Should().Be((int)HttpStatusCode.NotFound);
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Status.Should().BeFalse();
        
        http.Data.Should().BeNull();
    }
    
    [Fact]
    public async Task Delete() 
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", master.Token!);
        
        IndustryDto industryDto = await _helper.CreateIndustry(master);
        AreaDto areaDto = await _helper.CreateArea();

        UserResultTest user = await _helper.CreateAndGetUser();
        UserResultTest candidate = await _helper.CreateAndGetUser();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Tokens!.Token!);
        
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        var loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token!);
        VacancyDto vacancyDto = await _helper.CreateVacancy(areaDto);
        VacancyDto vacancy = await _helper.ChangeStatusVacancy(vacancyDto.Id, VacancyStatusEnum.Open);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", candidate.Tokens!.Token!);
        ApplicationVacancyDto application = await _helper.CreateApplication(vacancy, candidate);

        HttpResponseMessage message = await _client.DeleteAsync($"{_URL}/{application.Id}");
        message.StatusCode.Should().Be(HttpStatusCode.NoContent);

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int) HttpStatusCode.NoContent);
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Status.Should().BeTrue();
        
        http.Data.Should().BeNull();
    }
    
    [Fact]
    public async Task DeleteNotFound() 
    {
        UserResultTest user = await _helper.CreateAndGetUser();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Tokens!.Token!);
        
        HttpResponseMessage message = await _client.DeleteAsync($"{_URL}/{Guid.NewGuid()}");
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        
        http.Should().NotBeNull();
        http.Code.Should().Be((int)HttpStatusCode.NotFound);
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Status.Should().BeFalse();
        
        http.Data.Should().BeNull();
    }
    
    [Fact]
    public async Task CreateAlreadyApplication() 
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", master.Token!);
        
        IndustryDto industryDto = await _helper.CreateIndustry(master);
        AreaDto areaDto = await _helper.CreateArea();

        UserResultTest user = await _helper.CreateAndGetUser();
        UserResultTest candidate = await _helper.CreateAndGetUser();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Tokens!.Token!);
        
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        var loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token!);
        VacancyDto vacancyDto = await _helper.CreateVacancy(areaDto);
        VacancyDto vacancy = await _helper.ChangeStatusVacancy(vacancyDto.Id, VacancyStatusEnum.Open);

        CreateApplicationVacancyDto dto = new CreateApplicationVacancyDto
        {
            VacancyId = vacancy.Id,
            CoverLetter = string.Concat(Enumerable.Repeat("AnyMessage", 20)),
        };

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", candidate.Tokens!.Token!);
        await _helper.CreateApplication(vacancy, candidate);
        
        HttpResponseMessage message = await _client.PostAsJsonAsync($"{_URL}", dto);
        
        message.StatusCode.Should().Be(HttpStatusCode.Conflict);

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int) HttpStatusCode.Conflict);
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Status.Should().BeFalse();
        
        http.Data.Should().BeNull();
    }

    [Fact]
    public async Task ExistsTrue() 
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", master.Token!);
        
        IndustryDto industryDto = await _helper.CreateIndustry(master);
        AreaDto areaDto = await _helper.CreateArea();

        UserResultTest user = await _helper.CreateAndGetUser();
        UserResultTest candidate = await _helper.CreateAndGetUser();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Tokens!.Token!);
        
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        var loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token!);
        VacancyDto vacancyDto = await _helper.CreateVacancy(areaDto);
        VacancyDto vacancy = await _helper.ChangeStatusVacancy(vacancyDto.Id, VacancyStatusEnum.Open);
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", candidate.Tokens!.Token!);
        await _helper.CreateApplication(vacancy, candidate);

        HttpResponseMessage message = await _client.GetAsync($"{_URL}/{vacancy.Id}/Exists");
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<bool>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<bool>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int) HttpStatusCode.OK);
        http.Message.Should().BeNullOrWhiteSpace();
        http.Status.Should().BeTrue();
        
        http.Data.Should().BeTrue();
    }
    
    [Fact]
    public async Task ExistsFalse() 
    {
        UserResultTest candidate = await _helper.CreateAndGetUser();
       
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", candidate.Tokens!.Token!);

        HttpResponseMessage message = await _client.GetAsync($"{_URL}/{Guid.NewGuid()}/Exists");
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<bool>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<bool>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int) HttpStatusCode.OK);
        http.Message.Should().BeNullOrWhiteSpace();
        http.Status.Should().BeTrue();
        
        http.Data.Should().BeFalse();
    }

    [Fact]
    public async Task GetAll()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", master.Token!);

        HttpResponseMessage message = await _client.GetAsync($"{_URL}");
        _output.WriteLine(message.Content.ReadAsStringAsync().Result);
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        Page<ApplicationVacancyDto>? page = await message.Content.ReadFromJsonAsync<Page<ApplicationVacancyDto>>();
        
        page.Should().NotBeNull();
        page.PageIndex.Should().Be(1);
        page.PageSize.Should().Be(10);
    }
    
    [Fact]
    public async Task GetAllWithLoadVacancy()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", master.Token!);
        
        IndustryDto industryDto = await _helper.CreateIndustry(master);
        AreaDto areaDto = await _helper.CreateArea();

        UserResultTest user = await _helper.CreateAndGetUser();
        UserResultTest candidate = await _helper.CreateAndGetUser();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Tokens!.Token!);
        
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        var loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token!);
        VacancyDto vacancyDto = await _helper.CreateVacancy(areaDto);
        VacancyDto vacancy = await _helper.ChangeStatusVacancy(vacancyDto.Id, VacancyStatusEnum.Open);
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", candidate.Tokens!.Token!);
        await _helper.CreateApplication(vacancy, candidate);
        
        HttpResponseMessage message = await _client.GetAsync($"{_URL}?LoadVacancy={true}");
        _output.WriteLine(message.Content.ReadAsStringAsync().Result);
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        Page<ApplicationVacancyDto>? page = await message.Content.ReadFromJsonAsync<Page<ApplicationVacancyDto>>();
        
        page.Should().NotBeNull();
        page.PageIndex.Should().Be(1);
        page.PageSize.Should().Be(10);
        
        page.Data.Should().NotBeNull();
        page.Data.First().Should().NotBeNull();
        page.Data.First().Vacancy.Should().NotBeNull();
        page.Data.First().User.Should().BeNull();
        
    }
    
    [Fact]
    public async Task GetAllWithLoadUser()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", master.Token!);
        
        IndustryDto industryDto = await _helper.CreateIndustry(master);
        AreaDto areaDto = await _helper.CreateArea();

        UserResultTest user = await _helper.CreateAndGetUser();
        UserResultTest candidate = await _helper.CreateAndGetUser();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Tokens!.Token!);
        
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        var loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token!);
        VacancyDto vacancyDto = await _helper.CreateVacancy(areaDto);
        VacancyDto vacancy = await _helper.ChangeStatusVacancy(vacancyDto.Id, VacancyStatusEnum.Open);
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", candidate.Tokens!.Token!);
        await _helper.CreateApplication(vacancy, candidate);
        
        HttpResponseMessage message = await _client.GetAsync($"{_URL}?LoadUser={true}");
        _output.WriteLine(message.Content.ReadAsStringAsync().Result);
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        Page<ApplicationVacancyDto>? page = await message.Content.ReadFromJsonAsync<Page<ApplicationVacancyDto>>();
        
        page.Should().NotBeNull();
        page.PageIndex.Should().Be(1);
        page.PageSize.Should().Be(10);
        
        page.Data.Should().NotBeNull();
        page.Data.First().Should().NotBeNull();
        page.Data.First().User.Should().NotBeNull();
        page.Data.First().Vacancy.Should().BeNull();
        
    }
    
}
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using JobVacancy.API.IntegrationTests.Utils;
using JobVacancy.API.models.dtos.Area;
using JobVacancy.API.models.dtos.Category;
using JobVacancy.API.models.dtos.Enterprise;
using JobVacancy.API.models.dtos.Industry;
using JobVacancy.API.models.dtos.Vacancy;
using JobVacancy.API.models.entities.Enums;
using JobVacancy.API.Utils.Page;
using JobVacancy.API.Utils.Res;
using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;

namespace JobVacancy.API.IntegrationTests.Vacancy;

public class VacancyControllerTest: IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly Helper _helper;
    private readonly IConfiguration _configuration;
    private readonly string _URL = "/api/v1/Vacancy";
    private readonly ITestOutputHelper _output;
    
    public VacancyControllerTest(CustomWebApplicationFactory factory, ITestOutputHelper output) 
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
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Tokens!.Token!);
        
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        var loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token!);

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

        HttpResponseMessage message = await _client.PostAsJsonAsync($"{_URL}", dto);
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
    }

    [Fact]
    public async Task Get()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", master.Token!);
        
        IndustryDto industryDto = await _helper.CreateIndustry(master);
        AreaDto areaDto = await _helper.CreateArea();

        UserResultTest user = await _helper.CreateAndGetUser();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Tokens!.Token!);
        
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        var loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token!);
        VacancyDto vacancyDto = await _helper.CreateVacancy(areaDto);

        HttpResponseMessage message = await _client.GetAsync($"{_URL}/{vacancyDto.Id}");
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<VacancyDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<VacancyDto>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int) HttpStatusCode.OK);
        http.Status.Should().BeTrue();
        http.Message.Should().NotBeNullOrWhiteSpace();
        
        http.Data.Should().NotBeNull();
        http.Data.Id.Should().NotBeNullOrWhiteSpace();
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
        http.Code.Should().Be((int) HttpStatusCode.NotFound);
        http.Status.Should().BeFalse();
        http.Message.Should().NotBeNullOrWhiteSpace();
        
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
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Tokens!.Token!);
        
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        var loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token!);
        VacancyDto vacancyDto = await _helper.CreateVacancy(areaDto);

        HttpResponseMessage message = await _client.DeleteAsync($"{_URL}/{vacancyDto.Id}");
        message.StatusCode.Should().Be(HttpStatusCode.NoContent);

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int) HttpStatusCode.NoContent);
        http.Status.Should().BeTrue();
        http.Message.Should().NotBeNullOrWhiteSpace();
        
        http.Data.Should().BeNull();
    }
    
    [Fact]
    public async Task DeleteNotFound()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", master.Token!);
        
        IndustryDto industryDto = await _helper.CreateIndustry(master);
        AreaDto areaDto = await _helper.CreateArea();

        UserResultTest user = await _helper.CreateAndGetUser();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Tokens!.Token!);
        
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        var loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token!);

        HttpResponseMessage message = await _client.DeleteAsync($"{_URL}/{Guid.NewGuid()}");
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int) HttpStatusCode.NotFound);
        http.Status.Should().BeFalse();
        http.Message.Should().NotBeNullOrWhiteSpace();
        
        http.Data.Should().BeNull();
    }
    
    [Fact]
    public async Task Patch()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", master.Token!);
        
        IndustryDto industryDto = await _helper.CreateIndustry(master);
        AreaDto areaDto = await _helper.CreateArea();
        AreaDto areaDtoB = await _helper.CreateArea();

        UserResultTest user = await _helper.CreateAndGetUser();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Tokens!.Token!);
        
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        var loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token!);
        VacancyDto vacancyDto = await _helper.CreateVacancy(areaDto);

        UpdateVacancyDto dto = new UpdateVacancyDto
        {
            ApplicationDeadLine = DateTime.UtcNow.AddDays(62),
            AreaId = areaDtoB.Id,
            Title = string.Concat(Enumerable.Repeat(Guid.NewGuid().ToString(), 5)),
            Description = string.Concat(Enumerable.Repeat(Guid.NewGuid().ToString(), 15)),
            Benefits = string.Concat(Enumerable.Repeat(Guid.NewGuid().ToString(), 15)),
            Currency = CurrencyEnum.Cad,
            EducationLevel = EducationLevelEnum.Master,
            EmploymentType = EmploymentTypeEnum.FullTime,
            ExperienceLevel = ExperienceLevelEnum.Senior,
            Opening = 1,
            Requirements = string.Concat(Enumerable.Repeat(Guid.NewGuid().ToString(), 15)),
            Responsibilities = string.Concat(Enumerable.Repeat(Guid.NewGuid().ToString(), 15)),
            SalaryMin = 2500.8m,
            SalaryMax = 4500.8m,
            Seniority = 1,
            WorkplaceType = WorkplaceTypeEnum.Onsite,
            Status = VacancyStatusEnum.Open 
        };
        
        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_URL}/{vacancyDto.Id}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<VacancyDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<VacancyDto>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int) HttpStatusCode.OK);
        http.Status.Should().BeTrue();
        http.Message.Should().NotBeNullOrWhiteSpace();
        
        http.Data.Should().NotBeNull();
        http.Data.Id.Should().Be(vacancyDto.Id);
        http.Data.Title.Should().Be(dto.Title);
        http.Data.Description.Should().Be(dto.Description);
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
        http.Data.Status.Should().Be(dto.Status);
        http.Data.ApplicationDeadLine.Should().BeCloseTo(dto.ApplicationDeadLine.Value, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public async Task GetAll()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", master.Token!);

        HttpResponseMessage message = await _client.GetAsync($"{_URL}");
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        Page<VacancyDto>? page = await message.Content.ReadFromJsonAsync<Page<VacancyDto>>();
        page.Should().NotBeNull();
        
        page.PageIndex.Should().Be(1);
        page.PageSize.Should().Be(10);
    }
    
}
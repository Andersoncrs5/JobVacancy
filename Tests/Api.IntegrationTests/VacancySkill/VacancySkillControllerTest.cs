using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using JobVacancy.API.IntegrationTests.Utils;
using JobVacancy.API.models.dtos.Area;
using JobVacancy.API.models.dtos.Enterprise;
using JobVacancy.API.models.dtos.Industry;
using JobVacancy.API.models.dtos.Skill;
using JobVacancy.API.models.dtos.Vacancy;
using JobVacancy.API.models.dtos.VacancySkill;
using JobVacancy.API.models.entities.Enums;
using JobVacancy.API.Utils.Page;
using JobVacancy.API.Utils.Res;
using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;

namespace JobVacancy.API.IntegrationTests.VacancySkill;

public class VacancySkillControllerTest: IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly Helper _helper;
    private readonly IConfiguration _configuration;
    private readonly string _URL = "/api/v1/VacancySkill";
    private readonly ITestOutputHelper _output;
    
    public VacancySkillControllerTest(CustomWebApplicationFactory factory, ITestOutputHelper output) 
    {
        _client = factory.CreateClient(); 
        _helper = new Helper(_client); 
        _configuration = factory.Configuration;
        _output = output;
    }
    
    [Fact]
    public async Task Patch()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", master.Token!);
        
        IndustryDto industryDto = await _helper.CreateIndustry(master);
        AreaDto areaDto = await _helper.CreateArea();
        
        SkillDto skillDto = await _helper.CreateSkill();
        SkillDto skillDtoB = await _helper.CreateSkill();

        UserResultTest user = await _helper.CreateAndGetUser();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Tokens!.Token!);
        
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        var loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token!);
        VacancyDto vacancyDto = await _helper.CreateVacancy(areaDto);

        VacancySkillDto skillVacancy = await _helper.AddVacancySkillToVacancy(vacancyDto, skillDto);

        UpdateVacancySkillDto dto = new UpdateVacancySkillDto
        {
            IsMandatory = false,
            RequiredLevel = SkillProficiencyLevelEnum.Intermediate,
            SkillId = skillDtoB.Id,
            Weight = 8,
            YearsOfExperienceRequired = 3,
            Notes = string.Concat(Enumerable.Repeat("Updated", 30))
        };
        
        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_URL}/{skillVacancy.Id}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<VacancySkillDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<VacancySkillDto>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int)HttpStatusCode.OK);
        http.Status.Should().BeTrue();

        http.Data.Should().NotBeNull();
        http.Data.Id.Should().Be(skillVacancy.Id);
        http.Data.IsMandatory.Should().Be(dto.IsMandatory.Value);
        http.Data.RequiredLevel.Should().Be(dto.RequiredLevel);
        http.Data.Weight.Should().Be(dto.Weight);
        http.Data.YearsOfExperienceRequired.Should().Be(dto.YearsOfExperienceRequired);
        http.Data.Notes.Should().Be(dto.Notes);
        http.Data.SkillId.Should().Be(dto.SkillId);
    }
    
    [Fact]
    public async Task ExistsFalse()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", master.Token!);
        
        IndustryDto industryDto = await _helper.CreateIndustry(master);
        
        UserResultTest user = await _helper.CreateAndGetUser();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Tokens!.Token!);
        
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        var loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token!);

        HttpResponseMessage message = await _client.GetAsync($"{_URL}/{Guid.NewGuid()}/{Guid.NewGuid()}/Exists");
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<bool>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<bool>>();
        http.Should().NotBeNull();
        http.Code.Should().Be(200);
        http.Status.Should().BeTrue();

        http.Data.Should().BeFalse();
    }
    
    [Fact]
    public async Task ExistsTrue()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", master.Token!);
        
        IndustryDto industryDto = await _helper.CreateIndustry(master);
        AreaDto areaDto = await _helper.CreateArea();
        SkillDto skillDto = await _helper.CreateSkill();

        UserResultTest user = await _helper.CreateAndGetUser();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Tokens!.Token!);
        
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        var loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token!);
        VacancyDto vacancyDto = await _helper.CreateVacancy(areaDto);

        VacancySkillDto skillVacancy = await _helper.AddVacancySkillToVacancy(vacancyDto, skillDto);

        HttpResponseMessage message = await _client.GetAsync($"{_URL}/{skillVacancy.VacancyId}/{skillVacancy.SkillId}/Exists");
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<bool>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<bool>>();
        http.Should().NotBeNull();
        http.Code.Should().Be(200);
        http.Status.Should().BeTrue();

        http.Data.Should().BeTrue();
    }
    
    [Fact]
    public async Task Get()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", master.Token!);
        
        IndustryDto industryDto = await _helper.CreateIndustry(master);
        AreaDto areaDto = await _helper.CreateArea();
        SkillDto skillDto = await _helper.CreateSkill();

        UserResultTest user = await _helper.CreateAndGetUser();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Tokens!.Token!);
        
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        var loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token!);
        VacancyDto vacancyDto = await _helper.CreateVacancy(areaDto);

        VacancySkillDto skillVacancy = await _helper.AddVacancySkillToVacancy(vacancyDto, skillDto);

        HttpResponseMessage message = await _client.GetAsync($"{_URL}/{skillVacancy.Id}");
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<VacancySkillDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<VacancySkillDto>>();
        http.Should().NotBeNull();
        http.Code.Should().Be(200);
        http.Status.Should().BeTrue();

        http.Data.Should().NotBeNull();
        http.Data.Id.Should().Be(skillVacancy.Id);
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
        http.Code.Should().Be(404);
        http.Status.Should().BeFalse();

        http.Data.Should().BeNull();
    }

    [Fact]
    public async Task Del()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", master.Token!);
        
        IndustryDto industryDto = await _helper.CreateIndustry(master);
        AreaDto areaDto = await _helper.CreateArea();
        SkillDto skillDto = await _helper.CreateSkill();

        UserResultTest user = await _helper.CreateAndGetUser();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Tokens!.Token!);
        
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        var loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token!);
        VacancyDto vacancyDto = await _helper.CreateVacancy(areaDto);

        VacancySkillDto skillVacancy = await _helper.AddVacancySkillToVacancy(vacancyDto, skillDto);

        HttpResponseMessage message = await _client.DeleteAsync($"{_URL}/{skillVacancy.Id}");
        message.StatusCode.Should().Be(HttpStatusCode.NoContent);

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Code.Should().Be(204);
        http.Status.Should().BeTrue();

        http.Data.Should().BeNull();
    }

    [Fact]
    public async Task DelNotFound()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", master.Token!);
        
        IndustryDto industryDto = await _helper.CreateIndustry(master);
        
        UserResultTest user = await _helper.CreateAndGetUser();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Tokens!.Token!);
        
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        var loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token!);

        HttpResponseMessage message = await _client.DeleteAsync($"{_URL}/{Guid.NewGuid()}");
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Code.Should().Be(404);
        http.Status.Should().BeFalse();

        http.Data.Should().BeNull();
    }

    [Fact]
    public async Task Create()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", master.Token!);
        
        IndustryDto industryDto = await _helper.CreateIndustry(master);
        AreaDto areaDto = await _helper.CreateArea();
        SkillDto skillDto = await _helper.CreateSkill();

        UserResultTest user = await _helper.CreateAndGetUser();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Tokens!.Token!);
        
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        var loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token!);
        VacancyDto vacancyDto = await _helper.CreateVacancy(areaDto);

        CreateVacancySkillDto dto = new CreateVacancySkillDto
        {
            VacancyId = vacancyDto.Id,
            SkillId = skillDto.Id,
            IsMandatory = true,
            RequiredLevel = SkillProficiencyLevelEnum.Beginner,
            Weight = 4,
            YearsOfExperienceRequired = 2
        };

        HttpResponseMessage message = await _client.PostAsJsonAsync($"{_URL}", dto);
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
    }

    [Fact]
    public async Task GetAll()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", master.Token!);

        HttpResponseMessage message = await _client.GetAsync($"{_URL}");
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        Page<VacancySkillDto>? page = await message.Content.ReadFromJsonAsync<Page<VacancySkillDto>>();
        
        page.Should().NotBeNull();
        page.PageIndex.Should().Be(1);
        page.PageSize.Should().Be(10);
    }
    
}
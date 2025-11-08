using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using JobVacancy.API.IntegrationTests.Utils;
using JobVacancy.API.models.dtos.Skill;
using JobVacancy.API.models.dtos.UserSkill;
using JobVacancy.API.models.entities.Enums;
using JobVacancy.API.Utils.Res;
using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;

namespace JobVacancy.API.IntegrationTests.UserSkill;

public class UserSkillControllerTest: IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly Helper _helper;
    private readonly IConfiguration _configuration;
    private readonly string _URL = "/api/v1/UserSkill";
    private readonly ITestOutputHelper _output;
    
    public UserSkillControllerTest(CustomWebApplicationFactory factory, ITestOutputHelper output) 
    {
        _client = factory.CreateClient(); 
        _helper = new Helper(_client); 
        _configuration = factory.Configuration;
        _output = output;
    }
    
    [Fact]
    public async Task GetAll()
    {
        UserResultTest user = await _helper.CreateAndGetUser();
        
        string token = user.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage message = await _client.GetAsync($"{_URL}");
        message.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task CreateNotFoundSkill()
    {
        UserResultTest user = await _helper.CreateAndGetUser();
        
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        token = user.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage message = await _client.PostAsync($"{_URL}/Toggle/{Guid.NewGuid()}", null);
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();

        http.Code.Should().Be(404);
        http.Status.Should().BeFalse();
        http.Message.Should().NotBeNullOrWhiteSpace();
    }
    
    [Fact]
    public async Task Create()
    {
        UserResultTest user = await _helper.CreateAndGetUser();
        
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        SkillDto skill = await _helper.CreateSkill();

        token = user.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage message = await _client.PostAsync($"{_URL}/Toggle/{skill.Id}", null);
        message.StatusCode.Should().Be(HttpStatusCode.Created);

        ResponseHttp<UserSkillDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<UserSkillDto>>();
        http.Should().NotBeNull();

        http.Code.Should().Be(201);
        http.Status.Should().BeTrue();
        http.Message.Should().NotBeNullOrWhiteSpace();
        
        http.Data.Should().NotBeNull();
        http.Data.Id.Should().NotBeNullOrWhiteSpace();
        http.Data.SkillId.Should().Be(skill.Id);
        http.Data.UserId.Should().Be(user.User!.Id);
        http.Data.ExternalCertificateUrl.Should().BeNull();
        http.Data.YearsOfExperience.Should().BeNull();
        http.Data.ProficiencyLevel.Should().BeNull();
    }

    [Fact]
    public async Task Remove()
    {
        UserResultTest user = await _helper.CreateAndGetUser();
        
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        SkillDto skill = await _helper.CreateSkill();

        token = user.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        UserSkillDto userSkill = await _helper.CreateUserSkill(skill);
        
        HttpResponseMessage message = await _client.PostAsync($"{_URL}/Toggle/{skill.Id}", null);
        message.StatusCode.Should().Be(HttpStatusCode.NoContent);

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();

        http.Code.Should().Be(204);
        http.Status.Should().BeTrue();
        http.Message.Should().NotBeNullOrWhiteSpace();
        
        http.Data.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAllFields()
    {
        UserResultTest user = await _helper.CreateAndGetUser();
        
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        SkillDto skill = await _helper.CreateSkill();

        token = user.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        UserSkillDto userSkill = await _helper.CreateUserSkill(skill);

        UpdateUserSkillDto dto = new UpdateUserSkillDto
        {
            ProficiencyLevel = SkillProficiencyLevelEnum.Advanced,
            ExternalCertificateUrl = "https://github.com/Andersoncrs5",
            YearsOfExperience = 3
        };

        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_URL}/{userSkill.Id}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        
        ResponseHttp<UserSkillDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<UserSkillDto>>();
        http.Should().NotBeNull();
        http.Code.Should().Be(200);
        http.Status.Should().BeTrue();
        http.Message.Should().NotBeNullOrWhiteSpace();
        
        http.Data.Should().NotBeNull();
        http.Data.Id.Should().Be(userSkill.Id);
        http.Data.ProficiencyLevel.Should().Be(dto.ProficiencyLevel);
        http.Data.ExternalCertificateUrl.Should().Be(dto.ExternalCertificateUrl);
        http.Data.YearsOfExperience.Should().Be(dto.YearsOfExperience);
        http.Data.SkillId.Should().Be(userSkill.SkillId);
        http.Data.UserId.Should().Be(userSkill.UserId);
    }
    
    [Fact]
    public async Task UpdateJustProficiencyLevel()
    {
        UserResultTest user = await _helper.CreateAndGetUser();
        
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        SkillDto skill = await _helper.CreateSkill();

        token = user.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        UserSkillDto userSkill = await _helper.CreateUserSkill(skill);

        UpdateUserSkillDto dto = new UpdateUserSkillDto
        {
            ProficiencyLevel = SkillProficiencyLevelEnum.Advanced,
        };

        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_URL}/{userSkill.Id}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        
        ResponseHttp<UserSkillDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<UserSkillDto>>();
        http.Should().NotBeNull();
        http.Code.Should().Be(200);
        http.Status.Should().BeTrue();
        http.Message.Should().NotBeNullOrWhiteSpace();
        
        http.Data.Should().NotBeNull();
        http.Data.Id.Should().Be(userSkill.Id);
        http.Data.ProficiencyLevel.Should().Be(dto.ProficiencyLevel);
        http.Data.ExternalCertificateUrl.Should().Be(userSkill.ExternalCertificateUrl);
        http.Data.YearsOfExperience.Should().Be(userSkill.YearsOfExperience);
        http.Data.SkillId.Should().Be(userSkill.SkillId);
        http.Data.UserId.Should().Be(userSkill.UserId);
    }
    
    [Fact]
    public async Task UpdateJustExternalCertificateUrl()
    {
        UserResultTest user = await _helper.CreateAndGetUser();
        
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        SkillDto skill = await _helper.CreateSkill();

        token = user.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        UserSkillDto userSkill = await _helper.CreateUserSkill(skill);

        UpdateUserSkillDto dto = new UpdateUserSkillDto
        {
            ExternalCertificateUrl = "https://github.com/Andersoncrs5",
        };

        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_URL}/{userSkill.Id}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        
        ResponseHttp<UserSkillDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<UserSkillDto>>();
        http.Should().NotBeNull();
        http.Code.Should().Be(200);
        http.Status.Should().BeTrue();
        http.Message.Should().NotBeNullOrWhiteSpace();
        
        http.Data.Should().NotBeNull();
        http.Data.Id.Should().Be(userSkill.Id);
        http.Data.ProficiencyLevel.Should().Be(userSkill.ProficiencyLevel);
        http.Data.ExternalCertificateUrl.Should().Be(dto.ExternalCertificateUrl);
        http.Data.YearsOfExperience.Should().Be(userSkill.YearsOfExperience);
        http.Data.SkillId.Should().Be(userSkill.SkillId);
        http.Data.UserId.Should().Be(userSkill.UserId);
        
    }
    
    [Fact]
    public async Task UpdateJustYearsOfExperience()
    {
        UserResultTest user = await _helper.CreateAndGetUser();
        
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        SkillDto skill = await _helper.CreateSkill();

        token = user.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        UserSkillDto userSkill = await _helper.CreateUserSkill(skill);

        UpdateUserSkillDto dto = new UpdateUserSkillDto
        {
            YearsOfExperience = 3
        };

        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_URL}/{userSkill.Id}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        
        ResponseHttp<UserSkillDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<UserSkillDto>>();
        http.Should().NotBeNull();
        http.Code.Should().Be(200);
        http.Status.Should().BeTrue();
        http.Message.Should().NotBeNullOrWhiteSpace();
        
        http.Data.Should().NotBeNull();
        http.Data.Id.Should().Be(userSkill.Id);
        http.Data.ProficiencyLevel.Should().Be(userSkill.ProficiencyLevel);
        http.Data.ExternalCertificateUrl.Should().Be(userSkill.ExternalCertificateUrl);
        http.Data.YearsOfExperience.Should().Be(dto.YearsOfExperience);
        http.Data.SkillId.Should().Be(userSkill.SkillId);
        http.Data.UserId.Should().Be(userSkill.UserId);
    }
    
    [Fact]
    public async Task ExistsBySkillIdReturnTrue()
    {
        UserResultTest user = await _helper.CreateAndGetUser();
        
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        SkillDto skill = await _helper.CreateSkill();

        token = user.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        UserSkillDto userSkill = await _helper.CreateUserSkill(skill);

        HttpResponseMessage message = await _client.GetAsync($"{_URL}/{userSkill.SkillId}/exists");
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        ResponseHttp<bool>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<bool>>();
        http.Should().NotBeNull();
        http.Code.Should().Be(200);
        http.Status.Should().BeTrue();
        
        http.Data.Should().BeTrue();
    }
    
    [Fact]
    public async Task ExistsBySkillIdReturnFalse()
    {
        UserResultTest user = await _helper.CreateAndGetUser();
        
        string token = user.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        HttpResponseMessage message = await _client.GetAsync($"{_URL}/{Guid.NewGuid()}/exists");
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        ResponseHttp<bool>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<bool>>();
        http.Should().NotBeNull();
        http.Code.Should().Be(200);
        http.Status.Should().BeTrue();
        
        http.Data.Should().BeFalse();
    }
    
}
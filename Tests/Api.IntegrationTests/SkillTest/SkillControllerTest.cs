using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using JobVacancy.API.IntegrationTests.Utils;
using JobVacancy.API.models.dtos.Skill;
using JobVacancy.API.Utils.Page;
using JobVacancy.API.Utils.Res;
using Microsoft.Extensions.Configuration;

namespace JobVacancy.API.IntegrationTests.SkillControllerTest;

public class SkillControllerTest: IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly Helper _helper;
    private readonly IConfiguration _configuration;
    private readonly string _URL = "/api/v1/Skill";
    
    public SkillControllerTest(CustomWebApplicationFactory factory) 
    {
        _client = factory.CreateClient(); 
        _helper = new Helper(_client); 
        _configuration = factory.Configuration;
    }

    [Fact]
    public async Task Create()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        CreateSkillDto dto = new CreateSkillDto
        {
            Name = "TestSkill" + Guid.NewGuid().ToString(),
            Description = "TestSkill" + Guid.NewGuid().ToString(),
            IconUrl = "https://github.com/Andersoncrs5",
            IsActive = true
        };

        HttpResponseMessage message = await _client.PostAsJsonAsync(_URL, dto);
        message.StatusCode.Should().Be(HttpStatusCode.Created);

        ResponseHttp<SkillDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<SkillDto>>();
        http.Should().NotBeNull();
        http.Data.Should().NotBeNull();
        http.Data.Name.Should().Be(dto.Name);
        http.Data.Description.Should().Be(dto.Description);
        http.Data.IconUrl.Should().Be(dto.IconUrl);
        http.Data.IsActive.Should().Be(true);
        http.Data.Id.Should().NotBeEmpty();
    }
    
    [Fact]
    public async Task CreateReturnForb()
    {
        UserResultTest user = await _helper.CreateAndGetUser();
        string token = user.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);


        CreateSkillDto dto = new CreateSkillDto
        {
            Name = "TestSkill" + Guid.NewGuid().ToString(),
            Description = "TestSkill" + Guid.NewGuid().ToString(),
            IconUrl = "https://github.com/Andersoncrs5",
            IsActive = true
        };

        HttpResponseMessage message = await _client.PostAsJsonAsync(_URL, dto);
        message.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task CreateReturnConflictName()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        SkillDto skill = await _helper.CreateSkill();

        CreateSkillDto dto = new CreateSkillDto
        {
            Name = skill.Name,
            Description = "TestSkill" + Guid.NewGuid().ToString(),
            IconUrl = "https://github.com/Andersoncrs5",
            IsActive = true
        };

        HttpResponseMessage message = await _client.PostAsJsonAsync(_URL, dto);
        message.StatusCode.Should().Be(HttpStatusCode.Conflict);

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Data.Should().BeNull();
        http.Message.Should().NotBeNull();
        http.Code.Should().Be(409);
    }

    [Fact]
    public async Task Get()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        SkillDto skill = await _helper.CreateSkill();

        HttpResponseMessage message = await _client.GetAsync(_URL+"/"+skill.Id);
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        ResponseHttp<SkillDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<SkillDto>>();
        http.Should().NotBeNull();
        http.Data.Should().NotBeNull();
        http.Code.Should().Be(200);
        http.Message.Should().NotBeNull();
        http.Data.Name.Should().Be(skill.Name);
        
    }
    
    [Fact]
    public async Task GetNotFound()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        SkillDto skill = await _helper.CreateSkill();

        HttpResponseMessage message = await _client.GetAsync(_URL+"/"+Guid.NewGuid().ToString());
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);
        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        
        http.Should().NotBeNull();
        http.Data.Should().BeNull();
        http.Code.Should().Be(404);
        http.Message.Should().NotBeNull();
        http.Status.Should().BeFalse();
    }
    
    [Fact]
    public async Task Del()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        SkillDto skill = await _helper.CreateSkill();

        HttpResponseMessage message = await _client.DeleteAsync(_URL+"/"+skill.Id);
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Data.Should().BeNull();
        http.Code.Should().Be(200);
        http.Message.Should().NotBeNull();
        http.Status.Should().BeTrue();
    }
    
    [Fact]
    public async Task DelThrowForb()
    {
        UserResultTest user = await _helper.CreateAndGetUser();
        string token = user.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage message = await _client.DeleteAsync(_URL+"/"+Guid.NewGuid().ToString());
        message.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task DelNotFound()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        SkillDto skill = await _helper.CreateSkill();

        HttpResponseMessage message = await _client.DeleteAsync(_URL+"/"+Guid.NewGuid().ToString());
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);
        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        
        http.Should().NotBeNull();
        http.Data.Should().BeNull();
        http.Code.Should().Be(404);
        http.Message.Should().NotBeNull();
        http.Status.Should().BeFalse();
    }

    [Fact]
    public async Task GetAll()
    {
        UserResultTest user = await _helper.CreateAndGetUser();
        string token = user.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage message = await _client.GetAsync(_URL);
        message.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task GetAllWithFilter()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        SkillDto skill = await _helper.CreateSkill();

        HttpResponseMessage message = await _client.GetAsync($"{_URL}?" +
                                                             $"Name={skill.Name}" +
                                                             $"&IsActive={skill.IsActive}" +
                                                             $"&IconUrl={skill.IconUrl}" +
                                                             $"&Id={skill.Id}");
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        Page<SkillDto>? page = await message.Content.ReadFromJsonAsync<Page<SkillDto>>();
        page.Should().NotBeNull();
        page.Data.Should().NotBeNull();
        page.Data.First().Name.Should().Be(skill.Name);
    }
    
    [Fact]
    public async Task UpdateAllFields()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        SkillDto skill = await _helper.CreateSkill();

        UpdateSkillDto dto = new UpdateSkillDto
        {
            Name = Guid.NewGuid().ToString(),
            Description = "Generally, those who know little speak much, and those who know much speak little.",
            IconUrl = skill.IconUrl,
            IsActive = true,
        };
        
        HttpResponseMessage message = await _client.PatchAsJsonAsync(_URL+"/"+skill.Id, dto);
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        ResponseHttp<SkillDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<SkillDto>>();
        http.Should().NotBeNull();
        http.Data.Should().NotBeNull();
        http.Code.Should().Be(200);
        http.Message.Should().NotBeNull();
        http.Status.Should().BeTrue();
        
        http.Data.Id.Should().Be(skill.Id);
        http.Data.Name.Should().Be(dto.Name);
        http.Data.Description.Should().Be(dto.Description);
        http.Data.IsActive.Should().BeTrue();
    }
    
    [Fact]
    public async Task UpdateJustName()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        SkillDto skill = await _helper.CreateSkill();

        UpdateSkillDto dto = new UpdateSkillDto
        {
            Name = Guid.NewGuid().ToString(),
        };
        
        HttpResponseMessage message = await _client.PatchAsJsonAsync(_URL+"/"+skill.Id, dto);
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        ResponseHttp<SkillDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<SkillDto>>();
        http.Should().NotBeNull();
        http.Data.Should().NotBeNull();
        http.Code.Should().Be(200);
        http.Message.Should().NotBeNull();
        http.Status.Should().BeTrue();
        
        http.Data.Id.Should().Be(skill.Id);
        http.Data.Name.Should().Be(dto.Name);
        http.Data.Description.Should().Be(skill.Description);
        http.Data.IsActive.Should().Be(skill.IsActive);
    }
    
    [Fact]
    public async Task UpdateJustDescription()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        SkillDto skill = await _helper.CreateSkill();

        UpdateSkillDto dto = new UpdateSkillDto
        {
            Description = "Generally, those who know little speak much, and those who know much speak little.",
        };
        
        HttpResponseMessage message = await _client.PatchAsJsonAsync(_URL+"/"+skill.Id, dto);
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        ResponseHttp<SkillDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<SkillDto>>();
        http.Should().NotBeNull();
        http.Data.Should().NotBeNull();
        http.Code.Should().Be(200);
        http.Message.Should().NotBeNull();
        http.Status.Should().BeTrue();
        
        http.Data.Id.Should().Be(skill.Id);
        http.Data.Name.Should().Be(skill.Name);
        http.Data.Description.Should().Be(dto.Description);
        http.Data.IsActive.Should().Be(skill.IsActive);
    }
    
    [Fact]
    public async Task UpdateJustIsActive()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        SkillDto skill = await _helper.CreateSkill();

        UpdateSkillDto dto = new UpdateSkillDto
        {
            IsActive = false,
        };
        
        HttpResponseMessage message = await _client.PatchAsJsonAsync(_URL+"/"+skill.Id, dto);
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        ResponseHttp<SkillDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<SkillDto>>();
        http.Should().NotBeNull();
        http.Data.Should().NotBeNull();
        http.Code.Should().Be(200);
        http.Message.Should().NotBeNull();
        http.Status.Should().BeTrue();
        
        http.Data.Id.Should().Be(skill.Id);
        http.Data.Name.Should().Be(skill.Name);
        http.Data.Description.Should().Be(skill.Description);
        http.Data.IsActive.Should().Be(dto.IsActive.Value);
    }
    
    [Fact]
    public async Task UpdateJustIconUrl()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        SkillDto skill = await _helper.CreateSkill();

        UpdateSkillDto dto = new UpdateSkillDto
        {
            IconUrl = "https://github.com/Andersoncrs5/"
        };
        
        HttpResponseMessage message = await _client.PatchAsJsonAsync(_URL+"/"+skill.Id, dto);
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        ResponseHttp<SkillDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<SkillDto>>();
        http.Should().NotBeNull();
        http.Data.Should().NotBeNull();
        http.Code.Should().Be(200);
        http.Message.Should().NotBeNull();
        http.Status.Should().BeTrue();
        
        http.Data.Id.Should().Be(skill.Id);
        http.Data.Name.Should().Be(skill.Name);
        http.Data.Description.Should().Be(skill.Description);
        http.Data.IsActive.Should().Be(skill.IsActive);
        http.Data.IconUrl.Should().Be(dto.IconUrl);
    }
    
    [Fact]
    public async Task UpdateConflictName()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        SkillDto skill = await _helper.CreateSkill();
        SkillDto skillB = await _helper.CreateSkill();

        UpdateSkillDto dto = new UpdateSkillDto
        {
            Name = skillB.Name,
        };
        
        HttpResponseMessage message = await _client.PatchAsJsonAsync(_URL+"/"+skill.Id, dto);
        message.StatusCode.Should().Be(HttpStatusCode.Conflict);
        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Data.Should().BeNull();
        http.Code.Should().Be(409);
        http.Message.Should().NotBeNull();
        http.Status.Should().BeFalse();
    }
    
    [Fact]
    public async Task ExistsByNameReturnTrue()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        SkillDto skill = await _helper.CreateSkill();

        HttpResponseMessage message = await _client.GetAsync(_URL+"/"+skill.Name+"/exists/name");
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        ResponseHttp<bool>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<bool>>();
        http.Should().NotBeNull();
        
        http.Code.Should().Be(200);
        http.Message.Should().NotBeNull();
        http.Status.Should().BeTrue();
        
        http.Status.Should().BeTrue();
    }
    
    [Fact]
    public async Task ExistsByNameReturnFalse()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        string name = "DecadenceIsNotEasy";
        HttpResponseMessage message = await _client.GetAsync($"{_URL}/{name}/exists/name");
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        ResponseHttp<bool>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<bool>>();
        http.Should().NotBeNull();
        
        http.Code.Should().Be(200);
        http.Message.Should().NotBeNull();
        http.Status.Should().BeTrue();
        
        http.Data.Should().BeFalse();
    }
    
}
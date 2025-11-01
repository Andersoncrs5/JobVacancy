using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using JobVacancy.API.IntegrationTests.Utils;
using JobVacancy.API.models.dtos.Skill;
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

    
    
    
}
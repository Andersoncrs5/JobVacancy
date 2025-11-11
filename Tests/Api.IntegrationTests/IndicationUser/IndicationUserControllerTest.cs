using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using JobVacancy.API.IntegrationTests.Utils;
using JobVacancy.API.models.dtos.IndicationUser;
using JobVacancy.API.Utils.Res;
using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;

namespace JobVacancy.API.IntegrationTests.IndicationUser;

public class IndicationUserControllerTest: IClassFixture<CustomWebApplicationFactory>
{
    
    private readonly HttpClient _client;
    private readonly Helper _helper;
    private readonly IConfiguration _configuration;
    private readonly string _URL = "/api/v1/IndicationUser";
    private readonly ITestOutputHelper _output;

    public IndicationUserControllerTest(CustomWebApplicationFactory factory, ITestOutputHelper output) 
    {
        _client = factory.CreateClient(); 
        _helper = new Helper(_client); 
        _configuration = factory.Configuration;
        _output = output;
    }

    [Fact]
    public async Task Create()
    {
        UserResultTest user = await _helper.CreateAndGetUser();
        UserResultTest endorsedUser = await _helper.CreateAndGetUser();

        CreateIndicationUserDto dto = new CreateIndicationUserDto()
        {
            EndorsedId = endorsedUser.User!.Id,
            Content = string.Concat(Enumerable.Repeat("HeIsAWellEmployee", 10)),
            SkillRating = Random.Shared.Next(0, 10),
        };
        
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", user.Tokens!.Token);

        HttpResponseMessage message = await _client.PostAsJsonAsync(_URL, dto);
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
    }
    
    [Fact]
    public async Task CreateConflictUser()
    {
        UserResultTest user = await _helper.CreateAndGetUser();
        UserResultTest endorsedUser = await _helper.CreateAndGetUser();

        CreateIndicationUserDto dto = new CreateIndicationUserDto()
        {
            EndorsedId = endorsedUser.User!.Id,
            Content = string.Concat(Enumerable.Repeat("HeIsAWellEmployee", 10)),
            SkillRating = Random.Shared.Next(0, 10),
        };
        
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", user.Tokens!.Token);

        IndicationUserDto indicationUser = await _helper.AddIndicationUser(endorsedUser);
        
        HttpResponseMessage message = await _client.PostAsJsonAsync(_URL, dto);
        message.StatusCode.Should().Be(HttpStatusCode.Conflict);

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int)HttpStatusCode.Conflict);
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Status.Should().BeFalse();
        
        http.Data.Should().BeNull();
    }

    [Fact]
    public async Task ExistsTrue()
    {
        UserResultTest user = await _helper.CreateAndGetUser();
        UserResultTest endorsedUser = await _helper.CreateAndGetUser();
        
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", user.Tokens!.Token);

        IndicationUserDto indicationUser = await _helper.AddIndicationUser(endorsedUser);

        HttpResponseMessage message = await _client.GetAsync($"{_URL}/{endorsedUser.User.Id}/Exists");
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<bool>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<bool>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int)HttpStatusCode.OK);
        http.Message.Should().BeNullOrWhiteSpace();
        http.Status.Should().BeTrue();

        http.Data.Should().BeTrue();
    }

    [Fact]
    public async Task ExistsFalse()
    {
        UserResultTest user = await _helper.CreateAndGetUser();
        UserResultTest endorsedUser = await _helper.CreateAndGetUser();
        
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", user.Tokens!.Token);
        
        HttpResponseMessage message = await _client.GetAsync($"{_URL}/{endorsedUser.User!.Id}/Exists");
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<bool>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<bool>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int)HttpStatusCode.OK);
        http.Message.Should().BeNullOrWhiteSpace();
        http.Status.Should().BeTrue();

        http.Data.Should().BeFalse();
    }

    [Fact]
    public async Task Delete()
    {
        UserResultTest user = await _helper.CreateAndGetUser();
        UserResultTest endorsedUser = await _helper.CreateAndGetUser();
        
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", user.Tokens!.Token);

        IndicationUserDto indicationUser = await _helper.AddIndicationUser(endorsedUser);

        HttpResponseMessage message = await _client.DeleteAsync($"{_URL}/{indicationUser.Id}");
        message.StatusCode.Should().Be(HttpStatusCode.NoContent);

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int)HttpStatusCode.NoContent);
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Status.Should().BeTrue();

        http.Data.Should().BeNull();
    }
    
    [Fact]
    public async Task DeleteNotFound()
    {
        UserResultTest user = await _helper.CreateAndGetUser();
        UserResultTest endorsedUser = await _helper.CreateAndGetUser();
        
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
    public async Task Get()
    {
        UserResultTest user = await _helper.CreateAndGetUser();
        UserResultTest endorsedUser = await _helper.CreateAndGetUser();
        
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", user.Tokens!.Token);

        IndicationUserDto indicationUser = await _helper.AddIndicationUser(endorsedUser);

        HttpResponseMessage message = await _client.GetAsync($"{_URL}/{indicationUser.Id}");
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<IndicationUserDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<IndicationUserDto>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int)HttpStatusCode.OK);
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Status.Should().BeTrue();

        http.Data.Should().NotBeNull();
        http.Data.Id.Should().Be(indicationUser.Id);
    }
    
    [Fact]
    public async Task GetNotFound()
    {
        UserResultTest user = await _helper.CreateAndGetUser();
        UserResultTest endorsedUser = await _helper.CreateAndGetUser();
        
        HttpResponseMessage message = await _client.GetAsync($"{_URL}/{Guid.NewGuid()}");
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int)HttpStatusCode.NotFound);
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Status.Should().BeFalse();

        http.Data.Should().BeNull();
    }
}
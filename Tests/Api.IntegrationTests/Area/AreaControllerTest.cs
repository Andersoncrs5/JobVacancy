using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using JobVacancy.API.models.dtos.Area;
using JobVacancy.API.Utils.Page;
using JobVacancy.API.Utils.Res;
using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;

namespace JobVacancy.API.IntegrationTests.Area;

public class AreaControllerTest: IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly Helper _helper;
    private readonly IConfiguration _configuration;
    private readonly string _URL = "/api/v1/Area";
    private readonly ITestOutputHelper _output;

    public AreaControllerTest(CustomWebApplicationFactory factory, ITestOutputHelper output) 
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
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        CreateAreaDto dto = new CreateAreaDto
        {
            IsActive = true,
            Name = $"Test {Guid.NewGuid()}",
            Description = $"Test description {Guid.NewGuid()}",
        };

        HttpResponseMessage message = await _client.PostAsJsonAsync("api/v1/Area", dto);
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
    }

    [Fact]
    public async Task Get()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        AreaDto area = await _helper.CreateArea();

        HttpResponseMessage message = await _client.GetAsync($"{_URL}/{area.Id}");
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        
        ResponseHttp<AreaDto>? response = await message.Content.ReadFromJsonAsync<ResponseHttp<AreaDto>>();
        response.Should().NotBeNull();
        response.Status.Should().BeTrue();
        response.Message.Should().NotBeNullOrWhiteSpace();
        response.Code.Should().Be((int) HttpStatusCode.OK);
        Assert.NotNull(response.Data);
        
        response.Data.Id.Should().Be(area.Id);
    }

    [Fact]
    public async Task GetNotFound()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage message = await _client.GetAsync($"{_URL}/{Guid.NewGuid()}");
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);
        
        ResponseHttp<object>? response = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        response.Should().NotBeNull();
        response.Status.Should().BeFalse();
        response.Message.Should().NotBeNullOrWhiteSpace();
        response.Code.Should().Be((int) HttpStatusCode.NotFound);

        response.Data.Should().BeNull();
    }
    
    [Fact]
    public async Task Del()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        AreaDto area = await _helper.CreateArea();

        HttpResponseMessage message = await _client.DeleteAsync($"{_URL}/{area.Id}");
        message.StatusCode.Should().Be(HttpStatusCode.NoContent);
        
        ResponseHttp<object>? response = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        response.Should().NotBeNull();
        response.Status.Should().BeTrue();
        response.Message.Should().NotBeNullOrWhiteSpace();
        response.Code.Should().Be((int) HttpStatusCode.NoContent);
        Assert.Null(response.Data);
        
    }

    [Fact]
    public async Task DelNotFound()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        AreaDto area = await _helper.CreateArea();

        HttpResponseMessage message = await _client.DeleteAsync($"{_URL}/{Guid.NewGuid()}");
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);
        
        ResponseHttp<object>? response = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        response.Should().NotBeNull();
        response.Status.Should().BeFalse();
        response.Message.Should().NotBeNullOrWhiteSpace();
        response.Code.Should().Be((int) HttpStatusCode.NotFound);
        Assert.Null(response.Data);
        
    }

    [Fact]
    public async Task Patch()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        AreaDto area = await _helper.CreateArea();

        UpdateAreaDto dto = new UpdateAreaDto()
        {
            Description = string.Concat(Enumerable.Repeat("TestUpdated", 20)),
            IsActive = false,
            Name = $"Test {Guid.NewGuid()}",
        };
        
        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_URL}/{area.Id}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        
        ResponseHttp<AreaDto>? response = await message.Content.ReadFromJsonAsync<ResponseHttp<AreaDto>>();
        response.Should().NotBeNull();
        response.Status.Should().BeTrue();
        response.Message.Should().NotBeNullOrWhiteSpace();
        response.Code.Should().Be((int) HttpStatusCode.OK);
        Assert.NotNull(response.Data);
        
        response.Data.Id.Should().Be(area.Id);
        response.Data.Name.Should().Be(dto.Name);
        response.Data.Description.Should().Be(dto.Description);
        response.Data.IsActive.Should().Be(dto.IsActive.Value);
    }
    
    [Fact]
    public async Task PatchConflictName()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        AreaDto area = await _helper.CreateArea();
        AreaDto areaB = await _helper.CreateArea();

        UpdateAreaDto dto = new UpdateAreaDto()
        {
            Description = string.Concat(Enumerable.Repeat("TestUpdated", 20)),
            IsActive = false,
            Name = areaB.Name,
        };
        
        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_URL}/{area.Id}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.Conflict);
        
        ResponseHttp<object>? response = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        response.Should().NotBeNull();
        response.Status.Should().BeFalse();
        response.Message.Should().NotBeNullOrWhiteSpace();
        response.Code.Should().Be((int) HttpStatusCode.Conflict);
        Assert.Null(response.Data);
    }

    [Fact]
    public async Task GetAll() 
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage message = await _client.GetAsync($"{_URL}");
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        
        Page<AreaDto>? response = await message.Content.ReadFromJsonAsync<Page<AreaDto>>();
        
        response.Should().NotBeNull();
        response.PageIndex.Should().Be(1);
        response.PageSize.Should().Be(10);
    }
    
    [Fact]
    public async Task ExistsTrue()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        AreaDto area = await _helper.CreateArea();

        HttpResponseMessage message = await _client.GetAsync($"{_URL}/{area.Name}/Exists/Name");
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        
        ResponseHttp<bool>? response = await message.Content.ReadFromJsonAsync<ResponseHttp<bool>>();
        response.Should().NotBeNull();
        response.Status.Should().BeTrue();
        response.Message.Should().NotBeNullOrWhiteSpace();
        response.Code.Should().Be((int) HttpStatusCode.OK);

        response.Data.Should().BeTrue();

    }

    [Fact]
    public async Task ExistsFalse()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        HttpResponseMessage message = await _client.GetAsync($"{_URL}/{Guid.NewGuid()}/Exists/Name");
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        
        ResponseHttp<bool>? response = await message.Content.ReadFromJsonAsync<ResponseHttp<bool>>();
        response.Should().NotBeNull();
        response.Status.Should().BeTrue();
        response.Message.Should().NotBeNullOrWhiteSpace();
        response.Code.Should().Be((int) HttpStatusCode.OK);

        response.Data.Should().BeFalse();

    }

    
}
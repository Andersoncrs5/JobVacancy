using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using JobVacancy.API.models.dtos.Industry;
using JobVacancy.API.models.dtos.Position;
using JobVacancy.API.Utils.Page;
using JobVacancy.API.Utils.Res;
using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;

namespace JobVacancy.API.IntegrationTests.PositionTest;

public class PositionControllerTest: IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly Helper _helper;
    private readonly IConfiguration _configuration;
    private readonly string _URL = "/api/v1/Position";
    private readonly ITestOutputHelper _output;

    public PositionControllerTest(CustomWebApplicationFactory factory, ITestOutputHelper output) 
    {
        _client = factory.CreateClient(); 
        _helper = new Helper(_client); 
        _configuration = factory.Configuration;
        _output = output;
    }

    [Fact]
    public async Task Create()
    {
        int num = Random.Shared.Next(1, 1000000);
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        CreatePositionDto dto = new CreatePositionDto()
        {
            Describe = string.Concat(Enumerable.Repeat("AnyDesc", 5)),
            Name = Guid.NewGuid().ToString(),
            IsActive = true
        };

        HttpResponseMessage message = await _client.PostAsJsonAsync(_URL, dto);
        message.StatusCode.Should().Be(HttpStatusCode.Created);
        ResponseHttp<PositionDto>? content = await message.Content.ReadFromJsonAsync<ResponseHttp<PositionDto>>();
        
        content.Should().NotBeNull();
        content.Status.Should().BeTrue();
        content.Message.Should().NotBeNullOrWhiteSpace();
        content.Code.Should().Be((int)HttpStatusCode.Created);
        
        content.Data.Should().NotBeNull();
        content.Data.Id.Should().NotBeNullOrWhiteSpace();
        content.Data.Name.Should().Be(dto.Name);
        content.Data.Describe.Should().Be(dto.Describe);
        content.Data.IsActive.Should().Be(dto.IsActive);
    }

    [Fact]
    public async Task Get()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        PositionDto positionDto = await _helper.CreatePositionAsync();

        HttpResponseMessage message = await _client.GetAsync(_URL+"/"+positionDto.Id);
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<PositionDto>? content = await message.Content.ReadFromJsonAsync<ResponseHttp<PositionDto>>();
        content.Should().NotBeNull();
        content.Status.Should().BeTrue();
        content.Message.Should().NotBeNullOrWhiteSpace();
        content.Code.Should().Be((int)HttpStatusCode.OK);
        
        content.Data.Should().NotBeNull();
        content.Data.Id.Should().Be(positionDto.Id);
        content.Data.Name.Should().Be(positionDto.Name);
    }
    
    [Fact]
    public async Task GetNotFound()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        HttpResponseMessage message = await _client.GetAsync(_URL+"/"+Guid.NewGuid());
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);

        ResponseHttp<object>? content = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        
        content.Should().NotBeNull();
        content.Status.Should().BeFalse();
        content.Message.Should().NotBeNullOrWhiteSpace();
        content.Code.Should().Be((int)HttpStatusCode.NotFound);
        
        content.Data.Should().BeNull();
    }
    
    [Fact]
    public async Task Delete()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        PositionDto positionDto = await _helper.CreatePositionAsync();

        HttpResponseMessage message = await _client.DeleteAsync(_URL+"/"+positionDto.Id);
        message.StatusCode.Should().Be(HttpStatusCode.NoContent);

        ResponseHttp<object>? content = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        content.Should().NotBeNull();
        content.Status.Should().BeTrue();
        content.Message.Should().NotBeNullOrWhiteSpace();
        content.Code.Should().Be((int)HttpStatusCode.NoContent);
        
        content.Data.Should().BeNull();
    }

    [Fact]
    public async Task DeleteNotFound()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        HttpResponseMessage message = await _client.DeleteAsync(_URL+"/"+Guid.NewGuid());
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);

        ResponseHttp<object>? content = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        content.Should().NotBeNull();
        content.Status.Should().BeFalse();
        content.Message.Should().NotBeNullOrWhiteSpace();
        content.Code.Should().Be((int)HttpStatusCode.NotFound);
        
        content.Data.Should().BeNull();
    }

    [Fact]
    public async Task PatchAllFields()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        PositionDto positionDto = await _helper.CreatePositionAsync();

        UpdatePositionDto dto = new UpdatePositionDto()
        {
            Name = Guid.NewGuid().ToString(),
            IsActive = false,
            Describe = "Updated",
        };

        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_URL}/{positionDto.Id}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<PositionDto>? content = await message.Content.ReadFromJsonAsync<ResponseHttp<PositionDto>>();
        content.Should().NotBeNull();
        content.Status.Should().BeTrue();
        content.Message.Should().NotBeNullOrWhiteSpace();
        content.Code.Should().Be((int)HttpStatusCode.OK);
        
        content.Data.Should().NotBeNull();
        content.Data.Id.Should().Be(positionDto.Id);
        content.Data.Name.Should().Be(dto.Name);
        content.Data.Describe.Should().Be(dto.Describe);
        content.Data.IsActive.Should().Be(dto.IsActive.Value);
    }
    
    [Fact]
    public async Task PatchConflictName()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        PositionDto positionDto = await _helper.CreatePositionAsync();
        PositionDto position2 = await _helper.CreatePositionAsync();

        UpdatePositionDto dto = new UpdatePositionDto()
        {
            Name = position2.Name,
        };

        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_URL}/{positionDto.Id}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.Conflict);
        
        ResponseHttp<object>? content = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        content.Should().NotBeNull();
        content.Status.Should().BeFalse();
        content.Message.Should().NotBeNullOrWhiteSpace();
        content.Code.Should().Be((int)HttpStatusCode.Conflict);
        
        content.Data.Should().BeNull();
    }
    
    [Fact]
    public async Task PatchJustName()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        PositionDto positionDto = await _helper.CreatePositionAsync();

        UpdatePositionDto dto = new UpdatePositionDto()
        {
            Name = Guid.NewGuid().ToString(),
        };

        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_URL}/{positionDto.Id}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        
        ResponseHttp<PositionDto>? content = await message.Content.ReadFromJsonAsync<ResponseHttp<PositionDto>>();
        content.Should().NotBeNull();
        content.Status.Should().BeTrue();
        content.Message.Should().NotBeNullOrWhiteSpace();
        content.Code.Should().Be((int)HttpStatusCode.OK);
        
        content.Data.Should().NotBeNull();
        content.Data.Id.Should().Be(positionDto.Id);
        content.Data.Name.Should().Be(dto.Name);
        content.Data.Describe.Should().Be(positionDto.Describe);
        content.Data.IsActive.Should().Be(positionDto.IsActive);
    }
    
    [Fact]
    public async Task PatchJustDescribe()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        PositionDto positionDto = await _helper.CreatePositionAsync();

        UpdatePositionDto dto = new UpdatePositionDto()
        {
            Describe = "Updated",
        };

        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_URL}/{positionDto.Id}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        
        ResponseHttp<PositionDto>? content = await message.Content.ReadFromJsonAsync<ResponseHttp<PositionDto>>();
        content.Should().NotBeNull();
        content.Status.Should().BeTrue();
        content.Message.Should().NotBeNullOrWhiteSpace();
        content.Code.Should().Be((int)HttpStatusCode.OK);
        
        content.Data.Should().NotBeNull();
        content.Data.Id.Should().Be(positionDto.Id);
        content.Data.Name.Should().Be(positionDto.Name);
        content.Data.Describe.Should().Be(dto.Describe);
        content.Data.IsActive.Should().Be(positionDto.IsActive);
    }
    
    [Fact]
    public async Task PatchJustIsActive()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        PositionDto positionDto = await _helper.CreatePositionAsync();

        UpdatePositionDto dto = new UpdatePositionDto()
        {
            IsActive = !positionDto.IsActive,
        };

        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_URL}/{positionDto.Id}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        
        ResponseHttp<PositionDto>? content = await message.Content.ReadFromJsonAsync<ResponseHttp<PositionDto>>();
        content.Should().NotBeNull();
        content.Status.Should().BeTrue();
        content.Message.Should().NotBeNullOrWhiteSpace();
        content.Code.Should().Be((int)HttpStatusCode.OK);
        
        content.Data.Should().NotBeNull();
        content.Data.Id.Should().Be(positionDto.Id);
        content.Data.Name.Should().Be(positionDto.Name);
        content.Data.Describe.Should().Be(positionDto.Describe);
        content.Data.IsActive.Should().Be(dto.IsActive.Value);
    }
    
    [Fact]
    public async Task ExistsByNameTrue()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        PositionDto positionDto = await _helper.CreatePositionAsync();

        HttpResponseMessage message = await _client.GetAsync(_URL+"/"+positionDto.Name+"/exists/name");
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<bool>? content = await message.Content.ReadFromJsonAsync<ResponseHttp<bool>>();
        content.Should().NotBeNull();
        content.Status.Should().BeTrue();
        content.Message.Should().NotBeNullOrWhiteSpace();
        content.Code.Should().Be((int)HttpStatusCode.OK);
        
        content.Data.Should().BeTrue();
    }
    
    [Fact]
    public async Task ExistsByNameFalse()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        HttpResponseMessage message = await _client.GetAsync(_URL+"/"+Guid.NewGuid()+"/exists/name");
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<bool>? content = await message.Content.ReadFromJsonAsync<ResponseHttp<bool>>();
        content.Should().NotBeNull();
        content.Status.Should().BeTrue();
        content.Message.Should().NotBeNullOrWhiteSpace();
        content.Code.Should().Be((int)HttpStatusCode.OK);
        
        content.Data.Should().BeFalse();
    }

    [Fact]
    public async Task GetAll()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        HttpResponseMessage message = await _client.GetAsync(_URL);
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        Page<PositionDto>? page = await message.Content.ReadFromJsonAsync<Page<PositionDto>>();
        page.Should().NotBeNull();
        page.PageIndex.Should().Be(1);
        page.PageSize.Should().Be(10);
    }

    [Fact]
    public async Task GetAllWithFilterJustId()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        PositionDto positionDto = await _helper.CreatePositionAsync();

        
        HttpResponseMessage message = await _client.GetAsync($"{_URL}?Id={positionDto.Id}");
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        
        Page<PositionDto>? page = await message.Content.ReadFromJsonAsync<Page<PositionDto>>();
        page.Should().NotBeNull();
        page.TotalCount.Should().Be(1);
        page.PageIndex.Should().Be(1);
        page.PageSize.Should().Be(10);
        page.Data.Should().NotBeNull();
        page.Data.First().Id.Should().Be(positionDto.Id);
    } 
    
    [Fact]
    public async Task GetAllWithFilterJustName()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        PositionDto positionDto = await _helper.CreatePositionAsync();

        
        HttpResponseMessage message = await _client.GetAsync($"{_URL}?Name={positionDto.Name}");
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        
        Page<PositionDto>? page = await message.Content.ReadFromJsonAsync<Page<PositionDto>>();
        page.Should().NotBeNull();
        page.TotalCount.Should().Be(1);
        page.PageIndex.Should().Be(1);
        page.PageSize.Should().Be(10);
        page.Data.Should().NotBeNull();
        page.Data.First().Id.Should().Be(positionDto.Id);
    }

}
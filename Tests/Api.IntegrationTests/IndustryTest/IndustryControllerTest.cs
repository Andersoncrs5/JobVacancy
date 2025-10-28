

using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using JobVacancy.API.IntegrationTests.Utils;
using JobVacancy.API.models.dtos.Category;
using JobVacancy.API.models.dtos.Industry;
using JobVacancy.API.Utils.Page;
using JobVacancy.API.Utils.Res;
using Microsoft.Extensions.Configuration;

namespace JobVacancy.API.IntegrationTests.IndustryTest;

public class IndustryControllerTest: IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly Helper _helper;
    private readonly IConfiguration _configuration;
    private readonly string _URL = "/api/v1/Industry";

    public IndustryControllerTest(CustomWebApplicationFactory factory) 
    {
        _client = factory.CreateClient(); 
        _helper = new Helper(_client); 
        _configuration = factory.Configuration;
    }
    
    [Fact]
    public async Task Create()
    {
        int num = Random.Shared.Next(1, 1000000);
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        CreateIndustryDto dto = new CreateIndustryDto
        {
            IsActive = true,
            Name = $"Test{num}",
            Description = $"Test description {num}",
        };

        HttpResponseMessage message = await _client.PostAsJsonAsync(_URL, dto);
        message.StatusCode.Should().Be(HttpStatusCode.Created);

        ResponseHttp<CategoryDto>? response = await message.Content.ReadFromJsonAsync<ResponseHttp<CategoryDto>>();
        
        Assert.NotNull(response);
        Assert.NotNull(response.Data);
        Assert.NotNull(response.Data.Id);
        
        response.Data.Name.Should().Be(dto.Name);
        response.Data.Description.Should().Be(dto.Description);
        response.Data.IsActive.Should().Be(dto.IsActive);
    }
    
    [Fact]
    public async Task CreateIndustryThrowConflictName()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto existingIndustry = await _helper.CreateIndustry(master);
    
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        CreateIndustryDto dto = new CreateIndustryDto 
        {
            IsActive = true,
            Name = existingIndustry.Name, 
            Description = $"Test description",
        };
    
        HttpResponseMessage message = await _client.PostAsJsonAsync("api/v1/Industry", dto);
    
        message.StatusCode.Should().Be(HttpStatusCode.Conflict);
    
        ResponseHttp<object>? response = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
    
        Assert.NotNull(response);
        Assert.Null(response.Data);
        Assert.False(response.Status);
        response.Message.Should().Be("Name is taken");
        response.Code.Should().Be(409); 
    }
    
    [Fact]
    public async Task CreateThrowForbidden()
    {
        UserResultTest user = await _helper.CreateAndGetUser();
        
        int num = Random.Shared.Next(1, 1000000);
        string token = user.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        CreateIndustryDto dto = new CreateIndustryDto
        {
            IsActive = true,
            Name = $"Test{num}",
            Description = $"Test description {num}",
        };

        HttpResponseMessage message = await _client.PostAsJsonAsync("api/v1/Category", dto);
        message.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    } 
    
    [Fact]
    public async Task Get()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        HttpResponseMessage message = await _client.GetAsync(_URL+"/"+industryDto.Id);
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        ResponseHttp<IndustryDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<IndustryDto>>();
        http.Should().NotBeNull();
        http.Data.Should().NotBeNull();
        
        http.Data.Id.Should().Be(industryDto.Id);
        http.Data.Name.Should().Be(industryDto.Name);
    }
    
    [Fact]
    public async Task GetNotFound()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);

        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        HttpResponseMessage message = await _client.GetAsync(_URL+"/"+Guid.NewGuid().ToString());
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);
        
        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        
        http!.Code.Should().Be(404);
        
        http.Data.Should().BeNull();
    }
    
    [Fact]
    public async Task DeleteThrowForbidden()
    {
        UserResultTest user = await _helper.CreateAndGetUser();
        
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);
        
        int num = Random.Shared.Next(1, 1000000);
        string token = user.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        HttpResponseMessage message = await _client.DeleteAsync(_URL+"/"+industryDto.Id);
        message.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task Delete()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        HttpResponseMessage message = await _client.DeleteAsync(_URL+"/"+industryDto.Id);
        message.StatusCode.Should().Be(HttpStatusCode.NoContent);
        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        
        http.Code.Should().Be((int)HttpStatusCode.NoContent);
        
        http.Data.Should().BeNull();
        http.Message.Should().Be("Industry deleted with success.");
        http.Code.Should().Be(204);
    }
    
    [Fact]
    public async Task DeleteNotFound()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
    
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        HttpResponseMessage message = await _client.DeleteAsync(_URL + "/" +  Guid.NewGuid().ToString());
    
        message.StatusCode.Should().Be(HttpStatusCode.NotFound); 

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
    
        http.Should().NotBeNull();
        http!.Code.Should().Be(404);
        http.Data.Should().BeNull();
        http.Message.Should().Contain("Industry not found");
    }
    
    [Fact]
    public async Task ExistsByName()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        HttpResponseMessage message = await _client.GetAsync(_URL+"/"+industryDto.Name+"/exists/name");
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        ResponseHttp<bool>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<bool>>();
        http.Should().NotBeNull();
        http.Data.Should().Be(true);
    }
    
    [Fact]
    public async Task ExistsByNameReturnFalse()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);

        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        HttpResponseMessage message = await _client.GetAsync(_URL+"/anyname/exists/name");
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        ResponseHttp<bool>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<bool>>();
        http.Should().NotBeNull();
        http.Data.Should().Be(false);
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
    }

    [Fact]
    public async Task GetAllSingleByNameAndIsActive()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        HttpResponseMessage message = await _client.GetAsync($"{_URL}?Name={industryDto.Name}&IsActive={industryDto.IsActive}");
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        
        Page<CategoryDto>? http = await message.Content.ReadFromJsonAsync<Page<CategoryDto>>();
        http.Should().NotBeNull();
        http.Data.Should().NotBeNull();
        http.Data.First().Id.Should().Be(industryDto.Id);
    }
    
    [Fact]
    public async Task Update()
    {
        int num = Random.Shared.Next(1, 100000000);
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        UpdateIndustryDto dto = new UpdateIndustryDto
        {
            Description = "Updated Description",
            Name = $"Updated Name {num}",
            IsActive = false,
        };
        
        HttpResponseMessage message = await _client.PatchAsJsonAsync(_URL+"/"+industryDto.Id, dto);
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        ResponseHttp<IndustryDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<IndustryDto>>();
        http.Should().NotBeNull();
        http.Data.Should().NotBeNull();
        
        http.Data.Id.Should().Be(industryDto.Id);
        http.Data.Name.Should().Be(dto.Name);
        http.Data.Description.Should().Be(dto.Description);
        http.Data.IsActive.Should().Be(false);
    }
    
    [Fact]
    public async Task UpdateJustName()
    {
        int num = Random.Shared.Next(1, 100000000);
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master); 
        
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        UpdateIndustryDto dto = new UpdateIndustryDto
        {
            Name = $"Updated Name {num}",
        };
        
        HttpResponseMessage message = await _client.PatchAsJsonAsync(_URL+"/"+industryDto.Id, dto);
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        ResponseHttp<IndustryDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<IndustryDto>>();
        http.Should().NotBeNull();
        http.Data.Should().NotBeNull();
        
        http.Data.Name.Should().Be(dto.Name);
        
        http.Data.Id.Should().Be(industryDto.Id);
        http.Data.Description.Should().Be(industryDto.Description);
    }

    [Fact]
    public async Task UpdateJustDescription()
    {
        int num = Random.Shared.Next(1, 100000000);
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        UpdateIndustryDto dto = new UpdateIndustryDto
        {
            Description = "Any Description " + num
        };
        
        HttpResponseMessage message = await _client.PatchAsJsonAsync(_URL+"/"+industryDto.Id, dto);
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        ResponseHttp<IndustryDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<IndustryDto>>();
        http.Should().NotBeNull();
        http.Data.Should().NotBeNull();
        
        http.Data.Description.Should().Be(dto.Description);
        
        http.Data.Id.Should().Be(industryDto.Id);
        http.Data.Name.Should().Be(industryDto.Name);
    }

    [Fact]
    public async Task UpdateJustIsActive()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master); 

        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        UpdateIndustryDto dto = new UpdateIndustryDto
        {
            IsActive = !industryDto.IsActive
        };
        
        HttpResponseMessage message = await _client.PatchAsJsonAsync(_URL+"/"+industryDto.Id, dto);
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        ResponseHttp<IndustryDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<IndustryDto>>();
        http.Should().NotBeNull();
        http.Data.Should().NotBeNull();
        
        http.Data.IsActive.Should().Be(!industryDto.IsActive);
        
        http.Data.Id.Should().Be(industryDto.Id);
        http.Data.Name.Should().Be(industryDto.Name);
        http.Data.Description.Should().Be(industryDto.Description);
    }
    
   [Fact]
    public async Task UpdateThrowForb()
    {
        int num = Random.Shared.Next(1, 100000000);
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);
        
        UserResultTest user = await _helper.CreateAndGetUser();

        string token = user.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        UpdateIndustryDto dto = new UpdateIndustryDto
        {
            Description = "Updated Description",
            Name = $"Updated Name {num}",
        };
        
        HttpResponseMessage message = await _client.PatchAsJsonAsync(_URL+"/"+industryDto.Id, dto);
        message.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task UpdateThrowConflict()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDtoA = await _helper.CreateIndustry(master); 
        IndustryDto industryDtoB = await _helper.CreateIndustry(master); 

        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        UpdateIndustryDto dto = new UpdateIndustryDto
        {
            Description = "Updated Description",
            Name = industryDtoB.Name,
        };
        
        HttpResponseMessage message = await _client.PatchAsJsonAsync(_URL+"/"+industryDtoA.Id, dto);
        message.StatusCode.Should().Be(HttpStatusCode.Conflict);
        
        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Data.Should().BeNull();
        http.Code.Should().Be(409);
        http.Message.Should().NotBeNull();
        http.Message.Should().Be("Industry name already exists");
    }
    
}
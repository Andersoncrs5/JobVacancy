using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using JobVacancy.API.IntegrationTests.Utils;
using JobVacancy.API.models.dtos.Category;
using JobVacancy.API.Utils.Page;
using JobVacancy.API.Utils.Res;
using Microsoft.Extensions.Configuration;

namespace JobVacancy.API.IntegrationTests.CategoryTests;

public class CategoryControllerTest: IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly Helper _helper;
    private readonly IConfiguration _configuration;
    private readonly string _URL = "/api/v1/Category";

    public CategoryControllerTest(CustomWebApplicationFactory factory) 
    {
        _client = factory.CreateClient(); 
        _helper = new Helper(_client); 
        _configuration = factory.Configuration;
    }

    [Fact]
    public async Task Get()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        CategoryDto categoryDto = await _helper.CreateCategory(master);

        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        HttpResponseMessage message = await _client.GetAsync(_URL+"/"+categoryDto.Id);
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        ResponseHttp<CategoryDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<CategoryDto>>();
        http.Should().NotBeNull();
        http.Data.Should().NotBeNull();
        
        http.Data.Id.Should().Be(categoryDto.Id);
        http.Data.Name.Should().Be(categoryDto.Name);
    }
    
    [Fact]
    public async Task GetAll()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        CategoryDto categoryDto = await _helper.CreateCategory(master);

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
        
        CategoryDto categoryDto = await _helper.CreateCategory(master);

        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        HttpResponseMessage message = await _client.GetAsync($"{_URL}?Name={categoryDto.Name}&IsActive={categoryDto.IsActive}");
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        
        Page<CategoryDto>? http = await message.Content.ReadFromJsonAsync<Page<CategoryDto>>();
        http.Should().NotBeNull();
        http.Data.Should().NotBeNull();
        http.Data.First().Id.Should().Be(categoryDto.Id);
    }
    
    [Fact]
    public async Task DeleteCategoryThrowForbidden()
    {
        UserResultTest user = await _helper.CreateAndGetUser();
        
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        CategoryDto categoryDto = await _helper.CreateCategory(master);
        
        int num = Random.Shared.Next(1, 1000000);
        string token = user.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        HttpResponseMessage message = await _client.DeleteAsync(_URL+"/"+categoryDto.Id);
        message.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task Delete()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        CategoryDto categoryDto = await _helper.CreateCategory(master);

        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        HttpResponseMessage message = await _client.DeleteAsync(_URL+"/"+categoryDto.Id);
        message.StatusCode.Should().Be(HttpStatusCode.NoContent);
        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        
        http.Code.Should().Be((int)HttpStatusCode.NoContent);
        
        http.Data.Should().BeNull();
    }
    
    [Fact]
    public async Task DeleteCategoryNotFound()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
    
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        string nonExistentId = Guid.NewGuid().ToString();
        HttpResponseMessage message = await _client.DeleteAsync(_URL + "/" + nonExistentId);
    
        message.StatusCode.Should().Be(HttpStatusCode.NotFound); 

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
    
        http.Should().NotBeNull();
        http!.Code.Should().Be(404);
        http.Data.Should().BeNull();
        http.Message.Should().Contain("Category not found");
    }
    
    [Fact]
    public async Task GetCategoryNotFound()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
    
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        string nonExistentId = Guid.NewGuid().ToString();
        HttpResponseMessage message = await _client.GetAsync(_URL + "/" + nonExistentId);
    
        message.StatusCode.Should().Be(HttpStatusCode.NotFound); 

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
    
        http.Should().NotBeNull();
        http!.Code.Should().Be(404);
        http.Data.Should().BeNull();
        http.Message.Should().Contain("Category not found");
    }
    
    [Fact]
    public async Task CreateCategory()
    {
        int num = Random.Shared.Next(1, 1000000);
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        CreateCategoryDto dto = new CreateCategoryDto
        {
            IsActive = true,
            Name = $"Test{num}",
            Description = $"Test description {num}",
        };

        HttpResponseMessage message = await _client.PostAsJsonAsync("api/v1/Category", dto);
        message.StatusCode.Should().Be(HttpStatusCode.Created);

        ResponseHttp<CategoryDto>? response = await message.Content.ReadFromJsonAsync<ResponseHttp<CategoryDto>>();
        
        Assert.NotNull(response);
        Assert.NotNull(response.Data);
        Assert.NotNull(response.Data.Id);
    }
    
    [Fact]
    public async Task CreateCategoryThrowConflictName()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        CategoryDto categoryDto = await _helper.CreateCategory(master);
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);


        CreateCategoryDto dto = new CreateCategoryDto
        {
            IsActive = true,
            Name = categoryDto.Name,
            Description = $"Test description",
        };
        HttpResponseMessage message = await _client.PostAsJsonAsync("api/v1/Category", dto);
        message.StatusCode.Should().Be(HttpStatusCode.Conflict);

        ResponseHttp<object>? response = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        
        Assert.NotNull(response);
        Assert.Null(response.Data);
        Assert.False(response.Status);
        response.Message.Should().Be("Name is taken");
    }
    
    [Fact]
    public async Task CreateCategoryThrowForbidden()
    {
        UserResultTest user = await _helper.CreateAndGetUser();
        
        int num = Random.Shared.Next(1, 1000000);
        string token = user.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        CreateCategoryDto dto = new CreateCategoryDto
        {
            IsActive = true,
            Name = $"Test{num}",
            Description = $"Test description {num}",
        };

        HttpResponseMessage message = await _client.PostAsJsonAsync("api/v1/Category", dto);
        message.StatusCode.Should().Be(HttpStatusCode.Forbidden);

        
    } 
    
}
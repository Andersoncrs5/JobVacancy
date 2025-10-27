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
    public async Task Update()
    {
        int num = Random.Shared.Next(1, 100000000);
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        CategoryDto categoryDto = await _helper.CreateCategory(master);

        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        UpdateCategoryDto dto = new UpdateCategoryDto
        {
            Description = "Updated Description",
            Name = $"Updated Name {num}",
        };
        
        HttpResponseMessage message = await _client.PatchAsJsonAsync(_URL+"/"+categoryDto.Id, dto);
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        ResponseHttp<CategoryDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<CategoryDto>>();
        http.Should().NotBeNull();
        http.Data.Should().NotBeNull();
        
        http.Data.Id.Should().Be(categoryDto.Id);
        http.Data.Name.Should().Be(dto.Name);
        http.Data.Description.Should().Be(dto.Description);
    }
    
    [Fact]
    public async Task UpdateJustName()
    {
        int num = Random.Shared.Next(1, 100000000);
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        CategoryDto categoryDto = await _helper.CreateCategory(master);

        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        UpdateCategoryDto dto = new UpdateCategoryDto
        {
            Name = $"Updated Name {num}",
        };
        
        HttpResponseMessage message = await _client.PatchAsJsonAsync(_URL+"/"+categoryDto.Id, dto);
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        ResponseHttp<CategoryDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<CategoryDto>>();
        http.Should().NotBeNull();
        http.Data.Should().NotBeNull();
        
        http.Data.Id.Should().Be(categoryDto.Id);
        http.Data.Name.Should().Be(dto.Name);
        http.Data.Description.Should().Be(categoryDto.Description);
    }
    
    [Fact]
    public async Task UpdateJustDescription()
    {
        int num = Random.Shared.Next(1, 100000000);
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        CategoryDto categoryDto = await _helper.CreateCategory(master);

        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        UpdateCategoryDto dto = new UpdateCategoryDto
        {
            Description = "Any " + num
        };
        
        HttpResponseMessage message = await _client.PatchAsJsonAsync(_URL+"/"+categoryDto.Id, dto);
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        ResponseHttp<CategoryDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<CategoryDto>>();
        http.Should().NotBeNull();
        http.Data.Should().NotBeNull();
        
        http.Data.Id.Should().Be(categoryDto.Id);
        http.Data.Name.Should().Be(categoryDto.Name);
        http.Data.Description.Should().Be(dto.Description);
    }
    
    [Fact]
    public async Task UpdateThrowForb()
    {
        int num = Random.Shared.Next(1, 100000000);
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        UserResultTest user = await _helper.CreateAndGetUser();

        CategoryDto categoryDto = await _helper.CreateCategory(master);

        string token = user.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        UpdateCategoryDto dto = new UpdateCategoryDto
        {
            Description = "Updated Description",
            Name = $"Updated Name {num}",
        };
        
        HttpResponseMessage message = await _client.PatchAsJsonAsync(_URL+"/"+categoryDto.Id, dto);
        message.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task UpdateThrowConflict()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        CategoryDto categoryDto = await _helper.CreateCategory(master);
        CategoryDto categoryDtoB = await _helper.CreateCategory(master);

        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        UpdateCategoryDto dto = new UpdateCategoryDto
        {
            Description = "Updated Description",
            Name = categoryDtoB.Name,
        };
        
        HttpResponseMessage message = await _client.PatchAsJsonAsync(_URL+"/"+categoryDto.Id, dto);
        message.StatusCode.Should().Be(HttpStatusCode.Conflict);
        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Data.Should().BeNull();
        http.Code.Should().Be(409);
        http.Message.Should().NotBeNull();
        http.Message.Should().Be("Category name already exists");
    }
    
    [Fact]
    public async Task ExistsByName()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        CategoryDto categoryDto = await _helper.CreateCategory(master);

        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        HttpResponseMessage message = await _client.GetAsync(_URL+"/"+categoryDto.Name+"/exists/name");
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        ResponseHttp<bool>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<bool>>();
        http.Should().NotBeNull();
        http.Data.Should().Be(true);
    }
    
    [Fact]
    public async Task ExistsByNameReturnFalse()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        CategoryDto categoryDto = await _helper.CreateCategory(master);

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
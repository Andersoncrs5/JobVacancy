using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using JobVacancy.API.models.dtos.Category;
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
}
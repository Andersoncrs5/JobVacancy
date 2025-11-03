using System.Collections.Immutable;
using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using JobVacancy.API.IntegrationTests.Utils;
using JobVacancy.API.models.dtos.Category;
using JobVacancy.API.models.dtos.Enterprise;
using JobVacancy.API.models.dtos.FavoritePostEnterprise;
using JobVacancy.API.models.dtos.Industry;
using JobVacancy.API.models.dtos.PostEnterprise;
using JobVacancy.API.Utils.Page;
using JobVacancy.API.Utils.Res;
using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;

namespace JobVacancy.API.IntegrationTests.FavoritePostEnterprise;

public class FavoritePostEnterpriseControllerTest: IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly Helper _helper;
    private readonly IConfiguration _configuration;
    private readonly string _url = "/api/v1/FavoritePostEnterprise";
    private readonly ITestOutputHelper _output;

    public FavoritePostEnterpriseControllerTest(CustomWebApplicationFactory factory, ITestOutputHelper output) 
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
        CategoryDto categoryDto = await _helper.CreateCategory(master);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        var loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        string token = loginUserWithNewRole.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        PostEnterpriseDto post = await _helper.CreatePostEnterprise(categoryDto);
        
        UserResultTest userB = await _helper.CreateAndGetUser();
        token = userB.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage message = await _client.PostAsync($"{_url}/{post.Id}/Toggle", null);
        message.StatusCode.Should().Be(HttpStatusCode.Created);

        ResponseHttp<FavoritePostEnterpriseDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<FavoritePostEnterpriseDto>>();
        http.Should().NotBeNull();
        http.Status.Should().BeTrue();
        
        http.Data.Should().NotBeNull();
        http.Data.Id.Should().NotBeNullOrWhiteSpace();
        http.Data.PostEnterpriseId.Should().Be(post.Id);
        http.Data.UserId.Should().Be(userB.User!.Id);
    }
    
    [Fact]
    public async Task CreateNotFoundPost()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        CategoryDto categoryDto = await _helper.CreateCategory(master);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        var loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        string token = loginUserWithNewRole.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        PostEnterpriseDto post = await _helper.CreatePostEnterprise(categoryDto);
        
        UserResultTest userB = await _helper.CreateAndGetUser();
        token = userB.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage message = await _client.PostAsync($"{_url}/{Guid.NewGuid()}/Toggle", null);
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Status.Should().BeFalse();
        http.Code.Should().Be(404);
        
        http.Data.Should().BeNull();
    }

    [Fact]
    public async Task Del()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        CategoryDto categoryDto = await _helper.CreateCategory(master);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        var loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        string token = loginUserWithNewRole.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        PostEnterpriseDto post = await _helper.CreatePostEnterprise(categoryDto);
        
        UserResultTest userB = await _helper.CreateAndGetUser();
        token = userB.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        FavoritePostEnterpriseDto dto = await _helper.AddFavoritePostEnterprise(post);
        
        HttpResponseMessage message = await _client.PostAsync($"{_url}/{post.Id}/Toggle", null);
        message.StatusCode.Should().Be(HttpStatusCode.NoContent);
        
        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Message.Should().NotBeNull();
        http.Status.Should().BeTrue();
        http.Code.Should().Be(204);
        
        http.Data.Should().BeNull();
    }

    [Fact]
    public async Task GetAll()
    {
        UserResultTest userB = await _helper.CreateAndGetUser();
        string token = userB.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage message = await _client.GetAsync($"{_url}");
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        Page<FavoritePostEnterpriseDto>? http = await message.Content.ReadFromJsonAsync<Page<FavoritePostEnterpriseDto>>();
        http.Should().NotBeNull();
        http.Data.Should().NotBeNull();
    }
    
    [Fact]
    public async Task GetAllFilterByUserParams()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        CategoryDto categoryDto = await _helper.CreateCategory(master);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        var loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        string token = loginUserWithNewRole.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        PostEnterpriseDto post = await _helper.CreatePostEnterprise(categoryDto);
        
        UserResultTest userB = await _helper.CreateAndGetUser();
        token = userB.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        FavoritePostEnterpriseDto dto = await _helper.AddFavoritePostEnterprise(post);
        
        HttpResponseMessage message = await _client.GetAsync($"{_url}?" +
                                                             $"UserId={userB.User!.Id}" +
                                                             $"&Username={userB.User.Username}" +
                                                             $"&Fullname={userB.User.FullName}" +
                                                             $"&Email={userB.User.Email}");
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        Page<FavoritePostEnterpriseDto>? http = await message.Content.ReadFromJsonAsync<Page<FavoritePostEnterpriseDto>>();
        http.Should().NotBeNull();
        http.Data.Should().NotBeNull();
        http.Data.First().Should().NotBeNull();
        http.Data.First().Id.Should().Be(dto.Id);
    }
    
    [Fact]
    public async Task GetAllFilterByPostParams()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        CategoryDto categoryDto = await _helper.CreateCategory(master);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        var loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        string token = loginUserWithNewRole.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        PostEnterpriseDto post = await _helper.CreatePostEnterprise(categoryDto);
        
        UserResultTest userB = await _helper.CreateAndGetUser();
        token = userB.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        FavoritePostEnterpriseDto dto = await _helper.AddFavoritePostEnterprise(post);
        
        HttpResponseMessage message = await _client.GetAsync($"{_url}?" +
                                                             $"PostId={post!.Id}" +
                                                             $"&Title={post.Title}" +
                                                             $"&ReadingTimeMinutesMin={post.ReadingTimeMinutes - 1}" +
                                                             $"&ReadingTimeMinutesMax={post.ReadingTimeMinutes + 1}" +
                                                             $"&CategoryId={post.CategoryId}");
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        Page<FavoritePostEnterpriseDto>? http = await message.Content.ReadFromJsonAsync<Page<FavoritePostEnterpriseDto>>();
        http.Should().NotBeNull();
        http.Data.Should().NotBeNull();
        http.Data.First().Should().NotBeNull();
        http.Data.First().Id.Should().Be(dto.Id);
    }

    [Fact]
    public async Task ExistsReturnTrue()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        CategoryDto categoryDto = await _helper.CreateCategory(master);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        var loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        string token = loginUserWithNewRole.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        PostEnterpriseDto post = await _helper.CreatePostEnterprise(categoryDto);
        
        UserResultTest userB = await _helper.CreateAndGetUser();
        token = userB.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        FavoritePostEnterpriseDto dto = await _helper.AddFavoritePostEnterprise(post);
        
        HttpResponseMessage message = await _client.GetAsync($"{_url}/{post.Id}/exists");
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        
        ResponseHttp<bool>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<bool>>();
        http.Should().NotBeNull();
        http.Code.Should().Be(200);
        http.Status.Should().BeTrue();
        
        http.Data.Should().BeTrue();
        
    }
    
    [Fact]
    public async Task ExistsReturnFalse()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        CategoryDto categoryDto = await _helper.CreateCategory(master);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        var loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        string token = loginUserWithNewRole.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        PostEnterpriseDto post = await _helper.CreatePostEnterprise(categoryDto);
        
        UserResultTest userB = await _helper.CreateAndGetUser();
        token = userB.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        HttpResponseMessage message = await _client.GetAsync($"{_url}/{post.Id}/exists");
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        
        ResponseHttp<bool>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<bool>>();
        http.Should().NotBeNull();
        http.Code.Should().Be(200);
        http.Status.Should().BeTrue();
        
        http.Data.Should().BeFalse();
        
    }
    
}
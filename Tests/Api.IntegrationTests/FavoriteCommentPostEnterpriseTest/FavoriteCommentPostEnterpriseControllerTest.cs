using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using JobVacancy.API.IntegrationTests.Utils;
using JobVacancy.API.models.dtos.Category;
using JobVacancy.API.models.dtos.CommentPostEnterprise;
using JobVacancy.API.models.dtos.FavoriteCommentPostEnterprise;
using JobVacancy.API.models.dtos.Industry;
using JobVacancy.API.models.dtos.PostEnterprise;
using JobVacancy.API.Utils.Page;
using JobVacancy.API.Utils.Res;
using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;

namespace JobVacancy.API.IntegrationTests.FavoriteCommentPostEnterpriseTest;

public class FavoriteCommentPostEnterpriseControllerTest: IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly Helper _helper;
    private readonly IConfiguration _configuration;
    private readonly string _url = "/api/v1/FavoriteCommentPostEnterprise";
    private readonly ITestOutputHelper _output;

    public FavoriteCommentPostEnterpriseControllerTest(CustomWebApplicationFactory factory, ITestOutputHelper output) 
    {
        _client = factory.CreateClient(); 
        _helper = new Helper(_client); 
        _configuration = factory.Configuration;
        _output = output;
    }
    
    [Fact]
    public async Task Del()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        CategoryDto categoryDto = await _helper.CreateCategory(master);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        await _helper.CreateEnterprise(user, industryDto);
        
        var loginUserWithNewRole = await _helper.LoginUser(user.User!.Email!, user.CreateUser!.PasswordHash);
        
        string token = loginUserWithNewRole.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        PostEnterpriseDto post = await _helper.CreatePostEnterprise(categoryDto);
        
        CommentPostEnterpriseDto comment = await _helper.CreateCommentPostEnterpriseDto(post.Id, true);

        await _helper.AddFavoriteCommentPostEnterprise(comment.Id);
        
        HttpResponseMessage message = await _client.PostAsync($"{_url}/{comment.Id}/Toggle", null);
        message.StatusCode.Should().Be(HttpStatusCode.NoContent);

        ResponseHttp<object>? content = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        
        content.Should().NotBeNull();
        content.Data.Should().BeNull();
        content.Message.Should().NotBeNullOrWhiteSpace();
        content.Code.Should().Be((int)HttpStatusCode.NoContent);
        content.Status.Should().BeTrue();
    }
    
    [Fact]
    public async Task Create()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        CategoryDto categoryDto = await _helper.CreateCategory(master);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        await _helper.CreateEnterprise(user, industryDto);
        
        var loginUserWithNewRole = await _helper.LoginUser(user.User!.Email!, user.CreateUser!.PasswordHash);
        
        string token = loginUserWithNewRole.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        PostEnterpriseDto post = await _helper.CreatePostEnterprise(categoryDto);
        
        CommentPostEnterpriseDto comment = await _helper.CreateCommentPostEnterpriseDto(post.Id, true);
        
        HttpResponseMessage message = await _client.PostAsync($"{_url}/{comment.Id}/Toggle", null);
        message.StatusCode.Should().Be(HttpStatusCode.Created);

        ResponseHttp<object>? content = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        
        content.Should().NotBeNull();
        content.Data.Should().BeNull();
        content.Code.Should().Be((int)HttpStatusCode.Created);
        content.Status.Should().BeTrue();
    }
    
    [Fact]
    public async Task CreateNotFoundComment()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        CategoryDto categoryDto = await _helper.CreateCategory(master);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        await _helper.CreateEnterprise(user, industryDto);
        
        var loginUserWithNewRole = await _helper.LoginUser(user.User!.Email!, user.CreateUser!.PasswordHash);
        
        string token = loginUserWithNewRole.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        PostEnterpriseDto post = await _helper.CreatePostEnterprise(categoryDto);
        
        HttpResponseMessage message = await _client.PostAsync($"{_url}/{Guid.NewGuid()}/Toggle", null);
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);

        ResponseHttp<object>? content = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        
        content.Should().NotBeNull();
        content.Data.Should().BeNull();
        content.Code.Should().Be((int)HttpStatusCode.NotFound);
        content.Status.Should().BeFalse();
    }
    
    [Fact]
    public async Task ExistsTrue()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        CategoryDto categoryDto = await _helper.CreateCategory(master);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        await _helper.CreateEnterprise(user, industryDto);
        
        var loginUserWithNewRole = await _helper.LoginUser(user.User!.Email!, user.CreateUser!.PasswordHash);
        
        string token = loginUserWithNewRole.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        PostEnterpriseDto post = await _helper.CreatePostEnterprise(categoryDto);
        
        CommentPostEnterpriseDto comment = await _helper.CreateCommentPostEnterpriseDto(post.Id, true);

        await _helper.AddFavoriteCommentPostEnterprise(comment.Id);
        
        HttpResponseMessage message = await _client.GetAsync($"{_url}/{comment.Id}/Exists");
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<bool>? content = await message.Content.ReadFromJsonAsync<ResponseHttp<bool>>();
        
        content.Should().NotBeNull();
        content.Code.Should().Be((int)HttpStatusCode.OK);
        content.Status.Should().BeTrue();
        
        content.Data.Should().BeTrue();
    }
    
    [Fact]
    public async Task ExistsFalse()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        CategoryDto categoryDto = await _helper.CreateCategory(master);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        await _helper.CreateEnterprise(user, industryDto);
        
        var loginUserWithNewRole = await _helper.LoginUser(user.User!.Email!, user.CreateUser!.PasswordHash);
        
        string token = loginUserWithNewRole.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        PostEnterpriseDto post = await _helper.CreatePostEnterprise(categoryDto);
        
        CommentPostEnterpriseDto comment = await _helper.CreateCommentPostEnterpriseDto(post.Id, true);
        
        HttpResponseMessage message = await _client.GetAsync($"{_url}/{comment.Id}/Exists");
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<bool>? content = await message.Content.ReadFromJsonAsync<ResponseHttp<bool>>();
        
        content.Should().NotBeNull();
        content.Code.Should().Be((int)HttpStatusCode.OK);
        content.Status.Should().BeTrue();
        
        content.Data.Should().BeFalse();
    }
     
    [Fact]
    public async Task ExistsNotFoundComment()
    {
        UserResultTest user = await _helper.CreateAndGetUser();
        string token = user.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        HttpResponseMessage message = await _client.GetAsync($"{_url}/{Guid.NewGuid()}/Exists");
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);

        ResponseHttp<object>? content = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        
        content.Should().NotBeNull();
        content.Code.Should().Be((int)HttpStatusCode.NotFound);
        content.Status.Should().BeFalse();
        content.Message.Should().NotBeNullOrWhiteSpace();

        content.Data.Should().BeNull();
    }
    
    [Fact]
    public async Task GetAll()
    {
        UserResultTest user = await _helper.CreateAndGetUser();
        string token = user.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage message = await _client.GetAsync($"{_url}");
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        _output.WriteLine(message.Content.ReadAsStringAsync().Result);

        Page<FavoriteCommentPostEnterpriseDto>? page = await message.Content.ReadFromJsonAsync<Page<FavoriteCommentPostEnterpriseDto>>();
        page.Should().NotBeNull();
        page.PageIndex.Should().Be(1);
        page.PageSize.Should().Be(10);
    }
    
}
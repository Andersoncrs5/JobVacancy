using System.Net;
using System.Net.Http.Json;
using JobVacancy.API.IntegrationTests.Utils;
using JobVacancy.API.models.dtos.Category;
using JobVacancy.API.models.dtos.CommentPostUser;
using JobVacancy.API.models.dtos.PostUser;
using JobVacancy.API.Utils.Res;
using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;
using FluentAssertions;
using JobVacancy.API.models.dtos.FavoriteCommentPostUser;
using JobVacancy.API.Utils.Page;

namespace JobVacancy.API.IntegrationTests.FavoriteCommentPostUserTest;

public class FavoriteCommentPostUserControllerTest: IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly Helper _helper;
    private readonly IConfiguration _configuration;
    private readonly string _url = "/api/v1/FavoriteCommentPostUser";
    private readonly ITestOutputHelper _output;

    public FavoriteCommentPostUserControllerTest(CustomWebApplicationFactory factory, ITestOutputHelper output) 
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
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        CategoryDto categoryDto = await _helper.CreateCategory(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        token = user.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        PostUserDto postUser = await _helper.CreatePostUser(categoryDto, user.User!.Id);

        CommentPostUserDto comment = await _helper.CreateComment(postUser);
        
        await _helper.AddFavoriteCommentPostUser(comment.Id);
        
        HttpResponseMessage message = await _client.PostAsync($"{_url}/{comment.Id}/Toggle", null);
        message.StatusCode.Should().Be(HttpStatusCode.NoContent);

        ResponseHttp<object>? content = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        
        content.Should().NotBeNull();
        content.Data.Should().BeNull();
        content.Code.Should().Be((int)HttpStatusCode.NoContent);
        content.Status.Should().BeTrue();
    }
    
    [Fact]
    public async Task ExistsTrue()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        CategoryDto categoryDto = await _helper.CreateCategory(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        token = user.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        PostUserDto postUser = await _helper.CreatePostUser(categoryDto, user.User!.Id);

        CommentPostUserDto comment = await _helper.CreateComment(postUser);
        
        await _helper.AddFavoriteCommentPostUser(comment.Id);
        
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
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        CategoryDto categoryDto = await _helper.CreateCategory(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        token = user.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        PostUserDto postUser = await _helper.CreatePostUser(categoryDto, user.User!.Id);

        CommentPostUserDto comment = await _helper.CreateComment(postUser);
        
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
    public async Task Create()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        CategoryDto categoryDto = await _helper.CreateCategory(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        token = user.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        PostUserDto postUser = await _helper.CreatePostUser(categoryDto, user.User!.Id);

        CommentPostUserDto comment = await _helper.CreateComment(postUser);
        
        HttpResponseMessage message = await _client.PostAsync($"{_url}/{comment.Id}/Toggle", null);
        message.StatusCode.Should().Be(HttpStatusCode.Created);

        ResponseHttp<object>? content = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        
        content.Should().NotBeNull();
        content.Data.Should().BeNull();
        content.Code.Should().Be((int)HttpStatusCode.Created);
        content.Status.Should().BeTrue();
    }
    
    [Fact]
    public async Task CreateNotFound()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        CategoryDto categoryDto = await _helper.CreateCategory(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        token = user.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        PostUserDto postUser = await _helper.CreatePostUser(categoryDto, user.User!.Id);

        CommentPostUserDto comment = await _helper.CreateComment(postUser);
        
        HttpResponseMessage message = await _client.PostAsync($"{_url}/{Guid.NewGuid()}/Toggle", null);
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);

        ResponseHttp<object>? content = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        
        content.Should().NotBeNull();
        content.Data.Should().BeNull();
        content.Code.Should().Be((int)HttpStatusCode.NotFound);
        content.Status.Should().BeFalse();
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

        Page<FavoriteCommentPostUserDto>? page = await message.Content.ReadFromJsonAsync<Page<FavoriteCommentPostUserDto>>();
        page.Should().NotBeNull();
        page.PageIndex.Should().Be(1);
        page.PageSize.Should().Be(10);
    }
    
}
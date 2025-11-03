using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using JobVacancy.API.IntegrationTests.Utils;
using JobVacancy.API.models.dtos.Category;
using JobVacancy.API.models.dtos.FavoritePost;
using JobVacancy.API.models.dtos.PostUser;
using JobVacancy.API.Utils.Page;
using JobVacancy.API.Utils.Res;
using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;

namespace JobVacancy.API.IntegrationTests.FavoritePostUser;

public class FavoritePostUserControllerTest: IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly Helper _helper;
    private readonly IConfiguration _configuration;
    private readonly string _url = "/api/v1/FavoritePostUser";
    private readonly ITestOutputHelper _output;

    public FavoritePostUserControllerTest(CustomWebApplicationFactory factory, ITestOutputHelper output) 
    {
        _client = factory.CreateClient(); 
        _helper = new Helper(_client); 
        _configuration = factory.Configuration;
        _output = output;
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
    }
    
    [Fact]
    public async Task GetAllFilterByUser()
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
        FavoritePostUserDto favoritePost = await _helper.AddFavoritePost(postUser);

        HttpResponseMessage message = await _client.GetAsync($"{_url}?" +
                                                             $"UserId={postUser.UserId}" +
                                                             $"&Username={user.User.Username}" +
                                                             $"&Fullname={user.User.FullName}" +
                                                             $"&Email={user.User.Email}");
        
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        Page<FavoritePostUserDto>? page = await message.Content.ReadFromJsonAsync<Page<FavoritePostUserDto>>();
        page.Should().NotBeNull();
        
        page.Data.Should().NotBeNull();
        page.Data.First().Should().NotBeNull();
        page.Data.First().UserId.Should().Be(postUser.UserId);
        page.Data.First().PostUserId.Should().Be(postUser.Id);
        page.Data.First().Id.Should().Be(favoritePost.Id);
    }
    
    [Fact]
    public async Task GetAllFilterByPost()
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
        FavoritePostUserDto favoritePost = await _helper.AddFavoritePost(postUser);

        HttpResponseMessage message = await _client.GetAsync($"{_url}?" +
                                                             $"PostUserId={postUser.Id}" +
                                                             $"&Title={postUser.Title}" +
                                                             $"&ReadingTimeMinutesMin={postUser.ReadingTimeMinutes - 2}" +
                                                             $"&ReadingTimeMinutesMax={postUser.ReadingTimeMinutes + 2}" +
                                                             $"&CategoryId={postUser.CategoryId}");
        
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        Page<FavoritePostUserDto>? page = await message.Content.ReadFromJsonAsync<Page<FavoritePostUserDto>>();
        page.Should().NotBeNull();
        
        page.Data.Should().NotBeNull();
        page.Data.First().Should().NotBeNull();
        page.Data.First().UserId.Should().Be(postUser.UserId);
        page.Data.First().PostUserId.Should().Be(postUser.Id);
        page.Data.First().Id.Should().Be(favoritePost.Id);
    }
    
    [Fact]
    public async Task ExistsReturnFalse()
    {
        
        UserResultTest user = await _helper.CreateAndGetUser();
        string token = user.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        
        HttpResponseMessage message = await _client.GetAsync($"{_url}/{Guid.NewGuid()}/exists");
        
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<bool>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<bool>>();
        http.Should().NotBeNull();
        http.Data.Should().BeFalse();
        http.Status.Should().BeTrue();
        http.Code.Should().Be(200);
    }
    [Fact]
    public async Task ExistsReturnTrue()
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
        await _helper.AddFavoritePost(postUser);

        HttpResponseMessage message = await _client.GetAsync($"{_url}/{postUser.Id}/exists");
        
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<bool>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<bool>>();
        http.Should().NotBeNull();
        http.Data.Should().BeTrue();
        http.Status.Should().BeTrue();
        http.Code.Should().Be(200);
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

        HttpResponseMessage message = await _client.PostAsync($"{_url}/{postUser.Id}/Toggle", null);
        
        message.StatusCode.Should().Be(HttpStatusCode.Created);

        ResponseHttp<FavoritePostUserDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<FavoritePostUserDto>>();
        http.Should().NotBeNull();
        http.Data.Should().NotBeNull();
        http.Status.Should().BeTrue();
        
        http.Data.UserId.Should().Be(user.User.Id);
        http.Data.PostUserId.Should().Be(postUser.Id);
    }
    
    [Fact]
    public async Task CreateThrowForb()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        

        string token = master!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        HttpResponseMessage message = await _client.PostAsync($"{_url}/{Guid.NewGuid()}/Toggle", null);
        
        message.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task CreateNotFoundPost()
    {
        UserResultTest user = await _helper.CreateAndGetUser();
        string token = user.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage message = await _client.PostAsync($"{_url}/{Guid.NewGuid()}/Toggle", null);
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Data.Should().BeNull();
        http.Status.Should().BeFalse();
        http.Code.Should().Be((int)HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task Remove()
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
        await _helper.AddFavoritePost(postUser);

        HttpResponseMessage message = await _client.PostAsync($"{_url}/{postUser.Id}/Toggle", null);
        
        message.StatusCode.Should().Be(HttpStatusCode.NoContent);

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Data.Should().BeNull();
        http.Status.Should().BeTrue();
        http.Code.Should().Be(204);
    }
    
    [Fact]
    public async Task UpdateAllFields()
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
        FavoritePostUserDto favor = await _helper.AddFavoritePost(postUser);

        UpdateFavoritePostUserDto dto = new UpdateFavoritePostUserDto
        {
            UserNotes = "Any notes",
            UserRating = 5
        };
        
        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_url}/{favor.Id}", dto);
        
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<FavoritePostUserDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<FavoritePostUserDto>>();
        http.Should().NotBeNull();
        http.Data.Should().NotBeNull();
        http.Status.Should().BeTrue();
        http.Code.Should().Be(200);
        
        http.Data.Id.Should().Be(favor.Id);
        http.Data.UserId.Should().Be(postUser.UserId);
        http.Data.UserNotes.Should().Be(dto.UserNotes);
        http.Data.UserRating.Should().Be(dto.UserRating);
    }
    
    [Fact]
    public async Task UpdateJustUserNotes()
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
        FavoritePostUserDto favor = await _helper.AddFavoritePost(postUser);

        UpdateFavoritePostUserDto dto = new UpdateFavoritePostUserDto
        {
            UserNotes = "Any notes",
        };
        
        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_url}/{favor.Id}", dto);
        
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<FavoritePostUserDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<FavoritePostUserDto>>();
        http.Should().NotBeNull();
        http.Data.Should().NotBeNull();
        http.Status.Should().BeTrue();
        http.Code.Should().Be(200);
        
        http.Data.Id.Should().Be(favor.Id);
        http.Data.UserId.Should().Be(postUser.UserId);
        http.Data.UserNotes.Should().Be(dto.UserNotes);
        http.Data.UserRating.Should().Be(favor.UserRating);
    }
    
    [Fact]
    public async Task UpdateJustUserRating()
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
        FavoritePostUserDto favor = await _helper.AddFavoritePost(postUser);

        UpdateFavoritePostUserDto dto = new UpdateFavoritePostUserDto
        {
            UserRating = 4
        };
        
        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_url}/{favor.Id}", dto);
        
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<FavoritePostUserDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<FavoritePostUserDto>>();
        http.Should().NotBeNull();
        http.Data.Should().NotBeNull();
        http.Status.Should().BeTrue();
        http.Code.Should().Be(200);
        
        http.Data.Id.Should().Be(favor.Id);
        http.Data.UserId.Should().Be(postUser.UserId);
        http.Data.UserNotes.Should().Be(favor.UserNotes);
        http.Data.UserRating.Should().Be(dto.UserRating);
    }
    
}
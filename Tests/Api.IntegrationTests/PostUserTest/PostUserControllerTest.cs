using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using JobVacancy.API.IntegrationTests.Utils;
using JobVacancy.API.models.dtos.Category;
using JobVacancy.API.models.dtos.PostUser;
using JobVacancy.API.Utils.Page;
using JobVacancy.API.Utils.Res;
using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;

namespace JobVacancy.API.IntegrationTests.PostUserTest;

public class PostUserControllerTest: IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly Helper _helper;
    private readonly IConfiguration _configuration;
    private readonly string _url = "/api/v1/PostUser";
    private readonly ITestOutputHelper _output;

    public PostUserControllerTest(CustomWebApplicationFactory factory, ITestOutputHelper output) 
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
        CategoryDto categoryDto = await _helper.CreateCategory(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        token = user.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        CreatePostUserDto dto = new CreatePostUserDto
        {
            CategoryId = categoryDto.Id,
            Content = string.Concat(Enumerable.Repeat("AnyContent", 30)),
            IsActive = true,
            ImageUrl = "https://github.com/Andersoncrs5",
            ReadingTimeMinutes = 4,
            Title = "A Title simple to a post simple"
        };

        HttpResponseMessage message = await _client.PostAsJsonAsync($"{_url}", dto);
        
        message.StatusCode.Should().Be(HttpStatusCode.Created);
        ResponseHttp<PostUserDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<PostUserDto>>();
        http.Should().NotBeNull();
        http.Data.Should().NotBeNull();
        
        http.Data.Id.Should().NotBeEmpty();
        http.Data.Title.Should().Be(dto.Title);
        http.Data.Content.Should().Be(dto.Content);
        http.Data.CategoryId.Should().Be(dto.CategoryId);
        http.Data.IsActive.Should().Be(dto.IsActive);
        http.Data.ReadingTimeMinutes.Should().Be(dto.ReadingTimeMinutes);
        http.Data.ImageUrl.Should().Be(dto.ImageUrl);
        http.Data.UserId.Should().Be(user.User!.Id);
    }
    
    [Fact]
    public async Task CreateReturnNotFoundCategory()
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

        CreatePostUserDto dto = new CreatePostUserDto
        {
            CategoryId = Guid.NewGuid().ToString(),
            Content = string.Concat(Enumerable.Repeat("AnyContent", 30)),
            IsActive = true,
            ImageUrl = "https://github.com/Andersoncrs5",
            ReadingTimeMinutes = 4,
            Title = "A Title simple to a post simple"
        };

        HttpResponseMessage message = await _client.PostAsJsonAsync($"{_url}", dto);
        
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);
        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Data.Should().BeNull();
        http.Message.Should().NotBeNull();
        http.Code.Should().Be(404);
    }
 
    [Fact]
    public async Task CreateReturnNotForbidden()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        CategoryDto categoryDto = await _helper.CreateCategory(master);
        
        CreatePostUserDto dto = new CreatePostUserDto
        {
            CategoryId = Guid.NewGuid().ToString(),
            Content = string.Concat(Enumerable.Repeat("AnyContent", 30)),
            IsActive = true,
            ImageUrl = "https://github.com/Andersoncrs5",
            ReadingTimeMinutes = 4,
            Title = "A Title simple to a post simple"
        };

        HttpResponseMessage message = await _client.PostAsJsonAsync($"{_url}", dto);
        
        message.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task CreateReturnCategoryDisabled()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        CategoryDto categoryDto = await _helper.CreateCategory(master, false);

        UserResultTest user = await _helper.CreateAndGetUser();
        token = user.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        CreatePostUserDto dto = new CreatePostUserDto
        {
            CategoryId = categoryDto.Id,
            Content = string.Concat(Enumerable.Repeat("AnyContent", 30)),
            IsActive = true,
            ImageUrl = "https://github.com/Andersoncrs5",
            ReadingTimeMinutes = 4,
            Title = "A Title simple to a post simple"
        };

        HttpResponseMessage message = await _client.PostAsJsonAsync($"{_url}", dto);
        
        message.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Data.Should().BeNull();
        http.Message.Should().NotBeNull();
        http.Code.Should().Be(400);
    }

    [Fact]
    public async Task Get()
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

        HttpResponseMessage message = await _client.GetAsync($"{_url}/{postUser.Id}");
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<PostUserDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<PostUserDto>>();
        http.Should().NotBeNull();
        http.Data.Should().NotBeNull();
        http.Code.Should().Be(200);
        http.Message.Should().NotBeNull();
        http.Status.Should().BeTrue();
        
        http.Data.Id.Should().Be(postUser.Id);
    }
    
    [Fact]
    public async Task GetNotFound()
    {
        string token;
        UserResultTest user = await _helper.CreateAndGetUser();
        token = user.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage message = await _client.GetAsync($"{_url}/{Guid.NewGuid()}");
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Data.Should().BeNull();
        http.Code.Should().Be(404);
        http.Message.Should().NotBeNull();
        http.Status.Should().BeFalse();
    }
    
    [Fact]
    public async Task Delete()
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

        HttpResponseMessage message = await _client.DeleteAsync($"{_url}/{postUser.Id}");
        message.StatusCode.Should().Be(HttpStatusCode.NoContent);

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Data.Should().BeNull();
        http.Code.Should().Be(204);
        http.Message.Should().NotBeNull();
        http.Status.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteNotFound()
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

        HttpResponseMessage message = await _client.DeleteAsync($"{_url}/{Guid.NewGuid()}");
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Data.Should().BeNull();
        http.Code.Should().Be(404);
        http.Message.Should().NotBeNull();
        http.Status.Should().BeFalse();
    }
    
    [Fact]
    public async Task DeleteNotFoundThrowForb()
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
        
        token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage message = await _client.DeleteAsync($"{_url}/{Guid.NewGuid()}");
        message.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task DeleteNotFoundThrowForbBecauseAnotherUser()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        CategoryDto categoryDto = await _helper.CreateCategory(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        UserResultTest user1 = await _helper.CreateAndGetUser();
        token = user.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        PostUserDto postUser = await _helper.CreatePostUser(categoryDto, user.User!.Id);
        
        token = user1.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        HttpResponseMessage message = await _client.DeleteAsync($"{_url}/{postUser.Id}");
        message.StatusCode.Should().Be(HttpStatusCode.Forbidden);

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Data.Should().BeNull();
        http.Code.Should().Be(403);
        http.Message.Should().NotBeNull();
        http.Status.Should().BeFalse();
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

        Page<PostUserDto>? http = await message.Content.ReadFromJsonAsync<Page<PostUserDto>>();
        http.Should().NotBeNull();
        http.PageIndex.Should().Be(1);
        http.PageSize.Should().Be(10);
    }
    
    [Fact]
    public async Task GetAllByFilterPost()
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

        PostUserDto postUser = await _helper.CreatePostUser(categoryDto, user.User!.Id, 10);
        
        HttpResponseMessage message = await _client.GetAsync($"{_url}?" +
                                                             $"Title={postUser.Title}" +
                                                             $"&Content={postUser.Content}" +
                                                             $"&IsActive={postUser.IsActive}" +
                                                             $"&IsFeatured={postUser.IsFeatured}" +
                                                             $"&ReadingTimeMinutesBefore={(postUser.ReadingTimeMinutes + 2)}" +
                                                             $"&ReadingTimeMinutesAfter={(postUser.ReadingTimeMinutes - 2)}" + 
                                                             $"&CategoryId={postUser.CategoryId}");
        
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        Page<PostUserDto>? http = await message.Content.ReadFromJsonAsync<Page<PostUserDto>>();
        http.Should().NotBeNull();
        http.PageIndex.Should().Be(1);
        http.PageSize.Should().Be(10);
        
        http.Data.Should().NotBeNull();
        http.Data.First().Should().NotBeNull();
        http.Data.First().Id.Should().Be(postUser.Id);
    }

    [Fact]
    public async Task GetAllByFilterCategory()
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

        PostUserDto postUser = await _helper.CreatePostUser(categoryDto, user.User!.Id, 10);
        
        HttpResponseMessage message = await _client.GetAsync($"{_url}?" +
                                                             $"CategoryId={postUser.CategoryId}" +
                                                             $"&NameCategory={postUser.Category!.Name}");
        
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        Page<PostUserDto>? http = await message.Content.ReadFromJsonAsync<Page<PostUserDto>>();
        http.Should().NotBeNull();
        http.PageIndex.Should().Be(1);
        http.PageSize.Should().Be(10);
        
        http.Data.Should().NotBeNull();
        http.Data.First().Should().NotBeNull();
        http.Data.First().Id.Should().Be(postUser.Id);
    }
    
    [Fact]
    public async Task GetAllByFilterUser()
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

        PostUserDto postUser = await _helper.CreatePostUser(categoryDto, user.User!.Id, 10);
        
        HttpResponseMessage message = await _client.GetAsync($"{_url}?" +
                                                             $"UserId={postUser.UserId}" +
                                                             $"&FullNameUser={postUser.User!.FullName}" +
                                                             $"&UserName={postUser.User!.Username}" +
                                                             $"&EmailUser={postUser.User!.Email}");
        
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        Page<PostUserDto>? http = await message.Content.ReadFromJsonAsync<Page<PostUserDto>>();
        http.Should().NotBeNull();
        http.PageIndex.Should().Be(1);
        http.PageSize.Should().Be(10);
        
        http.Data.Should().NotBeNull();
        http.Data.First().Should().NotBeNull();
        http.Data.First().Id.Should().Be(postUser.Id);
    }

    [Fact]
    public async Task Update()
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

        UpdatePostUserDto dto = new UpdatePostUserDto
        {
            Content = string.Concat(Enumerable.Repeat("ContentUpdated", 30)),
            IsActive = true,
            ImageUrl = "https://github.com/Andersoncrs5",
            ReadingTimeMinutes = 6,
            Title = "A simple title updated!"
        };

        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_url}/{postUser.Id}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<PostUserDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<PostUserDto>>();
        http.Should().NotBeNull();
        http.Data.Should().NotBeNull();
        http.Data.Id.Should().Be(postUser.Id);
        http.Data.Content.Should().Be(dto.Content);
        http.Data.IsActive.Should().BeTrue();
        http.Data.ImageUrl.Should().Be(dto.ImageUrl);
        http.Data.ReadingTimeMinutes.Should().Be(dto.ReadingTimeMinutes);
        http.Data.Title.Should().Be(dto.Title);
    }
    
    [Fact]
    public async Task UpdateJustTitle()
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

        UpdatePostUserDto dto = new UpdatePostUserDto
        {
            Title = "A simple title updated!"
        };

        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_url}/{postUser.Id}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<PostUserDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<PostUserDto>>();
        http.Should().NotBeNull();
        http.Data.Should().NotBeNull();
        http.Data.Id.Should().Be(postUser.Id);
        http.Data.Content.Should().Be(postUser.Content);
        http.Data.ImageUrl.Should().Be(postUser.ImageUrl);
        http.Data.ReadingTimeMinutes.Should().Be(postUser.ReadingTimeMinutes);
        http.Data.Title.Should().Be(dto.Title);
    }
    
    [Fact]
    public async Task UpdateJustContent()
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

        UpdatePostUserDto dto = new UpdatePostUserDto
        {
            Content = string.Concat(Enumerable.Repeat("ContentUpdated", 30)),
        };

        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_url}/{postUser.Id}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<PostUserDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<PostUserDto>>();
        http.Should().NotBeNull();
        http.Data.Should().NotBeNull();
        http.Data.Id.Should().Be(postUser.Id);
        http.Data.Title.Should().Be(postUser.Title);
        http.Data.ImageUrl.Should().Be(postUser.ImageUrl);
        http.Data.ReadingTimeMinutes.Should().Be(postUser.ReadingTimeMinutes);
        http.Data.Content.Should().Be(dto.Content);
    }
    
    [Fact]
    public async Task UpdateJustReadingTimeMinutes()
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

        UpdatePostUserDto dto = new UpdatePostUserDto
        {
            ReadingTimeMinutes = Random.Shared.Next(1, 20),
        };

        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_url}/{postUser.Id}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<PostUserDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<PostUserDto>>();
        http.Should().NotBeNull();
        http.Data.Should().NotBeNull();
        http.Data.Id.Should().Be(postUser.Id);
        http.Data.Title.Should().Be(postUser.Title);
        http.Data.ImageUrl.Should().Be(postUser.ImageUrl);
        http.Data.Content.Should().Be(postUser.Content);
        http.Data.ReadingTimeMinutes.Should().Be(dto.ReadingTimeMinutes);
    }
    
    [Fact]
    public async Task UpdateNotFound()
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

        UpdatePostUserDto dto = new UpdatePostUserDto {};

        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_url}/{Guid.NewGuid()}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Data.Should().BeNull();
        http.Message.Should().NotBeNull();
        http.Code.Should().Be(404);
    }
    
    [Fact]
    public async Task UpdateReturnForb()
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

        token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        UpdatePostUserDto dto = new UpdatePostUserDto {};

        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_url}/{Guid.NewGuid()}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task UpdateReturnForbBecauseAnotherUser()
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

        UserResultTest userB = await _helper.CreateAndGetUser();
        token = userB.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        UpdatePostUserDto dto = new UpdatePostUserDto {};

        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_url}/{postUser.Id}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
    
}
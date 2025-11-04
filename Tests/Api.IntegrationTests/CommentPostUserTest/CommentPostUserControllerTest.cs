using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;
using FluentAssertions;
using JobVacancy.API.IntegrationTests.Utils;
using JobVacancy.API.models.dtos.Category;
using JobVacancy.API.models.dtos.CommentPostUser;
using JobVacancy.API.models.dtos.PostUser;
using JobVacancy.API.Utils.Page;
using JobVacancy.API.Utils.Res;

namespace JobVacancy.API.IntegrationTests.CommentPostUserTest;

public class CommentPostUserControllerTest: IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly Helper _helper;
    private readonly IConfiguration _configuration;
    private readonly string _url = "/api/v1/CommentPostUser";
    private readonly ITestOutputHelper _output;

    public CommentPostUserControllerTest(CustomWebApplicationFactory factory, ITestOutputHelper output) 
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

        Page<CommentPostUserDto>? content = await message.Content.ReadFromJsonAsync<Page<CommentPostUserDto>>();
    }
    
    [Fact]
    public async Task GetAllFilterByComment()
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

        CommentPostUserDto comment = await _helper.CreateComment(postUser, title:"ToFilter");
        
        HttpResponseMessage message = await _client.GetAsync($"{_url}?Content={comment.Content}" +
                                                             $"&IsActive={comment.IsActive}" +
                                                             $"&DepthMin={comment.Depth -1 }" +
                                                             $"&DepthMax={comment.Depth + 1 }" +
                                                             $"&Id={comment.Id}");
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        Page<CommentPostUserDto>? content = await message.Content.ReadFromJsonAsync<Page<CommentPostUserDto>>();
        
        content.Should().NotBeNull();
        content.Data.Should().NotBeNull();
        content.Data.First().Should().NotBeNull();
        content.Data.First().Id.Should().Be(comment.Id);
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

        CommentPostUserDto comment = await _helper.CreateComment(postUser, title:"ToFilter");
        
        HttpResponseMessage message = await _client.GetAsync($"{_url}?UserId={user.User.Id}" +
                                                             $"&Username={user.User.Username}" +
                                                             $"&Email={user.User.Email}" +
                                                             $"&Fullname={user.User.FullName}");
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        Page<CommentPostUserDto>? content = await message.Content.ReadFromJsonAsync<Page<CommentPostUserDto>>();
        
        content.Should().NotBeNull();
        content.Data.Should().NotBeNull();
        content.Data.First().Should().NotBeNull();
        content.Data.First().Id.Should().Be(comment.Id);
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

        CommentPostUserDto comment = await _helper.CreateComment(postUser, title:"ToFilter");
        
        HttpResponseMessage message = await _client.GetAsync($"{_url}?CategoryId={postUser.CategoryId}" +
                                                             $"&Title={postUser.Title}" +
                                                             $"&ContentPost={postUser.Content}" +
                                                             $"&ReadingTimeMinutesMin={postUser.ReadingTimeMinutes - 1}" +
                                                             $"&ReadingTimeMinutesMax={postUser.ReadingTimeMinutes + 1}");
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        Page<CommentPostUserDto>? content = await message.Content.ReadFromJsonAsync<Page<CommentPostUserDto>>();
        
        content.Should().NotBeNull();
        content.Data.Should().NotBeNull();
        content.Data.First().Should().NotBeNull();
        content.Data.First().Id.Should().Be(comment.Id);
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

        CreateCommentPostUserDto dto = new CreateCommentPostUserDto()
        {
            Content = string.Concat(Enumerable.Repeat("AnyContent", 30)),
            Depth = 5,
            PostId = postUser.Id,
            ImageUrl = "https://github.com/Andersoncrs5",
            IsActive = true
        };
        
        HttpResponseMessage message = await _client.PostAsJsonAsync($"{_url}?parentId={null}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.Created);

        ResponseHttp<CommentPostUserDto>? content = await message.Content.ReadFromJsonAsync<ResponseHttp<CommentPostUserDto>>();
        content.Should().NotBeNull();
        content.Data.Should().NotBeNull();
        content.Code.Should().Be((int)HttpStatusCode.Created);
        content.Status.Should().BeTrue();
        
        content.Data.PostId.Should().Be(postUser.Id);
        content.Data.Id.Should().NotBeNullOrEmpty();
        content.Data.UserId.Should().Be(user.User.Id);
        content.Data.ParentCommentId.Should().BeNull();
    }
    
    [Fact]
    public async Task CreateOnComment()
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

        CreateCommentPostUserDto dto = new CreateCommentPostUserDto()
        {
            Content = string.Concat(Enumerable.Repeat("AnyContent", 30)),
            Depth = 5,
            PostId = postUser.Id,
            ImageUrl = "https://github.com/Andersoncrs5",
            IsActive = true
        };
        
        HttpResponseMessage message = await _client.PostAsJsonAsync($"{_url}?parentId={comment.Id}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.Created);

        ResponseHttp<CommentPostUserDto>? content = await message.Content.ReadFromJsonAsync<ResponseHttp<CommentPostUserDto>>();
        content.Should().NotBeNull();
        content.Data.Should().NotBeNull();
        content.Code.Should().Be((int)HttpStatusCode.Created);
        content.Status.Should().BeTrue();
        
        content.Data.PostId.Should().Be(postUser.Id);
        content.Data.Id.Should().NotBeNullOrEmpty();
        content.Data.UserId.Should().Be(user.User.Id);
        content.Data.ParentCommentId.Should().Be(comment.Id);
    }
    
    [Fact]
    public async Task CreateOnCommentReturnNotFound()
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
        
        CreateCommentPostUserDto dto = new CreateCommentPostUserDto()
        {
            Content = string.Concat(Enumerable.Repeat("AnyContent", 30)),
            Depth = 5,
            PostId = postUser.Id,
            ImageUrl = "https://github.com/Andersoncrs5",
            IsActive = true
        };
        
        HttpResponseMessage message = await _client.PostAsJsonAsync($"{_url}?parentId={Guid.NewGuid()}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);

        ResponseHttp<object>? content = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        content.Should().NotBeNull();
        content.Message.Should().NotBeNull();
        content.Code.Should().Be((int)HttpStatusCode.NotFound);
        content.Status.Should().BeFalse();
        
        content.Data.Should().BeNull();
    }
        
    [Fact]
    public async Task CreateReturnNotFoundPost()
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

        CreateCommentPostUserDto dto = new CreateCommentPostUserDto()
        {
            Content = string.Concat(Enumerable.Repeat("AnyContent", 30)),
            Depth = 5,
            PostId = Guid.NewGuid().ToString(),
            ImageUrl = "https://github.com/Andersoncrs5",
            IsActive = true
        };
        
        HttpResponseMessage message = await _client.PostAsJsonAsync($"{_url}?parentId={null}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);

        ResponseHttp<object>? content = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        content.Should().NotBeNull();
        content.Data.Should().BeNull();
        content.Message.Should().NotBeNull();
        content.Code.Should().Be((int)HttpStatusCode.NotFound);
        content.Status.Should().BeFalse();
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

        CommentPostUserDto comment = await _helper.CreateComment(postUser);

        HttpResponseMessage message = await _client.GetAsync($"{_url}/{comment.Id}");
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<CommentPostUserDto>? content = await message.Content.ReadFromJsonAsync<ResponseHttp<CommentPostUserDto>>();
        
        content.Should().NotBeNull();
        content.Data.Should().NotBeNull();
        content.Code.Should().Be((int)HttpStatusCode.OK);
        content.Status.Should().BeTrue();
        
        content.Data.UserId.Should().Be(user.User.Id);
        content.Data.Id.Should().Be(comment.Id);
    }

    [Fact]
    public async Task GetNotFound()
    {
        
        UserResultTest user = await _helper.CreateAndGetUser();
        string token = user.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        HttpResponseMessage message = await _client.GetAsync($"{_url}/{Guid.NewGuid()}");
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);

        ResponseHttp<object>? content = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        
        content.Should().NotBeNull();
        content.Code.Should().Be((int)HttpStatusCode.NotFound);
        content.Status.Should().BeFalse();

        content.Data.Should().BeNull();
    }

    [Fact]
    public async Task UpdateFullFields()
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

        UpdateCommentPostUserDto dto = new UpdateCommentPostUserDto
        {
            Content = string.Concat(Enumerable.Repeat("AnyContent", 30)),
            Depth = 5,
            ImageUrl = "https://github.com/Andersoncrs5",
            IsActive = true
        };
        
        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_url}/{comment.Id}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<CommentPostUserDto>? content = await message.Content.ReadFromJsonAsync<ResponseHttp<CommentPostUserDto>>();
        
        content.Should().NotBeNull();
        content.Data.Should().NotBeNull();
        content.Code.Should().Be((int)HttpStatusCode.OK);
        content.Status.Should().BeTrue();
        
        content.Data.UserId.Should().Be(user.User.Id);
        content.Data.Id.Should().Be(comment.Id);
        
        content.Data.Content.Should().Be(dto.Content);
        content.Data.Depth.Should().Be(dto.Depth);
        content.Data.IsActive.Should().Be(dto.IsActive.Value);
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

        CommentPostUserDto comment = await _helper.CreateComment(postUser);

        UpdateCommentPostUserDto dto = new UpdateCommentPostUserDto
        {
            Content = string.Concat(Enumerable.Repeat("Updated", 30)),
        };
        
        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_url}/{comment.Id}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<CommentPostUserDto>? content = await message.Content.ReadFromJsonAsync<ResponseHttp<CommentPostUserDto>>();
        
        content.Should().NotBeNull();
        content.Data.Should().NotBeNull();
        content.Code.Should().Be((int)HttpStatusCode.OK);
        content.Status.Should().BeTrue();
        
        content.Data.UserId.Should().Be(user.User.Id);
        content.Data.PostId.Should().Be(postUser.Id);
        content.Data.Id.Should().Be(comment.Id);
        
        content.Data.Content.Should().Be(dto.Content);
        content.Data.Depth.Should().Be(comment.Depth);
        content.Data.IsActive.Should().Be(comment.IsActive);
        content.Data.ImageUrl.Should().Be(comment.ImageUrl);
    }

    [Fact]
    public async Task UpdateJustIsActive()
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

        UpdateCommentPostUserDto dto = new UpdateCommentPostUserDto
        {
            IsActive = false
        };
        
        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_url}/{comment.Id}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<CommentPostUserDto>? content = await message.Content.ReadFromJsonAsync<ResponseHttp<CommentPostUserDto>>();
        
        content.Should().NotBeNull();
        content.Data.Should().NotBeNull();
        content.Code.Should().Be((int)HttpStatusCode.OK);
        content.Status.Should().BeTrue();
        
        content.Data.UserId.Should().Be(user.User.Id);
        content.Data.PostId.Should().Be(postUser.Id);
        content.Data.Id.Should().Be(comment.Id);
        
        content.Data.Content.Should().Be(comment.Content);
        content.Data.Depth.Should().Be(comment.Depth);
        content.Data.IsActive.Should().Be(dto.IsActive.Value);
        content.Data.ImageUrl.Should().Be(comment.ImageUrl);
    }

    [Fact]
    public async Task UpdateJustImageUrl()
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

        UpdateCommentPostUserDto dto = new UpdateCommentPostUserDto
        {
            ImageUrl = "https://github.com/Andersoncrs5/JobVacancy",
        };
        
        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_url}/{comment.Id}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<CommentPostUserDto>? content = await message.Content.ReadFromJsonAsync<ResponseHttp<CommentPostUserDto>>();
        
        content.Should().NotBeNull();
        content.Data.Should().NotBeNull();
        content.Code.Should().Be((int)HttpStatusCode.OK);
        content.Status.Should().BeTrue();
        
        content.Data.UserId.Should().Be(user.User.Id);
        content.Data.PostId.Should().Be(postUser.Id);
        content.Data.Id.Should().Be(comment.Id);
        
        content.Data.Content.Should().Be(comment.Content);
        content.Data.Depth.Should().Be(comment.Depth);
        content.Data.IsActive.Should().Be(comment.IsActive);
        content.Data.ImageUrl.Should().Be(dto.ImageUrl);
    }

    [Fact]
    public async Task UpdateJustDepth()
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

        UpdateCommentPostUserDto dto = new UpdateCommentPostUserDto
        {
            Depth = 1
        };
        
        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_url}/{comment.Id}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<CommentPostUserDto>? content = await message.Content.ReadFromJsonAsync<ResponseHttp<CommentPostUserDto>>();
        
        content.Should().NotBeNull();
        content.Data.Should().NotBeNull();
        content.Code.Should().Be((int)HttpStatusCode.OK);
        content.Status.Should().BeTrue();
        
        content.Data.UserId.Should().Be(user.User.Id);
        content.Data.PostId.Should().Be(postUser.Id);
        content.Data.Id.Should().Be(comment.Id);
        
        content.Data.Content.Should().Be(comment.Content);
        content.Data.Depth.Should().Be(dto.Depth);
        content.Data.IsActive.Should().Be(comment.IsActive);
        content.Data.ImageUrl.Should().Be(comment.ImageUrl);
    }

    [Fact]
    public async Task DelNotFound()
    {
        
        UserResultTest user = await _helper.CreateAndGetUser();
        string token = user.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        HttpResponseMessage message = await _client.DeleteAsync($"{_url}/{Guid.NewGuid()}");
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);

        ResponseHttp<object>? content = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        
        content.Should().NotBeNull();
        content.Message.Should().NotBeNull();
        content.Code.Should().Be((int)HttpStatusCode.NotFound);
        content.Status.Should().BeFalse();

        content.Data.Should().BeNull();
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

        HttpResponseMessage message = await _client.DeleteAsync($"{_url}/{comment.Id}");
        message.StatusCode.Should().Be(HttpStatusCode.NoContent);

        ResponseHttp<object>? content = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        
        content.Should().NotBeNull();
        content.Message.Should().NotBeNull();
        content.Data.Should().BeNull();
        content.Code.Should().Be((int)HttpStatusCode.NoContent);
        content.Status.Should().BeTrue();
    }
}
using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using JobVacancy.API.IntegrationTests.Utils;
using JobVacancy.API.models.dtos.Category;
using JobVacancy.API.models.dtos.CommentPostEnterprise;
using JobVacancy.API.models.dtos.Enterprise;
using JobVacancy.API.models.dtos.Industry;
using JobVacancy.API.models.dtos.PostEnterprise;
using JobVacancy.API.Utils.Page;
using JobVacancy.API.Utils.Res;
using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;

namespace JobVacancy.API.IntegrationTests.CommentPostEnterpriseTest;

public class CommentPostEnterpriseControllerTest: IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly Helper _helper;
    private readonly IConfiguration _configuration;
    private readonly string _url = "/api/v1/CommentPostEnterprise";
    private readonly ITestOutputHelper _output;

    public CommentPostEnterpriseControllerTest(CustomWebApplicationFactory factory, ITestOutputHelper output) 
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

        CreateCommentPostEnterpriseDto dto = new CreateCommentPostEnterpriseDto
        {
            Content = string.Concat(Enumerable.Repeat("AnyContent", 30)),
            Depth = 5,
            PostId = post.Id,
            ImageUrl = "https://github.com/Andersoncrs5",
            IsActive = true
        };
        
        HttpResponseMessage message = await _client.PostAsJsonAsync($"{_url}?parentId={null}", dto);
        
        message.StatusCode.Should().Be(HttpStatusCode.Created);

        ResponseHttp<CommentPostEnterpriseDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<CommentPostEnterpriseDto>>();
        http.Should().NotBeNull();
        http.Code.Should().Be(201);
        http.Data.Should().NotBeNull();
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Status.Should().BeTrue();
        
        http.Data.Id.Should().NotBeNullOrWhiteSpace();
        http.Data.PostId.Should().Be(post.Id);
        
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

        CreateCommentPostEnterpriseDto dto = new CreateCommentPostEnterpriseDto
        {
            Content = string.Concat(Enumerable.Repeat("AnyContent", 30)),
            Depth = 5,
            PostId = Guid.NewGuid().ToString(),
            ImageUrl = "https://github.com/Andersoncrs5",
            IsActive = true
        };
        
        HttpResponseMessage message = await _client.PostAsJsonAsync($"{_url}?parentId={null}", dto);
        
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Code.Should().Be(404);
        http.Data.Should().BeNull();
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Status.Should().BeFalse();
    }
    
    [Fact]
    public async Task CreateOnComment()
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

        CreateCommentPostEnterpriseDto dto = new CreateCommentPostEnterpriseDto
        {
            Content = string.Concat(Enumerable.Repeat("AnyContent", 30)),
            Depth = 5,
            PostId = post.Id,
            ImageUrl = "https://github.com/Andersoncrs5",
            IsActive = true
        };

        CommentPostEnterpriseDto commentCreated = await _helper.CreateCommentPostEnterpriseDto(post.Id, true, null);

        HttpResponseMessage message = await _client.PostAsJsonAsync($"{_url}?parentId={commentCreated.Id}", dto);
        
        message.StatusCode.Should().Be(HttpStatusCode.Created);

        ResponseHttp<CommentPostEnterpriseDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<CommentPostEnterpriseDto>>();
        http.Should().NotBeNull();
        http.Code.Should().Be(201);
        http.Data.Should().NotBeNull();
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Status.Should().BeTrue();
        
        http.Data.Id.Should().NotBeNullOrWhiteSpace();
        http.Data.PostId.Should().Be(post.Id);
        http.Data.ParentCommentId.Should().Be(commentCreated.Id);
        
    }
    
    [Fact]
    public async Task CreateOnCommentNotFound()
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

        CreateCommentPostEnterpriseDto dto = new CreateCommentPostEnterpriseDto
        {
            Content = string.Concat(Enumerable.Repeat("AnyContent", 30)),
            Depth = 5,
            PostId = post.Id,
            ImageUrl = "https://github.com/Andersoncrs5",
            IsActive = true
        };

        HttpResponseMessage message = await _client.PostAsJsonAsync($"{_url}?parentId={Guid.NewGuid()}", dto);
        
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Code.Should().Be(404);
        http.Data.Should().BeNull();
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Status.Should().BeFalse();
        
    }
    
    [Fact]
    public async Task Get()
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
        
        CommentPostEnterpriseDto commentCreated = await _helper.CreateCommentPostEnterpriseDto(post.Id, true, null);

        HttpResponseMessage message = await _client.GetAsync($"{_url}/{commentCreated.Id}");
        
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<CommentPostEnterpriseDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<CommentPostEnterpriseDto>>();
        http.Should().NotBeNull();
        http.Code.Should().Be(200);
        http.Data.Should().NotBeNull();
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Status.Should().BeTrue();
        
        http.Data.Id.Should().NotBeNullOrWhiteSpace();
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

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Code.Should().Be(404);
        http.Data.Should().BeNull();
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Status.Should().BeFalse();
        
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
        
        CommentPostEnterpriseDto commentCreated = await _helper.CreateCommentPostEnterpriseDto(post.Id, true, null);

        HttpResponseMessage message = await _client.DeleteAsync($"{_url}/{commentCreated.Id}");
        
        message.StatusCode.Should().Be(HttpStatusCode.NoContent);

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Code.Should().Be(204);
        http.Data.Should().BeNull();
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Status.Should().BeTrue();
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

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Code.Should().Be(404);
        http.Data.Should().BeNull();
        http.Message.Should().NotBeNullOrWhiteSpace();
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

        Page<CommentPostEnterpriseDto>? page = await message.Content.ReadFromJsonAsync<Page<CommentPostEnterpriseDto>>();
        page.Should().NotBeNull();
        page.PageIndex.Should().Be(1);
        page.PageSize.Should().Be(10);
    }
    
    [Fact]
    public async Task GetAllFilterByComment()
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
        
        CommentPostEnterpriseDto commentCreated = await _helper.CreateCommentPostEnterpriseDto(post.Id, true, null, content: "NoKnow");
        
        HttpResponseMessage message = await _client.GetAsync($"{_url}?Content={commentCreated.Content}" +
                                                             $"&IsActive={commentCreated.IsActive}" +
                                                             $"&DepthMin={commentCreated.Depth - 1}" +
                                                             $"&DepthMax={commentCreated.Depth + 1}" +
                                                             $"&Id={commentCreated.Id}");
        
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        Page<CommentPostEnterpriseDto>? page = await message.Content.ReadFromJsonAsync<Page<CommentPostEnterpriseDto>>();
        page.Should().NotBeNull();
        page.TotalCount.Should().Be(1);
        page.PageIndex.Should().Be(1);
        page.PageSize.Should().Be(10);
        
        page.Data.Should().NotBeNull();
        page.Data.First().Should().NotBeNull();
        page.Data.First().Id.Should().Be(commentCreated.Id);
        
    }
    
    [Fact]
    public async Task GetAllFilterByUser()
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
        
        CommentPostEnterpriseDto commentCreated = await _helper.CreateCommentPostEnterpriseDto(post.Id, true, null, content: "NoKnow");
        
        HttpResponseMessage message = await _client.GetAsync($"{_url}?UserId={user.User.Id}" +
                                                             $"&Username={user.User.Username}" +
                                                             $"&Email={user.User.Email}" +
                                                             $"&Fullname={user.User.FullName}");
        
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        Page<CommentPostEnterpriseDto>? page = await message.Content.ReadFromJsonAsync<Page<CommentPostEnterpriseDto>>();
        page.Should().NotBeNull();
        page.TotalCount.Should().Be(1);
        page.PageIndex.Should().Be(1);
        page.PageSize.Should().Be(10);
        
        page.Data.Should().NotBeNull();
        page.Data.First().Should().NotBeNull();
        page.Data.First().Id.Should().Be(commentCreated.Id);
        
    }
    
    [Fact]
    public async Task GetAllFilterByPost()
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
        
        CommentPostEnterpriseDto commentCreated = await _helper.CreateCommentPostEnterpriseDto(post.Id, true, null, content: "NoKnow");
        
        HttpResponseMessage message = await _client.GetAsync($"{_url}?Title={post.Title}" +
                                                             $"&PostId={post.Id}" +
                                                             $"&ContentPost={post.Content}" +
                                                             $"&ReadingTimeMinutesMin={post.ReadingTimeMinutes -1}" +
                                                             $"&ReadingTimeMinutesMax={post.ReadingTimeMinutes +1}");
        
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        Page<CommentPostEnterpriseDto>? page = await message.Content.ReadFromJsonAsync<Page<CommentPostEnterpriseDto>>();
        page.Should().NotBeNull();
        page.TotalCount.Should().Be(1);
        page.PageIndex.Should().Be(1);
        page.PageSize.Should().Be(10);
        
        page.Data.Should().NotBeNull();
        page.Data.First().Should().NotBeNull();
        page.Data.First().Id.Should().Be(commentCreated.Id);
        
    }
    
    [Fact]
    public async Task GetAllFilterByEnterprise()
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
        
        CommentPostEnterpriseDto commentCreated = await _helper.CreateCommentPostEnterpriseDto(post.Id, true, null, content: "NoKnow");
        
        HttpResponseMessage message = await _client.GetAsync($"{_url}?EnterpriseId={enterprise.Id}" +
                                                             $"&NameEnterprise={enterprise.Name}" +
                                                             $"&Type={enterprise.Type}");
        
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        Page<CommentPostEnterpriseDto>? page = await message.Content.ReadFromJsonAsync<Page<CommentPostEnterpriseDto>>();
        page.Should().NotBeNull();
        page.TotalCount.Should().Be(1);
        page.PageIndex.Should().Be(1);
        page.PageSize.Should().Be(10);
        
        page.Data.Should().NotBeNull();
        page.Data.First().Should().NotBeNull();
        page.Data.First().Id.Should().Be(commentCreated.Id);
        
    }

    [Fact]
    public async Task Update()
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
        
        CommentPostEnterpriseDto commentCreated = await _helper.CreateCommentPostEnterpriseDto(post.Id, true, null);

        UpdateCommentPostEnterpriseDto dto = new UpdateCommentPostEnterpriseDto
        {
            Content = "Updated",
            Depth = 2,
            ImageUrl = "https://github.com/Andersoncrs5/JobVacancy",
            IsActive = false
        };
        
        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_url}/{commentCreated.Id}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<CommentPostEnterpriseDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<CommentPostEnterpriseDto>>();
        http.Should().NotBeNull();
        http.Code.Should().Be(200);
        http.Data.Should().NotBeNull();
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Status.Should().BeTrue();
        
        http.Data.Id.Should().Be(commentCreated.Id);
        http.Data.PostId.Should().Be(commentCreated.PostId);
        http.Data.UserId.Should().Be(commentCreated.UserId);
        http.Data.ParentCommentId.Should().Be(commentCreated.ParentCommentId);
        
        http.Data.Content.Should().Be(dto.Content);
        http.Data.Depth.Should().Be(dto.Depth);
        http.Data.ImageUrl.Should().Be(dto.ImageUrl);
        http.Data.IsActive.Should().Be(dto.IsActive.Value);
    }
    
    [Fact]
    public async Task UpdateJustImageUrl()
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
        
        CommentPostEnterpriseDto commentCreated = await _helper.CreateCommentPostEnterpriseDto(post.Id, true, null);

        UpdateCommentPostEnterpriseDto dto = new UpdateCommentPostEnterpriseDto
        {
            ImageUrl = "https://github.com/Andersoncrs5/JobVacancy",
        };
        
        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_url}/{commentCreated.Id}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<CommentPostEnterpriseDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<CommentPostEnterpriseDto>>();
        http.Should().NotBeNull();
        http.Code.Should().Be(200);
        http.Data.Should().NotBeNull();
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Status.Should().BeTrue();
        
        http.Data.Id.Should().Be(commentCreated.Id);
        http.Data.PostId.Should().Be(commentCreated.PostId);
        http.Data.UserId.Should().Be(commentCreated.UserId);
        http.Data.ParentCommentId.Should().Be(commentCreated.ParentCommentId);
        
        http.Data.Content.Should().Be(commentCreated.Content);
        http.Data.Depth.Should().Be(commentCreated.Depth);
        http.Data.ImageUrl.Should().Be(dto.ImageUrl);
        http.Data.IsActive.Should().Be(commentCreated.IsActive);
    }
    
    [Fact]
    public async Task UpdateJustIsActive()
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
        
        CommentPostEnterpriseDto commentCreated = await _helper.CreateCommentPostEnterpriseDto(post.Id, true, null);

        UpdateCommentPostEnterpriseDto dto = new UpdateCommentPostEnterpriseDto
        {
            IsActive = false 
        };
        
        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_url}/{commentCreated.Id}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<CommentPostEnterpriseDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<CommentPostEnterpriseDto>>();
        http.Should().NotBeNull();
        http.Code.Should().Be(200);
        http.Data.Should().NotBeNull();
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Status.Should().BeTrue();
        
        http.Data.Id.Should().Be(commentCreated.Id);
        http.Data.PostId.Should().Be(commentCreated.PostId);
        http.Data.UserId.Should().Be(commentCreated.UserId);
        http.Data.ParentCommentId.Should().Be(commentCreated.ParentCommentId);
        
        http.Data.Content.Should().Be(commentCreated.Content);
        http.Data.Depth.Should().Be(commentCreated.Depth);
        http.Data.ImageUrl.Should().Be(commentCreated.ImageUrl);
        http.Data.IsActive.Should().Be(dto.IsActive.Value);
    }
    
    [Fact]
    public async Task UpdateJustContent()
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
        
        CommentPostEnterpriseDto commentCreated = await _helper.CreateCommentPostEnterpriseDto(post.Id, true, null);

        UpdateCommentPostEnterpriseDto dto = new UpdateCommentPostEnterpriseDto
        {
            Content = "Updated",
        };
        
        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_url}/{commentCreated.Id}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<CommentPostEnterpriseDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<CommentPostEnterpriseDto>>();
        http.Should().NotBeNull();
        http.Code.Should().Be(200);
        http.Data.Should().NotBeNull();
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Status.Should().BeTrue();
        
        http.Data.Id.Should().Be(commentCreated.Id);
        http.Data.PostId.Should().Be(commentCreated.PostId);
        http.Data.UserId.Should().Be(commentCreated.UserId);
        http.Data.ParentCommentId.Should().Be(commentCreated.ParentCommentId);
        
        http.Data.Content.Should().Be(dto.Content);
        http.Data.Depth.Should().Be(commentCreated.Depth);
        http.Data.ImageUrl.Should().Be(commentCreated.ImageUrl);
        http.Data.IsActive.Should().Be(commentCreated.IsActive);
    }
    
    [Fact]
    public async Task UpdateJustDepth()
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
        
        CommentPostEnterpriseDto commentCreated = await _helper.CreateCommentPostEnterpriseDto(post.Id, true, null);

        UpdateCommentPostEnterpriseDto dto = new UpdateCommentPostEnterpriseDto
        {
            Depth = 6
        };
        
        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_url}/{commentCreated.Id}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<CommentPostEnterpriseDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<CommentPostEnterpriseDto>>();
        http.Should().NotBeNull();
        http.Code.Should().Be(200);
        http.Data.Should().NotBeNull();
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Status.Should().BeTrue();
        
        http.Data.Id.Should().Be(commentCreated.Id);
        http.Data.PostId.Should().Be(commentCreated.PostId);
        http.Data.UserId.Should().Be(commentCreated.UserId);
        http.Data.ParentCommentId.Should().Be(commentCreated.ParentCommentId);
        
        http.Data.Content.Should().Be(commentCreated.Content);
        http.Data.Depth.Should().Be(dto.Depth);
        http.Data.ImageUrl.Should().Be(commentCreated.ImageUrl);
        http.Data.IsActive.Should().Be(commentCreated.IsActive);
    }
    
    
}
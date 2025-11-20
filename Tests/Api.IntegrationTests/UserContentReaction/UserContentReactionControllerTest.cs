using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using JobVacancy.API.IntegrationTests.Utils;
using JobVacancy.API.models.dtos.Category;
using JobVacancy.API.models.dtos.CommentPostUser;
using JobVacancy.API.models.dtos.PostUser;
using JobVacancy.API.models.dtos.UserContentReaction;
using JobVacancy.API.models.entities.Enums;
using JobVacancy.API.Utils.Page;
using JobVacancy.API.Utils.Res;
using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;

namespace JobVacancy.API.IntegrationTests.UserContentReaction;

public class UserContentReactionControllerTest: IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly Helper _helper;
    private readonly IConfiguration _configuration;
    private readonly string _url = "/api/v1/UserContentReaction";
    private readonly ITestOutputHelper _output;
    
    public UserContentReactionControllerTest(CustomWebApplicationFactory factory, ITestOutputHelper output) 
    {
        _client = factory.CreateClient(); 
        _helper = new Helper(_client); 
        _configuration = factory.Configuration;
        _output = output;
    }

    [Fact]
    public async Task CreateReactionWithLikeToPostUser()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", master.Token!);
        CategoryDto categoryDto = await _helper.CreateCategory(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        UserResultTest userToReact = await _helper.CreateAndGetUser();
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Tokens!.Token!);

        PostUserDto postUser = await _helper.CreatePostUser(categoryDto, user.User!.Id);

        CreateUserContentReactionDto dto = new CreateUserContentReactionDto
        {
            ContentId = postUser.Id,
            ReactionType = ReactionTypeEnum.Like,
            TargetType = ReactionTargetEnum.PostUser
        };

        HttpResponseMessage message = await _client.PostAsJsonAsync($"{_url}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.Created);

        ResponseHttp<UserContentReactionDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<UserContentReactionDto>>();
        
        http.Should().NotBeNull();
        http.Code.Should().Be((int) HttpStatusCode.Created);
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Status.Should().BeTrue();
        
        http.Data.Should().NotBeNull();
        http.Data.Id.Should().NotBeNullOrWhiteSpace();
        http.Data.PostUserId.Should().Be(postUser.Id);
        http.Data.ReactionType.Should().Be(ReactionTypeEnum.Like);
        http.Data.TargetType.Should().Be(ReactionTargetEnum.PostUser);
    }

    [Fact]
    public async Task CreateReactionWithCommentToPostUser()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        CategoryDto categoryDto = await _helper.CreateCategory(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Tokens!.Token!);

        PostUserDto postUser = await _helper.CreatePostUser(categoryDto, user.User!.Id);

        CommentPostUserDto comment = await _helper.CreateComment(postUser);

        CreateUserContentReactionDto dto = new CreateUserContentReactionDto
        {
            ContentId = comment.Id,
            ReactionType = ReactionTypeEnum.Like,
            TargetType = ReactionTargetEnum.CommentUser
        };

        HttpResponseMessage message = await _client.PostAsJsonAsync($"{_url}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.Created);

        ResponseHttp<UserContentReactionDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<UserContentReactionDto>>();
        
        http.Should().NotBeNull();
        http.Code.Should().Be((int) HttpStatusCode.Created);
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Status.Should().BeTrue();
        
        http.Data.Should().NotBeNull();
        http.Data.Id.Should().NotBeNullOrWhiteSpace();
        http.Data.CommentUserId.Should().Be(dto.ContentId);
        http.Data.ReactionType.Should().Be(dto.ReactionType);
        http.Data.TargetType.Should().Be(dto.TargetType);
    }
    
    [Fact]
    public async Task ToggleReactionWithDislikeToLikeInCommentPostUser()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        CategoryDto categoryDto = await _helper.CreateCategory(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Tokens!.Token!);

        PostUserDto postUser = await _helper.CreatePostUser(categoryDto, user.User!.Id);

        CommentPostUserDto comment = await _helper.CreateComment(postUser);
        
        CreateUserContentReactionDto dto = new CreateUserContentReactionDto
        {
            ContentId = comment.Id,
            ReactionType = ReactionTypeEnum.Dislike,
            TargetType = ReactionTargetEnum.CommentUser
        };

        await _helper.ReactionTo(comment.Id, ReactionTypeEnum.Like, dto.TargetType, HttpStatusCode.Created);
        HttpResponseMessage message = await _client.PostAsJsonAsync($"{_url}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<UserContentReactionDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<UserContentReactionDto>>();
        
        http.Should().NotBeNull();
        http.Code.Should().Be((int) HttpStatusCode.OK);
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Status.Should().BeTrue();
        
        http.Data.Should().NotBeNull();
        http.Data.Id.Should().NotBeNullOrWhiteSpace();
        http.Data.CommentUserId.Should().Be(dto.ContentId);
        http.Data.ReactionType.Should().Be(dto.ReactionType);
        http.Data.TargetType.Should().Be(dto.TargetType);
    }
    
    [Fact]
    public async Task ToggleReactionWithLikeToDislikeInCommentPostUser()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        CategoryDto categoryDto = await _helper.CreateCategory(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Tokens!.Token!);

        PostUserDto postUser = await _helper.CreatePostUser(categoryDto, user.User!.Id);

        CommentPostUserDto comment = await _helper.CreateComment(postUser);
        
        CreateUserContentReactionDto dto = new CreateUserContentReactionDto
        {
            ContentId = comment.Id,
            ReactionType = ReactionTypeEnum.Like,
            TargetType = ReactionTargetEnum.CommentUser
        };

        await _helper.ReactionTo(comment.Id, ReactionTypeEnum.Dislike, dto.TargetType, HttpStatusCode.Created);
        HttpResponseMessage message = await _client.PostAsJsonAsync($"{_url}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<UserContentReactionDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<UserContentReactionDto>>();
        
        http.Should().NotBeNull();
        http.Code.Should().Be((int) HttpStatusCode.OK);
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Status.Should().BeTrue();
        
        http.Data.Should().NotBeNull();
        http.Data.Id.Should().NotBeNullOrWhiteSpace();
        http.Data.CommentUserId.Should().Be(dto.ContentId);
        http.Data.ReactionType.Should().Be(dto.ReactionType);
        http.Data.TargetType.Should().Be(dto.TargetType);
    }

    [Fact]
    public async Task GetAll()
    {
        UserResultTest user = await _helper.CreateAndGetUser();
        string token = user.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage message = await _client.GetAsync($"{_url}");
        _output.WriteLine(message.Content.ReadAsStringAsync().Result);
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        Page<UserContentReactionDto>? http = await message.Content.ReadFromJsonAsync<Page<UserContentReactionDto>>();
        http.Should().NotBeNull();
        http.PageIndex.Should().Be(1);
        http.PageSize.Should().Be(10);
    }
    
    [Fact]
    public async Task GetAllLoadJustLoadUser()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        CategoryDto categoryDto = await _helper.CreateCategory(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Tokens!.Token!);

        PostUserDto postUser = await _helper.CreatePostUser(categoryDto, user.User!.Id);

        CommentPostUserDto comment = await _helper.CreateComment(postUser);
        
        await _helper.ReactionTo(comment.Id, ReactionTypeEnum.Like, ReactionTargetEnum.CommentUser, HttpStatusCode.Created);

        HttpResponseMessage message = await _client.GetAsync($"{_url}?LoadUser={true}&PageSize={1}");
        _output.WriteLine(message.Content.ReadAsStringAsync().Result);
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        Page<UserContentReactionDto>? http = await message.Content.ReadFromJsonAsync<Page<UserContentReactionDto>>();
        http.Should().NotBeNull();
        http.PageIndex.Should().Be(1);
        http.PageSize.Should().Be(1);
        
        http.Data.Should().NotBeNull();
        http.Data.First().Should().NotBeNull();
        http.Data.First().User.Should().NotBeNull();
        http.Data.First().PostUser.Should().BeNull();
        http.Data.First().PostEnterprise.Should().BeNull();
        http.Data.First().CommentUser.Should().BeNull();
        http.Data.First().CommentEnterprise.Should().BeNull();
    }
    
    [Fact]
    public async Task GetAllLoadJustLoadPostUser()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        CategoryDto categoryDto = await _helper.CreateCategory(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Tokens!.Token!);

        PostUserDto postUser = await _helper.CreatePostUser(categoryDto, user.User!.Id);

        CommentPostUserDto comment = await _helper.CreateComment(postUser);
        
        await _helper.ReactionTo(comment.Id, ReactionTypeEnum.Like, ReactionTargetEnum.CommentUser, HttpStatusCode.Created);

        HttpResponseMessage message = await _client.GetAsync($"{_url}?LoadPostUser={true}&PageSize={1}");
        _output.WriteLine(message.Content.ReadAsStringAsync().Result);
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        Page<UserContentReactionDto>? http = await message.Content.ReadFromJsonAsync<Page<UserContentReactionDto>>();
        http.Should().NotBeNull();
        http.PageIndex.Should().Be(1);
        http.PageSize.Should().Be(1);
        
        http.Data.Should().NotBeNull();
        http.Data.First().Should().NotBeNull();
        http.Data.First().User.Should().BeNull();
        http.Data.First().PostUser.Should().NotBeNull();
        http.Data.First().PostEnterprise.Should().BeNull();
        http.Data.First().CommentUser.Should().BeNull();
        http.Data.First().CommentEnterprise.Should().BeNull();
    }
}
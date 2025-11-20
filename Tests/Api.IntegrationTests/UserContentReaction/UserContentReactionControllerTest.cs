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
    
}
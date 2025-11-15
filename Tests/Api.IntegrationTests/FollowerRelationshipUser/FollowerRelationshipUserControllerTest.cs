using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using JobVacancy.API.IntegrationTests.Utils;
using JobVacancy.API.models.dtos.FollowerRelationshipUser;
using JobVacancy.API.Utils.Page;
using JobVacancy.API.Utils.Res;
using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;

namespace JobVacancy.API.IntegrationTests.FollowerRelationshipUser;

public class FollowerRelationshipUserControllerTest: IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly Helper _helper;
    private readonly IConfiguration _configuration;
    private readonly string _url = "/api/v1/FollowerRelationshipUser";
    private readonly ITestOutputHelper _output;

    public FollowerRelationshipUserControllerTest(CustomWebApplicationFactory factory, ITestOutputHelper output) 
    {
        _client = factory.CreateClient(); 
        _helper = new Helper(_client); 
        _configuration = factory.Configuration;
        _output = output;
    }

    [Fact]
    public async Task Create()
    {
        UserResultTest follower = await _helper.CreateAndGetUser();
        UserResultTest followed = await _helper.CreateAndGetUser();
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", follower.Tokens!.Token);

        HttpResponseMessage message = await _client.PostAsync($"{_url}/{followed.User!.Id}/Toggle", null);
        message.StatusCode.Should().Be(HttpStatusCode.Created);

        ResponseHttp<FollowerRelationshipUserDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<FollowerRelationshipUserDto>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int) HttpStatusCode.Created);
        http.Status.Should().BeTrue();
        
        http.Data.Should().NotBeNull();
        http.Data.Id.Should().NotBeNullOrWhiteSpace();
        http.Data.FollowedId.Should().Be(followed.User.Id);
        http.Data.FollowerId.Should().Be(follower.User!.Id);
    }
    
    [Fact]
    public async Task CreateNotFound()
    {
        UserResultTest follower = await _helper.CreateAndGetUser();
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", follower.Tokens!.Token);

        HttpResponseMessage message = await _client.PostAsync($"{_url}/{Guid.NewGuid()}/Toggle", null);
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int) HttpStatusCode.NotFound);
        http.Status.Should().BeFalse();
        
        http.Data.Should().BeNull();
    }
    
    [Fact]
    public async Task Remove()
    {
        UserResultTest follower = await _helper.CreateAndGetUser();
        UserResultTest followed = await _helper.CreateAndGetUser();
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", follower.Tokens!.Token);
        await _helper.CreateFollowerRelationshipUser(follower, followed);
        
        HttpResponseMessage message = await _client.PostAsync($"{_url}/{followed.User!.Id}/Toggle", null);
        message.StatusCode.Should().Be(HttpStatusCode.NoContent);

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int) HttpStatusCode.NoContent);
        http.Status.Should().BeTrue();
        
        http.Data.Should().BeNull();
    }
    
    [Fact]
    public async Task ExistsTrue()
    {
        UserResultTest follower = await _helper.CreateAndGetUser();
        UserResultTest followed = await _helper.CreateAndGetUser();
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", follower.Tokens!.Token);
        await _helper.CreateFollowerRelationshipUser(follower, followed);
        
        HttpResponseMessage message = await _client.GetAsync($"{_url}/{followed.User!.Id}/Exists");
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<bool>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<bool>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int) HttpStatusCode.OK);
        http.Status.Should().BeTrue();
        
        http.Data.Should().BeTrue();
    }
    
    [Fact]
    public async Task ExistsFalse()
    {
        UserResultTest follower = await _helper.CreateAndGetUser();
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", follower.Tokens!.Token);
        
        HttpResponseMessage message = await _client.GetAsync($"{_url}/{Guid.NewGuid()}/Exists");
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<bool>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<bool>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int) HttpStatusCode.OK);
        http.Status.Should().BeTrue();
        
        http.Data.Should().BeFalse();
    }

    [Fact]
    public async Task Update()
    {
        UserResultTest follower = await _helper.CreateAndGetUser();
        UserResultTest followed = await _helper.CreateAndGetUser();
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", follower.Tokens!.Token);
        FollowerRelationshipUserDto followOriginal = await _helper.CreateFollowerRelationshipUser(follower, followed);

        UpdateFollowerRelationshipUserDto dto = new UpdateFollowerRelationshipUserDto()
        {
            WishReceiveNotifyByNewComment = false,
            WishReceiveNotifyByNewInteraction = false,
            WishReceiveNotifyByNewPost = false
        };
        
        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_url}/{followOriginal.Id}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<FollowerRelationshipUserDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<FollowerRelationshipUserDto>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int) HttpStatusCode.OK);
        http.Status.Should().BeTrue();
        
        http.Data.Should().NotBeNull();
        http.Data.Id.Should().Be(followOriginal.Id);
        http.Data.FollowedId.Should().Be(followed.User!.Id);
        http.Data.FollowerId.Should().Be(follower.User!.Id);
        http.Data.WishReceiveNotifyByNewComment.Should().Be(dto.WishReceiveNotifyByNewComment.Value);
        http.Data.WishReceiveNotifyByNewInteraction.Should().Be(dto.WishReceiveNotifyByNewInteraction.Value);
        http.Data.WishReceiveNotifyByNewPost.Should().Be(dto.WishReceiveNotifyByNewPost.Value);
    }
    
    [Fact]
    public async Task UpdateReturnForb()
    {
        UserResultTest follower = await _helper.CreateAndGetUser();
        UserResultTest followed = await _helper.CreateAndGetUser();
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", follower.Tokens!.Token);
        FollowerRelationshipUserDto followOriginal = await _helper.CreateFollowerRelationshipUser(follower, followed);

        UpdateFollowerRelationshipUserDto dto = new UpdateFollowerRelationshipUserDto()
        {
            WishReceiveNotifyByNewComment = false,
            WishReceiveNotifyByNewInteraction = false,
            WishReceiveNotifyByNewPost = false
        };
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", followed.Tokens!.Token);
        
        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_url}/{followOriginal.Id}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.Forbidden);

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int) HttpStatusCode.Forbidden);
        http.Status.Should().BeFalse();
        
        http.Data.Should().BeNull();
    }
    
    [Fact]
    public async Task UpdateNotFound()
    {
        UserResultTest follower = await _helper.CreateAndGetUser();
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", follower.Tokens!.Token);

        UpdateFollowerRelationshipUserDto dto = new UpdateFollowerRelationshipUserDto()
        {
            WishReceiveNotifyByNewComment = false,
            WishReceiveNotifyByNewInteraction = false,
            WishReceiveNotifyByNewPost = false
        };
        
        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_url}/{Guid.NewGuid()}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        
        http.Code.Should().Be((int) HttpStatusCode.NotFound);
        http.Status.Should().BeFalse();
        
        http.Data.Should().BeNull();
    }

    [Fact]
    public async Task GetAllSimple()
    {
        UserResultTest follower = await _helper.CreateAndGetUser();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", follower.Tokens!.Token);
        
        HttpResponseMessage message = await _client.GetAsync($"{_url}");
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        _output.WriteLine(message.Content.ReadAsStringAsync().Result);

        Page<FollowerRelationshipUserDto>? page = await message.Content.ReadFromJsonAsync<Page<FollowerRelationshipUserDto>>();
        
        page.Should().NotBeNull();
        page.PageIndex.Should().Be(1);
        page.PageSize.Should().Be(10);
    }
    
    [Fact]
    public async Task GetAllLoadJustFollower()
    {
        UserResultTest follower = await _helper.CreateAndGetUser();
        UserResultTest followed = await _helper.CreateAndGetUser();
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", follower.Tokens!.Token);
        FollowerRelationshipUserDto followOriginal = await _helper.CreateFollowerRelationshipUser(follower, followed);
        
        HttpResponseMessage message = await _client.GetAsync($"{_url}?LoadFollower={true}");
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        _output.WriteLine(message.Content.ReadAsStringAsync().Result);

        Page<FollowerRelationshipUserDto>? page = await message.Content.ReadFromJsonAsync<Page<FollowerRelationshipUserDto>>();
        
        page.Should().NotBeNull();
        page.PageIndex.Should().Be(1);
        page.PageSize.Should().Be(10);

        page.Data.Should().NotBeNull();
        page.Data.First().Should().NotBeNull();
        page.Data.First().Follower.Should().NotBeNull();
        page.Data.First().Followed.Should().BeNull();
    }
    [Fact]
    public async Task GetAllLoadJustFollowed()
    {
        UserResultTest follower = await _helper.CreateAndGetUser();
        UserResultTest followed = await _helper.CreateAndGetUser();
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", follower.Tokens!.Token);
        FollowerRelationshipUserDto followOriginal = await _helper.CreateFollowerRelationshipUser(follower, followed);
        
        HttpResponseMessage message = await _client.GetAsync($"{_url}?LoadFollowed={true}");
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        Page<FollowerRelationshipUserDto>? page = await message.Content.ReadFromJsonAsync<Page<FollowerRelationshipUserDto>>();
        
        page.Should().NotBeNull();
        page.PageIndex.Should().Be(1);
        page.PageSize.Should().Be(10);

        page.Data.Should().NotBeNull();
        page.Data.First().Should().NotBeNull();
        page.Data.First().Followed.Should().NotBeNull();
        page.Data.First().Follower.Should().BeNull();
    }
    
}
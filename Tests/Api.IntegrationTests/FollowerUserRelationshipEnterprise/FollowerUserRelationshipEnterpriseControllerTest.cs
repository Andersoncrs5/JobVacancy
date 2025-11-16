using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using JobVacancy.API.IntegrationTests.Utils;
using JobVacancy.API.models.dtos.Enterprise;
using JobVacancy.API.models.dtos.FollowerUserRelationshipEnterprise;
using JobVacancy.API.models.dtos.Industry;
using JobVacancy.API.Utils.Page;
using JobVacancy.API.Utils.Res;
using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;

namespace JobVacancy.API.IntegrationTests.FollowerUserRelationshipEnterprise;

public class FollowerUserRelationshipEnterpriseControllerTest: IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly Helper _helper;
    private readonly IConfiguration _configuration;
    private readonly string _url = "/api/v1/FollowerUserRelationshipEnterprise";
    private readonly ITestOutputHelper _output;

    public FollowerUserRelationshipEnterpriseControllerTest(CustomWebApplicationFactory factory, ITestOutputHelper output) 
    {
        _client = factory.CreateClient(); 
        _helper = new Helper(_client); 
        _configuration = factory.Configuration;
        _output = output;
    }

    [Fact]
    public async Task ExistsReturnTrue()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        UserResultTest userFollowed = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userFollowed.Tokens!.Token!);
        await _helper.AddUserFollowEnterprise(enterprise, userFollowed);

        HttpResponseMessage message = await _client.GetAsync($"{_url}/{enterprise.Id}/Exists");
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        
        ResponseHttp<bool>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<bool>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int) HttpStatusCode.OK);
        http.Status.Should().BeTrue();
        http.Message.Should().BeNullOrWhiteSpace();
        
        http.Data.Should().BeTrue();
    }
    
    [Fact]
    public async Task ExistsReturnFalse()
    {
        UserResultTest user = await _helper.CreateAndGetUser();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Tokens!.Token!);
        
        HttpResponseMessage message = await _client.GetAsync($"{_url}/{Guid.NewGuid()}/Exists");
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        
        ResponseHttp<bool>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<bool>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int) HttpStatusCode.OK);
        http.Status.Should().BeTrue();
        http.Message.Should().BeNullOrWhiteSpace();
        
        http.Data.Should().BeFalse();
    }
    
    [Fact]
    public async Task CreateNotFoundEnterprise()
    {
        UserResultTest user = await _helper.CreateAndGetUser();
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Tokens!.Token!);

        HttpResponseMessage message = await _client.PostAsync($"{_url}/{Guid.NewGuid()}/Toggle", null);
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int) HttpStatusCode.NotFound);
        http.Status.Should().BeFalse();
        
        http.Data.Should().BeNull();
    }

    [Fact]
    public async Task Create()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        UserResultTest userFollowed = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userFollowed.Tokens!.Token!);

        HttpResponseMessage message = await _client.PostAsync($"{_url}/{enterprise.Id}/Toggle", null);
        message.StatusCode.Should().Be(HttpStatusCode.Created);

        ResponseHttp<FollowerUserRelationshipEnterpriseDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<FollowerUserRelationshipEnterpriseDto>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int) HttpStatusCode.Created);
        http.Status.Should().BeTrue();
        
        http.Data.Should().NotBeNull();
        http.Data.UserId.Should().Be(userFollowed.User!.Id);
        http.Data.EnterpriseId.Should().Be(enterprise.Id);
        http.Data.WishReceiveNotifyByNewComment.Should().BeTrue();
        http.Data.WishReceiveNotifyByNewPost.Should().BeTrue();
        http.Data.WishReceiveNotifyByNewVacancy.Should().BeTrue();
        http.Data.WishReceiveNotifyByNewInteraction.Should().BeFalse();
    }

    [Fact]
    public async Task Remove()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        UserResultTest userFollowed = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userFollowed.Tokens!.Token!);
        await _helper.AddUserFollowEnterprise(enterprise, userFollowed);

        HttpResponseMessage message = await _client.PostAsync($"{_url}/{enterprise.Id}/Toggle", null);
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        
        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int) HttpStatusCode.OK);
        http.Status.Should().BeTrue();
        http.Message.Should().NotBeNullOrWhiteSpace();
        
        http.Data.Should().BeNull();
    }
    
    [Fact]
    public async Task Patch()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        UserResultTest userFollowed = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userFollowed.Tokens!.Token!);
        FollowerUserRelationshipEnterpriseDto follow = await _helper.AddUserFollowEnterprise(enterprise, userFollowed);

        UpdateFollowerUserRelationshipEnterpriseDto dto = new UpdateFollowerUserRelationshipEnterpriseDto()
        {
            WishReceiveNotifyByNewComment = !follow.WishReceiveNotifyByNewComment,
            WishReceiveNotifyByNewInteraction = !follow.WishReceiveNotifyByNewInteraction,
            WishReceiveNotifyByNewVacancy = !follow.WishReceiveNotifyByNewVacancy,
            WishReceiveNotifyByNewPost = !follow.WishReceiveNotifyByNewPost,
        };
        
        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_url}/{follow.Id}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        
        ResponseHttp<FollowerUserRelationshipEnterpriseDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<FollowerUserRelationshipEnterpriseDto>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int) HttpStatusCode.OK);
        http.Status.Should().BeTrue();
        http.Message.Should().NotBeNullOrWhiteSpace();
        
        http.Data.Should().NotBeNull();
        http.Data.UserId.Should().Be(userFollowed.User!.Id);
        http.Data.EnterpriseId.Should().Be(enterprise.Id);
        http.Data.WishReceiveNotifyByNewComment.Should().Be(dto.WishReceiveNotifyByNewComment.Value);
        http.Data.WishReceiveNotifyByNewPost.Should().Be(dto.WishReceiveNotifyByNewPost.Value);
        http.Data.WishReceiveNotifyByNewVacancy.Should().Be(dto.WishReceiveNotifyByNewVacancy.Value);
        http.Data.WishReceiveNotifyByNewInteraction.Should().Be(dto.WishReceiveNotifyByNewInteraction.Value);
    }
    
    [Fact]
    public async Task PatchNotFound()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        UserResultTest userFollowed = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userFollowed.Tokens!.Token!);
        FollowerUserRelationshipEnterpriseDto follow = await _helper.AddUserFollowEnterprise(enterprise, userFollowed);

        UpdateFollowerUserRelationshipEnterpriseDto dto = new UpdateFollowerUserRelationshipEnterpriseDto()
        {
            WishReceiveNotifyByNewComment = !follow.WishReceiveNotifyByNewComment,
            WishReceiveNotifyByNewInteraction = !follow.WishReceiveNotifyByNewInteraction,
            WishReceiveNotifyByNewVacancy = !follow.WishReceiveNotifyByNewVacancy,
            WishReceiveNotifyByNewPost = !follow.WishReceiveNotifyByNewPost,
        };
        
        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_url}/{Guid.NewGuid()}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);
        
        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int) HttpStatusCode.NotFound);
        http.Status.Should().BeFalse();
        http.Message.Should().NotBeNullOrWhiteSpace();
        
        http.Data.Should().BeNull();
    }
    
    [Fact]
    public async Task PatchForbidden()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        UserResultTest userFollowed = await _helper.CreateAndGetUser();
        UserResultTest userConflicted = await _helper.CreateAndGetUser();
        
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userFollowed.Tokens!.Token!);
        FollowerUserRelationshipEnterpriseDto follow = await _helper.AddUserFollowEnterprise(enterprise, userFollowed);

        UpdateFollowerUserRelationshipEnterpriseDto dto = new UpdateFollowerUserRelationshipEnterpriseDto()
        {
            WishReceiveNotifyByNewComment = !follow.WishReceiveNotifyByNewComment,
            WishReceiveNotifyByNewInteraction = !follow.WishReceiveNotifyByNewInteraction,
            WishReceiveNotifyByNewVacancy = !follow.WishReceiveNotifyByNewVacancy,
            WishReceiveNotifyByNewPost = !follow.WishReceiveNotifyByNewPost,
        };
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userConflicted.Tokens!.Token!);
        
        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_url}/{follow.Id}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        
        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int) HttpStatusCode.Forbidden);
        http.Status.Should().BeFalse();
        http.Message.Should().NotBeNullOrWhiteSpace();
        
        http.Data.Should().BeNull();
    }

    [Fact]
    public async Task GetAllSimple()
    {
        UserResultTest follower = await _helper.CreateAndGetUser();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", follower.Tokens!.Token);
        
        HttpResponseMessage message = await _client.GetAsync($"{_url}");
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        Page<FollowerUserRelationshipEnterpriseDto>? page = await message.Content.ReadFromJsonAsync<Page<FollowerUserRelationshipEnterpriseDto>>();
        
        page.Should().NotBeNull();
        page.PageIndex.Should().Be(1);
        page.PageSize.Should().Be(10);
    }
    
    [Fact]
    public async Task GetAllJustLoadUser()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        UserResultTest userFollowed = await _helper.CreateAndGetUser();
        
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userFollowed.Tokens!.Token!);
        await _helper.AddUserFollowEnterprise(enterprise, userFollowed);
        
        HttpResponseMessage message = await _client.GetAsync($"{_url}?LoadUser={true}");
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        _output.WriteLine(message.Content.ReadAsStringAsync().Result);

        Page<FollowerUserRelationshipEnterpriseDto>? page = await message.Content.ReadFromJsonAsync<Page<FollowerUserRelationshipEnterpriseDto>>();
        
        page.Should().NotBeNull();
        page.PageIndex.Should().Be(1);
        page.PageSize.Should().Be(10);
        
        page.Data.Should().NotBeNull();
        page.Data.First().Should().NotBeNull();
        page.Data.First().User.Should().NotBeNull();
        page.Data.First().Enterprise.Should().BeNull();
    }

    [Fact]
    public async Task GetAllJustLoadEnterprise()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        UserResultTest userFollowed = await _helper.CreateAndGetUser();
        
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userFollowed.Tokens!.Token!);
        await _helper.AddUserFollowEnterprise(enterprise, userFollowed);
        
        HttpResponseMessage message = await _client.GetAsync($"{_url}?LoadEnterprise={true}");
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        _output.WriteLine(message.Content.ReadAsStringAsync().Result);

        Page<FollowerUserRelationshipEnterpriseDto>? page = await message.Content.ReadFromJsonAsync<Page<FollowerUserRelationshipEnterpriseDto>>();
        
        page.Should().NotBeNull();
        page.PageIndex.Should().Be(1);
        page.PageSize.Should().Be(10);
        
        page.Data.Should().NotBeNull();
        page.Data.First().Should().NotBeNull();
        page.Data.First().User.Should().BeNull();
        page.Data.First().Enterprise.Should().NotBeNull();
    }

    
}
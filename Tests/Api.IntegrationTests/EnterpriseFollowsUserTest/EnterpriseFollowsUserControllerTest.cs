using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using JobVacancy.API.IntegrationTests.Utils;
using JobVacancy.API.models.dtos.Enterprise;
using JobVacancy.API.models.dtos.EnterpriseFollowsUser;
using JobVacancy.API.models.dtos.Industry;
using JobVacancy.API.Utils.Page;
using JobVacancy.API.Utils.Res;
using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;

namespace JobVacancy.API.IntegrationTests.EnterpriseFollowsUserTest;

public class EnterpriseFollowsUserControllerTest: IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly Helper _helper;
    private readonly IConfiguration _configuration;
    private readonly string _URL = "/api/v1/EnterpriseFollowsUser";
    private readonly ITestOutputHelper _output;

    public EnterpriseFollowsUserControllerTest(CustomWebApplicationFactory factory, ITestOutputHelper output) 
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
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        UserResultTest userToFollow = await _helper.CreateAndGetUser();
        
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);

        var userEnterprise = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userEnterprise.Token!);

        HttpResponseMessage message = await _client.PostAsync($"{_URL}/{userToFollow.User!.Id}", null);
        message.StatusCode.Should().Be(HttpStatusCode.Created);

        ResponseHttp<EnterpriseFollowsUserDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<EnterpriseFollowsUserDto>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int)HttpStatusCode.Created);
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Status.Should().BeTrue();
        
        http.Data.Should().NotBeNull();
        http.Data.Id.Should().NotBeNullOrWhiteSpace();
        http.Data.EnterpriseId.Should().Be(enterprise.Id);
        http.Data.UserId.Should().Be(userToFollow.User.Id);
    }

    [Fact]
    public async Task Remove()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        UserResultTest userToFollow = await _helper.CreateAndGetUser();
        
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);

        var userEnterprise = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userEnterprise.Token!);

        await _helper.AddEnterpriseFollowsUser(userToFollow, enterprise);
        
        HttpResponseMessage message = await _client.PostAsync($"{_URL}/{userToFollow.User!.Id}", null);
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int)HttpStatusCode.OK);
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Status.Should().BeTrue();
        
        http.Data.Should().BeNull();
    }
    
    [Fact]
    public async Task CreateReturnNotFoundUser()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        UserResultTest userToFollow = await _helper.CreateAndGetUser();
        
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);

        var userEnterprise = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userEnterprise.Token!);

        HttpResponseMessage message = await _client.PostAsync($"{_URL}/{Guid.NewGuid()}", null);
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int) HttpStatusCode.NotFound);
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Status.Should().BeFalse();
    }
    
    [Fact]
    public async Task ExistsTrue()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        UserResultTest userToFollow = await _helper.CreateAndGetUser();
        
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);

        var userEnterprise = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userEnterprise.Token!);

        await _helper.AddEnterpriseFollowsUser(userToFollow, enterprise);
        
        HttpResponseMessage message = await _client.GetAsync($"{_URL}/{userToFollow.User!.Id}/{enterprise.Id}");
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<bool>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<bool>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int)HttpStatusCode.OK);
        http.Message.Should().BeNullOrWhiteSpace();
        http.Status.Should().BeTrue();
        
        http.Data.Should().BeTrue();
    }

    [Fact]
    public async Task ExistsFalse()
    {
        UserResultTest user = await _helper.CreateAndGetUser();
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Tokens!.Token!);
        
        HttpResponseMessage message = await _client.GetAsync($"{_URL}/{Guid.NewGuid()}/{Guid.NewGuid()}");
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<bool>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<bool>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int)HttpStatusCode.OK);
        http.Message.Should().BeNullOrWhiteSpace();
        http.Status.Should().BeTrue();
        
        http.Data.Should().BeFalse();
    }

    [Fact]
    public async Task Update() 
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        UserResultTest userToFollow = await _helper.CreateAndGetUser();
        
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);

        var userEnterprise = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userEnterprise.Token!);

        EnterpriseFollowsUserDto follow = await _helper.AddEnterpriseFollowsUser(userToFollow, enterprise);

        UpdateEnterpriseFollowsUserDto dto = new UpdateEnterpriseFollowsUserDto
        {
            WishReceiveNotifyByNewEndorsement = !follow.WishReceiveNotifyByNewEndorsement,
            WishReceiveNotifyByNewInteraction = !follow.WishReceiveNotifyByNewInteraction,
            WishReceiveNotifyByNewPost = !follow.WishReceiveNotifyByNewPost,
            WishReceiveNotifyByProfileUpdate = !follow.WishReceiveNotifyByProfileUpdate,
        };
        
        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_URL}/{follow.Id}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<EnterpriseFollowsUserDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<EnterpriseFollowsUserDto>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int)HttpStatusCode.OK);
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Status.Should().BeTrue();
        
        http.Data.Should().NotBeNull();
        http.Data.Id.Should().Be(follow.Id);
        http.Data.EnterpriseId.Should().Be(follow.EnterpriseId);
        http.Data.UserId.Should().Be(follow.UserId);
        http.Data.WishReceiveNotifyByNewEndorsement.Should().Be(dto.WishReceiveNotifyByNewEndorsement.Value);
        http.Data.WishReceiveNotifyByNewInteraction.Should().Be(dto.WishReceiveNotifyByNewInteraction.Value);
        http.Data.WishReceiveNotifyByNewPost.Should().Be(dto.WishReceiveNotifyByNewPost.Value);
        http.Data.WishReceiveNotifyByProfileUpdate.Should().Be(dto.WishReceiveNotifyByProfileUpdate.Value);
    }
    
    [Fact]
    public async Task GetAllSimple()
    {
        UserResultTest follower = await _helper.CreateAndGetUser();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", follower.Tokens!.Token);
        
        HttpResponseMessage message = await _client.GetAsync($"{_URL}");
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        Page<EnterpriseFollowsUserDto>? page = await message.Content.ReadFromJsonAsync<Page<EnterpriseFollowsUserDto>>();
        
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
        UserResultTest userToFollow = await _helper.CreateAndGetUser();
        
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);

        var userEnterprise = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userEnterprise.Token!);

        await _helper.AddEnterpriseFollowsUser(userToFollow, enterprise);
        
        HttpResponseMessage message = await _client.GetAsync($"{_URL}?LoadUser={true}");
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        Page<EnterpriseFollowsUserDto>? page = await message.Content.ReadFromJsonAsync<Page<EnterpriseFollowsUserDto>>();
        
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
        UserResultTest userToFollow = await _helper.CreateAndGetUser();
        
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);

        var userEnterprise = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userEnterprise.Token!);

        await _helper.AddEnterpriseFollowsUser(userToFollow, enterprise);
        
        HttpResponseMessage message = await _client.GetAsync($"{_URL}?LoadEnterprise={true}");
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        Page<EnterpriseFollowsUserDto>? page = await message.Content.ReadFromJsonAsync<Page<EnterpriseFollowsUserDto>>();
        
        page.Should().NotBeNull();
        page.PageIndex.Should().Be(1);
        page.PageSize.Should().Be(10);
        
        page.Data.Should().NotBeNull();
        page.Data.First().Should().NotBeNull();
        page.Data.First().User.Should().BeNull();
        page.Data.First().Enterprise.Should().NotBeNull();
    }

    
    
}
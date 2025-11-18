using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using JobVacancy.API.IntegrationTests.Utils;
using JobVacancy.API.models.dtos.ReviewUser;
using JobVacancy.API.Utils.Page;
using JobVacancy.API.Utils.Res;
using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;

namespace JobVacancy.API.IntegrationTests.ReviewUser;

public class ReviewUserControllerTest: IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly Helper _helper;
    private readonly IConfiguration _configuration;
    private readonly string _url = "/api/v1/ReviewUser";
    private readonly ITestOutputHelper _output;
    
    public ReviewUserControllerTest(CustomWebApplicationFactory factory, ITestOutputHelper output) 
    {
        _client = factory.CreateClient(); 
        _helper = new Helper(_client); 
        _configuration = factory.Configuration;
        _output = output;
    }

    [Fact]
    public async Task Create()
    {
        UserResultTest actor = await _helper.CreateAndGetUser();
        UserResultTest target = await _helper.CreateAndGetUser();

        CreateReviewUserDto dto = new CreateReviewUserDto()
        {
            TargetUserId = target.User!.Id,
            Content = string.Concat(Enumerable.Repeat("Content", 20)),
            IsAnonymous = false,
            RatingCompensation = 4,
            RatingCulture = 4,
            RatingManagement = 4,
            RatingOverall = 4,
            RatingWorkLifeBalance = 4,
            Recommendation = true,
            Title = string.Concat(Enumerable.Repeat("Title", 10)),
        };

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", actor.Tokens!.Token!);

        HttpResponseMessage message = await _client.PostAsJsonAsync($"{_url}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.Created);

        ResponseHttp<ReviewUserDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<ReviewUserDto>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int)HttpStatusCode.Created);
        http.Status.Should().BeTrue();
        http.Message.Should().NotBeNullOrWhiteSpace();
        
        http.Data.Should().NotBeNull();
        http.Data.Id.Should().NotBeNullOrWhiteSpace();
        http.Data.Title.Should().Be(dto.Title);
        http.Data.IsAnonymous.Should().Be(dto.IsAnonymous);
        http.Data.RatingCompensation.Should().Be(dto.RatingCompensation);
        http.Data.RatingCulture.Should().Be(dto.RatingCulture);
        http.Data.RatingManagement.Should().Be(dto.RatingManagement);
        http.Data.RatingOverall.Should().Be(dto.RatingOverall);
        http.Data.RatingWorkLifeBalance.Should().Be(dto.RatingWorkLifeBalance);
        http.Data.Recommendation.Should().Be(dto.Recommendation);
        http.Data.TargetUserId.Should().Be(dto.TargetUserId);
        http.Data.Content.Should().Be(dto.Content);
    }
    
    [Fact]
    public async Task CreateConflict()
    {
        UserResultTest actor = await _helper.CreateAndGetUser();
        UserResultTest target = await _helper.CreateAndGetUser();

        CreateReviewUserDto dto = new CreateReviewUserDto()
        {
            TargetUserId = target.User!.Id,
            Content = string.Concat(Enumerable.Repeat("Content", 20)),
            IsAnonymous = false,
            RatingCompensation = 4,
            RatingCulture = 4,
            RatingManagement = 4,
            RatingOverall = 4,
            RatingWorkLifeBalance = 4,
            Recommendation = true,
            Title = string.Concat(Enumerable.Repeat("Title", 10)),
        };

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", actor.Tokens!.Token!);
        await _helper.CreateReviewToUser(target, actor);

        HttpResponseMessage message = await _client.PostAsJsonAsync($"{_url}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.Conflict);

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int)HttpStatusCode.Conflict);
        http.Status.Should().BeFalse();
        http.Message.Should().NotBeNullOrWhiteSpace();
        
        http.Data.Should().BeNull();
    }

    [Fact]
    public async Task Get()
    {
        UserResultTest actor = await _helper.CreateAndGetUser();
        UserResultTest target = await _helper.CreateAndGetUser();
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", actor.Tokens!.Token!);
        ReviewUserDto reviewDto = await _helper.CreateReviewToUser(target, actor);

        HttpResponseMessage message = await _client.GetAsync($"{_url}/{reviewDto.Id}");
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<ReviewUserDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<ReviewUserDto>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int)HttpStatusCode.OK);
        http.Status.Should().BeTrue();
        http.Message.Should().NotBeNullOrWhiteSpace();
        
        http.Data.Should().NotBeNull();
        http.Data.Id.Should().Be(reviewDto.Id);
        http.Data.Title.Should().Be(reviewDto.Title);
        http.Data.IsAnonymous.Should().Be(reviewDto.IsAnonymous);
        http.Data.RatingCompensation.Should().Be(reviewDto.RatingCompensation);
        http.Data.RatingCulture.Should().Be(reviewDto.RatingCulture);
        http.Data.RatingManagement.Should().Be(reviewDto.RatingManagement);
        http.Data.RatingOverall.Should().Be(reviewDto.RatingOverall);
        http.Data.RatingWorkLifeBalance.Should().Be(reviewDto.RatingWorkLifeBalance);
        http.Data.Recommendation.Should().Be(reviewDto.Recommendation);
        http.Data.TargetUserId.Should().Be(reviewDto.TargetUserId);
        http.Data.Content.Should().Be(reviewDto.Content);
    }

    [Fact]
    public async Task GetNotFound() 
    {
        UserResultTest actor = await _helper.CreateAndGetUser();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", actor.Tokens!.Token!);

        HttpResponseMessage message = await _client.GetAsync($"{_url}/{Guid.NewGuid()}");
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int)HttpStatusCode.NotFound);
        http.Status.Should().BeFalse();
        http.Message.Should().NotBeNullOrWhiteSpace();

        http.Data.Should().BeNull();
    }

    [Fact]
    public async Task Del() 
    {
        UserResultTest actor = await _helper.CreateAndGetUser();
        UserResultTest target = await _helper.CreateAndGetUser();
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", actor.Tokens!.Token!);
        ReviewUserDto reviewDto = await _helper.CreateReviewToUser(target, actor);

        HttpResponseMessage message = await _client.DeleteAsync($"{_url}/{reviewDto.Id}");
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int)HttpStatusCode.OK);
        http.Status.Should().BeTrue();
        http.Message.Should().NotBeNullOrWhiteSpace();
        
        http.Data.Should().BeNull();
    }
    
    [Fact]
    public async Task DelNotFound() 
    {
        UserResultTest actor = await _helper.CreateAndGetUser();
        UserResultTest target = await _helper.CreateAndGetUser();
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", actor.Tokens!.Token!);

        HttpResponseMessage message = await _client.DeleteAsync($"{_url}/{Guid.NewGuid()}");
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int)HttpStatusCode.NotFound);
        http.Status.Should().BeFalse();
        http.Message.Should().NotBeNullOrWhiteSpace();

        http.Data.Should().BeNull();
    }
    
    [Fact]
    public async Task DelReturnForb() 
    {
        UserResultTest actor = await _helper.CreateAndGetUser();
        UserResultTest target = await _helper.CreateAndGetUser();
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", actor.Tokens!.Token!);
        ReviewUserDto reviewDto = await _helper.CreateReviewToUser(target, actor);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", target.Tokens!.Token!);
        
        HttpResponseMessage message = await _client.DeleteAsync($"{_url}/{reviewDto.Id}");
        message.StatusCode.Should().Be(HttpStatusCode.Forbidden);

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int)HttpStatusCode.Forbidden);
        http.Status.Should().BeFalse();
        http.Message.Should().NotBeNullOrWhiteSpace();

        http.Data.Should().BeNull();
    }
    
    [Fact]
    public async Task Patch()
    {
        UserResultTest actor = await _helper.CreateAndGetUser();
        UserResultTest target = await _helper.CreateAndGetUser();
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", actor.Tokens!.Token!);
        ReviewUserDto reviewDto = await _helper.CreateReviewToUser(target, actor);

        UpdateReviewUserDto dto = new UpdateReviewUserDto()
        {
            Content = string.Concat(Enumerable.Repeat("ABC", 10)),
            IsAnonymous = !reviewDto.IsAnonymous,
            RatingCompensation = 1,
            RatingCulture = 1,
            RatingManagement = 1,
            RatingOverall = 1,
            RatingWorkLifeBalance = 1,
            Recommendation = !reviewDto.Recommendation,
            Title = string.Concat(Enumerable.Repeat("ABC", 10)),
        };
        
        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_url}/{reviewDto.Id}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<ReviewUserDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<ReviewUserDto>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int)HttpStatusCode.OK);
        http.Status.Should().BeTrue();
        http.Message.Should().NotBeNullOrWhiteSpace();
        
        http.Data.Should().NotBeNull();
        http.Data.Id.Should().Be(reviewDto.Id);
        http.Data.Title.Should().Be(dto.Title);
        http.Data.IsAnonymous.Should().Be(dto.IsAnonymous.Value);
        http.Data.RatingCompensation.Should().Be(dto.RatingCompensation);
        http.Data.RatingCulture.Should().Be(dto.RatingCulture);
        http.Data.RatingManagement.Should().Be(dto.RatingManagement);
        http.Data.RatingOverall.Should().Be(dto.RatingOverall);
        http.Data.RatingWorkLifeBalance.Should().Be(dto.RatingWorkLifeBalance);
        http.Data.Recommendation.Should().Be(dto.Recommendation);
        http.Data.TargetUserId.Should().Be(reviewDto.TargetUserId);
        http.Data.Content.Should().Be(dto.Content);
    }

    [Fact]
    public async Task PatchNotFound()
    {
        UserResultTest actor = await _helper.CreateAndGetUser();
        UserResultTest target = await _helper.CreateAndGetUser();
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", actor.Tokens!.Token!);

        UpdateReviewUserDto dto = new UpdateReviewUserDto()
        {
            Content = string.Concat(Enumerable.Repeat("ABC", 10)),
            IsAnonymous = false,
            RatingCompensation = 1,
            RatingCulture = 1,
            RatingManagement = 1,
            RatingOverall = 1,
            RatingWorkLifeBalance = 1,
            Recommendation = false,
            Title = string.Concat(Enumerable.Repeat("ABC", 10)),
        };
        
        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_url}/{Guid.NewGuid()}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int)HttpStatusCode.NotFound);
        http.Status.Should().BeFalse();
        http.Message.Should().NotBeNullOrWhiteSpace();
        
        http.Data.Should().BeNull();
    }

    [Fact]
    public async Task PatchReturnConflict()
    {
        UserResultTest actor = await _helper.CreateAndGetUser();
        UserResultTest target = await _helper.CreateAndGetUser();
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", actor.Tokens!.Token!);
        ReviewUserDto reviewDto = await _helper.CreateReviewToUser(target, actor);
        
        UpdateReviewUserDto dto = new UpdateReviewUserDto()
        {
            Content = string.Concat(Enumerable.Repeat("ABC", 10)),
            IsAnonymous = false,
            RatingCompensation = 1,
            RatingCulture = 1,
            RatingManagement = 1,
            RatingOverall = 1,
            RatingWorkLifeBalance = 1,
            Recommendation = false,
            Title = string.Concat(Enumerable.Repeat("ABC", 10)),
        };
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", target.Tokens!.Token!);
        
        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_url}/{reviewDto.Id}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.Forbidden);

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int)HttpStatusCode.Forbidden);
        http.Status.Should().BeFalse();
        http.Message.Should().NotBeNullOrWhiteSpace();
        
        http.Data.Should().BeNull();
    }

    [Fact]
    public async Task GetAllSimple()
    {
        UserResultTest actor = await _helper.CreateAndGetUser();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", actor.Tokens!.Token!);
        
        HttpResponseMessage message = await _client.GetAsync($"{_url}");
        _output.WriteLine(await message.Content.ReadAsStringAsync());
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        Page<object>? http = await message.Content.ReadFromJsonAsync<Page<object>>();
        http.Should().NotBeNull();
    }
    
    [Fact]
    public async Task GetAllJustLoadTarget()
    {
        UserResultTest actor = await _helper.CreateAndGetUser();
        UserResultTest target = await _helper.CreateAndGetUser();
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", actor.Tokens!.Token!);
        ReviewUserDto reviewDto = await _helper.CreateReviewToUser(target, actor);
        
        HttpResponseMessage message = await _client.GetAsync($"{_url}?LoadTarget={true}");
        _output.WriteLine(await message.Content.ReadAsStringAsync());
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        Page<ReviewUserDto>? http = await message.Content.ReadFromJsonAsync<Page<ReviewUserDto>>();
        http.Should().NotBeNull();
        http.Data.Should().NotBeNull();
        
        http.Data.First().Should().NotBeNull();
        http.Data.First().TargetUser.Should().NotBeNull();
        http.Data.First().Actor.Should().BeNull();
    }
    
    [Fact]
    public async Task GetAllJustLoadActor()
    {
        UserResultTest actor = await _helper.CreateAndGetUser();
        UserResultTest target = await _helper.CreateAndGetUser();
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", actor.Tokens!.Token!);
        ReviewUserDto reviewDto = await _helper.CreateReviewToUser(target, actor);
        
        HttpResponseMessage message = await _client.GetAsync($"{_url}?LoadActor={true}");
        _output.WriteLine(await message.Content.ReadAsStringAsync());
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        Page<ReviewUserDto>? http = await message.Content.ReadFromJsonAsync<Page<ReviewUserDto>>();
        http.Should().NotBeNull();
        http.Data.Should().NotBeNull();
        
        http.Data.First().Should().NotBeNull();
        http.Data.First().Actor.Should().NotBeNull();
        http.Data.First().TargetUser.Should().BeNull();
    }
    
    [Fact]
    public async Task ExistsTrue()
    {
        UserResultTest actor = await _helper.CreateAndGetUser();
        UserResultTest target = await _helper.CreateAndGetUser();
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", actor.Tokens!.Token!);
        ReviewUserDto reviewDto = await _helper.CreateReviewToUser(target, actor);
        
        HttpResponseMessage message = await _client.GetAsync($"{_url}/{target.User!.Id}/Exists");
        _output.WriteLine(await message.Content.ReadAsStringAsync());
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<bool>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<bool>>();
        http.Should().NotBeNull();

        http.Data.Should().BeTrue();
    }
    
    [Fact]
    public async Task ExistsFalse()
    {
        UserResultTest actor = await _helper.CreateAndGetUser();
        UserResultTest target = await _helper.CreateAndGetUser();
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", actor.Tokens!.Token!);
        ReviewUserDto reviewDto = await _helper.CreateReviewToUser(target, actor);
        
        HttpResponseMessage message = await _client.GetAsync($"{_url}/{Guid.NewGuid()}/Exists");
        _output.WriteLine(await message.Content.ReadAsStringAsync());
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<bool>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<bool>>();
        http.Should().NotBeNull();

        http.Data.Should().BeFalse();
    }
    
    
}
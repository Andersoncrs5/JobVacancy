using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using JobVacancy.API.IntegrationTests.Utils;
using JobVacancy.API.models.dtos.Category;
using JobVacancy.API.models.dtos.PostUser;
using JobVacancy.API.models.dtos.PostUserMedia;
using JobVacancy.API.Utils.Res;
using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;

namespace JobVacancy.API.IntegrationTests.PostUserMediaTest;

public class PostUserMediaControllerTest: IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly Helper _helper;
    private readonly IConfiguration _configuration;
    private readonly string _url = "/api/v1/PostUserMedia";
    private readonly ITestOutputHelper _output;

    public PostUserMediaControllerTest(CustomWebApplicationFactory factory, ITestOutputHelper output) 
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
            new AuthenticationHeaderValue("Bearer", token);
        CategoryDto categoryDto = await _helper.CreateCategory(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        token = user.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        PostUserDto postUser = await _helper.CreatePostUser(categoryDto, user.User!.Id);

        using var form = new MultipartFormDataContent();
        
        form.Add(new StringContent(postUser.Id), "PostId");
        var fakeFileBytes = new byte[] { 1, 2, 3, 4, 5 };
        var fileContent = new ByteArrayContent(fakeFileBytes);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/png");

        form.Add(fileContent, "File", "image.png");
        
        var response = await _client.PostAsync(_url, form);
        _output.WriteLine(await response.Content.ReadAsStringAsync());
        
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        ResponseHttp<PostUserMediaDto>? http = await response.Content.ReadFromJsonAsync<ResponseHttp<PostUserMediaDto>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int) HttpStatusCode.Created);
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Status.Should().BeTrue();

        http.Data.Should().NotBeNull();
        http.Data.PostId.Should().Be(postUser.Id);
        http.Data.Id.Should().NotBeNullOrWhiteSpace();
        http.Data.BucketName.Should().Be(_configuration["Buckets:ImageBucketName"]);
        http.Data.ObjectName.Should().NotBeNullOrWhiteSpace();
    }
    
    [Fact]
    public async Task Create_ShouldReturnForbidden_WhenAttemptedByNonOwnerUser()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        CategoryDto categoryDto = await _helper.CreateCategory(master);
    
        UserResultTest userA = await _helper.CreateAndGetUser();
    
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", userA.Tokens!.Token!);
        PostUserDto postUser = await _helper.CreatePostUser(categoryDto, userA.User!.Id);

        UserResultTest userB = await _helper.CreateAndGetUser();
    
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", userB.Tokens!.Token!);

        using var form = new MultipartFormDataContent();
        form.Add(new StringContent(postUser.Id), "PostId");
    
        var fakeFileBytes = new byte[] { 1, 2, 3 };
        var fileContent = new ByteArrayContent(fakeFileBytes);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/png");
        form.Add(fileContent, "File", "image.png");

        var response = await _client.PostAsync(_url, form);
    
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    
        _output.WriteLine(await response.Content.ReadAsStringAsync());
    }
    
}
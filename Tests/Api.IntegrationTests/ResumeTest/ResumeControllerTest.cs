using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using CommunityToolkit.HighPerformance.Helpers;
using FluentAssertions;
using JobVacancy.API.IntegrationTests.Utils;
using JobVacancy.API.models.dtos.Resume;
using JobVacancy.API.Utils.Res;
using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;

namespace JobVacancy.API.IntegrationTests.ResumeTest;

public class ResumeControllerTest: IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly Helper _helper;
    private readonly IConfiguration _configuration;
    private readonly string _url = "/api/v1/Resume";
    private readonly ITestOutputHelper _output;

    public ResumeControllerTest(CustomWebApplicationFactory factory, ITestOutputHelper output) 
    {
        _client = factory.CreateClient(); 
        _helper = new Helper(_client); 
        _configuration = factory.Configuration;
        _output = output;
    }
    
    [Fact]
    public async Task Post_Resume_WithMissingFile_ShouldReturn400BadRequest()
    {
        UserResultTest user = await _helper.CreateAndGetUser();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Tokens!.Token!);

        using var content = new MultipartFormDataContent();
        content.Add(new StringContent("Currículo sem arquivo"), name: "Name"); 

        var response = await _client.PostAsync(_url, content);
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Create()
    {
        UserResultTest user = await _helper.CreateAndGetUser();
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", user.Tokens!.Token!);

        using var form = new MultipartFormDataContent();

        form.Add(new StringContent("Meu Currículo Teste"), "Name");

        form.Add(new StringContent("1"), "Version");

        var fakeFileBytes = new byte[] { 1, 2, 3, 4, 5 };
        var fileContent = new ByteArrayContent(fakeFileBytes);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

        form.Add(fileContent, "File", "resume.pdf");

        var response = await _client.PostAsync(_url, form);
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        ResponseHttp<ResumeDto>? body = await response.Content.ReadFromJsonAsync<ResponseHttp<ResumeDto>>();
        
        body.Should().NotBeNull();
        body.Status.Should().BeTrue();
        body.Message.Should().NotBeNullOrWhiteSpace();
        body.Code.Should().Be((int)HttpStatusCode.Created);
        
        body.Data.Should().NotBeNull();
        body.Data.Name.Should().Be("Meu Currículo Teste");
        body.Data.Url.Should().BeNullOrEmpty();
        body.Data.BucketName.Should().Be("resumes");
        body.Data.ObjectKey.Should().NotBeNullOrWhiteSpace();
        
        body.Data.userId.Should().Be(user.User!.Id);
    }

    [Fact]
    public async Task Get()
    {
        UserResultTest user = await _helper.CreateAndGetUser();
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", user.Tokens!.Token!);

        ResumeDto resume = await _helper.CreateResume();

        HttpResponseMessage message = await _client.GetAsync(_url+"/"+resume.Id);
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<ResumeDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<ResumeDto>>();
        http.Should().NotBeNull();
        http.Status.Should().BeTrue();
        http.Message.Should().NotBeNull();
        http.Code.Should().Be((int)HttpStatusCode.OK);
        http.Data.Should().NotBeNull();
        
        http.Data.Id.Should().Be(resume.Id);
    }
    
    [Fact]
    public async Task GetNotFound()
    {
        UserResultTest user = await _helper.CreateAndGetUser();
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", user.Tokens!.Token!);

        HttpResponseMessage message = await _client.GetAsync(_url+"/"+Guid.NewGuid());
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Status.Should().BeFalse();
        http.Message.Should().NotBeNull();
        http.Code.Should().Be((int)HttpStatusCode.NotFound);
        http.Data.Should().BeNull();
    }
    
    [Fact]
    public async Task Delete() 
    {
        UserResultTest user = await _helper.CreateAndGetUser();
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", user.Tokens!.Token!);

        ResumeDto resume = await _helper.CreateResume();

        HttpResponseMessage message = await _client.DeleteAsync(_url+"/"+resume.Id);
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Status.Should().BeTrue();
        http.Message.Should().NotBeNull();
        http.Code.Should().Be((int)HttpStatusCode.OK);
        
        http.Data.Should().BeNull();
    }
    
    [Fact]
    public async Task DeleteNotFound() 
    {
        UserResultTest user = await _helper.CreateAndGetUser();
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", user.Tokens!.Token!);

        HttpResponseMessage message = await _client.DeleteAsync(_url+"/"+Guid.NewGuid());
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Status.Should().BeFalse();
        http.Message.Should().NotBeNull();
        http.Code.Should().Be((int)HttpStatusCode.NotFound);
        http.Data.Should().BeNull();
    }

    [Fact]
    public async Task GetAll()
    {
        UserResultTest user = await _helper.CreateAndGetUser();
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", user.Tokens!.Token!);

        HttpResponseMessage message = await _client.GetAsync(_url);
        message.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
}
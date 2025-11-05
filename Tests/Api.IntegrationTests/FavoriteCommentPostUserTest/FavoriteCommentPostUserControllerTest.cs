using System.Net;
using System.Net.Http.Json;
using JobVacancy.API.IntegrationTests.Utils;
using JobVacancy.API.models.dtos.Category;
using JobVacancy.API.models.dtos.CommentPostUser;
using JobVacancy.API.models.dtos.PostUser;
using JobVacancy.API.Utils.Res;
using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;
using FluentAssertions;

namespace JobVacancy.API.IntegrationTests.FavoriteCommentPostUserTest;

public class FavoriteCommentPostUserControllerTest: IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly Helper _helper;
    private readonly IConfiguration _configuration;
    private readonly string _url = "/api/v1/FavoriteCommentPostUser";
    private readonly ITestOutputHelper _output;

    public FavoriteCommentPostUserControllerTest(CustomWebApplicationFactory factory, ITestOutputHelper output) 
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
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        CategoryDto categoryDto = await _helper.CreateCategory(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        token = user.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        PostUserDto postUser = await _helper.CreatePostUser(categoryDto, user.User!.Id);

        CommentPostUserDto comment = await _helper.CreateComment(postUser);
        
        
        HttpResponseMessage message = await _client.PostAsync($"{_url}/{comment.Id}/Toggle", null);
        message.StatusCode.Should().Be(HttpStatusCode.Created);

        ResponseHttp<object>? content = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        
        content.Should().NotBeNull();
        content.Data.Should().BeNull();
        content.Code.Should().Be((int)HttpStatusCode.Created);
        content.Status.Should().BeTrue();
    }
    
}
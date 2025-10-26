using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using JobVacancy.API.IntegrationTests.Utils;
using JobVacancy.API.models.dtos.Users;
using JobVacancy.API.Utils.Page;
using JobVacancy.API.Utils.Res;

namespace JobVacancy.API.IntegrationTests.UserTests;

public class UserControllerTest: IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly Helper _helper;

    public UserControllerTest(CustomWebApplicationFactory factory) 
    {
        _client = factory.CreateClient(); 
        _helper = new Helper(_client); 
    }
    
    [Fact]
    public async Task GetSingleUserById()
    {
        UserResultTest result = await _helper.CreateAndGetUser();
        result.Should().NotBeNull();
        result.User.Should().NotBeNull();
        result.Tokens.Should().NotBeNull();
        result.Tokens.Token.Should().NotBeNull();
        
        string token = result.Tokens.Token;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        HttpResponseMessage getById = await _client.GetAsync($"/api/v1/User?id={result.User.Id}");
        getById.StatusCode.Should().Be(HttpStatusCode.OK);

        Page<UserDto>? page = await getById.Content.ReadFromJsonAsync<Page<UserDto>>();
        page.Should().NotBeNull();
        page.Data.Should().NotBeNull();
        page.Data.First().Should().BeEquivalentTo(result.User);
    }
    
    [Fact]
    public async Task GetAllUsers()
    {
        UserResultTest result = await _helper.CreateAndGetUser();
        result.Should().NotBeNull();
        result.User.Should().NotBeNull();
        result.Tokens.Should().NotBeNull();
        result.Tokens.Token.Should().NotBeNull();
        
        string token = result.Tokens.Token;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        HttpResponseMessage getByEmail = await _client.GetAsync($"/api/v1/User?email={result.User.Email}");
        getByEmail.StatusCode.Should().Be(HttpStatusCode.OK);
        
        HttpResponseMessage getByUsername = await _client.GetAsync($"/api/v1/User?username={result.User.Username}");
        getByUsername.StatusCode.Should().Be(HttpStatusCode.OK);

        HttpResponseMessage getByFullname = await _client.GetAsync($"/api/v1/User?fullName={result.User.FullName}");
        getByFullname.StatusCode.Should().Be(HttpStatusCode.OK);

    }
    
    [Fact]
    public async Task UpdateUser()
    {
        var num = Random.Shared.Next(1, 100000000);

        UpdateUserDto dto = new UpdateUserDto
        {
            Name = $"pochita{num}",
            FullName = "pochita chainsaw demon",
            Password = "4873586275&&$vfsHi"
        };

        UserResultTest result = await _helper.CreateAndGetUser();
        result.Should().NotBeNull();
        result.Tokens.Should().NotBeNull();
        result.Tokens.Token.Should().NotBeNull();
        
        string token = result.Tokens.Token;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        HttpResponseMessage httpResponseMessage = await _client.PutAsJsonAsync("/api/v1/User", dto);
        httpResponseMessage.EnsureSuccessStatusCode();
        
    }
    
}
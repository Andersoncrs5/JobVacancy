using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using JobVacancy.API.IntegrationTests.Utils;
using JobVacancy.API.models.dtos.Users;
using JobVacancy.API.Utils.Res;

namespace JobVacancy.API.IntegrationTests;

public class UnitTest1: IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly Helper _helper;

    public UnitTest1(CustomWebApplicationFactory factory) 
    {
        _client = factory.CreateClient(); 
        _helper = new Helper(_client); 
    }

    /*
    [Fact]
    public async Task CreateRefreshToken()
    {
        UserResultTest result = await _helper.CreateAndGetUser();
        result.Should().NotBeNull();
        result.Tokens.Should().NotBeNull();
        result.Tokens.RefreshToken.Should().NotBeNullOrEmpty();
        string refreshToken =  result.Tokens.RefreshToken;
        
        HttpResponseMessage? response = await _client.GetAsync($"/api/v1/Auth/RefreshToken/{refreshToken}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        ResponseHttp<ResponseTokens>? content = await response.Content.ReadFromJsonAsync<ResponseHttp<ResponseTokens>>();
        
        content.Should().NotBeNull();
        content.Data.Should().NotBeNull();
        content.Data.Token.Should().NotBeNullOrWhiteSpace();
        content.Data.RefreshToken.Should().NotBeNullOrWhiteSpace();
        content.Message.Should().NotBeNullOrWhiteSpace();
        content.Message.Should().Be("Tokens created succeeded!");
    }
    */
    
    [Fact]
    public async Task Login()
    {
        UserResultTest result = await _helper.CreateAndGetUser();
        result.Should().NotBeNull();
        result.CreateUser.Should().NotBeNull();

        LoginDto dto = new LoginDto
        {
            Email = result.CreateUser.Email,
            Password = result.CreateUser.PasswordHash
        };
        
        HttpResponseMessage getResponse = await _client.PostAsJsonAsync($"/api/v1/Auth/Login", dto);
        
        getResponse.Should().NotBeNull();
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<ResponseTokens>? content = await getResponse.Content.ReadFromJsonAsync<ResponseHttp<ResponseTokens>>();
        content.Should().NotBeNull();
        
        content.Should().NotBeNull();
        content.Data.Should().NotBeNull();
        content.Data.Token.Should().NotBeNullOrWhiteSpace();
        content.Data.RefreshToken.Should().NotBeNullOrWhiteSpace();
        content.Message.Should().NotBeNullOrWhiteSpace();
        content.Message.Should().Be("Login succeeded");
    }
    
    [Fact]
    public async Task LoginReturnLoginInvalidByReasonPassword()
    {
        UserResultTest result = await _helper.CreateAndGetUser();
        result.Should().NotBeNull();
        result.CreateUser.Should().NotBeNull();

        LoginDto dto = new LoginDto
        {
            Email = result.CreateUser.Email,
            Password = "14566367367357"
        };
        
        HttpResponseMessage getResponse = await _client.PostAsJsonAsync($"/api/v1/Auth/Login", dto);
        
        getResponse.Should().NotBeNull();
        getResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        ResponseHttp<object>? content = await getResponse.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        content.Should().NotBeNull();
        
        content.Should().NotBeNull();
        content.Data.Should().BeNull();
        content.Message.Should().NotBeNullOrWhiteSpace();
        content.Message.Should().Be("Login failed.");
    }
    
    [Fact]
    public async Task LoginReturnLoginInvalidByReasonEmail()
    {
        UserResultTest result = await _helper.CreateAndGetUser();
        result.Should().NotBeNull();
        result.CreateUser.Should().NotBeNull();

        LoginDto dto = new LoginDto
        {
            Email = "user@gmail.com",
            Password = result.CreateUser.PasswordHash
        };
        
        HttpResponseMessage getResponse = await _client.PostAsJsonAsync($"/api/v1/Auth/Login", dto);
        
        getResponse.Should().NotBeNull();
        getResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        ResponseHttp<object>? content = await getResponse.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        content.Should().NotBeNull();
        
        content.Should().NotBeNull();
        content.Data.Should().BeNull();
        content.Message.Should().NotBeNullOrWhiteSpace();
        content.Message.Should().Be("Login failed.");
    }
    
    [Fact]
    public async Task DeleteUserReturnUnauthorized()
    {
        HttpResponseMessage getResponse = await _client.DeleteAsync($"/api/v1/Auth");
        
        getResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task DeleteUser()
    {
        UserResultTest result = await _helper.CreateAndGetUser();
        result.Should().NotBeNull();
        result.Tokens.Should().NotBeNull();
        result.Tokens.Token.Should().NotBeNull();
        
        string token = result.Tokens.Token;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        HttpResponseMessage getResponse = await _client.DeleteAsync($"/api/v1/Auth");
        
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        ResponseHttp<object>? response = await getResponse.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        response.Should().NotBeNull();
        
        response.Data.Should().BeNull();
        response.Code.Should().Be(200);
        response.Message.Should().Be("User successfully deleted");
    }
    
    [Fact]
    public async Task GetUserReturnUnauthorized()
    {
        HttpResponseMessage getResponse = await _client.GetAsync($"/api/v1/Auth");
        
        getResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task GetUser()
    {
        int num = Random.Shared.Next(1, 100000000);
        CreateUserDto dto = new CreateUserDto();
        
        dto.Email = $"email{num}@gmail.com";
        dto.Username = $"username{num}";
        dto.PasswordHash = "45356675@3Afecv13$";
        dto.FullName = $"fullname{num}";
        
        HttpResponseMessage response = await _client.PostAsJsonAsync("/api/v1/Auth", dto);
        
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        ResponseHttp<ResponseTokens>? content = await response.Content.ReadFromJsonAsync<ResponseHttp<ResponseTokens>>();

        content.Should().NotBeNull();
        content.Data.Should().NotBeNull();
        content.Data.Token.Should().NotBeNullOrWhiteSpace();
        
        string token = content.Data.Token;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        HttpResponseMessage getResponse = await _client.GetAsync($"/api/v1/Auth");
        
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        ResponseHttp<UserDto>? user = await getResponse.Content.ReadFromJsonAsync<ResponseHttp<UserDto>>();
        user.Should().NotBeNull();
        user.Data.Should().NotBeNull();
        user.Data.Id.Should().NotBeNullOrWhiteSpace();
        user.Data.Email.Should().Be(dto.Email);
        user.Data.Username.Should().Be(dto.Username);   
        user.Data.FullName.Should().Be(dto.FullName);
    }
    
    [Fact]
    public async Task CreateUser()
    {
        int num = Random.Shared.Next(1, 100000000);
        CreateUserDto dto = new CreateUserDto();
        
        dto.Email = $"email{num}@gmail.com";
        dto.Username = $"username{num}";
        dto.PasswordHash = "45356675@3Afecv13$";
        
        HttpResponseMessage response = await _client.PostAsJsonAsync("/api/v1/Auth", dto);
        
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        ResponseHttp<ResponseTokens>? content = await response.Content.ReadFromJsonAsync<ResponseHttp<ResponseTokens>>();

        content.Should().NotBeNull();
        content.Data.Should().NotBeNull();
        content.Data.Token.Should().NotBeNullOrWhiteSpace();
        content.Data.RefreshToken.Should().NotBeNullOrWhiteSpace();
        content.Message.Should().NotBeNullOrWhiteSpace();
        content.Message.Should().Be("Welcome");
    }
    
    [Fact]
    public async Task CreateUserConflictUsername()
    {
        int num = Random.Shared.Next(1, 100000000);
        CreateUserDto dto = new CreateUserDto();
        
        dto.Email = $"email{num}@gmail.com";
        dto.Username = $"username{num}";
        dto.PasswordHash = "45356675@3Afecv13$";
        
        HttpResponseMessage response = await _client.PostAsJsonAsync("/api/v1/Auth", dto);
        
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        
        HttpResponseMessage response_2 = await _client.PostAsJsonAsync("/api/v1/Auth", dto);
        
        response_2.StatusCode.Should().Be(HttpStatusCode.Conflict);
        
        ResponseHttp<object>? content = await response_2.Content.ReadFromJsonAsync<ResponseHttp<object>>();

        content.Should().NotBeNull();
        content.Data.Should().BeNull();
        content.Message.Should().NotBeNullOrWhiteSpace();
        content.Message.Should().Be("Username already exists.");
    }
    
    [Fact]
    public async Task CreateUserConflictEmail()
    {
        int num = Random.Shared.Next(1, 100000000);
        CreateUserDto dto = new CreateUserDto();
        
        dto.Email = $"email{num}@gmail.com";
        dto.Username = $"username{num}";
        dto.PasswordHash = "45356675@3Afecv13$";
        
        HttpResponseMessage response = await _client.PostAsJsonAsync("/api/v1/Auth", dto);
        
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        
        dto.Username = $"username{num+1}";
        
        HttpResponseMessage response_2 = await _client.PostAsJsonAsync("/api/v1/Auth", dto);
        
        response_2.StatusCode.Should().Be(HttpStatusCode.Conflict);
        
        ResponseHttp<object>? content = await response_2.Content.ReadFromJsonAsync<ResponseHttp<object>>();

        content.Should().NotBeNull();
        content.Data.Should().BeNull();
        content.Message.Should().NotBeNullOrWhiteSpace();
        content.Message.Should().Be("Email already exists.");
    }
    
}

using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using JobVacancy.API.IntegrationTests.Utils;
using JobVacancy.API.models.dtos.Users;
using JobVacancy.API.Utils.Res;
using Microsoft.Extensions.Configuration;

namespace JobVacancy.API.IntegrationTests;

public class Helper(
    HttpClient client
    )
{
    public async Task<ResponseTokens> LoginMaster(IConfiguration configuration)
    {
        var datasSystemSection = configuration.GetSection("DataSystem");
        string systemUserEmail = datasSystemSection["systemUserEmail"] ?? throw new InvalidOperationException("System user email configuration is missing.");
        string systemUserPassword = datasSystemSection["SystemUserPassword"] ?? throw new InvalidOperationException("System user password configuration is missing.");
        
        var http = await _Login(systemUserEmail, systemUserPassword);
        return http.Data!;
    }
    public async Task<ResponseTokens> LoginUser(string email, string password)
    {
        var http = await _Login(email, password);
        http.Data.Should().NotBeNull();
        return http.Data;
    }

    private async Task<ResponseHttp<ResponseTokens>> _Login(string email, string password)
    {
        LoginDto dto = new LoginDto
        {
            Email = email,
            Password = password
        };
        
        HttpResponseMessage response = await client.PostAsJsonAsync($"/api/v1/Auth/Login", dto);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Should().NotBeNull();
        
        ResponseHttp<ResponseTokens>? content = await response.Content.ReadFromJsonAsync<ResponseHttp<ResponseTokens>>();
        
        content.Should().NotBeNull();
        content.Data.Should().NotBeNull();
        content.Data.Token.Should().NotBeNull();
        content.Data.RefreshToken.Should().NotBeNull();
        
        return content;
    }
    
    public async Task<UserResultTest> CreateAndGetUser()
    {
        int num = Random.Shared.Next(1, 100000000);
        CreateUserDto dto = new CreateUserDto();
        
        dto.Email = $"email{num}@gmail.com";
        dto.Username = $"username{num}";
        dto.PasswordHash = "45356675@3Afecv13$";
        dto.FullName = $"fullname{num}";
        
        HttpResponseMessage response = await client.PostAsJsonAsync("/api/v1/Auth", dto);
        
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        ResponseHttp<ResponseTokens>? content = await response.Content.ReadFromJsonAsync<ResponseHttp<ResponseTokens>>();

        content.Should().NotBeNull();
        content.Data.Should().NotBeNull();
        content.Data.Token.Should().NotBeNullOrWhiteSpace();
        
        string token = content.Data.Token;
        client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        HttpResponseMessage getResponse = await client.GetAsync($"/api/v1/Auth");
        
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        ResponseHttp<UserDto>? user = await getResponse.Content.ReadFromJsonAsync<ResponseHttp<UserDto>>();
        user.Should().NotBeNull();
        user.Data.Should().NotBeNull();
        user.Data.Id.Should().NotBeNullOrWhiteSpace();
        user.Data.Email.Should().Be(dto.Email);
        user.Data.Username.Should().Be(dto.Username);   
        user.Data.FullName.Should().Be(dto.FullName);

        return new UserResultTest
        {
            CreateUser = dto,
            Tokens = content.Data,
            User = user.Data
        };
    }
}
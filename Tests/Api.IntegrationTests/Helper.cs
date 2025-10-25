using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using JobVacancy.API.IntegrationTests.Utils;
using JobVacancy.API.models.dtos.Users;
using JobVacancy.API.Utils.Res;

namespace JobVacancy.API.IntegrationTests;

public class Helper(
    HttpClient client
    )
{
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
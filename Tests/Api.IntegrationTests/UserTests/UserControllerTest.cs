using System.Net.Http.Json;
using FluentAssertions;
using JobVacancy.API.IntegrationTests.Utils;
using JobVacancy.API.models.dtos.Users;

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
    public async Task UpdateUser()
    {
        UpdateUserDto dto = new UpdateUserDto
        {
            Name = "userupdated12365",
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
using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using JobVacancy.API.IntegrationTests.Utils;
using JobVacancy.API.models.dtos.Category;
using JobVacancy.API.models.dtos.Enterprise;
using JobVacancy.API.models.dtos.Industry;
using JobVacancy.API.models.dtos.Users;
using JobVacancy.API.models.entities.Enums;
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

    public async Task<CategoryDto> CreateCategory(ResponseTokens master)
    {
        int num = Random.Shared.Next(1, 10000000);
        
        string token = master.Token!;
        client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        CreateCategoryDto dto = new CreateCategoryDto
        {
            IsActive = true,
            Name = $"Test{num}",
            Description = $"Test description {num}",
        };

        HttpResponseMessage message = await client.PostAsJsonAsync("api/v1/Category", dto);
        message.StatusCode.Should().Be(HttpStatusCode.Created);

        ResponseHttp<CategoryDto>? response = await message.Content.ReadFromJsonAsync<ResponseHttp<CategoryDto>>();
        
        Assert.NotNull(response);
        Assert.NotNull(response.Data);
        Assert.NotNull(response.Data.Id);
        
        return response.Data;
    }

    public async Task<EnterpriseDto> CreateEnterprise(UserResultTest user, IndustryDto industryDto)
    {
        string token = user.Tokens!.Token!;
        client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        CreateEnterpriseDto dto = new CreateEnterpriseDto
        {
            Description = "New description",
            Name = "New name" + Guid.NewGuid(),
            Type = EnterpriseTypeEnum.MediumBusiness,
        };

        HttpResponseMessage message = await client.PostAsJsonAsync("/api/v1/Enterprise", dto);
        message.StatusCode.Should().Be(HttpStatusCode.Created);
        
        ResponseHttp<EnterpriseDto>? response = await message.Content.ReadFromJsonAsync<ResponseHttp<EnterpriseDto>>();
        response.Should().NotBeNull();
        response.Code.Should().Be(201);
        response.Status.Should().BeTrue();
        
        response.Data.Should().NotBeNull();
        response.Data.Id.Should().NotBeEmpty();
        response.Data.Description.Should().Be(dto.Description);
        response.Data.Name.Should().Be(dto.Name);
        response.Data.Type.Should().Be(dto.Type);
        response.Data.UserId.Should().Be(user.User!.Id);
        
        return response.Data;
    }
    
    public async Task<IndustryDto> CreateIndustry(ResponseTokens master)
    {
        long num = Random.Shared.NextInt64(1, 10000000000000);
        
        string token = master.Token!;
        client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        CreateIndustryDto dto = new CreateIndustryDto
        {
            IsActive = true,
            Name = $"Test{num}",
            Description = $"Test description {num}",
        };

        HttpResponseMessage message = await client.PostAsJsonAsync("/api/v1/Industry", dto);
        message.StatusCode.Should().Be(HttpStatusCode.Created);

        ResponseHttp<IndustryDto>? response = await message.Content.ReadFromJsonAsync<ResponseHttp<IndustryDto>>();
        
        Assert.NotNull(response);
        Assert.NotNull(response.Data);
        Assert.NotNull(response.Data.Id);
        
        response.Data.Name.Should().Be(dto.Name);
        response.Data.Description.Should().Be(dto.Description);
        response.Data.IsActive.Should().Be(dto.IsActive);
        
        return response.Data;
    }
}
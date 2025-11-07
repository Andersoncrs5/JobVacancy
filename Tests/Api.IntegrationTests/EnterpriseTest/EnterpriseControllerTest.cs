using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using JobVacancy.API.IntegrationTests.Utils;
using JobVacancy.API.models.dtos.Enterprise;
using JobVacancy.API.models.dtos.Industry;
using JobVacancy.API.models.entities.Enums;
using JobVacancy.API.Utils.Page;
using JobVacancy.API.Utils.Res;
using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;

namespace JobVacancy.API.IntegrationTests.EnterpriseTest;

public class EnterpriseControllerTest: IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly Helper _helper;
    private readonly IConfiguration _configuration;
    private readonly string _URL = "/api/v1/Enterprise";
    private readonly ITestOutputHelper _output;
    
    public EnterpriseControllerTest(CustomWebApplicationFactory factory, ITestOutputHelper output) 
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
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        string token = user.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        CreateEnterpriseDto dto = new CreateEnterpriseDto
        {
            Description = "New description",
            Name = "New name" + Guid.NewGuid(),
            Type = EnterpriseTypeEnum.MediumBusiness,
        };

        HttpResponseMessage message = await _client.PostAsJsonAsync(_URL, dto);
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
    }

    [Fact]
    public async Task CreateConflictName()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        string token = user.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        CreateEnterpriseDto dto = new CreateEnterpriseDto
        {
            Description = "New description",
            Name = enterprise.Name,
            Type = EnterpriseTypeEnum.MediumBusiness,
        };

        HttpResponseMessage message = await _client.PostAsJsonAsync(_URL, dto);
        message.StatusCode.Should().Be(HttpStatusCode.Conflict);
        
        ResponseHttp<object>? response = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        response.Should().NotBeNull();
        response.Code.Should().Be(409);
        response.Status.Should().BeFalse();
        
        response.Data.Should().BeNull();
    }
    
    [Fact]
    public async Task CreateConflictUserId()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        string token = user.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        CreateEnterpriseDto dto = new CreateEnterpriseDto
        {
            Description = "New description",
            Name = "New name" + Guid.NewGuid(),
            Type = EnterpriseTypeEnum.MediumBusiness,
        };

        HttpResponseMessage message = await _client.PostAsJsonAsync(_URL, dto);
        message.StatusCode.Should().Be(HttpStatusCode.Conflict);
        
        ResponseHttp<object>? response = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        response.Should().NotBeNull();
        response.Code.Should().Be(409);
        response.Status.Should().BeFalse();
        
        response.Data.Should().BeNull();
    }
    
    [Fact]
    public async Task CreateForbUserMaster()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);
        
        string token = master.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        CreateEnterpriseDto dto = new CreateEnterpriseDto
        {
            Description = "New description",
            Name = "New name" + Guid.NewGuid(),
            Type = EnterpriseTypeEnum.MediumBusiness,
        };

        HttpResponseMessage message = await _client.PostAsJsonAsync(_URL, dto);
        message.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Get()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        string token = user.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage message = await _client.GetAsync(_URL+"/"+enterprise.Id);
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        
        ResponseHttp<EnterpriseDto>? response = await message.Content.ReadFromJsonAsync<ResponseHttp<EnterpriseDto>>();
        response.Should().NotBeNull();
        response.Code.Should().Be(200);
        response.Status.Should().BeTrue();
        response.Data.Should().NotBeNull();
        response.Data.Id.Should().Be(enterprise.Id);
    }
    
    [Fact]
    public async Task GetNotFound()
    {
        UserResultTest user = await _helper.CreateAndGetUser();
        
        string token = user.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage message = await _client.GetAsync(_URL+"/"+Guid.NewGuid().ToString());
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);
        
        ResponseHttp<object>? response = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        response.Should().NotBeNull();
        response.Code.Should().Be(404);
        response.Status.Should().BeFalse();
        response.Data.Should().BeNull();
    }
    
    [Fact]
    public async Task Delete()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);

        var loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        string token = loginUserWithNewRole.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage message = await _client.DeleteAsync(_URL+"/"+enterprise.Id);
        message.StatusCode.Should().Be(HttpStatusCode.NoContent);
        
        ResponseHttp<object>? response = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        response.Should().NotBeNull();
        response.Code.Should().Be(204);
        response.Status.Should().BeTrue();
        response.Data.Should().BeNull();
    }
    
    [Fact]
    public async Task DeleteNotFound()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);

        var loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        string token = loginUserWithNewRole.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage message = await _client.DeleteAsync(_URL+"/"+Guid.NewGuid().ToString());
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);
        
        ResponseHttp<object>? response = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        response.Should().NotBeNull();
        response.Code.Should().Be(404);
        response.Status.Should().BeFalse();
        response.Data.Should().BeNull();
    }

    [Fact]
    public async Task GetAll()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        string token = user.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage message = await _client.GetAsync(_URL);
        message.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task GetSingleByIdAndNameAndType()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        string token = user.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage message = await _client.GetAsync($"{_URL}?Name={enterprise.Name}&Id={enterprise.Id}&Type={enterprise.Type}");
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        
        Page<EnterpriseDto>? response = await message.Content.ReadFromJsonAsync<Page<EnterpriseDto>>();
        response.Should().NotBeNull();
        response.Data.Should().NotBeNull();
        response.Data.First().Id.Should().Be(enterprise.Id);
    }
    
    [Fact]
    public async Task GetSingleByUserIdAndNameAndEmail()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        string token = user.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage message = await _client.GetAsync($"{_URL}?UserFilterParams.Email={user.User!.Email}&UserFilterParams.UserName={user.User!.Username}&UserFilterParams.Id={user.User!.Id}");
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        
        Page<EnterpriseDto>? response = await message.Content.ReadFromJsonAsync<Page<EnterpriseDto>>();
        response.Should().NotBeNull();
        response.Data.Should().NotBeNull();
        response.Data.First().Id.Should().Be(enterprise.Id);
    }
    
    [Fact]
    public async Task PatchSuccessUpdateAllFields()
    {
        int num = Random.Shared.Next(1, 100000000);
        
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);
    
        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
    
        var loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        string token = loginUserWithNewRole.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        UpdateEnterpriseDto dto = new UpdateEnterpriseDto
        {
            Name = $"Updated Enterprise {num}",
            Description = "New description for enterprise patch",
            WebSiteUrl = "https://newsite.com",
            LogoUrl = "https://newlogo.com/logo.png",
            Type = EnterpriseTypeEnum.LargeEnterprise
        };

        HttpResponseMessage message = await _client.PatchAsJsonAsync(_URL + "/" + enterprise.Id, dto);

        message.StatusCode.Should().Be(HttpStatusCode.OK);
        ResponseHttp<EnterpriseDto>? response = await message.Content.ReadFromJsonAsync<ResponseHttp<EnterpriseDto>>();
    
        response.Should().NotBeNull();
        response.Data.Should().NotBeNull();

        response.Data.Id.Should().Be(enterprise.Id);
        response.Data.Name.Should().Be(dto.Name);
        response.Data.Description.Should().Be(dto.Description);
        response.Data.WebSiteUrl.Should().Be(dto.WebSiteUrl);
        response.Data.LogoUrl.Should().Be(dto.LogoUrl);
        response.Data.Type.Should().Be(dto.Type);
    
        response.Data.UserId.Should().Be(enterprise.UserId);
        response.Data.CreatedAt.Should().BeCloseTo(enterprise.CreatedAt, TimeSpan.FromSeconds(1));
    }
    
    [Fact]
    public async Task PatchSuccessUpdateJustName()
    {
        int num = Random.Shared.Next(1, 100000000);
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);
    
        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
    
        var loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        string token = loginUserWithNewRole.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        UpdateEnterpriseDto dto = new UpdateEnterpriseDto
        {
            Name = $"New Unique Name {num}",
        };

        HttpResponseMessage message = await _client.PatchAsJsonAsync(_URL + "/" + enterprise.Id, dto);

        message.StatusCode.Should().Be(HttpStatusCode.OK);
        ResponseHttp<EnterpriseDto>? response = await message.Content.ReadFromJsonAsync<ResponseHttp<EnterpriseDto>>();
    
        response.Should().NotBeNull();
        response.Data.Should().NotBeNull();

        response.Data.Name.Should().Be(dto.Name);
    
        response.Data.Id.Should().Be(enterprise.Id);
        response.Data.Description.Should().Be(enterprise.Description);
        response.Data.Type.Should().Be(enterprise.Type);
    }
    
    [Fact]
    public async Task PatchFailureNotFound()
    { 
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);
    
        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        var loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        string token = loginUserWithNewRole.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        UpdateEnterpriseDto dto = new UpdateEnterpriseDto
        {
            Name = "Any name"
        };
        string nonExistentId = Guid.NewGuid().ToString();

        // Act
        HttpResponseMessage message = await _client.PatchAsJsonAsync(_URL + "/" + nonExistentId, dto);

        // Assert
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);
        ResponseHttp<object>? response = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
    
        response.Should().NotBeNull();
        response.Code.Should().Be(404);
        response.Message.Should().Be("Enterprise not found");
    }
    
    [Fact]
    public async Task PatchFailureConflictNameTaken()
    {
       
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);
    
        UserResultTest user = await _helper.CreateAndGetUser();
        
        EnterpriseDto enterpriseA = await _helper.CreateEnterprise(user, industryDto);
    
        UserResultTest userB = await _helper.CreateAndGetUser();
        EnterpriseDto enterpriseB = await _helper.CreateEnterprise(userB, industryDto);
    
        var loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        string token = loginUserWithNewRole.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        UpdateEnterpriseDto dto = new UpdateEnterpriseDto
        {
            Name = enterpriseB.Name 
        };

        HttpResponseMessage message = await _client.PatchAsJsonAsync(_URL + "/" + enterpriseA.Id, dto);

        message.StatusCode.Should().Be(HttpStatusCode.Conflict);
        ResponseHttp<object>? response = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
    
        response.Should().NotBeNull();
        response.Code.Should().Be(409);
        response.Message.Should().Be("Name is taken");
    }
    
}
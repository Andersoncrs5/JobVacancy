using System.Net;
using System.Net.Http.Json;
using System.Text;
using FluentAssertions;
using JobVacancy.API.IntegrationTests.Utils;
using JobVacancy.API.models.dtos.Enterprise;
using JobVacancy.API.models.dtos.EnterpriseIndustry;
using JobVacancy.API.models.dtos.Industry;
using JobVacancy.API.Utils.Page;
using JobVacancy.API.Utils.Res;
using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;

namespace JobVacancy.API.IntegrationTests.EnterpriseIndustryTest;

public class EnterpriseIndustryControllerTest: IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly Helper _helper;
    private readonly IConfiguration _configuration;
    private readonly string _URL = "/api/v1/EnterpriseIndustry";
    private readonly ITestOutputHelper _output;

    public EnterpriseIndustryControllerTest(CustomWebApplicationFactory factory, ITestOutputHelper output) 
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
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);

        var loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        string token = loginUserWithNewRole.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        CreateEnterpriseIndustryDto dto = new CreateEnterpriseIndustryDto
        {
            EnterpriseId = enterprise.Id,
            IndustryId = industryDto.Id,
            IsPrimary = true
        };

        HttpResponseMessage message = await _client.PostAsJsonAsync(_URL, dto);
        message.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task CreateThrowForb()
    {
        
        UserResultTest user = await _helper.CreateAndGetUser();
        
        string token = user.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        CreateEnterpriseIndustryDto dto = new CreateEnterpriseIndustryDto
        {
            EnterpriseId = Guid.NewGuid().ToString(),
            IndustryId = Guid.NewGuid().ToString(),
            IsPrimary = true
        };

        HttpResponseMessage message = await _client.PostAsJsonAsync(_URL, dto);
        message.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task CreateReturnNotFoundIndustry()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);

        var loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        string token = loginUserWithNewRole.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        CreateEnterpriseIndustryDto dto = new CreateEnterpriseIndustryDto
        {
            EnterpriseId = enterprise.Id,
            IndustryId = Guid.NewGuid().ToString(),
            IsPrimary = true
        };

        HttpResponseMessage message = await _client.PostAsJsonAsync(_URL, dto);
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);

        ResponseHttp<object>? jsonAsync = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        jsonAsync.Should().NotBeNull();
        jsonAsync.Message.Should().NotBeNull();
        jsonAsync.Code.Should().Be(404);
        jsonAsync.Data.Should().BeNull();
    }

    [Fact]
    public async Task CreateReturnBadRequestIndustryNotActive()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustryWithIsActive(master, false);

        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);

        var loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        string token = loginUserWithNewRole.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        CreateEnterpriseIndustryDto dto = new CreateEnterpriseIndustryDto
        {
            EnterpriseId = enterprise.Id,
            IndustryId = industryDto.Id,
            IsPrimary = true
        };

        HttpResponseMessage message = await _client.PostAsJsonAsync(_URL, dto);
        message.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        ResponseHttp<object>? response = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        response.Should().NotBeNull();
        response.Message.Should().Be("Industry is disabled");
        response.Data.Should().BeNull();
        response.Code.Should().Be(400);
    }

    [Fact]
    public async Task GetAll()
    {
        UserResultTest user = await _helper.CreateAndGetUser();
        
        string token = user.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage message = await _client.GetAsync(_URL);
        message.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task GetAllSingleByIndustry()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);

        var loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        string token = loginUserWithNewRole.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        await _helper.CreateEnterpriseIndustry(enterprise.Id, industryDto.Id);

        HttpResponseMessage message = await _client.GetAsync($"{_URL}" +
                                                             $"?IndustryFilterParams.Name={industryDto.Name}" +
                                                             $"&IndustryFilterParams.IsActive={industryDto.IsActive}" +
                                                             $"&IndustryFilterParams.Id={industryDto.Id}");
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        Page<EnterpriseIndustryDto>? page = await message.Content.ReadFromJsonAsync<Page<EnterpriseIndustryDto>>();
        page.Should().NotBeNull();
        page.Data.Should().NotBeNull();
        page.Data.First().Should().NotBeNull();
        page.Data.First().IndustryId.Should().Be(industryDto.Id);
        page.Data.First().Industry.Id.Should().Be(industryDto.Id);
        page.Data.First().EnterpriseId.Should().Be(enterprise.Id);
        page.Data.First().Enterprise.Id.Should().Be(enterprise.Id);
    }
    
    [Fact]
    public async Task GetAllSingleByEnterprise()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);

        var loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        string token = loginUserWithNewRole.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        await _helper.CreateEnterpriseIndustry(enterprise.Id, industryDto.Id);

        HttpResponseMessage message = await _client.GetAsync($"{_URL}" +
                                                             $"?EnterpriseFilterParam.Id={enterprise.Id}" +
                                                             $"&EnterpriseFilterParam.Name={enterprise.Name}" +
                                                             $"&EnterpriseFilterParam.Type={enterprise.Type}"
                                                             );
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        Page<EnterpriseIndustryDto>? page = await message.Content.ReadFromJsonAsync<Page<EnterpriseIndustryDto>>();
        page.Should().NotBeNull();
        page.Data.Should().NotBeNull();
        page.Data.First().Should().NotBeNull();
        page.Data.First().IndustryId.Should().Be(industryDto.Id);
        page.Data.First().Industry.Id.Should().Be(industryDto.Id);
        page.Data.First().EnterpriseId.Should().Be(enterprise.Id);
        page.Data.First().Enterprise.Id.Should().Be(enterprise.Id);
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
        
        await _helper.CreateEnterpriseIndustry(enterprise.Id, industryDto.Id);
        
        HttpResponseMessage message = await _client.DeleteAsync("/api/v1/EnterpriseIndustry/" +  industryDto.Id);
        message.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task TogglePrimaryFalse()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);

        var loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        string token = loginUserWithNewRole.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        string saved = await _helper.CreateEnterpriseIndustry(enterprise.Id, industryDto.Id, true);

        HttpResponseMessage message = await _client.PatchAsync($"{_URL}/{saved}/toggle/status/is-primary", new StringContent("{}", Encoding.UTF8, "application/json"));
        
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        ResponseHttp<bool>? content = await message.Content.ReadFromJsonAsync<ResponseHttp<bool>>();
        content.Should().NotBeNull();
        content.Status.Should().BeTrue();
        
        content.Data.Should().BeFalse();
    }
    
    [Fact]
    public async Task TogglePrimaryTrue()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);

        var loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        string token = loginUserWithNewRole.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        string saved = await _helper.CreateEnterpriseIndustry(enterprise.Id, industryDto.Id, false);

        HttpResponseMessage message = await _client.PatchAsync($"{_URL}/{saved}/toggle/status/is-primary", new StringContent("{}", Encoding.UTF8, "application/json"));
        
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        ResponseHttp<bool>? content = await message.Content.ReadFromJsonAsync<ResponseHttp<bool>>();
        content.Should().NotBeNull();
        content.Status.Should().BeTrue();
        
        content.Data.Should().BeTrue();
    }
    
    [Fact]
    public async Task TogglePrimaryReturnForbidden()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);

        var loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        string token = loginUserWithNewRole.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        string saved = await _helper.CreateEnterpriseIndustry(enterprise.Id, industryDto.Id, false);
        
        token = user.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage message = await _client.PatchAsync($"{_URL}/{saved}/toggle/status/is-primary", new StringContent("{}", Encoding.UTF8, "application/json"));
        message.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task TogglePrimaryReturnNotFound()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);

        var loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        string token = loginUserWithNewRole.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        string saved = await _helper.CreateEnterpriseIndustry(enterprise.Id, industryDto.Id, false);

        HttpResponseMessage message = await _client.PatchAsync($"{_URL}/{Guid.NewGuid()}/toggle/status/is-primary", new StringContent("{}", Encoding.UTF8, "application/json"));
        
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);
        ResponseHttp<object>? content = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        
        content.Should().NotBeNull();
        content.Status.Should().BeFalse();
        content.Data.Should().BeNull();
    }
    
    [Fact]
    public async Task ToggleSuccessCreatesLinkReturns201()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master); // Industry existe
    
        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
    
        var loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        string token = loginUserWithNewRole.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage message = await _client.PostAsync($"{_URL}/{industryDto.Id}/toggle", null);

        message.StatusCode.Should().Be(HttpStatusCode.Created);
        ResponseHttp<string>? response = await message.Content.ReadFromJsonAsync<ResponseHttp<string>>();
        response.Should().NotBeNull();
        response.Status.Should().BeTrue();
        response.Data.Should().NotBeNullOrEmpty();
        response.Message.Should().Be("Industry added with successfully");
    }
    
    [Fact]
    public async Task ToggleSuccessDeletesLinkReturns200()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);
    
        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);

        var loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        string token = loginUserWithNewRole.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        await _helper.CreateEnterpriseIndustry(enterprise.Id, industryDto.Id, false); 
    
        HttpResponseMessage message = await _client.PostAsync($"{_URL}/{industryDto.Id}/toggle", null);

        message.StatusCode.Should().Be(HttpStatusCode.OK);
        ResponseHttp<object>? response = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        response.Should().NotBeNull();
        response.Status.Should().BeTrue();
        response.Data.Should().BeNull();
        response.Message.Should().Be("Industry removed with successfully");
    }
    
    [Fact]
    public async Task ToggleFailNoTokenReturns401()
    {
        _client.DefaultRequestHeaders.Authorization = null;
        HttpResponseMessage message = await _client.PostAsync($"{_URL}/{Guid.NewGuid()}/toggle", null);
        message.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
}
using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using JobVacancy.API.IntegrationTests.Utils;
using JobVacancy.API.models.dtos.EmployeeInvitation;
using JobVacancy.API.models.dtos.Enterprise;
using JobVacancy.API.models.dtos.Industry;
using JobVacancy.API.models.entities.Enums;
using JobVacancy.API.Utils.Res;
using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;

namespace JobVacancy.API.IntegrationTests.EmployeeInvitationTest;

public class EmployeeInvitationControllerTest: IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly Helper _helper;
    private readonly IConfiguration _configuration;
    private readonly string _URL = "/api/v1/EmployeeInvitation";
    private readonly ITestOutputHelper _output;
    
    public EmployeeInvitationControllerTest(CustomWebApplicationFactory factory, ITestOutputHelper output) 
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

        UserResultTest userGuest = await _helper.CreateAndGetUser();
        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        var loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        string token = loginUserWithNewRole.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        CreateEmployeeInvitationDto dto = new CreateEmployeeInvitationDto
        {
            UserId = userGuest.User!.Id,
            Currency = CurrencyEnum.Aud,
            EmploymentType = EmploymentTypeEnum.Temporary,
            Message = "AnyMessage",
            Position = "DEVOPS",
            ProposedStartDate = DateTime.UtcNow.AddDays(7),
            ProposedEndDate = DateTime.UtcNow.AddDays(20),
            SalaryRange = "5000-7000",
            
        };

        HttpResponseMessage message = await _client.PostAsJsonAsync($"{_URL}", dto);
        
        message.StatusCode.Should().Be(HttpStatusCode.Created);

        ResponseHttp<EmployeeInvitationDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<EmployeeInvitationDto>>();
        http.Should().NotBeNull();
        http.Data.Should().NotBeNull();
        http.Status.Should().BeTrue();
        http.Code.Should().Be((int)HttpStatusCode.Created);
        http.Message.Should().NotBeNullOrWhiteSpace();
        
        http.Data.Id.Should().NotBeNullOrWhiteSpace();
        http.Data.UserId.Should().Be(dto.UserId);
        http.Data.Currency.Should().Be(dto.Currency);
        http.Data.EmploymentType.Should().Be(dto.EmploymentType);
        http.Data.Message.Should().Be(dto.Message);
        http.Data.Position.Should().Be(dto.Position);
        http.Data.ProposedStartDate.Should().Be(dto.ProposedStartDate);
        http.Data.ProposedEndDate.Should().Be(dto.ProposedEndDate);
        http.Data.SalaryRange.Should().Be(dto.SalaryRange);
        http.Data.EnterpriseId.Should().Be(enterprise.Id);
        http.Data.InviteSenderId.Should().Be(enterprise.UserId);
    }
    
    [Fact]
    public async Task CreateNotFoundUser()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);
        
        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        var loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        string token = loginUserWithNewRole.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        CreateEmployeeInvitationDto dto = new CreateEmployeeInvitationDto
        {
            UserId = Guid.NewGuid().ToString(),
            Currency = CurrencyEnum.Aud,
            EmploymentType = EmploymentTypeEnum.Temporary,
            Message = "AnyMessage",
            Position = "DEVOPS",
            ProposedStartDate = DateTime.UtcNow.AddDays(7),
            ProposedEndDate = DateTime.UtcNow.AddDays(20),
            SalaryRange = "5000-7000",
        };

        HttpResponseMessage message = await _client.PostAsJsonAsync($"{_URL}", dto);
        
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Data.Should().BeNull();
        http.Status.Should().BeFalse();
        http.Code.Should().Be((int)HttpStatusCode.NotFound);
        http.Message.Should().NotBeNullOrWhiteSpace();
    }
    
    [Fact]
    public async Task CreateThrowForb()
    {
        UserResultTest user = await _helper.CreateAndGetUser();
        
        string token = user.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        CreateEmployeeInvitationDto dto = new CreateEmployeeInvitationDto
        {
            UserId = Guid.NewGuid().ToString(),
            Currency = CurrencyEnum.Aud,
            EmploymentType = EmploymentTypeEnum.Temporary,
            Message = "AnyMessage",
            Position = "DEVOPS",
            ProposedStartDate = DateTime.UtcNow.AddDays(7),
            ProposedEndDate = DateTime.UtcNow.AddDays(20),
            SalaryRange = "5000-7000",
        };

        HttpResponseMessage message = await _client.PostAsJsonAsync($"{_URL}", dto);
        
        message.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Get()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        UserResultTest userGuest = await _helper.CreateAndGetUser();
        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        ResponseTokens loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        string token = loginUserWithNewRole.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        EmployeeInvitationDto invitation = await _helper.CreateEmployeeInvitation(userGuest);

        HttpResponseMessage message = await _client.GetAsync($"{_URL}/{invitation.Id}");
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        ResponseHttp<EmployeeInvitationDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<EmployeeInvitationDto>>();
        
        http.Should().NotBeNull();
        http.Data.Should().NotBeNull();
        http.Status.Should().BeTrue();
        http.Code.Should().Be((int)HttpStatusCode.OK);
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Data.Id.Should().Be(invitation.Id);
    }
     
    [Fact]
    public async Task GetNotFound()
    {
        UserResultTest user = await _helper.CreateAndGetUser();
        string token = user.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        HttpResponseMessage message = await _client.GetAsync($"{_URL}/{Guid.NewGuid()}");
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);
        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        
        http.Should().NotBeNull();
        http.Data.Should().BeNull();
        http.Status.Should().BeFalse();
        http.Code.Should().Be((int)HttpStatusCode.NotFound);
        http.Message.Should().NotBeNullOrWhiteSpace();
    }
     
    [Fact]
    public async Task Del()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        UserResultTest userGuest = await _helper.CreateAndGetUser();
        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        ResponseTokens loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        string token = loginUserWithNewRole.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        EmployeeInvitationDto invitation = await _helper.CreateEmployeeInvitation(userGuest);

        HttpResponseMessage message = await _client.DeleteAsync($"{_URL}/{invitation.Id}");
        message.StatusCode.Should().Be(HttpStatusCode.NoContent);
        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        
        http.Should().NotBeNull();
        http.Data.Should().BeNull();
        http.Status.Should().BeTrue();
        http.Code.Should().Be((int)HttpStatusCode.NoContent);
        http.Message.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task DelNotFound()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        UserResultTest userGuest = await _helper.CreateAndGetUser();
        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        ResponseTokens loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        string token = loginUserWithNewRole.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        HttpResponseMessage message = await _client.DeleteAsync($"{_URL}/{Guid.NewGuid()}");
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);
        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        
        http.Should().NotBeNull();
        http.Data.Should().BeNull();
        http.Status.Should().BeFalse();
        http.Code.Should().Be((int)HttpStatusCode.NotFound);
        http.Message.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task DelThrowForb()
    {
        UserResultTest user = await _helper.CreateAndGetUser();
        string token = user.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        HttpResponseMessage message = await _client.DeleteAsync($"{_URL}/{Guid.NewGuid()}");
        message.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task UpdateByEnterprise()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        UserResultTest userGuest = await _helper.CreateAndGetUser();
        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        ResponseTokens loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        string token = loginUserWithNewRole.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        EmployeeInvitationDto invitation = await _helper.CreateEmployeeInvitation(userGuest);

        UpdateEmployeeInvitationDto dto = new UpdateEmployeeInvitationDto
        {
            ProposedStartDate = DateTime.UtcNow.AddDays(1),
            ProposedEndDate = DateTime.UtcNow.AddDays(10),
            Currency = CurrencyEnum.Eur,
            EmploymentType = EmploymentTypeEnum.FullTime,
            Message = "AnyMessageUpdate",
            Position = "SOFTWARE ENGINEER",
            SalaryRange = "2000-7000",
        };
        
        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_URL}/{invitation.Id}/By/Enterprise", dto);
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<EmployeeInvitationDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<EmployeeInvitationDto>>();
        http.Should().NotBeNull();
        http.Data.Should().NotBeNull();
        http.Status.Should().BeTrue();
        http.Code.Should().Be((int)HttpStatusCode.OK);
        http.Message.Should().NotBeNullOrWhiteSpace();
        
        http.Data.Id.Should().Be(invitation.Id);
        http.Data.EnterpriseId.Should().Be(invitation.EnterpriseId);
        http.Data.InviteSenderId.Should().Be(invitation.InviteSenderId);
        http.Data.Status.Should().Be(invitation.Status);
        
        http.Data.ProposedStartDate.Day.Should().Be(dto.ProposedStartDate.Value.Day);
        http.Data.ProposedEndDate.Should().NotBeNull();
        http.Data.ProposedEndDate.Value.Day.Should().Be(dto.ProposedEndDate.Value.Day);
        http.Data.Currency.Should().Be(dto.Currency);
        http.Data.EmploymentType.Should().Be(dto.EmploymentType);
        http.Data.Message.Should().Be(dto.Message);
        http.Data.Position.Should().Be(dto.Position);
        http.Data.SalaryRange.Should().Be(dto.SalaryRange);
    }
    
    [Fact]
    public async Task UpdateByEnterpriseJustProposedStartDate()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        UserResultTest userGuest = await _helper.CreateAndGetUser();
        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        ResponseTokens loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        string token = loginUserWithNewRole.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        EmployeeInvitationDto invitation = await _helper.CreateEmployeeInvitation(userGuest);

        UpdateEmployeeInvitationDto dto = new UpdateEmployeeInvitationDto
        {
            ProposedStartDate = DateTime.UtcNow.AddDays(1),
        };
        
        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_URL}/{invitation.Id}/By/Enterprise", dto);
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<EmployeeInvitationDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<EmployeeInvitationDto>>();
        http.Should().NotBeNull();
        http.Data.Should().NotBeNull();
        http.Status.Should().BeTrue();
        http.Code.Should().Be((int)HttpStatusCode.OK);
        http.Message.Should().NotBeNullOrWhiteSpace();
        
        http.Data.Id.Should().Be(invitation.Id);
        http.Data.EnterpriseId.Should().Be(invitation.EnterpriseId);
        http.Data.InviteSenderId.Should().Be(invitation.InviteSenderId);
        http.Data.Status.Should().Be(invitation.Status);
        
        http.Data.ProposedStartDate.Day.Should().Be(dto.ProposedStartDate.Value.Day);
        http.Data.ProposedEndDate.Should().NotBeNull();
        http.Data.ProposedEndDate.Value.Day.Should().Be(invitation.ProposedEndDate!.Value.Day);
        http.Data.Currency.Should().Be(invitation.Currency);
        http.Data.EmploymentType.Should().Be(invitation.EmploymentType);
        http.Data.Message.Should().Be(invitation.Message);
        http.Data.Position.Should().Be(invitation.Position);
        http.Data.SalaryRange.Should().Be(invitation.SalaryRange);
    }
    
    [Fact]
    public async Task UpdateByEnterpriseJustProposedEndDate()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        UserResultTest userGuest = await _helper.CreateAndGetUser();
        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        ResponseTokens loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        string token = loginUserWithNewRole.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        EmployeeInvitationDto invitation = await _helper.CreateEmployeeInvitation(userGuest);

        UpdateEmployeeInvitationDto dto = new UpdateEmployeeInvitationDto
        {
            ProposedEndDate = DateTime.UtcNow.AddDays(18),
        };
        
        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_URL}/{invitation.Id}/By/Enterprise", dto);
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<EmployeeInvitationDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<EmployeeInvitationDto>>();
        http.Should().NotBeNull();
        http.Data.Should().NotBeNull();
        http.Status.Should().BeTrue();
        http.Code.Should().Be((int)HttpStatusCode.OK);
        http.Message.Should().NotBeNullOrWhiteSpace();
        
        http.Data.Id.Should().Be(invitation.Id);
        http.Data.EnterpriseId.Should().Be(invitation.EnterpriseId);
        http.Data.InviteSenderId.Should().Be(invitation.InviteSenderId);
        http.Data.Status.Should().Be(invitation.Status);
        
        http.Data.ProposedStartDate.Should().Be(invitation.ProposedStartDate);
        http.Data.ProposedEndDate.Should().Be(dto.ProposedEndDate);
        http.Data.Currency.Should().Be(invitation.Currency);
        http.Data.EmploymentType.Should().Be(invitation.EmploymentType);
        http.Data.Message.Should().Be(invitation.Message);
        http.Data.Position.Should().Be(invitation.Position);
        http.Data.SalaryRange.Should().Be(invitation.SalaryRange);
    }
    
    
    
}
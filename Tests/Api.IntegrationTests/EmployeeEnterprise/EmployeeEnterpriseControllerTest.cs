using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using JobVacancy.API.IntegrationTests.Utils;
using JobVacancy.API.models.dtos.EmployeeEnterprise;
using JobVacancy.API.models.dtos.EmployeeInvitation;
using JobVacancy.API.models.dtos.Enterprise;
using JobVacancy.API.models.dtos.Industry;
using JobVacancy.API.models.dtos.Position;
using JobVacancy.API.models.entities.Enums;
using JobVacancy.API.Utils.Page;
using JobVacancy.API.Utils.Res;
using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;

namespace JobVacancy.API.IntegrationTests.EmployeeEnterprise;

public class EmployeeEnterpriseControllerTest: IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly Helper _helper;
    private readonly IConfiguration _configuration;
    private readonly string _url = "/api/v1/EmployeeEnterprise";
    private readonly ITestOutputHelper _output;
    
    public EmployeeEnterpriseControllerTest(CustomWebApplicationFactory factory, ITestOutputHelper output) 
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
        PositionDto positionDto = await _helper.CreatePositionAsync();

        UserResultTest userGuest = await _helper.CreateAndGetUser();
        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        ResponseTokens loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        string token = loginUserWithNewRole.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", token);

        EmployeeInvitationDto invitation = await _helper.CreateEmployeeInvitation(userGuest, positionDto);
        
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", userGuest.Tokens!.Token);

        EmployeeInvitationDto invitationUpdated = await _helper.UpdateInvitationByUser(StatusEnum.Accepted, invitation);

        CreateEmployeeEnterpriseDto dto = new CreateEmployeeEnterpriseDto()
        {
            InviteSenderId = invitationUpdated.InviteSenderId,
            UserId = invitationUpdated.UserId,
            ContractLegalType = ContractLegalTypeEnum.Indefinite,
            ContractLink = null,
            Currency = CurrencyEnum.Cad,
            EmploymentStatus = EmploymentStatusEnum.CurrentEmployee,
            EmploymentType = invitationUpdated.EmploymentType,
            PositionId = invitationUpdated.PositionId,
            EndDate = null,
            Notes = string.Concat(Enumerable.Repeat("OKOK", 20)),
            PaymentFrequency = PaymentFrequencyEnum.Monthly,
            SalaryRange = invitationUpdated.SalaryRange,
            SalaryValue = 8659.75m,
            StartDate = invitationUpdated.ProposedStartDate,
        };
        
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token!);

        HttpResponseMessage message = await _client.PostAsJsonAsync($"{_url}/{invitationUpdated.Id}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.Created);

        ResponseHttp<EmployeeEnterpriseDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<EmployeeEnterpriseDto>>();
        http.Should().NotBeNull();
        http.Version.Should().Be(1);
        http.Code.Should().Be((int)HttpStatusCode.Created);
        http.Status.Should().BeTrue();
        
        http.Data.Should().NotBeNull();
        http.Data.Id.Should().NotBeNullOrWhiteSpace();
        http.Data.InviteSenderId.Should().Be(invitationUpdated.InviteSenderId);
        http.Data.UserId.Should().Be(invitationUpdated.UserId);
        http.Data.ContractLegalType.Should().Be(dto.ContractLegalType);
        http.Data.ContractLink.Should().Be(dto.ContractLink);
        http.Data.Currency.Should().Be(dto.Currency);
        http.Data.EmploymentStatus.Should().Be(dto.EmploymentStatus);
        http.Data.EmploymentType.Should().Be(dto.EmploymentType);
        http.Data.PositionId.Should().Be(dto.PositionId);
        http.Data.EndDate.Should().Be(dto.EndDate);
        http.Data.Notes.Should().Be(dto.Notes);
        http.Data.PaymentFrequency.Should().Be(dto.PaymentFrequency);
        http.Data.SalaryRange.Should().Be(dto.SalaryRange);
        http.Data.SalaryValue.Should().Be(dto.SalaryValue);
        http.Data.StartDate.Should().Be(dto.StartDate);
    }
    
    [Fact]
    public async Task CreateNotAcceptedInvitation()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);
        PositionDto positionDto = await _helper.CreatePositionAsync();

        UserResultTest userGuest = await _helper.CreateAndGetUser();
        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        ResponseTokens loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        string token = loginUserWithNewRole.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", token);

        EmployeeInvitationDto invitation = await _helper.CreateEmployeeInvitation(userGuest, positionDto);
        
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", userGuest.Tokens!.Token);
        
        CreateEmployeeEnterpriseDto dto = new CreateEmployeeEnterpriseDto()
        {
            InviteSenderId = invitation.InviteSenderId,
            UserId = invitation.UserId,
            ContractLegalType = ContractLegalTypeEnum.Indefinite,
            ContractLink = null,
            Currency = CurrencyEnum.Cad,
            EmploymentStatus = EmploymentStatusEnum.CurrentEmployee,
            EmploymentType = invitation.EmploymentType,
            PositionId = invitation.PositionId,
            EndDate = null,
            Notes = string.Concat(Enumerable.Repeat("OKOK", 20)),
            PaymentFrequency = PaymentFrequencyEnum.Monthly,
            SalaryRange = invitation.SalaryRange,
            SalaryValue = 8659.75m,
            StartDate = invitation.ProposedStartDate,
        };
        
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token!);

        HttpResponseMessage message = await _client.PostAsJsonAsync($"{_url}/{invitation.Id}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.Conflict);

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Version.Should().Be(1);
        http.Code.Should().Be((int)HttpStatusCode.Conflict);
        http.Status.Should().BeFalse();
        http.Message.Should().NotBeNullOrWhiteSpace();
        
        http.Data.Should().BeNull();
    }
    
    [Fact]
    public async Task CreateNotFoundInvitation()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);
        PositionDto positionDto = await _helper.CreatePositionAsync();

        UserResultTest userGuest = await _helper.CreateAndGetUser();
        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        ResponseTokens loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        string token = loginUserWithNewRole.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", token);

        EmployeeInvitationDto invitation = await _helper.CreateEmployeeInvitation(userGuest, positionDto);
        
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", userGuest.Tokens!.Token);

        EmployeeInvitationDto invitationUpdated = await _helper.UpdateInvitationByUser(StatusEnum.Accepted, invitation);

        CreateEmployeeEnterpriseDto dto = new CreateEmployeeEnterpriseDto()
        {
            InviteSenderId = invitationUpdated.InviteSenderId,
            UserId = invitationUpdated.UserId,
            ContractLegalType = ContractLegalTypeEnum.Indefinite,
            ContractLink = null,
            Currency = CurrencyEnum.Cad,
            EmploymentStatus = EmploymentStatusEnum.CurrentEmployee,
            EmploymentType = invitationUpdated.EmploymentType,
            PositionId = invitationUpdated.PositionId,
            EndDate = null,
            Notes = string.Concat(Enumerable.Repeat("OKOK", 20)),
            PaymentFrequency = PaymentFrequencyEnum.Monthly,
            SalaryRange = invitationUpdated.SalaryRange,
            SalaryValue = 8659.75m,
            StartDate = invitationUpdated.ProposedStartDate,
        };
        
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token!);

        HttpResponseMessage message = await _client.PostAsJsonAsync($"{_url}/{Guid.NewGuid()}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Data.Should().BeNull();
        http.Code.Should().Be((int) HttpStatusCode.NotFound);
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Status.Should().BeFalse();
    }

    [Fact]
    public async Task Get()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);
        PositionDto positionDto = await _helper.CreatePositionAsync();

        UserResultTest userGuest = await _helper.CreateAndGetUser();
        UserResultTest user = await _helper.CreateAndGetUser();
        await _helper.CreateEnterprise(user, industryDto);
        
        ResponseTokens loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token!);

        EmployeeInvitationDto invitation = await _helper.CreateEmployeeInvitation(userGuest, positionDto);
        
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", userGuest.Tokens!.Token);

        EmployeeInvitationDto invitationUpdated = await _helper.UpdateInvitationByUser(StatusEnum.Accepted, invitation);

        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token);
        
        EmployeeEnterpriseDto dto = await _helper.CreateEmployeeEnterprise(invitationUpdated);

        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", userGuest.Tokens!.Token);
        
        HttpResponseMessage message = await _client.GetAsync($"{_url}/{dto.Id}");
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        
        ResponseHttp<EmployeeEnterpriseDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<EmployeeEnterpriseDto>>();
        http.Should().NotBeNull();
        http.Data.Should().NotBeNull();
        http.Code.Should().Be((int) HttpStatusCode.OK);
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Status.Should().BeTrue();
        
        http.Data.Id.Should().Be(dto.Id);
        
    }
    
    [Fact]
    public async Task GetNotFound()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", master.Token);
        
        HttpResponseMessage message = await _client.GetAsync($"{_url}/{Guid.NewGuid()}");
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);
        
        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Data.Should().BeNull();
        http.Code.Should().Be((int) HttpStatusCode.NotFound);
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Status.Should().BeFalse();
        
    }
    
    [Fact]
    public async Task Del()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);
        PositionDto positionDto = await _helper.CreatePositionAsync();

        UserResultTest userGuest = await _helper.CreateAndGetUser();
        UserResultTest user = await _helper.CreateAndGetUser();
        await _helper.CreateEnterprise(user, industryDto);
        
        ResponseTokens loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token!);

        EmployeeInvitationDto invitation = await _helper.CreateEmployeeInvitation(userGuest, positionDto);
        
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", userGuest.Tokens!.Token);

        EmployeeInvitationDto invitationUpdated = await _helper.UpdateInvitationByUser(StatusEnum.Accepted, invitation);

        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token);
        
        EmployeeEnterpriseDto dto = await _helper.CreateEmployeeEnterprise(invitationUpdated);
        
        HttpResponseMessage message = await _client.DeleteAsync($"{_url}/{dto.Id}");
        message.StatusCode.Should().Be(HttpStatusCode.NoContent);
        
        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int) HttpStatusCode.NoContent);
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Status.Should().BeTrue();
        
        http.Data.Should().BeNull();
    }
    
    [Fact]
    public async Task DelNotFound()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);
        PositionDto positionDto = await _helper.CreatePositionAsync();

        UserResultTest userGuest = await _helper.CreateAndGetUser();
        UserResultTest user = await _helper.CreateAndGetUser();
        await _helper.CreateEnterprise(user, industryDto);
        
        ResponseTokens loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token!);

        EmployeeInvitationDto invitation = await _helper.CreateEmployeeInvitation(userGuest, positionDto);
        
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", userGuest.Tokens!.Token);

        EmployeeInvitationDto invitationUpdated = await _helper.UpdateInvitationByUser(StatusEnum.Accepted, invitation);

        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token);
        
        HttpResponseMessage message = await _client.DeleteAsync($"{_url}/{Guid.NewGuid()}");
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);
        
        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int) HttpStatusCode.NotFound);
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Status.Should().BeFalse();
        
        http.Data.Should().BeNull();
    }
    
    [Fact]
    public async Task DelThrowForb()
    {
        UserResultTest userGuest = await _helper.CreateAndGetUser();
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", userGuest.Tokens!.Token);
        
        HttpResponseMessage message = await _client.DeleteAsync($"{_url}/{Guid.NewGuid()}");
        message.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task PatchAllFields()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);
        PositionDto positionDto = await _helper.CreatePositionAsync();
        PositionDto positionDtoB = await _helper.CreatePositionAsync();

        UserResultTest userGuest = await _helper.CreateAndGetUser();
        UserResultTest user = await _helper.CreateAndGetUser();
        await _helper.CreateEnterprise(user, industryDto);
        
        ResponseTokens loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token!);

        EmployeeInvitationDto invitation = await _helper.CreateEmployeeInvitation(userGuest, positionDto);
        
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", userGuest.Tokens!.Token);

        EmployeeInvitationDto invitationUpdated = await _helper.UpdateInvitationByUser(StatusEnum.Accepted, invitation);

        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token);
        
        EmployeeEnterpriseDto emp = await _helper.CreateEmployeeEnterprise(invitationUpdated);

        UpdateEmployeeEnterpriseDto dto = new UpdateEmployeeEnterpriseDto()
        {
            ContractLink = "https://github.com/Andersoncrs5/",
            SalaryRange = "7890-9000",
            TerminationReason = null,
            Notes = null,
            SalaryValue = 6756.34m,
            PaymentFrequency = PaymentFrequencyEnum.Weekly,
            ContractLegalType = ContractLegalTypeEnum.Internship,
            EmploymentType = EmploymentTypeEnum.Contract,
            EmploymentStatus = EmploymentStatusEnum.Probation,
            Currency = CurrencyEnum.Zar,
            StartDate = DateTime.UtcNow.AddMonths(1),
            EndDate = DateTime.UtcNow.AddYears(1),
            PositionId = positionDtoB.Id
        };
        
        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_url}/{emp.Id}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        
        ResponseHttp<EmployeeEnterpriseDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<EmployeeEnterpriseDto>>();
        http.Should().NotBeNull();
        http.Code.Should().Be((int) HttpStatusCode.OK);
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Status.Should().BeTrue();
        
        http.Data.Should().NotBeNull();
        http.Data.Id.Should().Be(emp.Id);
        http.Data.ContractLegalType.Should().Be(dto.ContractLegalType);
        http.Data.ContractLink.Should().Be(dto.ContractLink);
        http.Data.SalaryRange.Should().Be(dto.SalaryRange);
        http.Data.TerminationReason.Should().Be(emp.TerminationReason);
        http.Data.Notes.Should().Be(emp.Notes);
        http.Data.SalaryValue.Should().Be(dto.SalaryValue);
        http.Data.PaymentFrequency.Should().Be(dto.PaymentFrequency);
        http.Data.ContractLegalType.Should().Be(dto.ContractLegalType);
        http.Data.EmploymentType.Should().Be(dto.EmploymentType);
        http.Data.EmploymentStatus.Should().Be(dto.EmploymentStatus);
        http.Data.Currency.Should().Be(dto.Currency);
        http.Data.StartDate.Should().Be(dto.StartDate);
        http.Data.EndDate.Should().Be(dto.EndDate);
        http.Data.PositionId.Should().Be(dto.PositionId);
        
    }
    
    [Fact]
    public async Task PatchThrowForb()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", master.Token);
        
        UpdateEmployeeEnterpriseDto dto = new UpdateEmployeeEnterpriseDto();
        
        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_url}/{Guid.NewGuid()}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task GetAll()
    {
        UserResultTest user = await _helper.CreateAndGetUser();
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", user.Tokens!.Token);

        HttpResponseMessage message = await _client.GetAsync($"{_url}");
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        Page<EmployeeEnterpriseDto>? page = await message.Content.ReadFromJsonAsync<Page<EmployeeEnterpriseDto>>();
        page.Should().NotBeNull();
        
        page.PageIndex.Should().Be(1);
        page.PageSize.Should().Be(10);
    }
    
    
    
}
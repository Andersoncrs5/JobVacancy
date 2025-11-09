using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using JobVacancy.API.IntegrationTests.Utils;
using JobVacancy.API.models.dtos.EmployeeEnterprise;
using JobVacancy.API.models.dtos.EmployeeInvitation;
using JobVacancy.API.models.dtos.Enterprise;
using JobVacancy.API.models.dtos.Industry;
using JobVacancy.API.models.dtos.Position;
using JobVacancy.API.models.entities.Enums;
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
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        EmployeeInvitationDto invitation = await _helper.CreateEmployeeInvitation(userGuest, positionDto);
        
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", userGuest.Tokens!.Token);

        EmployeeInvitationDto invitationUpdated = await _helper.UpdateInvitationByUser(StatusEnum.Accepted, invitation);

        CreateEmployeeEnterpriseDto dto = new CreateEmployeeEnterpriseDto()
        {
            InviteSenderId = invitationUpdated.InviteSenderId,
            UserId = invitationUpdated.UserId,
            ContractLegalType = ContractLegalTypeEnum.Indefinite,
            ContractLink = null,
            ContractType = EmploymentTypeEnum.Temporary,
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
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token!);

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
        http.Data.ContractType.Should().Be(dto.ContractType);
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
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        EmployeeInvitationDto invitation = await _helper.CreateEmployeeInvitation(userGuest, positionDto);
        
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", userGuest.Tokens!.Token);
        
        CreateEmployeeEnterpriseDto dto = new CreateEmployeeEnterpriseDto()
        {
            InviteSenderId = invitation.InviteSenderId,
            UserId = invitation.UserId,
            ContractLegalType = ContractLegalTypeEnum.Indefinite,
            ContractLink = null,
            ContractType = EmploymentTypeEnum.Temporary,
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
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token!);

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
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        EmployeeInvitationDto invitation = await _helper.CreateEmployeeInvitation(userGuest, positionDto);
        
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", userGuest.Tokens!.Token);

        EmployeeInvitationDto invitationUpdated = await _helper.UpdateInvitationByUser(StatusEnum.Accepted, invitation);

        CreateEmployeeEnterpriseDto dto = new CreateEmployeeEnterpriseDto()
        {
            InviteSenderId = invitationUpdated.InviteSenderId,
            UserId = invitationUpdated.UserId,
            ContractLegalType = ContractLegalTypeEnum.Indefinite,
            ContractLink = null,
            ContractType = EmploymentTypeEnum.Temporary,
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
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token!);

        HttpResponseMessage message = await _client.PostAsJsonAsync($"{_url}/{Guid.NewGuid()}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Data.Should().BeNull();
        http.Code.Should().Be((int) HttpStatusCode.NotFound);
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Status.Should().BeFalse();
    }
    
}
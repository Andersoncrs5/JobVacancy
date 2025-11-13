using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using JobVacancy.API.IntegrationTests.Utils;
using JobVacancy.API.models.dtos.ApplicationVacancy;
using JobVacancy.API.models.dtos.Area;
using JobVacancy.API.models.dtos.Enterprise;
using JobVacancy.API.models.dtos.Industry;
using JobVacancy.API.models.dtos.Vacancy;
using JobVacancy.API.models.entities.Enums;
using JobVacancy.API.Utils.Res;
using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;

namespace JobVacancy.API.IntegrationTests.ApplicationVacancy;

public class ApplicationVacancyControllerTest: IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly Helper _helper;
    private readonly IConfiguration _configuration;
    private readonly string _URL = "/api/v1/ApplicationVacancy";
    private readonly ITestOutputHelper _output;
    
    public ApplicationVacancyControllerTest(CustomWebApplicationFactory factory, ITestOutputHelper output) 
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
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", master.Token!);
        
        IndustryDto industryDto = await _helper.CreateIndustry(master);
        AreaDto areaDto = await _helper.CreateArea();

        UserResultTest user = await _helper.CreateAndGetUser();
        UserResultTest candidate = await _helper.CreateAndGetUser();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Tokens!.Token!);
        
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        var loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginUserWithNewRole.Token!);
        VacancyDto vacancyDto = await _helper.CreateVacancy(areaDto);
        VacancyDto vacancy = await _helper.ChangeStatusVacancy(vacancyDto.Id, VacancyStatusEnum.Open);

        CreateApplicationVacancyDto dto = new CreateApplicationVacancyDto
        {
            VacancyId = vacancy.Id,
            CoverLetter = string.Concat(Enumerable.Repeat("AnyMessage", 20)),
        };

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", candidate.Tokens!.Token!);
        
        HttpResponseMessage message = await _client.PostAsJsonAsync($"{_URL}", dto);
        _output.WriteLine(message.Content.ReadAsStringAsync().Result);
        message.StatusCode.Should().Be(HttpStatusCode.Created);
    }
    
}
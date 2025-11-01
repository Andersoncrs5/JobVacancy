using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using JobVacancy.API.IntegrationTests.Utils;
using JobVacancy.API.models.dtos.Category;
using JobVacancy.API.models.dtos.Enterprise;
using JobVacancy.API.models.dtos.Industry;
using JobVacancy.API.models.dtos.PostEnterprise;
using JobVacancy.API.Utils.Page;
using JobVacancy.API.Utils.Res;
using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;

namespace JobVacancy.API.IntegrationTests.PostEnterpriseTest;

public class PostEnterpriseControllerTest: IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly Helper _helper;
    private readonly IConfiguration _configuration;
    private readonly string _url = "/api/v1/PostEnterprise";
    private readonly ITestOutputHelper _output;

    public PostEnterpriseControllerTest(CustomWebApplicationFactory factory, ITestOutputHelper output) 
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
        CategoryDto categoryDto = await _helper.CreateCategory(master);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        var loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        string token = loginUserWithNewRole.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        CreatePostEnterpriseDto dto = new CreatePostEnterpriseDto
        {
            CategoryId = categoryDto.Id,
            Content = string.Concat(Enumerable.Repeat("AnyContent", 30)),
            IsActive = true,
            ImageUrl = "https://github.com/Andersoncrs5",
            ReadingTimeMinutes = 4,
            Title = "A Title simple to a post simple"
        };

        HttpResponseMessage message = await _client.PostAsJsonAsync($"{_url}", dto);
        
        message.StatusCode.Should().Be(HttpStatusCode.Created);
        ResponseHttp<PostEnterpriseDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<PostEnterpriseDto>>();
        http.Should().NotBeNull();
        http.Data.Should().NotBeNull();
        
        http.Data.Id.Should().NotBeEmpty();
        http.Data.Title.Should().Be(dto.Title);
        http.Data.Content.Should().Be(dto.Content);
        http.Data.CategoryId.Should().Be(dto.CategoryId);
        http.Data.IsActive.Should().Be(dto.IsActive);
        http.Data.ReadingTimeMinutes.Should().Be(dto.ReadingTimeMinutes);
        http.Data.ImageUrl.Should().Be(dto.ImageUrl);
        http.Data.EnterpriseId.Should().Be(enterprise.Id);
    }
    
    [Fact]
    public async Task CreateThrowForbBecauseNoRoleEnterprise()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        CategoryDto categoryDto = await _helper.CreateCategory(master);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        
        string token = user.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        CreatePostEnterpriseDto dto = new CreatePostEnterpriseDto
        {
            CategoryId = categoryDto.Id,
            Content = string.Concat(Enumerable.Repeat("AnyContent", 30)),
            IsActive = true,
            ImageUrl = "https://github.com/Andersoncrs5",
            ReadingTimeMinutes = 4,
            Title = "A Title simple to a post simple"
        };

        HttpResponseMessage message = await _client.PostAsJsonAsync($"{_url}", dto);
        
        message.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        
    }

    [Fact]
    public async Task CreateReturnNotFoundCategory()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        var loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        string token = loginUserWithNewRole.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        CreatePostEnterpriseDto dto = new CreatePostEnterpriseDto
        {
            CategoryId = Guid.NewGuid().ToString(),
            Content = string.Concat(Enumerable.Repeat("AnyContent", 30)),
            IsActive = true,
            ImageUrl = "https://github.com/Andersoncrs5",
            ReadingTimeMinutes = 4,
            Title = "A Title simple to a post simple"
        };

        HttpResponseMessage message = await _client.PostAsJsonAsync($"{_url}", dto);
        
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        
        http.Should().NotBeNull();
        http.Data.Should().BeNull();
        http.Message.Should().NotBeNull();
        http.Code.Should().Be(404);
        
    }

    [Fact]
    public async Task Get()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        CategoryDto categoryDto = await _helper.CreateCategory(master);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        var loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        string token = loginUserWithNewRole.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        PostEnterpriseDto post = await _helper.CreatePostEnterprise(categoryDto);

        HttpResponseMessage message = await _client.GetAsync($"{_url}/{post.Id}");
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        ResponseHttp<PostEnterpriseDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<PostEnterpriseDto>>();
        http.Should().NotBeNull();
        http.Data.Should().NotBeNull();
        http.Message.Should().NotBeNull();
        http.Code.Should().Be(200);
        http.Status.Should().BeTrue();
        
        http.Data.Id.Should().Be(post.Id);
    }
 
    [Fact]
    public async Task GetNotFound()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        CategoryDto categoryDto = await _helper.CreateCategory(master);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        var loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        string token = loginUserWithNewRole.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        PostEnterpriseDto post = await _helper.CreatePostEnterprise(categoryDto);

        HttpResponseMessage message = await _client.GetAsync($"{_url}/{Guid.NewGuid()}");
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Data.Should().BeNull();
        http.Message.Should().NotBeNull();
        http.Code.Should().Be(404);
        http.Status.Should().BeFalse();
    }

    [Fact]
    public async Task Delete()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        CategoryDto categoryDto = await _helper.CreateCategory(master);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        var loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        string token = loginUserWithNewRole.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        PostEnterpriseDto post = await _helper.CreatePostEnterprise(categoryDto);

        HttpResponseMessage message = await _client.DeleteAsync($"{_url}/{post.Id}");
        message.StatusCode.Should().Be(HttpStatusCode.NoContent);

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        
        http.Should().NotBeNull();
        http.Data.Should().BeNull();
        http.Message.Should().NotBeNull();
        http.Code.Should().Be(204);
    }
    
    [Fact]
    public async Task DeleteNotFound()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        CategoryDto categoryDto = await _helper.CreateCategory(master);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        var loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        string token = loginUserWithNewRole.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        PostEnterpriseDto post = await _helper.CreatePostEnterprise(categoryDto);

        HttpResponseMessage message = await _client.DeleteAsync($"{_url}/{Guid.NewGuid()}");
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);

        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        
        http.Should().NotBeNull();
        http.Data.Should().BeNull();
        http.Message.Should().NotBeNull();
        http.Code.Should().Be(404);
    }
    
    [Fact]
    public async Task DeleteThrowForb()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        CategoryDto categoryDto = await _helper.CreateCategory(master);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        var loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        string token = loginUserWithNewRole.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        PostEnterpriseDto post = await _helper.CreatePostEnterprise(categoryDto);

        UserResultTest userb = await _helper.CreateAndGetUser();
        token = userb.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        HttpResponseMessage message = await _client.DeleteAsync($"{_url}/{Guid.NewGuid()}");
        message.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task GetAll()
    {
        UserResultTest user = await _helper.CreateAndGetUser();
        string token = user.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage message = await _client.GetAsync($"{_url}");
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        Page<PostEnterpriseDto>? http = await message.Content.ReadFromJsonAsync<Page<PostEnterpriseDto>>();
        http.Should().NotBeNull();
        http.PageIndex.Should().Be(1);
        http.PageSize.Should().Be(10);
    }
    
    [Fact]
    public async Task GetAllByFilterPost()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        CategoryDto categoryDto = await _helper.CreateCategory(master);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        var loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        string token = loginUserWithNewRole.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        PostEnterpriseDto postUser = await _helper.CreatePostEnterprise(categoryDto);
        
        HttpResponseMessage message = await _client.GetAsync($"{_url}?" +
                                                             $"Title={postUser.Title}" +
                                                             $"&Content={postUser.Content}" +
                                                             $"&IsActive={postUser.IsActive}" +
                                                             $"&IsFeatured={postUser.IsFeatured}" +
                                                             $"&ReadingTimeMinutesBefore={(postUser.ReadingTimeMinutes + 2)}" +
                                                             $"&ReadingTimeMinutesAfter={(postUser.ReadingTimeMinutes - 2)}" + 
                                                             $"&CategoryId={postUser.CategoryId}");
        
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        Page<PostEnterpriseDto>? http = await message.Content.ReadFromJsonAsync<Page<PostEnterpriseDto>>();
        http.Should().NotBeNull();
        http.PageIndex.Should().Be(1);
        http.PageSize.Should().Be(10);
        
        http.Data.Should().NotBeNull();
        http.Data.First().Should().NotBeNull();
        http.Data.First().Id.Should().Be(postUser.Id);
    }
 
    [Fact]
    public async Task GetAllByFilterCategory()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        CategoryDto categoryDto = await _helper.CreateCategory(master);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        var loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        string token = loginUserWithNewRole.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        PostEnterpriseDto postUser = await _helper.CreatePostEnterprise(categoryDto);
        
        HttpResponseMessage message = await _client.GetAsync($"{_url}?" +
                                                             $"CategoryId={postUser.CategoryId}" +
                                                             $"&NameCategory={postUser.Category!.Name}");
        
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        Page<PostEnterpriseDto>? http = await message.Content.ReadFromJsonAsync<Page<PostEnterpriseDto>>();
        http.Should().NotBeNull();
        http.PageIndex.Should().Be(1);
        http.PageSize.Should().Be(10);
        
        http.Data.Should().NotBeNull();
        http.Data.First().Should().NotBeNull();
        http.Data.First().Id.Should().Be(postUser.Id);
    }
 
    [Fact]
    public async Task GetAllByFilterEnterprise()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        CategoryDto categoryDto = await _helper.CreateCategory(master);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        var loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        string token = loginUserWithNewRole.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        PostEnterpriseDto postUser = await _helper.CreatePostEnterprise(categoryDto);
        
        HttpResponseMessage message = await _client.GetAsync($"{_url}?" +
                                                             $"NameEnterprise={enterprise.Name}" +
                                                             $"&TypeEnterprise={enterprise.Type}");
        
        message.StatusCode.Should().Be(HttpStatusCode.OK);

        Page<PostEnterpriseDto>? http = await message.Content.ReadFromJsonAsync<Page<PostEnterpriseDto>>();
        http.Should().NotBeNull();
        http.PageIndex.Should().Be(1);
        http.PageSize.Should().Be(10);
        
        http.Data.Should().NotBeNull();
        http.Data.First().Should().NotBeNull();
        http.Data.First().Id.Should().Be(postUser.Id);
    }
    
    [Fact]
    public async Task PatchAllFields_ReturnsOkAndUpdatesPost()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        CategoryDto categoryDto = await _helper.CreateCategory(master);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        var loginUserWithRole = await _helper.LoginUser(user.User!.Email!, user.CreateUser!.PasswordHash);
        string token = loginUserWithRole.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        PostEnterpriseDto postEnterprise = await _helper.CreatePostEnterprise(categoryDto);

        UpdatePostEnterpriseDto dto = new UpdatePostEnterpriseDto
        {
            Content = "Novo Conteúdo Atualizado para o Post da Empresa",
            IsActive = false,
            ImageUrl = "https://new.image.url/updated.png",
            ReadingTimeMinutes = 10,
            Title = "Título Atualizado pela Empresa"
        };

        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_url}/{postEnterprise.Id}", dto);
        
        message.StatusCode.Should().Be(HttpStatusCode.OK); 

        ResponseHttp<PostEnterpriseDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<PostEnterpriseDto>>();
        http.Should().NotBeNull();
        http.Data.Should().NotBeNull();
        
        http.Data.Id.Should().Be(postEnterprise.Id);
        http.Data.Title.Should().Be(dto.Title);
        http.Data.Content.Should().Be(dto.Content);
        http.Data.IsActive.Should().BeFalse();
        http.Data.ReadingTimeMinutes.Should().Be(dto.ReadingTimeMinutes);
        http.Data.ImageUrl.Should().Be(dto.ImageUrl);
        
        http.Data.CategoryId.Should().Be(postEnterprise.CategoryId);
        http.Data.EnterpriseId.Should().Be(postEnterprise.EnterpriseId);
    }
    
    [Fact]
    public async Task PatchPartialFields_UpdatesOnlyProvidedFields()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        CategoryDto categoryDto = await _helper.CreateCategory(master);
        IndustryDto industryDto = await _helper.CreateIndustry(master);
        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
    
        var loginUserWithRole = await _helper.LoginUser(user.User!.Email!, user.CreateUser!.PasswordHash);
        string token = loginUserWithRole.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
    
        PostEnterpriseDto postEnterprise = await _helper.CreatePostEnterprise(categoryDto);

        UpdatePostEnterpriseDto dto = new UpdatePostEnterpriseDto
        {
            Title = "Novo Título Parcial",
            IsActive = false
        };

        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_url}/{postEnterprise.Id}", dto);
    
        message.StatusCode.Should().Be(HttpStatusCode.OK); 

        ResponseHttp<PostEnterpriseDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<PostEnterpriseDto>>();
        http.Should().NotBeNull();
        http.Data.Should().NotBeNull();
    
        http.Data.Title.Should().Be(dto.Title);
        http.Data.IsActive.Should().BeFalse();
    
        http.Data.Content.Should().Be(postEnterprise.Content);
        http.Data.ImageUrl.Should().Be(postEnterprise.ImageUrl);
        http.Data.ReadingTimeMinutes.Should().Be(postEnterprise.ReadingTimeMinutes);
    }
    
    [Fact]
    public async Task Patch_ReturnsForbidden_IfUserHasNoEnterpriseRole()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        CategoryDto categoryDto = await _helper.CreateCategory(master);
        IndustryDto industryDto = await _helper.CreateIndustry(master);

        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
        
        var loginUserWithNewRole = await _helper.LoginUser(user!.User!.Email!, user.CreateUser!.PasswordHash);
        
        string token = loginUserWithNewRole.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        PostEnterpriseDto post = await _helper.CreatePostEnterprise(categoryDto);

        UserResultTest userb = await _helper.CreateAndGetUser();
        token = userb.Tokens!.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        UpdatePostEnterpriseDto dto = new UpdatePostEnterpriseDto { Title = "Tentativa de Hacking" };
        
        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_url}/{post.Id}", dto);
        
        message.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task Patch_ReturnsNotFound_IfPostDoesNotExist()
    {
        ResponseTokens master = await _helper.LoginMaster(_configuration);
        IndustryDto industryDto = await _helper.CreateIndustry(master);
        UserResultTest user = await _helper.CreateAndGetUser();
        EnterpriseDto enterprise = await _helper.CreateEnterprise(user, industryDto);
    
        var loginUserWithRole = await _helper.LoginUser(user.User!.Email!, user.CreateUser!.PasswordHash);
        string token = loginUserWithRole.Token!;
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
    
        UpdatePostEnterpriseDto dto = new UpdatePostEnterpriseDto();

        HttpResponseMessage message = await _client.PatchAsJsonAsync($"{_url}/{Guid.NewGuid()}", dto);
    
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);
        ResponseHttp<object>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        http.Should().NotBeNull();
        http.Message.Should().Contain("Post not found");
    }
    
}
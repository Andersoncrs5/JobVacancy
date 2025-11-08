using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using JobVacancy.API.IntegrationTests.Utils;
using JobVacancy.API.models.dtos.Category;
using JobVacancy.API.models.dtos.CommentPostEnterprise;
using JobVacancy.API.models.dtos.CommentPostUser;
using JobVacancy.API.models.dtos.EmployeeInvitation;
using JobVacancy.API.models.dtos.Enterprise;
using JobVacancy.API.models.dtos.EnterpriseIndustry;
using JobVacancy.API.models.dtos.FavoritePost;
using JobVacancy.API.models.dtos.FavoritePostEnterprise;
using JobVacancy.API.models.dtos.Industry;
using JobVacancy.API.models.dtos.Position;
using JobVacancy.API.models.dtos.PostEnterprise;
using JobVacancy.API.models.dtos.PostUser;
using JobVacancy.API.models.dtos.Skill;
using JobVacancy.API.models.dtos.Users;
using JobVacancy.API.models.dtos.UserSkill;
using JobVacancy.API.models.entities.Enums;
using JobVacancy.API.Utils.Res;
using Microsoft.Extensions.Configuration;

namespace JobVacancy.API.IntegrationTests;

public class Helper(
    HttpClient client
    )
{
    public async Task<PositionDto> CreatePositionAsync()
    {
        string _URL = "/api/v1/Position";
        int num = Random.Shared.Next(1, 1000000);
        
        CreatePositionDto dto = new CreatePositionDto()
        {
            Describe = string.Concat(Enumerable.Repeat("AnyDesc", 5)),
            Name = Guid.NewGuid().ToString(),
            IsActive = true
        };

        HttpResponseMessage message = await client.PostAsJsonAsync(_URL, dto);
        message.StatusCode.Should().Be(HttpStatusCode.Created);
        ResponseHttp<PositionDto>? content = await message.Content.ReadFromJsonAsync<ResponseHttp<PositionDto>>();
        
        content.Should().NotBeNull();
        content.Status.Should().BeTrue();
        content.Message.Should().NotBeNullOrWhiteSpace();
        content.Code.Should().Be((int)HttpStatusCode.Created);
        
        content.Data.Should().NotBeNull();
        content.Data.Id.Should().NotBeNullOrWhiteSpace();
        content.Data.Name.Should().Be(dto.Name);
        content.Data.Describe.Should().Be(dto.Describe);
        content.Data.IsActive.Should().Be(dto.IsActive);
        
        return content.Data;
    }
    
    public async Task<EmployeeInvitationDto> CreateEmployeeInvitation(UserResultTest userGuest, PositionDto position)
    {
        string _URL = "/api/v1/EmployeeInvitation";
        CreateEmployeeInvitationDto dto = new CreateEmployeeInvitationDto
        {
            UserId = userGuest.User!.Id,
            Currency = CurrencyEnum.Aud,
            EmploymentType = EmploymentTypeEnum.Temporary,
            Message = "AnyMessage",
            PositionId = position.Id,
            ProposedStartDate = DateTime.UtcNow.AddDays(7),
            ProposedEndDate = DateTime.UtcNow.AddDays(20),
            SalaryRange = "5000-7000",
            
        };

        HttpResponseMessage message = await client.PostAsJsonAsync($"{_URL}", dto);
        
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
        http.Data.PositionId.Should().Be(dto.PositionId);
        http.Data.ProposedStartDate.Should().Be(dto.ProposedStartDate);
        http.Data.ProposedEndDate.Should().Be(dto.ProposedEndDate);
        http.Data.SalaryRange.Should().Be(dto.SalaryRange);

        return http.Data;
    }
    
    public async Task AddFavoriteCommentPostEnterprise(string commentId)
    {
        string _url = "/api/v1/FavoriteCommentPostEnterprise";
            
        HttpResponseMessage message = await client.PostAsync($"{_url}/{commentId}/Toggle", null);
        message.StatusCode.Should().Be(HttpStatusCode.Created);

        ResponseHttp<object>? content = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        
        content.Should().NotBeNull();
        content.Data.Should().BeNull();
        content.Code.Should().Be((int)HttpStatusCode.Created);
        content.Status.Should().BeTrue();
    }
    
    public async Task<CommentPostEnterpriseDto> CreateCommentPostEnterpriseDto(string postId, bool isActive, string? parentId = null, string? content = null)
    {
        string _url = "/api/v1/CommentPostEnterprise";
        
        CreateCommentPostEnterpriseDto dto = new CreateCommentPostEnterpriseDto
        {
            Content = string.Concat(Enumerable.Repeat(content != null ? content : "AnyContent", 30)),
            Depth = 5,
            PostId = postId,
            ImageUrl = "https://github.com/Andersoncrs5",
            IsActive = isActive
        };
        
        HttpResponseMessage message = await client.PostAsJsonAsync($"{_url}?parentId={parentId}", dto);
        
        message.StatusCode.Should().Be(HttpStatusCode.Created);

        ResponseHttp<CommentPostEnterpriseDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<CommentPostEnterpriseDto>>();
        http.Code.Should().Be(201);
        http.Data.Should().NotBeNull();
        http.Message.Should().NotBeNullOrWhiteSpace();
        http.Status.Should().BeTrue();
        
        http.Data.Id.Should().NotBeNullOrWhiteSpace();
        
        return http.Data;
    }
    
    public async Task AddFavoriteCommentPostUser(string commentId)
    {
        string _url = "/api/v1/FavoriteCommentPostUser";
        HttpResponseMessage message = await client.PostAsync($"{_url}/{commentId}/Toggle", null);
        message.StatusCode.Should().Be(HttpStatusCode.Created);

        ResponseHttp<object>? content = await message.Content.ReadFromJsonAsync<ResponseHttp<object>>();
        
        content.Should().NotBeNull();
        content.Data.Should().BeNull();
        content.Code.Should().Be((int)HttpStatusCode.Created);
        content.Status.Should().BeTrue();
    }
    
    public async Task<FavoritePostEnterpriseDto> AddFavoritePostEnterprise(PostEnterpriseDto post)
    {
        string _url = "/api/v1/FavoritePostEnterprise";
        
        HttpResponseMessage message = await client.PostAsync($"{_url}/{post.Id}/Toggle", null);
        message.StatusCode.Should().Be(HttpStatusCode.Created);

        ResponseHttp<FavoritePostEnterpriseDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<FavoritePostEnterpriseDto>>();
        http.Should().NotBeNull();
        http.Status.Should().BeTrue();
        
        http.Data.Should().NotBeNull();
        http.Data.Id.Should().NotBeNullOrWhiteSpace();
        
        return http.Data;
    }
    
    public async Task<FavoritePostUserDto> AddFavoritePost(PostUserDto postUser)
    {
        string _url = "/api/v1/FavoritePostUser";  
        
        HttpResponseMessage message = await client.PostAsync($"{_url}/{postUser.Id}/Toggle", null);
        
        message.StatusCode.Should().Be(HttpStatusCode.Created);

        ResponseHttp<FavoritePostUserDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<FavoritePostUserDto>>();
        http.Should().NotBeNull();
        http.Data.Should().NotBeNull();
        http.Status.Should().BeTrue();
        
        return http.Data;
    }
    
    public async Task<UserSkillDto> CreateUserSkill(SkillDto skill)
    {
        string _URL = "/api/v1/UserSkill";
        HttpResponseMessage message = await client.PostAsync($"{_URL}/Toggle/{skill.Id}", null);
        message.StatusCode.Should().Be(HttpStatusCode.Created);

        ResponseHttp<UserSkillDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<UserSkillDto>>();
        http.Should().NotBeNull();

        http.Code.Should().Be(201);
        http.Status.Should().BeTrue();
        http.Message.Should().NotBeNullOrWhiteSpace();
        
        http.Data.Should().NotBeNull();
        http.Data.Id.Should().NotBeNullOrWhiteSpace();
        http.Data.SkillId.Should().Be(skill.Id);
        http.Data.ExternalCertificateUrl.Should().BeNull();
        http.Data.YearsOfExperience.Should().BeNull();
        http.Data.ProficiencyLevel.Should().BeNull();
        
        return http.Data;
    }
    
    public async Task<SkillDto> CreateSkill()
    {
        CreateSkillDto dto = new CreateSkillDto
        {
            Name = "TestSkill" + Guid.NewGuid().ToString(),
            Description = "TestSkill" + Guid.NewGuid().ToString(),
            IconUrl = "https://github.com/Andersoncrs5",
            IsActive = true
        };

        HttpResponseMessage message = await client.PostAsJsonAsync("/api/v1/Skill", dto);
        message.StatusCode.Should().Be(HttpStatusCode.Created);

        ResponseHttp<SkillDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<SkillDto>>();
        http.Should().NotBeNull();
        http.Data.Should().NotBeNull();
        http.Data.Name.Should().Be(dto.Name);
        http.Data.Description.Should().Be(dto.Description);
        http.Data.IconUrl.Should().Be(dto.IconUrl);
        http.Data.IsActive.Should().Be(true);
        http.Data.Id.Should().NotBeEmpty();
        
        return http.Data;
    }
    
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

    public async Task<CommentPostUserDto> CreateComment(PostUserDto postUser, string? title = null, string? parentId = null)
    {
        string _url = "/api/v1/CommentPostUser";
        CreateCommentPostUserDto dto = new CreateCommentPostUserDto()
        {
            Content = title == null? string.Concat(Enumerable.Repeat("AnyContent", 30)) : string.Concat(Enumerable.Repeat(title, 30)) ,
            Depth = 5,
            PostId = postUser.Id,
            ImageUrl = "https://github.com/Andersoncrs5",
            IsActive = true
        };
        
        HttpResponseMessage message = await client.PostAsJsonAsync($"{_url}?parentId={parentId}", dto);
        message.StatusCode.Should().Be(HttpStatusCode.Created);

        ResponseHttp<CommentPostUserDto>? content = await message.Content.ReadFromJsonAsync<ResponseHttp<CommentPostUserDto>>();
        content.Should().NotBeNull();
        content.Data.Should().NotBeNull();
        content.Code.Should().Be((int)HttpStatusCode.Created);
        content.Status.Should().BeTrue();
        
        content.Data.PostId.Should().Be(postUser.Id);
        content.Data.Id.Should().NotBeNullOrEmpty();
        
        return content.Data;
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

    public async Task<CategoryDto> CreateCategory(ResponseTokens master, bool isActive = true)
    {
        int num = Random.Shared.Next(1, 10000000);
        
        string token = master.Token!;
        client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        CreateCategoryDto dto = new CreateCategoryDto
        {
            IsActive = isActive,
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
    
    public async Task<IndustryDto> CreateIndustryWithIsActive(ResponseTokens master, bool isActive)
    {
        long num = Random.Shared.NextInt64(1, 10000000000000);
        
        string token = master.Token!;
        client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        CreateIndustryDto dto = new CreateIndustryDto
        {
            IsActive = isActive,
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

    public async Task<string> CreateEnterpriseIndustry(string enterpriseId, string industryId, bool isPrimary = true)
    {
        CreateEnterpriseIndustryDto dto = new CreateEnterpriseIndustryDto
        {
            EnterpriseId = enterpriseId,
            IndustryId = industryId,
            IsPrimary = isPrimary
        };

        HttpResponseMessage message = await client.PostAsJsonAsync("/api/v1/EnterpriseIndustry", dto);
        message.StatusCode.Should().Be(HttpStatusCode.Created);

        ResponseHttp<string>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<string>>();
        
        http.Should().NotBeNull();
        http.Status.Should().BeTrue();
        http.Data.Should().NotBeNull();
        
        return http.Data;
    }

    public async Task<PostUserDto> CreatePostUser(CategoryDto categoryDto, string userId, int readingTimeMinutes = 5)
    {
        CreatePostUserDto dto = new CreatePostUserDto
        {
            CategoryId = categoryDto.Id,
            Content = string.Concat(Enumerable.Repeat("AnyContent", 30)),
            IsActive = true,
            ImageUrl = "https://github.com/Andersoncrs5",
            ReadingTimeMinutes = readingTimeMinutes,
            Title = "A Title simple to a post simple"
        };
        
        HttpResponseMessage message = await client.PostAsJsonAsync("/api/v1/PostUser", dto);
        message.StatusCode.Should().Be(HttpStatusCode.Created);
        ResponseHttp<PostUserDto>? http = await message.Content.ReadFromJsonAsync<ResponseHttp<PostUserDto>>();
        http.Should().NotBeNull();
        http.Data.Should().NotBeNull();
        
        http.Data.Id.Should().NotBeEmpty();
        http.Data.Title.Should().Be(dto.Title);
        http.Data.Content.Should().Be(dto.Content);
        http.Data.CategoryId.Should().Be(dto.CategoryId);
        http.Data.IsActive.Should().Be(dto.IsActive);
        http.Data.ReadingTimeMinutes.Should().Be(dto.ReadingTimeMinutes);
        http.Data.ImageUrl.Should().Be(dto.ImageUrl);
        http.Data.UserId.Should().Be(userId);
        
        return http.Data;
    }

    public async Task<PostEnterpriseDto> CreatePostEnterprise(CategoryDto categoryDto, bool isActive = true)
    {
        CreatePostEnterpriseDto dto = new CreatePostEnterpriseDto
        {
            CategoryId = categoryDto.Id,
            Content = string.Concat(Enumerable.Repeat("AnyContent", 30)),
            IsActive = isActive,
            ImageUrl = "https://github.com/Andersoncrs5",
            ReadingTimeMinutes = 4,
            Title = "A Title simple to a post simple"
        };

        HttpResponseMessage message = await client.PostAsJsonAsync("/api/v1/PostEnterprise", dto);
        
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
        
        return http.Data;
    }
    
}
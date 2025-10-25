using JobVacancy.API.models.dtos.Users;
using JobVacancy.API.Utils.Res;

namespace JobVacancy.API.IntegrationTests.Utils;

public class UserResultTest
{
    public ResponseTokens? Tokens { get; set; }
    public UserDto? User { get; set; }
    public CreateUserDto? CreateUser { get; set; }
}
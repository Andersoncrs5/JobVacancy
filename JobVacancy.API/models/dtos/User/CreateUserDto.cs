using System.ComponentModel.DataAnnotations;

namespace JobVacancy.API.models.dtos.Users;

public class CreateUserDto
{
    [Required(ErrorMessage = "Field is required")]
    [StringLength(100, ErrorMessage = "Max size of 100")]
    public string Username { get; set; } = string.Empty; 

    [Required(ErrorMessage = "Field is required")]
    [EmailAddress(ErrorMessage = "Invalid Email")]
    [StringLength(150, ErrorMessage = "Max size of 150")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Field is required")]
    [StringLength(50, ErrorMessage = "Max size of 50", MinimumLength = 6)] 
    public string PasswordHash { get; set; } = string.Empty;
}
    

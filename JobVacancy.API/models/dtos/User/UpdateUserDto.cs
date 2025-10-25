using System.ComponentModel.DataAnnotations;

namespace JobVacancy.API.models.dtos.Users;

public class UpdateUserDto
{
    [Required(ErrorMessage = "Field is required")]
    [StringLength(100, ErrorMessage = "Max size of 100")]
    public string Name { get; set; } = string.Empty; 

    [Required(ErrorMessage = "Field is required")]
    [StringLength(200, ErrorMessage = "Max size of 100")]
    public string FullName { get; set; } = string.Empty; 
    
    [Required(ErrorMessage = "Field is required")]
    [StringLength(50, ErrorMessage = "Max size of 50", MinimumLength = 6)] 
    public string Password { get; set; } = string.Empty;
}
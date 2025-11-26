using System.ComponentModel.DataAnnotations;

namespace JobVacancy.API.Utils.Annotations.Validations.Global;

public class AllowedExtensions(string[] extensions): ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName);
            if (!extensions.Contains(extension.ToLower()))
            {
                return new ValidationResult("File type not allowed. Only " + string.Join(", ", extensions) + " are accepted.");
            }
        }
        return ValidationResult.Success;
    }
}
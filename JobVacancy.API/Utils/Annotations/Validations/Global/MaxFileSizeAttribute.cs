using System.ComponentModel.DataAnnotations;

namespace JobVacancy.API.Utils.Annotations.Validations.Global;

public class MaxFileSizeAttribute(int maxFileSize) : ValidationAttribute
{
    private readonly int _maxFileSizeInBytes = maxFileSize * 1024 * 1024;
    
    private readonly int _maxFileSizeInMB = maxFileSize;

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is IFormFile file) 
            if (file.Length > _maxFileSizeInBytes)
                return new ValidationResult($"The maximum allowed file size is {_maxFileSizeInMB} MB.");
        
        return ValidationResult.Success;
    }
}

using System.ComponentModel.DataAnnotations;
using JobVacancy.API.Repositories.Interfaces;

namespace JobVacancy.API.Utils.Annotations.Validations;

public class ExistsCategoryByName: ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null || string.IsNullOrEmpty(value.ToString()))
        {
            return ValidationResult.Success;
        }
        
        string name = value.ToString()!;
        var service = validationContext.GetService(typeof(IServiceProvider)) as IServiceProvider;
        var repository = service?.GetService(typeof(ICategoryRepository)) as ICategoryRepository;
        
        if (repository == null)
        {
            throw new InvalidOperationException("Category repository service not registered.");
        }

        bool exists = repository.ExistsByName(name).GetAwaiter().GetResult();
        
        if (exists)
        {
            return new ValidationResult(
                ErrorMessage ?? $"The name '{name}' already exists.",
                new[] { validationContext.MemberName! }
            );
        }

        return ValidationResult.Success;
        
    }
}
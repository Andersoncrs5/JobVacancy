using System.ComponentModel.DataAnnotations;
using JobVacancy.API.Repositories.Interfaces;

namespace JobVacancy.API.Utils.Annotations.Validations;

public class ExistsAreaByName: ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null || string.IsNullOrEmpty(value.ToString()))
            return ValidationResult.Success;
        
        var name = value.ToString()!;
        
        var serviceProvider = validationContext.GetService(typeof(IServiceProvider)) as IServiceProvider;
        var repository = serviceProvider?.GetService(typeof(IAreaRepository)) as IAreaRepository;

        if (repository == null)
            throw new InvalidOperationException("Area repository service not registered.");

        bool exists = repository.ExistsByName(name).GetAwaiter().GetResult();
        
        if (exists)
        {
            return new ValidationResult(
                ErrorMessage ?? $"The '{name}' already exists.",
                new[] { validationContext.MemberName! }
            );
        }

        return ValidationResult.Success;
    }
}
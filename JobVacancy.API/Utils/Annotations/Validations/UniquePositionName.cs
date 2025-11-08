using System.ComponentModel.DataAnnotations;
using JobVacancy.API.Repositories.Interfaces;
using JobVacancy.API.Services.Interfaces;

namespace JobVacancy.API.Utils.Annotations.Validations;

public class UniquePositionName: ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null || string.IsNullOrEmpty(value.ToString()))
        {
            return ValidationResult.Success;
        }

        var name = value.ToString()!;
        var serviceProvider = validationContext.GetService(typeof(IServiceProvider)) as IServiceProvider;
        var positionRepository = serviceProvider?.GetService(typeof(IPositionRepository)) as IPositionRepository;

        if (positionRepository == null)
        {
            throw new InvalidOperationException("Position repository service not registered.");
        }

        bool exists = positionRepository.ExistsByName(name).GetAwaiter().GetResult();
        
        if (exists)
        {
            return new ValidationResult(
                ErrorMessage ?? $"The cargo '{name}' already exists.",
                new[] { validationContext.MemberName! }
            );
        }

        return ValidationResult.Success;
    }
}

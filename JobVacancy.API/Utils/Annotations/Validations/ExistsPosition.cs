using System.ComponentModel.DataAnnotations;
using JobVacancy.API.Repositories.Interfaces;
using JobVacancy.API.Services.Interfaces;

namespace JobVacancy.API.Utils.Annotations.Validations;

public class ExistsPosition: ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
        {
            return ValidationResult.Success;
        }
        
        var employeeId = (string) value;
        
        var serviceProvider = validationContext.GetService(typeof(IServiceProvider)) as IServiceProvider;
        var positionRepository = serviceProvider?.GetService(typeof(IPositionRepository)) as IPositionRepository;

        if (positionRepository == null)
        {
            throw new InvalidOperationException("Position repository service not registered.");
        }
        
        bool exists = positionRepository.ExistsById(employeeId).GetAwaiter().GetResult();
        
        if (!exists)
        {
            return new ValidationResult(
                ErrorMessage ?? $"Position not exists",
                new[] { validationContext.MemberName! }
            );
        }

        return ValidationResult.Success;
    }
}
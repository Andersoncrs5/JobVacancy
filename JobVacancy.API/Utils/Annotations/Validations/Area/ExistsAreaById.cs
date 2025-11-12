using System.ComponentModel.DataAnnotations;
using JobVacancy.API.Repositories.Interfaces;

namespace JobVacancy.API.Utils.Annotations.Validations.Area;

public class ExistsAreaById: ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            return ValidationResult.Success;
        
        string id = value.ToString()!;
        
        var service = validationContext.GetService(typeof(IServiceProvider)) as IServiceProvider;
        var repository = service?.GetService(typeof(IAreaRepository)) as IAreaRepository;
        
        if (repository == null)
            throw new InvalidOperationException("Area repository service not registered.");
        
        bool exists = repository.ExistsById(id).GetAwaiter().GetResult();

        if (!exists)
        {
            return new ValidationResult(
                ErrorMessage ?? $"Area not found",
                new[] { validationContext.MemberName! }
            );
        }

        return ValidationResult.Success;
    }
}
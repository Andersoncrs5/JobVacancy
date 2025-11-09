using System.ComponentModel.DataAnnotations;
using JobVacancy.API.models.entities;
using JobVacancy.API.Repositories.Interfaces;

namespace JobVacancy.API.Utils.Annotations.Validations;

public class ExistsUserById: ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
        {
            return ValidationResult.Success;
        }
        
        string id = value.ToString()!;
        
        var service = validationContext.GetService(typeof(IServiceProvider)) as IServiceProvider;
        var repository = service?.GetService(typeof(IUserRepository)) as IUserRepository;
        
        if (repository == null)
        {
            throw new InvalidOperationException("User repository service not registered.");
        }

        UserEntity? result = repository.GetById(id).GetAwaiter().GetResult();

        if (result == null)
        {
            return new ValidationResult(
                ErrorMessage ?? $"User not found",
                new[] { validationContext.MemberName! }
            );
        }

        return ValidationResult.Success;
    }
}
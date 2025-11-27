using System.ComponentModel.DataAnnotations;
using JobVacancy.API.models.entities;
using JobVacancy.API.Repositories.Interfaces;

namespace JobVacancy.API.Utils.Annotations.Validations.PostUser;

public class ExistsPostUserById: ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            return ValidationResult.Success;
        
        string id = value.ToString()!;

        var service = validationContext.GetService(typeof(IServiceProvider)) as IServiceProvider;
        
        if (service == null)
            throw new InvalidOperationException("Service not initialized");
        
        var repository = service.GetService(typeof(IPostUserRepository)) as IPostUserRepository;

        if (repository == null)
            throw new InvalidOperationException("Post User repository service not registered.");

        PostUserEntity? result = repository.GetByIdAsync(id).GetAwaiter().GetResult();
        
        if (result == null)
        {
            return new ValidationResult(
                ErrorMessage ?? $"Post not found",
                new[] { validationContext.MemberName! }
            );
        }

        return ValidationResult.Success;
    }
}
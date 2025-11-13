using System.ComponentModel.DataAnnotations;
using JobVacancy.API.models.entities;
using JobVacancy.API.Repositories.Interfaces;

namespace JobVacancy.API.Utils.Annotations.Validations.Skill;

public class ExistsSkillById: ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            return ValidationResult.Success;
        
        string id = value.ToString()!;
        
        var service = validationContext.GetService(typeof(IServiceProvider)) as IServiceProvider;
        var repository = service?.GetService(typeof(ISkillRepository)) as ISkillRepository;
        
        if (repository == null)
            throw new InvalidOperationException("Skill repository service not registered.");

        SkillEntity? result = repository.GetByIdAsync(id).GetAwaiter().GetResult();

        if (result == null)
        {
            return new ValidationResult(
                ErrorMessage ?? $"Skill not found",
                new[] { validationContext.MemberName! }
            );
        }

        return ValidationResult.Success;
    }
}
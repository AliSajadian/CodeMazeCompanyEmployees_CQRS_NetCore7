using FluentValidation;
using FluentValidation.Results;
//using System.ComponentModel.DataAnnotations;

using Application.Companies.Commands; 

namespace Application.Validators;


public sealed class CreateCompanyCommandValidator : AbstractValidator<CreateCompanyCommand> 
{
    public CreateCompanyCommandValidator()
    { 
        RuleFor(c => c.Company.Name).NotEmpty().MaximumLength(60); 
        RuleFor(c => c.Company.Address).NotEmpty().MaximumLength(60); 
    } 

    public override ValidationResult Validate(ValidationContext<CreateCompanyCommand> context) 
    { 
        return context.InstanceToValidate.Company is null 
            ? new ValidationResult(new[] 
            { 
                new FluentValidation.Results.ValidationFailure("CompanyForCreationDto", "CompanyForCreationDto object is null") 
            }) 
            : base.Validate(context);
    }

    protected override bool PreValidate(ValidationContext<CreateCompanyCommand> context, ValidationResult result) 
    {
        if (context.InstanceToValidate == null) 
        {
            result.Errors.Add(new ValidationFailure("", "Please ensure a model was supplied."));
            return false;
        }
        return true;
    }
}



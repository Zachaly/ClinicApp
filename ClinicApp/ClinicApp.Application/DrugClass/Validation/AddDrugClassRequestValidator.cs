using ClinicApp.Domain.Request.Add;
using FluentValidation;

namespace ClinicApp.Application.Validation;

public class AddDrugClassRequestValidator : AbstractValidator<AddDrugClassRequest>
{
    public AddDrugClassRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
    }
}

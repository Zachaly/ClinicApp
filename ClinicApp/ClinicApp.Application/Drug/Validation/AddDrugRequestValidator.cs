using ClinicApp.Domain.Request.Add;
using FluentValidation;

namespace ClinicApp.Application.Validation;

public class AddDrugRequestValidator : AbstractValidator<AddDrugRequest>
{
    public AddDrugRequestValidator()
    {
        RuleFor(x => x.GenericName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.BrandName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Price).GreaterThan(0);
    }
}

using ClinicApp.Domain.Request.Update;
using FluentValidation;

namespace ClinicApp.Application.Validation;

public class UpdateDrugRequestValidator : AbstractValidator<UpdateDrugRequest>
{
    public UpdateDrugRequestValidator()
    {
        RuleFor(x => x.GenericName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.BrandName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Price).GreaterThan(0);
    }
}

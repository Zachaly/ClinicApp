using ClinicApp.Domain.Request.Update;
using FluentValidation;

namespace ClinicApp.Application.Validation;

public class UpdateDrugClassRequestValidator : AbstractValidator<UpdateDrugClassRequest>
{
    public UpdateDrugClassRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
    }
}

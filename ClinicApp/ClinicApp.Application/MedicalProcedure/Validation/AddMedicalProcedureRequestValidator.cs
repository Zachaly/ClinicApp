using ClinicApp.Domain.Request.Add;
using FluentValidation;

namespace ClinicApp.Application.Validation;

public class AddMedicalProcedureRequestValidator : AbstractValidator<AddMedicalProcedureRequest>
{
    public AddMedicalProcedureRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Cost).GreaterThan(0);
    }
}

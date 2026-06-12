using ClinicApp.Domain.Request.Update;
using FluentValidation;

namespace ClinicApp.Application.Validation;

public class UpdateMedicalProcedureRequestValidator : AbstractValidator<UpdateMedicalProcedureRequest>
{
    public UpdateMedicalProcedureRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Cost).GreaterThan(0);
    }
}

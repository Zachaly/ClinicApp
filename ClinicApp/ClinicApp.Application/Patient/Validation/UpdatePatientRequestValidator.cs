using ClinicApp.Domain.Request.Update;
using FluentValidation;

namespace ClinicApp.Application.Validation;

public class UpdatePatientRequestValidator : AbstractValidator<UpdatePatientRequest>
{
    public UpdatePatientRequestValidator()
    {
        RuleFor(x => x.Address).NotEmpty().MaximumLength(75);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.City).NotEmpty().MaximumLength(50);
        RuleFor(x => x.PostalCode).Length(6).Matches("^[0-9]{2}-[0-9]{3}$"); ;
    }
}

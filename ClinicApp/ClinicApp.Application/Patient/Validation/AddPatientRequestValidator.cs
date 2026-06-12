using ClinicApp.Domain.Request.Add;
using FluentValidation;

namespace ClinicApp.Application.Validation;

public class AddPatientRequestValidator : AbstractValidator<AddPatientRequest>
{
    public AddPatientRequestValidator()
    {
        RuleFor(x => x.Address).NotEmpty().MaximumLength(75);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.City).NotEmpty().MaximumLength(50);
        RuleFor(x => x.PeselNumber).Length(11).Matches("^[0-9]+$")
            .WithMessage("Invalid PESEL number");
        RuleFor(x => x.PostalCode).Length(6).Matches("^[0-9]{2}-[0-9]{3}$");
        RuleFor(x => x.BirthDate).LessThan(DateTimeOffset.UtcNow);
    }
}

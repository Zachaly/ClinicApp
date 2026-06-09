using ClinicApp.Domain.Enum;
using FluentValidation;
using ClinicApp.Domain.Request;

namespace ClinicApp.Application.Validation;

public class AddAppointmentValidator: AbstractValidator<AddAppointmentRequest>
{
    public AddAppointmentValidator()
    {
        RuleFor(x => x.PatientId).NotEmpty().WithMessage("Patient ID jest wymagany");
        RuleFor(x => x.DoctorId).NotEmpty().WithMessage("Lekarza ID jest wymagany");
        RuleFor(x => x.DateOfAppointment).GreaterThan(DateTime.UtcNow).WithMessage("Wizyta musi być w przyszłości");
    }
}
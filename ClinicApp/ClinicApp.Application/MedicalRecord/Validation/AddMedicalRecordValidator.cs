using ClinicApp.Domain.Enum;
using FluentValidation;
using ClinicApp.Domain.Request;
using FluentValidation.Validators;

namespace ClinicApp.Application.Validation;

public class AddMedicalRecordValidator:AbstractValidator<AddMedicalRecordRequest>
{
    public AddMedicalRecordValidator()
    {
        RuleFor(x=>x.PatientId).NotEmpty().WithMessage("ID pacjenta jest wymagany");
        RuleFor(x=>x.DoctorId).NotEmpty().WithMessage("ID lekarza jest wymagany");
        RuleFor(x=>x.DocumentScanUrl).NotEmpty().WithMessage("Dokumentacja jest wymagana").MaximumLength(1000);
    }
}
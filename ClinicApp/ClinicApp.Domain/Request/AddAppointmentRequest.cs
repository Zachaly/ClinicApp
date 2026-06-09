using ClinicApp.Domain.Enum;
namespace ClinicApp.Domain.Request;

public record AddAppointmentRequest(
    DateTime DateOfAppointment,
    Guid     DoctorId,
    Guid     PatientId
);
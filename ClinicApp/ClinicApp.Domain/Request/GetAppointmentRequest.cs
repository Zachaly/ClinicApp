using ClinicApp.Domain.Enum;
namespace ClinicApp.Domain.Request;

public record GetAppointmentRequest(
    Guid? PatientId,
    Guid?Doctor,
    AppointmentStatus? Status,
    DateTime?From,
    DateTime? To
);
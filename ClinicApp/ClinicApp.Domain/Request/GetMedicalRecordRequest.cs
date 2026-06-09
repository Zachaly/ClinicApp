namespace ClinicApp.Domain.Request;

public record GetMedicalRecordRequest(
    Guid MedicalRecordId,
    Guid PatientId,
    Guid DoctorId,
    string DocumentScanUrl,
    DateTime CreatedAt,
    DateTime? UpdatedAt
    );
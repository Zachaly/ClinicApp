namespace ClinicApp.Domain.Request;

public class AddMedicalRecordRequest
{
    public Guid PatientId;
    public Guid DoctorId;
    public string DocumentScanUrl;
}
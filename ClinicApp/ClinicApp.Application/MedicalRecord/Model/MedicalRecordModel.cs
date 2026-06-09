namespace ClinicApp.Application.Model;

public class MedicalRecordModel
{
    public Guid PatientId {get;set;}
    public string DocumentScanUrl{get;set;}=string.Empty;
    public DateTime CreatedAt {get;set;}
    public DateTime? UpdatedAt {get;set;} 
    public bool IsDeleted {get;set;}
}
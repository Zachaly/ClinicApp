namespace ClinicApp.Domain;

public class AddAppointmentRequest
{
    public DateTime CreatedAt {get;set;}
    public DateTime DateofAppointment {get;set;}
    public string Doctor {get;set;}=string.Empty;
    public Guid PatientId {get;set;} 
    public AppointmentStatus Status {get;set;}
}
using ClinicApp.Domain.Enum;
namespace ClinicApp.Domain.Entity;

public class Appointment
{ 
    public DateTime CreatedAt {get;set;}
    public DateTime DateofAppointment {get;set;}
    public Guid DoctorId  {get;set;}
    public Guid PatientId {get;set;} 
    public AppointmentStatus Status {get;set;}
}
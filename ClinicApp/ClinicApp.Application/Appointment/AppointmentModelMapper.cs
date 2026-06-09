namespace ClinicApp.Application;
using ClinicApp.Application.Model;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Request;
using Riok.Mapperly.Abstractions;
[Mapper]
public partial class AppointmentModelMapper
{
    public partial AppointmentModel MapAppointmentToModel(Appointment a);
    public partial Appointment MapRequestToEntity(AddAppointmentRequest request);
}
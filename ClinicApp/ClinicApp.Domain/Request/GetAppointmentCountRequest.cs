using ClinicApp.Domain.Enum;
namespace ClinicApp.Domain.Request;

public class GetAppointmentCountRequest
{
    public AppointmentStatus? Status { get; set; }
    public string? Doctor { get; set; }
}

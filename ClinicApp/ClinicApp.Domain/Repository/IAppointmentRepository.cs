using ClinicApp.Domain.Request;
using  ClinicApp.Domain.Entity;
namespace ClinicApp.Domain;

public interface IAppointmentRepository
{
    Task<Appointment?> GetByIdAsync(Guid id,
        CancellationToken ct = default);
    Task<IReadOnlyList<Appointment>> GetAsync(
        GetAppointmentRequest request,
        CancellationToken ct = default);
    Task<int> CountAsync(
        GetAppointmentRequest request,
        CancellationToken ct = default);
    Task AddAsync(Appointment entity,
        CancellationToken ct = default);
    Task UpdateAsync(Appointment entity,
        CancellationToken ct = default);
    Task DeleteAsync(Appointment entity,
        CancellationToken ct = default);
}
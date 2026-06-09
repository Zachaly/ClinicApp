using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Request;
namespace ClinicApp.Domain;

public interface  IMedicalRecordRepository
{
    Task<MedicalRecord?> GetByIdAsync(Guid id,
        CancellationToken ct = default);
    Task<IReadOnlyList<MedicalRecord>> GetAsync(
        GetMedicalRecord request,
        CancellationToken ct = default);
    Task<int> CountAsync(
        GetMedicalRecord request,
        CancellationToken ct = default);
    Task AddAsync(MedicalRecord entity,
        CancellationToken ct = default);
    Task UpdateAsync(MedicalRecord entity,
        CancellationToken ct = default);
    Task DeleteAsync(MedicalRecord entity,
        CancellationToken ct = default);
}
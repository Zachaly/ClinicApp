using ClinicApp.Application.Model;
using ClinicApp.Domain.Repository;

namespace ClinicApp.Application.Handler;

public record GetPatientByIdRequest(Guid Id);

public class GetPatientByIdHandler
{
    private readonly IPatientRepository _repository;
    private readonly PatientModelMapper _mapper;

    public GetPatientByIdHandler(IPatientRepository repository)
    {
        _repository = repository;
        _mapper = new PatientModelMapper();
    }

    public async Task<PatientModel?> Handle(GetPatientByIdRequest request)
    {
        var entity = await _repository.GetByIdAsync(request.Id);

        return entity is null ? null : _mapper.MapPatientToModel(entity);
    }
}

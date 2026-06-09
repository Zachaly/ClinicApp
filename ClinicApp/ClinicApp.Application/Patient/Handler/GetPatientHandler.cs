using ClinicApp.Application.Model;
using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Request;

namespace ClinicApp.Application.Handler;

public class GetPatientHandler
{
    private readonly IPatientRepository _repository;
    private readonly PatientModelMapper _mapper;

    public GetPatientHandler(IPatientRepository repository)
    {
        _repository = repository;
        _mapper = new PatientModelMapper();
    }

    public async Task<List<PatientModel>> Handle(GetPatientRequest request)
    {
        var entities = await _repository.GetAsync(request);

        return entities.Select(_mapper.MapPatientToModel).ToList();
    }
}

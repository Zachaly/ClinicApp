using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Request;

namespace ClinicApp.Application.Handler;

public class GetPatientCountRequest : GetPatientRequest;

public class GetPatientCountHandler 
{
    private readonly IPatientRepository _repository;

    public GetPatientCountHandler(IPatientRepository repository)
    {
        _repository = repository;
    }

    public Task<int> Handle(GetPatientCountRequest request)
        => _repository.GetCountAsync(request);
}

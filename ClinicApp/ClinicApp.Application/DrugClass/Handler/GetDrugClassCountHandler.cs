using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Request.Get;

namespace ClinicApp.Application.Handler;

public class GetDrugClassCountRequest : GetDrugClassRequest;

public class GetDrugClassCountHandler
{
    private readonly IDrugClassRepository _repository;

    public GetDrugClassCountHandler(IDrugClassRepository repository)
    {
        _repository = repository;
    }

    public Task<int> Handle(GetDrugClassCountRequest request)
        => _repository.GetCountAsync(request);
}

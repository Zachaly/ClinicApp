using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Request.Get;

namespace ClinicApp.Application.Handler;

public class GetDrugCountRequest : GetDrugRequest;

public class GetDrugCountHandler
{
    private readonly IDrugRepository _repository;

    public GetDrugCountHandler(IDrugRepository repository)
    {
        _repository = repository;
    }

    public Task<int> Handle(GetDrugCountRequest request)
    {
        return _repository.GetCountAsync(request);
    }
}

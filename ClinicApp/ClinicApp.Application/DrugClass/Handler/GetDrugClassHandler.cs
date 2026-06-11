using ClinicApp.Application.Model;
using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Request.Get;

namespace ClinicApp.Application.Handler;

public class GetDrugClassHandler
{
    private readonly IDrugClassRepository _repository;
    private readonly DrugClassModelMapper _mapper;

    public GetDrugClassHandler(IDrugClassRepository repository)
    {
        _repository = repository;
        _mapper = new DrugClassModelMapper();
    }

    public async Task<List<DrugClassModel>> Handle(GetDrugClassRequest request)
    {
        var entities = await _repository.GetAsync(request);

        return entities.Select(_mapper.MapEntityToModel).ToList();
    }
}

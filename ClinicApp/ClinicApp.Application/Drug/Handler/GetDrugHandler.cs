using ClinicApp.Application.Model;
using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Request.Get;

namespace ClinicApp.Application.Handler;

public class GetDrugHandler
{
    private readonly IDrugRepository _repository;
    private readonly DrugModelMapper _mapper;

    public GetDrugHandler(IDrugRepository repository)
    {
        _repository = repository;
        _mapper = new DrugModelMapper();
    }

    public async Task<List<DrugModel>> Handle(GetDrugRequest request)
    {
        var entities = await _repository.GetAsync(request, ["Class"]);

        return entities.Select(_mapper.MapEntityToModel).ToList();
    } 
}

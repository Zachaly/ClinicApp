using ClinicApp.Application.Model;
using ClinicApp.Domain.Repository;

namespace ClinicApp.Application.Handler;

public record GetDrugByIdRequest(Guid Id);

public class GetDrugByIdHandler
{
    private readonly IDrugRepository _repository;
    private readonly DrugModelMapper _mapper;

    public GetDrugByIdHandler(IDrugRepository repository)
    {
        _repository = repository;
        _mapper = new DrugModelMapper();
    }

    public async Task<DrugModel?> Handle(GetDrugByIdRequest request)
    {
        var entity = await _repository.GetByIdAsync(request.Id);

        return entity is null ? null : _mapper.MapEntityToModel(entity);
    }
}

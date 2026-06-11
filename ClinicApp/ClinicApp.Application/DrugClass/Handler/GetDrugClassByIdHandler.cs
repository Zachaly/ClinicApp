using ClinicApp.Application.Model;
using ClinicApp.Domain.Repository;

namespace ClinicApp.Application.Handler;

public record GetDrugClassByIdRequest(Guid Id);

public class GetDrugClassByIdHandler
{
    private readonly IDrugClassRepository _repository;
    private readonly DrugClassModelMapper _mapper;

    public GetDrugClassByIdHandler(IDrugClassRepository repository)
    {
        _repository = repository;
        _mapper = new DrugClassModelMapper();
    }

    public async Task<DrugClassModel?> Handle(GetDrugClassByIdRequest request)
    {
        var entity = await _repository.GetByIdAsync(request.Id);

        return entity is null ? null : _mapper.MapEntityToModel(entity);
    }
}

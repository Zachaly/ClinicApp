using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Response;

namespace ClinicApp.Application.Handler;

public record DeleteDrugByIdRequest(Guid Id);

public class DeleteDrugByIdHandler
{
    private readonly IDrugRepository _repository;

    public DeleteDrugByIdHandler(IDrugRepository repository)
    {
        _repository = repository;
    }

    public async Task<ResponseModel> Handle(DeleteDrugByIdRequest request)
    {
        var entity = await _repository.GetByIdAsync(request.Id);

        if(entity is null)
        {
            return new ResponseModel();
        }

        await _repository.DeleteAsync(entity);

        return new ResponseModel();
    }
}

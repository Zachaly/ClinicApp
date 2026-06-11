using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Response;

namespace ClinicApp.Application.Handler;

public record DeleteDrugClassByIdRequest(Guid Id);

public class DeleteDrugClassByIdHandler
{
    private readonly IDrugClassRepository _repository;

    public DeleteDrugClassByIdHandler(IDrugClassRepository repository)
    {
        _repository = repository;
    }

    public async Task<ResponseModel> Handle(DeleteDrugClassByIdRequest request)
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

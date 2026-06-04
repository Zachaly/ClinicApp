using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Response;

namespace ClinicApp.Application.Handler;

public record DeletePatientByIdRequest(Guid Id);

public class DeletePatientByIdHandler
{
    private readonly IPatientRepository _repository;

    public DeletePatientByIdHandler(IPatientRepository repository)
    {
        _repository = repository;
    }

    public async Task<ResponseModel> Handle(DeletePatientByIdRequest request)
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

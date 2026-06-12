using ClinicApp.Application.Abstraction;
using ClinicApp.Application.Model;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Repository;

namespace ClinicApp.Application.Handler;

public record GetDrugClassByIdRequest(Guid Id) : GetEntityByIdRequest(Id);

public class GetDrugClassByIdHandler : GetEntityByIdHandler<DrugClass, DrugClassModel, GetDrugClassByIdRequest>
{
    public GetDrugClassByIdHandler(IDrugClassRepository repository) : base(repository, new DrugClassModelMapper())
    {
    }
}

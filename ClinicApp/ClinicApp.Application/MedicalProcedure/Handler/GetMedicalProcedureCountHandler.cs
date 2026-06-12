using ClinicApp.Application.Abstraction;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Request.Get;

namespace ClinicApp.Application.Handler;

public class GetMedicalProcedureCountRequest : GetMedicalProcedureRequest;

public class GetMedicalProcedureCountHandler : GetCountHandler<MedicalProcedure, GetMedicalProcedureRequest, GetMedicalProcedureCountRequest>
{
    public GetMedicalProcedureCountHandler(IMedicalProcedureRepository repository) : base(repository)
    {
    }
}

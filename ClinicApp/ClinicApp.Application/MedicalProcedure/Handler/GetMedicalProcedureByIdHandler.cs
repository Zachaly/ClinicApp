using ClinicApp.Application.Abstraction;
using ClinicApp.Application.Model;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Application.Handler;

public record GetMedicalProcedureByIdRequest(Guid Id) : GetEntityByIdRequest(Id);

public class GetMedicalProcedureByIdHandler : GetEntityByIdHandler<MedicalProcedure, MedicalProcedureModel, GetMedicalProcedureByIdRequest>
{
    public GetMedicalProcedureByIdHandler(IMedicalProcedureRepository repository) : base(repository, new MedicalProcedureModelMapper())
    {
    }
}

using ClinicApp.Application.Abstraction;
using ClinicApp.Application.Model;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Request.Get;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Application.Handler;

public class GetMedicalProcedureHandler : GetEntityHandler<MedicalProcedure, GetMedicalProcedureRequest, MedicalProcedureModel>
{
    public GetMedicalProcedureHandler(IMedicalProcedureRepository repository) : base(repository, new MedicalProcedureModelMapper())
    {
    }
}

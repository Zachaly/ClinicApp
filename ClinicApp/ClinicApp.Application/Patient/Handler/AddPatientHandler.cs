using ClinicApp.Application.Abstraction;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Request;
using ClinicApp.Domain.Request.Add;
using ClinicApp.Domain.Request.Get;
using ClinicApp.Domain.Response;
using FluentValidation;

namespace ClinicApp.Application.Handler;

public class AddPatientHandler : AddEntityHandler<Patient, AddPatientRequest>
{
    private readonly IPatientRepository _patientRepository;

    public AddPatientHandler(IPatientRepository repository, IValidator<AddPatientRequest> validator)
        : base(repository, validator, new PatientModelMapper())
    {
        _patientRepository = repository;
    }

    public override async Task<ValidationResponseModel> Handle(AddPatientRequest request)
    {
        var samePeselPatients = await _patientRepository.GetAsync(new GetPatientRequest
        {
            PeselNumber = request.PeselNumber
        });

        if(samePeselPatients.Any())
        {
            return new ValidationResponseModel("Patient with this PESEL already present");
        }

        return await base.Handle(request);
    }
}

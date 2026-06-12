using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Request;
using ClinicApp.Domain.Request.Add;
using ClinicApp.Domain.Request.Get;
using ClinicApp.Domain.Response;
using FluentValidation;

namespace ClinicApp.Application.Handler;

public class AddPatientHandler
{
    private readonly IPatientRepository _repository;
    private readonly IValidator<AddPatientRequest> _validator;
    private readonly PatientModelMapper _mapper;

    public AddPatientHandler(IPatientRepository repository, IValidator<AddPatientRequest> validator)
    {
        _repository = repository;
        _validator = validator;
        _mapper = new PatientModelMapper();
    }

    public async Task<ValidationResponseModel> Handle(AddPatientRequest request)
    {
        var samePeselPatients = await _repository.GetAsync(new GetPatientRequest
        {
            PeselNumber = request.PeselNumber
        });

        if(samePeselPatients.Any())
        {
            return new ValidationResponseModel("Patient with this PESEL already present");
        }

        var validation = _validator.Validate(request);

        if(!validation.IsValid)
        {
            return new ValidationResponseModel(validation.ToDictionary());
        }

        var entity = _mapper.MapRequestToEntity(request);

        await _repository.AddAsync(entity);

        return new ValidationResponseModel();
    }
}

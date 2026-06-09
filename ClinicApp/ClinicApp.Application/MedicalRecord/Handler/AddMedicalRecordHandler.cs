using ClinicApp.Application.Validation;
using ClinicApp.Domain;
using ClinicApp.Domain.Request;
using ClinicApp.Domain.Response;
using FluentValidation;
using ClinicApp.Application;


namespace ClinicApp.Application.Handler;

public class AddMedicalRecordHandler
{
    private readonly IMedicalRecordRepository _repository;
    private readonly IValidator<AddMedicalRecordRequest> _validator;
    private readonly MedicalRecordMapper _mapper;

    public AddMedicalRecordHandler(IMedicalRecordRepository repository, IValidator<AddMedicalRecordRequest> validator)
    {
        _repository = repository;
        _validator = validator;
        _mapper = new MedicalRecordMapper();
    }

    public async Task<ValidationResponseModel> Handle(AddMedicalRecordRequest request)
    {

        var validation = await _validator.ValidateAsync(request);

        if(!validation.IsValid)
        {
            return new ValidationResponseModel(validation.ToDictionary());
        }

        var entity = _mapper.MapRequestToEntity(request);

        await _repository.AddAsync(entity);

        return new ValidationResponseModel();
    }
}


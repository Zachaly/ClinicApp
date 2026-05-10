namespace ClinicApp.Domain.Response;

public class ValidationResponseModel : ResponseModel
{
    public IDictionary<string, string[]>? ValidationErrors { get; set; }

    public ValidationResponseModel(string error, IDictionary<string, string[]> validationErrors) : base(error)
    {
        ValidationErrors = validationErrors;
    }

    public ValidationResponseModel(IDictionary<string, string[]>? validationErrors) : this("Validation failed", validationErrors)
    {
    }

    public ValidationResponseModel() : base() 
    {
        ValidationErrors = null;
    }

    public ValidationResponseModel(string error) : base(error)
    {
        ValidationErrors = null;
    }
}

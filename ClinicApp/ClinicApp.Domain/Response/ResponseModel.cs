namespace ClinicApp.Domain.Response;

public class ResponseModel
{
    public bool IsSuccess { get; set; }
    public string? Error { get; set; }

    public ResponseModel()
    {
        IsSuccess = true;
        Error = null;
    }

    public ResponseModel(string error)
    {
        Error = error;
        IsSuccess = false;
    }
}

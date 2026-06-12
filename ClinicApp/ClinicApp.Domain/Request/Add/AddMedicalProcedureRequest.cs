namespace ClinicApp.Domain.Request.Add;

public record AddMedicalProcedureRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Cost { get; set; }
}

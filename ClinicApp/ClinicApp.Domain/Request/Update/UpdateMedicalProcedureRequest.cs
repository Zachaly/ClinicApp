namespace ClinicApp.Domain.Request.Update;

public record UpdateMedicalProcedureRequest : IUpdateRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Cost { get; set; }
}

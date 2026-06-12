namespace ClinicApp.Application.Model;

public class MedicalProcedureModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Cost { get; set; }
}

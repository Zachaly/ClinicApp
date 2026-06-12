namespace ClinicApp.Domain.Entity;

public class MedicalProcedure : IEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Cost { get; set; }
}

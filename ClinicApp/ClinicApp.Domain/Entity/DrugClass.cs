namespace ClinicApp.Domain.Entity;

public class DrugClass : IEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public ICollection<Drug> Drugs { get; set; }
}

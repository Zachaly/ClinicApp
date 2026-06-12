namespace ClinicApp.Domain.Entity;

public class Drug : IEntity
{
    public Guid Id { get; set; }
    public string BrandName { get; set; }
    public string GenericName { get; set; }

    public Guid ClassId { get; set; }
    public DrugClass Class { get; set; }
    public decimal Price { get; set; }
}
